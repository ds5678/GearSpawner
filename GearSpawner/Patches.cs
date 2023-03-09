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
			LootTableManager.ConfigureLootTableData(__instance);
		}
	}

	//patch the scenes for loose items as they load
	/// <summary>
	/// Other than GameManager.SetAudioModeForLoadedScene(), QualitySettingsManager.ApplyCurrentQualitySettings is the last method called within GameManager.Update() before save file saving and loading occur. They only get called after the loading panel has closed, and they each only get called once. If GameManager.SetAudioModeForLoadedScene() was not inlined, it would be used instead.
	/// </summary>
	[HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]
	internal static class GameManager_ApplyCurrentQualitySettings
	{
		private static void Prefix()
		{
			GearSpawnManager.PrepareScene();
		}
	}

	// this patch is required to allow non md5 asset reference names to be "valid"
	[HarmonyPatch(typeof(AssetReference), nameof(AssetReference.RuntimeKeyIsValid))]
	internal static class RuntimeKeyIsValid
	{
		private static void Postfix(AssetReference __instance, ref bool __result)
		{
			if (__instance.AssetGUID != null && __instance.AssetGUID.StartsWith("GEAR_"))
			{
				GameObject? testObject = Addressables.LoadAssetAsync<GameObject>(__instance.AssetGUID).WaitForCompletion();
				if (testObject != null && testObject.name == __instance.AssetGUID)
				{
					__result = true;
				}
			}
		}
	}
}
