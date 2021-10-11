using MelonLoader;

namespace GearSpawner
{
    internal class GearSpawnerMod : MelonMod
    {
		public override void OnApplicationStart()
		{
			Settings.instance.AddToModSettings("Gear Spawn Settings");
		}
	}
}
