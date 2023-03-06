using Il2Cpp;
using MelonLoader;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.AddressableAssets;

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

			GameObject prefab = Addressables.LoadAssetAsync<GameObject>(eachEntry.PrefabName).WaitForCompletion().Cast<GameObject>();
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


	internal static bool ConfigureLootTableData(LootTableData lootTableData)
	{
		bool tableChanged = false;
		if (lootTableData == null)
		{
			return false;
		}

		List<LootTableEntry> entries;
		if (lootTableEntries.TryGetValue(lootTableData.name.ToLowerInvariant(), out entries))
		{

			//MelonLoader.MelonLogger.Warning("FOUND " + lootTableData.name.ToLowerInvariant());
			//MelonLoader.MelonLogger.Warning("to ADD " + entries.Count);

			//lootTableData.m_BaseLoot = new Il2CppSystem.Collections.Generic.List<LootTableItemReference>();
			//lootTableData.m_Loot = new Il2CppSystem.Collections.Generic.List<LootTableItemReference>();

			foreach (LootTableEntry entry in entries)
			{
				LootTableItemReference itemRef = new();
//				string guid = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(entry.PrefabName.ToLower()))).Replace("-", "");
				itemRef.m_GearItem = new AssetReferenceGearItem(entry.PrefabName);
				itemRef.m_Weight = entry.Weight;

				//if (!gearItemAssets.ContainsKey(guid.ToLower()))
				//{
				//	gearItemAssets.Add(guid.ToLower(), entry.PrefabName.ToLower());
				//}


				if (!lootTableData.m_BaseLoot.Contains(itemRef))
				{
					tableChanged = true;
					//MelonLoader.MelonLogger.Warning("m_BaseLoot ADDED " + entry.PrefabName);
					lootTableData.m_BaseLoot.Add(itemRef);
				}
				if (!lootTableData.m_Loot.Contains(itemRef))
				{
					tableChanged = true;
					//MelonLoader.MelonLogger.Warning("m_Loot ADDED " + entry.PrefabName);
					lootTableData.m_Loot.Add(itemRef);
				}

				lootTableData.m_SumOfWeights += itemRef.m_Weight;

			}


		}
		return tableChanged;
	}

}
