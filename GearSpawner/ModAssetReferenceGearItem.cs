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

public class ModAssetReferenceGearItem : Il2Cpp.AssetReferenceGearItem
{
	public ModAssetReferenceGearItem(string guid)
	: base(null)
	{
	}

	public GameObject GetOrLoadAsset()
	{

		MelonLoader.MelonLogger.Warning("HELLO WORLD!");

		return base.GetOrLoadAsset();
	}


}




