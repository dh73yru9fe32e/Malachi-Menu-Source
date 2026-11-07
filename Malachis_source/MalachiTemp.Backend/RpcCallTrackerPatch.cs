using HarmonyLib;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonkeAgent), "GetRPCCallTracker")]
internal class RpcCallTrackerPatch
{
	private static bool Prefix()
	{
		return false;
	}
}
