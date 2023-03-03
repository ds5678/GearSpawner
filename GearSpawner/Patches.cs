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
			MelonLoader.MelonLogger.Warning("LootTableData_GetPrefabByName");
		}
		private static void Postfix(LootTableData __instance, ref UnityEngine.Object __result)
		{
			MelonLoader.MelonLogger.Warning("LootTableData_GetPrefabByName | " + __result.name);
		}
	}

	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.GetRandomGearItemPrefab))]
	internal static class LootTableData_GetRandomGearItemPrefab
	{
		private static void Prefix(LootTableData __instance)
		{
			MelonLoader.MelonLogger.Warning("LootTableData_GetRandomGearItemPrefab");
		}
		private static void Postfix(LootTableData __instance, ref UnityEngine.Object __result)
		{
			MelonLoader.MelonLogger.Warning("LootTableData_GetRandomGearItemPrefab | " + __result.name);
		}
	}

	[HarmonyPatch(typeof(LootTableData), nameof(LootTableData.Awake))]
	internal static class LootTableData_Awake
	{
		private static void Prefix(LootTableData __instance)
		{
			//if (GearSpawnManager.IsNonGameScene())
			//{
			//	return;
			//}

			MelonLoader.MelonLogger.Warning("LootTableData_Awake | " + __instance.name + "|" + GameManager.m_ActiveScene);
			MelonLoader.MelonLogger.Warning("OG m_BaseLoot(" + __instance.m_BaseLoot.Count + ") m_Loot(" + __instance.m_Loot.Count + ") m_SumOfWeights(" + __instance.m_SumOfWeights + ")");

			LootTableManager.ConfigureLootTableData(__instance);

			MelonLoader.MelonLogger.Warning("NEW m_BaseLoot(" + __instance.m_BaseLoot.Count + ") m_Loot(" + __instance.m_Loot.Count + ") m_SumOfWeights(" + __instance.m_SumOfWeights + ")");
			//__instance.EvaluateWeights();
		}
	}

	//[HarmonyPatch(typeof(AssetReferenceGearItem), nameof(AssetReferenceGearItem.GetOrLoadAsset))]
	//internal static class GetOrLoadAsset
	//{
	//	internal static void Prefix(AssetReferenceGearItem __instance, ref bool __runOriginal)
	//	{
	//		MelonLoader.MelonLogger.Warning("GetOrLoadAsset_PREFIX | " + __instance.AssetGUID);
	//		//if (ModAssetBundleManager.IsKnownAsset(name))
	//		//{
	//		//	__runOriginal = false;
	//		//}
	//	}
	//	internal static void Postfix(AssetReferenceGearItem __instance, ref bool __runOriginal, ref GameObject __result)
	//	{
	//		MelonLoader.MelonLogger.Warning("GetOrLoadAsset_POSTFIX | " + __instance.AssetGUID + " | " + (__result != null));
	//		//if (ModAssetBundleManager.IsKnownAsset(name) && __result == null)
	//		//{
	//		//	__result = ModAssetBundleManager.LoadAsset(name).Cast<GameObject>().transform.Cast<GearItem>();
	//		//	__runOriginal = false;
	//		//}
	//	}
	//}

	[HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]//Exists
	internal static class GameManager_ApplyCurrentQualitySettings
	{
		private static void Prefix()
		{
			//MelonLoader.MelonLogger.Warning("GameManager_ApplyCurrentQualitySettings");
			GearSpawnManager.PrepareScene();
		}
	}

}
