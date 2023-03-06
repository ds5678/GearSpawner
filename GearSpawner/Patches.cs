using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.AddressableAssets;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Reflection;
using UnityEngine;

namespace GearSpawner;

internal static class Patches
{
	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.GetPrefabByName))]
	internal static class LootTableData_GetPrefabByName
	{
		private static void Prefix(LootTableData __instance)
		{
			//MelonLoader.MelonLogger.Warning("LootTableData_GetPrefabByName");
		}
		private static void Postfix(LootTableData __instance, ref UnityEngine.Object __result)
		{
			//MelonLoader.MelonLogger.Warning("LootTableData_GetPrefabByName | " + __result.name);
		}
	}

	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.GetRandomGearItemPrefab))]
	internal static class LootTableData_GetRandomGearItemPrefab
	{
		private static void Prefix(LootTableData __instance)
		{
			//MelonLoader.MelonLogger.Warning("LootTableData_GetRandomGearItemPrefab");
		}
		private static void Postfix(LootTableData __instance, ref UnityEngine.Object __result)
		{
			//MelonLoader.MelonLogger.Warning("LootTableData_GetRandomGearItemPrefab | " + __result.name);
		}
	}
	

	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.Initialize))]
	internal static class LootTableData_Awake
	{
		private static void Postfix(LootTableData __instance)
		{
			//if (GearSpawnManager.IsNonGameScene())
			//{
			//	return;
			//}

			//MelonLoader.MelonLogger.Warning("LootTableData_Awake | " + __instance.name + "|" + GameManager.m_ActiveScene);
			//MelonLoader.MelonLogger.Warning("OG m_BaseLoot(" + __instance.m_BaseLoot.Count + ") m_Loot(" + __instance.m_Loot.Count + ") m_SumOfWeights(" + __instance.m_SumOfWeights + ")");


				bool changed = LootTableManager.ConfigureLootTableData(__instance);
			//	if (changed)
			//	{
			//		foreach (LootTableItemReference ir in __instance.m_BaseLoot)
			//		{
			//			if (ir.m_GearItem != null && ir.m_GearItem.AssetGUID != null)
			//			{
			//				MelonLoader.MelonLogger.Warning(ir.m_GearItem.AssetGUID + " | " + ir.m_GearItem.IsValid() + " | " + ir.m_GearItem.RuntimeKeyIsValid());
			//				//MelonLoader.MelonLogger.Warning(ir.m_GearItem.AssetGUID + " => " + Addressables.LoadAssetAsync<GameObject>(ir.m_GearItem.AssetGUID).WaitForCompletion().GetComponent<GearItem>().DisplayName);
			//			}
			//		}
			//}

//			MelonLoader.MelonLogger.Warning("NEW m_BaseLoot(" + __instance.m_BaseLoot.Count + ") m_Loot(" + __instance.m_Loot.Count + ") m_SumOfWeights(" + __instance.m_SumOfWeights + ")");
			//			__instance.EvaluateWeights();
		}
	}


	[HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]//Exists
	internal static class GameManager_ApplyCurrentQualitySettings
	{
		private static void Prefix()
		{
			//MelonLoader.MelonLogger.Warning("GameManager_ApplyCurrentQualitySettings");
			GearSpawnManager.PrepareScene();
		}
	}
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
				//MelonLoader.MelonLogger.Warning(__instance.AssetGUID + " | " + __result);
			}
		}
	}

}
