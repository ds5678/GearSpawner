extern alias Hinterland;
using Hinterland;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace GearSpawner;

internal static class LootTableManager
{
	private static Dictionary<string, List<LootTableEntry>> lootTableEntries = new Dictionary<string, List<LootTableEntry>>();

	internal static void AddLootTableEntry(string lootTable, LootTableEntry entry)
	{
		string normalizedLootTableName = GetNormalizedLootTableName(lootTable);

		if (!lootTableEntries.ContainsKey(normalizedLootTableName))
		{
			lootTableEntries.Add(normalizedLootTableName, new List<LootTableEntry>());
		}

		lootTableEntries[normalizedLootTableName].Add(entry.Normalize());
	}

	private static void AddEntries(LootTable lootTable, List<LootTableEntry> entries)
	{
		foreach (LootTableEntry eachEntry in entries)
		{
			int index = GetIndex(lootTable, eachEntry.PrefabName);
			if (index != -1)
			{
				lootTable.m_Weights[index] = eachEntry.Weight;
				continue;
			}

			GameObject prefab = Resources.Load(eachEntry.PrefabName).Cast<GameObject>();
			if (prefab == null)
			{
				MelonLogger.Warning("Could not find prefab '{0}'.", eachEntry.PrefabName);
				continue;
			}

			lootTable.m_Prefabs.Add(prefab);
			lootTable.m_Weights.Add(eachEntry.Weight);
		}
	}

	internal static void ConfigureLootTable(LootTable lootTable)
	{
		if (lootTable == null)
		{
			return;
		}

		List<LootTableEntry> entries;
		if (lootTableEntries.TryGetValue(lootTable.name.ToLowerInvariant(), out entries))
		{
			AddEntries(lootTable, entries);
		}
	}

	private static int GetIndex(LootTable lootTable, string prefabName)
	{
		for (int i = 0; i < lootTable.m_Prefabs.Count; i++)
		{
			if (lootTable.m_Prefabs[i] != null && lootTable.m_Prefabs[i].name.Equals(prefabName, System.StringComparison.InvariantCultureIgnoreCase))
			{
				return i;
			}
		}

		return -1;
	}

	private static string GetNormalizedLootTableName(string lootTable)
	{
		if (lootTable.StartsWith("Loot", System.StringComparison.InvariantCultureIgnoreCase))
		{
			return lootTable.ToLowerInvariant();
		}
		if (lootTable.StartsWith("Cargo", System.StringComparison.InvariantCultureIgnoreCase))
		{
			return "loot" + lootTable.ToLowerInvariant();
		}
		return "loottable" + lootTable.ToLowerInvariant();
	}
}
