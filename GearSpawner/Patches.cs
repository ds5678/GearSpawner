using HarmonyLib;

namespace GearSpawner
{
	internal static class Patches
	{
		[HarmonyPatch(typeof(LootTable), "GetPrefab")]
		internal static class LootTable_GetPrefab
		{
			private static void Prefix(LootTable __instance)
			{
				LootTableManager.ConfigureLootTable(__instance);
			}
		}
		[HarmonyPatch(typeof(LootTable), "GetRandomGearPrefab")]
		internal static class LootTable_GetRandomGearPrefab
		{
			private static void Prefix(LootTable __instance)
			{
				LootTableManager.ConfigureLootTable(__instance);
			}
		}

		[HarmonyPatch(typeof(GameManager), "SetAudioModeForLoadedScene")]//Exists
		internal static class GameManager_SetAudioModeForLoadedScene
		{
			private static void Prefix()
			{
				GearSpawnManager.PrepareScene();
			}
		}
	}
}
