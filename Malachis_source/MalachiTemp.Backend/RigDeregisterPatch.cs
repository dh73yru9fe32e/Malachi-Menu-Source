using HarmonyLib;
using UnityEngine;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
public static class RigDeregisterPatch
{
	public static bool Prefix(VRRigJobManager __instance, VRRig rig)
	{
		return !((Object)(object)__instance == (Object)(object)Mods.OwnRig());
	}
}
