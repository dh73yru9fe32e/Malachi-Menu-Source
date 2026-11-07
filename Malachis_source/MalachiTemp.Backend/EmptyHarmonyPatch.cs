using HarmonyLib;

namespace MalachiTemp.Backend;

[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class EmptyHarmonyPatch
{
	private static bool Prefix()
	{
		return false;
	}
}
