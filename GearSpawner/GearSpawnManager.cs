using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GearSpawner;

internal static class GearSpawnManager
{
	private static Dictionary<string, List<GearSpawnInfo>> gearSpawnInfos = new Dictionary<string, List<GearSpawnInfo>>();

	internal static void AddGearSpawnInfo(string sceneName, GearSpawnInfo gearSpawnInfo)
	{
		string normalizedSceneName = GetNormalizedSceneName(sceneName);
		if (!gearSpawnInfos.ContainsKey(normalizedSceneName))
		{
			gearSpawnInfos.Add(normalizedSceneName, new List<GearSpawnInfo>());
		}

		List<GearSpawnInfo> sceneGearSpawnInfos = gearSpawnInfos[normalizedSceneName];
		sceneGearSpawnInfos.Add(gearSpawnInfo);
	}

	private static string? GetNormalizedGearName(string gearName)
	{
		if (gearName != null && !gearName.ToLowerInvariant().StartsWith("gear_"))
		{
			return "gear_" + gearName;
		}
		else
		{
			return gearName;
		}
	}

	private static string GetNormalizedSceneName(string sceneName) => sceneName.ToLowerInvariant();

	private static IEnumerable<GearSpawnInfo> GetSpawnInfos(string sceneName)
	{
		if (gearSpawnInfos.TryGetValue(sceneName, out List<GearSpawnInfo> result))
		{
			MelonLogger.Msg($"Found {result.Count} spawn entries for '{sceneName}'");
			return result;
		}
		else
		{
			MelonLogger.Msg($"Could not find any spawn entries for '{sceneName}'");
			return Enumerable.Empty<GearSpawnInfo>();
		}
	}

	internal static void PrepareScene()
	{
		if (IsNonGameScene())
		{
			return;
		}

		string sceneName = GameManager.m_ActiveScene;
		MelonLogger.Msg($"Spawning items for scene '{sceneName}' ...");
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();

		GearItem[] spawnedItems = SpawnGearForScene(GetNormalizedSceneName(sceneName));

		stopwatch.Stop();
		MelonLogger.Msg($"Spawned '{ProbabilityManager.GetDifficultyLevel()}' items for scene '{sceneName}' in {stopwatch.ElapsedMilliseconds} ms");

		SpawnManager.InvokeEvent(spawnedItems);
	}

	private static bool IsNonGameScene()
	{
		return string.IsNullOrEmpty(GameManager.m_ActiveScene) || GameManager.m_ActiveScene == "MainMenu" || GameManager.m_ActiveScene == "Boot" || GameManager.m_ActiveScene == "Empty";
	}

	/// <summary>
	/// Spawns the items into the scene. However, this can be overwritten by deserialization
	/// </summary>
	/// <param name="sceneName"></param>
	private static GearItem[] SpawnGearForScene(string sceneName)
	{
		IEnumerable<GearSpawnInfo> sceneGearSpawnInfos = GetSpawnInfos(sceneName);
		if (sceneGearSpawnInfos == null)
		{
			return new GearItem[0];
		}

		List<GearItem> spawnedItems = new List<GearItem>();

		foreach (GearSpawnInfo eachGearSpawnInfo in sceneGearSpawnInfos)
		{
			string? normalizedGearName = GetNormalizedGearName(eachGearSpawnInfo.PrefabName);
			UnityEngine.Object prefab = Resources.Load(normalizedGearName);

			if (prefab == null)
			{
				MelonLogger.Warning("Could not find prefab '{0}' to spawn in scene '{1}'.", eachGearSpawnInfo.PrefabName, sceneName);
				continue;
			}

			float spawnProbability = ProbabilityManager.GetAdjustedProbability(eachGearSpawnInfo);
			if (RandomUtils.RollChance(spawnProbability))
			{
				GameObject gear = UnityEngine.Object.Instantiate(prefab, eachGearSpawnInfo.Position, eachGearSpawnInfo.Rotation).Cast<GameObject>();
				gear.name = prefab.name;
				DisableObjectForXPMode xpmode = gear.GetComponent<DisableObjectForXPMode>();
				if (xpmode != null)
				{
					UnityEngine.Object.Destroy(xpmode);
				}

				spawnedItems.Add(gear.GetComponent<GearItem>());
			}
		}
		return spawnedItems.ToArray();
	}
}
