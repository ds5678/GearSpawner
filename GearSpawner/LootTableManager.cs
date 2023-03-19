using HarmonyLib;
using Il2Cpp;

namespace GearSpawner;

[HarmonyPatch]
internal static class LootTableManager
{
	private static readonly Dictionary<string, List<LootTableEntry>> lootTableEntries = new();

	internal static void AddLootTableEntry(string lootTable, LootTableEntry entry)
	{
		string normalizedLootTableName = GetNormalizedLootTableName(lootTable);

		if (!lootTableEntries.TryGetValue(normalizedLootTableName, out List<LootTableEntry>? entryList))
		{
			entryList = new();
			lootTableEntries.Add(normalizedLootTableName, entryList);
		}

		entryList.Add(entry.Normalize());
	}

	internal static bool ConfigureLootTableData(LootTableData? lootTableData)
	{
		bool tableChanged = false;
		if (lootTableData == null)
		{
			return false;
		}

		if (lootTableEntries.TryGetValue(lootTableData.name.ToLowerInvariant(), out List<LootTableEntry>? entries))
		{

			foreach (LootTableEntry entry in entries)
			{
				LootTableItemReference itemRef = new()
				{
					m_GearItem = new AssetReferenceGearItem(entry.PrefabName),
					m_Weight = entry.Weight
				};

				if (!lootTableData.m_BaseLoot.Contains(itemRef))
				{
					tableChanged = true;
					lootTableData.m_BaseLoot.Add(itemRef);
				}
				if (!lootTableData.m_Loot.Contains(itemRef))
				{
					tableChanged = true;
					lootTableData.m_Loot.Add(itemRef);
				}
				lootTableData.m_SumOfWeights += itemRef.m_Weight;
			}
		}
		return tableChanged;
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

	// patch the look tables as they initialize
	[HarmonyPostfix]
	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.Initialize))]
	private static void LootTableData_Initialize(LootTableData __instance)
	{
			ConfigureLootTableData(__instance);
	}
}
