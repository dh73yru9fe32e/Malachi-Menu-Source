using HarmonyLib;
using UnityEngine;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(VRRig), "OnDisable")]
internal class GhostPatch : MonoBehaviour
{
	public static bool Prefix(VRRig __instance)
	{
		return !((Object)(object)__instance == (Object)(object)Mods.OwnRig());
	}
}
