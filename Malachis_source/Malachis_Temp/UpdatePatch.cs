using GTAG_NotificationLib;
using GorillaLocomotion;
using HarmonyLib;
using MalachiTemp.Backend;
using MalachiTemp.UI;
using UnityEngine;

namespace Malachis_Temp;

[HarmonyPatch(typeof(GTPlayer), "FixedUpdate")]
internal class UpdatePatch
{
	private static bool a;

	public static GameObject g;

	private static void Postfix()
	{
		if (!a)
		{
			a = true;
			g = new GameObject();
			g.AddComponent<Plugin>();
			g.AddComponent<WristMenu>();
			g.AddComponent<CoroutineRunner>();
			g.AddComponent<RigUtils>();
			g.AddComponent<Mods>();
			g.AddComponent<GhostPatch>();
			g.AddComponent<NotifiLib>();
			Mods.Load();
			Mods.Load1();
			Mods.LoadFavs();
			Object.DontDestroyOnLoad((Object)(object)g);
		}
	}
}
