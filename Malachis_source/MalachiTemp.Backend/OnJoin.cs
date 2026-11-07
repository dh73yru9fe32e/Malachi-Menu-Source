using HarmonyLib;
using MalachiTemp.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
internal class OnJoin : HarmonyPatch
{
	private static void Prefix(Player newPlayer)
	{
		WristMenu.IsUsing(newPlayer, out var s);
		Mods.usings.Add(newPlayer);
		string text = ((s == "<color=#FF00FF>") ? ("\n" + s + "The Owner Of The Menu Has Joined</color>") : "");
		if (Mods.joinnotis)
		{
			Mods.SendNoti("[<color=blue>ROOM</color>] Player: " + s + newPlayer.NickName + "</color> Joined The Lobby" + text);
		}
	}
}
