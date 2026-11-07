using HarmonyLib;
using MalachiTemp.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
internal class OnLeave : HarmonyPatch
{
	private static void Prefix(Player otherPlayer)
	{
		if (otherPlayer != Mods.Local())
		{
			if (Mods.joinnotis)
			{
				Mods.SendNoti(Mods.MasterCheckNoti(otherPlayer));
			}
			WristMenu.lea.Add(otherPlayer);
		}
	}
}
