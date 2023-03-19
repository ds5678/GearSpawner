using MelonLoader;

namespace GearSpawner;

public static class SpawnTagManager
{
	private static readonly DefaultGearSpawnHandler defaultHandler = new();
	private static readonly Dictionary<string, GearSpawnHandler> taggedHandlers = new();

	public static void AddFunction(string tag, Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function)
	{
		AddHandler(tag, new FunctionGearSpawnHandler(function));
	}

	public static void AddHandler(string tag, GearSpawnHandler handler)
	{
		string tagToLower = tag.ToLowerInvariant();
		if (tagToLower == "none")
		{
			GearSpawnerMod.Logger.Error("The spawn tag 'None' is reserved for GearSpawner internal workings.");
		}
		else if (taggedHandlers.ContainsKey(tagToLower))
		{
			GearSpawnerMod.Logger.Error($"Spawn tag {tag} already registered. Overwriting...");
			taggedHandlers[tagToLower] = handler;
		}
		else
		{
			taggedHandlers.Add(tagToLower, handler);
		}
	}

	internal static GearSpawnHandler GetHandler(string tag)
	{
		return taggedHandlers.TryGetValue(tag, out GearSpawnHandler? handler)
			? handler
			: defaultHandler;
	}
}
