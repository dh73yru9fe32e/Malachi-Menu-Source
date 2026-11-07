using HarmonyLib;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonkeAgent), "DispatchReport")]
public class ReportDispatchPatch
{
	private static bool Prefix()
	{
		return false;
	}
}
