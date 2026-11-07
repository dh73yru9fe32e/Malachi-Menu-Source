using HarmonyLib;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonkeAgent), "CheckReports")]
public class CheckReportsPatch
{
	private static bool Prefix()
	{
		return false;
	}
}
