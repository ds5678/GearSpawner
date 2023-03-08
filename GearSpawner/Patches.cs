using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GearSpawner;

internal static class Patches
{
	// patch the look tables as they initialize
	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.Initialize))]
	internal static class LootTableData_Awake
	{
		private static void Postfix(LootTableData __instance)
		{
				bool changed = LootTableManager.ConfigureLootTableData(__instance);
		}
	}

	//patch the scenes for loose items as they load
	[HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]//Exists
	internal static class GameManager_ApplyCurrentQualitySettings
	{
		private static void Prefix()
		{
			GearSpawnManager.PrepareScene();
		}
	}

	// this patch is required to allow non md5 asset reference names to be "valid"
	[HarmonyPatch(typeof(AssetReference), nameof(AssetReference.RuntimeKeyIsValid))]//Exists
	internal static class RuntimeKeyIsValid
	{
		private static void Postfix(AssetReference __instance, ref bool __result, ref bool __runOriginal)
		{
			if (__instance.AssetGUID != null && __instance.AssetGUID.StartsWith("GEAR_")) {
				GameObject? testObject = Addressables.LoadAssetAsync<GameObject>(__instance.AssetGUID).WaitForCompletion();
				if (testObject != null && testObject.name == __instance.AssetGUID)
				{
					__result = true;
					__runOriginal = false;
				}
			}
		}
	}
}
