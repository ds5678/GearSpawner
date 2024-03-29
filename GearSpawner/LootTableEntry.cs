﻿using UnityEngine;

namespace GearSpawner;

public readonly record struct LootTableEntry(string PrefabName, int Weight)
{
	internal LootTableEntry Normalize()
	{
		return new()
		{
			PrefabName = NormalizePrefabName(PrefabName),
			Weight = Mathf.Clamp(Weight, 0, int.MaxValue)
		};
	}

	private static string NormalizePrefabName(string prefabName)
	{
		return prefabName.StartsWith("GEAR_", StringComparison.InvariantCulture)
			? prefabName
			: "GEAR_" + prefabName;
	}
}
