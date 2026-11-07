using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(GameObject))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class MenuFixer : MonoBehaviour
{
	private static void Postfix(GameObject __result)
	{
		__result.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
	}
}
