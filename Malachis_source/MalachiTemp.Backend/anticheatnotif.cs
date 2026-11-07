using HarmonyLib;
using UnityEngine;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonkeAgent), "SendReport")]
internal class anticheatnotif : MonoBehaviour
{
	private static bool Prefix(string susReason, string susId, string susNick)
	{
		if (susReason != "empty rig" && susId == Mods.Local().UserId)
		{
			Mods.SendNoti("[<color=red>ANTICHEAT</color>] REPORTED FOR: " + susReason);
		}
		return false;
	}
}
