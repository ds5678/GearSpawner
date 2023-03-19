using UnityEngine;

namespace GearSpawner;

public readonly record struct GearSpawnInfo(string Tag, Vector3 Position, string PrefabName, Quaternion Rotation, float SpawnChance)
{
	internal GearSpawnInfo NormalizePrefabName()
	{
		return PrefabName.StartsWith("GEAR_", StringComparison.Ordinal)
			? this
			: new GearSpawnInfo (Tag, Position, "GEAR_" + PrefabName, Rotation, SpawnChance);
	}
}
