using MelonLoader;

namespace GearSpawner;

internal sealed class GearSpawnerMod : MelonMod
{
	internal static MelonLogger.Instance Logger { get; } = new MelonLogger.Instance(BuildInfo.Name);

	public override void OnInitializeMelon()
	{
		Settings.Instance.AddToModSettings("Gear Spawn Settings");
	}
}
