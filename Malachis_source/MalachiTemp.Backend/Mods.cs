using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BepInEx;
using ExitGames.Client.Photon;
using GTAG_NotificationLib;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using GorillaNetworking.Store;
using GorillaTag;
using GorillaTag.Rendering;
using GorillaTag.Sports;
using GorillaTagScripts;
using GorillaTagScripts.Builder;
using GorillaTagScripts.Subscription.AlarmClocks;
using HarmonyLib;
using MalachiTemp.UI;
using MalachiTemp.Utilities;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Voxels;
using MalachiTemp.Gui;

namespace MalachiTemp.Backend;

internal class Mods : MonoBehaviour
{
	public class Delay
	{
		public Vector3 Orbit(string i, Vector3 p, float x = 1f, float y = 0f, float z = 1f, float s = 4f, Dictionary<string, float> f = null)
		{
			if (!da.ContainsKey(i))
			{
				da[i] = 0f;
			}
			da[i] += s * Time.deltaTime;
			float[] array = new float[3]
			{
				Mathf.Cos(da[i]),
				1f,
				Mathf.Sin(da[i])
			};
			if (f != null)
			{
				if (f.ContainsKey("x"))
				{
					array[0] = f["x"];
				}
				if (f.ContainsKey("y"))
				{
					array[1] = f["y"];
				}
				if (f.ContainsKey("z"))
				{
					array[2] = f["z"];
				}
			}
			return new Vector3(p.x + x * array[0], p.y + y * array[1], p.z + z * array[2]);
		}

		public void D(float t, Action s)
		{
			int metadataToken = s.Method.MetadataToken;
			if (!dd.ContainsKey(metadataToken) || Time.time > dd[metadataToken])
			{
				s();
				dd[metadataToken] = Time.time + t;
			}
		}
	}

	public static bool searching = false;

	public static bool boardspa = false;

	public static GameObject keyboard = null;

	public static GameObject butts = null;

	public static string typed = "";

	public static List<SharedBlocksKeyboardButton> keys = new List<SharedBlocksKeyboardButton>();

	public static GameObject type;

	public static TextMesh typetext;

	public static GameObject reference1 = null;

	public static bool refreshed = false;

	public static object[] refreshs = new object[0];

	public static int ognum = -1;

	public static bool actsearch = false;

	public static bool doser = false;

	public static List<GameObject> rsp = new List<GameObject>();

	public static List<GameObject> lsp = new List<GameObject>();

	public static Font[] fonts = new Font[6]
	{
		Resources.GetBuiltinResource<Font>("Arial.ttf"),
		Font.CreateDynamicFontFromOSFont("Agency FB", 15),
		Font.CreateDynamicFontFromOSFont("Verdana", 15),
		Font.CreateDynamicFontFromOSFont("Comic Sans MS", 15),
		Font.CreateDynamicFontFromOSFont("Consolas", 10),
		Font.CreateDynamicFontFromOSFont("Candara", 10)
	};

	public static float smooth = 26.5f;

	public static PrimitiveType pointshape = (PrimitiveType)0;

	public static GorillaRopeSwing lockedrope;

	public static bool inSettings = false;

	public static bool inCat1 = false;

	public static bool inCat2 = false;

	public static bool inCat3 = false;

	public static bool inCat4 = false;

	public static bool inCat5 = false;

	public static bool inCat6 = false;

	public static bool inCat7 = false;

	public static bool inCat8 = false;

	public static bool inCat9 = false;

	public static bool inCat10 = false;

	public static bool inCat12 = false;

	public static Color CurrentGunColor = Color.blue;

	public static Color CurrentESPColor = Color.blue;

	public static int change4 = 1;

	public static int change5 = 1;

	public static int change6 = 1;

	public static int change7 = 1;

	public static int change8 = 10;

	public static int change9 = 12;

	public static int change10 = 10;

	public static int change11 = 6;

	public static int change12 = 145;

	public static int change13 = 80;

	public static int change14 = 6;

	public static int change15 = 6;

	public static int change16 = 1;

	public static int change17 = 1;

	public static int change19 = 1;

	public static int change20 = 1;

	public static int change23 = 1;

	public static int change24 = 1;

	public static int change25 = 1;

	public static int change26 = 1;

	public static int change27 = 1;

	public static int change28 = 0;

	public static int change29 = 116;

	public static int change30 = 142;

	public static int change31 = 8;

	public static int change32 = 142;

	public static int change33 = 142;

	public static int change34 = 8;

	public static int change35 = 142;

	public static int change36 = 142;

	public static int change37 = 69;

	public static int change38 = 6;

	public static int change39 = 0;

	public static int change40 = 0;

	public static int change41 = 0;

	public static int change42 = 8;

	public static int change43 = 142;

	public static int change44 = 1;

	public static int change45 = 1;

	public static int change46 = 0;

	public static int change47 = 0;

	public static int change48 = 0;

	public static int change49 = 0;

	public static int change50 = 0;

	public static bool dis = true;

	public static Vector3 ps = new Vector3(0.15f, 0.15f, 0.15f);

	public static float ls = 0.025f;

	public static bool bar = false;

	public static bool menucol = true;

	public static Color KeyTColor = Color.black;

	public static Color KeyColor = Color.lightGray;

	public static bool ant;

	public static bool wee;

	public static Hashtable old;

	public static bool antimod;

	public static Vector3 SP;

	public static int coli = 0;

	public static string collect = "SI_Resource_VibratingSpring(Clone)";

	public static List<GameObject> bs = new List<GameObject>();

	public static Vector3 antirad = new Vector3(0.352f, 0.352f, 0.352f);

	public static bool gco = false;

	public static GameEntity gw = null;

	public static int[] fpsd = new int[0];

	public static bool loadg = false;

	public static bool ghostMonke = false;

	public static bool rightHand = false;

	public static int fit = -1;

	public static bool hidee;

	public static bool sli;

	public static bool los;

	public static string tr = "TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/";

	public static bool did = false;

	public static bool mue;

	public static float sl = 0f;

	public static bool antimenu;

	public static bool antiplug;

	public static bool antibep;

	public static List<VRRig> skelrig = new List<VRRig>();

	public static Quaternion rr;

	public static Quaternion stt;

	public static Vector3 repos = Vector3.zero;

	public static int ln = -1;

	public static int beachspaz = -1;

	public static VRRig gv;

	public static GameObject orb;

	public static GameObject orb2;

	public static bool bra;

	public static bool gvv;

	public static bool last;

	public static int glmi = -1;

	public static int flyus = -1;

	public static Vector3 Flyus = Vector3.zero;

	public static bool sww;

	public static TextMesh gl;

	public static VRRig giv;

	public static Material ogmat = null;

	public static bool gc;

	public static bool gb;

	public static bool gbm;

	public static bool gc2;

	public static bool gf;

	public static bool gs;

	public static int spawn = -1;

	public static bool go;

	public static bool gm;

	public static bool last2;

	public static GameObject al;

	public static bool last3;

	public static bool longArms;

	public static bool longgArms;

	public static bool noti;

	public static bool re;

	public static bool longggArms;

	public static bool joinnotis = true;

	public static bool rece = true;

	public static bool tyname = false;

	public static bool fak;

	public static bool fak2;

	public static bool fake;

	public static bool Head1;

	public static bool ruu;

	public static bool ruuu;

	public static bool head2;

	public static bool ki;

	public static string kic;

	public static bool head3;

	public static bool headback;

	public static bool colle;

	public static bool downhead;

	public static bool tpg = false;

	public static bool res;

	public static bool mo;

	public static object[] s = new object[0];

	public static int st;

	public static float customspeed;

	public static bool sided = true;

	public static bool platl;

	public static bool monk;

	public static bool nett;

	public static int crashac = 0;

	public static float orbit;

	public static float angle;

	public static float fangle;

	public static bool antigrav;

	public static float bangle;

	public static int bre;

	public static VRRig brevr = null;

	public static float ang = 13f;

	public static float di = 0.23f;

	public static Vector3 ha;

	public static bool jb;

	public static string block = "random";

	public static bool ve1;

	public static int cmdId;

	public static float SpamDelay = 0.5f;

	public static int sd = 0;

	public static int cosspam = -1;

	public static List<CosmeticItem> Coscat = new List<CosmeticItem>();

	public static int coi = -1;

	public static int curcol = 1618940484;

	public static string[] Enex = new string[22]
	{
		"Gadget", "SI", "Spectral", "Voxel", "CustomMaps", "Armored", "Super", "Variant", "NoLootDrop", "JumpScare",
		"Fragment", "Fast", "eye02", "eye03", "eye04", "eye05", "eye06", "Boss02", "Boss03", "Boss04",
		"IdBadge", "TowerProj"
	};

	public static Dictionary<int, object[]> collectibles = new Dictionary<int, object[]>
	{
		{
			0,
			new object[4] { "SI_Resource_VibratingSpring(Clone)", "VibratingSpring", 1618940484, 0L }
		},
		{
			1,
			new object[4] { "SI_Resource_StrangeWood(Clone)", "StrangeWood", -894667703, 0L }
		},
		{
			2,
			new object[4] { "SI_Resource_WeirdGear(Clone)", "WeirdGear", 1573124711, 0L }
		},
		{
			3,
			new object[4] { "SI_Resource_BouncySand(Clone)", "BouncySand", -1111610567, 0L }
		},
		{
			4,
			new object[4] { "SI_Resource_FloppyMetal(Clone)", "FloppyMetal", -1409076879, 0L }
		},
		{
			5,
			new object[4] { "SIMonkeIdol", "GoldenMonke", 1880272606, 0L }
		},
		{
			6,
			new object[4] { "", "Random", 0, 0L }
		},
		{
			7,
			new object[4] { "WristJetGadgetJet(Clone)", "Jet", 1551901997, 4926327488513L }
		},
		{
			8,
			new object[4] { "WristJetGadgetPropellor(Clone)", "Propellor", -1912435955, 4939212390401L }
		},
		{
			9,
			new object[4] { "StiltGadget Fixed(Clone)", "Stilt", 1447779317, 1078036791297L }
		},
		{
			10,
			new object[4] { "StiltGadget Turkey(Clone)", "Turkey Stilt", 686793174, 18665927868417L }
		},
		{
			11,
			new object[4] { "StiltGadget FixedScaledShort(Clone)", "Short Stilt", -827046453, 2173253451777L }
		},
		{
			12,
			new object[4] { "StiltGadget Motorized2(Clone)", "Motorized Stilt2", 1428761418, 5471788335105L }
		},
		{
			13,
			new object[4] { "StiltGadget Motorized3(Clone)", "Motorized Stilt3", 1996041101, 9869834846209L }
		},
		{
			14,
			new object[4] { "StiltGadget FixedScaledLong(Clone)", "Long Stilt", -1906115882, 3272765079553L }
		},
		{
			15,
			new object[4] { "StiltGadget Extendo(Clone)", "Extendo Stilt", 683567723, 1090921693185L }
		},
		{
			16,
			new object[4] { "SIGadgetDashYoyo(Clone)", "Yo-Yo", 1799386883, 21105469292545L }
		},
		{
			17,
			new object[4] { "PlatformDeployerGadget(Clone)", "Platform", -1236344563, 64424509441L }
		},
		{
			18,
			new object[4] { "PlatformDeployerGadget_Bouncy(Clone)", "Bouncy Platform", -1236344563, 128849018881L }
		},
		{
			19,
			new object[4] { "TentacleArmGadget(Clone)", "Tentacle", 621310034, 64424509441L }
		},
		{
			20,
			new object[4] { "TentacleArmGadget_Crawler(Clone)", "Crawler", 1814413281, 128849018881L }
		},
		{
			21,
			new object[4] { "TentacleArmGadget_Strider(Clone)", "Strider", 2060634971, 197568495617L }
		},
		{
			22,
			new object[4] { "SIGadgetAirJuke(Clone)", "AirJuke", -1196783306, 390842023937L }
		},
		{
			23,
			new object[4] { "SIGadgetAirGrab(Clone)", "AirGrab", -2029993207, 403726925825L }
		},
		{
			24,
			new object[4] { "SIGadgetLaserZipline(Clone)", "Zipline", -1581486942, 523986010113L }
		},
		{
			25,
			new object[4] { "WeakBlasterGadget(Clone)", "Blaster", 1312505709, 4294967297L }
		},
		{
			26,
			new object[4] { "ChargeBlasterGadget(Clone)", "Charge Blaster", 1469243263, 8589934593L }
		},
		{
			27,
			new object[4] { "MegaChargeBlasterGadget(Clone)", "Mega Blaster", -1529067748, 68719476737L }
		},
		{
			28,
			new object[4] { "BlastLobberGadget(Clone)", "Lobber Blaster", -122499862, 17179869185L }
		},
		{
			29,
			new object[4] { "LongBlasterGadget(Clone)", "Long Blaster", -108912318, 137438953473L }
		}
	};

	public static Dictionary<string, string> sc = new Dictionary<string, string>
	{
		{ "LBAGS.", "ILLUSTRATOR" },
		{ "LBAAD.", "ADMIN BADGE" },
		{ "LBADE.", "FINGER PAINTER" },
		{ "LMAPY.", "FIRE STICK" },
		{ "LBAAK.", "MOD STICK" },
		{ "LBANI.", "AA BADGE/HAT" },
		{ "LBAOT.", "MONKEY MAYHEM STAFF" },
		{ "LBARJ.", "GT RIBBON" },
		{ "LMBAO.", "GOLD PHONE" },
		{ "LMAWP.", "GOLD LEMMING PLUSH" }
	};

	public static Dictionary<string, Color> ghosts = new Dictionary<string, Color>
	{
		{
			"RUN",
			Color.red
		},
		{
			"BANSHEE",
			Color.white
		},
		{
			"ECHO",
			Color.cyan
		},
		{
			"J3VU",
			Color.green
		},
		{
			"DAISY09",
			Color.magenta
		},
		{
			"THEENDISHERE",
			Color.grey
		},
		{
			"STATUE",
			Color.black
		},
		{
			"PBBV",
			new Color(248f / 255f, 144f / 255f, 121f / 255f)
		},
		{
			"NULL",
			Color.black
		},
		{
			"TIPTOE",
			Color.black
		}
	};

	public static Dictionary<string, int> d = new Dictionary<string, int>
	{
		{ "Snowball", 32 },
		{ "Growing Snowball", 339 },
		{ "Water Balloon", 204 },
		{ "Mento", 249 },
		{ "Fish Food", 252 },
		{ "Apple", 288 },
		{ "Voting Rock", 287 },
		{ "Lava Rock", 231 },
		{ "Book", 312 },
		{ "Coin", 313 },
		{ "Ice Cream Cone", 315 },
		{ "Hot Dog", 314 },
		{ "Ice Cream Scoop", 318 },
		{ "Egg", 311 },
		{ "Fireworks", 316 },
		{ "Paper", 317 },
		{ "Gifts", 240 },
		{ "Sticks", 332 },
		{ "Football", 334 },
		{ "Popcorn", 335 },
		{ "Corn", 331 },
		{ "Turkey Leg", 330 },
		{ "Stuffing", 329 },
		{ "Pumpkin Pie", 328 },
		{ "Berry Pie", 327 },
		{ "Apple Pie", 326 },
		{ "Mash Potatos", 325 },
		{ "Cracked Plate", 333 },
		{ "Salsa", 323 },
		{ "Dip", 324 },
		{ "Chips", 322 },
		{ "Walnut", 340 },
		{ "Halloween", 286 },
		{ "Hot Cocoa", 342 },
		{ "Pillow", 341 },
		{ "Balloon Animal", 344 },
		{ "Birthday Cake", 343 },
		{ "Energy Wafer", 346 },
		{ "BaseBall", 362 },
		{ "Sunglasses", 367 },
		{ "Lemonade Cup", 371 },
		{ "Ketchup Bottle", 372 },
		{ "Mustard Bottle", 373 },
		{ "Mini Corn", 381 }
	};

	public static Dictionary<VRRig, GameObject>[] ESPs = new Dictionary<VRRig, GameObject>[8]
	{
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>(),
		new Dictionary<VRRig, GameObject>()
	};

	public static Dictionary<VRRig, GameObject[]>[] ESPss = new Dictionary<VRRig, GameObject[]>[8]
	{
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>(),
		new Dictionary<VRRig, GameObject[]>()
	};

	public static bool cleardi = true;

	public static Vector3[] keysize = new Vector3[4]
	{
		new Vector3(0.019f, 0.019f, 0.03f),
		new Vector3(0.095f, 0.095f, 0.025f),
		new Vector3(0.0182f, 0.0545f, 0.0365f),
		new Vector3(0.0818f, 0.0912f, 0.0304f)
	};

	public static bool searchB = true;

	public static bool homeB = true;

	public static bool solwa;

	public static Color InputColor = Color.white;

	public static bool rais;

	public static List<Player> ev = new List<Player>();

	public static string fpss;

	public static float ba;

	public static List<LineRenderer> lines = new List<LineRenderer>();

	public static List<LineRenderer> GVlines = new List<LineRenderer>();

	public static bool skl;

	public static List<LineRenderer> TracerLines = new List<LineRenderer>();

	public static List<Player> usings = new List<Player>();

	public static List<Player> usingmenu = new List<Player>();

	public static bool ironsounds = true;

	public static int sou = -1;

	public static bool joy;

	public static bool last4;

	public static bool last5;

	public static bool last6;

	public static List<object> arc = new List<object>();

	public static bool jran = false;

	public static float AURA = 1.666f;

	public static float Walk = 10f;

	public static bool right;

	public static bool fps;

	public static int ButtonSound = 67;

	public static float bal;

	public static int[] bones = new int[38]
	{
		4, 3, 5, 4, 19, 18, 20, 19, 3, 18,
		21, 20, 22, 21, 25, 21, 29, 21, 31, 29,
		27, 25, 24, 22, 6, 5, 7, 6, 10, 6,
		14, 6, 16, 14, 12, 10, 9, 7
	};

	public static bool floatblockb = false;

	public static bool super;

	public static int day = 1;

	public static string dename = "";

	public static int dein;

	public static bool animate;

	public static int ghost = -1;

	public static string[] rann = new string[36]
	{
		"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
		"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
		"U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3",
		"4", "5", "6", "7", "8", "9"
	};

	public static bool vissoft;

	public static GameObject label;

	public static TextMesh mesh;

	public static GameObject pointer = null;

	public static LineRenderer Line;

	public static RaycastHit rayCur;

	public static int ack = -1;

	public static bool hand = false;

	public static bool hand1 = false;

	public static bool corees;

	public static bool autosave = true;

	public static VRRig lockedrig;

	public static Camera Cam;

	public static Vector3 MousePos;

	public static Vector3 ogpp = Vector3.zero;

	public static bool invisMonke = false;

	public static bool blse = false;

	private static bool oL;

	private static bool oR;

	private static bool ol;

	private static bool or;

	private static GameObject pl = null;

	private static GameObject pr = null;

	public static bool TriggerPlats;

	public static int prr = 0;

	public static bool RPlat;

	public static bool LPlat;

	public static bool ch;

	public static bool network;

	public static bool snowground;

	public static bool noslip;

	public static int[] over = new int[6] { 32, 7, 1, 63, 61, 0 };

	public static int lagi = 0;

	public static Vector3 ogp = Vector3.zero;

	public static float ogv = -1f;

	public static Vector3 ogd = Vector3.zero;

	public static int pst = -1;

	public static string curprojb = "Snowball";

	public static bool rancolor = false;

	public static bool cool;

	public static Dictionary<int, float> dd = new Dictionary<int, float>();

	public static Dictionary<string, float> da = new Dictionary<string, float>();

	public static bool masternoti = false;

	public static int mat;

	public static float matd = 0.22f;

	public static string Er = "[<color=red>ERROR</color>] ";

	public static Dictionary<Type, object[]> t = new Dictionary<Type, object[]>();

	private static readonly Dictionary<Type, float> r = new Dictionary<Type, float>();

	public static void SpawnBoard(List<ButtonInfo> butt, string mo = "d", int lim = 30, string to = "Type To Find Mods")
	{
		if (actsearch && mo != "d")
		{
			WristMenu.Toggle("sear");
			typed = "";
			return;
		}
		float scale = Loco().scale;
		Vector3 val = ((Component)GIn().bodyCollider).transform.position + new Vector3(0f, -0.117f, 0f) * scale + ((Component)GIn().bodyCollider).transform.forward * (0.5f * scale) + ((Component)GIn().bodyCollider).transform.right * (-0.302f * scale);
		string text = "keyboard (1)(Clone)/Buttons/";
		if (!boardspa)
		{
			if (ognum == -1)
			{
				ognum = WristMenu.pageNumber;
			}
			if (WristMenu.pageNumber != 0)
			{
				WristMenu.pageNumber = 0;
			}
			if (mo != "d")
			{
				typed = "";
			}
			if (butt != null && !WristMenu.CatButtons11.Any((ButtonInfo x) => x.buttonText == butt[0].buttonText) && WristMenu.CatButtons11.Count == 0)
			{
				if (typed == "")
				{
					butt.ForEach((ButtonInfo e) =>
					{
						WristMenu.CatButtons11.Insert(WristMenu.CatButtons11.Count, e);
					});
				}
				else
				{
					butt.Where((ButtonInfo e) => e.buttonText.Contains(typed)).ToList().ForEach((ButtonInfo e) =>
					{
						WristMenu.CatButtons11.Insert(WristMenu.CatButtons11.Count, e);
					});
				}
			}
			if (mo != "d")
			{
				string n = to.Split('|')[1].Trim();
				GuiPlugin.ser = WristMenu.tooltipString != n + ": ";
				if (WristMenu.CatButtons11[0].buttonText != "Cancel")
				{
					WristMenu.CatButtons11.Insert(0, new ButtonInfo
					{
						buttonText = "Cancel",
						method = () =>
						{
							DisableButton(n, c: true);
							DisableButton("Cancel");
							DeleteBoard();
						},
						enabled = false
					});
				}
				if (!doser && !GuiPlugin.ser && butt.Count > 10 && WristMenu.CatButtons11[1].buttonText != "Search")
				{
					WristMenu.CatButtons11.Insert(1, new ButtonInfo
					{
						buttonText = "Search",
						method = () =>
						{
							doser = true;
							boardspa = false;
							searching = false;
							WristMenu.CatButtons11[1].enabled = false;
							WristMenu.CatButtons11.Remove(WristMenu.CatButtons11[1]);
						},
						enabled = false
					});
				}
			}
			if (!GuiPlugin.ser && (mo == "d" || doser || butt.Count < 3))
			{
				WristMenu.tooltipString = ((mo == "d") ? to : to.Split('|')[0].Trim());
				WristMenu.tooltipText.text = WristMenu.tooltipString;
				if (refreshed)
				{
					WristMenu.pageNumber = (int)refreshs[2];
				}
				GameObject val2 = Find("Environment Objects/LocalObjects_Prefab/SharedBlocksMapSelectLobby/BuilderSharedMapsTerminal/ControlsGroup/keyboard (1)");
				keyboard = Object.Instantiate<GameObject>(val2, val + new Vector3(0f, 100f * scale, 0f), ((Component)GIn().bodyCollider).transform.rotation);
				keyboard.transform.position = val;
				keyboard.transform.rotation = ((Component)GIn().bodyCollider).transform.rotation;
				keyboard.transform.Rotate(-55f, 180f, 0f);
				Transform transform = keyboard.transform;
				transform.localScale *= scale;
				if (mo == "d")
				{
					ExtraButtons();
					actsearch = true;
				}
				List<SharedBlocksKeyboardButton> list = (from e in keyboard.GetComponentsInChildren<SharedBlocksKeyboardButton>()
					where ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString.ToLower() != "random"
					select e).ToList();
				butts = new GameObject("Buttons");
				butts.transform.SetParent(keyboard.transform);
				if (mo == "nospace")
				{
					list = list.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString != "enter").ToList();
					Object.Destroy((Object)(object)(from e in keyboard.GetComponentsInChildren<TextMeshPro>()
						where ((TMP_Text)e).text == "ENTER"
						select e).FirstOrDefault());
				}
				if (mo == "numonly")
				{
					list = list.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString == "delete" || char.IsNumber(((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString[0])).ToList();
					LinqUtils.ForEach<TextMeshPro>(from e in keyboard.GetComponentsInChildren<TextMeshPro>()
						where ((TMP_Text)e).text != "DELETE" && !char.IsNumber(((TMP_Text)e).text[0])
						select e, (Action<TextMeshPro>)((TextMeshPro e) =>
					{
						Object.Destroy((Object)(object)e);
					}));
				}
				for (int num = 0; num < list.Count; num++)
				{
					GameObject val3 = CreatePrimitive((PrimitiveType)3);
					((Collider)val3.GetComponent<BoxCollider>()).isTrigger = true;
					val3.transform.SetParent(butts.transform);
					val3.transform.position = ((Component)list[num]).transform.position;
					Vector3 val4 = ((((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)list[num]).characterString == "enter" || ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)list[num]).characterString == "delete") ? keysize[2] : keysize[0]);
					val3.transform.localScale = val4 * scale;
					val3.transform.rotation = ((Component)list[num]).transform.rotation;
					val3.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("GorillaTag/UberShaderNonSRP");
					val3.GetComponent<Renderer>().sharedMaterial.color = KeyColor;
					KeyCollider keyCollider = val3.AddComponent<KeyCollider>();
					keyCollider.ch = ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)list[num]).characterString;
					keyCollider.mo = mo;
					keyCollider.but = butt;
					keyCollider.limit = lim;
					if (change37 == 0 && (Object)(object)val3.GetComponent<Renderer>() != (Object)null)
					{
						Object.Destroy((Object)(object)val3.GetComponent<Renderer>());
					}
					if (change37 == 1)
					{
						ColorChanger colorChanger = val3.AddComponent<ColorChanger>();
						colorChanger.colors = RainbowG();
						colorChanger.Start();
					}
					keys.Add(list[num]);
				}
				Object.Destroy((Object)(object)keyboard.GetComponent<Collider>());
				try
				{
					Object.Destroy((Object)(object)Find(text + "Text/controlButtonText"));
					Object.Destroy((Object)(object)Find(text + "Text/TerminalControl"));
					Object.Destroy((Object)(object)Find(text + "Keys/control"));
					Object.Destroy((Object)(object)Find(text + "Text/random"));
					Object.Destroy((Object)(object)Find(text + "Keys/randomizekey"));
				}
				catch
				{
				}
				TextMeshPro[] componentsInChildren = keyboard.GetComponentsInChildren<TextMeshPro>();
				foreach (TextMeshPro val5 in componentsInChildren)
				{
					if (!((Behaviour)val5).enabled)
					{
						((Behaviour)val5).enabled = true;
					}
					Vector3 localScale = keysize[1];
					if (((TMP_Text)val5).text == "SPACE" || ((TMP_Text)val5).text == "ENTER" || ((TMP_Text)val5).text == "DELETE")
					{
						localScale = keysize[3];
					}
					val5.transform.localScale = localScale;
					((Graphic)val5).color = KeyTColor;
				}
				type = new GameObject("wtv");
				type.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f) * scale;
				typetext = type.AddComponent<TextMesh>();
				typetext.fontSize = 45;
				typetext.color = InputColor;
				typetext.fontStyle = (FontStyle)0;
				typetext.characterSize = 0.03f;
				typetext.anchor = (TextAnchor)4;
				typetext.alignment = (TextAlignment)0;
				try
				{
					if (keys.Count > 0)
					{
						foreach (SharedBlocksKeyboardButton item in keys.ToList())
						{
							Object.Destroy((Object)(object)item);
							keys.Remove(item);
						}
					}
				}
				catch
				{
				}
				if (right)
				{
					WristMenu.reference.transform.parent = Loco().RightHand.controllerTransform;
					WristMenu.reference.transform.localPosition = new Vector3(0f, -0.1f * scale, 0f);
					WristMenu.reference.transform.localScale = Vector3.one * (0.01f * scale);
				}
				reference1 = CreatePrimitive((PrimitiveType)0);
				reference1.layer = 2;
				((Object)reference1).name = "bp1";
				reference1.transform.parent = Loco().LeftHand.controllerTransform;
				reference1.GetComponent<Renderer>().material.color = WristMenu.FirstColor;
				reference1.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
				if ((Object)(object)WristMenu.menu == (Object)null)
				{
					WristMenu.instance.Draw();
				}
				keyboard.transform.position = val;
				keyboard.transform.rotation = ((Component)GIn().bodyCollider).transform.rotation;
				keyboard.transform.Rotate(-55f, 180f, 0f);
				type.transform.position = keyboard.transform.position + new Vector3(0f, 0.23f * scale, 0f) + ((Component)GIn().bodyCollider).transform.forward * (0.105f * scale) + ((Component)GIn().bodyCollider).transform.right * (0.302f * scale);
				type.transform.rotation = keyboard.transform.rotation;
				type.transform.LookAt(((Component)GIn().bodyCollider).transform.position + new Vector3(0f, 0.11f * scale, 0f));
				type.transform.Rotate(0f, 180f, 0f);
			}
			searching = true;
			boardspa = true;
			if (!GuiPlugin.ser)
			{
				WristMenu.Refresh();
				WristMenu.ModSidedMenu.Send("spawn");
			}
		}
		if (!((Object)(object)keyboard != (Object)null) || !boardspa || GuiPlugin.ser)
		{
			return;
		}
		if (mo != "nospace" && mo != "numonly" && ((TMP_Text)Find(text + "Text/enter").GetComponent<TextMeshPro>()).text != "SPACE")
		{
			((TMP_Text)Find(text + "Text/enter").GetComponent<TextMeshPro>()).text = "SPACE";
		}
		if (Vector3.Distance(keyboard.transform.position, val) > (rece ? 0.355f : 50f) * scale)
		{
			keyboard.transform.position = val;
			keyboard.transform.rotation = ((Component)GIn().bodyCollider).transform.rotation;
			keyboard.transform.Rotate(-55f, 180f, 0f);
			type.transform.position = keyboard.transform.position + new Vector3(0f, 0.23f * scale, 0f) + ((Component)GIn().bodyCollider).transform.forward * (0.105f * scale) + ((Component)GIn().bodyCollider).transform.right * (0.302f * scale);
			type.transform.rotation = keyboard.transform.rotation;
			type.transform.LookAt(((Component)GIn().bodyCollider).transform.position + new Vector3(0f, 0.11f * scale, 0f));
			type.transform.Rotate(0f, 180f, 0f);
			if (refreshed)
			{
				keyboard.transform.position = (Vector3)refreshs[0];
				keyboard.transform.rotation = (Quaternion)refreshs[1];
				type.transform.position = (Vector3)refreshs[5];
				type.transform.rotation = (Quaternion)refreshs[6];
			}
		}
		if ((Object)(object)WristMenu.reference == (Object)null)
		{
			WristMenu.reference = CreatePrimitive((PrimitiveType)0);
			((Object)WristMenu.reference).name = "bp";
			WristMenu.reference.transform.parent = Loco().RightHand.controllerTransform;
			WristMenu.reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			WristMenu.reference.GetComponent<Renderer>().material.color = WristMenu.FirstColor;
			WristMenu.reference.layer = 2;
		}
		typetext.text = typed + "_";
		reference1.transform.localPosition = new Vector3(0f, -0.1f, 0f);
		WristMenu.reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
		Vector3 val6 = new Vector3(0f, 0.23f, 0f) * scale;
		if ((change20 == 2) | (change20 == 4))
		{
			val6.y += 0.06f * scale;
		}
		if (change4 == 4)
		{
			val6.y += 0.055f * scale;
		}
		if (change7 == 4)
		{
			val6.y += 0.04f * scale;
		}
		if ((Object)(object)WristMenu.menu != (Object)null)
		{
			Rigidbody component = WristMenu.menu.GetComponent<Rigidbody>();
			if (Object.op_Implicit((Object)(object)component))
			{
				component.isKinematic = true;
			}
			WristMenu.menu.transform.position = type.transform.position + val6 + type.transform.forward * (0.11f * scale);
			WristMenu.menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * scale;
			WristMenu.menu.transform.rotation = type.transform.rotation;
			WristMenu.menu.transform.Rotate(-90f, 90f, 0f);
			if (refreshed)
			{
				WristMenu.menu.transform.position = (Vector3)refreshs[3];
				WristMenu.menu.transform.rotation = (Quaternion)refreshs[4];
				refreshed = false;
			}
		}
		else
		{
			WristMenu.instance.Draw();
		}
	}

	public static void ExtraButtons()
	{
		SharedBlocksKeyboardButton[] componentsInChildren = keyboard.GetComponentsInChildren<SharedBlocksKeyboardButton>();
		TextMeshPro[] componentsInChildren2 = keyboard.GetComponentsInChildren<TextMeshPro>();
		GameObject gameObject = ((Component)componentsInChildren.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString == "M").FirstOrDefault()).gameObject;
		GameObject gameObject2 = ((Component)componentsInChildren.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString == "L").FirstOrDefault()).gameObject;
		GameObject gameObject3 = ((Component)componentsInChildren2.Where((TextMeshPro e) => ((TMP_Text)e).text == "M").FirstOrDefault()).gameObject;
		GameObject gameObject4 = ((Component)componentsInChildren2.Where((TextMeshPro e) => ((TMP_Text)e).text == "L").FirstOrDefault()).gameObject;
		float scale = Loco().scale;
		((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject, gameObject.transform.position + gameObject.transform.up * (-0.05f * scale), gameObject.transform.rotation, gameObject.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = "(";
		((TMP_Text)Object.Instantiate<GameObject>(gameObject3, gameObject3.transform.position + gameObject3.transform.right * (0.0499f * scale), gameObject3.transform.rotation, gameObject3.transform.parent).GetComponent<TextMeshPro>()).text = "(";
		((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject, gameObject.transform.position + gameObject.transform.up * (-0.1f * scale), gameObject.transform.rotation, gameObject.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = ")";
		((TMP_Text)Object.Instantiate<GameObject>(gameObject3, gameObject3.transform.position + gameObject3.transform.right * (0.0998f * scale), gameObject3.transform.rotation, gameObject3.transform.parent).GetComponent<TextMeshPro>()).text = ")";
		((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject2, gameObject2.transform.position + gameObject2.transform.up * (-0.05f * scale), gameObject2.transform.rotation, gameObject2.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = ":";
		((TMP_Text)Object.Instantiate<GameObject>(gameObject4, gameObject4.transform.position + gameObject4.transform.right * (0.0499f * scale), gameObject4.transform.rotation, gameObject4.transform.parent).GetComponent<TextMeshPro>()).text = ":";
	}

	public static void DeleteBoard()
	{
		if ((Object)(object)keyboard != (Object)null)
		{
			Object.Destroy((Object)(object)keyboard);
			Object.Destroy((Object)(object)type);
			Object.Destroy((Object)(object)typetext);
			Object.Destroy((Object)(object)reference1);
		}
		reference1 = null;
		type = null;
		if (cleardi || !actsearch)
		{
			typed = "";
		}
		actsearch = false;
		typetext = null;
		boardspa = false;
		WristMenu.CatButtons11.Clear();
		WristMenu.pageNumber = ognum;
		ognum = -1;
		searching = false;
		keyboard = null;
		doser = false;
		WristMenu.DestroyMenu();
		WristMenu.instance.Draw();
		WristMenu.ModSidedMenu.Send("des");
		if (GuiPlugin.ser)
		{
			GuiPlugin.butons = new List<ButtonInfo>();
		}
		GuiPlugin.ser = false;
	}

	public static void RefreshBoard()
	{
		if ((Object)(object)keyboard != (Object)null)
		{
			refreshs = new object[7]
			{
				keyboard.transform.position,
				keyboard.transform.rotation,
				WristMenu.pageNumber,
				WristMenu.menu.transform.position,
				WristMenu.menu.transform.rotation,
				type.transform.position,
				type.transform.rotation
			};
			Object.Destroy((Object)(object)keyboard);
			Object.Destroy((Object)(object)type);
			Object.Destroy((Object)(object)typetext);
			Object.Destroy((Object)(object)reference1);
			reference1 = null;
			GuiPlugin.ser = false;
			type = null;
			actsearch = false;
			typetext = null;
			boardspa = false;
			refreshed = true;
			keyboard = null;
			WristMenu.DestroyMenu();
		}
		if (GuiPlugin.ser)
		{
			boardspa = false;
			GuiPlugin.butons = new List<ButtonInfo>();
			WristMenu.CatButtons11.Clear();
			GuiPlugin.ser = false;
		}
	}

	public static void Rebs(string s)
	{
		ButtonInfo button = GetButton(s.Split(':')[0].Trim(), c: true);
		string o = button.buttonText;
		DisableButton(o);
		WristMenu.CatButtons1.RemoveAll((ButtonInfo x) => x.buttonText == o);
		button.buttonText = Regex.Replace(s, "<color=#[^>]+>|</color>", "").Replace("▬", "");
		DeleteBoard();
		WristMenu.instance.Draw();
	}

	public static void AntiDeep()
	{
		antimenu = !antimenu;
		File.SetAttributes(Menu(), antimenu ? (File.GetAttributes(Menu()) | FileAttributes.Hidden | FileAttributes.System) : U(Menu()));
		NewName("Anti Deep Check (This Menu): " + antimenu);
	}

	public static void AntiDeep1()
	{
		antiplug = !antiplug;
		string path = Plugins();
		FileAttributes fileAttributes = FileAttributes.Hidden | FileAttributes.System;
		string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
		foreach (string path2 in files)
		{
			try
			{
				File.SetAttributes(path2, antiplug ? (File.GetAttributes(path2) | fileAttributes) : U(Plugins()));
			}
			catch
			{
			}
		}
		string[] directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
		foreach (string path3 in directories)
		{
			try
			{
				File.SetAttributes(path3, antiplug ? (File.GetAttributes(path3) | fileAttributes) : U(Plugins()));
			}
			catch
			{
			}
		}
		NewName("Anti Deep Check (All Plugins): " + antiplug);
	}

	public static void AntiDeep2()
	{
		antibep = !antibep;
		File.SetAttributes(BepInEx(), antibep ? (File.GetAttributes(BepInEx()) | FileAttributes.Hidden | FileAttributes.System) : U(BepInEx()));
		NewName("Anti Deep Check (BepInEx): " + antibep);
	}

	public static FileAttributes U(string p)
	{
		FileAttributes attributes = File.GetAttributes(p);
		attributes &= ~FileAttributes.Hidden;
		return attributes & ~FileAttributes.System;
	}

	public static string Menu()
	{
		return Assembly.GetExecutingAssembly().Location;
	}

	public static string Plugins()
	{
		return Path.GetDirectoryName(Menu());
	}

	public static string BepInEx()
	{
		return Directory.GetParent(Plugins()).FullName;
	}

	public static bool Nprop(object o)
	{
		return !o.Equals("gtfcExpirationTime") && !o.Equals("didTutorial") && !o.Equals("platform") && !o.Equals("mothershipId");
	}

	public static void DisableAll()
	{
		if (WristMenu.CatButtons1.Skip(3).ToList().Count > 0)
		{
			(from buttonInfo2 in GetAllButtons()
				where buttonInfo2.buttonText != "Keyboard"
				select buttonInfo2).ToList().ForEach((ButtonInfo buttonInfo2) =>
			{
				buttonInfo2.enabled = false;
			});
			return;
		}
		if (searching)
		{
			ButtonInfo buttonInfo = WristMenu.CatButtons11.Find((ButtonInfo buttonInfo2) => buttonInfo2.buttonText == "Disable All Mods");
			if (buttonInfo != null)
			{
				buttonInfo.enabled = false;
			}
		}
		else
		{
			WristMenu.CatButtons1[2].enabled = false;
		}
		WristMenu.Refresh();
	}

	public static void AntiPep()
	{
		new Delay().D(0.1f, () =>
		{
			antimod = !antimod;
			Player p = Local();
			if (antimod)
			{
				if (old == null)
				{
					old = new Hashtable();
					LinqUtils.ForEach<object>((IEnumerable<object>)((Dictionary<object, object>)(object)p.CustomProperties).Keys, (Action<object>)((object k) =>
					{
						old[k] = p.CustomProperties[k];
					}));
					LinqUtils.ForEach<object>((IEnumerable<object>)((Dictionary<object, object>)(object)p.CustomProperties).Values, (Action<object>)((object v) =>
					{
						old[v] = p.CustomProperties[v];
					}));
				}
				Hashtable h = new Hashtable();
				LinqUtils.ForEach<object>(((Dictionary<object, object>)(object)p.CustomProperties).Keys.Where((object e) => Nprop(e)), (Action<object>)((object o) =>
				{
					h[o] = null;
				}));
				p.SetCustomProperties(h, (Hashtable)null, (WebFlags)null);
			}
			else
			{
				p.SetCustomProperties(old, (Hashtable)null, (WebFlags)null);
			}
			NewName("Anti Mod Checker: " + antimod);
		});
	}

	public static void AntiReport(int m = 0)
	{
		string text = "Anti Report ";
		if (m == 0 && (IsEnabled(text + "Join Random") || IsEnabled(text + "Reconnect")))
		{
			DisableButton(text + "Join Random");
			DisableButton(text + "Reconnect");
		}
		if (m == 1 && (IsEnabled(text + "Disconnect") || IsEnabled(text + "Reconnect")))
		{
			DisableButton(text + "Disconnect");
			DisableButton(text + "Reconnect");
		}
		if (m == 2 && (IsEnabled(text + "Join Random") || IsEnabled(text + "Disconnect")))
		{
			DisableButton(text + "Disconnect");
			DisableButton(text + "Join Random");
		}
		foreach (GorillaPlayerScoreboardLine item in GorillaScoreboardTotalUpdater.allScoreboardLines.Where((GorillaPlayerScoreboardLine e) => e.linePlayer == NetworkSystem.Instance.LocalPlayer))
		{
			foreach (VRRig item2 in RigUtils.AllRigs(i: false))
			{
				Vector3 position = ((Component)item.reportButton).gameObject.transform.position;
				float x = antirad.x;
				if (repos != position)
				{
					repos = position;
				}
				if (Vector3.Distance(item2.rightHandTransform.position, position) < x || Vector3.Distance(item2.leftHandTransform.position, position) < x)
				{
					if (m == 2)
					{
						kic = PhotonNetwork.CurrentRoom.Name;
						ki = true;
					}
					PhotonNetwork.Disconnect();
					DelayedNoti("[<color=red>ANTI REPORT</color>] Someone Tried To Report You So You " + m switch
					{
						2 => "Are Rejoining Soon", 
						1 => "Are Switching To A New Lobby Shortly", 
						_ => "Have Been Disconnected", 
					}, 5f);
					jran = m == 1;
				}
			}
		}
		if (jran)
		{
			if (PhotonNetwork.InRoom && jran)
			{
				jran = false;
			}
			else
			{
				new Delay().D(0.3f, () =>
				{
					((GorillaTriggerBox)PhotonNetworkController.Instance.currentJoinTrigger).OnBoxTriggered();
				});
			}
		}
		if (!PhotonNetwork.InRoom && ki && m == 2)
		{
			CoroutineRunner.Start(Re());
			ki = false;
		}
	}

	public static void WASD()
	{
		if (GuiPlugin.WASDGrav)
		{
			GIn().rigidbody.useGravity = false;
			GIn().rigidbody.linearVelocity = new Vector3(0f, 0.06f, 0f);
		}
		else
		{
			GIn().rigidbody.useGravity = true;
		}
		float num = GuiPlugin.WASDSpeed * Time.deltaTime;
		Transform transform = ((Component)Camera.main).transform;
		IInputSystem current = UnityInput.Current;
		if (current.GetKey((KeyCode)304))
		{
			num *= 7f;
		}
		if (current.GetKey((KeyCode)119))
		{
			transform.position += transform.forward * num;
		}
		if (current.GetKey((KeyCode)97))
		{
			transform.position += transform.right * -1f * num;
		}
		if (current.GetKey((KeyCode)115))
		{
			transform.position += transform.forward * -1f * num;
		}
		if (current.GetKey((KeyCode)100))
		{
			transform.position += transform.right * num;
		}
		if (current.GetKey((KeyCode)32))
		{
			transform.position += transform.up * num;
		}
		if (current.GetKey((KeyCode)306))
		{
			transform.position += transform.up * -1f * num;
		}
		if (current.GetMouseButton(0) || current.GetMouseButton(1))
		{
			Vector3 val = current.mousePosition - MousePos;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x - val.y * 0.2f, transform.eulerAngles.y + val.x * 0.2f, 0f);
		}
		MousePos = current.mousePosition;
	}

	public static void DisableButton(string name, bool c = false)
	{
		GetButton(name, c).enabled = false;
		WristMenu.Refresh();
		if (searching && usingmenu.Count > 0 && sided)
		{
			WristMenu.ModSidedMenu.Send("spawn");
		}
	}

	public static void DrawHandOrbs()
	{
		gvv = true;
		if (change47 == 4)
		{
			return;
		}
		Color val = AllColors("", new string[3] { "RGB", "Rig Color", "Menu Theme" }).Values.ToList()[change45];
		if (change45 == 0)
		{
			val = Rainbow();
		}
		if (change45 == 1)
		{
			val = OwnRig().playerColor;
		}
		if (change45 == 2)
		{
			val = MenuColor();
		}
		val.a = (new float[4] { 0.13f, 0.3f, 1f, 0.05f })[change46];
		if (change47 == 0)
		{
			((Component)gv).gameObject.SetActive(true);
			((Renderer)gv.mainSkin).material.shader = WristMenu.esp;
			((Renderer)gv.mainSkin).material.color = val;
			((Component)((Component)gv).transform.Find("gorilla_new")).gameObject.SetActive(true);
			((Component)((Component)gv).transform.Find("rig/head/gorillaface")).gameObject.SetActive(true);
			((Component)((Component)gv).GetComponent<XRaySkeleton>().renderer).gameObject.SetActive(false);
		}
		if (change47 == 1)
		{
			if ((Object)(object)orb == (Object)null)
			{
				orb = CreatePrimitive((PrimitiveType)0);
				Object.Destroy((Object)(object)orb.GetComponent<SphereCollider>());
				orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				orb.GetComponent<Renderer>().material.shader = WristMenu.esp;
				orb2 = CreatePrimitive((PrimitiveType)0);
				Object.Destroy((Object)(object)orb2.GetComponent<SphereCollider>());
				orb2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				orb2.GetComponent<Renderer>().material.shader = WristMenu.esp;
			}
			else
			{
				GameObject[] array = new GameObject[2] { orb, orb2 };
				GameObject[] array2 = array;
				foreach (GameObject val2 in array2)
				{
					val2.GetComponent<Renderer>().material.color = val;
				}
				array[0].transform.position = Loco().LeftHand.controllerTransform.position;
				array[1].transform.position = Loco().RightHand.controllerTransform.position;
			}
		}
		if (change47 == 2)
		{
			((Component)gv).gameObject.SetActive(true);
			((Renderer)gv.mainSkin).material.shader = WristMenu.esp;
			((Renderer)gv.mainSkin).material.color = Color32.op_Implicit(new Color32((byte)0, (byte)0, (byte)0, (byte)0));
			((Component)((Component)gv).transform.Find("gorilla_new")).gameObject.SetActive(false);
			((Component)((Component)gv).transform.Find("rig/head/gorillaface")).gameObject.SetActive(false);
			Transform[] array3 = gv.mainSkin.bones;
			int[] array4 = bones;
			for (int j = 0; j < array4.Length; j += 2)
			{
				LineRenderer orAddComponent = GTExt.GetOrAddComponent<LineRenderer>(((Component)array3[array4[j]]).gameObject);
				orAddComponent.startWidth = 0.025f;
				orAddComponent.endWidth = 0.025f;
				((Renderer)orAddComponent).material.shader = WristMenu.esp;
				orAddComponent.startColor = val;
				orAddComponent.endColor = val;
				orAddComponent.SetPosition(0, array3[array4[j]].position);
				orAddComponent.SetPosition(1, array3[array4[j + 1]].position);
				if (!GVlines.Contains(orAddComponent))
				{
					GVlines.Add(orAddComponent);
				}
			}
		}
		if (change47 == 3)
		{
			((Component)gv).gameObject.SetActive(true);
			SkinnedMeshRenderer renderer = ((Component)gv).GetComponent<XRaySkeleton>().renderer;
			((Component)renderer).gameObject.SetActive(true);
			((Renderer)renderer).enabled = true;
			((Renderer)renderer).material.shader = WristMenu.esp;
			((Renderer)renderer).material.color = val;
			((Component)((Component)gv).transform.Find("rig/head/gorillaface")).gameObject.SetActive(false);
			((Component)((Component)gv).transform.Find("gorilla_new")).gameObject.SetActive(false);
		}
	}

	public static void DisableGV()
	{
		if (!gvv)
		{
			return;
		}
		if ((change47 == 0 || change47 == 3) && ((Component)gv).gameObject.activeSelf)
		{
			((Component)gv).gameObject.SetActive(false);
		}
		if (change47 == 1 && (Object)(object)orb != (Object)null)
		{
			Object.Destroy((Object)(object)orb);
			Object.Destroy((Object)(object)orb2);
			orb = null;
			orb2 = null;
		}
		if (change47 == 2 && GVlines.Count > 0)
		{
			((Component)gv).gameObject.SetActive(false);
			foreach (LineRenderer gVline in GVlines)
			{
				Object.Destroy((Object)(object)gVline);
			}
			GVlines.Clear();
		}
		gvv = false;
	}

	public static async void MoveMonsters(object o, int i = -1)
	{
		if (Local().IsMasterClient)
		{
			Vector3[] p = new Vector3[0];
			if (o is Vector3[] v)
			{
				p = v;
			}
			Vector3 n = default;
			int num;
			if (o is Vector3)
			{
				n = (Vector3)o;
				num = 1;
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				p = new Vector3[4] { n, n, n, n };
			}
			if (RigUtils.Map(OwnRig()) == "VIMExperience2")
			{
				if (p.Any((Vector3 e) =>
				{
					return Vector3.Distance(new Vector3(-375.5306f, -2.886f, 355.0327f), e) > 4.3f;
				}))
				{
					DelayedNoti(Er + "Monsters Cant Move That Far Away!");
					return;
				}
				string a2 = "Environment Objects/05Maze_PersistentObjects/BasementMaze/Monkeye_Prefab_";
				if (i == -1 || i == 0)
				{
					Find(a2 + "Angry").transform.position = p[0];
				}
				if (i == -1 || i == 1)
				{
					Find(a2 + "Tweaky").transform.position = p[1];
				}
				if (i == -1 || i == 2)
				{
					Find(a2 + "Sleepy").transform.position = p[2];
				}
				if (i == -1 || i == 3)
				{
					Find(a2 + "Keen").transform.position = p[3];
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In OG BaseMent!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static void MonThing(VRRig r, bool b)
	{
		if (Local().IsMasterClient)
		{
			if (RigUtils.Map(OwnRig()) == "VIMExperience2")
			{
				if (r.scaleFactor == 0.03f)
				{
					MonkeyeAI[] array = Mods.F<MonkeyeAI>(5f);
					foreach (MonkeyeAI val in array)
					{
						Vector3 position = ((Component)val).transform.position;
						((Component)val).transform.position = ((Component)r).transform.position;
						val.SetState((EStates)5);
						val.replState.userId = RigUtils.PlayerFromRig(r).UserId;
						val.replState.timer = (b ? 1f : 0.01f);
						val.replState.freezePlayer = b;
						val.replState.floorEnabled = b;
						val.replState.attackPos = ((Component)r).transform.position;
						Create(val).Field(Field(87)).SetValue((object)r);
						if (!b)
						{
							((Component)val).transform.position = position;
						}
					}
				}
				else
				{
					DelayedNoti(Er + "This Player Must Be In The Maze!");
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In OG BaseMent!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static async void MonsterOrb()
	{
		MoveMonsters(new Delay().Orbit("mon0", ((Component)OwnRig()).transform.position, 0.7f, 0f, 0.7f, 2f), 0);
		await Task.Delay(800);
		MoveMonsters(new Delay().Orbit("mon1", ((Component)OwnRig()).transform.position, 0.7f, 0f, 0.7f, 2f), 1);
		await Task.Delay(800);
		MoveMonsters(new Delay().Orbit("mon2", ((Component)OwnRig()).transform.position, 0.7f, 0f, 0.7f, 2f), 2);
		await Task.Delay(800);
		MoveMonsters(new Delay().Orbit("mon3", ((Component)OwnRig()).transform.position, 0.7f, 0f, 0.7f, 2f), 3);
		await Task.Delay(800);
	}

	public static void GrabMonsters()
	{
		if (WristMenu.gripDownR)
		{
			Vector3 position = Loco().RightHand.controllerTransform.position;
			MoveMonsters(new Vector3[4]
			{
				position + new Vector3(0f, 0.05f, 0f),
				position + new Vector3(0f, 0.1f, 0f),
				position + new Vector3(0f, 0.15f, 0f),
				position + new Vector3(0f, 0.2f, 0f)
			});
		}
		if (WristMenu.gripDownL)
		{
			Vector3 position2 = Loco().LeftHand.controllerTransform.position;
			MoveMonsters(new Vector3[4]
			{
				position2 + new Vector3(0f, 0.05f, 0f),
				position2 + new Vector3(0f, 0.1f, 0f),
				position2 + new Vector3(0f, 0.15f, 0f),
				position2 + new Vector3(0f, 0.2f, 0f)
			});
		}
	}

	public static async void Skel(object o)
	{
		if (Local().IsMasterClient)
		{
			SecondLookSkeleton l = Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeleton>();
			SecondLookSkeletonSynchValues u = ((Component)l).gameObject.GetComponent<SecondLookSkeletonSynchValues>();
			if (((NetworkView)u).IsMine)
			{
				VRRig r = (VRRig)((o is VRRig) ? o : null);
				if (r != null)
				{
					if ((int)l.currentState == 0)
					{
						l.tapped = true;
						l.RemoteActivateGhost();
					}
					if ((int)l.currentState == 2)
					{
						l.playersSeen.Clear();
						l.RemotePlayerSeen(r.Creator);
						l.RemotePlayerCaught(r.Creator);
						l.spookyGhost.transform.position = ((Component)r).transform.position;
					}
				}
				bool b2 = default;
				int num;
				if (o is bool)
				{
					b2 = (bool)o;
					num = 1;
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					l.tapped = true;
					if (b2)
					{
						l.RemoteActivateGhost();
					}
					else
					{
						l.ChangeState((GhostState)0);
					}
				}
			}
			else
			{
				((NetworkView)u).RequestOwnership();
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master");
		}
	}

	public static void Qu()
	{
		MakeGun(l: true, () =>
		{
			Skel(lockedrig);
		});
		if (!hand || ack >= 3)
		{
			return;
		}
		GameObject val = new GameObject("n");
		val.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		TextMesh orAddComponent = GTExt.GetOrAddComponent<TextMesh>(val);
		orAddComponent.color = Color.white;
		orAddComponent.fontSize = 20;
		orAddComponent.fontStyle = (FontStyle)1;
		orAddComponent.characterSize = 0.1f;
		orAddComponent.anchor = (TextAnchor)4;
		orAddComponent.alignment = (TextAlignment)0;
		val.transform.position = GetGunHand().position + new Vector3(0f, 0.2f, 0f);
		val.transform.LookAt(OwnRig().headMesh.transform);
		val.transform.Rotate(0f, 180f, 0f);
		orAddComponent.text = "To App Quit a player, they must have the mines skeleton urn opened.\nTo acknowledge this note, hold trigger for 3 seconds";
		Object.Destroy((Object)(object)val, Time.deltaTime);
		if (WristMenu.triggerDownR)
		{
			new Delay().D(1f, () =>
			{
				ack++;
			});
		}
	}

	public static void Beach(int i = 0)
	{
		if (!PhotonNetwork.InRoom || Local().IsMasterClient)
		{
			if (RigUtils.Map(OwnRig()) == "beach")
			{
				if (i == 2)
				{
					SportScoreboard.Instance.ResetScores();
				}
				else
				{
					SportScoreboard.Instance.TeamScored(i);
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In Beach!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static async void Lucy(object o)
	{
		if (Local().IsMasterClient)
		{
			HalloweenGhostChaser h = Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
			if ((Object)(object)h != (Object)null)
			{
				if (!((NetworkView)h).GetView.IsMine)
				{
					((NetworkView)h).RequestOwnership();
				}
				if (o is bool[] b2)
				{
					if (b2[0])
					{
						ChaseState g = (ChaseState)4;
						if (b2.Length == 2)
						{
							h.timeGongStarted = Time.time;
							h.currentState = g;
							h.isSummoned = b2[1];
							await Task.Delay(300);
							h.timeGongStarted = Time.time;
							h.currentState = g;
							h.isSummoned = b2[1];
						}
						else
						{
							h.currentState = g;
							h.isSummoned = b2[1];
						}
					}
					else
					{
						h.currentState = (ChaseState)1;
						h.isSummoned = false;
					}
				}
				float f = default;
				int num;
				if (o is float)
				{
					f = (float)o;
					num = 1;
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					if ((int)h.currentState != 1)
					{
						if ((int)h.currentState == 8)
						{
							h.currentSpeed = f;
							h.lastSpeedIncreased = Time.time + 99999f;
						}
						else
						{
							DelayedNoti(Er + "Lucy Isnt Chasing!");
						}
					}
					else
					{
						DelayedNoti(Er + "Lucy Isnt Spawned!");
					}
				}
				NetPlayer t = (NetPlayer)((o is NetPlayer) ? o : null);
				if (t != null)
				{
					if ((int)h.currentState != 1)
					{
						if ((int)h.currentState == 8)
						{
							h.followTarget = ((Component)RigUtils.RigFromNet(t)).transform;
							h.targetPlayer = t;
						}
						else
						{
							DelayedNoti(Er + "Lucy Isnt Chasing!");
						}
					}
					else
					{
						DelayedNoti(Er + "Lucy Isnt Spawned!");
					}
				}
				VRRig r = (VRRig)((o is VRRig) ? o : null);
				if (r != null)
				{
					new Delay().D(h.grabDuration + 0.16f, () =>
					{
						h.currentState = (ChaseState)16;
						h.grabTime = Time.time;
						h.targetPlayer = r.Creator;
					});
				}
				Vector3 p = default;
				int num2;
				if (o is Vector3)
				{
					p = (Vector3)o;
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
				if (num2 != 0)
				{
					h.currentState = (ChaseState)8;
					h.followTarget = ((Component)OwnRig()).transform;
					h.targetPlayer = OwnRig().Creator;
					((Component)h).transform.position = p;
				}
			}
			else
			{
				DelayedNoti(Er + "Couldnt Get Lucy!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static async void Lurk(NetPlayer p)
	{
		if (Local().IsMasterClient)
		{
			LurkerGhost h = Find("Environment Objects/05Maze_PersistentObjects/Ghosts/GhostLurker_Prefab").GetComponent<LurkerGhost>();
			if ((Object)(object)h != (Object)null)
			{
				if (!((NetworkView)h).GetView.IsMine)
				{
					((NetworkView)h).RequestOwnership();
				}
				Traverse t = Create(h);
				VRRig r = RigUtils.RigFromNet(p);
				t.Field(Field(80)).SetValue((object)3);
				t.Field(Field(81)).SetValue((object)p);
				t.Field(Field(82)).SetValue((object)r.head.rigTarget);
				t.Field(Field(83)).SetValue((object)r);
			}
			else
			{
				DelayedNoti(Er + "Couldnt Get Lurker!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static async void MuteP(int r)
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		new Delay().D(0.03f, () =>
		{
			for (int i = 0; i < 7; i++)
			{
				((LoadBalancingClient)((VoiceConnection)PhotonVoiceNetwork.Instance).Client).OpRaiseEvent((byte)202, (object)new object[3]
				{
					(byte)0,
					(byte)2,
					new object[1] { "MALACHIS MENU ONTOP" }
				}, new RaiseEventOptions
				{
					Receivers = (ReceiverGroup)(byte)r
				}, SendOptions.SendUnreliable);
			}
		});
	}

	public static void Break(int a, bool o)
	{
		if (Local().IsMasterClient)
		{
			if (IsGameMode((GameModeType)8))
			{
				if (o)
				{
					GameMode.OptOut(a);
				}
				else
				{
					GameMode.OptIn(a);
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In A Guardian GameMode!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must be Master!");
		}
	}

	public static void Dep(int i, string s)
	{
		string name = s + " Fossils Collected (M)";
		if (Local().IsMasterClient)
		{
			if (RigUtils.Map(OwnRig()) == "VIMExperience3")
			{
				new Delay().D(0.6f, () =>
				{
					GameEntity value = Create(Mods.F<VoxelSpawnableDeposit>(5f).FirstOrDefault()).Field(Field(78)).GetValue<GameEntity>();
					_003F val = value;
					long num = ((s == "Add 1") ? (value.GetState() + 1) : ((s == "Remove 1") ? (value.GetState() - 1) : i));
					((GameEntity)val).RequestState(num);
				});
				if (s != "Spaz")
				{
					DisableButton(name);
				}
			}
			else
			{
				SendNoti(Er + "You Must Be In The Fossil Hunt Map!");
				DisableButton(name);
			}
		}
		else
		{
			SendNoti(Er + "You Must Be Master!");
			DisableButton(name);
		}
	}

	public static async void Des(VRRig r)
	{
		PhotonNetwork.OpRemoveCompleteCacheOfPlayer(RigUtils.PlayerFromRig(r).ActorNumber);
	}

	public static void K()
	{
		if (PhotonNetwork.InRoom)
		{
			if (!PhotonNetwork.CurrentRoom.IsVisible)
			{
				if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Where((TouchingPlayerInfo e) =>
				{
					return TouchingPlayerInfo.op_Implicit(e) != Local().UserId;
				}).ToList().Count > 0)
				{
					kic = PhotonNetwork.CurrentRoom.Name;
					GorillaComputer.instance.OnGroupJoinButtonPress(0, GorillaComputer.instance.friendJoinCollider);
					ki = true;
					SendNoti("Kicking Players Please Wait...");
				}
				else
				{
					SendNoti(Er + "No One Is Near A Computer!");
				}
			}
			else
			{
				SendNoti(Er + "You Must Be In A Private Lobby!");
			}
		}
		DisableButton("Kick All Near Computer");
	}

	public static IEnumerator Re()
	{
		yield return (object)new WaitForSeconds(0.8f);
		GuiPlugin.Join(0, kic);
	}

	public static IEnumerator q(int m = 0)
	{
		if (m >= 69)
		{
			List<Player> list;
			switch (m)
			{
			case 88:
			{
				Player p = ((crashac != -1) ? RigUtils.PlayerFromActor(crashac) : null);
				GameEntityManager g = GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone);
				if (crashac != -1 && !InGR(RigUtils.RigFromPlayer(p)))
				{
					SendNoti(Er + "This Player Must Be In GhostReactor!");
					if (crashac == -1)
					{
						ruu = false;
					}
					crashac = 0;
				}
				else if (!((NetworkView)g).IsMine)
				{
					if (!Local().IsMasterClient && (from e in RigUtils.AllRigs(i: false)
						where InGR(e)
						select e).ToList().Count != 0)
					{
						SendNoti(Er + "You rae Not The 1st In The Map!");
						if (crashac == -1)
						{
							ruu = false;
						}
						crashac = 0;
					}
					else
					{
						Ow(g);
						yield return (object)new WaitForSeconds(0.2f);
					}
				}
				else
				{
					Crr(Vector3.zero, Vector3.zero, Quaternion.identity, l: true, new int[1] { -71 }, g, (crashac == -1) ? OwnRig() : RigUtils.RigFromPlayer(p));
					yield return (object)new WaitForSeconds(0.7f);
					SendNoti("[<color=green>CRASHED</color>] Crashed: " + ((crashac != -1) ? p.NickName : "Everyone"));
					if (crashac == -1)
					{
						ruu = false;
					}
					crashac = 0;
				}
				yield break;
			}
			default:
				list = (from e in RigUtils.AllPlayers()
					where e != brevr.Creator.GetPlayerRef()
					select e).ToList();
				break;
			case 69:
				list = RigUtils.AllPlayers();
				break;
			}
			List<Player> v = list;
			Breakm(v.Where((Player e) => InGR(RigUtils.RigFromPlayer(e))).ToList());
			ruuu = false;
			brevr = null;
		}
		else if (CosDone())
		{
			VRRig o = OwnRig();
			if (RigUtils.Map(o) == "city" || RigUtils.Map(o) == "mountain" || RigUtils.Map(o) == "GTFC")
			{
				Vector3 v2;
				if (RigUtils.Map(o) == "city")
				{
					v2 = new Vector3(-52.06f, 17.6f, -119.6f);
				}
				else
				{
					v2 = ((RigUtils.Map(o) == "mountain") ? new Vector3(-14.62f, 18.11f, -111.72f) : new Vector3(232.45f, 160f, -788.25f));
				}
				if (!o.inTryOnRoom)
				{
					((Behaviour)o).enabled = false;
					DrawHandOrbs();
					((Component)o).transform.position = v2;
				}
				string i = ((m == -1 || m == 2) ? "NOTHING" : (Field(67) + "."));
				string[] a2 = new string[16]
				{
					i, i, i, i, i, i, i, i, i, i,
					i, i, i, i, i, i
				};
				CosmeticsController c2 = Con();
				CosmeticSet s = (o.cosmeticSet = (c2.currentWornSet = new CosmeticSet(a2, c2)));
				if (!PhotonNetwork.InRoom)
				{
					OwnRig().LocalUpdateCosmeticsWithTryon(s, c2.tryOnSet, false);
				}
				else
				{
					try
					{
						GIn().myVRRig.SendRPC(RPC(10), (RpcTarget)0, new object[3]
						{
							s.ToPackedIDArray(),
							c2.tryOnSet.ToPackedIDArray(),
							false
						});
					}
					catch
					{
					}
				}
				yield return (object)new WaitForSeconds(0.1f);
				switch (m)
				{
				case -1:
					((Behaviour)o).enabled = true;
					ruu = false;
					yield break;
				default:
					((Behaviour)o).enabled = false;
					DrawHandOrbs();
					((Component)o).transform.position = v2 + new Vector3(0f, 7f, 0f);
					yield return (object)new WaitForSeconds(0.1f);
					((Component)o).transform.position = v2;
					yield return (object)new WaitForSeconds(0.1f);
					((Component)o).transform.position = v2 + new Vector3(0f, 7f, 0f);
					yield return (object)new WaitForSeconds(0.1f);
					((Component)o).transform.position = v2;
					break;
				case 2:
					break;
				}
				if (m > 0)
				{
					List<CosmeticItem> l = new List<CosmeticItem>();
					if (m == 1 || m == 2)
					{
						l = c2.allCosmetics.Where((CosmeticItem e) =>
						{
							return e.itemName != "LFAEK." && e.itemName != "LFAAJ." && e.itemName != "LFAAI." && e.itemName != "LFAAH." && e.itemName != "LFAAM." && e.canTryOn && !e.isHoldable && !e.IsCollectable && !e.isThrowable && (int)e.itemCategory != 6 && (int)e.itemCategory != 2 && (int)e.itemCategory != 11;
						}).ToList();
					}
					for (int ii = 0; ii < l.Count; ii++)
					{
						NotifiLib.ClearPast(1);
						SendNoti(string.Format("Please be patient, {0} cosmetics now. {1}/{2}", (m == 2) ? "removing" : "getting", ii, l.Count));
						CosmeticItem co = l[ii];
						c2.ApplyCosmeticItemToSet(c2.tryOnSet, co, false, false);
						try
						{
							c2.UpdateWornCosmetics(true);
						}
						catch
						{
						}
						yield return (object)new WaitForSeconds(PhotonNetwork.InRoom ? 0.085f : 0f);
						if (m != 2)
						{
							c2.ApplyCosmeticItemToSet(c2.tryOnSet, co, false, false);
							try
							{
								c2.UpdateWornCosmetics(true);
							}
							catch
							{
							}
						}
					}
					if (m == 2)
					{
						((Behaviour)o).enabled = false;
						((Component)o).transform.position = v2 + new Vector3(0f, 7f, 0f);
						yield return (object)new WaitForSeconds(0.1f);
						((Behaviour)o).enabled = true;
					}
					SendNoti("<color=green>COSMETICS FINISHED</color>");
				}
				((Behaviour)o).enabled = true;
				ruu = false;
			}
			else
			{
				SendNoti(Er + "You Must Be In City, Mountains, Or VIM Room!");
				ruu = false;
			}
		}
		else
		{
			SendNoti(Er + "You Must Be Unbanned!");
			ruu = false;
		}
	}

	public static void Gi(int m = 0)
	{
		MakeGun(l: true, () =>
		{
			if ((Object)(object)giv == (Object)null)
			{
				giv = lockedrig;
				SendNoti("[<color=green>CRASH</color>] Selected Player: " + giv.playerNameVisible);
			}
			else if ((Object)(object)lockedrig != (Object)(object)giv)
			{
				giv = lockedrig;
				SendNoti("[<color=green>CRASH</color>] Selected Player: " + giv.playerNameVisible);
			}
		});
		if (!((Object)(object)giv != (Object)null))
		{
			return;
		}
		if ((Object)(object)gl == (Object)null)
		{
			GameObject val = new GameObject("gl");
			val.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			gl = GTExt.GetOrAddComponent<TextMesh>(val);
			gl.color = Color.white;
			gl.fontSize = 25;
			gl.richText = true;
			gl.fontStyle = (FontStyle)0;
			gl.characterSize = 0.1f;
			gl.anchor = (TextAnchor)4;
			gl.alignment = (TextAlignment)1;
		}
		string text = "";
		List<VRRig> l = (from e in RigUtils.AllRigs(i: false)
			where (Object)(object)e != (Object)(object)giv
			select e).ToList();
		if (m == 0)
		{
			text = "ANYONE THIS PLAYER TOUCHES WILL CRASH";
			if ((Object)(object)bt(giv) == (Object)null)
			{
				DelayedNoti(Er + "Selected Player Must Be In MonkeBlocks");
			}
			else
			{
				foreach (VRRig item in l)
				{
					if ((double)Vector3.Distance(giv.rightHandTransform.position, ((Component)item).transform.position) < 0.31 || (double)Vector3.Distance(giv.leftHandTransform.position, ((Component)item).transform.position) < 0.31)
					{
						DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false, -1, item);
					}
				}
			}
		}
		if (m == 1)
		{
			text = "PLAYER MUST HOLD RIGHT TRIGGER TO FLING ALL";
			GuardMeth("Give Guardian Fling All", () =>
			{
				if (((VRMap)giv.rightIndex).calcT > 0.4f)
				{
					foreach (VRRig item2 in l)
					{
						Throw(item2, new Vector3(0f, 1000f, 0f));
					}
				}
			});
		}
		if (m == 2)
		{
			text = "PLAYER MUST HOLD RIGHT TRIGGER TO SPAZ ALL";
			GuardMeth("Give Guardian Spaz All", () =>
			{
				if (((VRMap)giv.rightIndex).calcT > 0.4f)
				{
					foreach (VRRig item3 in l)
					{
						Throw(item3, RandomV(1000f));
					}
				}
			});
		}
		if (m == 3)
		{
			text = "PLAYER MUST HOLD RIGHT TRIGGER TO ORBIT ALL";
			GuardMeth("Give Guardian Orbit All", () =>
			{
				if (((VRMap)giv.rightIndex).calcT > 0.4f)
				{
					foreach (VRRig item4 in l)
					{
						Vector3 val2 = ((Component)giv).transform.position - ((Component)item4).transform.position;
						Throw(item4, val2.normalized * 300f);
					}
				}
			});
		}
		if (m == 4)
		{
			text = "PLAYER MUST HOLD RIGHT TRIGGER TO MAT ALL";
			if (((VRMap)giv.rightIndex).calcT > 0.4f)
			{
				if (IsGameMode((GameModeType)3))
				{
					PaintMat(giv, 1);
				}
				else
				{
					new Delay().D(matd, () =>
					{
						mat++;
						if (mat > 2)
						{
							mat = 1;
						}
						if (mat == 1)
						{
							l.ForEach((VRRig val2) =>
							{
								Tagged(val2.Creator);
							});
						}
						if (mat == 2)
						{
							l.ForEach((VRRig val2) =>
							{
								UnTag(val2.Creator);
							});
						}
					});
				}
			}
		}
		if (m == 5)
		{
			text = "ANYONE WHO TOUCHES THIS PLAYER WILL CRASH";
			if ((Object)(object)bt(giv) == (Object)null)
			{
				DelayedNoti(Er + "Selected Player Must Be In MonkeBlocks");
			}
			else
			{
				foreach (VRRig item5 in l)
				{
					if ((double)Vector3.Distance(item5.rightHandTransform.position, ((Component)giv).transform.position) < 0.31 || (double)Vector3.Distance(item5.leftHandTransform.position, ((Component)giv).transform.position) < 0.31)
					{
						DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false, -1, item5);
					}
				}
			}
		}
		if (m == 6)
		{
			text = "ANYONE THIS PLAYER TOUCHES WILL FREEZE";
			foreach (VRRig r in l)
			{
				if ((double)Vector3.Distance(giv.rightHandTransform.position, ((Component)r).transform.position) < 0.31 || (double)Vector3.Distance(giv.leftHandTransform.position, ((Component)r).transform.position) < 0.31)
				{
					new Delay().D(2f, () =>
					{
						Breakm(r);
					});
				}
			}
		}
		if (m == 7)
		{
			text = "ANYONE WHO TOUCHES THIS PLAYER WILL FREEZE";
			foreach (VRRig r2 in l)
			{
				if ((double)Vector3.Distance(r2.rightHandTransform.position, ((Component)giv).transform.position) < 0.31 || (double)Vector3.Distance(r2.leftHandTransform.position, ((Component)giv).transform.position) < 0.31)
				{
					new Delay().D(2f, () =>
					{
						Breakm(r2);
					});
				}
			}
		}
		gl.text = text;
		((Component)gl).transform.position = ((Component)giv).transform.position + new Vector3(0f, 0.8f, 0f);
		((Component)gl).transform.rotation = ((Component)giv).transform.rotation;
		((Component)gl).transform.LookAt(Find("Main Camera").transform.position);
		((Component)gl).transform.Rotate(0f, 180f, 0f);
	}

	public static void Lava(RisingLavaState s, string b)
	{
		if (!PhotonNetwork.InRoom || Local().IsMasterClient)
		{
			GameObject g = Find("Rewind_2024-02_Forest");
			if ((Object)(object)g == (Object)null)
			{
				SendNoti(Er + "You Must Be In The Floor Is Lava Map!");
				DisableButton(b + " Lava (M)");
				return;
			}
			new Delay().D(1f, () =>
			{
				for (int i = 0; i < 50; i++)
				{
					InfectionLavaController component = ((Component)g.transform.Find("VIMForestLava (prefab)/ILavaYou_PrefabV/ForestLavaController")).gameObject.GetComponent<InfectionLavaController>();
					FieldInfo field = ((object)component).GetType().GetField(Field(57), BindingFlags.Instance | BindingFlags.NonPublic);
					object value = field.GetValue(component);
					value.GetType().GetField(Field(61)).SetValue(value, s);
					value.GetType().GetField(Field(62)).SetValue(value, PhotonNetwork.Time);
					field.SetValue(component, value);
					((object)component).GetType().GetMethod(Field(63), BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(component, null);
				}
				DisableButton(b + " Lava (M)");
			});
		}
		else
		{
			SendNoti(Er + "You Must Be Master!");
			DisableButton(b + " Lava (M)");
		}
	}

	public static void BreakMove(bool i = true)
	{
		MakeGun(i, () =>
		{
			if (ha == Vector3.zero)
			{
				ha = (Vector3)(i ? new Vector3(((Component)lockedrig).transform.position.x, Mathf.Min(((Component)lockedrig).transform.position.y - 0.45f, Mathf.Min(lockedrig.leftHandTransform.position.y - 0.061f, lockedrig.rightHandTransform.position.y - 0.061f)), ((Component)lockedrig).transform.position.z) : (pointer.transform.position + new Vector3(0f, 0.25f, 0f)));
			}
			if (Time.time > orbit)
			{
				Delay delay = new Delay();
				float num = ((bre == 6) ? 0.095f : ((bre == 2) ? 0.8f : 0.74f));
				delay.D(num, () =>
				{
					bre++;
					if (bre > 6)
					{
						bre = 2;
					}
					if (bre == 1)
					{
						ang = 13f;
						di = 0.23f;
					}
					if (bre == 2)
					{
						di = 0.5f;
					}
					if (bre == 3 || bre == 4)
					{
						ang = 8.92f;
						di = ((bre == 4) ? 0.74f : 0.8f);
					}
					if (bre == 5)
					{
						ang = 10f;
						di = 0.372f;
					}
					if (bre == 6)
					{
						ang = 13f;
						di = 0.015f;
					}
				});
				orbit = Time.time;
				angle += ang * Time.deltaTime;
				Vector3 val = new Vector3(ha.x + di * Mathf.Cos(angle), ha.y, ha.z + di * Mathf.Sin(angle));
				Quaternion val2 = Quaternion.LookRotation(ha - val);
				if (bre == 3)
				{
					val2 *= Quaternion.Euler(71f, 0f, 0f);
					val.y += 0.186f;
				}
				if (bre == 4)
				{
					val2 *= Quaternion.Euler(130f, 0f, 0f);
					val.y += 0.6f;
				}
				if (bre == 5)
				{
					val2 *= Quaternion.Euler(150f, 0f, 0f);
					val.y += 0.91f;
				}
				if (bre == 6)
				{
					val.y += 1.04f;
				}
				DropBlock("", val, Vector3.zero, val2, r: true, 439542361);
			}
		}, () =>
		{
			bre = 0;
			ha = Vector3.zero;
		});
	}

	public static void Elli()
	{
		Master("Block Ellipsoid Gun (M)", () =>
		{
			MakeGun(l: false, () =>
			{
				new Delay().D(2.2f, () =>
				{
					bre++;
					if (bre > 3)
					{
						bre = 1;
					}
				});
				if (ogpp == Vector3.zero)
				{
					ogpp = rayCur.point + new Vector3(0f, 1f, 0f);
				}
				Vector3 val = Vector3.zero;
				Quaternion ro = Quaternion.identity;
				if (bre == 1)
				{
					val = new Delay().Orbit("ell", ogpp, 0.8f, 0f, 0.8f, 3f);
					ro = Quaternion.LookRotation(ogpp - val);
				}
				if (bre == 2)
				{
					val = new Delay().Orbit("ell", ogpp, 0.9f, 0.8f, 1f, 3f, new Dictionary<string, float>
					{
						{
							"x",
							Mathf.Cos(da["ell"])
						},
						{
							"y",
							Mathf.Sin(da["ell"])
						},
						{ "z", 0f }
					});
					ro = Quaternion.LookRotation(Vector3.forward, ogpp - val);
				}
				if (bre == 3)
				{
					val = new Delay().Orbit("ell", ogpp, 0f, 0.8f, 0.9f, 3f, new Dictionary<string, float>
					{
						{ "x", 0f },
						{
							"y",
							Mathf.Cos(da["ell"])
						},
						{
							"z",
							Mathf.Sin(da["ell"])
						}
					});
					ro = Quaternion.LookRotation(Vector3.right, ogpp - val);
				}
				DropBlock(block, val, Vector3.zero, ro, r: true);
			}, () =>
			{
				bre = 0;
				ogpp = Vector3.zero;
			});
		});
	}

	public static async void DV(PhotonView v)
	{
		new Delay().D(0.0894f, () =>
		{
			R();
			PhotonNetwork.Destroy(v);
		});
		new Delay().D(0.3f, () =>
		{
			RPCFlush();
		});
	}

	public static void R()
	{
		try
		{
			if (!mo)
			{
				Type typeFromHandle = typeof(MonkeAgent);
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
				s = new object[7]
				{
					typeFromHandle.GetField(Field(44), bindingAttr),
					typeFromHandle.GetField(Field(45), bindingAttr),
					typeFromHandle.GetField(Field(46), bindingAttr),
					typeFromHandle.GetField(Field(47), bindingAttr),
					typeFromHandle.GetField(Field(48), bindingAttr),
					typeFromHandle.GetField(Field(49), bindingAttr),
					typeFromHandle.GetMethod(Field(50), bindingAttr)
				};
				mo = true;
			}
			if ((Object)(object)MonkeAgent.instance != (Object)null)
			{
				MonkeAgent instance = MonkeAgent.instance;
				((FieldInfo)s[0])?.SetValue(instance, "");
				((FieldInfo)s[1])?.SetValue(instance, "");
				((FieldInfo)s[2])?.SetValue(instance, "");
				((FieldInfo)s[3])?.SetValue(instance, false);
				if (((FieldInfo)s[4])?.GetValue(instance) is IList list)
				{
					list.Clear();
				}
				if (((FieldInfo)s[5])?.GetValue(instance) is IDictionary dictionary)
				{
					dictionary.Clear();
				}
				MonkeAgent instance2 = MonkeAgent.instance;
				((MethodInfo)s[6])?.Invoke(instance2, null);
				((FieldInfo)s[0])?.SetValue(instance2, "");
				((FieldInfo)s[1])?.SetValue(instance2, "");
				((FieldInfo)s[2])?.SetValue(instance2, "");
			}
		}
		catch
		{
		}
	}

	public static async void CreateLobby()
	{
		string b2 = "Create Public Lobby";
		if (WristMenu.customboards || CosDone())
		{
			GorillaNetworkJoinTrigger g = PhotonNetworkController.Instance.currentJoinTrigger;
			List<string> f = new List<string>();
			for (int _ = 0; _ < 9; _++)
			{
				f.Add(RandomString(s: false, 10));
			}
			RoomConfig val = new RoomConfig
			{
				createIfMissing = true,
				isJoinable = true,
				isPublic = true,
				MaxPlayers = 10
			};
			Hashtable val2 = new Hashtable();
			((Dictionary<object, object>)val2).Add((object)"gameMode", (object)(((Object)(object)g == (Object)null || g.networkZone == "private") ? Mods.F<GorillaNetworkJoinTrigger>(5f).OrderBy((GorillaNetworkJoinTrigger e) =>
			{
				return Vector3.Distance(((Component)OwnRig()).transform.position, ((Component)e).gameObject.transform.position);
			}).First()
				.GetFullDesiredGameModeString() : g.GetFullDesiredGameModeString()));
			((Dictionary<object, object>)val2).Add((object)"platform", (object)Create(PhotonNetworkController.Instance).Field(Field(40)).GetValue<string>());
			((Dictionary<object, object>)val2).Add((object)"queueName", (object)GorillaComputer.instance.currentQueue);
			((Dictionary<object, object>)val2).Add((object)"language", (object)((object)LocalisationManager.CurrentLanguage).ToString());
			((Dictionary<object, object>)val2).Add((object)"fan_club", (object)SubscriptionManager.IsLocalSubscribed().ToString().ToLower());
			val.CustomProps = val2;
			val.SearchFilter = null;
			RoomConfig r = val;
			r.SetFriendIDs(f);
			await NetworkSystem.Instance.ConnectToRoom((string)null, r, -1);
			SendNoti("[<color=blue>ROOM</color>] Creating A Public Lobby!");
		}
		else
		{
			SendNoti(Er + "You Must Be Unbanned To Use This Mod!");
		}
		DisableButton(b2);
	}

	public static async void Log2()
	{
		if (!PhotonNetwork.InRoom || Local().IsMasterClient)
		{
			List<HitTargetNetworkState> l = Mods.F<HitTargetNetworkState>(5f).ToList();
			if (new string[3] { "forest", "canyon", "mountain" }.Any((string e) => RigUtils.AllRigs().Any((VRRig r) => RigUtils.Map(r) == e)))
			{
				LinqUtils.ForEach<HitTargetNetworkState>(l.Where((HitTargetNetworkState e) => ((WatchableGenericSO<int>)(object)Create(e).Field(Field(19)).GetValue<WatchableIntSO>()).Value > -1), (Action<HitTargetNetworkState>)((HitTargetNetworkState h) =>
				{
					SetTarget(h, int.MinValue, 0f, a: false);
				}));
				new Delay().D(0.69f, () =>
				{
					l.ForEach((HitTargetNetworkState e) =>
					{
						SetTarget(e, 6767, 0f, a: false);
					});
				});
			}
			else
			{
				SendNoti(Er + "Someone Must Be In Forest, Canyons, Or Mountains!");
				DisableButton("Log Spam All V2 (M)");
			}
		}
		else
		{
			SendNoti(Er + "You Must Be Master!");
			DisableButton("Log Spam All V2 (M)");
		}
	}

	public static async void SetTarget(HitTargetNetworkState h, int i, float d = 0.69f, bool a = true)
	{
		if (!PhotonNetwork.InRoom || Local().IsMasterClient)
		{
			Traverse t = Create(h);
			if (!(Time.time <= t.Field(Field(36)).GetValue<float>()))
			{
				WatchableIntSO w = t.Field(Field(19)).GetValue<WatchableIntSO>();
				int n = ((i == 6767) ? (((WatchableGenericSO<int>)(object)w).Value + 1) : i);
				if (a)
				{
					h.PlayAudio(((WatchableGenericSO<int>)(object)w).Value, n);
				}
				((WatchableGenericSO<int>)(object)w).Value = n;
				t.Field(Field(36)).SetValue((object)(Time.time + d));
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static void TargetGun(int i)
	{
		MakeGun(l: false, () =>
		{
			if (Object.op_Implicit((Object)(object)((Component)rayCur.ReflectionEnums).GetComponentInParent<HitTargetNetworkState>()))
			{
				SetTarget(((Component)rayCur.ReflectionEnums).GetComponentInParent<HitTargetNetworkState>(), i);
			}
		});
	}

	public static async void Rank(float i)
	{
		if (!PhotonNetwork.InRoom || Local().IsMasterClient)
		{
			if (GuiPlugin.GetQueOrGameMode(0).ToUpper().Contains("RANKED") || RigUtils.Map(OwnRig()) == "ranked")
			{
				GorillaTagCompetitiveManager g = (GorillaTagCompetitiveManager)GorillaGameManager.instance;
				if (i != -67f)
				{
					Create(g).Field(Field(33)).SetValue((object)i);
				}
				else if (((GorillaTagManager)g).isCurrentlyTag)
				{
					((GorillaTagManager)g).infectedModeThreshold = 0;
				}
				else
				{
					SendNoti(Er + "Ranked Is Already Started!");
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In A Ranked Lobby!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
		}
	}

	public static void Ghostmonke()
	{
		VRRig val = OwnRig();
		((Behaviour)val).enabled = !ghostMonke;
		bool flag = (right ? WristMenu.ybuttonDown : WristMenu.bbuttonDown);
		if (ghostMonke)
		{
			((VRMap)val.leftThumb).calcT = 0f;
			((VRMap)val.rightThumb).calcT = 0f;
			((VRMap)val.leftThumb).LerpFinger(1f, false);
			((VRMap)val.rightThumb).LerpFinger(1f, false);
			DrawHandOrbs();
		}
		if (flag && !last)
		{
			ghostMonke = !ghostMonke;
		}
		last = flag;
	}

	public static void Invis()
	{
		((Behaviour)OwnRig()).enabled = !invisMonke;
		bool flag = (right ? WristMenu.ybuttonDown : WristMenu.bbuttonDown);
		if (invisMonke)
		{
			((Component)OwnRig()).transform.position = new Vector3(0f, -100f, 0f);
			DrawHandOrbs();
		}
		if (flag && !last2)
		{
			invisMonke = !invisMonke;
		}
		last2 = flag;
	}

	public static void GrabRig()
	{
		VRRig val = OwnRig();
		if (WristMenu.gripDownR)
		{
			DrawHandOrbs();
			((Behaviour)val).enabled = false;
			((Component)val).transform.position = Loco().RightHand.controllerTransform.position;
		}
		if (WristMenu.gripDownL)
		{
			DrawHandOrbs();
			((Behaviour)val).enabled = false;
			((Component)val).transform.position = Loco().LeftHand.controllerTransform.position;
		}
		if (!WristMenu.gripDownR && !WristMenu.gripDownL)
		{
			((Behaviour)val).enabled = true;
		}
	}

	public static void TPMaps()
	{
		List<ButtonInfo> list = new List<ButtonInfo>
		{
			new ButtonInfo
			{
				buttonText = "Stump",
				method = () =>
				{
					TPToMap("", new Vector3(-66.97f, 12.48f, -83.02f), "Stump", 400, () =>
					{
						Find("Environment Objects/LocalObjects_Prefab/TreeRoom").SetActive(true);
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Canyons",
				method = () =>
				{
					TPToMap(tr + "ForestCanyonTransition", new Vector3(-80.89f, 12.09f, -100.46f), "Canyons");
				}
			},
			new ButtonInfo
			{
				buttonText = "Caves",
				method = () =>
				{
					TPToMap(tr + "ForestToCave", new Vector3(-73.29f, -14.52f, -39.86f), "Caves");
				}
			},
			new ButtonInfo
			{
				buttonText = "OG Caves",
				method = () =>
				{
					TPToMap(tr + "ForestToCave", new Vector3(-45.24f, -6.3f, -75.57f), "OG Caves", 600, () =>
					{
						((GorillaTriggerBox)Find("Cave_Main_Prefab/SimpleZones/OldCaveEnable").GetComponent<LowEffortZone>()).OnBoxTriggered();
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "HoverPark",
				method = () =>
				{
					TPToMap(tr + "ForestToHoverboard", new Vector3(-90.33f, -16.03f, 29.96f), "HoverPark");
				}
			},
			new ButtonInfo
			{
				buttonText = "Beach",
				method = () =>
				{
					TPToMap(tr + "BeachToForest", new Vector3(-13.83f, 30f, -19.3f), "Beach");
				}
			},
			new ButtonInfo
			{
				buttonText = "Clouds",
				method = () =>
				{
					TPToMap(tr + "CityToSkyJungle", new Vector3(-76.34f, 164f, -98.04f), "Clouds");
				}
			},
			new ButtonInfo
			{
				buttonText = "City",
				method = () =>
				{
					TPToMap(tr + "ForestToForestWithCity", new Vector3(-62.49f, 17.04f, -100.95f), "City", 800);
				}
			},
			new ButtonInfo
			{
				buttonText = "Basement",
				method = () =>
				{
					TPToMap(tr + "CityBasementTransition", new Vector3(-35.81f, 14.8f, -93.18f), "Basement");
				}
			},
			new ButtonInfo
			{
				buttonText = "Critters",
				method = () =>
				{
					TPToMap("", new Vector3(96.76f, -91.18f, 44.16f), "Critters", 1000, () =>
					{
						((GorillaTriggerBox)Find("Environment Objects/" + tr + "ForestToGMM").GetComponent<GorillaSetZoneTrigger>()).OnBoxTriggered();
						((GorillaTriggerBox)Find("Environment Objects/LocalObjects_Prefab/CritterGMM_Connector/TeleportNode (orig)").GetComponent<TeleportNode>()).OnBoxTriggered();
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Mountains",
				method = () =>
				{
					TPToMap(tr + "CityMountainTransition", new Vector3(-19.8f, 18.8f, -106.72f), "Mountains", 1200, () =>
					{
						Find("City_Pretty").SetActive(false);
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Metro",
				method = () =>
				{
					TPToMap(tr + "CityToMetropolis", new Vector3(64.65f, 4.32f, -241.36f), "Metro", 400, () =>
					{
						Find("City_Pretty").SetActive(false);
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Rotating",
				method = () =>
				{
					TPToMap(tr + "CityToRotating", new Vector3(-29.78f, -9.77f, -343.98f), "Rotating");
				}
			},
			new ButtonInfo
			{
				buttonText = "Ranked",
				method = () =>
				{
					TPToMap("", new Vector3(-105.71f, 18.34f, -272.41f), "Ranked", 400, () =>
					{
						((GorillaTriggerBox)Find("Environment Objects/" + tr + "ForestToGMM").GetComponent<GorillaSetZoneTrigger>()).OnBoxTriggered();
						((GorillaTriggerBox)Find("Environment Objects/LocalObjects_Prefab/CityToRanked/TeleportNode_City").GetComponent<TeleportNode>()).OnBoxTriggered();
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "MonkeBlocks",
				method = () =>
				{
					TPToMap("LocalObjects_Prefab/MallToMonkeBlocks/ZoneTriggerMallToMonkeBlocks", new Vector3(-121.75f, 17.6f, -221.62f), "MonkeBlocks");
				}
			},
			new ButtonInfo
			{
				buttonText = "SharedBlocks",
				method = () =>
				{
					TPToMap("LocalObjects_Prefab/MallToMonkeBlocksShared/ZoneTriggerMonkeBlocksSharedMaps", new Vector3(-282.41f, 31.7f, -222.63f), "SharedBlocks");
				}
			},
			new ButtonInfo
			{
				buttonText = "GhostReactor",
				method = () =>
				{
					TPToMap("LocalObjects_Prefab/CaveToGhostReactor/ZoneTriggerCaveToGhostReactor", new Vector3(-23.59f, -24.6f, -51.17f), "GhostReactor");
				}
			},
			new ButtonInfo
			{
				buttonText = "GhostDrill",
				method = () =>
				{
					TPToMap("", new Vector3(-33.45f, -49.19f, -80.95f), "GhostDrill", 800, () =>
					{
						if ((Object)(object)Find("GhostReactorRoot/GhostReactorZoneDrillWithArt") == (Object)null)
						{
							SceneManager.LoadScene("GhostReactorDrill", (LoadSceneMode)1);
						}
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Bayou",
				method = () =>
				{
					TPToMap("", new Vector3(-145.44f, -14.14f, -63.88f), "Bayou", 600, () =>
					{
						((GorillaTriggerBox)Find("Environment Objects/" + tr + "ForestToGMM").GetComponent<GorillaSetZoneTrigger>()).OnBoxTriggered();
						((GorillaTriggerBox)Find("Environment Objects/LocalObjects_Prefab/GMM/Entrances_GMM/GMM_PropHuntEntrance/TeleportNode_Mallside").GetComponent<TeleportNode>()).OnBoxTriggered();
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "VIM",
				method = () =>
				{
					TPToMap(tr + "CityToGTFC", new Vector3(220.85f, 160.23f, -805.9f), "VIM", 400, () =>
					{
						WristMenu.VIM.e = true;
						Find("Environment Objects/LocalObjects_Prefab/GMMToGTFC/GTFC_Stairs GTFCside/VirtualStump_LaserDoorSolid (2)").SetActive(false);
					});
				}
			},
			new ButtonInfo
			{
				buttonText = "Lava",
				method = () =>
				{
					TPToMap("05Maze_PersistentObjects/GhostReactorElevatorManager/VIMForestLavaElevator/Triggers/VIMExp1_SetZoneTrigger", new Vector3(213.07f, 74.21f, 219.91f), "Lava");
				}
			},
			new ButtonInfo
			{
				buttonText = "GravDash",
				method = () =>
				{
					TPToMap("05Maze_PersistentObjects/GhostReactorElevatorManager/VIMExperience4Elevator/Triggers/VIMExp1_SetZoneTrigger", new Vector3(181.45f, 387.43f, 221.33f), "GravDash");
				}
			},
			new ButtonInfo
			{
				buttonText = "Fossil Hunt",
				method = () =>
				{
					TPToMap("05Maze_PersistentObjects/GhostReactorElevatorManager/VIMExperience3Elevator/Triggers/VIMExp1_SetZoneTrigger", new Vector3(194f, 299.77f, 245.01f), "Fossil Hunt");
				}
			},
			new ButtonInfo
			{
				buttonText = "VStump",
				method = () =>
				{
					TPToMap(tr + "ForestToGMM", new Vector3(-55.78f, 6.53f, -98.25f), "VStump", 666);
				}
			},
			new ButtonInfo
			{
				buttonText = "Space",
				method = () =>
				{
					TPToMap("", Vector3.zero, "Space");
				}
			},
			new ButtonInfo
			{
				buttonText = "Mall",
				method = () =>
				{
					TPToMap(tr + "ForestToGMM", new Vector3(-64.81f, 6.88f, -102.04f), "Mall");
				}
			},
			new ButtonInfo
			{
				buttonText = "OG Basement",
				method = () =>
				{
					TPToMap("05Maze_PersistentObjects/GhostReactorElevatorManager/VIMExperience2Elevator/Triggers/VIMExp1_SetZoneTrigger", new Vector3(-373.4f, -1.71f, 359.69f), "OG Basement");
				}
			}
		};
		list.ForEach((ButtonInfo e) =>
		{
			ButtonInfo buttonInfo = e;
			buttonInfo.method = (Action)Delegate.Combine(buttonInfo.method, (Action)(() =>
			{
				DisableButton(e.buttonText);
				DisableButton("TP To Maps");
				DeleteBoard();
			}));
		});
		SpawnBoard(list, "ed", 30, "Type A Map And Press It To TP To It|TP To Maps");
	}

	public static void Sw()
	{
		sww = true;
		if (Loco().IsHandTouching(true) || Loco().IsHandTouching(false))
		{
			RaycastHit value = Create(Loco()).Field(Field(56)).GetValue<RaycastHit>();
			Vector3 normal = value.normal;
			Vector3 normalized = normal.normalized;
			stt = Quaternion.LookRotation(Vector3.Cross(Vector3.right, normalized), normalized);
		}
		_ = rr;
		_ = stt;
		if (true)
		{
			rr = Quaternion.Slerp(rr, stt, 1f - Mathf.Exp(-5f * Time.deltaTime));
			GTPlayerTransform.ApplyRotationOverride(ref rr, Time.frameCount);
			Loco().SetGravityOverride((Object)(object)Loco(), (Action<GTPlayer>)((GTPlayer p) =>
			{
				p.AddForce(rr * Physics.gravity, (ForceMode)5);
			}));
		}
	}

	public static async void TPToMap(string m, Vector3 p, string b, int d = 400, Action s = null)
	{
		try
		{
			if (b != "Space")
			{
				if ((Object)(object)Find("City_Pretty") != (Object)null)
				{
					Create(Find("Environment Objects/LocalObjects_Prefab/GMM/MusicSoundPost_GMM/NormalReverbTrigger").GetComponent<ReverbTrigger>()).Field(Field(77)).GetValue<AudioMixerSnapshot>().TransitionTo(1f);
				}
				new Delay().D(0.5f, () =>
				{
					if (m != "")
					{
						((GorillaTriggerBox)Find("Environment Objects/" + m).GetComponent<GorillaSetZoneTrigger>()).OnBoxTriggered();
					}
					s?.Invoke();
				});
				await Task.Delay(d);
				Loco().TeleportTo(W(p), Quaternion.identity, false, false);
				ZoneShaderSettings.ActivateDefaultSettings();
				DisableButton(b);
			}
			else
			{
				((GorillaTriggerBox)Find("Environment Objects/" + tr + "ForestToGMM").GetComponent<GorillaSetZoneTrigger>()).OnBoxTriggered();
				await Task.Delay(250);
				((Tappable)Mods.F<TeleportStationTappable>(5f).Where((TeleportStationTappable e) =>
				{
					return Create(Create(e).Field(Field(69)).GetValue<TeleportStation>()).Field(Field(68)).GetValue<Vector3>() == new Vector3(-541.441f, 16.288f, 4.355f);
				}).FirstOrDefault()).OnTap(1f);
				ZoneShaderSettings.ActivateDefaultSettings();
				DisableButton(b);
			}
		}
		catch
		{
		}
	}

	public static void Changefly()
	{
		string[] array = new string[3] { "Head", "R Hand", "L Hand" };
		if (Flyus == Vector3.zero)
		{
			Flyus = Vector3.one;
		}
		flyus++;
		if (flyus > 2)
		{
			flyus = 0;
		}
		NewName("Change Fly Direction: " + array[flyus]);
	}

	public static void FlyMeth(float speed, object[] ve = null, bool v = true)
	{
		if (Flyus == Vector3.zero)
		{
			Changefly();
		}
		Rigidbody component = ((Component)Loco()).GetComponent<Rigidbody>();
		Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>
		{
			{
				0,
				((Component)Loco().headCollider).transform.forward
			},
			{
				1,
				Loco().RightHand.controllerTransform.forward - GIn().rightHandTransform.up
			},
			{
				2,
				Loco().LeftHand.controllerTransform.forward - GIn().leftHandTransform.up
			}
		};
		Flyus = dictionary[flyus];
		if (WristMenu.abuttonDown)
		{
			if (v)
			{
				Transform transform = ((Component)Loco()).transform;
				transform.position += Flyus * Time.deltaTime * speed;
			}
			if (ve != null)
			{
				component.linearVelocity += (Vector3)ve[0];
			}
			else
			{
				component.linearVelocity = Vector3.zero;
			}
		}
	}

	public static void IronMonke()
	{
		if (WristMenu.gripDownR)
		{
			if (ironsounds)
			{
				OwnRig().PlayHandTapLocal(115, false, 0.1f);
			}
			GIn().StartVibration(false, GIn().tapHapticStrength / 10f, 0.05f);
			((Component)Loco()).GetComponent<Rigidbody>().AddForce(new Vector3(15f * Loco().RightHand.controllerTransform.right.x, 15f * Loco().RightHand.controllerTransform.right.y, 15f * Loco().RightHand.controllerTransform.right.z), (ForceMode)5);
			GameObject val = CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val.GetComponent<Collider>());
			val.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
			val.transform.localPosition = OwnRig().rightHandTransform.position + new Vector3(0f, -0.05f, 0f);
			val.transform.rotation = OwnRig().rightHandTransform.rotation;
			val.GetComponent<Renderer>().material.shader = WristMenu.esp;
			val.GetComponent<Renderer>().material.color = Color32.op_Implicit(new Color32((byte)200, (byte)200, (byte)200, (byte)80));
			Object.Destroy((Object)(object)val, 0.3f);
		}
		if (WristMenu.gripDownL)
		{
			if (ironsounds)
			{
				OwnRig().PlayHandTapLocal(115, true, 0.1f);
			}
			GIn().StartVibration(true, GIn().tapHapticStrength / 10f, 0.05f);
			((Component)Loco()).GetComponent<Rigidbody>().AddForce(new Vector3(15f * Loco().LeftHand.controllerTransform.right.x * -1f, 15f * Loco().LeftHand.controllerTransform.right.y * -1f, 15f * Loco().LeftHand.controllerTransform.right.z * -1f), (ForceMode)5);
			GameObject val2 = CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val2.GetComponent<Collider>());
			val2.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
			val2.transform.localPosition = OwnRig().leftHandTransform.position + new Vector3(0f, -0.05f, 0f);
			val2.transform.rotation = OwnRig().leftHandTransform.rotation;
			val2.GetComponent<Renderer>().material.shader = WristMenu.esp;
			val2.GetComponent<Renderer>().material.color = Color32.op_Implicit(new Color32((byte)200, (byte)200, (byte)200, (byte)80));
			Object.Destroy((Object)(object)val2, 0.3f);
		}
	}

	public static void Frozone()
	{
		if (WristMenu.gripDownL)
		{
			GameObject val = CreatePrimitive((PrimitiveType)3);
			val.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
			val.transform.localPosition = OwnRig().leftHandTransform.position + new Vector3(0f, -0.05f, 0f);
			val.transform.rotation = OwnRig().leftHandTransform.rotation;
			val.AddComponent<GorillaSurfaceOverride>().overrideIndex = over[4];
			val.GetComponent<Renderer>().material.color = Color.cyan;
			Object.Destroy((Object)(object)val.GetComponent<Collider>(), 0.28f);
			Object.Destroy((Object)(object)val, 0.3f);
		}
		if (WristMenu.gripDownR)
		{
			GameObject val2 = CreatePrimitive((PrimitiveType)3);
			val2.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
			val2.transform.localPosition = OwnRig().rightHandTransform.position + new Vector3(0f, -0.05f, 0f);
			val2.transform.rotation = OwnRig().rightHandTransform.rotation;
			val2.AddComponent<GorillaSurfaceOverride>().overrideIndex = over[4];
			val2.GetComponent<Renderer>().material.color = Color.cyan;
			Object.Destroy((Object)(object)val2.GetComponent<Collider>(), 0.28f);
			Object.Destroy((Object)(object)val2, 0.3f);
		}
	}

	public static void UpnDown(Vector3 p)
	{
		Vector3 val = p * Time.deltaTime * 50f;
		Rigidbody component = ((Component)Loco()).GetComponent<Rigidbody>();
		if (WristMenu.triggerDownR)
		{
			component.linearVelocity += val;
		}
		if (WristMenu.triggerDownL)
		{
			component.linearVelocity -= val;
		}
	}

	public static void WallWalk()
	{
		bool flag = WristMenu.gripDownR;
		Transform val = Loco().RightHand.controllerTransform;
		if (change40 == 1)
		{
			flag = WristMenu.gripDownL;
			val = Loco().LeftHand.controllerTransform;
		}
		if (change40 == 2)
		{
			flag = WristMenu.gripDownR || WristMenu.gripDownL;
			val = (WristMenu.gripDownR ? Loco().RightHand.controllerTransform : Loco().LeftHand.controllerTransform);
		}
		if (flag)
		{
			RaycastHit val2 = default;
			Physics.Raycast(val.position, -val.up, ref val2);
			if (val2.distance < 2f)
			{
				Rigidbody attachedRigidbody = ((Collider)Loco().bodyCollider).attachedRigidbody;
				attachedRigidbody.linearVelocity -= val2.normal * (Walk * Time.deltaTime);
			}
		}
	}

	public static void PunchMod()
	{
		foreach (VRRig item in RigUtils.AllRigs(i: false))
		{
			GameObject[] array = new GameObject[2]
			{
				CreatePrimitive((PrimitiveType)0),
				CreatePrimitive((PrimitiveType)0)
			};
			GameObject[] array2 = array;
			foreach (GameObject val in array2)
			{
				Object.Destroy((Object)(object)val.GetComponent<SphereCollider>());
				val.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				val.GetComponent<Renderer>().material.color = Rainbow();
			}
			array[0].transform.position = item.leftHandTransform.position;
			array[1].transform.position = item.rightHandTransform.position;
			Object.Destroy((Object)(object)array[0], Time.deltaTime);
			Object.Destroy((Object)(object)array[1], Time.deltaTime);
			OnTouchMethod(item, o: true, () =>
			{
				((Component)Loco()).GetComponent<Rigidbody>().linearVelocity = -((Component)OwnRig()).transform.forward * 15f;
			});
		}
	}

	public static void ChangeSpeed()
	{
		if (WristMenu.triggerDownR && !WristMenu.triggerDownL)
		{
			new Delay().D(0.09f, () =>
			{
				if (customspeed < 16f)
				{
					customspeed += 0.1f;
					NotifiLib.ClearPast(1);
					SendNoti("[<color=blue>CUSTOM SPEED</color>] Current Speed: " + customspeed);
				}
			});
		}
		else
		{
			if (!WristMenu.triggerDownL || WristMenu.triggerDownR)
			{
				return;
			}
			new Delay().D(0.09f, () =>
			{
				if ((customspeed <= 16f || customspeed <= 17f) && customspeed > 0f)
				{
					customspeed -= 0.1f;
					NotifiLib.ClearPast(1);
					SendNoti("[<color=blue>CUSTOM SPEED</color>] Current Speed: " + customspeed);
				}
			});
		}
	}

	public static void SpawnPoint()
	{
		List<AlarmClockData> list = (from e in Create(AlarmClockManager.Instance).Field(Field(71)).GetValue<AlarmClockData[]>()
			where e.Key != "gtfc"
			select e).ToList();
		spawn++;
		if (spawn > list.Count - 1)
		{
			spawn = 0;
		}
		string key = list[spawn].Key;
		Create(AlarmClockManager.Instance).Field(Field(72)).SetValue((object)key);
		GorillaTelemetry.EnqueueTelemetryEvent(Field(73), (object)new Dictionary<string, object>
		{
			{
				Field(76),
				"set"
			},
			{
				Field(75),
				key
			}
		}, (string[])null);
		PlayerPrefs.SetString(Field(74), key);
		NewName("Set Alarm Clock: " + key.ToUpper());
	}

	public static void CustomSpam()
	{
		if (sou == -1)
		{
			List<ButtonInfo> list = new List<ButtonInfo>();
			List<MaterialData> v = Loco().materialData;
			int i;
			for (i = 0; i < v.Count; i++)
			{
				if (d.Values.Any((int e) => e == i) || v[i].matName.ToLower().EndsWith("pile") || v[i].matName.ToLower().EndsWith("grabzone"))
				{
					continue;
				}
				string n = v[i].matName;
				list.Insert(list.Count, new ButtonInfo
				{
					buttonText = v[i].matName,
					method = () =>
					{
						sou = v.FindIndex((MaterialData e) =>
						{
							return e.matName == n;
						});
					},
					enabled = false
				});
			}
			list.ForEach((ButtonInfo e) =>
			{
				ButtonInfo buttonInfo = e;
				buttonInfo.method = (Action)Delegate.Combine(buttonInfo.method, (Action)(() =>
				{
					DisableButton(e.buttonText);
					SendNoti("Now hold your right grip to spam that sound!");
					DeleteBoard();
				}));
			});
			SpawnBoard(list, "ed", 30, "Find A Sound And Click It|Custom Sound Spam");
		}
		else
		{
			Sound(sou);
		}
	}

	public static void LaunchProj(Vector3 po, Vector3 v, string s, float mu = 1f, bool f = false)
	{
		bool flag = prr == 2 && WristMenu.gripDownR && WristMenu.gripDownL;
		SnowballThrowable g = null;
		SnowballThrowable j = null;
		new Delay().D(flag ? 0.7f : 0.4f, () =>
		{
			if (WristMenu.gripDownR && (prr == 2 || prr == 0))
			{
				g = GetSnow(f ? "Growing Snowball" : s, (prr != 2 && mu != 30f) ? (-1) : 0);
			}
		});
		if (po != Vector3.zero)
		{
			if (Vector3.Distance(po, ((Component)OwnRig()).transform.position) > 3f)
			{
				((Behaviour)OwnRig()).enabled = false;
				((Component)OwnRig()).transform.position = (((Object)(object)lockedrig == (Object)null) ? po : ((Component)lockedrig).transform.position) + new Vector3(0f, 1f, 0f);
				DrawHandOrbs();
			}
			if (!((Behaviour)OwnRig()).enabled)
			{
				DrawHandOrbs();
				((Component)OwnRig()).transform.position = (((Object)(object)lockedrig == (Object)null) ? po : ((Component)lockedrig).transform.position) + new Vector3(0f, 1f, 0f);
			}
		}
		if (mu != 30f && WristMenu.gripDownL && (prr == 1 || prr == 2))
		{
			new Delay().D(flag ? 0.7f : 0.4f, () =>
			{
				j = GetSnow(s, 1);
			});
		}
		if ((Object)(object)g != (Object)null)
		{
			ThrowProj(g, po, (mu > 1f && mu != 30f) ? (-OwnRig().rightHandTransform.up) : v, mu, f);
		}
		if ((Object)(object)j != (Object)null)
		{
			ThrowProj(j, po, (mu > 1f) ? (-OwnRig().leftHandTransform.up) : v, mu, f);
		}
	}

	public static void ThrowProj(SnowballThrowable g, Vector3 po, Vector3 v, float mu, bool f)
	{
		bool flag = v != Vector3.zero;
		GTPlayer val = Loco();
		Rigidbody rigidbody = GIn().rigidbody;
		if ((ogp == Vector3.zero) & flag)
		{
			Vector3 linearVelocity = rigidbody.linearVelocity;
			ogv = linearVelocity.magnitude;
			linearVelocity = rigidbody.linearVelocity;
			Vector3 val2;
			if (!(linearVelocity.sqrMagnitude > 1E-06f))
			{
				val2 = ((Component)GIn()).transform.forward;
			}
			else
			{
				linearVelocity = rigidbody.linearVelocity;
				val2 = linearVelocity.normalized;
			}
			ogd = val2;
			ogp = BC().position;
		}
		g.maxLinSpeed = float.MaxValue;
		if (f && !IsEnabled("Auto Big Snowballs"))
		{
			AutoBigSnowballs();
		}
		if (flag)
		{
			rigidbody.linearVelocity = v.normalized * mu;
		}
		if (po != Vector3.zero)
		{
			((Component)g).gameObject.transform.position = po;
		}
		try
		{
			((HoldableObject)g).DropItemCleanup();
			if (!(ogp != Vector3.zero))
			{
				return;
			}
			if (!f || !((Behaviour)OwnRig()).enabled)
			{
				try
				{
					val.TeleportTo(W(ogp), ((Component)val).transform.rotation, false, false);
				}
				catch
				{
				}
			}
			rigidbody.linearVelocity = ogd * ogv;
			ogv = -1f;
			ogp = Vector3.zero;
		}
		catch
		{
		}
	}

	public static SnowballThrowable GetSnow(string na, int i = -1)
	{
		if (CosDone())
		{
			SnowballThrowable val = null;
			try
			{
				Ha((i != -1) ? (i == 0) : (prr == 0)).TryGetComponent<SnowballThrowable>(ref val);
			}
			catch
			{
				val = null;
			}
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
			SnowballMaker val2 = (((i != -1) ? (i == 0) : (prr == 0)) ? SnowballMaker.rightHandInstance : SnowballMaker.leftHandInstance);
			val2.TryCreateSnowball(d[na], ref val);
		}
		else
		{
			DelayedNoti(Er + "You Must Be Unbanned!");
		}
		return null;
	}

	public static void ChangeProj()
	{
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (string k in d.Keys)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k,
				method = () =>
				{
					curprojb = k;
					DisableButton(k);
					Rebs("Change Current Projectile: " + curprojb);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Projectile And Click It|Change Current Projectile");
	}

	public static void SpamCosmetics()
	{
		if (CosDone())
		{
			List<CosmeticItem> c2 = Coscat;
			if (Coscat.Count > 0)
			{
				new Delay().D(SpamDelay, () =>
				{
					cosspam++;
					if (cosspam > c2.Count)
					{
						cosspam = 0;
					}
					Con().PressWardrobeItemButton(c2[cosspam], false, false);
				});
			}
			else
			{
				SendNoti(Er + "You Must Select A Cosmetic Spam Category!");
				DisableButton("Spam Cosmetics");
			}
		}
		else
		{
			SendNoti(Er + "Cosmetics Have Not Yet Been Initialized!");
			DisableButton("Spam Cosmetics");
		}
	}

	public static void RanCos()
	{
		if (CosDone())
		{
			CosmeticsController val = Con();
			if (val.unlockedCosmetics.Count > 0)
			{
				List<CosmeticItem>[] array = new List<CosmeticItem>[10] { val.unlockedArms, val.unlockedBacks, val.unlockedBadges, val.unlockedChests, val.unlockedFaces, val.unlockedFurs, val.unlockedHats, val.unlockedPants, val.unlockedPaws, val.unlockedShirts };
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Count > 0)
					{
						val.PressWardrobeItemButton(array[i][Random.Range(0, array[i].Count)], false, false);
					}
				}
				DisableButton("Randomize Cosmetics");
			}
			else
			{
				SendNoti(Er + "You Dont Have Any Cosmetics!");
				DisableButton("Randomize Cosmetics");
			}
		}
		else
		{
			SendNoti(Er + "Cosmetics Have Not Yet Been Initialized!");
			DisableButton("Randomize Cosmetics");
		}
	}

	public static void ChangeCosSpamDelay()
	{
		sd++;
		if (sd > 4)
		{
			sd = 0;
		}
		float[] array = new float[4] { 0.5f, 0.07f, 0.03f, 0.8f };
		string[] array2 = new string[4] { "Normal", "Fast", "Super Fast", "Slow" };
		SpamDelay = array[sd];
		NewName("Change Cosmetic Spam Delay: " + array2[sd]);
	}

	public static void ChangeCosCat()
	{
		if (WristMenu.customboards || CosDone())
		{
			CosmeticsController val = Con();
			List<CosmeticItem>[] array = new List<CosmeticItem>[11]
			{
				val.unlockedArms, val.unlockedBacks, val.unlockedBadges, val.unlockedChests, val.unlockedFaces, val.unlockedFurs, val.unlockedHats, val.unlockedPants, val.unlockedPaws, val.unlockedShirts,
				val.unlockedCosmetics
			};
			if (array[10].Count > 0)
			{
				List<List<CosmeticItem>> list = new List<List<CosmeticItem>>();
				List<string> list2 = new List<string>();
				try
				{
					if (array[0].Count > 0)
					{
						list.Add(array[0]);
						list2.Add("Arms");
					}
				}
				catch
				{
				}
				try
				{
					if (array[1].Count > 0)
					{
						list.Add(array[1]);
						list2.Add("Back");
					}
				}
				catch
				{
				}
				try
				{
					if (array[2].Count > 0)
					{
						list.Add(array[2]);
						list2.Add("Badges");
					}
				}
				catch
				{
				}
				try
				{
					if (array[3].Count > 0)
					{
						list.Add(array[3]);
						list2.Add("Chest");
					}
				}
				catch
				{
				}
				try
				{
					if (array[4].Count > 0)
					{
						list.Add(array[4]);
						list2.Add("Face");
					}
				}
				catch
				{
				}
				try
				{
					if (array[5].Count > 0)
					{
						list.Add(array[5]);
						list2.Add("Furs");
					}
				}
				catch
				{
				}
				try
				{
					if (array[6].Count > 0)
					{
						list.Add(array[6]);
						list2.Add("Hats");
					}
				}
				catch
				{
				}
				try
				{
					if (array[7].Count > 0)
					{
						list.Add(array[7]);
						list2.Add("Pants");
					}
				}
				catch
				{
				}
				try
				{
					if (array[8].Count > 0)
					{
						list.Add(array[8]);
						list2.Add("Paws");
					}
				}
				catch
				{
				}
				try
				{
					if (array[9].Count > 0)
					{
						list.Add(array[9]);
						list2.Add("Shirts");
					}
				}
				catch
				{
				}
				try
				{
					list.Add(array[10]);
					list2.Add("All");
				}
				catch
				{
				}
				coi++;
				if (coi > list.Count)
				{
					coi = 0;
				}
				Coscat = list[coi];
				NewName("Change Cosmetic Spam Category: " + list2[coi]);
			}
			else
			{
				SendNoti(Er + "You Have No Cosmetics!");
				DisableButton("Change Cosmetic Spam Category");
			}
		}
		else
		{
			SendNoti(Er + "Cosmetics Have Not Yet Been Initialized!");
			DisableButton("Change Cosmetic Spam Category");
		}
	}

	public static void BlasterSpam(Vector3 p, object r, bool g = false)
	{
		if (!((WristMenu.triggerDownR || WristMenu.triggerDownL) | g))
		{
			return;
		}
		Quaternion q = (Quaternion)((r.GetType() == typeof(Quaternion)) ? ((Quaternion)r) : Quaternion.LookRotation((Vector3)r));
		WristMenu.UBB.e = true;
		SIGadgetCooldownBlaster s = (from i in Mods.F<SIGadgetCooldownBlaster>(5f)
			where ((SIGadget)Create(i).Field(Field(22)).GetValue<SIGadgetBlaster>()).gameEntity.AttachedPlayerActorNr == Local().ActorNumber
			select i).FirstOrDefault();
		if ((Object)(object)s != (Object)null)
		{
			SIGadgetBlaster b2 = Create(s).Field(Field(22)).GetValue<SIGadgetBlaster>();
			new Delay().D(0.077f, () =>
			{
				bool flag = default;
				((SIGadget)b2).FindAttachedHand(ref flag);
				if (flag ? (WristMenu.triggerDownL | g) : (WristMenu.triggerDownR | g))
				{
					s.FireProjectile(b2.NextFireId(), p, q);
				}
			});
		}
		else
		{
			SIGadgetChargeBlaster ss = (from i in Mods.F<SIGadgetChargeBlaster>(5f)
				where ((SIGadget)Create(i).Field(Field(22)).GetValue<SIGadgetBlaster>()).gameEntity.AttachedPlayerActorNr == Local().ActorNumber
				select i).FirstOrDefault();
			if ((Object)(object)ss != (Object)null)
			{
				SIGadgetBlaster b3 = Create(ss).Field(Field(22)).GetValue<SIGadgetBlaster>();
				new Delay().D(0.077f, () =>
				{
					bool flag = default;
					((SIGadget)b3).FindAttachedHand(ref flag);
					if (flag ? (WristMenu.triggerDownL | g) : (WristMenu.triggerDownR | g))
					{
						ss.FireProjectile(40f, b3.NextFireId(), p, q);
					}
				});
			}
			else
			{
				DelayedNoti(Er + "You Must Have A Blaster Equppied!");
			}
		}
		new Delay().D(2f, () =>
		{
			RPCFlush();
		});
	}

	public static void TentacleChanger()
	{
		SIGadgetTentacleArm[] array = (from i in Mods.F<SIGadgetTentacleArm>(5f)
			where ((SIGadget)i).gameEntity.IsSnappedByLocalPlayer()
			select i).ToArray();
		SIGadgetTentacleArm[] array2 = array;
		bool flag = default;
		foreach (SIGadgetTentacleArm val in array2)
		{
			((SIGadget)val).FindAttachedHand(ref flag);
			if (flag)
			{
				if (WristMenu.gripDownL)
				{
					val.LengthFactor += 0.1f;
				}
				if (WristMenu.xbuttonDown)
				{
					val.LengthFactor -= 0.1f;
				}
				Create(val).Field(Field(37)).SetValue((object)(val.LengthFactor + 3.4f));
			}
			else
			{
				if (WristMenu.gripDownR)
				{
					val.LengthFactor += 0.1f;
				}
				if (WristMenu.abuttonDown)
				{
					val.LengthFactor -= 0.1f;
				}
				Create(val).Field(Field(37)).SetValue((object)(val.LengthFactor + 3.4f));
			}
		}
	}

	public static void DCr()
	{
		MakeGun(l: false, () =>
		{
			if ((Object)(object)((Component)rayCur.ReflectionEnums).GetComponentInParent<GameEntity>() != (Object)null)
			{
				GameEntity e = ((Component)rayCur.ReflectionEnums).GetComponentInParent<GameEntity>();
				Transform transform = ((Component)e).gameObject.transform;
				if (Local().IsMasterClient)
				{
					new Delay().D(0.088f, () =>
					{
						e.manager.photonView.RPC(RPC(4), (RpcTarget)0, new object[1] { new int[1] { e.GetNetId() } });
					});
				}
				else
				{
					SetSP();
					if (e.pickupable && e.snappedByActorNumber == -1 && !e.IsHeld())
					{
						if (Vector3.Distance(transform.position, SP) < 7f)
						{
							e.manager.RequestGrabEntity(e.id, true, Vector3.zero, Quaternion.identity);
							e.manager.RequestThrowEntity(e.id, true, BC().position + new Vector3(0f, -5f, 0f), Vector3.zero, Vector3.zero);
						}
						else
						{
							((Behaviour)OwnRig()).enabled = false;
							((Component)OwnRig()).transform.position = transform.position + new Vector3(0f, -2f, 0f);
						}
					}
					else
					{
						DelayedNoti(Er + "You Can Not Destroy This Item!");
					}
				}
			}
			else
			{
				((Behaviour)OwnRig()).enabled = true;
			}
		}, () =>
		{
			((Behaviour)OwnRig()).enabled = true;
		});
	}

	public static void ItemElli()
	{
		MakeGun(l: false, () =>
		{
			new Delay().D(2.2f, () =>
			{
				bre++;
				if (bre > 3)
				{
					bre = 1;
				}
			});
			if (ogpp == Vector3.zero)
			{
				ogpp = rayCur.point + new Vector3(0f, 1f, 0f);
			}
			Vector3 val = Vector3.zero;
			Quaternion val2 = Quaternion.identity;
			if (bre == 1)
			{
				val = new Delay().Orbit("elli", ogpp, 0.8f, 0f, 0.8f, 3f);
				val2 = Quaternion.LookRotation(ogpp - val);
			}
			if (bre == 2)
			{
				val = new Delay().Orbit("elli", ogpp, 0.9f, 0.8f, 1f, 3f, new Dictionary<string, float>
				{
					{
						"x",
						Mathf.Cos(da["elli"])
					},
					{
						"y",
						Mathf.Sin(da["elli"])
					},
					{ "z", 0f }
				});
				val2 = Quaternion.LookRotation(Vector3.forward, ogpp - val);
			}
			if (bre == 3)
			{
				val = new Delay().Orbit("elli", ogpp, 0f, 0.8f, 0.9f, 3f, new Dictionary<string, float>
				{
					{ "x", 0f },
					{
						"y",
						Mathf.Cos(da["elli"])
					},
					{
						"z",
						Mathf.Sin(da["elli"])
					}
				});
				val2 = Quaternion.LookRotation(Vector3.right, ogpp - val);
			}
			Crr(val, Vector3.zero, new Quaternion[1] { val2 }, l: true);
		}, () =>
		{
			bre = 0;
			ogpp = Vector3.zero;
		});
	}

	public static bool InGR(VRRig r)
	{
		return (int)r.zoneEntity.currentZone == 24 || (int)r.zoneEntity.currentZone == 28;
	}

	public static void DAC(int br = 0)
	{
		new Delay().D(0.1f, () =>
		{
			List<int> list = new List<int>();
			GameEntityManager managerForZone = GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone);
			if (((NetworkView)managerForZone).IsMine)
			{
				List<GameEntity> list2 = Mods.F<GameEntity>(0.09f).ToList();
				if (br == 1)
				{
					list2.RemoveAll((GameEntity e) => e.typeId != 1517967647);
				}
				if (br == 2)
				{
					list2.RemoveAll((GameEntity e) => e.typeId != 1551901997);
				}
				if (list2.Any((GameEntity e) => e.IsScenePlaced))
				{
					list2.RemoveAll((GameEntity e) => e.IsScenePlaced);
				}
				list2.RemoveAll((GameEntity e) => e.typeId == 931983585);
				foreach (GameEntity item in list2)
				{
					((Component)item).gameObject.SetActive(true);
					int netId = item.GetNetId();
					if (!list.Contains(netId))
					{
						list.Add(netId);
					}
				}
				if (list.Count <= 0)
				{
					string name = ((br == 1) ? "Fix Movement All" : ((br == 2) ? "Fix Audio All" : "Destroy All Items"));
					DisableButton(name);
				}
				else
				{
					int[] array = new int[0];
					int num = 60;
					for (int num2 = 0; num2 < list.Count; num2 += num)
					{
						int num3 = Mathf.Min(num, list.Count - num2);
						array = new int[num3];
						list.CopyTo(num2, array, 0, num3);
					}
					managerForZone.photonView.RPC(RPC(4), (RpcTarget)0, new object[1] { array });
				}
			}
			else
			{
				Ow(managerForZone);
			}
		});
	}

	public static async void Ow(GameEntityManager g)
	{
		if (Local().IsMasterClient)
		{
			new Delay().D(2f, () =>
			{
				g.guard.RequestOwnershipImmediatelyWithGuaranteedAuthority();
			});
		}
		else if (RigUtils.AllRigs(i: false).Where((VRRig e) =>
		{
			return e.zoneEntity.currentZone == g.zone;
		}).ToList()
			.Count == 0)
		{
			new Delay().D(3f, () =>
			{
				g.guard.RequestOwnership((Action)null, (Action)null);
			});
		}
		else
		{
			DelayedNoti(Er + "Since You Are Not Master, You Must Be The 1st In This Map!");
		}
	}

	public static async void Breakm(object o, bool b = false)
	{
		if ((from e in Mods.F<GameEntity>(0.09f)
			where e.typeId == (b ? 1551901997 : 1517967647)
			select e).ToList().Count >= 100)
		{
			DAC((!b) ? 1 : 2);
		}
		Quaternion[] v = Enumerable.Repeat<Quaternion>(Quaternion.Euler(90f, 0f, 0f), 102).ToArray();
		int[] i = Enumerable.Repeat(b ? 1551901997 : 1517967647, 102).ToArray();
		if (o is List<Player> l)
		{
			if (l.Where((Player e) => InGR(RigUtils.RigFromPlayer(e))).ToList().Count == 0)
			{
				SendNoti(Er + "People Must Be In GhostReactor");
				return;
			}
			Crr(Enumerable.Repeat<Vector3>(Vector3.zero, 102).ToArray(), Vector3.zero, v, l: true, i, null, l.Select((Player e) => e.ActorNumber).ToArray());
		}
		VRRig r = (VRRig)((o is VRRig) ? o : null);
		if (r != null)
		{
			if (!InGR(r))
			{
				DelayedNoti(Er + "This Player Must Be In GhostReactor!");
				return;
			}
			Crr(Enumerable.Repeat<Vector3>(((Component)r).transform.position, 102).ToArray(), Vector3.zero, v, l: true, i, GameEntityManager.GetManagerForZone(r.zoneEntity.currentZone), new int[2]
			{
				r.Creator.GetPlayerRef().ActorNumber,
				Local().ActorNumber
			});
		}
	}

	public static void ChangeItem()
	{
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<int, object[]> k in collectibles)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = (string)k.Value[1],
				method = () =>
				{
					curcol = (int)k.Value[2];
					collect = (string)k.Value[0];
					DisableButton((string)k.Value[1]);
					Rebs("Change Item: " + (string)k.Value[1]);
				},
				enabled = false
			});
		}
		foreach (KeyValuePair<int, GameObject> k2 in from e in Create(GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone)).Field(Field(84)).GetValue<Dictionary<int, GameObject>>()
			where !Enex.Any((string r) => ((Object)e.Value).name.Contains(r))
			select e)
		{
			string n = ((Object)k2.Value).name.Replace("GhostReactor", "GhostReactor ").Replace("_", " ").Replace("GRU", "Upgrade ")
				.Replace("Collectible", "")
				.Replace("Monkeye01", "Monkeye");
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = n,
				method = () =>
				{
					curcol = k2.Key;
					collect = ((Object)k2.Value).name;
					DisableButton(n);
					Rebs("Change Item: " + n);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Item And Click It|Change Item");
	}

	public static void InstaSphere()
	{
		MakeGun(l: false, () =>
		{
			if (ogpp == Vector3.zero)
			{
				ogpp = rayCur.point + new Vector3(0f, 0.9f, 0f);
			}
			List<Quaternion> list = new List<Quaternion>();
			List<Vector3> list2 = new List<Vector3>();
			for (int i = 0; i < 102; i++)
			{
				Vector3 val = Sphere(ogpp, 0.85f);
				list2.Add(val);
				list.Add(Quaternion.LookRotation(ogpp - val));
			}
			Crr(list2.ToArray(), Vector3.zero, list.ToArray(), l: true, Enumerable.Repeat(curcol, 102).ToArray());
		}, () =>
		{
			ogpp = Vector3.zero;
		});
	}

	public static bool CanSpawn(int t, Vector3 p, GameEntityManager g, bool n = true, long cd = 0L, bool c = true)
	{
		List<IGameEntityZoneComponent> value = Create(g).Field(Field(86)).GetValue<List<IGameEntityZoneComponent>>();
		bool result = true;
		Dictionary<int, int> value2 = Create(g).Field(Field(34)).GetValue<Dictionary<int, int>>();
		value2.TryGetValue(t, out var value3);
		for (int i = 0; i < value.Count; i++)
		{
			bool flag = g.IsPositionInManagerBounds(p);
			bool flag2 = c && value3 >= 101;
			if ((!value[i].ValidateCreateItem(CreateNetId(1 + g.FactoryGetBuiltInEntityCountById(t), g, s: false), t, p, Quaternion.identity, cd, g.GetNetIdFromEntityId(GameEntityId.Invalid)) || !flag) | flag2)
			{
				if (n)
				{
					string er = Er;
					string text;
					if (flag2)
					{
						text = "Maxium Amount Of \"" + (string)GetCol(t)[1] + "'s\" Reached!";
					}
					else if (flag)
					{
						text = ((cd != 0L) ? "You Dont Have This Gadget Unlocked!" : "Cant Spawn This Item Here!");
					}
					else
					{
						text = "Cant Spawn This Item That Far Away!";
					}
					DelayedNoti(er + text);
				}
				result = false;
			}
		}
		return result;
	}

	public static void Crr(object pp, Vector3 v, object rr, bool l = false, int[] ss = null, GameEntityManager h = null, object t = null)
	{
		if (!PhotonNetwork.InRoom || !(WristMenu.gripDownR | l))
		{
			return;
		}
		int s = -1;
		if (ss != null && ss.Length == 1)
		{
			s = ss[0];
		}
		object p = Vector3.zero;
		if (pp is Vector3 val)
		{
			p = val;
		}
		if (pp is Vector3[] array)
		{
			p = array;
		}
		int i;
		if (s == -70)
		{
			i = 618507822;
		}
		else if (s == -69)
		{
			i = 46876848;
		}
		else if (s == -71)
		{
			i = 618507822;
		}
		else
		{
			i = ((s == -1) ? curcol : s);
		}
		object[] o = GetCol(i);
		GameEntityManager g = h ?? GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone);
		bool flag = true;
		if ((string)o[0] != "")
		{
			Vector3 val2 = Vector3.zero;
			if (p is Vector3[] array2)
			{
				val2 = array2[0];
			}
			if (p is Vector3)
			{
				Vector3 val3 = (Vector3)p;
				if (true)
				{
					val2 = val3;
				}
			}
			flag = CanSpawn(i, (val2 == Vector3.zero) ? ((Component)OwnRig()).transform.position : val2, g, n: true, (long)o[3]);
		}
		if (!flag && !((string)o[0] == ""))
		{
			return;
		}
		if (((NetworkView)g).IsMine)
		{
			object i2 = (int)o[2];
			bool b2 = v == Vector3.zero;
			if (ss != null && ss.Length > 1)
			{
				i2 = ss;
			}
			if (s == -71)
			{
				i2 = Enumerable.Repeat(618507822, 102).ToArray();
				rr = Enumerable.Repeat<Quaternion>(Quaternion.identity, 102).ToArray();
				p = Enumerable.Repeat<Vector3>(((Component)OwnRig()).transform.position, 102).ToArray();
			}
			new Delay().D((!b2) ? 0.141f : 0.1f, () =>
			{
				if ((string)o[0] == "")
				{
					Dictionary<int, int> value = Create(g).Field(Field(34)).GetValue<Dictionary<int, int>>();
					List<int> list = new List<int>();
					Vector3 rp = Vector3.zero;
					if (p is Vector3[] array3)
					{
						rp = array3[0];
					}
					if (p is Vector3)
					{
						Vector3 val4 = (Vector3)p;
						if (true)
						{
							rp = val4;
						}
					}
					List<object[]> list2 = RanCol().Values.Where((object[] e) =>
					{
						return CanSpawn((int)e[2], rp, g, n: false, (long)e[3], c: false) && !((string)e[0]).Contains("EnemyMoon") && (InGR(OwnRig()) || (long)e[3] == 0L) && (string)e[0] != "";
					}).ToList();
					if (!b2)
					{
						list2.RemoveAll((object[] e) => !Pickupable((int)e[2], g));
					}
					foreach (object[] item in list2)
					{
						int num = (int)item[2];
						value.TryGetValue(num, out var value2);
						if (value2 < 101)
						{
							list.Add(num);
						}
					}
					if (list.Count <= 0)
					{
						DelayedNoti(Er + "Maxium Items Reached!");
						return;
					}
					if (p is Vector3[])
					{
						List<int> list3 = new List<int>();
						for (int num2 = 0; num2 < list.Count; num2++)
						{
							list3.Add(list[Random.Range(0, list.Count)]);
						}
						i2 = list3.ToArray();
					}
					else
					{
						i2 = list[Random.Range(0, list.Count)];
					}
				}
				if (i2 is int)
				{
					int i3 = (int)i2;
					if (true && !b2 && !Pickupable(i3, g))
					{
						DelayedNoti(Er + "This Item Can Not Be Grabbed!");
						return;
					}
				}
				Cr(b2 ? p : ((object)(((Component)OwnRig()).transform.position + new Vector3(0f, 0.4f, 0f))), rr, i2, t ?? ((object)(RpcTarget)0), g);
				if (s == -69 || s == -70 || !b2)
				{
					GameEntity gameEntityFromNetId = g.GetGameEntityFromNetId(cmdId);
					if ((Object)(object)gameEntityFromNetId != (Object)null && gameEntityFromNetId.heldByActorNumber != Local().ActorNumber)
					{
						if (s != -69 && s != -70)
						{
							g.RequestGrabEntity(gameEntityFromNetId.id, false, Vector3.zero, Quaternion.identity);
							g.RequestThrowEntity(gameEntityFromNetId.id, false, (Vector3)p, v, v);
						}
						else
						{
							g.photonView.RPC(RPC(4), (RpcTarget)0, new object[1] { new int[1] { cmdId } });
						}
					}
				}
			});
		}
		else
		{
			Ow(g);
		}
	}

	public static async void Cr(object pp, object rr, object id, object q = null, GameEntityManager s = null)
	{
		GameEntityManager m = s ?? GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone);
		object t = q ?? ((object)(RpcTarget)0);
		int iv = m.GetNetIdFromEntityId(GameEntityId.Invalid);
		object[] w = new object[0];
		Quaternion r = Quaternion.identity;
		Quaternion[] qw = new Quaternion[0];
		if (rr is Quaternion[] gg)
		{
			qw = gg;
		}
		Quaternion qt = default;
		int num;
		if (rr is Quaternion)
		{
			qt = (Quaternion)rr;
			num = 1;
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			r = qt;
		}
		Vector3 p = Vector3.zero;
		Vector3[] vw = new Vector3[0];
		if (pp is Vector3[] vv)
		{
			vw = vv;
		}
		Vector3 vt = default;
		int num2;
		if (pp is Vector3)
		{
			vt = (Vector3)pp;
			num2 = 1;
		}
		else
		{
			num2 = 0;
		}
		if (num2 != 0)
		{
			p = vt;
		}
		int ii = default;
		int num3;
		if (id is int)
		{
			ii = (int)id;
			num3 = 1;
		}
		else
		{
			num3 = 0;
		}
		if (num3 != 0)
		{
			w = new object[6]
			{
				new int[1] { CreateNetId(1 + m.FactoryGetBuiltInEntityCountById(ii), m) },
				new int[1] { ii },
				new long[1] { BitPackUtils.PackWorldPosForNetwork(p) },
				new int[1] { BitPackUtils.PackQuaternionForNetwork(r) },
				new long[1] { (long)GetCol(ii)[3] },
				new int[1] { iv }
			};
		}
		if (id is int[] ig)
		{
			int n = ig.Length;
			List<int> ids = new List<int>();
			List<int> ns = new List<int>();
			List<long> cds = new List<long>();
			List<int> ro = new List<int>();
			List<long> po = new List<long>();
			for (int io = 0; io < n; io++)
			{
				ids.Add(ig[io]);
				ns.Add(CreateNetId(io + m.FactoryGetBuiltInEntityCountById(ig[io]), m));
				cds.Add((long)GetCol(ig[io])[3]);
				ro.Add(BitPackUtils.PackQuaternionForNetwork(qw[io]));
				po.Add(BitPackUtils.PackWorldPosForNetwork(vw[io]));
			}
			w = new object[6]
			{
				ns.ToArray(),
				ids.ToArray(),
				po.ToArray().ToArray(),
				ro.ToArray(),
				cds.ToArray(),
				Enumerable.Repeat(iv, n).ToArray()
			};
		}
		cmdId = ((int[])w[0])[0];
		RpcTarget e = default;
		int num4;
		if (t is RpcTarget)
		{
			e = (RpcTarget)t;
			num4 = 1;
		}
		else
		{
			num4 = 0;
		}
		if (num4 != 0)
		{
			SendCr(m, e, w);
		}
		Player k = (Player)((t is Player) ? t : null);
		if (k != null)
		{
			SendCr(m, k, w);
		}
		VRRig vr = (VRRig)((t is VRRig) ? t : null);
		if (vr != null)
		{
			long lp = BitPackUtils.PackWorldPosForNetwork(((Component)vr).transform.position);
			w[2] = new long[1] { lp };
			if (id is int[] le)
			{
				w[2] = Enumerable.Repeat(lp, le.Length).ToArray();
			}
			SendCr(m, vr, w);
		}
		if (t is int[] y)
		{
			bool je = ((int[])w[1])[0] == 1551901997;
			long lp2 = BitPackUtils.PackWorldPosForNetwork(((Component)RigUtils.RigFromPlayer(RigUtils.PlayerFromActor(y[0]))).transform.position - new Vector3(0f, je ? 4f : 0f, 0f));
			w[2] = new long[1] { lp2 };
			if (id is int[] le2)
			{
				w[2] = Enumerable.Repeat(lp2, le2.Length).ToArray();
			}
			SendCr(m, y, w);
			await Task.Delay(300);
			List<GameEntity> gh = new List<GameEntity>();
			if (id is int[] lg)
			{
				GameEntity[] array = Mods.F<GameEntity>(0.09f);
				foreach (GameEntity ge in array)
				{
					int[] array2 = lg;
					foreach (int ie in array2)
					{
						if (ge.typeId == ie)
						{
							gh.Add(ge);
						}
					}
				}
			}
			int kl = default;
			int num5;
			if (id is int)
			{
				kl = (int)id;
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			if (num5 != 0)
			{
				gh = (from ee in Mods.F<GameEntity>(0.09f)
					where ee.typeId == kl
					select ee).ToList();
			}
			gh.ForEach((GameEntity ee) =>
			{
				((Component)ee).gameObject.SetActive(false);
			});
		}
		new Delay().D(1.3f, () =>
		{
			RPCFlush();
		});
	}

	public static int CreateNetId(int n, GameEntityManager g, bool s = true)
	{
		int num = Create(g).Field(Field(85)).GetValue<int>();
		if (s)
		{
			Create(g).Field(Field(85)).SetValue((object)(num += n));
		}
		return num;
	}

	public static object[] Half(object[] o, bool f = true)
	{
		return ((IEnumerable<object>)o).Select((Func<object, object>)((object x) =>
		{
			Array array = (Array)x;
			Array array2 = Array.CreateInstance(array.GetType().GetElementType(), array.Length / 2);
			if (f)
			{
				Array.Copy(array, array2, array2.Length);
			}
			else
			{
				Array.Copy(array, array.Length - array2.Length, array2, 0, array2.Length);
			}
			return array2;
		})).ToArray();
	}

	public static async void SendCr(GameEntityManager g, object t, object[] o)
	{
		bool b2 = ((int[])o[0]).Length > 45;
		PhotonView p = g.photonView;
		RpcTarget e = default;
		int num;
		if (t is RpcTarget)
		{
			e = (RpcTarget)t;
			num = 1;
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			if (b2)
			{
				p.RPC(RPC(7), e, Half(o));
				await Task.Delay(75);
				p.RPC(RPC(7), e, Half(o, f: false));
			}
			else
			{
				p.RPC(RPC(7), e, o);
			}
		}
		Player k = (Player)((t is Player) ? t : null);
		if (k != null)
		{
			if (b2)
			{
				p.RPC(RPC(7), k, Half(o));
				await Task.Delay(75);
				p.RPC(RPC(7), k, Half(o, f: false));
			}
			else
			{
				p.RPC(RPC(7), k, o);
			}
		}
		VRRig r = (VRRig)((t is VRRig) ? t : null);
		if (r != null)
		{
			if ((Object)(object)r == (Object)(object)OwnRig())
			{
				if (b2)
				{
					p.RPC(RPC(7), (RpcTarget)1, Half(o));
					await Task.Delay(75);
					p.RPC(RPC(7), (RpcTarget)1, Half(o, f: false));
				}
				else
				{
					p.RPC(RPC(7), (RpcTarget)1, o);
				}
			}
			else if (b2)
			{
				p.RPC(RPC(7), r.Creator.GetPlayerRef(), Half(o));
				await Task.Delay(75);
				p.RPC(RPC(7), r.Creator.GetPlayerRef(), Half(o, f: false));
			}
			else
			{
				p.RPC(RPC(7), r.Creator.GetPlayerRef(), o);
			}
		}
		if (t is int[] y)
		{
			if (b2)
			{
				Speci(p, RPC(7), y, Half(o));
				await Task.Delay(75);
				Speci(p, RPC(7), y, Half(o, f: false));
			}
			else
			{
				Speci(p, RPC(7), y, o);
			}
		}
	}

	public static void GrabColl(string ob, GameEntity ge = null, bool n = true)
	{
		if (WristMenu.gripDownR)
		{
			if (gco)
			{
				return;
			}
			try
			{
				GameEntity val = gw;
				if ((Object)(object)val != (Object)null)
				{
					if (val.IsHeldByLocalPlayer())
					{
						((Behaviour)OwnRig()).enabled = true;
						gco = true;
						gw = null;
						return;
					}
					SetSP();
					if (Vector3.Distance(((Component)val).transform.position, SP) < 5f)
					{
						Transform transform = ((Component)val).gameObject.transform;
						Vector3 position = OwnRig().rightHandTransform.position;
						if (!((Behaviour)OwnRig()).enabled)
						{
							DrawHandOrbs();
							OwnRig().rightHandTransform.position = transform.position;
						}
						if (Vector3.Distance(SP, transform.position) < 0.67f || val.manager.IsPlayerHandNearEntity(GamePlayerLocal.instance.gamePlayer, val.GetNetId(), false, false, 16f))
						{
							val.manager.RequestGrabEntity(val.id, false, Vector3.zero, Quaternion.identity);
							OwnRig().rightHandTransform.position = position;
						}
					}
					else
					{
						((Behaviour)OwnRig()).enabled = false;
						DrawHandOrbs();
						((Component)OwnRig()).transform.position = ((Component)val).gameObject.transform.position;
					}
				}
				else if ((Object)(object)ge == (Object)null)
				{
					List<GameEntity> list = (from e in GhostShi(ob)
						where e.pickupable && e.snappedByActorNumber == -1 && !e.IsHeld()
						select e).OrderBy((GameEntity e) =>
					{
						return Vector3.Distance(((Component)OwnRig()).transform.position, ((Component)e).gameObject.transform.position);
					}).ToList();
					if (list.Count > 0)
					{
						gw = list.Last();
					}
					else if (n)
					{
						string er = Er;
						string text;
						if (ob.Contains("CollectibleCore"))
						{
							text = "Core";
						}
						else
						{
							text = (ob.Contains("SentientCore") ? "Seed" : ((string)collectibles[coli][1]));
						}
						DelayedNoti(er + "There Are Currently No Active " + text + "'s");
					}
				}
				else
				{
					gw = ge;
				}
				return;
			}
			catch
			{
				return;
			}
		}
		((Behaviour)OwnRig()).enabled = true;
		gco = false;
		gw = null;
	}

	public static void Move(Vector3 p, Vector3 v)
	{
		if (Vector3.Distance(((Component)OwnRig()).transform.position, p) > 5f)
		{
			((Behaviour)OwnRig()).enabled = false;
			DrawHandOrbs();
			((Component)OwnRig()).transform.position = p;
			return;
		}
		if (!((Behaviour)OwnRig()).enabled)
		{
			DrawHandOrbs();
			((Component)OwnRig()).transform.position = p;
		}
		BlasterSpam(p, v, GuiPlugin.emgun);
	}

	public static void ScAll()
	{
		try
		{
			SIHandScannerTrigger[] array = Mods.F<SIHandScannerTrigger>(5f);
			foreach (SIHandScannerTrigger val in array)
			{
				if (((Behaviour)val).isActiveAndEnabled)
				{
					((Behaviour)OwnRig()).enabled = false;
					((Component)OwnRig()).transform.position = ((Component)val).gameObject.transform.position;
					val.Click(false);
				}
			}
		}
		catch
		{
		}
		((Behaviour)OwnRig()).enabled = true;
		DisableButton("Takeover All Terminals");
	}

	public static void DisableBarr()
	{
		bar = !bar;
		string[] source = new string[7] { "Environment Objects/LocalObjects_Prefab/Forest/Super Infection Zone - Forest Variant/ForestDome_Prefab", "Mountain/Super Infection Zone - Mountain Variant/MountainDome_Prefab", "MetroMain/Super Infection Zone - Metropolis Variant/MetroDome_Prefab", "Canyon/Super Infection Zone - Canyon Variant/CanyonDome_Prefab", "Canyon/Canyon/Canyon_ForceVolumes/CanyonDome_CollisionOnly", "Beach/Super Infection Zone - Beach Variant/BeachDome_Prefab", "Beach/ForceVolumesOcean_Combo_V2/MapDome_Beach_Collision (1)" };
		try
		{
			foreach (string item in source.Where((string e) => (Object)(object)Find(e) != (Object)null))
			{
				Find(item).SetActive(!bar);
			}
		}
		catch
		{
		}
		NewName("Disable Map Barriers (CS): " + bar);
	}

	public static void CollectESP()
	{
		try
		{
			colle = true;
			foreach (object[] value in collectibles.Values)
			{
				foreach (GameEntity item in GhostShi((string)value[0]))
				{
					Material component = ((Component)item).GetComponent<Material>();
					try
					{
						component = ((Renderer)((Component)item).gameObject.GetComponentInChildren<MeshRenderer>()).material;
					}
					catch
					{
						try
						{
							component = ((Component)item).gameObject.GetComponentInChildren<Renderer>().material;
						}
						catch
						{
							component = ((Renderer)((Component)item).gameObject.GetComponentInChildren<SkinnedMeshRenderer>()).material;
						}
					}
					if ((Object)(object)component.shader != (Object)(object)WristMenu.esp)
					{
						component.shader = WristMenu.esp;
						component.color = Color.white;
					}
				}
			}
		}
		catch
		{
		}
	}

	public static void OffCollectESP()
	{
		try
		{
			if (!colle)
			{
				return;
			}
			foreach (object[] value in collectibles.Values)
			{
				foreach (GameEntity item in GhostShi((string)value[0]))
				{
					Material component = ((Component)item).GetComponent<Material>();
					try
					{
						component = ((Renderer)((Component)item).gameObject.GetComponentInChildren<MeshRenderer>()).material;
					}
					catch
					{
						try
						{
							component = ((Component)item).gameObject.GetComponentInChildren<Renderer>().material;
						}
						catch
						{
							component = ((Renderer)((Component)item).gameObject.GetComponentInChildren<SkinnedMeshRenderer>()).material;
						}
					}
					if ((Object)(object)component.shader == (Object)(object)WristMenu.esp)
					{
						component.shader = Shader.Find("GorillaTag/UberShader");
					}
				}
			}
			colle = false;
		}
		catch
		{
		}
	}

	public static void SpoofCos(bool u)
	{
		if (PhotonNetwork.InRoom)
		{
			CosmeticsController val = Con();
			if (val.unlockedCosmetics.Count > 0)
			{
				List<CosmeticItem>[] array = new List<CosmeticItem>[9] { val.unlockedBacks, val.unlockedBadges, val.unlockedChests, val.unlockedFaces, val.unlockedFurs, val.unlockedHats, val.unlockedPants, val.unlockedPaws, val.unlockedShirts };
				foreach (Player item in RigUtils.AllPlayers(i: false))
				{
					CosmeticSet val2 = (u ? OwnRig().cosmeticSet : CosmeticSet.EmptySet);
					if (!u)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].Count > 0)
							{
								Con().ApplyCosmeticItemToSet(val2, array[i][Random.Range(0, array[i].Count)], false, false);
							}
						}
					}
					GIn().myVRRig.SendRPC(RPC(10), NetPlayer.op_Implicit(item), new object[3]
					{
						val2.ToPackedIDArray(),
						val.tryOnSet.ToPackedIDArray(),
						false
					});
				}
			}
			else
			{
				SendNoti(Er + "You Dont Own Any Cosmetics!");
			}
		}
		else
		{
			SendNoti(Er + "You Must Be In A Room!");
		}
		DisableButton((u ? "Un" : "") + "Spoof Cosmetics");
	}

	public static void Spoof(string m)
	{
		string name = ((m == "") ? "Un" : "") + "Spoof Self" + ((m != "") ? (" " + m) : m);
		if (PhotonNetwork.InRoom)
		{
			SetSP();
			Vector3 position = ((Component)GorillaComputer.instance.friendJoinCollider).transform.position;
			((Behaviour)OwnRig()).enabled = false;
			((Component)OwnRig()).transform.position = position;
			if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(TouchingPlayerInfo.op_Implicit(Local().UserId)) || !(Vector3.Distance(SP, position) < 0.1f))
			{
				return;
			}
			string nickName = Local().NickName;
			Color playerColor = OwnRig().playerColor;
			if (m != "")
			{
				foreach (Player item in RigUtils.AllPlayers(i: false))
				{
					int index = Random.Range(0, ghosts.Count);
					Dictionary<string, object[]> dictionary = new Dictionary<string, object[]>();
					dictionary.Add("(Become Players)", new object[2]
					{
						RigUtils.RigFromPlayer(item).playerColor,
						item.NickName
					});
					dictionary.Add("(Random)", new object[2]
					{
						ranc(),
						RandomString()
					});
					dictionary.Add("(Gtag Ghosts)", new object[2]
					{
						ghosts.Values.ToList()[index],
						ghosts.Keys.ToList()[index]
					});
					dictionary.Add("(Player IDS)", new object[2]
					{
						playerColor,
						item.UserId.Substring(0, 12)
					});
					Dictionary<string, object[]> dictionary2 = dictionary;
					Color val = (Color)dictionary2[m][0];
					Local().NickName = (string)dictionary2[m][1];
					GIn().myVRRig.SendRPC(RPC(5), NetPlayer.op_Implicit(item), new object[3] { val.r, val.g, val.b });
				}
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					ChangeName(nickName, playerColor, "");
				}
			}
			((Behaviour)OwnRig()).enabled = true;
			DisableButton(name);
			if (m != "")
			{
				Local().NickName = nickName;
				GIn().UpdateColor(playerColor.r, playerColor.g, playerColor.b);
			}
		}
		else
		{
			SendNoti(Er + "You Must Be In A Room!");
			DisableButton(name);
		}
	}

	public static void CSMat()
	{
		if ((Object)(object)ogmat == (Object)null)
		{
			ogmat = ((Renderer)OwnRig().mainSkin).material;
		}
		new Delay().D(0.05f, () =>
		{
			Material[] value = Create(((Component)OwnRig()).GetComponent<XRaySkeleton>()).Field(Field(64)).GetValue<Material[]>();
			((Renderer)OwnRig().mainSkin).material = value[Random.Range(0, value.Length)];
		});
	}

	public static void ShowAntiReport()
	{
		ant = true;
		if ((Object)(object)Find("ShowAntiReport") != (Object)null)
		{
			return;
		}
		List<GorillaPlayerLineButton> list = Mods.F<GorillaPlayerLineButton>(5f).ToList();
		List<GameObject> list2 = new List<GameObject>();
		foreach (GorillaPlayerLineButton item in list)
		{
			if (item.parentLine.linePlayer == NetworkSystem.Instance.LocalPlayer && (int)item.buttonType == 4 && !list2.Contains(((Component)item).gameObject))
			{
				list2.Add(((Component)item).gameObject);
			}
		}
		GameObject val = new GameObject("ShowAntiReport");
		for (int i = 0; i < list2.Count; i++)
		{
			GameObject val2 = CreatePrimitive((PrimitiveType)0);
			Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
			val2.transform.SetParent(val.transform);
			val2.transform.position = list2[i].transform.position;
			val2.transform.localScale = antirad;
			val2.transform.rotation = list2[i].transform.rotation;
			val2.GetComponent<Renderer>().sharedMaterial.shader = WristMenu.esp;
			val2.GetComponent<Renderer>().sharedMaterial.color = Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)3));
			bs.Add(val2);
		}
	}

	public static void ShowTriggers()
	{
		nett = true;
		List<GorillaNetworkJoinTrigger> list = Mods.F<GorillaNetworkJoinTrigger>(5f).ToList();
		if ((Object)(object)Find("ShowTriggers") == (Object)null)
		{
			GameObject val = new GameObject("ShowTriggers");
			for (int i = 0; i < list.Count; i++)
			{
				GameObject val2 = CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
				val2.transform.SetParent(val.transform);
				val2.transform.position = ((Component)list[i]).gameObject.transform.position;
				val2.transform.localScale = ((Component)list[i]).gameObject.transform.localScale;
				val2.transform.rotation = ((Component)list[i]).gameObject.transform.rotation;
				val2.GetComponent<Renderer>().sharedMaterial.shader = WristMenu.esp;
				val2.GetComponent<Renderer>().sharedMaterial.color = Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)4));
			}
		}
	}

	public static void RingESP()
	{
		ESPCheck(6);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[6].ContainsKey(item))
			{
				GameObject val = new GameObject("RingESP");
				List<GameObject> list = new List<GameObject>(40);
				for (int i = 0; i < 40; i++)
				{
					float num = (float)i / 40f * (float)Math.PI * 2f;
					GameObject val2 = CreatePrimitive((PrimitiveType)3);
					Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
					val2.transform.SetParent(val.transform);
					val2.transform.localPosition = new Vector3(Mathf.Cos(num) * 0.5f, 0f, Mathf.Sin(num) * 0.5f);
					val2.transform.localRotation = Quaternion.Euler(0f, (0f - num) * 57.29578f, 0f);
					val2.transform.localScale = new Vector3(0.025f, 0.025f, (float)Math.PI / 40f);
					val2.GetComponent<Renderer>().sharedMaterial.shader = WristMenu.esp;
					list.Add(val2);
				}
				ESPss[6].Add(item, list.ToArray());
				ESPs[6].Add(item, val);
			}
			GameObject val3 = ESPs[6][item];
			val3.transform.position = ((Component)item).transform.position - new Vector3(0f, 0.1f, 0f);
			val3.transform.LookAt(((Component)Camera.main).transform.position);
			val3.transform.rotation = val3.transform.rotation * Quaternion.Euler(90f, 0f, 0f);
			GameObject[] array = ESPss[6][item];
			foreach (GameObject val4 in array)
			{
				HideEsp(val4, 0);
				if (val4.GetComponent<Renderer>().material.color != ESPColor(item))
				{
					val4.GetComponent<Renderer>().material.color = ESPColor(item);
				}
			}
		}
	}

	public static GameObject Ellip(float s = 1f, List<GameObject> l = null)
	{
		GameObject val = new GameObject("Ellip");
		GameObject val2 = new GameObject("x");
		val2.transform.SetParent(val.transform);
		GameObject val3 = new GameObject("y");
		val3.transform.SetParent(val.transform);
		GameObject val4 = new GameObject("z");
		val4.transform.SetParent(val.transform);
		for (int i = 0; i < 120; i++)
		{
			float num = (float)i / 40f * (float)Math.PI * 2f;
			GameObject val5 = CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val5.GetComponent<BoxCollider>());
			val5.transform.SetParent((i <= 40) ? val2.transform : ((i <= 80) ? val3.transform : val4.transform));
			val5.transform.localPosition = new Vector3(Mathf.Cos(num) * 0.5f, 0f, Mathf.Sin(num) * 0.5f);
			val5.transform.localRotation = Quaternion.Euler(0f, (0f - num) * 57.29578f, 0f);
			val5.transform.localScale = new Vector3(0.025f, 0.025f, (float)Math.PI / 40f);
			val5.GetComponent<Renderer>().sharedMaterial.shader = WristMenu.esp;
			l?.Add(val5);
		}
		val3.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
		val4.transform.rotation = Quaternion.Euler(0f, 90f, 90f);
		val.transform.localScale = new Vector3(s, s, s);
		return val;
	}

	public static void ElliESP()
	{
		ESPCheck(7);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[7].ContainsKey(item))
			{
				List<GameObject> list = new List<GameObject>(120);
				GameObject value = Ellip(1f, list);
				ESPss[7].Add(item, list.ToArray());
				ESPs[7].Add(item, value);
			}
			GameObject val = ESPs[7][item];
			val.transform.position = ((Component)item).transform.position - new Vector3(0f, 0.1f, 0f);
			GameObject[] array = ESPss[7][item];
			foreach (GameObject val2 in array)
			{
				HideEsp(val2, 0);
				val2.GetComponent<Renderer>().material.color = ESPColor(item);
			}
		}
	}

	public static void Tracers()
	{
		if (PhotonNetwork.InRoom)
		{
			foreach (VRRig item in ESPPlayers())
			{
				LineRenderer orAddComponent = GTExt.GetOrAddComponent<LineRenderer>(((Component)item).gameObject);
				HideEsp(((Component)orAddComponent).gameObject, 11);
				orAddComponent.startWidth = 0.01f * Loco().scale;
				orAddComponent.endWidth = 0.01f * Loco().scale;
				orAddComponent.positionCount = 2;
				orAddComponent.useWorldSpace = true;
				orAddComponent.SetPosition(0, GetTracerPos());
				orAddComponent.SetPosition(1, ((Component)item).transform.position);
				((Renderer)orAddComponent).material.shader = WristMenu.esp;
				orAddComponent.startColor = ESPColor(item);
				orAddComponent.endColor = ESPColor(item);
				if (!TracerLines.Contains(orAddComponent))
				{
					TracerLines.Add(orAddComponent);
				}
			}
			return;
		}
		if (TracerLines.Count <= 0)
		{
			return;
		}
		foreach (LineRenderer tracerLine in TracerLines)
		{
			Object.Destroy((Object)(object)tracerLine);
		}
		TracerLines.Clear();
	}

	public static void SphereESP()
	{
		ESPCheck(1);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[1].ContainsKey(item))
			{
				GameObject val = CreatePrimitive((PrimitiveType)0);
				Object.Destroy((Object)(object)val.GetComponent<SphereCollider>());
				val.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
				val.GetComponent<Renderer>().material.shader = WristMenu.esp;
				ESPs[1].Add(item, val);
			}
			GameObject val2 = ESPs[1][item];
			HideEsp(val2, 0);
			val2.transform.position = ((Component)item).transform.position;
			val2.GetComponent<Renderer>().material.color = ESPColor(item);
		}
	}

	public static void Boxmathshit(GameObject b, VRRig rig, out float[] f)
	{
		Transform[] array = rig.mainSkin.bones;
		int[] array2 = bones;
		float num = -0.5f;
		float num2 = 0.5f;
		float num3 = 0.5f;
		float num4 = -0.5f;
		float num5 = 0.03f;
		for (int i = 0; i < array2.Length; i += 2)
		{
			for (int j = 0; j <= 16; j++)
			{
				Vector3 val = b.transform.InverseTransformPoint(Vector3.Lerp(array[array2[i]].position, array[array2[i + 1]].position, (float)j / 6f));
				float num6 = Mathf.Abs(val.z) * 0.405f;
				if (val.x - num6 < num)
				{
					num = val.x - num6 - num5;
				}
				if (val.x + num6 > num2)
				{
					num2 = val.x + num6 + num5;
				}
				if (val.y + num6 > num3)
				{
					num3 = val.y + num6 + num5;
				}
				if (val.y - num6 < num4)
				{
					num4 = val.y - num6 - num5;
				}
			}
		}
		f = new float[8]
		{
			num2 - num,
			num3 - num4,
			(num + num2) / 2f,
			(num3 + num4) / 2f,
			num3,
			num4,
			num,
			num2
		};
	}

	public static void BoxESP()
	{
		ESPCheck(0);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[0].ContainsKey(item))
			{
				GameObject val = CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val.GetComponent<BoxCollider>());
				val.transform.localScale = new Vector3(1f, 1f, 0f);
				val.GetComponent<Renderer>().material.shader = WristMenu.esp;
				ESPs[0].Add(item, val);
			}
			GameObject val2 = ESPs[0][item];
			HideEsp(val2, 0);
			val2.transform.position = ((Component)item).transform.position;
			val2.GetComponent<Renderer>().material.color = ESPColor(item);
			val2.transform.LookAt(((Component)Camera.main).transform.position);
			Boxmathshit(val2, item, out var f);
			val2.transform.position = ((Component)item).transform.position + val2.transform.right * f[2] + val2.transform.up * f[3];
			val2.transform.localScale = new Vector3(f[0], f[1], 0f);
		}
	}

	public static void BoxESPV2()
	{
		ESPCheck(2);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[2].ContainsKey(item))
			{
				GameObject val = CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val.GetComponent<Renderer>());
				GameObject val2 = CreatePrimitive((PrimitiveType)3);
				GameObject val3 = CreatePrimitive((PrimitiveType)3);
				GameObject val4 = CreatePrimitive((PrimitiveType)3);
				GameObject val5 = CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val5.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val4.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val3.GetComponent<BoxCollider>());
				val2.transform.SetParent(val.transform);
				val5.transform.SetParent(val.transform);
				val4.transform.SetParent(val.transform);
				val3.transform.SetParent(val.transform);
				val2.transform.localPosition = new Vector3(0f, 0.5f, 0f);
				val5.transform.localPosition = new Vector3(0f, -0.5f, 0f);
				val4.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
				val3.transform.localPosition = new Vector3(0.5f, 0f, 0f);
				val2.transform.localScale = new Vector3(1.01f, 0.02f, 0.02f);
				val5.transform.localScale = new Vector3(1.01f, 0.02f, 0.02f);
				val4.transform.localScale = new Vector3(0.02f, 1f, 0.02f);
				val3.transform.localScale = new Vector3(0.02f, 1f, 0.02f);
				val2.GetComponent<Renderer>().material.shader = WristMenu.esp;
				val5.GetComponent<Renderer>().material.shader = WristMenu.esp;
				val4.GetComponent<Renderer>().material.shader = WristMenu.esp;
				val3.GetComponent<Renderer>().material.shader = WristMenu.esp;
				ESPss[2].Add(item, new GameObject[4] { val2, val3, val4, val5 });
				ESPs[2].Add(item, val);
			}
			GameObject val6 = ESPs[2][item];
			val6.transform.position = ((Component)item).transform.position;
			val6.transform.LookAt(((Component)Camera.main).transform.position);
			GameObject[] array = ESPss[2][item];
			Boxmathshit(val6, item, out var f);
			array[0].transform.localPosition = new Vector3(f[2], f[4], 0f);
			array[0].transform.localScale = new Vector3(f[0], 0.02f, 0.02f);
			array[1].transform.localPosition = new Vector3(f[2], f[5], 0f);
			array[1].transform.localScale = new Vector3(f[0], 0.02f, 0.02f);
			array[2].transform.localPosition = new Vector3(f[6], f[3], 0f);
			array[2].transform.localScale = new Vector3(0.02f, f[1], 0.02f);
			array[3].transform.localPosition = new Vector3(f[7], f[3], 0f);
			array[3].transform.localScale = new Vector3(0.02f, f[1], 0.02f);
			GameObject[] array2 = array;
			foreach (GameObject val7 in array2)
			{
				HideEsp(val7, 0);
				val7.GetComponent<Renderer>().material.color = ESPColor(item);
			}
		}
	}

	public static void BoxESPV3()
	{
		ESPCheck(3);
		foreach (VRRig item in ESPPlayers())
		{
			Shader esp = WristMenu.esp;
			PrimitiveType val = (PrimitiveType)3;
			if (!ESPs[3].ContainsKey(item))
			{
				GameObject val2 = CreatePrimitive(val);
				Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val2.GetComponent<Renderer>());
				GameObject val3 = CreatePrimitive(val);
				GameObject val4 = CreatePrimitive(val);
				GameObject val5 = CreatePrimitive(val);
				GameObject val6 = CreatePrimitive(val);
				GameObject val7 = CreatePrimitive(val);
				GameObject val8 = CreatePrimitive(val);
				GameObject val9 = CreatePrimitive(val);
				GameObject val10 = CreatePrimitive(val);
				Object.Destroy((Object)(object)val3.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val6.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val5.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val4.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val7.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val8.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val9.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val10.GetComponent<BoxCollider>());
				val3.transform.SetParent(val2.transform);
				val6.transform.SetParent(val2.transform);
				val5.transform.SetParent(val2.transform);
				val4.transform.SetParent(val2.transform);
				val7.transform.SetParent(val2.transform);
				val8.transform.SetParent(val2.transform);
				val9.transform.SetParent(val2.transform);
				val10.transform.SetParent(val2.transform);
				val3.transform.localPosition = new Vector3(-0.42f, 0.49f, 0f);
				val5.transform.localPosition = new Vector3(-0.49f, 0.42f, 0f);
				val6.transform.localPosition = new Vector3(0.42f, 0.49f, 0f);
				val4.transform.localPosition = new Vector3(0.49f, 0.42f, 0f);
				val7.transform.localPosition = new Vector3(-0.42f, -0.49f, 0f);
				val8.transform.localPosition = new Vector3(-0.49f, -0.42f, 0f);
				val9.transform.localPosition = new Vector3(0.42f, -0.49f, 0f);
				val10.transform.localPosition = new Vector3(0.49f, -0.42f, 0f);
				val3.transform.localScale = new Vector3(0.15f, 0.02f, 0.02f);
				val6.transform.localScale = new Vector3(0.15f, 0.02f, 0.02f);
				val5.transform.localScale = new Vector3(0.02f, 0.15f, 0.02f);
				val4.transform.localScale = new Vector3(0.02f, 0.15f, 0.02f);
				val7.transform.localScale = new Vector3(0.15f, 0.02f, 0.02f);
				val8.transform.localScale = new Vector3(0.02f, 0.15f, 0.02f);
				val9.transform.localScale = new Vector3(0.15f, 0.02f, 0.02f);
				val10.transform.localScale = new Vector3(0.02f, 0.15f, 0.02f);
				val3.GetComponent<Renderer>().material.shader = esp;
				val6.GetComponent<Renderer>().material.shader = esp;
				val5.GetComponent<Renderer>().material.shader = esp;
				val4.GetComponent<Renderer>().material.shader = esp;
				val7.GetComponent<Renderer>().material.shader = esp;
				val8.GetComponent<Renderer>().material.shader = esp;
				val9.GetComponent<Renderer>().material.shader = esp;
				val10.GetComponent<Renderer>().material.shader = esp;
				ESPss[3].Add(item, new GameObject[8] { val3, val4, val5, val6, val7, val8, val9, val10 });
				ESPs[3].Add(item, val2);
			}
			GameObject val11 = ESPs[3][item];
			val11.transform.position = ((Component)item).transform.position;
			val11.transform.LookAt(((Component)Camera.main).transform.position);
			GameObject[] array = ESPss[3][item];
			Boxmathshit(val11, item, out var f);
			float num = f[6];
			float num2 = f[7];
			float num3 = f[4];
			float num4 = f[5];
			float num5 = 0.075f;
			array[0].transform.localPosition = new Vector3(num + num5, num3, 0f);
			array[1].transform.localPosition = new Vector3(num2, num3 - num5, 0f);
			array[2].transform.localPosition = new Vector3(num, num3 - num5, 0f);
			array[3].transform.localPosition = new Vector3(num2 - num5, num3, 0f);
			array[4].transform.localPosition = new Vector3(num + num5, num4, 0f);
			array[5].transform.localPosition = new Vector3(num, num4 + num5, 0f);
			array[6].transform.localPosition = new Vector3(num2 - num5, num4, 0f);
			array[7].transform.localPosition = new Vector3(num2, num4 + num5, 0f);
			GameObject[] array2 = array;
			foreach (GameObject val12 in array2)
			{
				HideEsp(val12, 0);
				val12.GetComponent<Renderer>().material.color = ESPColor(item);
			}
		}
	}

	public static void Beacons()
	{
		ESPCheck(4);
		foreach (VRRig item in ESPPlayers())
		{
			if (!ESPs[4].ContainsKey(item))
			{
				GameObject val = CreatePrimitive((PrimitiveType)2);
				Object.Destroy((Object)(object)val.GetComponent<BoxCollider>());
				Object.Destroy((Object)(object)val.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val.GetComponent<Collider>());
				val.transform.rotation = Quaternion.identity;
				val.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
				val.GetComponent<Renderer>().material.shader = WristMenu.esp;
				ESPs[4].Add(item, val);
			}
			GameObject val2 = ESPs[4][item];
			HideEsp(val2, 0);
			val2.transform.position = ((Component)item).transform.position;
			val2.GetComponent<Renderer>().material.color = ESPColor(item);
		}
	}

	public static void Chams()
	{
		ch = true;
		foreach (VRRig item in ESPPlayers())
		{
			Material material = ((Renderer)item.mainSkin).material;
			material.shader = WristMenu.esp;
			material.color = ESPColor(item);
		}
	}

	public static void OffChams()
	{
		if (!ch)
		{
			return;
		}
		foreach (VRRig item in ESPPlayers())
		{
			((Renderer)item.mainSkin).material.shader = Shader.Find("GorillaTag/UberShader");
		}
		ch = false;
	}

	public static void SkellyESP()
	{
		skl = true;
		Shader esp = WristMenu.esp;
		float num = 0.025f;
		foreach (VRRig item in ESPPlayers())
		{
			Color val = ESPColor(item);
			Transform[] array = item.mainSkin.bones;
			int[] array2 = bones;
			Transform rigTarget = item.head.rigTarget;
			LineRenderer orAddComponent = GTExt.GetOrAddComponent<LineRenderer>(((Component)rigTarget).gameObject);
			HideEsp(((Component)orAddComponent).gameObject, 11);
			orAddComponent.startWidth = num;
			orAddComponent.endWidth = num;
			((Renderer)orAddComponent).material.shader = esp;
			orAddComponent.startColor = val;
			orAddComponent.endColor = val;
			orAddComponent.SetPosition(0, rigTarget.position + new Vector3(0f, 0.16f, 0f));
			orAddComponent.SetPosition(1, rigTarget.position - new Vector3(0f, 0.4f, 0f));
			if (!lines.Contains(orAddComponent))
			{
				lines.Add(orAddComponent);
			}
			for (int i = 0; i < array2.Length; i += 2)
			{
				orAddComponent = GTExt.GetOrAddComponent<LineRenderer>(((Component)array[array2[i]]).gameObject);
				HideEsp(((Component)orAddComponent).gameObject, 11);
				orAddComponent.startWidth = num;
				orAddComponent.endWidth = num;
				((Renderer)orAddComponent).material.shader = esp;
				orAddComponent.startColor = val;
				orAddComponent.endColor = val;
				orAddComponent.SetPosition(0, array[array2[i]].position);
				orAddComponent.SetPosition(1, array[array2[i + 1]].position);
				if (!lines.Contains(orAddComponent))
				{
					lines.Add(orAddComponent);
				}
			}
		}
	}

	public static void SkellyV2()
	{
		res = true;
		foreach (VRRig item in ESPPlayers())
		{
			SkinnedMeshRenderer renderer = ((Component)item).GetComponent<XRaySkeleton>().renderer;
			((Component)renderer).gameObject.SetActive(true);
			((Renderer)renderer).enabled = true;
			((Renderer)renderer).material.shader = WristMenu.esp;
			((Renderer)renderer).material.color = ESPColor(item);
			((Component)((Component)item).transform.Find("rig/head/gorillaface")).gameObject.SetActive(false);
			((Component)((Component)item).transform.Find("gorilla_new")).gameObject.SetActive(false);
			if (!skelrig.Contains(item))
			{
				skelrig.Add(item);
			}
		}
	}

	public static void OffSkellyV2()
	{
		if (!res)
		{
			return;
		}
		if (skelrig.Count > 0)
		{
			foreach (VRRig item in skelrig)
			{
				SkinnedMeshRenderer renderer = ((Component)item).GetComponent<XRaySkeleton>().renderer;
				((Renderer)renderer).enabled = false;
				((Component)renderer).gameObject.SetActive(false);
				((Component)((Component)item).transform.Find("rig/head/gorillaface")).gameObject.SetActive(true);
				((Component)((Component)item).transform.Find("gorilla_new")).gameObject.SetActive(true);
			}
			skelrig.Clear();
		}
		res = false;
	}

	public static void ChangeDay()
	{
		int num = 0;
		string text = "";
		day++;
		if (day > 4)
		{
			day = 1;
		}
		if (day == 1)
		{
			num = 1;
			text = "Morning";
		}
		if (day == 2)
		{
			num = 3;
			text = "Day";
		}
		if (day == 3)
		{
			num = 7;
			text = "Evening";
		}
		if (day == 4)
		{
			num = 0;
			text = "Night";
		}
		SendNoti("[<color=blue>TIME OF DAY</color>] Current Time: " + text);
		BetterDayNightManager.instance.SetTimeOfDay(num, false);
		DisableButton("Change Time Of Day (CS)");
	}

	public static void CoreESP()
	{
		try
		{
			corees = true;
			foreach (GameEntity item in GhostShi("GhostReactorCollectibleCore"))
			{
				MeshRenderer componentInChildren = ((Component)item).gameObject.GetComponentInChildren<MeshRenderer>();
				if ((Object)(object)((Renderer)componentInChildren).material.shader != (Object)(object)WristMenu.esp)
				{
					((Renderer)componentInChildren).material.shader = WristMenu.esp;
					((Renderer)componentInChildren).material.color = Color.white;
				}
			}
		}
		catch
		{
		}
	}

	public static void OffCoreESP()
	{
		try
		{
			if (!corees)
			{
				return;
			}
			foreach (GameEntity item in GhostShi("GhostReactorCollectibleCore"))
			{
				MeshRenderer componentInChildren = ((Component)item).gameObject.GetComponentInChildren<MeshRenderer>();
				if ((Object)(object)((Renderer)componentInChildren).material.shader == (Object)(object)WristMenu.esp)
				{
					((Renderer)componentInChildren).material.shader = Shader.Find("GorillaTag/UberShader");
				}
			}
			corees = false;
		}
		catch
		{
		}
	}

	public static void FPSboost()
	{
		if (fpsd.Length == 0)
		{
			fpsd = new int[5]
			{
				QualitySettings.vSyncCount,
				QualitySettings.globalTextureMipmapLimit,
				QualitySettings.maxQueuedFrames,
				DebugHudStats.FPS_THRESHOLD,
				Application.targetFrameRate
			};
		}
		fps = true;
		QualitySettings.vSyncCount = int.MinValue;
		QualitySettings.globalTextureMipmapLimit = 999999999;
		QualitySettings.maxQueuedFrames = 144;
		DebugHudStats.FPS_THRESHOLD = 144;
		Application.targetFrameRate = 144;
	}

	public static void fixFPS()
	{
		if (fps)
		{
			QualitySettings.vSyncCount = fpsd[0];
			QualitySettings.globalTextureMipmapLimit = fpsd[1];
			QualitySettings.maxQueuedFrames = fpsd[2];
			DebugHudStats.FPS_THRESHOLD = fpsd[3];
			Application.targetFrameRate = fpsd[4];
			fpsd = new int[0];
			fps = false;
		}
	}

	public static void AnimateName()
	{
		animate = true;
		new Delay().D(0.21f, () =>
		{
			if (dename == "")
			{
				dename = Local().NickName;
			}
			string text = "";
			dein++;
			if (dein > dename.Length)
			{
				dein = 1;
			}
			if (text.Length != dein)
			{
				for (int i = 0; i < dein; i++)
				{
					text += dename[i];
				}
			}
			ChangeName(text, Color.clear, "Animate Name (NC)");
		});
	}

	public static void DisableAnimateName()
	{
		if (!animate)
		{
			return;
		}
		VRRig val = OwnRig();
		if (PhotonNetwork.InRoom)
		{
			if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(TouchingPlayerInfo.op_Implicit(Local().UserId)) && !CosmeticWardrobeProximityDetector.IsUserNearWardrobe(Local().ActorNumber))
			{
				((Behaviour)val).enabled = false;
				((Component)val).transform.position = ((Component)GorillaComputer.instance.friendJoinCollider).transform.position;
			}
			else
			{
				ChangeName(dename, val.playerColor, "Animate Name (NC)");
			}
			if (Local().NickName == dename)
			{
				if (!IsEnabled("Ghost Monke"))
				{
					((Behaviour)val).enabled = true;
				}
				if (dename != "")
				{
					dename = "";
				}
				if (dein != 1)
				{
					dein = 1;
				}
				animate = false;
			}
		}
		else
		{
			ChangeName(dename, val.playerColor, "Animate Name (NC)");
			if (dename != "")
			{
				dename = "";
			}
			if (dein != 1)
			{
				dein = 1;
			}
			animate = false;
		}
	}

	public static void BecomeGhost()
	{
		ghost++;
		if (ghost > ghosts.Count)
		{
			ghost = 0;
		}
		ChangeName(ghosts.Keys.ToList()[ghost], ghosts.Values.ToList()[ghost], "Become Ghosts (NC)");
		SendNoti("[<color=cyan>GHOST</color>] You Are Now: " + ghosts.Keys.ToList()[ghost]);
		DisableButton("Become Gtag Ghosts (NC)");
	}

	public static void Monk()
	{
		string text = " Monkes (CS)";
		monk = IsEnabled("Wide" + text) || IsEnabled("Tall" + text) || IsEnabled("Short" + text) || IsEnabled("Thin" + text) || IsEnabled("2D" + text) || IsEnabled("Long" + text) || IsEnabled("Flat" + text);
	}

	public static void OffMonk()
	{
		if (!monk)
		{
			return;
		}
		string text = " Monkes (CS)";
		if (!IsEnabled("Wide" + text) && !IsEnabled("Tall" + text) && !IsEnabled("Short" + text) && !IsEnabled("Thin" + text) && !IsEnabled("2D" + text) && !IsEnabled("Long" + text) && !IsEnabled("Flat" + text))
		{
			monk = false;
			RigUtils.AllRigs(i: false).ForEach((VRRig r) =>
			{
				((Component)r).transform.localScale = Vector3.one;
			});
		}
	}

	public static void DoMonke()
	{
		foreach (VRRig item in ESPPlayers())
		{
			string text = " Monkes (CS)";
			Transform transform = ((Component)item).transform;
			Vector3 localScale = ((Component)item).transform.localScale;
			if (IsEnabled("Wide" + text))
			{
				localScale.x = 2.7f;
			}
			else
			{
				localScale.x = 1f;
			}
			if (IsEnabled("Tall" + text))
			{
				localScale.y = 2.7f;
			}
			else
			{
				localScale.y = 1f;
			}
			if (IsEnabled("Short" + text))
			{
				localScale.y = 0.34f;
			}
			else if (!IsEnabled("Tall" + text))
			{
				localScale.y = 1f;
			}
			if (IsEnabled("Flat" + text))
			{
				localScale.y = 0.001f;
			}
			else if (!IsEnabled("Tall" + text) && !IsEnabled("Short" + text))
			{
				localScale.y = 1f;
			}
			if (IsEnabled("Thin" + text))
			{
				localScale.x = 0.34f;
			}
			else if (!IsEnabled("Wide" + text))
			{
				localScale.x = 1f;
			}
			if (IsEnabled("Long" + text))
			{
				localScale.z = 2.7f;
			}
			else
			{
				localScale.z = 1f;
			}
			if (IsEnabled("2D" + text))
			{
				localScale.z = 0.001f;
			}
			else if (!IsEnabled("Long" + text))
			{
				localScale.z = 1f;
			}
			transform.localScale = localScale;
		}
	}

	public static void PlatformLabel()
	{
		platl = IsEnabled("NameTags") || IsEnabled("Platform Labels") || IsEnabled("FPS Labels") || IsEnabled("Special Cosmetic Labels");
	}

	public static void DoPlatformLabel()
	{
		ESPCheck(5);
		foreach (VRRig rig in ESPPlayers())
		{
			if (!ESPs[5].ContainsKey(rig))
			{
				GameObject val = new GameObject("PlatformShit");
				val.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
				TextMesh val2 = val.AddComponent<TextMesh>();
				val2.fontSize = 24;
				val2.fontStyle = (FontStyle)1;
				val2.characterSize = 0.1f;
				val2.anchor = (TextAnchor)4;
				val2.alignment = (TextAlignment)1;
				ESPs[5].Add(rig, val);
			}
			Player val3 = RigUtils.PlayerFromRig(rig);
			GameObject val4 = ESPs[5][rig];
			TextMesh component = val4.GetComponent<TextMesh>();
			HideEsp(((Component)component).gameObject, 0);
			component.color = ESPColor(rig, s: true);
			string plat = "";
			string nickName = val3.NickName;
			bool flag = IsEnabled("NameTags");
			bool flag2 = IsEnabled("Platform Labels");
			bool flag3 = IsEnabled("FPS Labels");
			string text = (flag ? (nickName + "\n") : "");
			float num = ((flag & flag2) ? 0.67f : 0.57f);
			if (num != 0.67f)
			{
				num = 0.57f;
			}
			else
			{
				num = (flag3 ? 0.77f : 0.67f);
			}
			if (flag2)
			{
				plat = text + GuiPlugin.GetAllPlayersPlats(all: false, val3).ToUpper();
			}
			else if (flag)
			{
				plat = nickName;
			}
			string text2 = ((flag | flag2) ? "\n" : "");
			plat += (flag3 ? (text2 + Create(rig).Field(Field(12)).GetValue().ToString()) : "");
			if (IsEnabled("Special Cosmetic Labels"))
			{
				LinqUtils.ForEach<string>(sc.Keys.Where((string ss) => GetCos(rig).Contains(ss)), (Action<string>)((string ss) =>
				{
					plat = plat + "\n" + sc[ss];
				}));
			}
			component.text = plat;
			val4.transform.position = ((Component)rig).transform.position + new Vector3(0f, num, 0f);
			val4.transform.LookAt(((Component)Camera.main).transform.position);
			val4.transform.Rotate(0f, 180f, 0f);
		}
	}

	public static void OffPlatform()
	{
		if (platl && !IsEnabled("Platform Labels") && !IsEnabled("FPS Labels") && !IsEnabled("NameTags") && !IsEnabled("Special Cosmetic Labels"))
		{
			platl = false;
			OffESPS(5);
		}
	}

	public static void CompLabel()
	{
		ve1 = true;
		if ((Object)(object)label == (Object)null)
		{
			label = new GameObject("CompShit");
			label.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			mesh = label.AddComponent<TextMesh>();
			mesh.color = Color.white;
			mesh.fontSize = 24;
			mesh.fontStyle = (FontStyle)1;
			mesh.characterSize = 0.1f;
			mesh.anchor = (TextAnchor)4;
			mesh.alignment = (TextAlignment)0;
		}
		HideEsp(((Component)mesh).gameObject, 0);
		List<VRRig> list = new List<VRRig>();
		string text = "";
		string text2 = "";
		VRRig val = null;
		string text3 = "";
		VRRig val2 = null;
		VRRig self = OwnRig();
		List<VRRig> list2 = RigUtils.AllRigs();
		string text4 = "";
		if (PhotonNetwork.InRoom)
		{
			foreach (VRRig item in list2)
			{
				if (ActualTagged(item) && !list.Contains(item))
				{
					list.Add(item);
				}
				else if (!ActualTagged(item) && list.Contains(item))
				{
					list.Remove(item);
				}
			}
			text = list.Count + "/" + list2.Count;
			try
			{
				val = list2.Where((VRRig rig) => !rig.isLocal && ActualTagged(rig)).OrderBy((VRRig rig) =>
				{
					return Vector3.Distance(((Component)rig).transform.position, ((Component)self).transform.position);
				}).FirstOrDefault();
				text2 = Mathf.RoundToInt(Vector3.Distance(((Component)val).transform.position, ((Component)self).transform.position)) + "M";
			}
			catch
			{
				text2 = "0M";
			}
			try
			{
				val2 = list2.Where((VRRig rig) => !rig.isLocal && !ActualTagged(rig)).OrderBy((VRRig rig) =>
				{
					return Vector3.Distance(((Component)rig).transform.position, ((Component)self).transform.position);
				}).FirstOrDefault();
				text3 = Mathf.RoundToInt(Vector3.Distance(((Component)val2).transform.position, ((Component)self).transform.position)) + "M";
			}
			catch
			{
				text3 = "0M";
			}
			int num = Array.FindIndex(RigUtils.AllPlayers().ToArray(), (Player p) => p == Local());
			text4 = num + 1 + "/" + RigUtils.AllPlayers().Count;
		}
		else
		{
			if (list.Count > 0)
			{
				list.Clear();
			}
			text = "N/A";
			text2 = "N/A";
			text3 = "N/A";
			text4 = "N/A";
		}
		if (Time.time > ba + 0.1f)
		{
			ba = Time.time;
			fpss = "FPS: " + Mathf.RoundToInt(1f / Time.deltaTime);
		}
		string text5 = "\n";
		string[] array = new string[13]
		{
			fpss, text5, null, null, null, null, null, null, null, null,
			null, null, null
		};
		Vector3 linearVelocity = ((Component)Loco()).GetComponent<Rigidbody>().linearVelocity;
		array[2] = "Velocity: " + linearVelocity.magnitude.ToString("F1") + " m/s";
		array[3] = text5;
		array[4] = "Tagged Players: " + text;
		array[5] = text5;
		array[6] = "Closest Tagged: " + text2;
		array[7] = text5;
		array[8] = "Closest UnTagged: " + text3;
		array[9] = text5;
		array[10] = "Placement In Lobby: " + text4;
		array[11] = text5;
		array[12] = "Current Master: " + (PhotonNetwork.InRoom ? PhotonNetwork.MasterClient.NickName : Local().NickName);
		string text6 = string.Concat(array);
		mesh.text = text6;
		Transform transform = Find("Main Camera").transform;
		label.transform.position = transform.position + transform.forward * 2.7f - transform.right / 1f + new Vector3(0f, 0.65f, 0f);
		label.transform.rotation = transform.rotation;
		label.transform.LookAt(transform.position);
		label.transform.Rotate(0f, 180f, 0f);
	}

	public static void InfoGun()
	{
		MakeGun(l: true, () =>
		{
			VRRig r = lockedrig;
			Player val = RigUtils.PlayerFromRig(r);
			if (GuiPlugin.items.Count == 0)
			{
				foreach (CosmeticItem allCosmetic in CosmeticsController.instance.allCosmetics)
				{
					if (GetCos(r).Contains(allCosmetic.itemName) && !GuiPlugin.items.Contains(allCosmetic) && allCosmetic.overrideDisplayName != "Slingshot" && !allCosmetic.overrideDisplayName.Contains("CHICKEN STEP") && (int)allCosmetic.itemCategory != 24)
					{
						GuiPlugin.items.Add(allCosmetic);
					}
				}
			}
			List<CosmeticItem> source = GuiPlugin.items.OrderBy((CosmeticItem q) =>
			{
				return r.CheckCosmeticAge(q.itemName);
			}).ToList();
			string text = string.Concat("Name: " + val.NickName, "\n", "ID: " + val.UserId, "\n", "Color: " + Mathf.RoundToInt(r.playerColor.r * 9f) + ", " + Mathf.RoundToInt(r.playerColor.g * 9f) + ", " + Mathf.RoundToInt(r.playerColor.b * 9f), "\n", "Platform: " + GuiPlugin.GetAllPlayersPlats(all: false, val), "\n", "FPS: " + Create(r).Field(Field(12)).GetValue().ToString(), "\n", "Total Cosmetics: " + GuiPlugin.items.Count, "\n", "Turn Type: " + Create(r).Field(Field(31)).GetValue().ToString(), "\n", "Turn Speed: " + Create(r).Field(Field(32)).GetValue().ToString(), (GuiPlugin.items.Count > 0) ? ("\nOldest: " + source.Last().overrideDisplayName + " | " + r.CheckCosmeticAge(source.Last().itemName) + "d") : "", (GuiPlugin.items.Count > 0) ? ("\nNewest: " + source.First().overrideDisplayName + " | " + r.CheckCosmeticAge(source.First().itemName) + "d") : "");
			GameObject val2 = new GameObject("InfoLabel");
			val2.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
			TextMesh orAddComponent = GTExt.GetOrAddComponent<TextMesh>(val2);
			orAddComponent.color = Color.white;
			orAddComponent.fontSize = 22;
			orAddComponent.fontStyle = (FontStyle)1;
			orAddComponent.characterSize = 0.1f;
			orAddComponent.anchor = (TextAnchor)4;
			orAddComponent.alignment = (TextAlignment)0;
			orAddComponent.text = text;
			val2.transform.position = GetGunHand().position + new Vector3(0f, 0.2f, 0f);
			val2.transform.LookAt(OwnRig().headMesh.transform);
			val2.transform.Rotate(0f, 180f, 0f);
			Object.Destroy((Object)(object)val2, Time.deltaTime);
		}, () =>
		{
			if (GuiPlugin.items.Count > 0)
			{
				GuiPlugin.items.Clear();
			}
		});
	}

	public static void ModGun()
	{
		MakeGun(l: true, () =>
		{
			VRRig p = lockedrig;
			Player val = RigUtils.PlayerFromRig(p);
			string text = "";
			foreach (object item in ((Dictionary<object, object>)(object)val.CustomProperties).Keys.Where((object e) => !e.Equals("didTutorial") && !e.Equals("platform") && !e.Equals("mothershipId") && !e.Equals(WristMenu.cus[0])))
			{
				text = text + item.ToString() + "\n";
			}
			if (text == "")
			{
				text = "No Mods Found";
			}
			GameObject val2 = new GameObject("check");
			val2.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
			TextMesh orAddComponent = GTExt.GetOrAddComponent<TextMesh>(val2);
			orAddComponent.color = Color.white;
			orAddComponent.fontSize = 20;
			orAddComponent.fontStyle = (FontStyle)1;
			orAddComponent.characterSize = 0.1f;
			orAddComponent.anchor = (TextAnchor)4;
			orAddComponent.alignment = (TextAlignment)0;
			orAddComponent.text = text;
			val2.transform.position = GetGunHand().position + new Vector3(0f, 0.2f, 0f);
			val2.transform.LookAt(OwnRig().headMesh.transform);
			val2.transform.Rotate(0f, 180f, 0f);
			Object.Destroy((Object)(object)val2, Time.deltaTime);
		});
	}

	public static void FixHit(VRRig r, bool a = false)
	{
		if (!CanHit(r))
		{
			return;
		}
		new Delay().D(a ? 0.088f : 0.18f, () =>
		{
			SnowballThrowable snow = GetSnow("Mento", 0);
			try
			{
				((Component)snow).gameObject.transform.position = ((Component)r).transform.position;
				((HoldableObject)snow).DropItemCleanup();
			}
			catch
			{
			}
		});
	}

	public static void Aimbot(float d)
	{
		if (IsGameMode((GameModeType)3))
		{
			VRRig val = OwnRig();
			if ((Object)(object)val.projectileWeapon == (Object)null)
			{
				return;
			}
			VRRig val2 = val;
			ProjectileWeapon projectileWeapon = val.projectileWeapon;
			LJLabel((Slingshot)(object)((projectileWeapon is Slingshot) ? projectileWeapon : null), (d == 1.9f) ? "SoftAim" : "HardAim");
			if (((TransferrableObject)val.projectileWeapon).InHand())
			{
				if (!WristMenu.ljoy)
				{
					return;
				}
				{
					foreach (SlingshotProjectile pp in (from p in Mods.F<SlingshotProjectile>(5f)
						where p.projectileOwner == NetworkSystem.Instance.LocalPlayer && ((Component)p).gameObject.activeSelf && (((Object)p).name.Contains("Slingshot") || ((Object)p).name.Contains("Bow") || ((Object)p).name.Contains("Arrow"))
						select p).ToList())
					{
						SoftAimVisual(pp);
						try
						{
							List<VRRig> list = RigUtils.AllRigs(i: false).Where((VRRig r) =>
							{
								return CanHit(r) && Vector3.Distance(((Component)pp).gameObject.transform.position, ((Component)r).transform.position) < d;
							}).ToList();
							if (list.Count > 0)
							{
								val2 = list.OrderBy((VRRig r) =>
								{
									return Vector3.Distance(((Component)r).transform.position, ((Component)pp).transform.position);
								}).FirstOrDefault();
							}
						}
						catch
						{
							val2 = val;
						}
						if ((Object)(object)val2 != (Object)(object)val)
						{
							Create(pp).Field(Field(55)).GetValue<Rigidbody>().MovePosition(((Component)val2).transform.position);
						}
					}
					return;
				}
			}
			if ((Object)(object)al != (Object)null)
			{
				Object.Destroy((Object)(object)al);
				al = null;
			}
		}
		else
		{
			SendNoti(Er + "You Must Be In PaintBrawl!");
			DisableButton("PaintBrawl " + ((d == 1.9f) ? "Soft" : "Hard") + " AimBot");
		}
	}

	public static void LJLabel(Slingshot s, string n)
	{
		if ((Object)(object)s != (Object)null && ((TransferrableObject)s).InHand())
		{
			if ((Object)(object)al == (Object)null)
			{
				al = new GameObject("AL");
				al.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
				TextMesh orAddComponent = GTExt.GetOrAddComponent<TextMesh>(al);
				HideEsp(((Component)orAddComponent).gameObject, 0);
				orAddComponent.color = Color.white;
				orAddComponent.fontSize = 24;
				orAddComponent.fontStyle = (FontStyle)1;
				orAddComponent.characterSize = 0.1f;
				orAddComponent.anchor = (TextAnchor)4;
				orAddComponent.alignment = (TextAlignment)0;
			}
			new Delay().D(0.1f, () =>
			{
				al.GetComponent<TextMesh>().text = n + "\nLJ To Toggle\n" + (WristMenu.ljoy ? "<color=green>On</color>" : "<color=red>Off</color>");
			});
			al.transform.position = ((Component)s).gameObject.transform.position - ((Component)s).gameObject.transform.right * 0.0608f + ((Component)s).gameObject.transform.forward * 0.032f;
			al.transform.LookAt(OwnRig().headMesh.transform);
			al.transform.Rotate(0f, 180f, 0f);
		}
	}

	public static void TagSelf()
	{
		if (!PhotonNetwork.InRoom)
		{
			DisableButton("Tag Self");
			return;
		}
		if (IsGameMode((GameModeType)3) && !Local().IsMasterClient)
		{
			DisableButton("Tag Self");
			return;
		}
		VRRig val = OwnRig();
		if (!ActualTagged(val))
		{
			if (Local().IsMasterClient)
			{
				Tagged(NetworkSystem.Instance.LocalPlayer);
				DisableButton("Tag Self");
			}
			else if (IsGameMode((GameModeType)8))
			{
				Shit();
			}
			else
			{
				((Behaviour)val).enabled = false;
				DrawHandOrbs();
				((Component)val).transform.position = (IsGameMode((GameModeType)2) ? RigUtils.RigFromNet(((GorillaHuntManager)GorillaGameManager.instance).GetTargetOf(NetworkSystem.Instance.LocalPlayer)) : SlowestVel()).rightHandTransform.position;
			}
		}
		else
		{
			if (!Local().IsMasterClient)
			{
				((Behaviour)val).enabled = true;
			}
			DisableButton("Tag Self");
		}
	}

	public static void TagAll()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (IsGameMode((GameModeType)8))
		{
			DisableButton("Tag All");
			return;
		}
		VRRig val = OwnRig();
		GorillaTagger val2 = GIn();
		if (WristMenu.triggerDownR)
		{
			if (IsGameMode((GameModeType)2))
			{
				NetPlayer targetOf = ((GorillaHuntManager)GorillaGameManager.instance).GetTargetOf(NetworkSystem.Instance.LocalPlayer);
				if (targetOf == null)
				{
					return;
				}
				VRRig rig = RigUtils.RigFromNet(targetOf);
				if (Local().IsMasterClient)
				{
					if (ActualTagged(rig))
					{
						Tagged(targetOf);
					}
					return;
				}
				SetSP();
				((Behaviour)val).enabled = false;
				((Component)val).transform.position = ((Component)rig).transform.position + RandomV(1f);
				((Component)val2.myVRRig).transform.position = ((Component)rig).transform.position + RandomV(1f);
				val2.maxTagDistance = float.MaxValue;
				if (Vector3.Distance(SP, ((Component)rig).transform.position) < 5f)
				{
					new Delay().D(0.095f, () =>
					{
						GameMode.ReportTag(rig.Creator);
					});
				}
				val2.StartVibration(false, val2.tapHapticStrength / 6f, 0.05f);
				return;
			}
			if (IsGameMode((GameModeType)3))
			{
				try
				{
					if (Local().IsMasterClient)
					{
						RigUtils.AllNets(i: false).ForEach((NetPlayer r) =>
						{
							Tagged(r);
						});
					}
					else
					{
						FixHit((from e in RigUtils.AllRigs(i: false)
							where CanHit(e)
							select e).OrderBy((VRRig e) =>
						{
							return Vector3.Distance(((Component)e).transform.position, ((Component)OwnRig()).transform.position);
						}).FirstOrDefault());
					}
					return;
				}
				catch
				{
					return;
				}
			}
			if (ActualTagged(val))
			{
				if (!((Behaviour)val).enabled)
				{
					DrawHandOrbs();
				}
				{
					foreach (VRRig rig2 in RigUtils.AllRigs())
					{
						if (!ActualTagged(rig2))
						{
							if (Local().IsMasterClient)
							{
								Tagged(rig2.Creator);
							}
							else
							{
								SetSP();
								((Behaviour)val).enabled = false;
								((Component)val).transform.position = ((Component)rig2).transform.position + RandomV(0.05f);
								((Component)val2.myVRRig).transform.position = ((Component)rig2).transform.position + RandomV(0.05f);
								val2.maxTagDistance = float.MaxValue;
								if (Vector3.Distance(SP, ((Component)rig2).transform.position) < 5f)
								{
									new Delay().D(0.095f, () =>
									{
										GameMode.ReportTag(rig2.Creator);
									});
								}
								val2.StartVibration(false, val2.tapHapticStrength / 6f, 0.05f);
							}
						}
						if (IsEveryoneTagged() && !Local().IsMasterClient && !((Behaviour)val).enabled)
						{
							RPCFlush();
							((Behaviour)val).enabled = true;
						}
					}
					return;
				}
			}
			if (Local().IsMasterClient)
			{
				Tagged(NetworkSystem.Instance.LocalPlayer);
				return;
			}
			((Behaviour)val).enabled = false;
			DrawHandOrbs();
			((Component)val).transform.position = SlowestVel().rightHandTransform.position;
		}
		else
		{
			((Behaviour)val).enabled = true;
		}
	}

	public static void TagAllEx()
	{
		MakeGun(l: true, () =>
		{
			if (IsGameMode((GameModeType)8) || IsGameMode((GameModeType)2))
			{
				DisableButton("Tag All Except");
			}
			else if (!ActualTagged(lockedrig))
			{
				VRRig val = OwnRig();
				GorillaTagger val2 = GIn();
				if (IsGameMode((GameModeType)3))
				{
					try
					{
						if (Local().IsMasterClient)
						{
							LinqUtils.ForEach<VRRig>(from e in RigUtils.AllRigs(i: false)
								where (Object)(object)e != (Object)(object)lockedrig
								select e, (Action<VRRig>)((VRRig r) =>
							{
								Tagged(r.Creator);
							}));
						}
						else
						{
							FixHit((from e in RigUtils.AllRigs(i: false)
								where (Object)(object)e != (Object)(object)lockedrig && CanHit(e)
								select e).OrderBy((VRRig e) =>
							{
								return Vector3.Distance(((Component)e).transform.position, ((Component)OwnRig()).transform.position);
							}).FirstOrDefault());
						}
						return;
					}
					catch
					{
						return;
					}
				}
				if (ActualTagged(val))
				{
					if (!((Behaviour)val).enabled)
					{
						DrawHandOrbs();
					}
					{
						foreach (VRRig rig in from e in RigUtils.AllRigs(i: false)
							where (Object)(object)e != (Object)(object)lockedrig
							select e)
						{
							if (!ActualTagged(rig))
							{
								if (Local().IsMasterClient)
								{
									Tagged(rig.Creator);
								}
								else
								{
									SetSP();
									((Behaviour)val).enabled = false;
									((Component)val).transform.position = ((Component)rig).transform.position + RandomV(0.05f);
									((Component)val2.myVRRig).transform.position = ((Component)rig).transform.position + RandomV(0.05f);
									val2.maxTagDistance = float.MaxValue;
									if (Vector3.Distance(SP, ((Component)rig).transform.position) < 5f)
									{
										new Delay().D(0.095f, () =>
										{
											GameMode.ReportTag(rig.Creator);
										});
									}
									val2.StartVibration(false, val2.tapHapticStrength / 6f, 0.05f);
								}
							}
							if (!((Behaviour)val).enabled)
							{
								RPCFlush();
								((Behaviour)val).enabled = true;
							}
						}
						return;
					}
				}
				if (Local().IsMasterClient)
				{
					Tagged(NetworkSystem.Instance.LocalPlayer);
				}
				else
				{
					((Behaviour)val).enabled = false;
					DrawHandOrbs();
					((Component)val).transform.position = SlowestVel().rightHandTransform.position;
				}
			}
		}, () =>
		{
			((Behaviour)OwnRig()).enabled = true;
		});
	}

	public static void TagAura()
	{
		if (!WristMenu.gripDownR)
		{
			return;
		}
		foreach (VRRig rig in RigUtils.AllRigs(i: false))
		{
			if (!(Vector3.Distance(rig.headMesh.transform.position, OwnRig().head.rigTarget.position) < AURA) || !(IsGameMode((GameModeType)3) ? CanHit(rig) : (!ActualTagged(rig))))
			{
				continue;
			}
			if (Local().IsMasterClient)
			{
				Tagged(rig.Creator);
			}
			else if (IsGameMode((GameModeType)3))
			{
				FixHit(rig);
			}
			else if (IsGameMode((GameModeType)2) ? (((GorillaHuntManager)GorillaGameManager.instance).GetTargetOf(NetworkSystem.Instance.LocalPlayer) == rig.Creator) : ActualTagged(OwnRig()))
			{
				GorillaTagger val = GIn();
				val.maxTagDistance = float.MaxValue;
				Loco().RightHand.controllerTransform.position = rig.headMesh.transform.position;
				Loco().LeftHand.controllerTransform.position = rig.headMesh.transform.position;
				new Delay().D(0.2f, () =>
				{
					GameMode.ReportTag(rig.Creator);
					RPCFlush();
				});
				val.StartVibration(false, val.tapHapticStrength / 6f, 0.05f);
				val.StartVibration(true, val.tapHapticStrength / 6f, 0.05f);
			}
		}
	}

	public static void UnTagAll(int n)
	{
		Master("UnTag All (M)", () =>
		{
			foreach (VRRig item in RigUtils.AllRigs(n == 0))
			{
				UnTag(item.Creator);
			}
			if (n == 0)
			{
				DisableButton("UnTag All (M)");
			}
		});
	}

	public static void Shit()
	{
		try
		{
			re = true;
			if (IsGameMode((GameModeType)8))
			{
				if (!ActualTagged(OwnRig()))
				{
					if (!Local().IsMasterClient)
					{
						TappableGuardianIdol t = (from e in Mods.F<TappableGuardianIdol>(5f)
							where ((Behaviour)e).enabled && ((Component)e).gameObject.activeSelf && ((Behaviour)e).isActiveAndEnabled
							select e).ToList().OrderBy((TappableGuardianIdol e) =>
						{
							return Vector3.Distance(((Component)e).transform.position, ((Component)OwnRig()).transform.position);
						}).FirstOrDefault();
						if (!((Object)(object)t != (Object)null))
						{
							return;
						}
						if (Vector3.Distance(((Component)t).transform.position, ((Component)OwnRig()).transform.position) > 1f)
						{
							((Behaviour)OwnRig()).enabled = false;
							((Component)OwnRig()).transform.position = ((Component)t).transform.position;
							DrawHandOrbs();
							return;
						}
						if (!noti)
						{
							noti = true;
							return;
						}
						new Delay().D(0.03f, () =>
						{
							((Component)OwnRig()).transform.position = ((Component)t).transform.position;
							((Tappable)t).OnTap(1f);
						});
						DrawHandOrbs();
					}
					else
					{
						Tagged(NetworkSystem.Instance.LocalPlayer);
					}
				}
				else
				{
					if (noti)
					{
						SendNoti("[<color=green>GUARD</color>] You Are Now Guard");
						((Behaviour)OwnRig()).enabled = true;
						noti = false;
					}
					DisableButton("Get Guardian");
				}
			}
			else
			{
				((Behaviour)OwnRig()).enabled = true;
				SendNoti(Er + "You Must Be In The Guardian GameMode!");
				DisableButton("Get Guardian");
			}
		}
		catch
		{
		}
	}

	public static async void ChangeTeam(VRRig r, int s)
	{
		Paint(() =>
		{
			GorillaPaintbrawlManager val = (GorillaPaintbrawlManager)GorillaGameManager.instance;
			int actorNumber = r.Creator.ActorNumber;
			if (s == 3 || s == 999)
			{
				val.playerLives[actorNumber] = s;
			}
			else
			{
				float value = ((val.hitCooldown == 0f) ? (Time.time + 0.3f) : (Time.time - (2.78f + ((s == 69) ? 0f : 2f))));
				if (s == 67)
				{
					val.playerStunTimes[actorNumber] = value;
				}
				else if (s == 69)
				{
					val.playerHitTimes[actorNumber] = value;
				}
				else
				{
					val.playerStatusDict[actorNumber] = (PaintbrawlStatus)s;
				}
			}
		});
	}

	public static void Paint(Action s, Action q = null)
	{
		if (Local().IsMasterClient)
		{
			if (IsGameMode((GameModeType)3))
			{
				s();
				return;
			}
			DelayedNoti(Er + "You Must Be In PaintBrawl!");
			q?.Invoke();
		}
		else
		{
			DelayedNoti(Er + "You Must Be Master!");
			q?.Invoke();
		}
	}

	public static void CosBro()
	{
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (CosmeticItem i in Con().allCosmetics.Where((CosmeticItem e) =>
		{
			return e.canTryOn && (int)e.itemCategory != 25;
		}).ToList())
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = i.overrideDisplayName,
				method = () =>
				{
					if (Con().currentCart.Contains(i))
					{
						Con().RemoveItemFromCart(i);
					}
					else
					{
						Con().currentCart.Insert(0, i);
					}
					Con().UpdateShoppingCart();
					DisableButton(i.overrideDisplayName);
					DisableButton("TryOn Cosmetic Browser");
					DeleteBoard();
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Cosmetic And Click It To Add It To Your Cart|TryOn Cosmetic Browser");
	}

	public static void ClearPreset()
	{
		change10 = -1;
		ThemeChangerV1();
		change12 = -1;
		ThemeChangerV3();
		change13 = -1;
		ThemeChangerV4();
		change29 = -1;
		ChangeDisColor();
		change31 = -1;
		ChangeNPColor();
		change34 = -1;
		ChangeSearchBColor();
		change42 = -1;
		ChangeHomeBColor();
		DisableButton("Clear Menu Preset");
	}

	public static void RainbowPreset()
	{
		change10 = 0;
		ThemeChangerV1();
		change12 = -1;
		ThemeChangerV3();
		change13 = -1;
		ThemeChangerV4();
		change29 = 0;
		ChangeDisColor();
		change31 = 0;
		ChangeNPColor();
		change34 = 0;
		ChangeSearchBColor();
		change42 = 0;
		ChangeHomeBColor();
		DisableButton("Rainbow Menu Preset");
	}

	public static void MalachiPreset()
	{
		change4 = 2;
		Changedisconnect();
		change7 = 2;
		Changepagebutton();
		change10 = 7;
		ThemeChangerV1();
		change11 = 5;
		ThemeChangerV2();
		change12 = 6;
		ThemeChangerV3();
		change13 = 6;
		ThemeChangerV4();
		change14 = 141;
		ThemeChangerV5();
		change15 = 113;
		ThemeChangerV6();
		change16 = 6;
		ThemeChangerV7();
		change20 = 0;
		ChangeMenuSize();
		GuiPlugin.but = 10;
		GuiPlugin.tex = 2;
		GuiPlugin.bor = 1;
		GuiPlugin.bac = 10;
		GuiPlugin.solidback = false;
		change28 = -1;
		ChangeFont();
		change29 = 115;
		ChangeDisColor();
		change30 = 141;
		ChangeDisTColor();
		change31 = 7;
		ChangeNPColor();
		change32 = 141;
		ChangeNPTColor();
		change33 = 141;
		ChangeTitleColor();
		change34 = 7;
		ChangeSearchBColor();
		change35 = 113;
		ChangeSearchTColor();
		change36 = 141;
		ChangeInputColor();
		change37 = 7;
		ChangeKeyColor();
		change38 = 113;
		ChangeKeyTColor();
		change39 = 1;
		ChangeKeySize();
		searchB = false;
		ToggleSearch();
		homeB = false;
		ToggleHome();
		change42 = 7;
		ChangeHomeBColor();
		change43 = 113;
		ChangeHomeTColor();
		DisableButton("Malachis Personal Preset");
	}

	public static void RandomPreset()
	{
		change10 = Random.Range(-1, AllColors().Count - 1);
		ThemeChangerV1();
		change11 = Random.Range(-1, AllColors("").Count - 1);
		ThemeChangerV2();
		change12 = Random.Range(-1, AllColors("Clear").Count - 1);
		ThemeChangerV3();
		change13 = Random.Range(-1, AllColors("Clear").Count - 1);
		ThemeChangerV4();
		change14 = Random.Range(-1, AllColors("").Count - 1);
		ThemeChangerV5();
		change15 = Random.Range(-1, AllColors("").Count - 1);
		ThemeChangerV6();
		change28 = Random.Range(-1, 4);
		ChangeFont();
		change29 = Random.Range(-1, AllColors().Count - 1);
		ChangeDisColor();
		change30 = Random.Range(-1, AllColors("").Count - 1);
		ChangeDisTColor();
		change31 = Random.Range(-1, AllColors().Count - 1);
		ChangeNPColor();
		change32 = Random.Range(-1, AllColors("").Count - 1);
		ChangeNPTColor();
		change33 = Random.Range(-1, AllColors("").Count - 1);
		ChangeTitleColor();
		change34 = Random.Range(-1, AllColors().Count - 1);
		ChangeSearchBColor();
		change35 = Random.Range(-1, AllColors("").Count - 1);
		ChangeSearchTColor();
		change42 = Random.Range(-1, AllColors().Count - 1);
		ChangeHomeBColor();
		change43 = Random.Range(-1, AllColors("").Count - 1);
		ChangeHomeTColor();
		DisableButton("Random Menu Preset");
	}

	public static void Vim()
	{
		if (CosDone())
		{
			if ((Object)(object)Find("GTFC Scene") == (Object)null)
			{
				loadg = true;
				SceneManager.LoadScene("GTFC", (LoadSceneMode)1);
				return;
			}
			StoreDisplay[] source = Resources.FindObjectsOfTypeAll<StoreDisplay>();
			StoreDisplay val = source.FirstOrDefault((StoreDisplay d) => d.displayName == "BakeryBox");
			if (val != null)
			{
				LinqUtils.ForEach<DynamicCosmeticStand>((IEnumerable<DynamicCosmeticStand>)val.Stands, (Action<DynamicCosmeticStand>)((DynamicCosmeticStand e) =>
				{
					e.AddToCartButton.Click(true);
				}));
			}
			StoreDisplay val2 = source.FirstOrDefault((StoreDisplay d) => d.displayName == "GTFC2");
			if (val2 != null)
			{
				LinqUtils.ForEach<DynamicCosmeticStand>((IEnumerable<DynamicCosmeticStand>)val2.Stands, (Action<DynamicCosmeticStand>)((DynamicCosmeticStand e) =>
				{
					e.AddToCartButton.Click(true);
				}));
			}
			if (loadg)
			{
				SceneManager.UnloadSceneAsync("GTFC");
				loadg = false;
			}
			DisableButton("Add VIM Cosmetics To Cart");
		}
		else
		{
			SendNoti(Er + "Cosmetics Have Not Yet Been Initialized!");
			DisableButton("Add VIM Cosmetics To Cart");
		}
	}

	public static void Ele(int i, string b, string e = "")
	{
		if (e != "")
		{
			Find("Environment Objects/05Maze_PersistentObjects/GhostReactorElevatorManager/" + e + "/HasVisualComponents/Buttons/Call Elevator Up/CallButton/Button").GetComponent<GorillaPressableButton>().Click(true);
		}
		foreach (GRElevator allElevator in GRElevatorManager._instance.allElevators)
		{
			if (e == "")
			{
				allElevator.PressButton(i);
			}
			if (b.StartsWith("Send"))
			{
				allElevator.PressButton(5);
			}
		}
		DisableButton(b);
	}

	public static void NoSlipWalls()
	{
		noslip = true;
		try
		{
			if (Sur("pit upper slippery wall").overrideIndex != over[5])
			{
				Sur("pit lower slippery wall").overrideIndex = over[5];
				Sur("pit upper slippery wall").overrideIndex = over[5];
			}
		}
		catch
		{
		}
	}

	public static void OffNoSlipWalls()
	{
		if (!noslip)
		{
			return;
		}
		try
		{
			if (Sur("pit upper slippery wall").overrideIndex != over[3])
			{
				Sur("pit lower slippery wall").overrideIndex = over[2];
				Sur("pit upper slippery wall").overrideIndex = over[3];
			}
		}
		catch
		{
		}
		noslip = false;
	}

	public static void NoM(GameObject o)
	{
		SetSP();
		((Behaviour)OwnRig()).enabled = false;
		((Component)OwnRig()).transform.position = o.transform.position + new Vector3(0f, -0.2f, 0f);
		DrawHandOrbs();
		if (Vector3.Distance(SP, o.transform.position) < 0.6f)
		{
			if (Object.op_Implicit((Object)(object)o.GetComponent<TransferrableBall>()))
			{
				((object)o.GetComponent<TransferrableBall>()).GetType().GetMethod(Field(65), BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o.GetComponent<TransferrableBall>(), new object[0]);
			}
			else
			{
				o.GetComponent<TransferrableObject>().WorldShareableRequestOwnership();
			}
		}
	}

	public static GameObject GetObj(string n, string a)
	{
		TransferrableObject t = null;
		if (n.Contains("ID Card"))
		{
			t = Mods.F<TransferrableObject>(5f).FirstOrDefault((TransferrableObject e) => ((Object)e).name == n);
		}
		else if (n == "Ball")
		{
			t = (TransferrableObject)(object)Mods.F<TransferrableBall>(5f).FirstOrDefault((TransferrableBall e) => ((Object)e).name == n);
		}
		else
		{
			ThrowableBug[] array = (from e in Mods.F<ThrowableBug>(5f)
				where ((Object)e).name == n
				select e).ToArray();
			t = (TransferrableObject)(object)array.FirstOrDefault();
			if (!n.Contains("Bat"))
			{
				ThrowableBug[] array2 = array;
				foreach (ThrowableBug val in array2)
				{
					string name = ((Object)((Renderer)((Component)((Component)val).transform.Find("model/PlumpBeetle")).GetComponent<SkinnedMeshRenderer>()).material).name;
					if (name.Contains("PlumpBeetle") && !name.Contains("Alebrije") && ((a == "Bug") ? (!name.Contains("2")) : name.Contains("2")))
					{
						t = (TransferrableObject)(object)val;
					}
				}
			}
		}
		if (!t.IsMyItem() && (t.Dropped() || (int)t.currentState == 0))
		{
			if (!Local().IsMasterClient)
			{
				NoM(((Component)t).gameObject);
				return null;
			}
			new Delay().D(0.7f, () =>
			{
				t.WorldShareableRequestOwnership();
			});
		}
		if (t.IsMyItem() || !PhotonNetwork.InRoom)
		{
			((Behaviour)OwnRig()).enabled = true;
			t.maxDistanceFromOriginBeforeRespawn = float.MaxValue;
			t.maxDistanceFromTargetPlayerBeforeRespawn = float.MaxValue;
		}
		return ((Component)t).gameObject;
	}

	public static void GrabObj(GameObject p)
	{
		if (WristMenu.gripDownR)
		{
			MoveObj(p, Loco().RightHand.controllerTransform.position);
		}
		else if (WristMenu.gripDownL)
		{
			MoveObj(p, Loco().LeftHand.controllerTransform.position);
		}
	}

	public static void MoveObj(GameObject g, Vector3 p)
	{
		if ((Object)(object)g == (Object)null)
		{
			DelayedNoti(Er + "Trying To Get Ownership Of Object, Please Be Patient!");
			return;
		}
		g.transform.position = p;
		if (Object.op_Implicit((Object)(object)g.GetComponent<TransferrableBall>()) && (int)((TransferrableObject)g.GetComponent<TransferrableBall>()).currentState == 128)
		{
			((TransferrableObject)g.GetComponent<TransferrableBall>()).rigidbodyInstance.linearVelocity = Vector3.zero;
		}
	}

	public static void SaveMenuFeatures()
	{
		string text = WristMenu.MenuTitle.Substring(21);
		List<List<ButtonInfo>> list = new List<List<ButtonInfo>>
		{
			WristMenu.settingsbuttons.Skip(1).ToList(),
			WristMenu.CatButtons2.Skip(1).ToList(),
			WristMenu.CatButtons3.Skip(1).ToList(),
			WristMenu.CatButtons4.Skip(1).ToList(),
			WristMenu.CatButtons5.Skip(1).ToList(),
			WristMenu.CatButtons6.Skip(1).ToList(),
			WristMenu.CatButtons7.Skip(1).ToList(),
			WristMenu.CatButtons8.Skip(1).ToList(),
			WristMenu.CatButtons9.Skip(1).ToList(),
			(from buttonInfo in WristMenu.CatButtons10.Skip(1)
				where buttonInfo.buttonText != "Keyboard"
				select buttonInfo).ToList()
		};
		List<string> u = new List<string> { "THESE FEATURES ARE FROM THE MALACHIS MENU REBORN " + text + "\n\n" + I("LETTER MEANINGS") + "\n" + WristMenu.LetterMeanings + "\n\n" + I("TOTAL MODS") + "\n" + (GetAllButtons().Count - 11) };
		u.Add("\n" + I($"SETTINGS | {list[0].Count} MODS"));
		list[0].ForEach((ButtonInfo e) =>
		{
			u.Add(e.n);
		});
		u.Add("\n" + I("ENABLED"));
		u.Add("MODS THAT YOU HAVE ENABLED WILL BE IN THIS CATEGORY");
		u.Add("\n" + I("FAVORITE"));
		u.Add("MODS THAT YOU HAVE FAVORITED WILL BE IN THIS CATEGORY");
		u.Add("\n" + I($"OP | {list[1].Count} MODS"));
		list[1].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"RIG | {list[2].Count} MODS"));
		list[2].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"MOVEMENT | {list[3].Count} MODS"));
		list[3].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"SUPERTAG | {list[4].Count} MODS"));
		list[4].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"BLOCK | {list[5].Count} MODS"));
		list[5].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"SPAMMER | {list[6].Count} MODS"));
		list[6].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"VISUAL | {list[7].Count} MODS"));
		list[7].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"TAG | {list[8].Count} MODS"));
		list[8].ForEach((ButtonInfo e) =>
		{
			u.Add(e.buttonText);
		});
		u.Add("\n" + I($"OTHER | {list[9].Count} MODS"));
		list[9].ForEach((ButtonInfo buttonInfo) =>
		{
			u.Add(buttonInfo.buttonText);
		});
		string text2 = "\n\n⫷>[____GUI FEATURES____]<⫸\n\n[__MENU__]\n Lets you use mods from the menu on the gui\n\n[__LOBBY__]\n> Info\n Shows info of the lobby you're in such as room code, map, amount of players, gamemode and more\n\n> Joiner\n - (text box)\n - Join Room\n - Join Random Room\n - Join Random Room In Current Map\n - Join Random Priv Room\n - Rejoin Last Room\n - Change GameMode\n - Change Queue\n\n> PlayerList\n Shows info about everyone. To the right of their info there are 2 buttons: \"Cosmetics\" & \"More\". \"Cosmetics\" shows all of their cosmetics, if equipped, category, and when they bought them. \"More\" shows extra info of the player. In More if you click Mods it acts as a basic mod checker.\n\n> Name & Color\n - (text box)\n - Set Name\n - (red slider)\n - (green slider)\n - (blue slider)\n - Set Color\n - Change Turn Type\n - Change Turn Speed\n - Change Mic Input\n\n> My Cosmetics\n Shows all the cosmetics you own. Clicking one will toggle the cosmetic\n\n[__OTHER__]\n> WASD\n - WASD Enabled\n - WASD No Gravity\n - WASD Speed Changer\n (Controller Emulator)\n - L/R Trigger\n - L/R Grip\n - A/B/X/Y\n - L/R Joystick\n - Emulate PC Gun\n - First Person Camera\n\n> GUI Settings\n - Button Color\n - Text Color\n - Border Color\n - Background Color\n - Solid Background\n - Draggable GUI\n - Save Settings\n - Load Settings\n\n> Keybinds\n [ (Left Bracket) = Recenter GUI\n \\ (Backslash) = Disable All Mods\n Insert (FN + Delete) = Toggle GUI\n Q = Hold Q And Press A Mod In \"Menu\" To Favorite It\n";
		u.Add(text2 + "\n\nMENU IS AVAILABLE AT THESE LINKS: https://discord.gg/2AmFSzYeAV  |  https://github.com/Malachi-the-modder/ownerids/releases/latest");
		File.WriteAllLines("Malachis_Menu_Reborn\\Menu_Features_" + text + ".txt", u);
		string[] files = Directory.GetFiles("Malachis_Menu_Reborn");
		foreach (string text3 in files)
		{
			if (text3.Contains("Menu_Features_") && !text3.EndsWith(text + ".txt"))
			{
				File.Delete(text3);
			}
		}
	}

	public static void GetAllIDs()
	{
		if (!PhotonNetwork.InRoom)
		{
			DisableButton("Get All IDS");
		}
		string text = "IDS GRABBED WITH " + WristMenu.MenuTitle.ToUpper() + " | DISCORD.GG/2AmFSzYeAV\nROOM: " + PhotonNetwork.CurrentRoom.Name + " | REGION: " + NetworkSystem.Instance.CurrentRegion.Replace("/*", "").ToUpper() + " | QUEUE: " + GuiPlugin.GetQueOrGameMode(1) + " | MODE: " + GuiPlugin.GetQueOrGameMode(0).ToUpper() + "\n\n";
		foreach (Player item in RigUtils.AllPlayers())
		{
			VRRig val = RigUtils.RigFromPlayer(item);
			Color playerColor = val.playerColor;
			string text2 = "";
			foreach (string key in sc.Keys)
			{
				if (GetCos(val).Contains(key))
				{
					text2 = text2 + sc[key] + ", ";
				}
			}
			text = text + "NAME: " + item.NickName + " | ID: " + item.UserId + " | COLOR CODE: " + Mathf.RoundToInt(playerColor.r * 9f) + Mathf.RoundToInt(playerColor.g * 9f) + Mathf.RoundToInt(playerColor.b * 9f) + " | PLATFORM: " + GuiPlugin.GetAllPlayersPlats(all: false, item).ToUpper() + ((text2 != "") ? (" | SPECIAL COSMETICS: " + text2) : "") + "\n";
		}
		File.WriteAllText("Malachis_Menu_Reborn/Player_IDS.txt", text);
		SendNoti("[<color=blue>IDS</color>] Saved All IDS To Your Game Folder!");
		DisableButton("Get All IDS");
	}

	public static void AutoBigSnowballs()
	{
		try
		{
			GrowingSnowballThrowable[] ty = Mods.GetTy<GrowingSnowballThrowable>();
			foreach (GrowingSnowballThrowable val in ty)
			{
				if (val.SizeLevel != 5)
				{
					((object)val).GetType().GetMethod(Field(PhotonNetwork.InRoom ? 1 : 60), BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(val, new object[1] { 5 });
				}
			}
		}
		catch
		{
		}
	}

	public static void JoinRandomGhostCode()
	{
		string[] array = new string[11]
		{
			"SREN17", "SREN18", "DAISY09", "PBBV", "J3VU", "ECHO", "STATUE", "BANSHEE", "NULL", "RUN",
			"TIPTOE"
		};
		GuiPlugin.Join(0, array[Random.Range(0, array.Length)]);
		DisableButton("Join Ghost Codes");
	}

	public static void PaintMat(VRRig v, int a = -1)
	{
		new Delay().D(matd, () =>
		{
			mat++;
			if (mat > 12)
			{
				mat = 1;
			}
			List<VRRig> list = ((a == -1) ? new List<VRRig> { v } : ((a == 0) ? RigUtils.AllRigs(i: false) : (from e in RigUtils.AllRigs(i: false)
				where (Object)(object)e != (Object)(object)v
				select e).ToList()));
			if (mat == 1)
			{
				foreach (VRRig item in list)
				{
					UnTag(item.Creator);
					ChangeTeam(item, 1);
				}
			}
			if (mat == 2)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 67);
				});
			}
			if (mat == 3)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 69);
				});
			}
			if (mat == 4)
			{
				list.ForEach((VRRig r) =>
				{
					Tagged(r.Creator);
				});
			}
			if (mat == 5)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 2);
				});
			}
			if (mat == 6)
			{
				list.ForEach((VRRig r) =>
				{
					UnTag(r.Creator);
				});
			}
			if (mat == 7)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 67);
				});
			}
			if (mat == 8)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 69);
				});
			}
			if (mat == 9)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 4);
				});
			}
			if (mat == 10)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 67);
				});
			}
			if (mat == 11)
			{
				list.ForEach((VRRig r) =>
				{
					ChangeTeam(r, 69);
				});
			}
			if (mat == 12)
			{
				list.ForEach((VRRig r) =>
				{
					Tagged(r.Creator);
				});
			}
		});
	}

	public static void GuardMeth(string b, Action s)
	{
		if (IsGameMode((GameModeType)8))
		{
			if (ActualTagged(OwnRig()))
			{
				new Delay().D(0.5f, () =>
				{
					s();
				});
			}
			else
			{
				DelayedNoti(Er + "You Must Be Guardian!");
			}
		}
		else
		{
			SendNoti(Er + "You Must Be In The Guardian GameMode!");
			DisableButton(b.StartsWith("Give") ? b : ("Guardian " + b));
		}
	}

	public static void Throw(VRRig rig, Vector3 t)
	{
		NetworkView val = RigUtils.NetViewFromRig(rig);
		val.SendRPC(RPC(8), rig.Creator, new object[3] { true, false, false });
		val.SendRPC(RPC(11), rig.Creator, new object[1] { t });
	}

	public static void Brac(bool e, int i)
	{
		if (i == 2)
		{
			if (!PhotonNetwork.InRoom)
			{
				OwnRig().nonCosmeticLeftHandItem.EnableItem(e);
				OwnRig().nonCosmeticRightHandItem.EnableItem(e);
				return;
			}
			GIn().myVRRig.SendRPC(RPC(9), (RpcTarget)0, new object[2] { e, true });
			GIn().myVRRig.SendRPC(RPC(9), (RpcTarget)0, new object[2] { e, false });
		}
		else if (!PhotonNetwork.InRoom)
		{
			if (i == 1)
			{
				OwnRig().nonCosmeticLeftHandItem.EnableItem(e);
			}
			else
			{
				OwnRig().nonCosmeticRightHandItem.EnableItem(e);
			}
		}
		else
		{
			GIn().myVRRig.SendRPC(RPC(9), (RpcTarget)0, new object[2]
			{
				e,
				i == 1
			});
		}
	}

	public static async void Speci(PhotonView pv, string m, int[] a, object[] p)
	{
		LoadBalancingPeer loadBalancingPeer = PhotonNetwork.NetworkingClient.LoadBalancingPeer;
		Hashtable val = new Hashtable();
		val.Add((byte)0, (object)pv.ViewID);
		val.Add((byte)2, (object)PhotonNetwork.ServerTimestamp);
		val.Add((byte)3, (object)m);
		val.Add((byte)4, (object)p);
		val.Add((byte)5, (object)(byte)PhotonNetwork.PhotonServerSettings.RpcList.IndexOf(m));
		RaiseEventOptions val2 = new RaiseEventOptions
		{
			TargetActors = a
		};
		SendOptions val3 = new SendOptions
		{
			DeliveryMode = (DeliveryMode)3,
			Encrypt = false
		};
		val3.Reliability = true;
		loadBalancingPeer.OpRaiseEvent((byte)200, (object)val, val2, val3);
	}

	public static void Sound(int s)
	{
		if (!WristMenu.gripDownR)
		{
			return;
		}
		new Delay().D(0.2f, () =>
		{
			if (PhotonNetwork.InRoom)
			{
				GIn().myVRRig.SendRPC(RPC(6), (RpcTarget)0, new object[3] { s, false, 999999f });
			}
			else
			{
				GuiPlugin.PT(b: false, s);
			}
		});
	}

	public static void Water(Vector3 p, Quaternion r)
	{
		if (PhotonNetwork.InRoom)
		{
			if (Vector3.Distance(p, ((Component)OwnRig()).transform.position) > 3f)
			{
				((Behaviour)OwnRig()).enabled = false;
				((Component)OwnRig()).transform.position = (((Object)(object)lockedrig == (Object)null) ? p : ((Component)lockedrig).transform.position) + new Vector3(0f, 0.8f, 0f);
				DrawHandOrbs();
				return;
			}
			if (!((Behaviour)OwnRig()).enabled)
			{
				((Component)OwnRig()).transform.position = (((Object)(object)lockedrig == (Object)null) ? p : ((Component)lockedrig).transform.position) + new Vector3(0f, 0.8f, 0f);
				DrawHandOrbs();
			}
			new Delay().D(0.2f, () =>
			{
				GIn().myVRRig.SendRPC(RPC(3), (RpcTarget)0, new object[6] { p, r, 4f, 100f, true, false });
			});
			new Delay().D(1.1f, () =>
			{
				RPCFlush();
			});
		}
		else
		{
			DelayedNoti(Er + "You Must Be In A Room First!");
		}
	}

	public static void Gl(Vector3 p, bool s = false, bool c = true)
	{
		if ((Object)(object)Find("skyjungle") != (Object)null)
		{
			GliderHoldable[] array = Mods.F<GliderHoldable>(5f);
			foreach (GliderHoldable val in array)
			{
				if (((NetworkView)val).GetView.AmOwner || ((NetworkView)val).GetView.AmController)
				{
					if (c)
					{
						((Component)val).gameObject.transform.position = p;
					}
					if (s)
					{
						((Component)val).gameObject.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
					}
				}
				else
				{
					((NetworkHoldableObject)val).OnHover((InteractionPoint)null, (GameObject)null);
				}
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be In Clouds To Use This Mod!");
		}
	}

	public static void GliderMinigun()
	{
		if (!WristMenu.gripDownR)
		{
			return;
		}
		if ((Object)(object)Find("skyjungle") != (Object)null)
		{
			new Delay().D(0.1f, () =>
			{
				GliderHoldable[] array = Mods.F<GliderHoldable>(5f);
				glmi++;
				if (glmi > array.Length)
				{
					glmi = 0;
				}
				GliderHoldable val = array[glmi];
				if (((NetworkView)val).GetView.AmOwner || ((NetworkView)val).GetView.AmController)
				{
					((Component)val).gameObject.transform.position = Loco().RightHand.controllerTransform.position + new Vector3(0f, 0.5f, 0f);
					((Component)val).gameObject.transform.rotation = Loco().RightHand.controllerTransform.rotation;
					Rigidbody component = ((Component)val).gameObject.GetComponent<Rigidbody>();
					component.isKinematic = false;
					component.useGravity = true;
					component.linearVelocity = Loco().RightHand.controllerTransform.forward - GIn().rightHandTransform.up * 35f;
				}
				else
				{
					((NetworkHoldableObject)val).OnHover((InteractionPoint)null, (GameObject)null);
				}
			});
		}
		else
		{
			DelayedNoti(Er + "You Must Be In Clouds To Use This Mod!");
		}
	}

	public static async void FlingRope(GorillaRopeSwing g, Vector3 v, Transform n = null, bool h = false)
	{
		SetSP();
		n = n ?? g.nodes.Last();
		if (Vector3.Distance(SP, n.position) > 5f && !h)
		{
			((Behaviour)OwnRig()).enabled = false;
			((Component)OwnRig()).transform.position = n.position;
			DrawHandOrbs();
		}
		else
		{
			if (!((Behaviour)OwnRig()).enabled)
			{
				DrawHandOrbs();
				((Component)OwnRig()).transform.position = n.position;
			}
			if (Vector3.Distance(SP, n.position) < 5f)
			{
				new Delay().D(0.15f, () =>
				{
					RopeSwingManager.instance.SendSetVelocity_RPC(g.ropeId, g.GetBoneIndex(n), v, true);
				});
			}
		}
		new Delay().D(2f, () =>
		{
			RPCFlush();
		});
		if (Vector3.Distance(((Component)OwnRig()).transform.position, n.position) > 2f)
		{
			((Behaviour)OwnRig()).enabled = true;
		}
	}

	public static void HoverBoardSpam(Vector3 v)
	{
		if (WristMenu.gripDownR)
		{
			new Delay().D(0.3f, () =>
			{
				FreeHoverboardManager.instance.SendDropBoardRPC(OwnRig().rightHandTransform.position, OwnRig().rightHandTransform.rotation, v, v, Rainbow(), Vector3.one);
			});
		}
	}

	public static void ChangeP(string n, Color c)
	{
		if (n != "")
		{
			Local().NickName = n;
			GorillaComputer.instance.currentName = n;
			GorillaComputer.instance.savedName = n;
			OwnRig().UpdateName();
			PlayerPrefs.SetString("playerName", n);
		}
		PlayerPrefs.SetFloat("redValue", c.r);
		PlayerPrefs.SetFloat("greenValue", c.g);
		PlayerPrefs.SetFloat("blueValue", c.b);
		PlayerPrefs.Save();
		GIn().UpdateColor(c.r, c.g, c.b);
		GorillaComputer.instance.UpdateColor(c.r, c.g, c.b);
	}

	public static void ChangeName(string n, Color c, string b)
	{
		Color val = ((c == Color.clear) ? OwnRig().playerColor : c);
		if (PhotonNetwork.InRoom)
		{
			if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(TouchingPlayerInfo.op_Implicit(Local().UserId)) || CosmeticWardrobeProximityDetector.IsUserNearWardrobe(Local().ActorNumber))
			{
				ChangeP(n, val);
				GIn().myVRRig.SendRPC(RPC(5), (RpcTarget)0, new object[3] { val.r, val.g, val.b });
				return;
			}
			if (!b.Contains("Animate"))
			{
				SendNoti(Er + "Please Be Near A Computer Or A Wardrobe First");
			}
			if (b != "")
			{
				DisableButton(b);
			}
		}
		else
		{
			ChangeP(n, val);
		}
	}

	public static void BlockObject(int n)
	{
		string n2 = "1858470402";
		string text = "Environment Objects/MonkeBlocksRoomPersistent/";
		Vector3 zero = Vector3.zero;
		if (n == 0)
		{
			DropBlock(n2, Find(text + "DropZones/DropZone_Hallway").transform.position + new Vector3(Random.Range(-1.13f, 1.2f), Random.Range(-0.8f, 1.248f), Random.Range(-0.35f, 0.65f)), zero, Quaternion.Euler(new Vector3(180f, 120f, 90f)), r: true);
		}
		if (n == 1)
		{
			DropBlock(n2, Find(text + "AtticScoreBoard").transform.position + new Vector3(Random.Range(1.7f, 2.2f), Random.Range(0f, 0.8f), Random.Range(0.9f, 1.72f)), zero, Quaternion.Euler(new Vector3(180f, 53f, 90f)), r: true);
		}
		if (n == 2)
		{
			DropBlock(n2, Find(text + "MonkeBlocksComputer/GorillaComputerObject/ComputerUI").transform.position + new Vector3(Random.Range(-0.4f, 0.75f), Random.Range(0.06f, 0.26f), Random.Range(-0.44f, 0.21f)), zero, Quaternion.Euler(new Vector3(180f, 90f, 180f)), r: true);
		}
		if (n == 3)
		{
			DropBlock(n2, Find("MonkeBlocksRoomScene/GameModeSelector_MonkeBlocks (prefab)").transform.position + new Vector3(Random.Range(-0.007f, 0.08f), Random.Range(0.34f, 0.73f), Random.Range(-0.49f, -0.2f)), zero, Quaternion.Euler(new Vector3(180f, 100f, 90f)), r: true);
		}
		if (n == 4)
		{
			string text2 = text + "ShrinkStations/SnapPieceShrink01";
			string text3 = "/ScaleRoot/Builder_ShrinkStation/Builder_ShrinkStation_Glass";
			GameObject[] array = new GameObject[4]
			{
				Find(text2 + text3),
				Find(text2 + " (1)" + text3),
				Find(text2 + " (2)" + text3),
				Find(text2 + " (3)" + text3)
			};
			DropBlock(n2, array[Random.Range(0, array.Length)].transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.5f), Random.Range(-0.1f, 0.1f)), zero, array[Random.Range(0, array.Length)].transform.rotation, r: true);
		}
		if (n == 5)
		{
			DropBlock(n2, ((Component)bt().allPrivatePlots[Random.Range(0, bt().allPrivatePlots.Count)]).transform.position + new Vector3(Random.Range(-0.45f, 0.45f), Random.Range(0f, 1.2f), Random.Range(-0.45f, 0.45f)), zero, Quaternion.identity, r: true);
		}
		if (n == 6)
		{
			DropBlock(n2, Find("Environment Objects/05Maze_PersistentObjects/GhostReactorElevatorManager/MonkeBlocksElevator/HasVisualComponents/GhostReactor_Elevator/GhostReactor_Elevator_Door_Top").transform.position + new Vector3(Random.Range(0.6f, 0.7f), Random.Range(0.91f, -0.62f), Random.Range(-1.3f, 0.1f)), zero, Quaternion.Euler(new Vector3(0f, 355f, 85f)), r: true);
		}
		if (n == 7)
		{
			DropBlock(n2, new Vector3(Random.Range(-121.48f, -123.9f), Random.Range(18.8016f, 20.8f), -233.4f), zero, Quaternion.Euler(new Vector3(0f, 75f, 85f)), r: true);
		}
	}

	public static async void Block(BuilderPiece pie, Vector3 po, Vector3 ve, Quaternion ro, bool r, float d, int ty = -1, RpcTarget tr = (RpcTarget)0, Player pla = null)
	{
		new Delay().D(d, async () =>
		{
			if (Local().IsMasterClient)
			{
				int typ = ((ty != -1 || (Object)(object)pie == (Object)null) ? ty : pie.pieceType);
				if (r)
				{
					int id = bt().CreatePieceId();
					object[] pa = new object[8]
					{
						typ,
						id,
						BitPackUtils.PackWorldPosForNetwork(po),
						BitPackUtils.PackQuaternionForNetwork(ro),
						0,
						(byte)4,
						1,
						Local()
					};
					if (pla == null)
					{
						((MonoBehaviourPun)bt().builderNetworking).photonView.RPC(RPC(2), tr, pa);
					}
					else
					{
						((MonoBehaviourPun)bt().builderNetworking).photonView.RPC(RPC(2), pla, pa);
					}
				}
				else
				{
					int id2 = bt().CreatePieceId();
					PhotonView v = ((MonoBehaviourPun)bt().builderNetworking).photonView;
					Player p = Local();
					Transform t = OwnRig().leftHandTransform;
					Vector3 og = t.position;
					object[][] pa2 = new object[3][]
					{
						new object[8]
						{
							typ,
							id2,
							BitPackUtils.PackWorldPosForNetwork(((Component)OwnRig()).transform.position + new Vector3(0f, 0.9f, 0f)),
							BitPackUtils.PackQuaternionForNetwork(Quaternion.identity),
							0,
							(byte)4,
							1,
							p
						},
						new object[5]
						{
							cmdId++,
							id2,
							true,
							BitPackUtils.PackHandPosRotForNetwork(t.position, t.rotation),
							p
						},
						new object[7]
						{
							cmdId++,
							id2,
							po,
							ro,
							ve,
							ve,
							p
						}
					};
					if (pla == null)
					{
						v.RPC(RPC(2), tr, pa2[0]);
						v.RPC(RPC(1), tr, pa2[1]);
						if (Vector3.Distance(t.position, po) > 2.5f)
						{
							t.position = po;
						}
						v.RPC(RPC(0), tr, pa2[2]);
						t.position = og;
					}
					else
					{
						if (IsEnabled("Block Crash All Except (M)"))
						{
							int[] i = (from e in RigUtils.AllPlayers(i: false)
								where e != RigUtils.PlayerFromRig(lockedrig)
								select e.ActorNumber).ToArray();
							Speci(v, RPC(2), i, pa2[0]);
							Speci(v, RPC(1), i, pa2[1]);
							if (Vector3.Distance(t.position, po) > 2.5f)
							{
								t.position = po;
							}
							Speci(v, RPC(0), i, pa2[2]);
						}
						else
						{
							v.RPC(RPC(2), pla, pa2[0]);
							v.RPC(RPC(1), pla, pa2[1]);
							if (Vector3.Distance(t.position, po) > 2.5f)
							{
								t.position = po;
							}
							v.RPC(RPC(0), pla, pa2[2]);
						}
						t.position = og;
					}
				}
			}
			else
			{
				BuilderPiece pi = (from val in Mods.F<BuilderPiece>(5f)
					where ((Component)val).gameObject.activeInHierarchy
					where val.pieceType == ty
					where !val.isBuiltIntoTable
					select val).Where((BuilderPiece val) =>
				{
					return val.CanPlayerGrabPiece(Local().ActorNumber, ((Component)val).transform.position);
				}).Where((BuilderPiece val) =>
				{
					return Vector3.Distance(((Component)val).transform.position, Loco().LeftHand.controllerTransform.position) < 2.5f;
				}).OrderBy((BuilderPiece val) =>
				{
					return Vector3.Distance(((Component)val).transform.position, Loco().LeftHand.controllerTransform.position);
				})
					.FirstOrDefault() ?? null;
				if ((Object)(object)pi == (Object)null)
				{
					pi = (from val in Mods.F<BuilderPiece>(5f)
						where ((Component)val).gameObject.activeInHierarchy
						where !val.isBuiltIntoTable
						select val).Where((BuilderPiece val) =>
					{
						return val.CanPlayerGrabPiece(Local().ActorNumber, ((Component)val).transform.position);
					}).Where((BuilderPiece val) =>
					{
						return Vector3.Distance(((Component)val).transform.position, Loco().LeftHand.controllerTransform.position) < 2.5f;
					}).OrderBy((BuilderPiece val) =>
					{
						return Vector3.Distance(((Component)val).transform.position, Loco().LeftHand.controllerTransform.position);
					})
						.FirstOrDefault() ?? null;
				}
				if (!((Object)(object)pi == (Object)null))
				{
					bt().builderNetworking.RequestGrabPiece(pi, true, Vector3.zero, Quaternion.identity);
					bt().builderNetworking.RequestDropPiece(pi, po, ro, ve, ve);
				}
			}
		});
	}

	public static async void DropBlock(string n, Vector3 po, Vector3 v, Quaternion ro, bool r, int i = -1, VRRig rr = null)
	{
		if (PhotonNetwork.InRoom)
		{
			if ((Object)(object)bt() != (Object)null)
			{
				if (!r && n == "1858470402")
				{
					DelayedNoti(Er + "You Must Turn On Floating Blocks For Arm Block Mods");
					return;
				}
				float d = 0.0255f;
				if (n == "random")
				{
					List<int> p = bt().builderPool.piecePoolLookup.Keys.Where((int e) => e != 1858470402 && e != 764108395).ToList();
					Block(null, po, v, ro, r, d, p[Random.Range(0, p.Count)], (RpcTarget)0);
					return;
				}
				int[] b2 = new int[5] { -741198570, -1447051713, -1961073389, 577606505, -1468322569 };
				Vector3 crp = Find("Environment Objects/MonkeBlocksRoomPersistent/RoomGeo/BuilderFactory (1)/BuilderFactory_FloorCollision/Cube").transform.position + new Vector3(0f, 3.255f, 0f);
				int num;
				switch (n)
				{
				case "crash":
					Block(null, crp, v, ro, r: false, d, b2[Random.Range(0, b2.Length)], (RpcTarget)1, RigUtils.PlayerFromRigSafe(lockedrig ?? rr));
					return;
				case "fling":
					Block(null, po, v, ro, r: false, 0.03f, -566818631, (RpcTarget)0, RigUtils.PlayerFromRigSafe(lockedrig));
					return;
				case "":
					num = ((i != -1) ? 1 : 0);
					break;
				default:
					num = 0;
					break;
				}
				if (num != 0)
				{
					Block(null, po, v, ro, r, 0.02175f, i, (RpcTarget)0);
				}
				else if (n == "log")
				{
					Block(null, new Vector3(-118.28f, 18.61f, -231.55f), v, ro, r, d, 1475373407, (RpcTarget)1);
				}
				else if (n != "crash" && n != "log" && n != "fling" && n != "")
				{
					Block(null, po, v, ro, r, d, int.Parse(n), (RpcTarget)0, ((Object)(object)rr == (Object)null) ? null : rr.Creator.GetPlayerRef());
				}
			}
			else
			{
				DelayedNoti(Er + "You Must Be In The MonkeBlocks Map!");
			}
		}
		else
		{
			DelayedNoti(Er + "You Must Be In A Room!");
		}
	}

	public static async void RPCFlush()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		try
		{
			int i = int.MaxValue;
			MonkeAgent.instance.rpcCallLimit = i;
			MonkeAgent.instance.rpcErrorMax = i;
			MonkeAgent.instance.logErrorMax = i;
			Create(MonkeAgent.instance).Field(Field(24)).SetValue((object)float.MaxValue);
			PhotonNetwork.OpCleanActorRpcBuffer(Local().ActorNumber);
			PhotonNetwork.RemoveBufferedRPCs(GIn().myVRRig.ViewID, (string)null, (int[])null);
			PhotonNetwork.SendAllOutgoingCommands();
			try
			{
				MonkeAgent.instance.OnPlayerLeftRoom(NetworkSystem.Instance.LocalPlayer);
			}
			catch
			{
			}
		}
		catch
		{
		}
	}

	public static void OnTouchMethod(VRRig r, bool o, Action shit)
	{
		if (o)
		{
			if ((double)Vector3.Distance(r.rightHandTransform.position, ((Component)OwnRig()).transform.position) < 0.35 || (double)Vector3.Distance(r.leftHandTransform.position, ((Component)OwnRig()).transform.position) < 0.35)
			{
				shit();
			}
		}
		else if ((double)Vector3.Distance(OwnRig().rightHandTransform.position, ((Component)r).transform.position) < 0.35 || (double)Vector3.Distance(OwnRig().leftHandTransform.position, ((Component)r).transform.position) < 0.35)
		{
			shit();
		}
	}

	public static void SaveFavs()
	{
		string text = "";
		foreach (string item in (from e in WristMenu.CatButtons12.Skip(1)
			select e.buttonText).ToList())
		{
			string text2 = item;
			if (item.Contains(":"))
			{
				text2 = item.Split(':')[0].Trim();
			}
			text = text + text2 + "\n";
		}
		File.WriteAllText("Malachis_Menu_Reborn\\Saved_Favs.txt", text);
		WristMenu.Refresh();
	}

	public static void LoadFavs()
	{
		try
		{
			LinqUtils.ForEach<string>((IEnumerable<string>)File.ReadAllLines("Malachis_Menu_Reborn\\Saved_Favs.txt"), (Action<string>)((string n) =>
			{
				WristMenu.CatButtons12.Insert(1, GetButton(n));
			}));
		}
		catch
		{
		}
		WristMenu.Refresh();
	}

	public static void Save1()
	{
		File.WriteAllLines("Malachis_Menu_Reborn\\Saved_Mods.txt", (from e in GetAllButtons(i: false)
			where IsEnabled(e)
			select e.buttonText).ToList());
		if (searching)
		{
			ButtonInfo buttonInfo = WristMenu.CatButtons11.Find((ButtonInfo buttonInfo2) => buttonInfo2.buttonText == "Save Mods");
			if (buttonInfo != null)
			{
				buttonInfo.enabled = false;
			}
		}
		else
		{
			WristMenu.CatButtons1[1].enabled = false;
		}
		WristMenu.Refresh();
		SendNoti("[<color=blue>SAVE</color>] Saved Mods Successfully!");
	}

	public static void Load1()
	{
		try
		{
			LinqUtils.ForEach<string>((IEnumerable<string>)File.ReadAllLines("Malachis_Menu_Reborn\\Saved_Mods.txt"), (Action<string>)((string text) =>
			{
				LinqUtils.ForEach<ButtonInfo>(from e in GetAllButtons(i: false)
					where e.buttonText == text
					select e, (Action<ButtonInfo>)((ButtonInfo e) =>
				{
					e.enabled = true;
				}));
			}));
		}
		catch
		{
		}
		WristMenu.Refresh();
	}

	public static void Save(bool n = true)
	{
		WristMenu.settingsbuttons[1].enabled = false;
		WristMenu.Refresh();
		File.WriteAllLines("Malachis_Menu_Reborn\\Saved_Settings.txt", (from e in WristMenu.settingsbuttons
			where IsEnabled(e) && e.buttonText != "Save Settings"
			select e.buttonText).ToList());
		string text = TriggerPlats + "\n" + change4 + "\n" + right + "\n" + change7 + "\n" + change8 + "\n" + change9 + "\n" + change10 + "\n" + change11 + "\n" + change12 + "\n" + change13 + "\n" + change14 + "\n" + change15 + "\n" + change16 + "\n" + change5 + "\n" + change17 + "\n" + change19 + "\n" + change20 + "\n" + GuiPlugin.open + "\n" + GuiPlugin.but + "\n" + GuiPlugin.tex + "\n" + GuiPlugin.bor + "\n" + GuiPlugin.bac + "\n" + GuiPlugin.solidback + "\n" + GuiPlugin.CanDrag + "\n" + change23 + "\n" + change24 + "\n" + change25 + "\n" + vissoft + "\n" + hidee + "\n" + change26 + "\n" + change27 + "\n" + change28 + "\n" + change29 + "\n" + change30 + "\n" + change31 + "\n" + change32 + "\n" + change33 + "\n" + autosave + "\n" + change34 + "\n" + change35 + "\n" + change36 + "\n" + change37 + "\n" + change38 + "\n" + ironsounds + "\n" + change39 + "\n" + searchB + "\n" + cleardi + "\n" + change40 + "\n" + change41 + "\n" + sided + "\n" + joinnotis + "\n" + homeB + "\n" + change42 + "\n" + change43 + "\n" + change44 + "\n" + change45 + "\n" + change46 + "\n" + change47 + "\n" + rece + "\n" + change48 + "\n" + change49 + "\n" + dis + "\n" + change50 + "\n" + menucol + "\n" + tyname;
		File.WriteAllText("Malachis_Menu_Reborn/Saved_Settings2.txt", text.ToString());
		if (n)
		{
			SendNoti("[<color=blue>SAVE</color>] Saved Settings Successfully!");
		}
	}

	public static void Load()
	{
		try
		{
			string[] array = File.ReadAllLines("Malachis_Menu_Reborn\\Saved_Settings.txt");
			string[] array2 = array;
			foreach (string text in array2)
			{
				foreach (ButtonInfo settingsbutton in WristMenu.settingsbuttons)
				{
					if (settingsbutton.buttonText == text)
					{
						settingsbutton.enabled = true;
					}
				}
			}
			string[] array3 = File.ReadAllText("Malachis_Menu_Reborn/Saved_Settings2.txt").Split(new string[1] { "\n" }, StringSplitOptions.None);
			TriggerPlats = !bool.Parse(array3[0]);
			Changeplat();
			change4 = int.Parse(array3[1]) - 1;
			Changedisconnect();
			right = !bool.Parse(array3[2]);
			Changemenu();
			change7 = int.Parse(array3[3]) - 1;
			Changepagebutton();
			change8 = int.Parse(array3[4]) - 1;
			ChangeOrbColor();
			change9 = int.Parse(array3[5]) - 1;
			ChangeVisualColor();
			change10 = int.Parse(array3[6]) - 1;
			ThemeChangerV1();
			change11 = int.Parse(array3[7]) - 1;
			ThemeChangerV2();
			change12 = int.Parse(array3[8]) - 1;
			ThemeChangerV3();
			change13 = int.Parse(array3[9]) - 1;
			ThemeChangerV4();
			change14 = int.Parse(array3[10]) - 1;
			ThemeChangerV5();
			change15 = int.Parse(array3[11]) - 1;
			ThemeChangerV6();
			change16 = int.Parse(array3[12]) - 1;
			ThemeChangerV7();
			change5 = int.Parse(array3[13]) - 1;
			ChangeTagAura();
			change17 = int.Parse(array3[14]) - 1;
			ChangeWallWalk();
			change19 = int.Parse(array3[15]) - 1;
			ChangeNotiDelay();
			change20 = int.Parse(array3[16]) - 1;
			ChangeMenuSize();
			GuiPlugin.open = !bool.Parse(array3[17]);
			EnabledOrDisableGUI();
			GuiPlugin.but = int.Parse(array3[18]);
			GuiPlugin.tex = int.Parse(array3[19]);
			GuiPlugin.bor = int.Parse(array3[20]);
			GuiPlugin.bac = int.Parse(array3[21]);
			GuiPlugin.solidback = bool.Parse(array3[22]);
			GuiPlugin.CanDrag = bool.Parse(array3[23]);
			change23 = int.Parse(array3[24]) - 1;
			ChangeGunHand();
			change24 = int.Parse(array3[25]) - 1;
			ChangeGunLiner();
			change25 = int.Parse(array3[26]) - 1;
			ChangeTracerPos();
			vissoft = bool.Parse(array3[27]);
			hidee = !bool.Parse(array3[28]);
			Changehide();
			change26 = int.Parse(array3[29]) - 1;
			ChangePS();
			change27 = int.Parse(array3[30]) - 1;
			ChangeLS();
			change28 = int.Parse(array3[31]) - 1;
			ChangeFont();
			change29 = int.Parse(array3[32]) - 1;
			ChangeDisColor();
			change30 = int.Parse(array3[33]) - 1;
			ChangeDisTColor();
			change31 = int.Parse(array3[34]) - 1;
			ChangeNPColor();
			change32 = int.Parse(array3[35]) - 1;
			ChangeNPTColor();
			change33 = int.Parse(array3[36]) - 1;
			ChangeTitleColor();
			autosave = !bool.Parse(array3[37]);
			AutoSave();
			change34 = int.Parse(array3[38]) - 1;
			ChangeSearchBColor();
			change35 = int.Parse(array3[39]) - 1;
			ChangeSearchTColor();
			change36 = int.Parse(array3[40]) - 1;
			ChangeInputColor();
			change37 = int.Parse(array3[41]) - 1;
			ChangeKeyColor();
			change38 = int.Parse(array3[42]) - 1;
			ChangeKeyTColor();
			ironsounds = !bool.Parse(array3[43]);
			IronSounds();
			change39 = int.Parse(array3[44]) - 1;
			ChangeKeySize();
			searchB = !bool.Parse(array3[45]);
			ToggleSearch();
			cleardi = !bool.Parse(array3[46]);
			ClearDis();
			change40 = int.Parse(array3[47]) - 1;
			ChangeWallHand();
			change41 = int.Parse(array3[48]) - 1;
			ChangeAnti();
			sided = !bool.Parse(array3[49]);
			ModSided();
			joinnotis = !bool.Parse(array3[50]);
			ToggleJoin();
			homeB = !bool.Parse(array3[51]);
			ToggleHome();
			change42 = int.Parse(array3[52]) - 1;
			ChangeHomeBColor();
			change43 = int.Parse(array3[53]) - 1;
			ChangeHomeTColor();
			change44 = int.Parse(array3[54]) - 1;
			ChangeNotiSize();
			change45 = int.Parse(array3[55]) - 1;
			ChangeGVColor();
			change46 = int.Parse(array3[56]) - 1;
			ChangeGVTrans();
			change47 = int.Parse(array3[57]) - 1;
			ChangeGVType();
			rece = !bool.Parse(array3[58]);
			Recen();
			change48 = int.Parse(array3[59]) - 1;
			ChangePointerShape();
			change49 = int.Parse(array3[60]) - 1;
			ChangeClickCool();
			dis = !bool.Parse(array3[61]);
			ToggleDis();
			change50 = int.Parse(array3[62]) - 1;
			ChangeArrow();
			menucol = !bool.Parse(array3[63]);
			Togglecoll();
			tyname = !bool.Parse(array3[64]);
			ToggleTyName();
		}
		catch
		{
		}
		RefreshButtons();
		SendNoti("[<color=blue>LOAD</color>] Loaded settings successfully!");
		DisableButton("Load Settings");
	}

	public static void MakeStick(bool l, GameObject p)
	{
		int overrideIndex = p.GetComponent<GorillaSurfaceOverride>().overrideIndex;
		Quaternion rotation = p.transform.rotation;
		Vector3 val = (l ? (-p.transform.right) : p.transform.right) * 0.021f;
		PrimitiveType val2 = (PrimitiveType)3;
		GameObject val3 = CreatePrimitive(val2);
		GameObject val4 = CreatePrimitive(val2);
		GameObject val5 = CreatePrimitive(val2);
		GameObject val6 = CreatePrimitive(val2);
		GameObject val7 = CreatePrimitive(val2);
		val3.transform.position = p.transform.position + (l ? (-p.transform.right) : p.transform.right) * 0.049f;
		val4.transform.position = p.transform.position + (l ? (-p.transform.up) : p.transform.up) * 0.0525f + val;
		val5.transform.position = p.transform.position + (l ? p.transform.up : (-p.transform.up)) * 0.0525f + val;
		val6.transform.position = p.transform.position + (l ? (-p.transform.forward) : p.transform.forward) * 0.09f + val;
		val7.transform.position = p.transform.position + (l ? p.transform.forward : (-p.transform.forward)) * 0.09f + val;
		val3.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
		val4.transform.localScale = new Vector3(0.045f, 0.016f, 0.3825f);
		val5.transform.localScale = new Vector3(0.045f, 0.016f, 0.3825f);
		val6.transform.localScale = new Vector3(0.045f, 0.28f, 0.016f);
		val7.transform.localScale = new Vector3(0.045f, 0.28f, 0.016f);
		List<GameObject> list = new List<GameObject> { val3, val4, val5, val6, val7 };
		foreach (GameObject item in list)
		{
			item.transform.rotation = rotation;
			item.AddComponent<GorillaSurfaceOverride>().overrideIndex = overrideIndex;
			Object.Destroy((Object)(object)item.GetComponent<Renderer>());
		}
		val3.transform.parent = p.transform;
		val4.transform.parent = val3.transform;
		val5.transform.parent = val3.transform;
		val6.transform.parent = val3.transform;
		val7.transform.parent = val3.transform;
		if (l)
		{
			lsp = list;
		}
		else
		{
			rsp = list;
		}
	}

	public static void DisableSticky(bool l)
	{
		if (l)
		{
			if (lsp.Count <= 0)
			{
				return;
			}
			foreach (GameObject item in lsp.ToList())
			{
				Object.Destroy((Object)(object)item.GetComponent<Collider>());
			}
			lsp.Clear();
		}
		else
		{
			if (rsp.Count <= 0)
			{
				return;
			}
			foreach (GameObject item2 in rsp.ToList())
			{
				Object.Destroy((Object)(object)item2.GetComponent<Collider>());
			}
			rsp.Clear();
		}
	}

	public static void Platforms(bool i = false, bool s = false)
	{
		if (TriggerPlats)
		{
			RPlat = WristMenu.triggerDownR;
			LPlat = WristMenu.triggerDownL;
		}
		else
		{
			RPlat = WristMenu.gripDownR;
			LPlat = WristMenu.gripDownL;
		}
		if (RPlat)
		{
			if (!oR && (Object)(object)pr == (Object)null)
			{
				pr = CreatePrimitive((PrimitiveType)3);
				pr.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
				pr.transform.position = new Vector3(0f, (!s) ? (-0.095f) : (-0.01f), 0f) + Loco().RightHand.controllerTransform.position;
				pr.transform.rotation = Loco().RightHand.controllerTransform.rotation;
				pr.AddComponent<GorillaSurfaceOverride>().overrideIndex = pr.GetComponent<GorillaSurfaceOverride>().overrideIndex;
				oR = true;
				or = false;
				if (!i)
				{
					if (change10 == 11)
					{
						ColorChanger colorChanger = pr.AddComponent<ColorChanger>();
						colorChanger.colors = RainbowG();
						colorChanger.Start();
					}
					else
					{
						GradientColorKey[] array = new GradientColorKey[4];
						array[0].color = WristMenu.FirstColor;
						array[0].time = 0f;
						array[1].color = WristMenu.FirstColor;
						array[1].time = 0.3f;
						array[2].color = WristMenu.SecondColor;
						array[2].time = 0.6f;
						array[3].color = WristMenu.FirstColor;
						array[3].time = 1f;
						ColorChanger colorChanger2 = pr.AddComponent<ColorChanger>();
						colorChanger2.colors = new Gradient
						{
							colorKeys = array
						};
						colorChanger2.Start();
					}
				}
				else
				{
					Object.Destroy((Object)(object)pr.GetComponent<Renderer>());
				}
				if (s)
				{
					MakeStick(l: false, pr);
				}
			}
		}
		else if (!or && (Object)(object)pr != (Object)null)
		{
			Object.Destroy((Object)(object)pr.GetComponent<Collider>());
			if (s)
			{
				DisableSticky(l: false);
			}
			Component val = pr.AddComponent(typeof(Rigidbody));
			Rigidbody val2 = (Rigidbody)(object)((val is Rigidbody) ? val : null);
			val2.linearVelocity = Loco().RightHand.velocityTracker.GetAverageVelocity(true, 5f, false);
			Object.Destroy((Object)(object)pr, 2f);
			pr = null;
			oR = false;
			or = true;
		}
		if (LPlat)
		{
			if (oL || !((Object)(object)pl == (Object)null))
			{
				return;
			}
			pl = CreatePrimitive((PrimitiveType)3);
			pl.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
			pl.transform.position = new Vector3(0f, (!s) ? (-0.095f) : (-0.01f), 0f) + Loco().LeftHand.controllerTransform.position;
			pl.transform.rotation = Loco().LeftHand.controllerTransform.rotation;
			pl.AddComponent<GorillaSurfaceOverride>().overrideIndex = pl.GetComponent<GorillaSurfaceOverride>().overrideIndex;
			oL = true;
			ol = false;
			if (!i)
			{
				if (change10 == 11)
				{
					ColorChanger colorChanger3 = pl.AddComponent<ColorChanger>();
					colorChanger3.colors = RainbowG();
					colorChanger3.Start();
				}
				else
				{
					GradientColorKey[] array2 = new GradientColorKey[4];
					array2[0].color = WristMenu.FirstColor;
					array2[0].time = 0f;
					array2[1].color = WristMenu.FirstColor;
					array2[1].time = 0.3f;
					array2[2].color = WristMenu.SecondColor;
					array2[2].time = 0.6f;
					array2[3].color = WristMenu.FirstColor;
					array2[3].time = 1f;
					ColorChanger colorChanger4 = pl.AddComponent<ColorChanger>();
					colorChanger4.colors = new Gradient
					{
						colorKeys = array2
					};
					colorChanger4.Start();
				}
			}
			else
			{
				Object.Destroy((Object)(object)pl.GetComponent<Renderer>());
			}
			if (s)
			{
				MakeStick(l: true, pl);
			}
		}
		else if (!ol && (Object)(object)pl != (Object)null)
		{
			Object.Destroy((Object)(object)pl.GetComponent<Collider>());
			if (s)
			{
				DisableSticky(l: true);
			}
			Component val3 = pl.AddComponent(typeof(Rigidbody));
			Rigidbody val4 = (Rigidbody)(object)((val3 is Rigidbody) ? val3 : null);
			val4.linearVelocity = Loco().LeftHand.velocityTracker.GetAverageVelocity(true, 5f, false);
			Object.Destroy((Object)(object)pl, 2f);
			pl = null;
			oL = false;
			ol = true;
		}
	}

	public static void NewName(string ne, bool d = true, bool c = false)
	{
		ne = Regex.Replace(ne, "<color=#[^>]+>|</color>", "").Replace("▬", "");
		string n = ne.Split(':')[0].Trim();
		GetAllButtons()[GetAllButtons().FindIndex((ButtonInfo x) => x.buttonText.Contains(n))].buttonText = ne;
		if (d)
		{
			DisableButton(c ? n : ne, c);
		}
		else if (searching && usingmenu.Count > 0 && sided)
		{
			WristMenu.ModSidedMenu.Send("spawn");
		}
	}

	public static ButtonInfo GetButton(string n, bool c = false)
	{
		foreach (ButtonInfo allButton in GetAllButtons(i: true, actsearch))
		{
			if (c ? allButton.buttonText.Contains(n) : (allButton.buttonText == n))
			{
				return allButton;
			}
		}
		return null;
	}

	public static ButtonInfo GetButtonO(string n)
	{
		foreach (ButtonInfo item in WristMenu.CatButtons10)
		{
			if (item.buttonText == n)
			{
				return item;
			}
		}
		return null;
	}

	public static List<ButtonInfo> GetAllButtons(bool i = true, bool ii = false, bool c = false)
	{
		List<ButtonInfo> l = new List<ButtonInfo>();
		if (i)
		{
			if (!ii)
			{
				WristMenu.CatButtons11.ForEach((ButtonInfo e) =>
				{
					l.Add(e);
				});
			}
			WristMenu.settingsbuttons.ForEach((ButtonInfo e) =>
			{
				l.Add(e);
			});
		}
		if (c)
		{
			l.Add(WristMenu.CatButtons1[1]);
			l.Add(WristMenu.CatButtons1[2]);
		}
		l.Add(WristMenu.CatButtons12[0]);
		WristMenu.CatButtons2.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons3.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons4.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons5.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons6.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons7.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons8.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons9.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		WristMenu.CatButtons10.ForEach((ButtonInfo e) =>
		{
			l.Add(e);
		});
		return l;
	}

	public static void Settings()
	{
		WristMenu.settingsbuttons[0].enabled = false;
		WristMenu.buttons[0].enabled = false;
		inSettings = !inSettings;
		if (!inSettings && autosave)
		{
			Save(n: false);
		}
		WristMenu.pageNumber = 0;
		WristMenu.Refresh();
	}

	public static void Cat1()
	{
		WristMenu.CatButtons1[0].enabled = false;
		WristMenu.buttons[1].enabled = false;
		inCat1 = !inCat1;
		WristMenu.pageNumber = 0;
		WristMenu.Refresh();
	}

	public static void Cat12()
	{
		WristMenu.CatButtons12[0].enabled = false;
		WristMenu.buttons[2].enabled = false;
		inCat12 = !inCat12;
		WristMenu.pageNumber = 0;
		WristMenu.Refresh();
	}

	public static void Cat2()
	{
		WristMenu.CatButtons2[0].enabled = false;
		WristMenu.buttons[3].enabled = false;
		inCat2 = !inCat2;
		WristMenu.pageNumber = 0;
		WristMenu.Refresh();
	}

	public static void Cat3()
	{
		WristMenu.CatButtons3[0].enabled = false;
		WristMenu.buttons[4].enabled = false;
		inCat3 = !inCat3;
		int pageNumber = 0;
		if (WristMenu.pageSize == 4 && !inCat3)
		{
			pageNumber = 1;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat4()
	{
		WristMenu.CatButtons4[0].enabled = false;
		WristMenu.buttons[5].enabled = false;
		inCat4 = !inCat4;
		int pageNumber = 0;
		if (WristMenu.pageSize == 4 && !inCat4)
		{
			pageNumber = 1;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat5()
	{
		WristMenu.CatButtons5[0].enabled = false;
		WristMenu.buttons[6].enabled = false;
		inCat5 = !inCat5;
		int pageNumber = 0;
		if ((WristMenu.pageSize == 4 || WristMenu.pageSize == 6) && !inCat5)
		{
			pageNumber = 1;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat6()
	{
		WristMenu.CatButtons6[0].enabled = false;
		WristMenu.buttons[7].enabled = false;
		inCat6 = !inCat6;
		int pageNumber = 0;
		if ((WristMenu.pageSize == 4 || WristMenu.pageSize == 6) && !inCat6)
		{
			pageNumber = 1;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat7()
	{
		WristMenu.CatButtons7[0].enabled = false;
		WristMenu.buttons[8].enabled = false;
		inCat7 = !inCat7;
		int pageNumber = ((!inCat7) ? 1 : 0);
		if (WristMenu.pageSize == 4 && !inCat7)
		{
			pageNumber = 2;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat8()
	{
		WristMenu.CatButtons8[0].enabled = false;
		WristMenu.buttons[9].enabled = false;
		inCat8 = !inCat8;
		int pageNumber = ((!inCat8) ? 1 : 0);
		if (WristMenu.pageSize == 4 && !inCat8)
		{
			pageNumber = 2;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat9()
	{
		WristMenu.CatButtons9[0].enabled = false;
		WristMenu.buttons[10].enabled = false;
		inCat9 = !inCat9;
		int pageNumber = ((!inCat9) ? 1 : 0);
		if (WristMenu.pageSize == 4 && !inCat9)
		{
			pageNumber = 2;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void Cat10()
	{
		WristMenu.CatButtons10[0].enabled = false;
		WristMenu.buttons[11].enabled = false;
		inCat10 = !inCat10;
		int pageNumber = ((!inCat10) ? 1 : 0);
		if (WristMenu.pageSize == 4 && !inCat10)
		{
			pageNumber = 2;
		}
		WristMenu.pageNumber = pageNumber;
		WristMenu.Refresh();
	}

	public static void RefreshButtons()
	{
		TriggerPlats = !TriggerPlats;
		Changeplat();
		change4--;
		Changedisconnect();
		right = !right;
		Changemenu();
		change7--;
		Changepagebutton();
		change8--;
		ChangeOrbColor();
		change9--;
		ChangeVisualColor();
		change10--;
		ThemeChangerV1();
		change11--;
		ThemeChangerV2();
		change12--;
		ThemeChangerV3();
		change13--;
		ThemeChangerV4();
		change14--;
		ThemeChangerV5();
		change15--;
		ThemeChangerV6();
		change16--;
		ThemeChangerV7();
		change5--;
		ChangeTagAura();
		change17--;
		ChangeWallWalk();
		change19--;
		ChangeNotiDelay();
		change20--;
		ChangeMenuSize();
		GuiPlugin.open = !GuiPlugin.open;
		EnabledOrDisableGUI();
		change23--;
		ChangeGunHand();
		change24--;
		ChangeGunLiner();
		change25--;
		ChangeTracerPos();
		hidee = !hidee;
		Changehide();
		change26--;
		ChangePS();
		change27--;
		ChangeLS();
		change28--;
		ChangeFont();
		change29--;
		ChangeDisColor();
		change30--;
		ChangeDisTColor();
		change31--;
		ChangeNPColor();
		change32--;
		ChangeNPTColor();
		change33--;
		ChangeTitleColor();
		autosave = !autosave;
		AutoSave();
		change34--;
		ChangeSearchBColor();
		change35--;
		ChangeSearchTColor();
		change36--;
		ChangeInputColor();
		change37--;
		ChangeKeyColor();
		change38--;
		ChangeKeyTColor();
		ironsounds = !ironsounds;
		IronSounds();
		change39--;
		ChangeKeySize();
		searchB = !searchB;
		ToggleSearch();
		cleardi = !cleardi;
		ClearDis();
		change40--;
		ChangeWallHand();
		change41--;
		ChangeAnti();
		sided = !sided;
		ModSided();
		joinnotis = !joinnotis;
		ToggleJoin();
		homeB = !homeB;
		ToggleHome();
		change42--;
		ChangeHomeBColor();
		change43--;
		ChangeHomeTColor();
		change44--;
		ChangeNotiSize();
		change45--;
		ChangeGVColor();
		change46--;
		ChangeGVTrans();
		change47--;
		ChangeGVType();
		rece = !rece;
		Recen();
		change48--;
		ChangePointerShape();
		change49--;
		ChangeClickCool();
		dis = !dis;
		ToggleDis();
		change50--;
		ChangeArrow();
		menucol = !menucol;
		Togglecoll();
		tyname = !tyname;
		ToggleTyName();
	}

	public static void ChangeSearchBColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Search Button Color";
		if (l)
		{
			change34++;
			WristMenu.Searchb = c2.Values.ToList()[change34];
			NewName(b2 + ": " + c2.Keys.ToList()[change34]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change34 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.Searchb = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeSearchTColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Search Button Text Color";
		if (l)
		{
			change35++;
			WristMenu.SearchT = c2.Values.ToList()[change35];
			NewName(b2 + ": " + c2.Keys.ToList()[change35]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change35 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.SearchT = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeHomeBColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Home Button Color";
		if (l)
		{
			change42++;
			WristMenu.Homeb = c2.Values.ToList()[change42];
			NewName(b2 + ": " + c2.Keys.ToList()[change42]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change42 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.Homeb = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeHomeTColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Home Button Text Color";
		if (l)
		{
			change43++;
			WristMenu.HomeT = c2.Values.ToList()[change43];
			NewName(b2 + ": " + c2.Keys.ToList()[change43]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change43 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.HomeT = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeInputColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Search Input Color";
		if (l)
		{
			change36++;
			InputColor = c2.Values.ToList()[change36];
			NewName(b2 + ": " + c2.Keys.ToList()[change36]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change36 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					InputColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeKeyColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Search Keys Color";
		if (l)
		{
			change37++;
			KeyColor = c2.Values.ToList()[change37];
			NewName(b2 + ": " + c2.Keys.ToList()[change37]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change37 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					KeyColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeKeyTColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Search Keys Text Color";
		if (l)
		{
			change38++;
			KeyTColor = c2.Values.ToList()[change38];
			NewName(b2 + ": " + c2.Keys.ToList()[change38]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change38 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					KeyTColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeKeySize()
	{
		Vector3[][] array = new Vector3[3][]
		{
			new Vector3[4]
			{
				new Vector3(0.019f, 0.019f, 0.03f),
				new Vector3(0.095f, 0.095f, 0.025f),
				new Vector3(0.0182f, 0.0545f, 0.0365f),
				new Vector3(0.0818f, 0.0912f, 0.0304f)
			},
			new Vector3[4]
			{
				new Vector3(0.0245f, 0.0245f, 0.03f),
				new Vector3(0.11f, 0.11f, 0.025f),
				new Vector3(0.025f, 0.06f, 0.0365f),
				new Vector3(0.09f, 0.095f, 0.0304f)
			},
			new Vector3[4]
			{
				new Vector3(0.03f, 0.03f, 0.03f),
				new Vector3(0.16f, 0.16f, 0.025f),
				new Vector3(0.03f, 0.07f, 0.0365f),
				new Vector3(0.1f, 0.1f, 0.0304f)
			}
		};
		string[] array2 = new string[3] { "Normal", "Big", "Extra Big" };
		change39++;
		if (change39 > 2)
		{
			change39 = 0;
		}
		keysize = array[change39];
		RefreshBoard();
		NewName("Change Search Keys Size: " + array2[change39]);
	}

	public static void Recen()
	{
		rece = !rece;
		NewName("Recenter While Searching: " + rece);
	}

	public static void ChangePointerShape()
	{
		PrimitiveType[] array = new PrimitiveType[5];
		RuntimeHelpers.InitializeArray(array, __ldtoken(_003CPrivateImplementationDetails_003E.FA413D84BC0D46F5C98F088121D4BE9EE3A708A1D476FB8E530A578036A317C7));
		PrimitiveType[] array2 = array;
		string[] array3 = new string[5] { "Sphere", "Cube", "Cylinder", "Capsule", "Ellipsoid" };
		change48++;
		if (change48 > 4)
		{
			change48 = 0;
		}
		pointshape = array2[change48];
		if ((Object)(object)pointer != (Object)null)
		{
			Object.Destroy((Object)(object)pointer);
		}
		NewName("Change Gun Pointer Shape: " + array3[change48]);
	}

	public static void ChangeClickCool()
	{
		int[] array = new int[3] { 10, 15, 20 };
		string[] array2 = new string[3] { "Normal", "Slow", "Slower" };
		change49++;
		if (change49 > 2)
		{
			change49 = 0;
		}
		WristMenu.clickcool = array[change49];
		NewName("Change Click Cooldown: " + array2[change49]);
	}

	public static void ModSided()
	{
		sided = !sided;
		if (!sided && WristMenu.modsidedmenus.Keys.ToList().Count > 0)
		{
			foreach (Player item in WristMenu.modsidedmenus.Keys.ToList())
			{
				Object.Destroy((Object)(object)WristMenu.modsidedmenus[item]);
				WristMenu.modsidedextras.Remove(item);
				WristMenu.modsidedmenus.Remove(item);
			}
			WristMenu.ModSidedMenu.SendMenuStuff(70, new object[1] { "des" });
		}
		NewName("Mod Sided Menu: " + sided);
	}

	public static void IronSounds()
	{
		ironsounds = !ironsounds;
		NewName("Toggle Iron Monke Sounds: " + ironsounds);
	}

	public static void ClearDis()
	{
		cleardi = !cleardi;
		NewName("Clear Search On Disable: " + cleardi);
	}

	public static void ToggleJoin()
	{
		joinnotis = !joinnotis;
		NewName("Toggle Join/Leave Notis: " + joinnotis);
	}

	public static void ToggleTyName()
	{
		tyname = !tyname;
		NewName("Toggle Animated Menu Title: " + tyname);
	}

	public static void ChangeWallHand()
	{
		string text = "";
		change40++;
		if (change40 > 2)
		{
			change40 = 0;
		}
		if (change40 == 0)
		{
			text = "R Hand";
		}
		if (change40 == 1)
		{
			text = "L Hand";
		}
		if (change40 == 2)
		{
			text = "Both Hands";
		}
		NewName("Change Wall Walk Hand: " + text);
	}

	public static void ChangeAnti()
	{
		Vector3[] array = new Vector3[3]
		{
			new Vector3(0.352f, 0.352f, 0.352f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.2f, 0.2f, 0.2f)
		};
		string[] array2 = new string[3] { "Normal", "Big", "Small" };
		change41++;
		if (change41 > 2)
		{
			change41 = 0;
		}
		antirad = array[change41];
		if ((Object)(object)Find("ShowAntiReport") != (Object)null)
		{
			Object.Destroy((Object)(object)Find("ShowAntiReport"));
		}
		NewName("Change Anti Report Radius: " + array2[change41]);
	}

	public static void ToggleSearch()
	{
		searchB = !searchB;
		if (!searchB && searching)
		{
			WristMenu.Toggle("Search");
		}
		NewName("Toggle Search Button: " + searchB);
	}

	public static void ToggleHome()
	{
		homeB = !homeB;
		NewName("Toggle Home Button: " + homeB);
	}

	public static void Togglecoll()
	{
		menucol = !menucol;
		NewName("Toggle Menu Collision: " + menucol);
	}

	public static void ToggleDis()
	{
		dis = !dis;
		NewName("Toggle Disconnect Button: " + dis);
	}

	public static void ChangeBlock()
	{
		if ((Object)(object)bt() != (Object)null)
		{
			List<ButtonInfo> list = new List<ButtonInfo>();
			foreach (int k in bt().builderPool.piecePoolLookup.Keys.Where((int e) => e != 1858470402 && e != 764108395).ToList())
			{
				string n = ((Object)bt().GetPiecePrefab(k)).name.Replace("SnapPiece_", "").Replace("SnapPiece", "");
				list.Insert(list.Count, new ButtonInfo
				{
					buttonText = n,
					method = () =>
					{
						block = k.ToString();
						DisableButton(n);
						Rebs("Change Block: " + n);
					},
					enabled = false
				});
			}
			list.Insert(0, new ButtonInfo
			{
				buttonText = "ArmShelf",
				method = () =>
				{
					block = "1858470402";
					DisableButton("ArmShelf");
					Rebs("Change Block: ArmShelf");
				},
				enabled = false
			});
			list.Insert(0, new ButtonInfo
			{
				buttonText = "Random",
				method = () =>
				{
					block = "random";
					DisableButton("Random");
					Rebs("Change Block: Random");
				},
				enabled = false
			});
			SpawnBoard(list, "ed", 30, "Find A Block And Click It|Change Block");
		}
		else
		{
			SendNoti(Er + "You Must Be In The MonkeBlocks Map!");
			DisableButton("Change Block", c: true);
		}
	}

	public static void Changeplat()
	{
		TriggerPlats = !TriggerPlats;
		NewName("Change Platform Keybind: " + (TriggerPlats ? "Triggers" : "Grips"));
	}

	public static void AutoSave()
	{
		autosave = !autosave;
		NewName("Auto Save Settings: " + autosave);
	}

	public static void Changehide()
	{
		hidee = !hidee;
		NewName("Hide Visuals On PC Screen: " + hidee);
	}

	public static void ChangeGunHand()
	{
		change23++;
		if (change23 > 2)
		{
			change23 = 1;
		}
		NewName("Change Gun Hand: " + ((change23 == 1) ? "Right" : "Left"));
	}

	public static void ChangeGunLiner()
	{
		change24++;
		if (change24 > 2)
		{
			change24 = 1;
		}
		NewName("Toggle Gun Line: " + (change24 == 1));
	}

	public static void EnabledOrDisableGUI()
	{
		GuiPlugin.open = !GuiPlugin.open;
		NewName("Toggle Gui: " + GuiPlugin.open);
	}

	public static void ChangeMenuSize()
	{
		string text = "";
		change20++;
		if (change20 > 4)
		{
			change20 = 1;
		}
		if (change20 == 1)
		{
			text = "Normal";
		}
		if (change20 == 2)
		{
			text = "Long";
		}
		if (change20 == 3)
		{
			text = "Wide";
		}
		if (change20 == 4)
		{
			text = "Long & Wide";
		}
		NewName("Change Menu Size: " + text);
	}

	public static void ChangeTracerPos()
	{
		string text = "";
		change25++;
		if (change25 > 4)
		{
			change25 = 1;
		}
		if (change25 == 1)
		{
			text = "R Hand";
		}
		if (change25 == 2)
		{
			text = "L Hand";
		}
		if (change25 == 3)
		{
			text = "Body";
		}
		if (change25 == 4)
		{
			text = "Head";
		}
		NewName("Change Tracers Position: " + text);
	}

	public static void ChangeTagAura()
	{
		string text = "";
		float aURA = 0f;
		change5++;
		if (change5 > 4)
		{
			change5 = 1;
		}
		if (change5 == 1)
		{
			aURA = 1.666f;
			text = "Normal";
		}
		if (change5 == 2)
		{
			aURA = 3f;
			text = "Far";
		}
		if (change5 == 3)
		{
			aURA = 4.4f;
			text = "Max";
		}
		if (change5 == 4)
		{
			aURA = 0.777f;
			text = "Short";
		}
		AURA = aURA;
		NewName("Change Aura Distance: " + text);
	}

	public static void ChangeWallWalk()
	{
		string text = "";
		float walk = 0f;
		change17++;
		if (change17 > 3)
		{
			change17 = 1;
		}
		if (change17 == 1)
		{
			walk = 10f;
			text = "Normal";
		}
		if (change17 == 2)
		{
			walk = 30f;
			text = "Strong";
		}
		if (change17 == 3)
		{
			walk = 5f;
			text = "Weak";
		}
		Walk = walk;
		NewName("Change Wall Walk Strength: " + text);
	}

	public static void ChangePS()
	{
		string text = "";
		float num = 0f;
		change26++;
		if (change26 > 3)
		{
			change26 = 1;
		}
		if (change26 == 1)
		{
			num = 0.15f;
			text = "Normal";
		}
		if (change26 == 2)
		{
			num = 0.23f;
			text = "Big";
		}
		if (change26 == 3)
		{
			num = 0.1f;
			text = "Small";
		}
		ps = new Vector3(num, num, num);
		NewName("Change Gun Pointer Size: " + text);
	}

	public static void ChangeLS()
	{
		string text = "";
		float num = 0f;
		change27++;
		if (change27 > 3)
		{
			change27 = 1;
		}
		if (change27 == 1)
		{
			num = 0.025f;
			text = "Normal";
		}
		if (change27 == 2)
		{
			num = 0.04f;
			text = "Big";
		}
		if (change27 == 3)
		{
			num = 0.01f;
			text = "Small";
		}
		ls = num;
		NewName("Change Gun Line Size: " + text);
	}

	public static void ChangeNotiDelay()
	{
		string text = "";
		int notificationDecayTime = 0;
		change19++;
		if (change19 > 3)
		{
			change19 = 1;
		}
		if (change19 == 1)
		{
			notificationDecayTime = 150;
			text = "Normal";
		}
		if (change19 == 2)
		{
			notificationDecayTime = 85;
			text = "Fast";
		}
		if (change19 == 3)
		{
			notificationDecayTime = 250;
			text = "Slow";
		}
		NotifiLib.NotificationDecayTime = notificationDecayTime;
		NewName("Change Noti Delay: " + text);
	}

	public static float NotiDelay()
	{
		return change19 switch
		{
			1 => 1.2f, 
			2 => 1f, 
			3 => 2.25f, 
			_ => 0f, 
		};
	}

	public static void Changedisconnect()
	{
		string text = "";
		change4++;
		if (change4 > 4)
		{
			change4 = 1;
		}
		if (change4 == 1)
		{
			text = "Right";
		}
		if (change4 == 2)
		{
			text = "Left";
		}
		if (change4 == 3)
		{
			text = "Top";
		}
		if (change4 == 4)
		{
			text = "Bottom";
		}
		NewName("Change Disconnect Location: " + text);
	}

	public static void Changemenu()
	{
		right = !right;
		NewName("Change Menu Location: " + (right ? "R Hand" : "L Hand"));
	}

	public static void Changepagebutton()
	{
		string text = "";
		change7++;
		if (change7 > 6)
		{
			change7 = 1;
		}
		if (change7 == 1)
		{
			text = "On Menu";
		}
		if (change7 == 2)
		{
			text = "Top";
		}
		if (change7 == 3)
		{
			text = "Sides";
		}
		if (change7 == 4)
		{
			text = "Bottom";
		}
		if (change7 == 5)
		{
			text = "Triggers";
		}
		if (change7 == 6)
		{
			text = "Grips";
		}
		NewName("Change Next & Prev Location: " + text);
	}

	public static void ChangeOrbColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("allnocl");
		string b2 = "Change Gun Color";
		if (l)
		{
			change8++;
			CurrentGunColor = c2.Values.ToList()[change8];
			NewName(b2 + ": " + c2.Keys.ToList()[change8]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change8 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					CurrentGunColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeGVColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("", new string[3] { "RGB", "Rig Color", "Menu Theme" });
		string b2 = "Change GhostView Color";
		if (l)
		{
			change45++;
			NewName(b2 + ": " + c2.Keys.ToList()[change45]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change45 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeGVTrans()
	{
		string[] array = new string[4] { "Normal", "Semi Solid", "Solid", "Light" };
		change46++;
		if (change46 > 3)
		{
			change46 = 0;
		}
		NewName("Change GhostView Transparency: " + array[change46]);
	}

	public static void ChangeGVType()
	{
		DisableGV();
		string[] array = new string[5] { "Rig", "Orbs", "Skelly", "Realistic Skelly", "None" };
		change47++;
		if (change47 > 4)
		{
			change47 = 0;
		}
		NewName("Change GhostView Type: " + array[change47]);
	}

	public static void ChangeVisualColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("", new string[4] { "RGB", "Rig Color", "Tag & Untag", "Menu Theme" });
		string b2 = "Change ESP Color";
		if (l)
		{
			change9++;
			CurrentESPColor = c2.Values.ToList()[change9];
			NewName(b2 + ": " + c2.Keys.ToList()[change9]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change9 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					CurrentESPColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV1(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Menu 1st Color";
		if (l)
		{
			change10++;
			WristMenu.FirstColor = c2.Values.ToList()[change10];
			NewName(b2 + ": " + c2.Keys.ToList()[change10]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change10 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.FirstColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV2(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Menu 2nd Color";
		if (l)
		{
			change11++;
			WristMenu.SecondColor = c2.Values.ToList()[change11];
			NewName(b2 + ": " + c2.Keys.ToList()[change11]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change11 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.SecondColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV3(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("Clear");
		string b2 = "Change Disabled Button Color";
		if (l)
		{
			change12++;
			WristMenu.ButtonColorDisable = c2.Values.ToList()[change12];
			NewName(b2 + ": " + c2.Keys.ToList()[change12]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change12 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.ButtonColorDisable = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV4(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("Clear");
		string b2 = "Change Enabled Button Color";
		if (l)
		{
			change13++;
			WristMenu.ButtonColorEnabled = c2.Values.ToList()[change13];
			NewName(b2 + ": " + c2.Keys.ToList()[change13]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change13 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.ButtonColorEnabled = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV5(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Enabled Text Color";
		if (l)
		{
			change14++;
			WristMenu.EnableTextColor = c2.Values.ToList()[change14];
			NewName(b2 + ": " + c2.Keys.ToList()[change14]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change14 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.EnableTextColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV6(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Disabled Text Color";
		if (l)
		{
			change15++;
			WristMenu.DIsableTextColor = c2.Values.ToList()[change15];
			NewName(b2 + ": " + c2.Keys.ToList()[change15]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change15 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.DIsableTextColor = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ThemeChangerV7()
	{
		string text = "";
		int buttonSound = 0;
		change16++;
		if (change16 > 9)
		{
			change16 = 1;
		}
		if (change16 == 1)
		{
			buttonSound = 67;
			text = "Normal";
		}
		if (change16 == 2)
		{
			buttonSound = 8;
			text = "Stump";
		}
		if (change16 == 3)
		{
			buttonSound = 203;
			text = "AK47";
		}
		if (change16 == 4)
		{
			buttonSound = 50;
			text = "Glass";
		}
		if (change16 == 5)
		{
			buttonSound = 66;
			text = "KeyBoard";
		}
		if (change16 == 6)
		{
			buttonSound = 114;
			text = "Cayon Bridge";
		}
		if (change16 == 7)
		{
			buttonSound = 189;
			text = "Cayon Thing";
		}
		if (change16 == 8)
		{
			buttonSound = 351;
			text = "Space Teleporter";
		}
		if (change16 == 9)
		{
			buttonSound = 84;
			text = "Pop";
		}
		ButtonSound = buttonSound;
		NewName("Change Button Sound: " + text);
	}

	public static void ChangeFont()
	{
		string[] array = new string[6] { "Default", "Agency", "Verdana", "Sans", "Consolas", "Candara" };
		change28++;
		if (change28 > 5)
		{
			change28 = 0;
		}
		WristMenu.MenuFont = fonts[change28];
		try
		{
			NotifiLib.Testtext.font = fonts[change28];
		}
		catch
		{
		}
		NewName("Change Menu Font: " + array[change28]);
	}

	public static void ChangeNotiSize()
	{
		Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
		dictionary.Add(0, new object[2] { 0.7f, "Small" });
		dictionary.Add(1, new object[2] { 1f, "Normal" });
		dictionary.Add(2, new object[2] { 1.45f, "Big" });
		dictionary.Add(3, new object[2] { 1.95f, "Massive" });
		Dictionary<int, object[]> dictionary2 = dictionary;
		change44++;
		if (change44 > 3)
		{
			change44 = 0;
		}
		try
		{
			float num = (float)dictionary2[change44][0];
			NotifiLib.HUDObj.transform.localScale = new Vector3(num, num, num);
		}
		catch
		{
		}
		NewName("Change Noti Size: " + (string)dictionary2[change44][1]);
	}

	public static void ChangeArrow()
	{
		string[] array = new string[10] { "< >", "← →", "⇦ ⇨", "↩ ↪", "◀ ▶", "◁ ▷", "◅ ▻", "◂ ▸", "⇚ ⇛", "⇇ ⇉" };
		change50++;
		if (change50 > 9)
		{
			change50 = 0;
		}
		WristMenu.arrow = array[change50];
		NewName("Change Next & Prev Arrow: " + WristMenu.arrow);
	}

	public static void ChangeDisColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Disconnect Color";
		if (l)
		{
			change29++;
			WristMenu.DICo = c2.Values.ToList()[change29];
			NewName(b2 + ": " + c2.Keys.ToList()[change29]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change29 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.DICo = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeDisTColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Disconnect Text Color";
		if (l)
		{
			change30++;
			WristMenu.DITCo = c2.Values.ToList()[change30];
			NewName(b2 + ": " + c2.Keys.ToList()[change30]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change30 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.DITCo = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeNPColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors();
		string b2 = "Change Next & Prev Color";
		if (l)
		{
			change31++;
			WristMenu.NPCo = c2.Values.ToList()[change31];
			NewName(b2 + ": " + c2.Keys.ToList()[change31]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change31 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.NPCo = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeNPTColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Next & Prev Text Color";
		if (l)
		{
			change32++;
			WristMenu.NPTC = c2.Values.ToList()[change32];
			NewName(b2 + ": " + c2.Keys.ToList()[change32]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change32 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.NPTC = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void ChangeTitleColor(bool l = true)
	{
		Dictionary<string, Color> c2 = AllColors("");
		string b2 = "Change Menu Title Color";
		if (l)
		{
			change33++;
			WristMenu.MenuTC = c2.Values.ToList()[change33];
			NewName(b2 + ": " + c2.Keys.ToList()[change33]);
			return;
		}
		List<ButtonInfo> list = new List<ButtonInfo>();
		foreach (KeyValuePair<string, Color> k in c2)
		{
			list.Insert(list.Count, new ButtonInfo
			{
				buttonText = k.Key,
				method = () =>
				{
					change33 = c2.Keys.ToList().FindIndex((string e) => e == k.Key);
					WristMenu.MenuTC = k.Value;
					DisableButton(k.Key);
					Rebs(b2 + ": " + k.Key);
				},
				enabled = false
			});
		}
		SpawnBoard(list, "ed", 30, "Find A Color And Click It|" + b2);
	}

	public static void MakeGun(Transform arm, bool loc, Action s, Action o, string bu = "", bool r = true)
	{
		if ((Object)(object)arm == (Object)(object)Loco().RightHand.controllerTransform)
		{
			hand = WristMenu.gripDownR;
			hand1 = WristMenu.triggerDownR;
		}
		else if ((Object)(object)arm == (Object)(object)Loco().LeftHand.controllerTransform)
		{
			hand = WristMenu.gripDownL;
			hand1 = WristMenu.triggerDownL;
		}
		bool flag = (Mouse.current != null && (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)) || GuiPlugin.emgun;
		if (!hand && !Mouse.current.rightButton.isPressed && !GuiPlugin.emgun)
		{
			if ((Object)(object)pointer != (Object)null)
			{
				Object.Destroy((Object)(object)pointer);
				pointer = null;
			}
			if ((Object)(object)Line != (Object)null)
			{
				Object.Destroy((Object)(object)((Component)Line).gameObject);
				Line = null;
			}
			if (loc && (Object)(object)lockedrig != (Object)null)
			{
				lockedrig = null;
			}
			return;
		}
		if (flag && (Object)(object)Cam == (Object)null)
		{
			GameObject val = Find("Player Objects/Third Person Camera/Shoulder Camera");
			Camera val2 = ((val != null) ? val.GetComponent<Camera>() : null);
			if ((Object)(object)val2 != (Object)null)
			{
				Cam = val2;
			}
		}
		RaycastHit val3 = default;
		bool flag2 = false;
		int num = NoBarrier(r);
		float num2 = float.MaxValue;
		if ((Object)(object)lockedrig == (Object)null)
		{
			if (flag && (Object)(object)Cam != (Object)null)
			{
				flag2 = Physics.Raycast(Cam.ScreenPointToRay(UnityInput.Current.mousePosition), ref rayCur, num2, num);
				val3 = rayCur;
			}
			else
			{
				flag2 = Physics.Raycast(arm.position, -arm.up, ref rayCur, num2, num);
				val3 = rayCur;
			}
		}
		if ((Object)(object)pointer == (Object)null)
		{
			if ((int)pointshape != 5)
			{
				pointer = CreatePrimitive(pointshape);
			}
			else
			{
				pointer = Ellip();
			}
			pointer.transform.position = val3.point;
		}
		pointer.transform.localScale = ps * Loco().scale;
		Color gc;
		if (change8 != 0)
		{
			gc = ((change8 == 1) ? MenuColor() : CurrentGunColor);
		}
		else
		{
			gc = Rainbow();
		}
		if ((int)pointshape != 5)
		{
			pointer.GetComponent<Renderer>().material.shader = WristMenu.esp;
			pointer.GetComponent<Renderer>().material.color = gc;
		}
		else
		{
			LinqUtils.ForEach<Renderer>((IEnumerable<Renderer>)pointer.GetComponentsInChildren<Renderer>(), (Action<Renderer>)((Renderer mm) =>
			{
				mm.material.shader = WristMenu.esp;
			}));
			LinqUtils.ForEach<Renderer>((IEnumerable<Renderer>)pointer.GetComponentsInChildren<Renderer>(), (Action<Renderer>)((Renderer mm) =>
			{
				mm.material.color = gc;
			}));
		}
		try
		{
			Object.Destroy((Object)(object)pointer.GetComponent<Rigidbody>());
			Object.Destroy((Object)(object)pointer.GetComponent<Collider>());
		}
		catch
		{
		}
		if (change24 == 1)
		{
			if ((Object)(object)Line == (Object)null)
			{
				GameObject val4 = new GameObject("Line");
				Line = val4.AddComponent<LineRenderer>();
				((Renderer)Line).material.shader = WristMenu.esp;
				Line.positionCount = 2;
				Line.useWorldSpace = true;
				Line.SetPosition(1, val3.point);
			}
			Line.startWidth = ls * Loco().scale;
			Line.endWidth = ls * Loco().scale;
			Line.startColor = gc;
			Line.endColor = gc;
			Line.SetPosition(0, arm.position);
		}
		else if ((Object)(object)Line != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)Line).gameObject);
			Line = null;
		}
		if (loc)
		{
			MakeLock();
		}
		else if (flag2)
		{
			LineRenderer line = Line;
			if (line != null)
			{
				line.SetPosition(1, Vector3.Lerp(Line.GetPosition(1), val3.point, Time.deltaTime * smooth));
			}
			pointer.transform.position = Vector3.Lerp(pointer.transform.position, val3.point, Time.deltaTime * smooth);
		}
		bool flag3 = true;
		if (loc)
		{
			flag3 = (Object)(object)lockedrig != (Object)null && ((Component)lockedrig).gameObject.activeSelf;
		}
		bool flag4 = !(bu == "") || (Object)(object)val3.ReflectionEnums != (Object)(object)Find("barr").GetComponent<Collider>();
		if (hand1 || Mouse.current.leftButton.isPressed)
		{
			if (flag3 & flag4)
			{
				try
				{
					s?.Invoke();
					return;
				}
				catch
				{
					return;
				}
			}
			return;
		}
		if (loc && (Object)(object)lockedrig != (Object)null)
		{
			lockedrig = null;
		}
		lockedrope = null;
		try
		{
			o?.Invoke();
		}
		catch
		{
		}
	}

	public static void MakeLock()
	{
		Vector3 position = pointer.transform.position;
		Vector3 val = (((Object)(object)Line == (Object)null) ? Vector3.zero : Line.GetPosition(1));
		if (hand1 || Mouse.current.leftButton.isPressed)
		{
			if ((Object)(object)((Component)rayCur.ReflectionEnums).GetComponentInParent<VRRig>() != (Object)null && !((Component)rayCur.ReflectionEnums).GetComponentInParent<VRRig>().isOfflineVRRig && (Object)(object)lockedrig == (Object)null)
			{
				lockedrig = ((Component)rayCur.ReflectionEnums).GetComponentInParent<VRRig>();
			}
			if ((Object)(object)lockedrig != (Object)null)
			{
				val = ((Component)lockedrig).transform.position;
				position = ((Component)lockedrig).transform.position;
			}
			else
			{
				position = rayCur.point;
			}
		}
		else
		{
			position = rayCur.point;
		}
		if ((Object)(object)lockedrig == (Object)null)
		{
			position = rayCur.point;
		}
		LineRenderer line = Line;
		if (line != null)
		{
			line.SetPosition(1, Vector3.Lerp(val, position, Time.deltaTime * smooth));
		}
		pointer.transform.position = Vector3.Lerp(pointer.transform.position, position, Time.deltaTime * smooth);
	}

	public static void RopeLock()
	{
		if ((Object)(object)((Component)rayCur.ReflectionEnums).GetComponentInParent<GorillaRopeSwing>() != (Object)null && (Object)(object)lockedrope == (Object)null)
		{
			lockedrope = ((Component)rayCur.ReflectionEnums).GetComponentInParent<GorillaRopeSwing>();
		}
		if ((Object)(object)lockedrope != (Object)null)
		{
			LineRenderer line = Line;
			if (line != null)
			{
				line.SetPosition(1, lockedrope.nodes.Last().position);
			}
			pointer.transform.position = lockedrope.nodes.Last().position;
		}
	}

	public static int NoBarrier(bool r = true)
	{
		return ~((IEnumerable<string>)new string[8]
		{
			"TransparentFX",
			"Ignore Raycast",
			"Zone",
			"Gorilla Trigger",
			"Gorilla Boundary",
			"GorillaCosmetics",
			"GorillaParticle",
			r ? "GorillaInteractable" : ""
		}).Select((Func<string, int>)LayerMask.NameToLayer).Aggregate(0, (int num, int l) => num | (1 << l));
	}

	public static Transform GetGunHand()
	{
		return (change23 == 1) ? Loco().RightHand.controllerTransform : Loco().LeftHand.controllerTransform;
	}

	public static void MakeGun(bool l, Action s, string b = "", bool r = true)
	{
		MakeGun(GetGunHand(), l, s, null, b, r);
	}

	public static void MakeGun(bool l, Action s, Action s1, string b = "", bool r = true)
	{
		MakeGun(GetGunHand(), l, s, s1, b, r);
	}

	public static List<GameEntity> GhostShi(string n)
	{
		return (from g in Mods.F<GameEntity>(5f)
			where ((Object)((Component)g).gameObject).name.Contains(n) && ((Component)g).gameObject.activeSelf && (Object)(object)g != (Object)null
			select g).ToList();
	}

	public static void HideEsp(GameObject g, int n)
	{
		g.layer = (hidee ? 19 : n);
	}

	public static Dictionary<int, object[]> RanCol()
	{
		Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
		foreach (KeyValuePair<int, object[]> collectible in collectibles)
		{
			dictionary.Add(collectible.Key, collectible.Value);
		}
		foreach (KeyValuePair<int, GameObject> item in from e in Create(GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone)).Field(Field(84)).GetValue<Dictionary<int, GameObject>>()
			where !Enex.Any((string r) => ((Object)e.Value).name.Contains(r) && !((Object)e.Value).name.Contains("EnemyMoonBoss"))
			select e)
		{
			dictionary.Add(item.Key, new object[4]
			{
				((Object)item.Value).name,
				((Object)item.Value).name.Replace("GhostReactor", "GhostReactor ").Replace("_", " ").Replace("Collectible", "")
					.Replace("Monkeye01", "Monkeye"),
				item.Key,
				0L
			});
		}
		return dictionary;
	}

	public static bool Pickupable(int i, GameEntityManager g)
	{
		return Create(g).Field(Field(84)).GetValue<Dictionary<int, GameObject>>()[i].GetComponent<GameEntity>().pickupable;
	}

	public static object[] GetCol(int i)
	{
		foreach (KeyValuePair<int, object[]> collectible in collectibles)
		{
			if ((int)collectible.Value[2] == i)
			{
				return collectible.Value;
			}
		}
		foreach (KeyValuePair<int, GameObject> item in Create(GameEntityManager.GetManagerForZone(OwnRig().zoneEntity.currentZone)).Field(Field(84)).GetValue<Dictionary<int, GameObject>>())
		{
			if (item.Key == i)
			{
				return new object[4]
				{
					((Object)item.Value).name,
					((Object)item.Value).name.Replace("GhostReactor", "GhostReactor ").Replace("_", " ").Replace("Collectible", "")
						.Replace("Monkeye01", "Monkeye"),
					item.Key,
					0L
				};
			}
		}
		return new object[0];
	}

	public static void OffESPS(int m)
	{
		if (ESPs[m].Keys.Count <= 0)
		{
			return;
		}
		if (ESPss[m].Keys.Count > 0)
		{
			foreach (KeyValuePair<VRRig, GameObject[]> item in ESPss[m].ToList())
			{
				LinqUtils.ForEach<GameObject>((IEnumerable<GameObject>)item.Value, (Action<GameObject>)((GameObject e) =>
				{
					Object.Destroy((Object)(object)e);
				}));
			}
			ESPss[m].Clear();
		}
		foreach (KeyValuePair<VRRig, GameObject> item2 in ESPs[m].ToList())
		{
			Object.Destroy((Object)(object)item2.Value);
		}
		ESPs[m].Clear();
	}

	public static void ESPCheck(int m)
	{
		if (ESPs[m].Keys.ToList().Count <= 0 || !((Object)(object)ESPs[m].Keys.ToList()[0] != (Object)(object)OwnRig()))
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			foreach (KeyValuePair<VRRig, GameObject> item in ESPs[m].ToList())
			{
				if (!ESPPlayers().Contains(item.Key))
				{
					if (ESPss[m].Keys.Count != 0)
					{
						foreach (GameObject item2 in ESPss[m][item.Key].ToList())
						{
							Object.Destroy((Object)(object)item2);
						}
						ESPss[m].Remove(item.Key);
					}
					Object.Destroy((Object)(object)item.Value);
					ESPs[m].Remove(item.Key);
				}
			}
			return;
		}
		OffESPS(m);
	}

	public static bool InMain()
	{
		return !inCat1 && !inCat2 && !inCat3 && !inCat10 && !inCat4 && !inCat5 && !inCat6 && !inCat7 && !inCat9 && !inCat8 && !inSettings && !inCat12;
	}

	public static bool IsEnabled(string n)
	{
		return GetButton(n).enabled == true;
	}

	public static bool IsEnabled(ButtonInfo b)
	{
		return b.enabled == true;
	}

	public static List<ButtonInfo> CurrentButtons()
	{
		int pageNumber = WristMenu.pageNumber;
		int pageSize = WristMenu.pageSize;
		List<ButtonInfo> list = LocalCB().Skip(pageNumber * pageSize).Take(pageSize).ToList();
		if (inCat1 && pageNumber != 0)
		{
			list.Insert(0, WristMenu.CatButtons1[0]);
		}
		return list;
	}

	public static List<ButtonInfo> LocalCB()
	{
		if (searching)
		{
			return WristMenu.CatButtons11;
		}
		if (inSettings)
		{
			return WristMenu.settingsbuttons;
		}
		if (inCat1)
		{
			return WristMenu.CatButtons1;
		}
		if (inCat2)
		{
			return WristMenu.CatButtons2;
		}
		if (inCat3)
		{
			return WristMenu.CatButtons3;
		}
		if (inCat4)
		{
			return WristMenu.CatButtons4;
		}
		if (inCat5)
		{
			return WristMenu.CatButtons5;
		}
		if (inCat6)
		{
			return WristMenu.CatButtons6;
		}
		if (inCat7)
		{
			return WristMenu.CatButtons7;
		}
		if (inCat8)
		{
			return WristMenu.CatButtons8;
		}
		if (inCat9)
		{
			return WristMenu.CatButtons9;
		}
		if (inCat10)
		{
			return WristMenu.CatButtons10;
		}
		if (inCat12)
		{
			return WristMenu.CatButtons12;
		}
		if (InMain())
		{
			return WristMenu.buttons;
		}
		return new List<ButtonInfo>();
	}

	public static VRRig SlowestVel()
	{
		return (from w in RigUtils.AllRigs(i: false)
			where ActualTagged(w)
			select w).OrderBy((VRRig w) =>
		{
			Vector3 val = w.LatestVelocity();
			return val.magnitude;
		}).FirstOrDefault();
	}

	public static List<VRRig> ESPPlayers()
	{
		return (PhotonNetwork.IsConnectedAndReady || CosDone() || (!CosDone() && PhotonNetwork.InRoom)) ? RigUtils.AllRigs(i: false) : new List<VRRig> { OwnRig() };
	}

	public static string I(string cat)
	{
		return "⫷__" + cat + "__⫸";
	}

	public static Vector3 W(Vector3 pos)
	{
		return pos - ((Component)GIn().bodyCollider).transform.position + ((Component)GIn()).transform.position;
	}

	public static BuilderTable bt(VRRig r = null)
	{
		try
		{
			BuilderTable result = default;
			BuilderTable.TryGetBuilderTableForZone(((Object)(object)r != (Object)null) ? r.zoneEntity.currentZone : OwnRig().zoneEntity.currentZone, ref result);
			return result;
		}
		catch
		{
			return null;
		}
	}

	public static string RandomString(bool s = false, int o = -1)
	{
		string text = "";
		for (int i = 0; i < ((o != -1) ? o : (s ? 10 : Random.Range(1, 12))); i++)
		{
			text += rann[Random.Range(0, rann.Length - 1)];
		}
		return text;
	}

	public static void SoftAimVisual(SlingshotProjectile ga)
	{
		if (((Component)ga).gameObject.activeSelf && ((Behaviour)ga).isActiveAndEnabled)
		{
			GameObject val = CreatePrimitive((PrimitiveType)0);
			Object.Destroy((Object)(object)val.GetComponent<SphereCollider>());
			Transform transform = val.transform;
			transform.position = ((Component)ga).gameObject.transform.position;
			transform.localScale = new Vector3(2f, 2f, 2f);
			((Component)transform).GetComponent<Renderer>().material.shader = WristMenu.esp;
			((Component)transform).GetComponent<Renderer>().material.color = Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)5));
			Object.Destroy((Object)(object)val, Time.deltaTime);
		}
	}

	public static Color ranc()
	{
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	public static bool ProjHand()
	{
		if (prr == 0)
		{
			return WristMenu.gripDownR;
		}
		if (prr == 1)
		{
			return WristMenu.gripDownL;
		}
		if (prr == 2)
		{
			return WristMenu.gripDownR || WristMenu.gripDownL;
		}
		return false;
	}

	public static GameObject Ha(bool r)
	{
		return r ? EquipmentInteractor.instance.rightHandHeldEquipment.gameObject : EquipmentInteractor.instance.leftHandHeldEquipment.gameObject;
	}

	public static Vector3 RainVector(float h, float d = 3f)
	{
		return ((Component)OwnRig()).transform.position + new Vector3(Random.Range(0f - d, d), h, Random.Range(0f - d, d));
	}

	public static Color Rainbow()
	{
		return RainbowG().Evaluate(Mathf.PingPong(Time.time / 2f, 1f));
	}

	public static Gradient RainbowG()
	{
		GradientColorKey[] array = new GradientColorKey[7];
		array[0].color = Color.red;
		array[0].time = 0f;
		array[1].color = Color.yellow;
		array[1].time = 0.2f;
		array[2].color = Color.green;
		array[2].time = 0.3f;
		array[3].color = Color.cyan;
		array[3].time = 0.5f;
		array[4].color = Color.blue;
		array[4].time = 0.6f;
		array[5].color = Color.magenta;
		array[5].time = 0.8f;
		array[6].color = Color.red;
		array[6].time = 1f;
		return new Gradient
		{
			colorKeys = array
		};
	}

	public static Color MenuColor()
	{
		if (change10 == 0)
		{
			return Color.Lerp(WristMenu.FirstColor, WristMenu.SecondColor, Mathf.PingPong(Time.time, 1f));
		}
		return WristMenu.menuObj.GetComponent<ColorChanger>().color;
	}

	public static Vector3 GetTracerPos()
	{
		if (change25 == 1)
		{
			return Loco().RightHand.controllerTransform.position;
		}
		if (change25 == 2)
		{
			return Loco().LeftHand.controllerTransform.position;
		}
		if (change25 == 3)
		{
			return BC().position - new Vector3(0f, 0.3f, 0f);
		}
		if (change25 == 4)
		{
			return BC().position + new Vector3(0f, 0.5f, 0f);
		}
		return Vector3.zero;
	}

	public static GameObject Find(string p)
	{
		return GameObject.Find(p);
	}

	public static void DelayedNoti(string n, float d = 1.65f)
	{
		new Delay().D((d == 1.65f) ? NotiDelay() : d, () =>
		{
			SendNoti(n);
		});
	}

	public static void Grav(float g)
	{
		Loco().playerRigidBody.AddForce(-Physics.gravity * Loco().playerRigidBody.mass * g, (ForceMode)0);
	}

	public static GorillaSurfaceOverride Sur(string p)
	{
		return Find(p).GetComponent<GorillaSurfaceOverride>();
	}

	public static string MasterCheckNoti(Player p)
	{
		string text;
		if (usingmenu.Contains(p))
		{
			text = ((WristMenu.own == p) ? "<color=#FF00FF>" : "<color=#00FFFF>");
		}
		else
		{
			text = "<color=white>";
		}
		string text2 = "[<color=blue>ROOM</color>] Player: " + text + p.NickName + "</color> Left The Lobby";
		if (Local().IsMasterClient && !masternoti && !WristMenu.alreadyma)
		{
			masternoti = true;
			return text2 + "\n<color=yellow>YOU ARE NOW MASTER!</color>";
		}
		return text2;
	}

	public static bool ActualTagged(VRRig rig)
	{
		try
		{
			GorillaGameManager instance = GorillaGameManager.instance;
			string text = TaggedName(rig);
			bool flag = (Object)(object)rig != (Object)(object)OwnRig();
			bool enabled = ((Behaviour)OwnRig()).enabled;
			if (IsGameMode((GameModeType)1))
			{
				GorillaTagManager val = (GorillaTagManager)instance;
				bool flag2 = text == "LAVA MONKE" || text == "ROCK MONKE";
				return (val.currentInfected.Contains(rig.Creator) || val.currentIt == rig.Creator) && (flag ? flag2 : (!enabled || (enabled & flag2)));
			}
			if (IsGameMode((GameModeType)5))
			{
				GorillaFreezeTagManager val2 = (GorillaFreezeTagManager)instance;
				bool flag3 = text == "ICE" || text == "FROZEN";
				return (((GorillaTagManager)val2).currentInfected.Contains(rig.Creator) || val2.currentFrozen.ContainsKey(rig.Creator)) && (flag ? flag3 : (!enabled || (enabled & flag3)));
			}
			if (IsGameMode((GameModeType)10))
			{
				GorillaTagCompetitiveManager val3 = (GorillaTagCompetitiveManager)instance;
				bool flag4 = text == "LAVA MONKE" || text == "ROCK MONKE";
				return (((GorillaTagManager)val3).currentInfected.Contains(rig.Creator) || ((GorillaTagManager)val3).currentIt == rig.Creator) && (flag ? flag4 : (!enabled || (enabled & flag4)));
			}
			if (IsGameMode((GameModeType)11))
			{
				SuperInfectionGame instance2 = SuperInfectionGame.instance;
				bool flag5 = text == "LAVA MONKE" || text == "ROCK MONKE";
				return (((GorillaTagManager)instance2).currentInfected.Contains(rig.Creator) || ((GorillaTagManager)instance2).currentIt == rig.Creator) && (flag ? flag5 : (!enabled || (enabled & flag5)));
			}
			if (IsGameMode((GameModeType)9))
			{
				return ((GorillaTagManager)(GorillaPropHuntGameManager)instance).currentInfected.Contains(rig.Creator);
			}
			if (IsGameMode((GameModeType)2))
			{
				bool flag6 = text == "HUNTED";
				return ((GorillaHuntManager)instance).currentHunted.Contains(rig.Creator) && (flag ? flag6 : (!enabled || (enabled & flag6)));
			}
			if (IsGameMode((GameModeType)6) || IsGameMode((GameModeType)4))
			{
				bool flag7 = text == "GHOST";
				return ((GorillaTagManager)(GorillaAmbushManager)instance).currentInfected.Contains(rig.Creator) && (flag ? flag7 : (!enabled || (enabled & flag7)));
			}
			if (IsGameMode((GameModeType)8))
			{
				bool flag8 = text == "GUARDIAN";
				return ((GorillaGuardianManager)instance).IsPlayerGuardian(rig.Creator) && (flag ? flag8 : (!enabled || (enabled & flag8)));
			}
			if (IsGameMode((GameModeType)3))
			{
				bool flag9 = text == "KILLED";
				return ((GorillaPaintbrawlManager)instance).GetPlayerLives(rig.Creator) == 0 && (flag ? flag9 : (!enabled || (enabled & flag9)));
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool IsEveryoneTagged()
	{
		try
		{
			List<VRRig> list = RigUtils.AllRigs();
			GorillaGameManager instance = GorillaGameManager.instance;
			if (IsGameMode((GameModeType)1))
			{
				return ((GorillaTagManager)instance).currentInfected.Count == list.Count;
			}
			if (IsGameMode((GameModeType)6) || IsGameMode((GameModeType)4))
			{
				return ((GorillaTagManager)(GorillaAmbushManager)instance).currentInfected.Count == list.Count;
			}
			if (IsGameMode((GameModeType)5))
			{
				GorillaFreezeTagManager val = (GorillaFreezeTagManager)instance;
				return ((GorillaTagManager)val).currentInfected.Count + val.currentFrozen.Keys.Count >= list.Count;
			}
			if (IsGameMode((GameModeType)9))
			{
				return ((GorillaTagManager)(GorillaPropHuntGameManager)instance).currentInfected.Count == list.Count;
			}
			if (IsGameMode((GameModeType)10))
			{
				return ((GorillaTagManager)(GorillaTagCompetitiveManager)instance).currentInfected.Count == list.Count;
			}
			if (IsGameMode((GameModeType)11))
			{
				return ((GorillaTagManager)SuperInfectionGame.instance).currentInfected.Count == list.Count;
			}
			if (IsGameMode((GameModeType)2))
			{
				return ((GorillaHuntManager)instance).currentHunted.Count == list.Count - 1;
			}
			if (IsGameMode((GameModeType)3))
			{
				return list.Where((VRRig ri) => !CanHit(ri)).ToList().Count == list.Count - 1;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static async void Tagged(NetPlayer p)
	{
		if (ActualTagged(RigUtils.RigFromNet(p)))
		{
			return;
		}
		GorillaGameManager m = GorillaGameManager.instance;
		if (IsGameMode((GameModeType)1))
		{
			((GorillaTagManager)m).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)4))
		{
			((GorillaTagManager)(GorillaAmbushManager)m).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)9))
		{
			((GorillaTagManager)(GorillaPropHuntGameManager)m).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)5))
		{
			((GorillaTagManager)(GorillaFreezeTagManager)m).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)10))
		{
			((GorillaTagManager)(GorillaTagCompetitiveManager)m).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)11))
		{
			((GorillaTagManager)SuperInfectionGame.instance).AddInfectedPlayer(p, true);
		}
		if (IsGameMode((GameModeType)2))
		{
			((GorillaHuntManager)m).currentHunted.Add(p);
		}
		if (IsGameMode((GameModeType)8))
		{
			GorillaGuardianZoneManager.zoneManagers.ForEach((GorillaGuardianZoneManager g) =>
			{
				g.SetGuardian(p);
			});
		}
		if (IsGameMode((GameModeType)3))
		{
			((GorillaPaintbrawlManager)m).playerLives[p.GetPlayerRef().ActorNumber] = 0;
		}
		if (IsGameMode((GameModeType)6) || IsGameMode((GameModeType)4))
		{
			((GorillaTagManager)(GorillaAmbushManager)m).AddInfectedPlayer(p, true);
		}
	}

	public static async void UnTag(NetPlayer p)
	{
		if (ActualTagged(RigUtils.RigFromNet(p)))
		{
			GorillaGameManager m = GorillaGameManager.instance;
			if (IsGameMode((GameModeType)1))
			{
				((GorillaTagManager)m).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)4))
			{
				((GorillaTagManager)(GorillaAmbushManager)m).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)9))
			{
				((GorillaTagManager)(GorillaPropHuntGameManager)m).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)5))
			{
				((GorillaTagManager)(GorillaFreezeTagManager)m).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)10))
			{
				((GorillaTagManager)(GorillaTagCompetitiveManager)m).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)11))
			{
				((GorillaTagManager)SuperInfectionGame.instance).currentInfected.Remove(p);
			}
			if (IsGameMode((GameModeType)2))
			{
				((GorillaHuntManager)m).currentHunted.Remove(p);
			}
			if (IsGameMode((GameModeType)8))
			{
				((GorillaGuardianManager)m).EjectGuardian(p);
			}
			if (IsGameMode((GameModeType)3))
			{
				((GorillaPaintbrawlManager)m).playerLives[p.GetPlayerRef().ActorNumber] = 3;
			}
			if (IsGameMode((GameModeType)6) || IsGameMode((GameModeType)4))
			{
				((GorillaTagManager)(GorillaAmbushManager)m).currentInfected.Remove(p);
			}
		}
	}

	public static GorillaRopeSwing Held()
	{
		return (from e in Mods.F<GorillaRopeSwing>(5f)
			where e.ropeId == OwnRig().grabbedRopeIndex
			select e).FirstOrDefault();
	}

	public static void Mat(NetPlayer p)
	{
		if (IsGameMode((GameModeType)3))
		{
			PaintMat(RigUtils.RigFromNet(p));
			return;
		}
		new Delay().D(matd, () =>
		{
			mat++;
			if (mat > 2)
			{
				mat = 1;
			}
			if (mat == 1)
			{
				Tagged(p);
			}
			if (mat == 2)
			{
				UnTag(p);
			}
		});
	}

	public static bool IsGameMode(GameModeType t)
	{
		if (!PhotonNetwork.InRoom)
		{
			return false;
		}
		return GorillaGameManager.instance.GameType() == t;
	}

	public static bool IsSuper()
	{
		return IsGameMode((GameModeType)11) || IsGameMode((GameModeType)12);
	}

	public static bool CanHit(VRRig r)
	{
		return TaggedName(r) != "HIT" && !ActualTagged(r) && (!((GorillaPaintbrawlManager)GorillaGameManager.instance).teamBattle || !((GorillaPaintbrawlManager)GorillaGameManager.instance).OnSameTeam(r.Creator, OwnRig().Creator));
	}

	public static string TaggedName(VRRig rig)
	{
		try
		{
			string name = ((Object)((Renderer)rig.mainSkin).material).name;
			if (IsGameMode((GameModeType)1) || IsGameMode((GameModeType)10) || IsGameMode((GameModeType)11))
			{
				if (name.Contains("fected"))
				{
					return "LAVA MONKE";
				}
				if (name.Contains("It"))
				{
					return "ROCK MONKE";
				}
			}
			if (IsGameMode((GameModeType)2) && name.Contains("ice (Instance)"))
			{
				return "HUNTED";
			}
			if ((IsGameMode((GameModeType)4) || IsGameMode((GameModeType)6)) && name.Contains("GhostSkeleton"))
			{
				return "GHOST";
			}
			if (IsGameMode((GameModeType)5))
			{
				if (name.Contains("Ice_Body"))
				{
					return "ICE";
				}
				if (rig.iceCubeLeft.activeSelf)
				{
					return "FROZEN";
				}
			}
			if (IsGameMode((GameModeType)9))
			{
				return ((GorillaTagManager)(GorillaPropHuntGameManager)GorillaGameManager.instance).currentInfected.Contains(rig.Creator) ? "FOUND" : "PROP";
			}
			if (IsGameMode((GameModeType)8) && ((GorillaGuardianManager)GorillaGameManager.instance).IsPlayerGuardian(rig.Creator))
			{
				return "GUARDIAN";
			}
			if (IsGameMode((GameModeType)3))
			{
				if (name.Contains("paintsplatter"))
				{
					return "KILLED";
				}
				if (name.Contains("stunned"))
				{
					return "STUNNED";
				}
				if (name.Contains("It") || name.ToLower().EndsWith("it"))
				{
					return "HIT";
				}
			}
			return "UNTAGGED";
		}
		catch
		{
			return "UNTAGGED";
		}
	}

	public static void Master(string b, Action s)
	{
		if (Local().IsMasterClient)
		{
			s();
			return;
		}
		SendNoti(Er + "You Must Be Master!");
		DisableButton(b);
	}

	public static void Speed(float s)
	{
		Loco().maxJumpSpeed = s;
	}

	public static void Arm(float l)
	{
		((Component)Loco()).transform.localScale = new Vector3(l, l, l);
	}

	public static void SendNoti(string noti)
	{
		NotifiLib.Send(noti);
	}

	public static Color ESPColor(VRRig r, bool s = false)
	{
		if (change9 == 0)
		{
			return Rainbow();
		}
		if (change9 == 1)
		{
			return r.playerColor;
		}
		if (change9 == 2)
		{
			byte b2 = (byte)(s ? byte.MaxValue : 100);
			if (ActualTagged(r))
			{
				return Color32.op_Implicit(new Color32(byte.MaxValue, (byte)0, (byte)0, b2));
			}
			return Color32.op_Implicit(new Color32((byte)0, byte.MaxValue, (byte)0, b2));
		}
		if (change9 == 3)
		{
			return MenuColor();
		}
		return CurrentESPColor;
	}

	public static Vector3 RandomV(float d)
	{
		Vector3[] array = new Vector3[14]
		{
			new Vector3(d, 0f, 0f),
			new Vector3(d, d, 0f),
			new Vector3(d, d, d),
			new Vector3(d, 0f, d),
			new Vector3(0f, 0f, d),
			new Vector3(0f, d, d),
			new Vector3(0f, d, 0f),
			new Vector3(0f - d, 0f, 0f),
			new Vector3(0f - d, 0f - d, 0f),
			new Vector3(0f - d, 0f - d, 0f - d),
			new Vector3(0f - d, 0f, 0f - d),
			new Vector3(0f, 0f, 0f - d),
			new Vector3(0f, 0f - d, 0f - d),
			new Vector3(0f, 0f - d, 0f)
		};
		return array[Random.Range(0, array.Length)];
	}

	public static void SetSP()
	{
		SP = ((SP == Vector3.zero) ? WristMenu.Sp : Vector3.Lerp(SP, WristMenu.S3(WristMenu.Sp), OwnRig().lerpValueBody * 0.66f));
	}

	public static Vector3 Sphere(Vector3 p, float d)
	{
		return p + Random.onUnitSphere * d;
	}

	public static HashSet<string> GetCos(VRRig r)
	{
		return WristMenu.allcos.Keys.Contains(RigUtils.PlayerFromRig(r)) ? WristMenu.allcos[((Object)(object)r == (Object)(object)OwnRig()) ? Local() : RigUtils.PlayerFromRig(r)] : Create(r).Field(Field(0)).GetValue<HashSet<string>>();
	}

	public static CosmeticItem Cos(string s)
	{
		return Con().GetItemFromDict(s);
	}

	public static string RPC(int r)
	{
		ReflectionEnums.RPC rPC = (ReflectionEnums.RPC)r;
		return rPC.ToString();
	}

	public static string Field(int f)
	{
		ReflectionEnums.Fields fields = (ReflectionEnums.Fields)f;
		return fields.ToString();
	}

	public static Player Local()
	{
		return PhotonNetwork.LocalPlayer;
	}

	public static Transform BC()
	{
		return ((Component)Loco().bodyCollider).transform;
	}

	public static VRRig OwnRig()
	{
		return VRRig.LocalRig;
	}

	public static GTPlayer Loco()
	{
		return GTPlayer.Instance;
	}

	public static GorillaTagger GIn()
	{
		return GorillaTagger.Instance;
	}

	public static Traverse Create(object o)
	{
		return Traverse.Create(o);
	}

	public static CosmeticsController Con()
	{
		return CosmeticsController.instance;
	}

	public static bool CosDone()
	{
		return Create(OwnRig()).Field(Field(58)).GetValue<bool>();
	}

	public static GameObject CreatePrimitive(PrimitiveType t)
	{
		return GameObject.CreatePrimitive(t);
	}

	public static T[] F<T>(float d = 5f) where T : Object
	{
		Type typeFromHandle = typeof(T);
		if (!r.TryGetValue(typeFromHandle, out var value))
		{
			value = -1f;
		}
		if (Time.time > value)
		{
			t.Remove(typeFromHandle);
			r[typeFromHandle] = Time.time + d;
		}
		if (!t.ContainsKey(typeFromHandle))
		{
			Dictionary<Type, object[]> dictionary = t;
			object[] value2 = (object[])(object)Object.FindObjectsByType<T>((FindObjectsInactive)1, (FindObjectsSortMode)0);
			dictionary.Add(typeFromHandle, value2);
		}
		return (T[])(object)t[typeFromHandle];
	}

	public static T[] GetTy<T>() where T : Object
	{
		EquipmentInteractor instance = EquipmentInteractor.instance;
		if (instance.leftHandHeldEquipment != null && instance.rightHandHeldEquipment != null)
		{
			T val = default;
			Ha(r: false).TryGetComponent<T>(ref val);
			T val2 = default;
			Ha(r: true).TryGetComponent<T>(ref val2);
			return new T[2] { val, val2 };
		}
		if (instance.leftHandHeldEquipment != null)
		{
			T val3 = default;
			Ha(r: false).TryGetComponent<T>(ref val3);
			return new T[1] { val3 };
		}
		if (instance.rightHandHeldEquipment != null)
		{
			T val4 = default;
			Ha(r: true).TryGetComponent<T>(ref val4);
			return new T[1] { val4 };
		}
		return new T[0];
	}

	public static Dictionary<string, Color> AllColors(string x = "both", string[] o = null)
	{
		Dictionary<string, Color> dictionary = new Dictionary<string, Color>();
		if (x != "" && o == null)
		{
			if (x == "both" || x.Contains("all") || x == "nocl")
			{
				if (!x.Contains("nocl"))
				{
					dictionary["Clear"] = Color.grey;
				}
				dictionary["RGB"] = Color.grey;
				if (x.Contains("all"))
				{
					dictionary["Menu Theme"] = Color.grey;
				}
			}
			else
			{
				dictionary[x] = Color.grey;
			}
		}
		if (o != null)
		{
			foreach (string key in o)
			{
				dictionary[key] = Color.grey;
			}
		}
		foreach (PropertyInfo item in from e in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
			where e.PropertyType == typeof(Color) && e.Name != "clear" && e.Name != "Grey" && !char.IsNumber(e.Name.Last())
			select e)
		{
			string text = char.ToUpper(item.Name[0]) + item.Name.Substring(1);
			string text2 = text.Substring(1);
			for (int num = 0; num < text2.Length; num++)
			{
				char c2 = text2[num];
				if (char.IsUpper(c2))
				{
					text = text.Replace(c2.ToString(), " " + c2);
				}
			}
			Color val = (Color)item.GetValue(null);
			dictionary[text + " <color=#" + ColorUtility.ToHtmlStringRGB(val) + ">▬</color>"] = val;
		}
		return dictionary;
	}

	static Mods()
	{
	}
}
