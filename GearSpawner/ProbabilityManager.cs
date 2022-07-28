extern alias Hinterland;
using Hinterland;
using System;
using UnityEngine;

namespace GearSpawner;

internal static class ProbabilityManager
{
	internal static float GetAdjustedProbability(GearSpawnInfo gearSpawnInfo)
	{
		if (Settings.instance.alwaysSpawnItems)
		{
			return 100f; //overrides everything else
		}

		DifficultyLevel difficultyLevel = GetDifficultyLevel();
		FirearmAvailability firearmAvailability = GetFirearmAvailability();

		if (SpawnTagManager.ContainsTag(gearSpawnInfo.tag))
		{
			return SpawnTagManager.GetTaggedFunction(gearSpawnInfo.tag).Invoke(difficultyLevel, firearmAvailability, gearSpawnInfo);
		}
		else
		{
			return GetAdjustedProbability(difficultyLevel, gearSpawnInfo.SpawnChance);
		}
	}

	private static float GetAdjustedProbability(DifficultyLevel difficultyLevel, float baseProbability)
	{
		var multiplier = difficultyLevel switch
		{
			DifficultyLevel.Pilgram => Math.Max(0f, Settings.instance.pilgramSpawnProbabilityMultiplier),
			DifficultyLevel.Voyager => Math.Max(0f, Settings.instance.voyagerSpawnProbabilityMultiplier),
			DifficultyLevel.Stalker => Math.Max(0f, Settings.instance.stalkerSpawnProbabilityMultiplier),
			DifficultyLevel.Interloper => Math.Max(0f, Settings.instance.interloperSpawnProbabilityMultiplier),
			DifficultyLevel.Storymode => Math.Max(0f, Settings.instance.storySpawnProbabilityMultiplier),
			DifficultyLevel.Challenge => Math.Max(0f, Settings.instance.challengeSpawnProbabilityMultiplier),
			_ => 1f,
		};
		if (multiplier == 0f)
		{
			return 0f; //can disable spawns for a game mode
		}

		float clampedProbability = Mathf.Clamp(baseProbability, 0f, 100f);//just to be safe

		if (clampedProbability == 100f)
		{
			return 100f; //for guaranteed spawns
		}
		else
		{
			return Mathf.Clamp(multiplier * clampedProbability, 0f, 100f); //for normal spawns
		}
	}

	public static DifficultyLevel GetDifficultyLevel()
	{
		if (GameManager.IsStoryMode())
		{
			return DifficultyLevel.Storymode;
		}
		ExperienceModeType experienceModeType = ExperienceModeManager.GetCurrentExperienceModeType();
		return experienceModeType switch
		{
			ExperienceModeType.Pilgrim => DifficultyLevel.Pilgram,
			ExperienceModeType.Voyageur => DifficultyLevel.Voyager,
			ExperienceModeType.Stalker => DifficultyLevel.Stalker,
			ExperienceModeType.Interloper => DifficultyLevel.Interloper,
			ExperienceModeType.Custom => GetCustomDifficultyLevel(),
			ExperienceModeType.ChallengeArchivist => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeDeadManWalking => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeHunted => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeHuntedPart2 => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeNomad => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeNowhereToHide => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeRescue => DifficultyLevel.Challenge,
			ExperienceModeType.ChallengeWhiteout => DifficultyLevel.Challenge,
			_ => DifficultyLevel.Other,
		};
	}

	private static DifficultyLevel GetCustomDifficultyLevel()
	{
		return GameManager.GetCustomMode().m_BaseWorldDifficulty switch
		{
			CustomExperienceModeManager.CustomTunableLMHV.VeryHigh => DifficultyLevel.Pilgram,
			CustomExperienceModeManager.CustomTunableLMHV.High => DifficultyLevel.Voyager,
			CustomExperienceModeManager.CustomTunableLMHV.Medium => DifficultyLevel.Stalker,
			CustomExperienceModeManager.CustomTunableLMHV.Low => DifficultyLevel.Interloper,
			_ => DifficultyLevel.Other,
		};
	}

	public static FirearmAvailability GetFirearmAvailability()
	{
		if (GameManager.IsStoryMode())
		{
			if (SaveGameSystem.m_CurrentEpisode == Episode.One || SaveGameSystem.m_CurrentEpisode == Episode.Two)
			{
				return FirearmAvailability.Rifle;
			}
			else
			{
				return FirearmAvailability.All;
			}
		}
		ExperienceModeType experienceModeType = ExperienceModeManager.GetCurrentExperienceModeType();
		return experienceModeType switch
		{
			ExperienceModeType.Interloper => FirearmAvailability.None,
			ExperienceModeType.Custom => GetCustomFirearmAvailability(),
			_ => FirearmAvailability.All,
		};
	}

	private static FirearmAvailability GetCustomFirearmAvailability()
	{
		bool revolvers = GameManager.GetCustomMode().m_RevolversInWorld;
		bool rifles = GameManager.GetCustomMode().m_RiflesInWorld;
		if (revolvers && rifles)
		{
			return FirearmAvailability.All;
		}
		else if (revolvers)
		{
			return FirearmAvailability.Revolver;
		}
		else if (rifles)
		{
			return FirearmAvailability.Rifle;
		}
		else
		{
			return FirearmAvailability.None;
		}
	}
}
