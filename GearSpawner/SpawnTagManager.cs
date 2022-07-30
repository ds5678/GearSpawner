using MelonLoader;
using System;
using System.Collections.Generic;

namespace GearSpawner;

public static class SpawnTagManager
{
	private static readonly DefaultGearSpawnHandler defaultHandler = new();
	private static readonly Dictionary<string, IGearSpawnHandler> taggedHandlers = new();

	public static void AddFunction(string tag, Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function)
	{
		AddHandler(tag, new FunctionGearSpawnHandler(function));
	}

	public static void AddHandler(string tag, IGearSpawnHandler handler)
	{
		string tagToLower = tag.ToLowerInvariant();
		if (tagToLower == "none")
		{
			MelonLogger.Error("The spawn tag 'None' is reserved for GearSpawner internal workings.");
		}
		else if (taggedHandlers.ContainsKey(tagToLower))
		{
			MelonLogger.Error("Spawn tag already registered. Overwriting...");
			taggedHandlers[tagToLower] = handler;
		}
		else
		{
			taggedHandlers.Add(tagToLower, handler);
		}
	}

	internal static IGearSpawnHandler GetHandler(string tag)
	{
		return taggedHandlers.TryGetValue(tag, out IGearSpawnHandler? handler) 
			? handler 
			: defaultHandler;
	}
}
