using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MalachiTemp.Backend;

internal class RigUtils : MonoBehaviour
{
	public static VRRig RigFromPlayer(Player p)
	{
		try
		{
			return GorillaGameManager.instance.FindPlayerVRRig(NetPlayer.op_Implicit(p));
		}
		catch
		{
			foreach (VRRig item in AllRigs())
			{
				if (item.playerNameVisible.ToLower() == p.NickName.ToLower() || PlayerFromRig(item) == p)
				{
					return item;
				}
			}
		}
		return null;
	}

	public static List<VRRig> AllRigs(bool i = true)
	{
		return i ? VRRigCache.ActiveRigs.ToList() : VRRigCache.ActiveRigs.Where((VRRig e) => !e.isOfflineVRRig).ToList();
	}

	public static List<Player> AllPlayers(bool i = true)
	{
		return i ? PhotonNetwork.PlayerList.ToList() : PhotonNetwork.PlayerListOthers.ToList();
	}

	public static List<NetPlayer> AllNets(bool i = true)
	{
		return i ? NetworkSystem.Instance.AllNetPlayers.ToList() : NetworkSystem.Instance.PlayerListOthers.ToList();
	}

	public static Player PlayerFromRig(VRRig p)
	{
		return p.Creator.GetPlayerRef();
	}

	public static Player PlayerFromRigSafe(VRRig p)
	{
		try
		{
			return p.Creator.GetPlayerRef();
		}
		catch
		{
			return null;
		}
	}

	public static Player PlayerFromActor(int a)
	{
		return (from e in AllPlayers()
			where e.ActorNumber == a
			select e).FirstOrDefault();
	}

	public static VRRig RigFromNet(NetPlayer p)
	{
		return RigFromPlayer(p.GetPlayerRef());
	}

	public static NetworkView NetViewFromRig(VRRig p)
	{
		return Mods.Create(p).Field(Mods.Field(23)).GetValue<NetworkView>();
	}

	public static string Map(VRRig r)
	{
		return EnumUtilExt.GetName<GTZone>(r.zoneEntity.currentZone);
	}
}
