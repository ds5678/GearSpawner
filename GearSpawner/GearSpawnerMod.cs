using MelonLoader;

namespace GearSpawner;

internal class GearSpawnerMod : MelonMod
{
	public override void OnApplicationStart()
	{
		Settings.Instance.AddToModSettings("Gear Spawn Settings");
	}
}
