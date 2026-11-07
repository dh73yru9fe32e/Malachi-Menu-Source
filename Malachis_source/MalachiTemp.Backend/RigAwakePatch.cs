using HarmonyLib;
using UnityEngine;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(VRRig), "Awake")]
public static class RigAwakePatch
{
	public static bool Prefix(VRRig __instance)
	{
		return ((Object)((Component)__instance).gameObject).name != "Local Gorilla Player(Clone)";
	}
}
