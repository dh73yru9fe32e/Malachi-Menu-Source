using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx;
using GorillaNetworking;
using GorillaTagScripts;
using GorillaTagScripts.VirtualStumpCustomMaps;
using HarmonyLib;
using MalachiTemp.Backend;
using MalachiTemp.UI;
using Modio.Mods;
using Photon.Pun;
using Photon.Realtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MalachiTemp.Gui;

[BepInPlugin("malachis.reborn", "malachis.reborn.GUI", "1.0.0")]
public class GuiPlugin : BaseUnityPlugin
{
	[HarmonyPatch(typeof(ControllerInputPoller), "LateUpdate")]
	public class EmPa
	{
		private static void Postfix(ControllerInputPoller __instance)
		{
			if (LGrip)
			{
				__instance.leftControllerGripFloat = 1f;
				__instance.leftGrab = true;
				__instance.leftGrabRelease = false;
			}
			if (RGrip)
			{
				__instance.rightControllerGripFloat = 1f;
				__instance.rightGrab = true;
				__instance.rightGrabRelease = false;
			}
			if (LTrig)
			{
				__instance.leftControllerIndexFloat = 1f;
			}
			if (RTrig)
			{
				__instance.rightControllerIndexFloat = 1f;
			}
			if (X)
			{
				__instance.leftControllerPrimaryButton = true;
			}
			if (A)
			{
				__instance.rightControllerPrimaryButton = true;
			}
			if (Y)
			{
				__instance.leftControllerSecondaryButton = true;
			}
			if (B)
			{
				__instance.rightControllerSecondaryButton = true;
			}
		}
	}

	public static bool redo = false;

	public static bool RTrig;

	public static bool LTrig;

	public static bool RGrip;

	public static bool LGrip;

	public static bool A;

	public static bool B;

	public static bool X;

	public static bool Y;

	public static bool emgun;

	public static bool props;

	public static bool nex;

	public static Color[] colors = new Color[9]
	{
		Color.blue,
		Color.red,
		Color.white,
		Color.green,
		Color.magenta,
		Color.cyan,
		Color.yellow,
		Color.black,
		Color.grey
	};

	public static string[] colornames = new string[11]
	{
		"BLUE", "RED", "WHITE", "GREEN", "MAGENTA", "CYAN", "YELLOW", "BLACK", "GREY", "RGB",
		"MENU THEME"
	};

	public static bool open = true;

	public static bool Menu;

	public static bool Lobby;

	public static bool Other;

	public static bool info;

	public static bool Joiner;

	public static bool PlayerList;

	public static bool NameAndColor;

	public static bool MyCos;

	public static bool guicolor;

	public static bool keys;

	public static bool wasd;

	public static bool WASD;

	public static bool fpsb = false;

	public static bool WASDGrav;

	public static bool cos;

	public static bool more;

	public static bool solidback = false;

	public static bool fpsc;

	public static bool CanDrag = true;

	public static bool ser = false;

	private bool dragging = false;

	public static float WASDSpeed = 1.5f;

	public static float Red = 0f;

	public static float Green = 0f;

	public static float Blue = 0f;

	public static string searched = "";

	public static string Name = "";

	public static string playername = "";

	public static string Last = "";

	public static string la = "";

	public static string CodeToJoin = "";

	public static string bordercolor = "white";

	public static int gh = 0;

	public static int gh1 = 0;

	public static int gh2;

	public static int gh3;

	public static int gh4;

	public static int but = 10;

	public static int tex = 2;

	public static int bor = 2;

	public static int bac = 7;

	public static int q = 0;

	public static Camera Cam;

	public static VRRig rigthingy;

	public static KeyCode OpenAndCloseGUI = (KeyCode)277;

	private static Vector2 scrollPosition;

	private static Vector2 scrollPosition1;

	private Vector2 dragstart;

	public static Vector3 ogc = Vector3.zero;

	public static Rect w = new Rect(0f, 786f, 340f, 280f);

	public static List<ButtonInfo> butons = new List<ButtonInfo>();

	public static List<CosmeticItem> items = new List<CosmeticItem>();

	public static GUIStyle l;

	public static Color buttoncolor;

	public static Color textcolor = Color.white;

	public static Texture2D t = new Texture2D(2, 2, (TextureFormat)4, false);

	public void OnGUI()
	{
		try
		{
			if ((open && (WristMenu.Version() >= WristMenu.ver || WristMenu.ver == -0.9f)) || WristMenu.owner)
			{
				if (UnityInput.Current.GetKeyDown((KeyCode)91))
				{
					w.position = new Vector2(1f, 67f);
				}
				if (UnityInput.Current.GetKeyDown((KeyCode)92))
				{
					Mods.DisableAll();
				}
				if (but == 9)
				{
					buttoncolor = Mods.Rainbow();
				}
				if (but == 10)
				{
					buttoncolor = Mods.MenuColor();
				}
				if (tex == 9)
				{
					textcolor = Mods.Rainbow();
				}
				if (tex == 10)
				{
					textcolor = Mods.MenuColor();
				}
				if (but < 9)
				{
					buttoncolor = colors[but];
				}
				if (tex < 9)
				{
					textcolor = colors[tex];
				}
				GUIStyle val = new GUIStyle(GUI.skin.box);
				val.normal.background = c();
				GUIStyle val2 = val;
				GUIStyle val3 = new GUIStyle(GUI.skin.box);
				val3.normal.background = c();
				GUIStyle val4 = val3;
				GUI.Box(new Rect(w.x, w.y - 13f, w.width, w.height + 23f), "", val2);
				if (more)
				{
					GUI.Box(new Rect(w.x, w.y - 215f, w.width, 180f), "<color=white>More Info On: " + playername + "</color>", val4);
				}
				if (cos)
				{
					GUI.Box(new Rect(w.x, w.y - 195f, w.width, 160f), "<color=white>" + playername + "'s Cosmetics</color>", val2);
				}
				GUI.backgroundColor = buttoncolor;
				GUI.contentColor = textcolor;
				bordercolor = colornames[bor];
				GUIStyle val5 = new GUIStyle(GUI.skin.label)
				{
					fontSize = 13
				};
				GUIStyle val6 = new GUIStyle(GUI.skin.label)
				{
					fontSize = ((WristMenu.MenuTitle.Substring(22).Length > 3) ? 12 : 13),
					fontStyle = (FontStyle)1
				};
				GUI.Label(new Rect(w.x + 0.7f, w.y - 34f, 500f, 500f), WristMenu.MenuTitle.ToUpper() + " | GG/2AmFSzYeAV", val6);
				GUI.Label(new Rect(w.x, w.y - 16f, 500f, 500f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val5);
				if (!ser || !Menu)
				{
					GUI.Label(new Rect(w.x + 104f, w.y + 5f, 500f, 500f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val5);
				}
				GUI.Label(new Rect(w.x + 340f, w.y - 16f, 500f, 500f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val5);
				GUI.Label(new Rect(w.x + 3f, w.y, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val5);
				GUI.Label(new Rect(w.x + 3f, w.y - 23f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val5);
				GUI.Label(new Rect(w.x + 3f, w.y + 250f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val5);
				GUI.Label(new Rect(w.x + 3f, w.y + 278f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val5);
				if (Bu(new Rect(w.x + 4f, w.y + 263f, 335f, 25f), "Disconnect"))
				{
					PhotonNetwork.Disconnect();
					PT();
				}
				if (Bu(new Rect(w.x + 4f, w.y - 10f, 112f, 20f), "Menu"))
				{
					Lobby = false;
					Other = false;
					Menu = !Menu;
					PT();
				}
				if (Bu(new Rect(w.x + 116f, w.y - 10f, 112f, 20f), "Lobby"))
				{
					Menu = false;
					Other = false;
					Lobby = !Lobby;
					PT();
				}
				if (Bu(new Rect(w.x + 228f, w.y - 10f, 112f, 20f), "Other"))
				{
					Lobby = false;
					Menu = false;
					Other = !Other;
					PT();
				}
				if (Menu)
				{
					DrawMenuCats();
					if (butons.Count != 0)
					{
						DrawButtons(butons);
					}
				}
				if (Lobby)
				{
					DrawLobby();
				}
				if (Other)
				{
					DrawOther();
				}
				if (cos)
				{
					DrawItems();
				}
				if (more)
				{
					try
					{
						DrawMore();
						if (props)
						{
							DrawProps(rigthingy);
						}
					}
					catch
					{
					}
				}
				Dragging();
				foreach (ButtonInfo item in WristMenu.CatButtons1.Skip(3))
				{
					if (la == null)
					{
						la += item.buttonText;
					}
					else
					{
						la = la + "<color=white>< </color>" + item.buttonText + "<color=white> ></color>\n";
					}
				}
				l = new GUIStyle(GUI.skin.label)
				{
					alignment = (TextAnchor)0,
					fontSize = 17
				};
				if (la != null)
				{
					GUI.Label(new Rect(2f, -1f, 500f, 806f), la, l);
					la = "";
				}
			}
			if (WASD)
			{
				Mods.WASD();
			}
			if (!fpsb)
			{
				return;
			}
			try
			{
				Camera component = Mods.Find("Shoulder Camera").GetComponent<Camera>();
				if ((Object)(object)component != (Object)null)
				{
					Cam = component;
					if (fpsc)
					{
						Cam.fieldOfView = 105f;
						((Behaviour)((Component)((Component)Cam).gameObject.transform.Find("CM vcam1")).GetComponent<CinemachineVirtualCamera>()).enabled = false;
						((Component)Cam).gameObject.transform.position = ((Component)Mods.GIn().headCollider).transform.position + ((Component)Mods.GIn().headCollider).transform.forward * 0.087f;
						((Component)Cam).gameObject.transform.rotation = Quaternion.Lerp(((Component)Cam).transform.rotation, ((Component)Mods.GIn().headCollider).transform.rotation, 0.075f);
					}
					else if (!((Behaviour)((Component)((Component)Cam).gameObject.transform.Find("CM vcam1")).GetComponent<CinemachineVirtualCamera>()).enabled)
					{
						Cam.fieldOfView = 60f;
						((Behaviour)((Component)((Component)Cam).gameObject.transform.Find("CM vcam1")).GetComponent<CinemachineVirtualCamera>()).enabled = true;
					}
				}
			}
			catch
			{
			}
		}
		catch
		{
		}
	}

	public static string On(bool b)
	{
		return b ? "ON" : "OFF";
	}

	public static void PT(bool b = false, int s = -1)
	{
		Mods.OwnRig().PlayHandTapLocal((s == -1) ? Mods.ButtonSound : s, b, 0.1f);
	}

	public static string GetQueOrGameMode(int num)
	{
		string result;
		if (PhotonNetwork.InRoom)
		{
			result = ((num == 0) ? ((RoomInfo)PhotonNetwork.CurrentRoom).CustomProperties[(object)"gameMode"].ToString().Replace(PhotonNetworkController.Instance.currentJoinTrigger.networkZone, "").Replace(";DEFAULT;", "")
				.Replace(";COMPETITIVE;", "")
				.Replace(";MINIGAMES;", "")
				.Replace(";", "")
				.Replace("InfectionCompetitive", "RANKED") : ((RoomInfo)PhotonNetwork.CurrentRoom).CustomProperties[(object)"gameMode"].ToString().Replace(PhotonNetworkController.Instance.currentJoinTrigger.networkZone, "").Replace(((RoomInfo)PhotonNetwork.CurrentRoom).CustomProperties[(object)"gameMode"].ToString().Replace(PhotonNetworkController.Instance.currentJoinTrigger.networkZone, "").Replace(";DEFAULT;", "")
				.Replace(";COMPETITIVE;", "")
				.Replace(";MINIGAMES;", ""), "")
				.Replace(";DEFAULT|", "DEFAULT")
				.Replace(";COMPETITIVE;", "COMPETITIVE")
				.Replace(";MINIGAMES;", "MINIGAMES")
				.Replace(";", ""));
		}
		else
		{
			result = "67";
		}
		return result;
	}

	public static string GetAllPlayersPlats(bool all, Player p)
	{
		try
		{
			if (all)
			{
				List<VRRig> list = new List<VRRig>();
				List<VRRig> list2 = new List<VRRig>();
				List<VRRig> list3 = new List<VRRig>();
				foreach (VRRig item in RigUtils.AllRigs(i: false))
				{
					HashSet<string> hashSet = Mods.GetCos(item);
					((Dictionary<object, object>)(object)p.CustomProperties).TryGetValue((object)"platform", out object value);
					if (value == null)
					{
						value = "Quest";
					}
					if ((value != null && value.Equals("Quest")) || (!hashSet.Contains("FIRST LOGIN") && !hashSet.Contains("S. FIRST LOGIN")))
					{
						list.Add(item);
					}
					if ((value != null && value.Equals("steam")) || hashSet.Contains("S. FIRST LOGIN"))
					{
						list3.Add(item);
					}
					if (((value != null && value.Equals("PC")) || hashSet.Contains("FIRST LOGIN")) && !list3.Contains(item) && !list.Contains(item))
					{
						list2.Add(item);
					}
				}
				return "Quest: " + list.Count + " | OculusPC: " + list2.Count + " | Steam: " + list3.Count;
			}
			object value2 = "quest";
			try
			{
				((Dictionary<object, object>)(object)p.CustomProperties).TryGetValue((object)"platform", out value2);
			}
			catch
			{
				value2 = "Quest";
			}
			if (value2 == null)
			{
				value2 = "Quest";
			}
			HashSet<string> hashSet2 = Mods.GetCos(RigUtils.RigFromPlayer(p));
			if ((value2 != null && value2.Equals("steam")) || hashSet2.Contains("S. FIRST LOGIN"))
			{
				return "Steam";
			}
			if ((value2 != null && value2.Equals("PC")) || hashSet2.Contains("FIRST LOGIN"))
			{
				return "OculusPC";
			}
			return "Quest";
		}
		catch
		{
			return "";
		}
	}

	public static void Join(int num, string code)
	{
		if (PhotonNetwork.InRoom && num != 0)
		{
			PhotonNetwork.Disconnect();
			Join(num, code);
			return;
		}
		switch (num)
		{
		case 0:
			if (WristMenu.VIM.e)
			{
				redo = true;
				WristMenu.VIM.e = false;
			}
			PhotonNetworkController.Instance.AttemptToJoinSpecificRoomAsync(code, (JoinType)0, (Action<NetJoinResult>)((NetJoinResult n) =>
			{
				if ((int)n == 2)
				{
					Mods.SendNoti(Mods.Er + "Room: " + code + " Is Full!");
				}
				if ((int)n == 4)
				{
					Mods.SendNoti(Mods.Er + "Failed To Join: " + code);
				}
			}), -1);
			Mods.SendNoti("[<color=blue>ROOM</color>] Attempting To Join: " + code);
			if (redo)
			{
				WristMenu.VIM.e = true;
				redo = false;
			}
			break;
		case 1:
		{
			GorillaNetworkJoinTrigger[] array = Object.FindObjectsByType<GorillaNetworkJoinTrigger>((FindObjectsSortMode)1);
			((GorillaTriggerBox)array[Random.Range(0, array.Length)]).OnBoxTriggered();
			break;
		}
		case 2:
			if ((Object)(object)PhotonNetworkController.Instance.currentJoinTrigger != (Object)null)
			{
				((GorillaTriggerBox)PhotonNetworkController.Instance.currentJoinTrigger).OnBoxTriggered();
			}
			else
			{
				Mods.SendNoti(Mods.Er + "Cant Get Current Map!");
			}
			break;
		}
	}

	public void Update()
	{
		if (UnityInput.Current.GetKeyDown(OpenAndCloseGUI))
		{
			open = !open;
		}
	}

	private void Dragging()
	{
		if (CanDrag)
		{
			if ((int)Event.current.type == 0 && w.Contains(Event.current.mousePosition))
			{
				dragging = true;
				dragstart = Event.current.mousePosition - new Vector2(w.x, w.y);
			}
			else if ((int)Event.current.type == 1)
			{
				dragging = false;
			}
			if (dragging)
			{
				w.position = Event.current.mousePosition - dragstart;
			}
		}
	}

	public static void DrawLobby()
	{
		GUIStyle val = new GUIStyle(GUI.skin.label)
		{
			richText = true,
			fontSize = 13
		};
		val.normal.textColor = Color.white;
		GUIStyle val2 = val;
		if (Bu(new Rect(w.x + 4f, w.y + 15f, 100f, 20f), "Info"))
		{
			PlayerList = false;
			Joiner = false;
			NameAndColor = false;
			MyCos = false;
			info = !info;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 37f, 100f, 20f), "Joiner"))
		{
			PlayerList = false;
			info = false;
			NameAndColor = false;
			MyCos = false;
			Joiner = !Joiner;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 59f, 100f, 20f), "PlayerList"))
		{
			info = false;
			Joiner = false;
			NameAndColor = false;
			MyCos = false;
			PlayerList = !PlayerList;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 81f, 100f, 20f), "Name & Color"))
		{
			info = false;
			Joiner = false;
			PlayerList = false;
			MyCos = false;
			NameAndColor = !NameAndColor;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 103f, 100f, 20f), "My Cosmetics"))
		{
			info = false;
			Joiner = false;
			PlayerList = false;
			NameAndColor = false;
			MyCos = !MyCos;
			PT();
		}
		if (info)
		{
			if (PhotonNetwork.InRoom)
			{
				GUI.Label(new Rect(w.x + 109f, w.y + 11f, 500f, 20f), "Master: " + RigUtils.RigFromPlayer(PhotonNetwork.MasterClient).playerNameVisible, val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 30f, 500f, 20f), $"Players: {RigUtils.AllPlayers().Count}  |  FanClub: " + (PhotonNetwork.CurrentRoom.MaxPlayers == 20), val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 50f, 500f, 20f), "GameMode: " + GetQueOrGameMode(0).ToUpper().Replace("MODDED_", ""), val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 70f, 500f, 20f), "Is Public: " + PhotonNetwork.CurrentRoom.IsVisible.ToString().ToUpper() + "  |  Modded: " + ((object)((RoomInfo)PhotonNetwork.CurrentRoom).CustomProperties).ToString().Contains("MODDED").ToString()
					.ToUpper(), val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 90f, 500f, 20f), "Code: " + Regex.Replace(PhotonNetwork.CurrentRoom.Name, "<size=[^>]+>|</size>", ""), val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 110f, 500f, 20f), "Map: " + PhotonNetworkController.Instance.currentJoinTrigger.networkZone.ToUpper(), val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 130f, 500f, 20f), "Queue: " + GetQueOrGameMode(1) + "  |  Region: " + NetworkSystem.Instance.CurrentRegion.Replace("/*", "").ToUpper(), val2);
				Rect val3 = new Rect(w.x + 109f, w.y + 150f, 500f, 20f);
				string text;
				if (!PhotonNetwork.CurrentRoom.Name.Contains("@"))
				{
					text = "FALSE";
				}
				else
				{
					string text2;
					if (!(CustomMapManager.GetRoomMapId() != ModId.Null))
					{
						text2 = "";
					}
					else
					{
						long id = CustomMapManager.GetRoomMapId()._id;
						text2 = "  |  ID: " + id;
					}
					text = "TRUE" + text2;
				}
				GUI.Label(val3, "Is VStump: " + text, val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 170f, 500f, 20f), "Players That Have VIM: " + RigUtils.AllRigs().Where((VRRig e) =>
				{
					return SubscriptionManager.GetSubscriptionDetails(e).active;
				}).ToList()
					.Count, val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 190f, 500f, 20f), "Players Near A Computer: " + GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Count, val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 210f, 500f, 20f), "Players Near A Wardobe: " + (from e in RigUtils.AllPlayers()
					where CosmeticWardrobeProximityDetector.IsUserNearWardrobe(e.ActorNumber)
					select e).ToList().Count, val2);
				GUI.Label(new Rect(w.x + 109f, w.y + 230f, 500f, 20f), GetAllPlayersPlats(all: true, Mods.Local()), val2);
			}
			else
			{
				GUI.Label(new Rect(w.x + 109f, w.y + 12f, 500f, 20f), "Please Join A Room First", val2);
			}
		}
		if (Joiner)
		{
			GUIStyle val4 = new GUIStyle(GUI.skin.textArea);
			val4.onNormal.textColor = Color.white;
			val4.normal.textColor = Color.white;
			GUIStyle val5 = val4;
			CodeToJoin = GUI.TextArea(new Rect(w.x + 107f, w.y + 15f, 233f, 20f), CodeToJoin, val5);
			if (CodeToJoin.Length > 10)
			{
				CodeToJoin = CodeToJoin.Substring(0, 10);
			}
			CodeToJoin = CodeToJoin.ToUpper();
			if (Bu(new Rect(w.x + 107f, w.y + 35f, 233f, 20f), "Join Room: " + CodeToJoin))
			{
				if (CodeToJoin != "")
				{
					Join(0, CodeToJoin);
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Please Enter A Room Code First!");
				}
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 55f, 233f, 20f), "Join Random Room"))
			{
				Join(1, "");
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 75f, 233f, 20f), "Join Random Room In Current Map"))
			{
				Join(2, "");
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 95f, 233f, 20f), "Join Random Priv Room"))
			{
				Join(0, Mods.RandomString(s: true));
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 115f, 233f, 20f), "Rejoin Last Room") && Last != "")
			{
				Join(0, Last);
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 135f, 233f, 20f), "Change GameMode: " + ((object)GorillaComputer.instance.currentGameMode).ToString().ToUpper()))
			{
				string[] array = new string[12]
				{
					"Casual", "Infection", "Guardian", "HuntDown", "Paintbrawl", "Ambush", "FreezeTag", "Ghost", "PropHunt", "SuperInfect",
					"SuperCasual", "Guardian"
				};
				gh++;
				if (gh > 9)
				{
					gh = 0;
				}
				GorillaComputer.instance.OnModeSelectButtonPress(array[gh], false);
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 155f, 233f, 20f), "Change Queue: " + GorillaComputer.instance.currentQueue.ToUpper()))
			{
				string[] array2 = new string[3] { "DEFAULT", "COMPETITIVE", "MINIGAMES" };
				gh1++;
				if (gh1 > 2)
				{
					gh1 = 0;
				}
				GorillaComputer.instance.currentQueue = array2[gh1];
				PlayerPrefs.SetString("currentQueue", array2[gh1]);
				PlayerPrefs.Save();
				PT();
			}
		}
		if (PlayerList)
		{
			if (PhotonNetwork.InRoom)
			{
				List<Player> list = RigUtils.AllPlayers();
				scrollPosition = GUI.BeginScrollView(new Rect(w.x + 100f, w.y + 15f, 240f, 240f), scrollPosition, new Rect(0f, 0f, 0f, (float)list.Count * 80f));
				for (int num = 0; num < list.Count; num++)
				{
					VRRig val6 = RigUtils.RigFromPlayer(list[num]);
					if (Bu(new Rect(152.7f, 15f + (float)num * 80f, 71.9f, 20f), "Cosmetics"))
					{
						playername = list[num].NickName;
						rigthingy = val6;
						HashSet<string> hashSet = Mods.GetCos(val6);
						foreach (CosmeticItem allCosmetic in CosmeticsController.instance.allCosmetics)
						{
							if (!hashSet.Contains(allCosmetic.itemName) && items.Contains(allCosmetic))
							{
								items.Remove(allCosmetic);
							}
							else if (hashSet.Contains(allCosmetic.itemName) && !items.Contains(allCosmetic) && allCosmetic.overrideDisplayName != "Slingshot" && !allCosmetic.overrideDisplayName.Contains("CHICKEN STEP") && (int)allCosmetic.itemCategory != 24)
							{
								items.Add(allCosmetic);
							}
						}
						more = false;
						cos = true;
						PT();
					}
					if (Bu(new Rect(176f, 35f + (float)num * 80f, 49f, 20f), "More"))
					{
						playername = list[num].NickName;
						rigthingy = val6;
						HashSet<string> hashSet2 = Mods.GetCos(val6);
						foreach (CosmeticItem allCosmetic2 in CosmeticsController.instance.allCosmetics)
						{
							if (!hashSet2.Contains(allCosmetic2.itemName) && items.Contains(allCosmetic2))
							{
								items.Remove(allCosmetic2);
							}
							else if (hashSet2.Contains(allCosmetic2.itemName) && !items.Contains(allCosmetic2) && allCosmetic2.overrideDisplayName != "Slingshot" && !allCosmetic2.overrideDisplayName.Contains("CHICKEN STEP") && (int)allCosmetic2.itemCategory != 24)
							{
								items.Add(allCosmetic2);
							}
						}
						cos = false;
						more = true;
						PT();
					}
					Color playerColor = val6.playerColor;
					List<object> list2 = ((Dictionary<object, object>)(object)list[num].CustomProperties).Keys.Where((object ee) => Mods.Nprop(ee) && !ee.Equals(WristMenu.cus[0])).ToList();
					GUI.Label(new Rect(8f, 0f + (float)num * 80f, 223f, 999f), list[num].NickName + "</color>  |  <color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">▬▬</color>\n<color=red>" + Mathf.RoundToInt(playerColor.r * 9f) + "</color>,<color=green>" + Mathf.RoundToInt(playerColor.g * 9f) + "</color>,<color=#9d9dfa>" + Mathf.RoundToInt(playerColor.b * 9f) + "</color> | <color=red>" + Mathf.RoundToInt(playerColor.r * 255f) + "</color>,<color=green>" + Mathf.RoundToInt(playerColor.g * 255f) + "</color>,<color=#9d9dfa>" + Mathf.RoundToInt(playerColor.b * 255f) + "</color>\nActorNumber:<color=yellow> " + list[num].ActorNumber + "</color>\nID:<color=cyan> " + list[num].UserId + "</color>\nPlatform:<color=white> " + GetAllPlayersPlats(all: false, list[num]) + ((list2.Count > 0) ? ("  |  " + list2.Count + " Mod" + ((list2.Count == 1) ? "" : "s")) : "") + "</color>", val2);
				}
				GUI.EndScrollView();
			}
			else
			{
				GUI.Label(new Rect(w.x + 109f, w.y + 12f, 500f, 20f), "Please Join A Room First", val2);
			}
		}
		if (NameAndColor)
		{
			GUI.Label(new Rect(w.x + 109f, w.y + 11f, 500f, 20f), "Current Name: " + Mods.Local().NickName, val2);
			Name = GUI.TextArea(new Rect(w.x + 107f, w.y + 29f, 233f, 20f), Name);
			if (Name.Length > 12)
			{
				Name = Name.Substring(0, 12);
			}
			Name = Name.ToUpper();
			if (Bu(new Rect(w.x + 107f, w.y + 49f, 233f, 20f), "Set Name: " + Name))
			{
				if (Name != "")
				{
					Mods.ChangeName(Name, Color.clear, "");
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Please Enter A Name First!");
				}
				PT();
			}
			Color playerColor2 = Mods.OwnRig().playerColor;
			GUI.Label(new Rect(w.x + 109f, w.y + 60f, 500f, 500f), "Current Color: <color=red>" + Mathf.RoundToInt(playerColor2.r * 9f) + "</color><color=green>" + Mathf.RoundToInt(playerColor2.g * 9f) + "</color><color=blue>" + Mathf.RoundToInt(playerColor2.b * 9f) + "</color> | <color=red>" + Mathf.RoundToInt(playerColor2.r * 255f) + "</color>,<color=green>" + Mathf.RoundToInt(playerColor2.g * 255f) + "</color>,<color=blue>" + Mathf.RoundToInt(playerColor2.b * 255f) + "</color> | <size=24><color=#" + ColorUtility.ToHtmlStringRGB(playerColor2) + ">▬</color></size>", val2);
			Red = GUI.HorizontalSlider(new Rect(w.x + 107f, w.y + 90f, 233f, 20f), Red, 0f, 1f);
			Green = GUI.HorizontalSlider(new Rect(w.x + 107f, w.y + 110f, 233f, 20f), Green, 0f, 1f);
			Blue = GUI.HorizontalSlider(new Rect(w.x + 107f, w.y + 130f, 233f, 20f), Blue, 0f, 1f);
			if (Bu(new Rect(w.x + 107f, w.y + 150f, 233f, 20f), "Set Color: <color=red>" + Mathf.RoundToInt(Red * 9f) + "</color><color=green>" + Mathf.RoundToInt(Green * 9f) + "</color><color=blue>" + Mathf.RoundToInt(Blue * 9f) + "</color> | <color=red>" + Mathf.RoundToInt(Red * 255f) + "</color>,<color=green>" + Mathf.RoundToInt(Green * 255f) + "</color>,<color=blue>" + Mathf.RoundToInt(Blue * 255f) + "</color> | <size=14><color=#" + ColorUtility.ToHtmlStringRGB(new Color(Red, Green, Blue)) + ">▬▬</color></size>", new GUIStyle(GUI.skin.button)
			{
				richText = true
			}))
			{
				Mods.ChangeName("", new Color(Red, Green, Blue), "");
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 170f, 233f, 20f), "Change Turn Type: " + PlayerPrefs.GetString("stickTurning")))
			{
				string[] array3 = new string[3] { "SMOOTH", "SNAP", "NONE" };
				gh2++;
				if (gh2 > 2)
				{
					gh2 = 0;
				}
				PlayerPrefs.SetString("stickTurning", array3[gh2]);
				PlayerPrefs.Save();
				Mods.F<GorillaSnapTurn>(5f).FirstOrDefault().ChangeTurnMode(array3[gh2], PlayerPrefs.GetInt("turnFactor"));
				GorillaComputer.instance.UpdateScreen();
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 190f, 233f, 20f), "Change Turn Speed: " + PlayerPrefs.GetInt("turnFactor")))
			{
				gh3++;
				if (gh3 > 9)
				{
					gh3 = 0;
				}
				PlayerPrefs.SetInt("turnFactor", gh3);
				PlayerPrefs.Save();
				Mods.F<GorillaSnapTurn>(5f).FirstOrDefault().ChangeTurnMode(PlayerPrefs.GetString("stickTurning"), gh3);
				GorillaComputer.instance.UpdateScreen();
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 210f, 233f, 20f), "Change Mic Input: " + GorillaComputer.instance.pttType))
			{
				string[] array4 = new string[3] { "OPEN MIC", "PUSH TO TALK", "PUSH TO MUTE" };
				gh4++;
				if (gh4 > 2)
				{
					gh4 = 0;
				}
				PlayerPrefs.SetString("pttType", array4[gh4]);
				PlayerPrefs.Save();
				GorillaComputer.instance.pttType = array4[gh4];
				GorillaComputer.instance.UpdateScreen();
				PT();
			}
		}
		if (!MyCos)
		{
			return;
		}
		if (Mods.CosDone())
		{
			List<CosmeticItem> list3 = new List<CosmeticItem>();
			foreach (CosmeticItem allCosmetic3 in CosmeticsController.instance.allCosmetics)
			{
				if (Mods.GetCos(Mods.OwnRig()).Contains(allCosmetic3.itemName) && !list3.Contains(allCosmetic3) && allCosmetic3.overrideDisplayName != "Slingshot" && !allCosmetic3.overrideDisplayName.Contains("CHICKEN STEP"))
				{
					list3.Add(allCosmetic3);
				}
			}
			float num2 = ((list3.Count <= 12) ? 232f : 218f);
			scrollPosition = GUI.BeginScrollView(new Rect(w.x + 100f, w.y + 15f, 240f, 240f), scrollPosition, new Rect(0f, 0f, 0f, (float)list3.Count * 22f));
			for (int num3 = 0; num3 < list3.Count; num3++)
			{
				if (Bu(new Rect(7f, (float)num3 * 22f, num2, 20f), "<color=" + (Mods.OwnRig().cosmeticSet.items.Contains(list3[num3]) ? "green" : "red") + ">" + list3[num3].overrideDisplayName + "</color>"))
				{
					CosmeticsController.instance.PressWardrobeItemButton(list3[num3], false, false);
					PT();
				}
			}
			GUI.EndScrollView();
		}
		else
		{
			GUI.Label(new Rect(w.x + 109f, w.y + 12f, 500f, 500f), "Please Wait For Cosmetics To Get\nInitialized", val2);
		}
	}

	public static void DrawProps(VRRig p)
	{
		GUIStyle val = new GUIStyle(GUI.skin.label)
		{
			fontSize = 15
		};
		val.normal.textColor = Color.white;
		GUIStyle val2 = val;
		string text = "";
		foreach (object item in ((Dictionary<object, object>)(object)RigUtils.PlayerFromRig(p).CustomProperties).Keys.Where((object e) => Mods.Nprop(e) && !e.Equals(WristMenu.cus[0])))
		{
			text = string.Concat(new string[3]
			{
				text,
				item.ToString() + " | " + RigUtils.PlayerFromRig(p).CustomProperties[(object)item.ToString()],
				"\n"
			});
		}
		if (text == "")
		{
			text = "No Mods Found";
		}
		GUI.Label(new Rect(w.x + 347f, w.y - 215f, 999f, 999f), text, val2);
	}

	public static void DrawMore()
	{
		VRRig r = rigthingy;
		if (!PhotonNetwork.InRoom || !RigUtils.AllRigs().Contains(r))
		{
			props = false;
			more = false;
		}
		GUIStyle val = new GUIStyle(GUI.skin.label)
		{
			fontSize = 15
		};
		val.normal.textColor = Color.white;
		GUIStyle val2 = val;
		GUIStyle val3 = new GUIStyle(GUI.skin.label)
		{
			fontSize = 13
		};
		GUI.Label(new Rect(w.x, w.y - 220f, w.width, 9999f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val3);
		GUI.Label(new Rect(w.x + 340f, w.y - 220f, w.width, 9999f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val3);
		GUI.Label(new Rect(w.x + 3f, w.y - 225.3f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val3);
		GUI.Label(new Rect(w.x + 3f, w.y - 45.5f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val3);
		if (Bu(new Rect(w.x + 294f, w.y - 212f, 45f, 20f), "Close"))
		{
			props = false;
			more = false;
			PT();
		}
		if ((Object)(object)rigthingy != (Object)(object)Mods.OwnRig())
		{
			if (Bu(new Rect(w.x + 294f, w.y - 192f, 45f, 20f), "Mods"))
			{
				props = !props;
				PT();
			}
		}
		else
		{
			props = false;
		}
		List<CosmeticItem> list = new List<CosmeticItem>();
		if (items.Count > 0)
		{
			list = items.OrderBy((CosmeticItem q) =>
			{
				return r.CheckCosmeticAge(q.itemName);
			}).ToList();
		}
		string[] array = new string[9]
		{
			"FPS: " + Mods.Create(r).Field(Mods.Field(12)).GetValue()
				.ToString() + "  |  Distance: " + $"{Vector3.Distance(((Component)Camera.main).transform.position, ((Component)rigthingy).transform.position):F1}m",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		string[] array2 = new string[5]
		{
			"\nQuest Score: ",
			r.GetCurrentQuestScore().ToString(),
			"  |  Velocity: ",
			null,
			null
		};
		Vector3 val4 = r.LatestVelocity();
		array2[3] = val4.magnitude.ToString("F1");
		array2[4] = " m/s";
		array[1] = string.Concat(array2);
		array[2] = "\nTagged: " + Mods.ActualTagged(r).ToString().ToUpper() + "  |  Mat: " + Mods.TaggedName(r);
		array[3] = "\nVIM: " + SubscriptionManager.GetSubscriptionDetails(r).active.ToString().ToUpper() + "  |  Map: " + RigUtils.Map(r);
		array[4] = "\nNearComputer: " + GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(TouchingPlayerInfo.op_Implicit(RigUtils.PlayerFromRig(r).UserId)).ToString().ToUpper() + "  |  NearWardobe: " + CosmeticWardrobeProximityDetector.IsUserNearWardrobe(RigUtils.PlayerFromRig(r).ActorNumber).ToString().ToUpper();
		array[5] = "\nInTryOn: " + r.inTryOnRoom.ToString().ToUpper() + "  |  Cosmetics: " + items.Count + "  |  Volume: " + Mathf.RoundToInt(((Component)r).GetComponent<GorillaSpeakerLoudness>().Loudness * 100f);
		array[6] = "\nTurn Type: " + Mods.Create(r).Field(Mods.Field(31)).GetValue()
			.ToString() + "  |  Turn Speed: " + Mods.Create(r).Field(Mods.Field(32)).GetValue()
			.ToString();
		array[7] = ((list.Count > 0) ? ("\nOldest Cosmetic: " + r.CheckCosmeticAge(list.Last().itemName) + "d  |  Newest Cosmetic: " + r.CheckCosmeticAge(list.First().itemName) + "d") : "");
		array[8] = string.Format("\nMSID: {0}", ((Dictionary<object, object>)(object)RigUtils.PlayerFromRig(r).CustomProperties).Keys.Contains("mothershipId") ? RigUtils.PlayerFromRig(r).CustomProperties[(object)"mothershipId"] : "");
		string text = string.Concat(array);
		GUI.Label(new Rect(w.x + 4f, w.y - 200f, w.width, 99999f), text, val2);
	}

	public static void DrawItems()
	{
		if (!PhotonNetwork.InRoom || !RigUtils.AllRigs().Contains(rigthingy))
		{
			cos = false;
		}
		GUIStyle val = new GUIStyle(GUI.skin.label)
		{
			fontSize = 10
		};
		val.normal.textColor = Color.white;
		GUIStyle val2 = val;
		GUIStyle val3 = new GUIStyle(GUI.skin.label)
		{
			fontSize = 13
		};
		GUI.Label(new Rect(w.x, w.y - 200.1f, w.width, 9999f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val3);
		GUI.Label(new Rect(w.x + 340f, w.y - 200.1f, w.width, 9999f), "<color=" + bordercolor + ">|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|</color>", val3);
		GUI.Label(new Rect(w.x + 3f, w.y - 205.3f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val3);
		GUI.Label(new Rect(w.x + 3f, w.y - 45.3f, 500f, 500f), "<color=" + bordercolor + ">------------------------------------------------------------------------------</color>", val3);
		if (Bu(new Rect(w.x + 294f, w.y - 192.2f, 45f, 20f), "Close"))
		{
			cos = false;
			PT();
		}
		if (items.Count > 0)
		{
			scrollPosition1 = GUI.BeginScrollView(new Rect(w.x, w.y - 175.5f, w.width, 140f), scrollPosition1, new Rect(0f, 0f, 0f, (float)items.Count * 19f));
			for (int i = 0; i < items.Count; i++)
			{
				GUI.Label(new Rect(4f, 0f + (float)i * 19f, w.width, 100000000f), string.Format("<color=yellow>{0}</color><color=white> | </color><color=cyan>{1}</color><color=white> | </color>{2}<color=white> | {3}D</color>", items[i].overrideDisplayName, items[i].itemCategory, rigthingy.cosmeticSet.items.Contains(items[i]) ? "<color=green>ON</color>" : "<color=red>OFF</color>", rigthingy.CheckCosmeticAge(items[i].itemName)), val2);
			}
			GUI.EndScrollView();
		}
		else
		{
			GUI.Label(new Rect(w.x + 4f, w.y - 180f, w.width, 99999f), "This Player Has No Cosmetics!", val2);
		}
	}

	public static void DrawOther()
	{
		GUIStyle val = new GUIStyle(GUI.skin.label)
		{
			fontSize = 14,
			fontStyle = (FontStyle)1
		};
		if (Bu(new Rect(w.x + 4f, w.y + 15f, 100f, 20f), "WASD"))
		{
			keys = false;
			guicolor = false;
			wasd = !wasd;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 37f, 100f, 20f), "GUI Settings"))
		{
			keys = false;
			wasd = false;
			guicolor = !guicolor;
			PT();
		}
		if (Bu(new Rect(w.x + 4f, w.y + 59f, 100f, 20f), "Keybinds"))
		{
			wasd = false;
			guicolor = false;
			keys = !keys;
			PT();
		}
		if (wasd)
		{
			if (Bu(new Rect(w.x + 107f, w.y + 15f, 233f, 20f), "WASD Enabled: " + WASD.ToString().ToUpper()))
			{
				WASD = !WASD;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 35f, 233f, 20f), "WASD No Gravity: " + WASDGrav.ToString().ToUpper()))
			{
				WASDGrav = !WASDGrav;
				PT();
			}
			GUI.Label(new Rect(w.x + 107f, w.y + 57f, 233f, 20f), "WASD Speed: " + WASDSpeed, val);
			WASDSpeed = GUI.HorizontalSlider(new Rect(w.x + 107f, w.y + 78f, 233f, 18f), WASDSpeed, 0f, 20f);
			GUI.Label(new Rect(w.x + 107f, w.y + 90f, 233f, 20f), "Controller Emulator:", val);
			if (Bu(new Rect(w.x + 107f, w.y + 112f, 100f, 20f), "L Trigger: " + On(LTrig)))
			{
				LTrig = !LTrig;
				PT();
			}
			if (Bu(new Rect(w.x + 239f, w.y + 112f, 100f, 20f), "R Trigger: " + On(RTrig)))
			{
				RTrig = !RTrig;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 132f, 92f, 20f), "L Grip: " + On(LGrip)))
			{
				LGrip = !LGrip;
				PT();
			}
			if (Bu(new Rect(w.x + 248f, w.y + 132f, 92f, 20f), "R Grip: " + On(RGrip)))
			{
				RGrip = !RGrip;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 152f, 58f, 20f), "A: " + On(A)))
			{
				A = !A;
				PT();
			}
			if (Bu(new Rect(w.x + 163f, w.y + 152f, 58f, 20f), "B: " + On(B)))
			{
				B = !B;
				PT();
			}
			if (Bu(new Rect(w.x + 221f, w.y + 152f, 58f, 20f), "X: " + On(X)))
			{
				X = !X;
				PT();
			}
			if (Bu(new Rect(w.x + 279f, w.y + 152f, 58f, 20f), "Y: " + On(Y)))
			{
				Y = !Y;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 173f, 104f, 20f), "L Joystick: " + On(WristMenu.ljoy)))
			{
				WristMenu.bu = !WristMenu.bu;
				PT();
			}
			if (Bu(new Rect(w.x + 235f, w.y + 173f, 104f, 20f), "R Joystick: " + On(WristMenu.rjoy)))
			{
				WristMenu.ru = !WristMenu.ru;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 193f, 233f, 20f), "Emulate PC Gun: " + On(emgun)))
			{
				emgun = !emgun;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 213f, 233f, 20f), "First Person Camera: " + On(fpsc)))
			{
				fpsc = !fpsc;
				fpsb = true;
				PT();
			}
		}
		if (guicolor)
		{
			if (Bu(new Rect(w.x + 107f, w.y + 15f, 233f, 20f), "Button Color: " + colornames[but]))
			{
				but++;
				if (but > 10)
				{
					but = 0;
				}
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 35f, 233f, 20f), "Text Color: " + colornames[tex]))
			{
				tex++;
				if (tex > 10)
				{
					tex = 0;
				}
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 55f, 233f, 20f), "Border Color: " + colornames[bor]))
			{
				bor++;
				if (bor > 8)
				{
					bor = 0;
				}
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 75f, 233f, 20f), "Background Color: " + colornames[bac]))
			{
				bac++;
				if (bac > 10)
				{
					bac = 0;
				}
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 95f, 233f, 20f), "Solid Background: " + solidback.ToString().ToUpper()))
			{
				solidback = !solidback;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 115f, 233f, 20f), "Draggable GUI: " + CanDrag.ToString().ToUpper()))
			{
				CanDrag = !CanDrag;
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 135f, 233f, 20f), "Save Settings"))
			{
				Mods.Save();
				PT();
			}
			if (Bu(new Rect(w.x + 107f, w.y + 155f, 233f, 20f), "Load Settings"))
			{
				string text = File.ReadAllText("Malachis_Menu_Reborn/Saved_Settings2.txt");
				string[] array = text.Split(new string[1] { "\n" }, StringSplitOptions.None);
				but = int.Parse(array[20]);
				tex = int.Parse(array[21]);
				bor = int.Parse(array[22]);
				bac = int.Parse(array[23]);
				solidback = bool.Parse(array[24]);
				CanDrag = bool.Parse(array[25]);
				PT();
			}
		}
		if (keys)
		{
			GUIStyle val2 = new GUIStyle(GUI.skin.label)
			{
				richText = true,
				fontSize = 14
			};
			val2.normal.textColor = Color.white;
			GUIStyle val3 = val2;
			GUI.Label(new Rect(w.x + 109f, w.y + 12f, 500f, 500f), "<b>[</b> (Left Bracket) = Recenter GUI\n\n<b>\\</b> (Backslash) = Disable All Mods\n\n<b>Insert</b> (FN + Delete) = Toggle GUI\n\n<b>Q</b> = Hold Q And Press A Mod In\n       \"Menu\" To Favorite It", val3);
		}
	}

	public static void DrawMenuCats()
	{
		if (!ser)
		{
			List<ButtonInfo> buttons = WristMenu.buttons;
			if (!nex)
			{
				if (Bu(new Rect(w.x + 4f, w.y + 15f, 100f, 20f), buttons[0].buttonText))
				{
					PT();
					butons = WristMenu.settingsbuttons.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 37f, 100f, 20f), buttons[1].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons1.Skip(1).ToList();
					q = 1;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 59f, 100f, 20f), buttons[2].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons12.Skip(1).ToList();
					q = 2;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 81f, 100f, 20f), buttons[3].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons2.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 102f, 100f, 20f), buttons[4].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons3.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 124f, 100f, 20f), buttons[5].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons4.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 146f, 100f, 20f), buttons[6].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons5.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 168f, 100f, 20f), buttons[7].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons6.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 190f, 100f, 20f), buttons[8].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons7.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 212f, 100f, 20f), buttons[9].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons8.Skip(1).ToList();
					q = 0;
				}
			}
			else
			{
				if (Bu(new Rect(w.x + 4f, w.y + 15f, 100f, 20f), buttons[10].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons9.Skip(1).ToList();
					q = 0;
				}
				if (Bu(new Rect(w.x + 4f, w.y + 37f, 100f, 20f), buttons[11].buttonText))
				{
					PT();
					butons = WristMenu.CatButtons10.Skip(1).ToList();
					q = 0;
				}
			}
			if (Bu(new Rect(w.x + 4f, w.y + 234f, 100f, 20f), nex ? "<<<<<<" : ">>>>>>"))
			{
				PT();
				nex = !nex;
			}
			if (Mods.searchB)
			{
				if (Bu(new Rect(w.x + 342f, w.y - 10f, 52f, 20f), "Search"))
				{
					PT();
					butons = new List<ButtonInfo>
					{
						new ButtonInfo
						{
							buttonText = "gui search"
						}
					};
					q = 0;
				}
				if (butons.Count != 0 && butons[0].buttonText == "gui search")
				{
					GUIStyle val = new GUIStyle(GUI.skin.textArea);
					val.onNormal.textColor = Color.white;
					val.normal.textColor = Color.white;
					GUIStyle val2 = val;
					searched = GUI.TextArea(new Rect(w.x + 107f, w.y + 15f, 233f, 20f), searched, val2);
					if (searched.Length > 30)
					{
						searched = searched.Substring(0, 30);
					}
					searched = searched.ToUpper();
				}
			}
			if (q == 1 && butons.Count != WristMenu.CatButtons1.Skip(1).ToList().Count)
			{
				butons = WristMenu.CatButtons1.Skip(1).ToList();
			}
			if (q == 2 && butons.Count != WristMenu.CatButtons12.Skip(1).ToList().Count)
			{
				butons = WristMenu.CatButtons12.Skip(1).ToList();
			}
		}
		else
		{
			GUIStyle val3 = new GUIStyle(GUI.skin.textArea);
			val3.onNormal.textColor = Color.white;
			val3.normal.textColor = Color.white;
			GUIStyle val4 = val3;
			Mods.typed = GUI.TextArea(new Rect(w.x + 4f, w.y + 15f, 330f, 20f), Mods.typed, val4);
			if (Mods.typed.Length > 30)
			{
				Mods.typed = Mods.typed.Substring(0, 30);
			}
			Mods.typed = Mods.typed.ToUpper();
			butons = WristMenu.CatButtons11;
		}
	}

	public static void DrawButtons(List<ButtonInfo> B)
	{
		bool flag = false;
		if (B[0].buttonText == "gui search" || ser)
		{
			flag = true;
			B = ((!ser) ? (from e in Mods.GetAllButtons(i: true, ii: true, c: true)
				where e.buttonText != "Keyboard" && !e.buttonText.Contains("Exit") && e.buttonText.ToUpper().Contains(searched)
				select e).ToList() : ((Mods.typed == "" || B.Count < 3) ? WristMenu.CatButtons11 : WristMenu.CatButtons11.Where((ButtonInfo e) => e.buttonText.ToUpper().Contains(Mods.typed)).ToList()));
		}
		scrollPosition = GUI.BeginScrollView(new Rect(w.x + ((!ser) ? 100f : 4f), w.y + (flag ? 40f : 15f), (!ser) ? 240f : 330f, flag ? 215f : 240f), scrollPosition, new Rect(0f, 0f, 0f, (float)B.Count * 22f));
		int i;
		for (i = 0; i < B.Count; i++)
		{
			float num = ((!ser) ? 7f : 1f);
			float num2 = (float)i * 22f;
			float num3;
			if (B.Count <= 13)
			{
				num3 = ((!ser) ? 232f : 340f);
			}
			else
			{
				num3 = ((!ser) ? 218f : 318f);
			}
			if (!Bu(new Rect(num, num2, num3, 20f), B[i].buttonText))
			{
				continue;
			}
			if (UnityInput.Current.GetKey((KeyCode)113) && (Mods.actsearch || !Mods.searching) && B[i].buttonText != "Favorite Mods")
			{
				if (WristMenu.CatButtons12.Any((ButtonInfo e) => e.buttonText == B[i].buttonText))
				{
					WristMenu.CatButtons12.RemoveAll((ButtonInfo e) => e.buttonText == B[i].buttonText);
				}
				else
				{
					WristMenu.CatButtons12.Insert(1, B[i]);
				}
				Mods.SaveFavs();
			}
			else if (B[i].buttonText == "Keyboard")
			{
				WristMenu.Toggle("sear");
			}
			else
			{
				B[i].enabled = !B[i].enabled;
				WristMenu.lx = i;
			}
			PT();
		}
		GUI.EndScrollView();
	}

	public static bool Bu(Rect pos, string n, GUIStyle g = null)
	{
		return (g == null) ? GUI.Button(pos, n) : GUI.Button(pos, n, g);
	}

	public static Texture2D c()
	{
		Color val = Color.white;
		float num = (solidback ? 1f : 0.29f);
		if (bac == 9)
		{
			val = Mods.Rainbow();
		}
		if (bac == 10)
		{
			val = Mods.MenuColor();
		}
		if (bac < 9)
		{
			val = colors[bac];
		}
		Color val2 = new Color(val.r, val.g, val.b, num);
		for (int i = 0; i < ((Texture)t).width; i++)
		{
			for (int j = 0; j < ((Texture)t).height; j++)
			{
				t.SetPixel(i, j, val2);
			}
		}
		t.Apply();
		return t;
	}

	static Gui()
	{
	}
}
