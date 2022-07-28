using MelonLoader;
using System;
using System.Collections.Generic;

namespace GearSpawner
{
	public static class SpawnTagManager
	{
		private static Dictionary<string, Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float>> taggedFunctions = new Dictionary<string, Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float>>();

		public static void AddToTaggedFunctions(string tag, Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function)
		{
			string tagToLower = tag.ToLowerInvariant();
			if (tagToLower == "none")
			{
				MelonLogger.Error("The spawn tag 'None' is reserved for GearSpawner internal workings.");
			}
			else if (taggedFunctions.ContainsKey(tagToLower))
			{
				MelonLogger.Error("Spawn tag already registered. Overwriting...");
				taggedFunctions[tagToLower] = function;
			}
			else
			{
				taggedFunctions.Add(tagToLower, function);
			}
		}
		public static Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float>? GetTaggedFunction(string tag)
		{
			return taggedFunctions.ContainsKey(tag) ? taggedFunctions[tag] : null;
		}
		public static bool ContainsTag(string tag) => taggedFunctions.ContainsKey(tag);
	}
}
