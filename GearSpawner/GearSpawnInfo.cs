using UnityEngine;

namespace GearSpawner;

public readonly record struct GearSpawnInfo(string Tag, Vector3 Position, string PrefabName, Quaternion Rotation, float SpawnChance)
{
}
