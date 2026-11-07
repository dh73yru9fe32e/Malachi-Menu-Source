using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using GTAG_NotificationLib;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Gameplay;
using GorillaLocomotion.Swimming;
using GorillaNetworking;
using GorillaTag.Gravity;
using GorillaTag.Rendering;
using GorillaTagScripts;
using GorillaTagScripts.Builder;
using GorillaTagScripts.ScavengerHunt;
using HarmonyLib;
using MalachiTemp.Backend;
using MalachiTemp.Utilities;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Voxels;
using MalachiTemp.Gui;

namespace MalachiTemp.UI;

internal class WristMenu : MonoBehaviour
{
	[HarmonyPatch(typeof(GameEntityManager), "CreateItemRPC")]
	public class GameEntityManagerCreateItemRPCPatch
	{
		public static bool e;

		public static bool a;

		public static bool Prefix(int[] netId, int[] entityTypeId, long[] packedPos, int[] packedRot, long[] createData, int[] createdByEntityNetId, PhotonMessageInfo info)
		{
			if (info.Sender != Mods.Local())
			{
				if (e && Extensions.Contains(entityTypeId, 1517967647))
				{
					return false;
				}
				if (a && Extensions.Contains(entityTypeId, 618507822))
				{
					return false;
				}
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Voxel_Pickaxe), "UpdateInteractionPoint")]
	public class VoxelPickaxeUpdateInteractionPointPatch
	{
		public static bool e;

		public static bool b;

		public static void Postfix(Voxel_Pickaxe __instance, ref InteractionPoint point)
		{
			__instance.mine.radius = (b ? 4.7f : 0.5f);
			__instance.hitCooldown = (e ? 0f : 0.5f);
			__instance.minMineSpeed = (e ? 0f : 5f);
			__instance.alignThreshold = (e ? 0f : 0.7f);
			__instance.minHitSpeed = (e ? 0f : 1f);
		}
	}

	[HarmonyPatch(typeof(LoadBalancingTransport), "onEventActionVoiceClient")]
	public class LoadBalancingTransportonEventActionVoiceClientPatch
	{
		public static bool e;

		public static bool Prefix(LoadBalancingTransport __instance, EventData ev)
		{
			if (e && ev.Sender != Mods.Local().ActorNumber)
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(VRRig), "GrabbedByPlayer")]
	public class VRRigGrabbedByPlayerPatch
	{
		public static bool e;

		public static bool Prefix(VRRig grabbedByRig, bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand)
		{
			if (e)
			{
				return Mods.TaggedName(grabbedByRig) != "GUARDIAN";
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(SecondLookSkeleton), "FloatPlayer")]
	public class SecondLookSkeletonFloatPlayerPatch
	{
		public static bool e;

		public static bool Prefix()
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(SecondLookSkeleton), "ChuckPlayer")]
	public class SecondLookSkeletonChuckPlayerPatch
	{
		public static bool Prefix()
		{
			return !SecondLookSkeletonFloatPlayerPatch.e;
		}
	}

	[HarmonyPatch(typeof(HalloweenGhostChaser), "RiseGrabbedLocalPlayer")]
	public class HalloweenGhostChaserRiseGrabbedLocalPlayerPatch
	{
		public static bool e;

		public static bool Prefix()
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(LurkerGhost), "SeekPlayer")]
	public class LurkerGhostSeekPlayerPatch
	{
		public static bool e;

		public static bool Prefix()
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(PhotonNetworkController), "AttemptToFollowIntoPub")]
	public class PhotonNetworkControllerAttemptToFollowIntoPubPatch
	{
		public static bool e;

		public static bool Prefix()
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(SubscriptionManager), "IsLocalSubscribed")]
	public class SubscriptionManagerIsLocalSubscribedPatch
	{
		public static bool e;

		public static bool Prefix(ref bool __result)
		{
			__result = e;
			return false;
		}
	}

	[HarmonyPatch(typeof(PhotonNetwork), "RunViewUpdate")]
	public class PhotonNetworkRunViewUpdatePatch
	{
		public static event Action o;

		public static bool Prefix()
		{
			o?.Invoke();
			return true;
		}
	}

	[HarmonyPatch(typeof(GTPlayer), "ApplyKnockback")]
	public class GTPlayerApplyKnockbackPatch
	{
		public static bool e;

		public static bool Prefix(Vector3 direction, float speed)
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(SIPlayer), "PlayerKnockback")]
	public class SIPlayerPlayerKnockbackPatch
	{
		public static bool e;

		public static bool Prefix(Vector3 directionAndMagnitude)
		{
			return !e;
		}
	}

	[HarmonyPatch(typeof(BuilderTableNetworking), "PieceCreatedByShelfRPC")]
	public class BuilderTableNetworkingPieceCreatedByShelfRPCPatch
	{
		public static bool e;

		public static void Postfix(BuilderTableNetworking __instance, int pieceType, int pieceId, long packedPosition, int packedRotation, int materialType, byte state, int shelfID, Player creatingPlayer, PhotonMessageInfo info)
		{
			if (!e || (creatingPlayer == Mods.Local() && info.Sender == Mods.Local()))
			{
				return;
			}
			BuilderPiece piece = Mods.Create(__instance).Field(Mods.Field(59)).GetValue<BuilderTable>()
				.GetPiece(pieceId);
			if (new int[6] { -741198570, -1447051713, -1961073389, 577606505, 1934114066, -1468322569 }.Any((int e) => pieceType == e))
			{
				try
				{
					piece.GetTable().builderRenderer.RemovePiece(piece);
					Object.Destroy((Object)(object)((Component)piece).gameObject);
					Object.Destroy((Object)(object)piece);
				}
				catch
				{
				}
			}
		}
	}

	[HarmonyPatch(typeof(GorillaScoreBoard), "RedrawPlayerLines")]
	public class GorillaScoreBoardRedrawPlayerLinesPatch
	{
		public static void Postfix(GorillaScoreBoard __instance)
		{
			string text = ((TMP_Text)__instance.boardText).text;
			try
			{
				GorillaPlayerScoreboardLine[] array = __instance.lines.ToArray();
				string[] array2 = ((TMP_Text)__instance.boardText).text.Split(new string[1] { "\n" }, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					int num = i + (PhotonNetwork.CurrentRoom.Name.Contains("\n\n") ? 4 : (PhotonNetwork.CurrentRoom.Name.Contains("\n") ? 3 : 2));
					if (num < 0 || num >= array2.Length)
					{
						continue;
					}
					Player playerRef = array[i].linePlayer.GetPlayerRef();
					if (playerRef == Mods.Local())
					{
						continue;
					}
					IsUsing(playerRef, out var text2);
					if (text2 == "<color=#00FFFF>" || text2 == "<color=#FF00FF>")
					{
						string text3 = (array2[num].Contains("<color=") ? array2[num].Split('>')[1].Trim().Replace("</color", "") : array2[num]);
						if (array2[num].Contains(text3))
						{
							array2[num] = array2[num].Replace(array2[num], text2 + " " + text3.Replace(" ", "") + "</color>");
						}
					}
				}
				((TMP_Text)__instance.boardText).text = string.Join("\n", array2);
			}
			catch
			{
				((TMP_Text)__instance.boardText).text = text;
			}
			if (Mods.ant && (Object)(object)Mods.Find("ShowAntiReport") != (Object)null)
			{
				Object.Destroy((Object)(object)Mods.Find("ShowAntiReport"));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetWristJet), "OnUpdateAuthority")]
	public class SIGadgetWristJetOnUpdateAuthorityPatch
	{
		public static bool e;

		public static float[] og = new float[3];

		public static float PhotonNetworkRunViewUpdatePatch = 0f;

		public static void Postfix(SIGadgetWristJet __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				if (og[2] == 0f)
				{
					og[0] = __instance.jetForce;
					og[1] = __instance.maxHorizontalSpeed;
					og[2] = __instance.maxVerticalSpeed;
				}
				if (e)
				{
					Mods.Create(__instance).Field(Mods.Field(2)).SetValue(Mods.Create(__instance).Field(Mods.Field(3)).GetValue());
				}
				bool flag = PhotonNetworkRunViewUpdatePatch == 0f;
				__instance.jetForce = (flag ? og[0] : PhotonNetworkRunViewUpdatePatch);
				__instance.maxHorizontalSpeed = (flag ? og[1] : PhotonNetworkRunViewUpdatePatch);
				__instance.maxVerticalSpeed = (flag ? og[2] : PhotonNetworkRunViewUpdatePatch);
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetTentacleArm), "OnUpdateAuthority")]
	public class SIGadgetTentacleArmOnUpdateAuthorityPatch
	{
		public static bool e;

		public static Dictionary<SIGadgetTentacleArm, float> ss = new Dictionary<SIGadgetTentacleArm, float>();

		public static void Postfix(SIGadgetTentacleArm __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!ss.ContainsKey(__instance))
				{
					ss.Add(__instance, __instance.MaxTentacleJumpSpeed);
				}
				if (e)
				{
					val.Field(Mods.Field(2)).SetValue(val.Field(Mods.Field(3)).GetValue());
				}
				float[][] array = new float[5][]
				{
					new float[2] { 1.5f, 5f },
					new float[2] { 5f, 10f },
					new float[2] { 10f, 15f },
					new float[2] { 15f, 20f },
					new float[2] { 20f, 25f }
				};
				float[] array2 = new float[3]
				{
					ss[__instance],
					10f,
					20f
				};
				if (!Mods.IsEnabled("Tentacle Size Changer"))
				{
					__instance.LengthFactor = array[yolaa][0];
					val.Field(Mods.Field(37)).SetValue((object)array[yolaa][1]);
				}
				__instance.MaxGrabAngle = 180f;
				val.Field(Mods.Field(38)).SetValue((object)float.MaxValue);
				val.Field(Mods.Field(39)).SetValue((object)array2[yolaaa]);
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetDashYoyo), "OnUpdateAuthority")]
	public class SIGadgetDashYoyoOnUpdateAuthorityPatch
	{
		public static bool e;

		public static float og;

		public static float os;

		public static float of;

		public static void Postfix(SIGadgetDashYoyo __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (og == 0f)
				{
					og = (float)val.Field(Mods.Field(5)).GetValue();
				}
				if (os == 0f)
				{
					os = (float)val.Field(Mods.Field(13)).GetValue();
				}
				if (of == 0f)
				{
					of = (float)val.Field(Mods.Field(16)).GetValue();
				}
				float[][] array = new float[3][]
				{
					new float[3] { os, 11f, 13f },
					new float[3] { 18f, 18f, 18f },
					new float[3] { 45f, 45f, 45f }
				};
				float[][] array2 = new float[3][]
				{
					new float[3] { of, 1.5f, 2f },
					new float[3] { 3f, 3f, 3f },
					new float[3] { 6.7f, 6.7f, 6.7f }
				};
				val.Field(Mods.Field(5)).SetValue((object)(e ? 0f : og));
				val.Field(Mods.Field(6)).SetValue((object)(e ? 0f : 6f));
				val.Field(Mods.Field(7)).SetValue((object)(e ? 0f : 5f));
				val.Field(Mods.Field(8)).SetValue((object)(e ? 0f : 2f));
				val.Field(Mods.Field(13)).SetValue((object)array[yost][0]);
				val.Field(Mods.Field(14)).SetValue((object)array[yost][1]);
				val.Field(Mods.Field(15)).SetValue((object)array[yost][2]);
				val.Field(Mods.Field(16)).SetValue((object)array2[yofs][0]);
				val.Field(Mods.Field(17)).SetValue((object)array2[yofs][1]);
				val.Field(Mods.Field(18)).SetValue((object)array2[yofs][2]);
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetPlatformDeployer), "OnUpdateAuthority")]
	public class SIGadgetPlatformDeployerOnUpdateAuthorityPatch
	{
		public static bool e;

		public static Dictionary<SIGadgetPlatformDeployer, int> PhotonNetworkRunViewUpdatePatch = new Dictionary<SIGadgetPlatformDeployer, int>();

		public static void Postfix(SIGadgetPlatformDeployer __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!PhotonNetworkRunViewUpdatePatch.ContainsKey(__instance))
				{
					PhotonNetworkRunViewUpdatePatch.Add(__instance, val.Field(Mods.Field(9)).GetValue<int>());
				}
				int num = PhotonNetworkRunViewUpdatePatch[__instance];
				val.Field(Mods.Field(9)).SetValue((object)(e ? (num * 1000) : num));
				val.Field(Mods.Field(10)).SetValue((object)(e ? (num * 1000) : 3));
				val.Field(Mods.Field(11)).SetValue((object)(e ? (num * 1000) : 5));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetStilt), "OnUpdateAuthority")]
	public class SIGadgetStiltOnUpdateAuthorityPatch
	{
		public static bool r;

		public static bool f;

		public static Dictionary<SIGadgetStilt, float[]> PhotonNetworkRunViewUpdatePatch = new Dictionary<SIGadgetStilt, float[]>();

		public static void Postfix(SIGadgetStilt __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!PhotonNetworkRunViewUpdatePatch.ContainsKey(__instance))
				{
					PhotonNetworkRunViewUpdatePatch.Add(__instance, new float[6]
					{
						val.Field(Mods.Field(25)).GetValue<float>(),
						val.Field(Mods.Field(26)).GetValue<float>(),
						val.Field(Mods.Field(27)).GetValue<float>(),
						val.Field(Mods.Field(28)).GetValue<float>(),
						val.Field(Mods.Field(29)).GetValue<float>(),
						val.Field(Mods.Field(30)).GetValue<float>()
					});
				}
				float[][] array = new float[4][]
				{
					new float[3]
					{
						PhotonNetworkRunViewUpdatePatch[__instance][0],
						PhotonNetworkRunViewUpdatePatch[__instance][3],
						PhotonNetworkRunViewUpdatePatch[__instance][4]
					},
					new float[3] { 4f, 4f, 4f },
					new float[3] { 8f, 8f, 8f },
					new float[3] { 20f, 20f, 20f }
				};
				val.Field(Mods.Field(25)).SetValue((object)array[yola][0]);
				val.Field(Mods.Field(28)).SetValue((object)array[yola][1]);
				val.Field(Mods.Field(29)).SetValue((object)array[yola][2]);
				val.Field(Mods.Field(26)).SetValue((object)(r ? 100f : PhotonNetworkRunViewUpdatePatch[__instance][1]));
				val.Field(Mods.Field(27)).SetValue((object)(r ? 100f : PhotonNetworkRunViewUpdatePatch[__instance][2]));
				val.Field(Mods.Field(30)).SetValue((object)(f ? 5555f : PhotonNetworkRunViewUpdatePatch[__instance][5]));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetChargeBlaster), "OnUpdateAuthority")]
	public class SIGadgetChargeBlasterOnUpdateAuthorityPatch
	{
		public static bool e;

		public static void Postfix(SIGadgetChargeBlaster __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Mods.Create(__instance).Field(Mods.Field(20)).SetValue((object)(e ? 0.0001f : 0.2f));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetCooldownBlaster), "OnUpdateAuthority")]
	public class SIGadgetCooldownBlasterOnUpdateAuthorityPatch
	{
		public static void Postfix(SIGadgetCooldownBlaster __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				__instance.fireCooldown = (SIGadgetChargeBlasterOnUpdateAuthorityPatch.e ? 0.0001f : 0.5f);
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetLaserZipline), "OnUpdateAuthority")]
	public class SIGadgetLaserZiplineOnUpdateAuthorityPatch
	{
		public static Dictionary<SIGadgetLaserZipline, object[]> ss = new Dictionary<SIGadgetLaserZipline, object[]>();

		public static bool b;

		public static bool c;

		public static bool y = false;

		public static void Postfix(SIGadgetLaserZipline __instance, float dt)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!ss.ContainsKey(__instance))
				{
					ss.Add(__instance, new object[2]
					{
						val.Field(Mods.Field(43)).GetValue<float>(),
						val.Field(Mods.Field(41)).GetValue<ResettableUseCounter>()
					});
				}
				if (c)
				{
					val.Field(Mods.Field(41)).SetValue((object)new ResettableUseCounter(int.MaxValue, int.MaxValue, (Action<bool>)null));
					y = false;
				}
				else if (!y)
				{
					val.Field(Mods.Field(41)).SetValue(ss[__instance][1]);
					y = true;
				}
				val.Field(Mods.Field(43)).SetValue((object)(b ? 30f : ((float)ss[__instance][0])));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetAirGrab), "FixedUpdate")]
	public class SIGadgetAirGrabFixedUpdatePatch
	{
		public static Dictionary<SIGadgetAirGrab, object[]> ss = new Dictionary<SIGadgetAirGrab, object[]>();

		public static bool a;

		public static bool b;

		public static bool y = false;

		public static void Postfix(SIGadgetAirGrab __instance)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!ss.ContainsKey(__instance))
				{
					ss.Add(__instance, new object[2]
					{
						val.Field(Mods.Field(52)).GetValue<float>(),
						val.Field(Mods.Field(51)).GetValue<ResettableUseCounter>()
					});
				}
				if (a)
				{
					val.Field(Mods.Field(51)).SetValue((object)new ResettableUseCounter(int.MaxValue, int.MaxValue, (Action<bool>)null));
					y = false;
				}
				else if (!y)
				{
					val.Field(Mods.Field(51)).SetValue(ss[__instance][1]);
					y = true;
				}
				val.Field(Mods.Field(52)).SetValue((object)(b ? float.MaxValue : ((float)ss[__instance][0])));
			}
		}
	}

	[HarmonyPatch(typeof(SIGadgetAirJuke), "FixedUpdate")]
	public class SIGadgetAirJukeFixedUpdatePatch
	{
		public static Dictionary<SIGadgetAirJuke, object[]> ss = new Dictionary<SIGadgetAirJuke, object[]>();

		public static bool a;

		public static bool b;

		public static bool y = false;

		public static void Postfix(SIGadgetAirJuke __instance)
		{
			if (Mods.IsSuper())
			{
				Traverse val = Mods.Create(__instance);
				if (!ss.ContainsKey(__instance))
				{
					ss.Add(__instance, new object[4]
					{
						val.Field(Mods.Field(53)).GetValue<float>(),
						val.Field(Mods.Field(13)).GetValue<float>(),
						val.Field(Mods.Field(51)).GetValue<ResettableUseCounter>(),
						val.Field(Mods.Field(54)).GetValue<float>()
					});
				}
				if (a)
				{
					val.Field(Mods.Field(51)).SetValue((object)new ResettableUseCounter(int.MaxValue, int.MaxValue, (Action<bool>)null));
					y = false;
				}
				else if (!y)
				{
					val.Field(Mods.Field(51)).SetValue(ss[__instance][2]);
					y = true;
				}
				val.Field(Mods.Field(54)).SetValue((object)(b ? 25f : ((float)ss[__instance][3])));
				val.Field(Mods.Field(53)).SetValue((object)(b ? 25f : ((float)ss[__instance][0])));
				val.Field(Mods.Field(13)).SetValue((object)(b ? 24f : ((float)ss[__instance][1])));
			}
		}
	}

	internal class ModSidedMenu : MonoBehaviour
	{
		private static int g;

		public static void SendMenuStuff(byte c, object[] o)
		{
			PhotonNetwork.RaiseEvent(c, (object)o, new RaiseEventOptions
			{
				TargetActors = Mods.usingmenu.Select((Player e) => e.ActorNumber).ToArray()
			}, SendOptions.SendReliable);
			Mods.RPCFlush();
		}

		public static object[] SB()
		{
			List<ButtonInfo> list = Mods.CurrentButtons();
			object[] array = new object[list.Count * 2];
			int num = 0;
			foreach (ButtonInfo item in list)
			{
				array[num++] = item.buttonText;
				array[num++] = item.enabled;
			}
			return array;
		}

		public static object[] GetButtons(object[] b)
		{
			List<ButtonInfo> list = new List<ButtonInfo>();
			for (int i = 0; i < b.Length; i += 2)
			{
				list.Add(new ButtonInfo
				{
					buttonText = (string)b[i],
					enabled = (bool)b[i + 1]
				});
			}
			return new object[1] { list };
		}

		public static void TrackMenus()
		{
			foreach (Player key in modsidedmenus.Keys)
			{
				if (!RigUtils.AllPlayers(i: false).Contains(key))
				{
					continue;
				}
				VRRig val = RigUtils.RigFromPlayer(key);
				if (!modsidedextras.ContainsKey(key))
				{
					continue;
				}
				if ((bool)modsidedextras[key][5])
				{
					SpawnSidedBoard(val, new object[8]
					{
						modsidedextras[key][6],
						modsidedextras[key][7],
						modsidedextras[key][8],
						modsidedextras[key][9],
						modsidedextras[key][10],
						modsidedextras[key][11],
						modsidedextras[key][12],
						modsidedextras[key][13]
					});
				}
				else
				{
					bool flag = (bool)modsidedextras[key][0];
					if ((Object)(object)modsidedmenus[key] != (Object)null)
					{
						try
						{
							if (flag ? (((VRMap)val.rightThumb).calcT > 0.3f) : (((VRMap)val.leftThumb).calcT > 0.3f))
							{
								GameObject val2 = modsidedmenus[key];
								Transform val3 = (flag ? val.rightHandTransform : val.leftHandTransform);
								val2.transform.position = (flag ? (val3.position + val3.forward * (0.053f * val.scaleFactor) + val3.right * (0.026f * val.scaleFactor) + val3.up * (0.11f * val.scaleFactor)) : (val3.position + val3.forward * (0.053f * val.scaleFactor) + val3.right * (-0.026f * val.scaleFactor) + val3.up * (0.11f * val.scaleFactor)));
								val2.transform.rotation = val3.rotation;
								val2.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * val.scaleFactor;
								if (!flag)
								{
									val2.transform.Rotate(-40f, 10f, 190f);
								}
								else
								{
									val2.transform.Rotate(-40f, 0f, -10f);
								}
							}
							else if (modsidedmenus.Keys.ToList().Contains(key))
							{
								Object.Destroy((Object)(object)modsidedmenus[key]);
								modsidedmenus[key] = null;
								modsidedextras.Remove(key);
								modsidedmenus.Remove(key);
								break;
							}
						}
						catch
						{
						}
					}
				}
				if (!((Object)(object)modsidedmenus[key] != (Object)null))
				{
					continue;
				}
				try
				{
					string text = (string)modsidedextras[key][3];
					string text2 = text;
					int num = (int)Mathf.PingPong(Time.time / 0.167f, (float)(text.Length + 1));
					if ((bool)modsidedextras[key][4])
					{
						text2 = ((num > 1) ? text.Substring(0, num) : "M");
					}
					Mods.Find("ModSidedname" + key.UserId).GetComponent<Text>().text = text2 + $"\n Fps: {Mods.Create(val).Field(Mods.Field(12)).GetValue()} | Page: {(int)modsidedextras[key][2] + 1}";
				}
				catch
				{
				}
			}
		}

		public static async void FixLag(string c)
		{
			int m = ++g;
			await Task.Delay(150);
			if (m == g)
			{
				Send(c);
			}
		}

		public static async void Send(string c)
		{
			if (Mods.usingmenu.Count > 0 && Mods.sided)
			{
				SendMenuStuff(72, new object[1] { SB() });
				await Task.Delay(150);
				SendMenuStuff(71, new object[38]
				{
					Mods.change20,
					c,
					Mods.change10,
					Mods.change11,
					Mods.change28,
					pageNumber,
					Mods.change33,
					tooltipString,
					pageSize,
					Mods.right,
					true,
					Version(),
					Mods.change7,
					Mods.change12,
					Mods.change13,
					Mods.change14,
					Mods.change15,
					Mods.change31,
					Mods.change4,
					Mods.change29,
					Mods.change30,
					Mods.change32,
					Mods.searchB,
					Mods.change34,
					Mods.change35,
					Mods.homeB,
					Mods.change42,
					Mods.change43,
					Mods.InMain(),
					Mods.dis,
					Mods.change50,
					Mods.tyname,
					Mods.change36,
					Mods.change37,
					Mods.change38,
					Mods.change39,
					(Object)(object)Mods.keyboard != (Object)null,
					Mods.typed
				});
			}
		}

		public static bool Valid(object[] o)
		{
			try
			{
				if ((int)o[0] < 1 || (int)o[0] > 4)
				{
					return false;
				}
				if ((int)o[1] < 0 || (int)o[1] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[2] < 0 || (int)o[2] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[3] < 0 || (int)o[3] > 5)
				{
					return false;
				}
				if ((int)o[5] < 0 || (int)o[5] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[7] < 4 || (int)o[7] > 8)
				{
					return false;
				}
				if (!(o[8] is bool))
				{
					return false;
				}
				if (!(o[9] is bool))
				{
					return false;
				}
				if ((float)o[10] < Version() - 0.02f || (float)o[10] > Version() + 0.02f)
				{
					return false;
				}
				if ((int)o[11] < 1 || (int)o[11] > 6)
				{
					return false;
				}
				if ((int)o[12] < 0 || (int)o[12] > Mods.AllColors("Clear").Count)
				{
					return false;
				}
				if ((int)o[13] < 0 || (int)o[13] > Mods.AllColors("Clear").Count)
				{
					return false;
				}
				if ((int)o[14] < 0 || (int)o[14] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[15] < 0 || (int)o[15] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[16] < 0 || (int)o[16] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[17] < 1 || (int)o[17] > 4)
				{
					return false;
				}
				if ((int)o[18] < 0 || (int)o[18] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[19] < 0 || (int)o[19] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[20] < 0 || (int)o[20] > Mods.AllColors("").Count)
				{
					return false;
				}
				if (!(o[21] is bool))
				{
					return false;
				}
				if ((int)o[22] < 0 || (int)o[22] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[23] < 0 || (int)o[23] > Mods.AllColors("").Count)
				{
					return false;
				}
				if (!(o[24] is bool))
				{
					return false;
				}
				if ((int)o[25] < 0 || (int)o[25] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[26] < 0 || (int)o[26] > Mods.AllColors("").Count)
				{
					return false;
				}
				if (!(o[27] is bool))
				{
					return false;
				}
				if (!(o[28] is bool))
				{
					return false;
				}
				if ((int)o[29] < 0 || (int)o[29] > 9)
				{
					return false;
				}
				if (!(o[30] is bool))
				{
					return false;
				}
				if ((int)o[31] < 0 || (int)o[31] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[32] < 0 || (int)o[32] > Mods.AllColors().Count)
				{
					return false;
				}
				if ((int)o[33] < 0 || (int)o[33] > Mods.AllColors("").Count)
				{
					return false;
				}
				if ((int)o[34] < 0 || (int)o[34] > 2)
				{
					return false;
				}
				if (!(o[35] is bool))
				{
					return false;
				}
				if (((string)o[36]).Length > 30)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void DrawMenu(EventData d)
		{
			try
			{
				object[] array = (object[])d.CustomData;
				Player player = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(d.Sender, false);
				if (!Mods.usingmenu.Contains(player))
				{
					return;
				}
				string text = (string)array[1];
				if (text == "des" && modsidedmenus.Keys.ToList().Contains(player))
				{
					Object.Destroy((Object)(object)modsidedmenus[player]);
					try
					{
						Object.Destroy((Object)(GameObject)sidedkeyboards[player][0]);
						Object.Destroy((Object)(GameObject)sidedkeyboards[player][1]);
						Object.Destroy((Object)(TextMesh)sidedkeyboards[player][2]);
						sidedkeyboards.Remove(player);
					}
					catch
					{
					}
					modsidedmenus[player] = null;
					modsidedextras.Remove(player);
					modsidedmenus.Remove(player);
				}
				if (!(text == "spawn"))
				{
					return;
				}
				if (modsidedmenus.Keys.ToList().Contains(player))
				{
					Object.Destroy((Object)(object)modsidedmenus[player]);
					try
					{
						Object.Destroy((Object)(GameObject)sidedkeyboards[player][0]);
						Object.Destroy((Object)(GameObject)sidedkeyboards[player][1]);
						Object.Destroy((Object)(TextMesh)sidedkeyboards[player][2]);
						sidedkeyboards.Remove(player);
					}
					catch
					{
					}
					modsidedmenus[player] = null;
					modsidedextras.Remove(player);
					modsidedmenus.Remove(player);
				}
				object[] array2 = new object[37]
				{
					array[0],
					array[2],
					array[3],
					array[4],
					array[5],
					array[6],
					array[7],
					array[8],
					array[9],
					array[10],
					array[11],
					array[12],
					array[13],
					array[14],
					array[15],
					array[16],
					array[17],
					array[18],
					array[19],
					array[20],
					array[21],
					array[22],
					array[23],
					array[24],
					array[25],
					array[26],
					array[27],
					array[28],
					array[29],
					array[30],
					array[31],
					array[32],
					array[33],
					array[34],
					array[35],
					array[36],
					array[37]
				};
				if (Valid(array2))
				{
					Color color = Mods.AllColors().Values.ToList()[(int)array2[1]];
					Color color2 = Mods.AllColors("").Values.ToList()[(int)array2[2]];
					Color color3 = Mods.AllColors("").Values.ToList()[(int)array2[5]];
					Font val = Mods.fonts[(int)array2[3]];
					GameObject val2 = Mods.CreatePrimitive((PrimitiveType)3);
					((Object)val2).name = "ModSidedMenu" + player.UserId;
					Object.Destroy((Object)(object)val2.GetComponent<Rigidbody>());
					Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
					Object.Destroy((Object)(object)val2.GetComponent<Renderer>());
					val2.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f);
					GameObject val3 = Mods.CreatePrimitive((PrimitiveType)3);
					Object.Destroy((Object)(object)val3.GetComponent<Rigidbody>());
					Object.Destroy((Object)(object)val3.GetComponent<BoxCollider>());
					Transform transform = val3.transform;
					transform.parent = val2.transform;
					transform.rotation = Quaternion.identity;
					if ((int)array2[0] == 1)
					{
						transform.localScale = new Vector3(0.1f, 1f, 1f) * 1f;
					}
					if ((int)array2[0] == 2)
					{
						transform.localScale = new Vector3(0.1f, 1f, 1.3f) * 1f;
					}
					if ((int)array2[0] == 3)
					{
						transform.localScale = new Vector3(0.1f, 1.25f, 1f) * 1f;
					}
					if ((int)array2[0] == 4)
					{
						transform.localScale = new Vector3(0.1f, 1.25f, 1.3f) * 1f;
					}
					Gradient val4 = new Gradient();
					if ((int)array2[1] == 1)
					{
						val4 = Mods.RainbowG();
					}
					else
					{
						GradientColorKey[] array3 = new GradientColorKey[4];
						array3[0].color = color;
						array3[0].time = 0f;
						array3[1].color = color;
						array3[1].time = 0.3f;
						array3[2].color = color2;
						array3[2].time = 0.6f;
						array3[3].color = color;
						array3[3].time = 1f;
						val4 = new Gradient
						{
							colorKeys = array3
						};
					}
					ColorChanger colorChanger = val3.AddComponent<ColorChanger>();
					colorChanger.colors = val4;
					colorChanger.Start();
					if ((int)array2[1] == 0)
					{
						Object.Destroy((Object)(object)val3.GetComponent<Renderer>());
						Object.Destroy((Object)(object)val3.GetComponent<ColorChanger>());
					}
					transform.position = new Vector3(0.05f, 0f, 0f) * 1f;
					GameObject val5 = new GameObject();
					val5.transform.parent = val2.transform;
					Canvas val6 = val5.AddComponent<Canvas>();
					CanvasScaler val7 = val5.AddComponent<CanvasScaler>();
					val5.AddComponent<GraphicRaycaster>();
					val6.renderMode = (RenderMode)2;
					val7.dynamicPixelsPerUnit = 2800f;
					GameObject val8 = new GameObject();
					val8.transform.parent = val5.transform;
					Text val9 = val8.AddComponent<Text>();
					((Object)((Component)val9).gameObject).name = "ModSidedname" + player.UserId;
					string text2 = ((float)array2[10]).ToString();
					int num = text2.IndexOf('.');
					if (num != -1 && num + 2 < text2.Length)
					{
						text2 = text2.Insert(num + 2, ".");
					}
					val9.font = val;
					string text3 = (val9.text = "Malachis Menu Reborn V" + text2);
					val9.fontSize = 1;
					val9.alignment = (TextAnchor)4;
					((Graphic)val9).color = color3;
					val9.resizeTextForBestFit = true;
					val9.resizeTextMinSize = 0;
					RectTransform component = ((Component)val9).GetComponent<RectTransform>();
					((Transform)component).localPosition = Vector3.zero;
					component.sizeDelta = new Vector2(0.28f, 0.05f);
					if (((int)array2[0] == 2) | ((int)array2[0] == 4))
					{
						((Transform)component).position = new Vector3(0.06f, 0f, 0.225f);
					}
					if (((int)array2[0] == 1) | ((int)array2[0] == 3))
					{
						((Transform)component).position = new Vector3(0.06f, 0f, 0.175f);
					}
					((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
					GameObject val10 = new GameObject();
					val10.transform.parent = val5.transform;
					Text val11 = val10.AddComponent<Text>();
					((Object)((Component)val11).gameObject).name = "MSC" + player.UserId;
					val11.font = val;
					val11.text = fcc;
					val11.fontSize = 4;
					val11.alignment = (TextAnchor)4;
					((Graphic)val11).color = color3;
					val11.resizeTextForBestFit = true;
					val11.resizeTextMinSize = 0;
					RectTransform component2 = ((Component)val11).GetComponent<RectTransform>();
					((Transform)component2).localPosition = Vector3.zero;
					component2.sizeDelta = new Vector2(0.28f, 0.05f);
					((Transform)component2).position = new Vector3(0.03f, 0f, 0f);
					((Transform)component2).rotation = Quaternion.Euler(new Vector3(0f, 90f, 90f));
					object[] o = new object[21]
					{
						val2,
						val5,
						val,
						array2[0],
						array2[4],
						array2[7],
						array2[16],
						array2[17],
						array2[11],
						array2[18],
						array2[19],
						array2[20],
						array2[21],
						array2[22],
						array2[23],
						array2[24],
						array2[25],
						array2[26],
						array2[27],
						array2[28],
						array2[29]
					};
					AddSidedPageButtons(o);
					object[] buttons = GetButtons(sidedbuttons[player]);
					string[] array4 = ((List<ButtonInfo>)buttons[0]).Select((ButtonInfo buttonInfo) => buttonInfo.buttonText).ToArray();
					if ((int)array2[4] != 0 && array4[0] == "Exit Enabled Mods")
					{
						array4 = array4.Skip(1).ToArray();
					}
					object[] o2 = new object[10]
					{
						val2,
						array2[0],
						array2[11],
						val5,
						val,
						array2[12],
						(List<ButtonInfo>)buttons[0],
						array2[15],
						array2[13],
						array2[14]
					};
					for (int num2 = 0; num2 < array4.Length; num2++)
					{
						AddSidedButton((float)num2 * 0.13f + 0.26f, array4[num2], o2);
					}
					GameObject val12 = new GameObject();
					val12.transform.SetParent(val5.transform);
					val12.transform.localPosition = new Vector3(0f, 0f, 1f) * 1f;
					Text val13 = val12.GetComponent<Text>();
					if ((Object)(object)val13 == (Object)null)
					{
						val13 = val12.AddComponent<Text>();
					}
					val13.font = val;
					val13.text = (string)array2[6];
					val13.fontSize = 20;
					val13.alignment = (TextAnchor)4;
					val13.resizeTextForBestFit = true;
					val13.resizeTextMinSize = 0;
					((Graphic)val13).color = color3;
					RectTransform component3 = val12.GetComponent<RectTransform>();
					((Transform)component3).localPosition = Vector3.zero;
					component3.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
					if (((int)array2[0] == 1) | ((int)array2[0] == 3))
					{
						((Transform)component3).position = new Vector3(0.06f, 0f, -0.18f) * 1f;
					}
					if (((int)array2[0] == 2) | ((int)array2[0] == 4))
					{
						((Transform)component3).position = new Vector3(0.06f, 0f, -0.23f) * 1f;
					}
					((Transform)component3).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
					modsidedextras.Add(player, new object[14]
					{
						array2[8],
						array2[9],
						array2[4],
						text3,
						array2[30],
						array2[35],
						array2[31],
						array2[32],
						array2[33],
						array2[34],
						array2[0],
						array2[17],
						array2[11],
						array2[36]
					});
					modsidedmenus.Add(player, val2);
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("ModSided Menu: " + ex.Message));
			}
		}

		public static void SpawnSidedBoard(VRRig r, object[] o)
		{
			float scaleFactor = r.scaleFactor;
			Vector3 val = ((Component)r).transform.position + new Vector3(0f, -0.3f, 0f) * scaleFactor + ((Component)r).transform.forward * (0.57f * scaleFactor) + ((Component)r).transform.right * (-0.302f * scaleFactor);
			Player val2 = RigUtils.PlayerFromRig(r);
			Vector3[] array = (new Vector3[3][]
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
			})[(int)o[3]];
			if (!sidedkeyboards.Keys.Contains(val2))
			{
				GameObject val3 = Mods.Find("Environment Objects/LocalObjects_Prefab/SharedBlocksMapSelectLobby/BuilderSharedMapsTerminal/ControlsGroup/keyboard (1)");
				GameObject val4 = Object.Instantiate<GameObject>(val3, val + new Vector3(0f, 6f * scaleFactor, 0f), ((Component)r).transform.rotation);
				((Object)val4).name = "SidedKeyboard" + val2.UserId;
				val4.transform.position = val;
				val4.transform.rotation = ((Component)r).transform.rotation;
				val4.transform.Rotate(-55f, 180f, 0f);
				Transform transform = val4.transform;
				transform.localScale *= scaleFactor;
				ExtraButtons(val4, scaleFactor);
				List<SharedBlocksKeyboardButton> list = (from e in val4.GetComponentsInChildren<SharedBlocksKeyboardButton>()
					where ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString.ToLower() != "random"
					select e).ToList();
				GameObject val5 = new GameObject("Buttons" + val2.UserId);
				val5.transform.SetParent(val4.transform);
				List<SharedBlocksKeyboardButton> list2 = new List<SharedBlocksKeyboardButton>();
				for (int num = 0; num < list.Count; num++)
				{
					GameObject val6 = Mods.CreatePrimitive((PrimitiveType)3);
					Object.Destroy((Object)(object)val6.GetComponent<Collider>());
					val6.transform.SetParent(val5.transform);
					val6.transform.position = ((Component)list[num]).transform.position;
					Vector3 val7 = ((((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)list[num]).characterString == "enter" || ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)list[num]).characterString == "delete") ? array[2] : array[0]);
					val6.transform.localScale = val7 * scaleFactor;
					val6.transform.rotation = ((Component)list[num]).transform.rotation;
					val6.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("GorillaTag/UberShaderNonSRP");
					val6.GetComponent<Renderer>().sharedMaterial.color = Mods.AllColors().Values.ToList()[(int)o[1]];
					if ((int)o[1] == 0 && (Object)(object)val6.GetComponent<Renderer>() != (Object)null)
					{
						Object.Destroy((Object)(object)val6.GetComponent<Renderer>());
					}
					if ((int)o[1] == 1)
					{
						ColorChanger colorChanger = val6.AddComponent<ColorChanger>();
						colorChanger.colors = Mods.RainbowG();
						colorChanger.Start();
					}
					list2.Add(list[num]);
				}
				string text = "SidedKeyboard" + val2.UserId + "/Buttons/";
				Object.Destroy((Object)(object)Mods.Find(text + "Text/controlButtonText"));
				Object.Destroy((Object)(object)Mods.Find(text + "Text/TerminalControl"));
				Object.Destroy((Object)(object)Mods.Find(text + "Keys/control"));
				Object.Destroy((Object)(object)val4.GetComponent<Collider>());
				Object.Destroy((Object)(object)Mods.Find(text + "Text/random"));
				Object.Destroy((Object)(object)Mods.Find(text + "Keys/randomizekey"));
				((TMP_Text)Mods.Find(text + "Text/enter").GetComponent<TextMeshPro>()).text = "SPACE";
				TextMeshPro[] componentsInChildren = val4.GetComponentsInChildren<TextMeshPro>();
				foreach (TextMeshPro val8 in componentsInChildren)
				{
					if (!((Behaviour)val8).enabled)
					{
						((Behaviour)val8).enabled = true;
					}
					Vector3 localScale = array[1];
					if (((TMP_Text)val8).text == "SPACE" || ((TMP_Text)val8).text == "ENTER" || ((TMP_Text)val8).text == "DELETE")
					{
						localScale = array[3];
					}
					val8.transform.localScale = localScale;
					((Graphic)val8).color = Mods.AllColors("").Values.ToList()[(int)o[2]];
				}
				GameObject val9 = new GameObject("wtv" + val2.UserId);
				val9.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f) * scaleFactor;
				TextMesh val10 = val9.AddComponent<TextMesh>();
				val10.fontSize = 45;
				val10.fontStyle = (FontStyle)0;
				val10.characterSize = 0.03f;
				val10.anchor = (TextAnchor)4;
				val10.alignment = (TextAlignment)0;
				val10.color = Mods.AllColors("").Values.ToList()[(int)o[0]];
				try
				{
					if (list2.Count > 0)
					{
						foreach (SharedBlocksKeyboardButton item in list2.ToList())
						{
							Object.Destroy((Object)(object)item);
							list2.Remove(item);
						}
					}
				}
				catch
				{
				}
				val4.transform.position = val;
				val4.transform.rotation = ((Component)r).transform.rotation;
				val4.transform.Rotate(-55f, 180f, 0f);
				val9.transform.position = val4.transform.position + new Vector3(0f, 0.23f * scaleFactor, 0f) + ((Component)r).transform.forward * (0.105f * scaleFactor) + ((Component)r).transform.right * (0.302f * scaleFactor);
				val9.transform.rotation = val4.transform.rotation;
				val9.transform.LookAt(((Component)r).transform.position + new Vector3(0f, -0.035f * scaleFactor, 0f));
				val9.transform.Rotate(0f, 180f, 0f);
				sidedkeyboards.Add(val2, new object[3] { val4, val9, val10 });
			}
			if (sidedkeyboards.Keys.Contains(val2) && (Object)(GameObject)sidedkeyboards[val2][0] != (Object)null)
			{
				if (Vector3.Distance(((GameObject)sidedkeyboards[val2][0]).transform.position, val) > 0.355f * scaleFactor)
				{
					((GameObject)sidedkeyboards[val2][0]).transform.position = val;
					((GameObject)sidedkeyboards[val2][0]).transform.rotation = ((Component)r).transform.rotation;
					((GameObject)sidedkeyboards[val2][0]).transform.Rotate(-55f, 180f, 0f);
					((GameObject)sidedkeyboards[val2][1]).transform.position = ((GameObject)sidedkeyboards[val2][0]).transform.position + new Vector3(0f, 0.23f * scaleFactor, 0f) + ((Component)r).transform.forward * (0.105f * scaleFactor) + ((Component)r).transform.right * (0.302f * scaleFactor);
					((GameObject)sidedkeyboards[val2][1]).transform.rotation = ((GameObject)sidedkeyboards[val2][0]).transform.rotation;
					((GameObject)sidedkeyboards[val2][1]).transform.LookAt(((Component)r).transform.position + new Vector3(0f, -0.035f * scaleFactor, 0f));
					((GameObject)sidedkeyboards[val2][1]).transform.Rotate(0f, 180f, 0f);
				}
				((TextMesh)sidedkeyboards[val2][2]).text = (string)o[7] + "_";
				Vector3 val11 = new Vector3(0f, 0.23f, 0f) * scaleFactor;
				if (((int)o[4] == 2) | ((int)o[5] == 4))
				{
					val11.y += 0.06f * scaleFactor;
				}
				if ((int)o[5] == 4)
				{
					val11.y += 0.055f * scaleFactor;
				}
				if ((int)o[6] == 4)
				{
					val11.y += 0.04f * scaleFactor;
				}
				try
				{
					modsidedmenus[val2].transform.position = ((GameObject)sidedkeyboards[val2][1]).transform.position + val11 + ((GameObject)sidedkeyboards[val2][1]).transform.forward * (0.11f * scaleFactor);
					modsidedmenus[val2].transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * scaleFactor;
					modsidedmenus[val2].transform.rotation = ((GameObject)sidedkeyboards[val2][1]).transform.rotation;
					modsidedmenus[val2].transform.Rotate(-90f, 90f, 0f);
				}
				catch
				{
				}
			}
		}

		public static void ExtraButtons(GameObject key, float a)
		{
			SharedBlocksKeyboardButton[] componentsInChildren = key.GetComponentsInChildren<SharedBlocksKeyboardButton>();
			TextMeshPro[] componentsInChildren2 = key.GetComponentsInChildren<TextMeshPro>();
			GameObject gameObject = ((Component)componentsInChildren.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString == "M").FirstOrDefault()).gameObject;
			GameObject gameObject2 = ((Component)componentsInChildren.Where((SharedBlocksKeyboardButton e) => ((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)e).characterString == "L").FirstOrDefault()).gameObject;
			GameObject gameObject3 = ((Component)componentsInChildren2.Where((TextMeshPro e) => ((TMP_Text)e).text == "M").FirstOrDefault()).gameObject;
			GameObject gameObject4 = ((Component)componentsInChildren2.Where((TextMeshPro e) => ((TMP_Text)e).text == "L").FirstOrDefault()).gameObject;
			((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject, gameObject.transform.position + gameObject.transform.up * (-0.05f * a), gameObject.transform.rotation, gameObject.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = "(";
			((TMP_Text)Object.Instantiate<GameObject>(gameObject3, gameObject3.transform.position + gameObject3.transform.right * (0.0499f * a), gameObject3.transform.rotation, gameObject3.transform.parent).GetComponent<TextMeshPro>()).text = "(";
			((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject, gameObject.transform.position + gameObject.transform.up * (-0.1f * a), gameObject.transform.rotation, gameObject.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = ")";
			((TMP_Text)Object.Instantiate<GameObject>(gameObject3, gameObject3.transform.position + gameObject3.transform.right * (0.0998f * a), gameObject3.transform.rotation, gameObject3.transform.parent).GetComponent<TextMeshPro>()).text = ")";
			((GorillaKeyButton<SharedBlocksKeyboardBindings>)(object)Object.Instantiate<GameObject>(gameObject2, gameObject2.transform.position + gameObject2.transform.up * (-0.05f * a), gameObject2.transform.rotation, gameObject2.transform.parent).GetComponent<SharedBlocksKeyboardButton>()).characterString = ":";
			((TMP_Text)Object.Instantiate<GameObject>(gameObject4, gameObject4.transform.position + gameObject4.transform.right * (0.0499f * a), gameObject4.transform.rotation, gameObject4.transform.parent).GetComponent<TextMeshPro>()).text = ":";
		}

		private static void AddSidedButton(float offset, string text, object[] o)
		{
			if (text == "Keyboard")
			{
				return;
			}
			GameObject val = (GameObject)o[0];
			List<ButtonInfo> list = (List<ButtonInfo>)o[6];
			GameObject val2 = Mods.CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val2.GetComponent<Rigidbody>());
			Object.Destroy((Object)(object)val2.GetComponent<BoxCollider>());
			val2.transform.parent = val.transform;
			val2.transform.rotation = Quaternion.identity;
			val2.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
			Transform transform = val2.transform;
			if (((int)o[1] == 3) | ((int)o[1] == 4))
			{
				transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
			}
			if ((int)o[2] == 1)
			{
				transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);
			}
			if (((int)o[2] == 2) | ((int)o[2] == 3) | ((int)o[2] == 4) | ((int)o[2] == 5) | ((int)o[2] == 6))
			{
				transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset);
			}
			if (((int)o[1] == 2) | ((int)o[1] == 4))
			{
				transform.localPosition = new Vector3(0.56f, 0f, 0.7f - offset);
			}
			if ((((int)o[1] == 2) | ((int)o[1] == 4)) && (int)o[2] == 1)
			{
				transform.localPosition = new Vector3(0.56f, 0f, 0.48f - offset);
			}
			int index = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (text == list[i].buttonText)
				{
					index = i;
					break;
				}
			}
			GameObject val3 = new GameObject();
			val3.transform.parent = ((GameObject)o[3]).transform;
			Text val4 = val3.AddComponent<Text>();
			val4.font = (Font)o[4];
			val4.text = text;
			val4.fontSize = 1;
			val4.alignment = (TextAnchor)4;
			val4.resizeTextForBestFit = true;
			val4.resizeTextMinSize = 0;
			RectTransform component = ((Component)val4).GetComponent<RectTransform>();
			((Transform)component).localPosition = Vector3.zero;
			component.sizeDelta = new Vector2((((int)o[1] == 3) | ((int)o[1] == 4)) ? 0.27f : 0.2f, 0.03f) * 1f;
			if ((int)o[2] == 1)
			{
				((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.111f - offset / 2.55f);
			}
			if (((int)o[2] == 2) | ((int)o[2] == 3) | ((int)o[2] == 4) | ((int)o[2] == 5) | ((int)o[2] == 6))
			{
				((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.237f - offset / 2.55f);
			}
			if (((int)o[1] == 2) | ((int)o[1] == 4))
			{
				((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.275f - offset / 2.55f);
			}
			if ((((int)o[1] == 2) | ((int)o[1] == 4)) && (int)o[2] == 1)
			{
				((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.19f - offset / 2.55f);
			}
			((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			Renderer component2 = val2.GetComponent<Renderer>();
			Color val5 = Mods.AllColors("Clear").Values.ToList()[(int)o[5]];
			Color[] array = Mods.AllColors("").Values.ToArray();
			Color val6 = Mods.AllColors("Clear").Values.ToList()[(int)o[8]];
			if (list[0].buttonText == "Exit Enabled Mods")
			{
				if (text != "Exit Enabled Mods" && text != "Disable All Mods" && text != "Save Mods")
				{
					component2.material.color = val6;
					((Graphic)val4).color = array[(int)o[9]];
					if ((int)o[8] == 0)
					{
						Object.Destroy((Object)(object)component2);
					}
				}
				else
				{
					component2.material.color = val5;
					((Graphic)val4).color = array[(int)o[7]];
					if ((int)o[5] == 0)
					{
						Object.Destroy((Object)(object)component2);
					}
				}
			}
			else
			{
				bool flag = Mods.IsEnabled(list[index]);
				component2.material.color = (flag ? val6 : val5);
				((Graphic)val4).color = (flag ? array[(int)o[9]] : array[(int)o[7]]);
				if (flag ? ((int)o[8] == 0) : ((int)o[5] == 0))
				{
					Object.Destroy((Object)(object)component2);
				}
			}
		}

		private static void AddSidedPageButtons(object[] o)
		{
			GameObject val = (GameObject)o[0];
			GameObject val2 = (GameObject)o[1];
			Font font = (Font)o[2];
			int num = (int)o[3];
			int num2 = (int)o[6];
			int num3 = (int)o[7];
			int num4 = (int)o[8];
			int num5 = (int)o[9];
			int index = (int)o[10];
			int index2 = (int)o[11];
			int num6 = (int)o[13];
			int num7 = (int)o[16];
			bool flag = (bool)o[12];
			string text = (new string[10] { "< >", "← →", "⇦ ⇨", "↩ ↪", "◀ ▶", "◁ ▷", "◅ ▻", "◂ ▸", "⇚ ⇛", "⇇ ⇉" })[(int)o[20]];
			if (num4 < 5)
			{
				GameObject val3 = Mods.CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val3.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val3.GetComponent<BoxCollider>());
				val3.transform.parent = val.transform;
				val3.transform.rotation = Quaternion.identity;
				val3.GetComponent<Renderer>().material.color = Mods.AllColors().Values.ToList()[num2];
				GameObject val4 = new GameObject();
				val4.transform.parent = val2.transform;
				Text val5 = val4.AddComponent<Text>();
				val5.font = font;
				val5.text = text[0].ToString();
				val5.fontSize = 1;
				val5.alignment = (TextAnchor)4;
				val5.resizeTextForBestFit = true;
				((Graphic)val5).color = Mods.AllColors("").Values.ToList()[index2];
				val5.resizeTextMinSize = 0;
				RectTransform component = ((Component)val5).GetComponent<RectTransform>();
				((Transform)component).localPosition = Vector3.zero;
				component.sizeDelta = new Vector2(0.2f, 0.03f);
				((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
				GameObject val6 = Mods.CreatePrimitive((PrimitiveType)3);
				Object.Destroy((Object)(object)val6.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val6.GetComponent<BoxCollider>());
				val6.transform.parent = val.transform;
				val6.transform.rotation = Quaternion.identity;
				val6.GetComponent<Renderer>().material.color = Mods.AllColors().Values.ToList()[num2];
				GameObject val7 = new GameObject();
				val7.transform.parent = val2.transform;
				Text val8 = val7.AddComponent<Text>();
				val8.font = font;
				val8.text = text[2].ToString();
				val8.fontSize = 1;
				val8.alignment = (TextAnchor)4;
				val8.resizeTextForBestFit = true;
				((Graphic)val8).color = Mods.AllColors("").Values.ToList()[index2];
				val8.resizeTextMinSize = 0;
				RectTransform component2 = ((Component)val8).GetComponent<RectTransform>();
				((Transform)component2).localPosition = Vector3.zero;
				component2.sizeDelta = new Vector2(0.2f, 0.03f);
				((Transform)component2).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
				if (num4 == 1)
				{
					float num8 = 0f;
					val3.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
					val3.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num8);
					((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.111f - num8 / 2.55f);
					num8 = 0.13f;
					val6.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
					val6.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num8);
					((Transform)component2).localPosition = new Vector3(0.064f, 0f, 0.111f - num8 / 2.55f);
					if ((num == 2) | (num == 4))
					{
						val3.transform.localPosition = new Vector3(0.56f, 0f, 0.45f);
						((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.17701961f);
						val6.transform.localPosition = new Vector3(0.56f, 0f, 0.45f - num8);
						((Transform)component2).localPosition = new Vector3(0.064f, 0f, 0.128f);
					}
					if ((num == 3) | (num == 4))
					{
						val3.transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
						val6.transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
					}
				}
				if (num4 == 2)
				{
					val3.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
					val3.transform.localPosition = new Vector3(0.56f, 0.37f, 0.541f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.11f, 0.215f);
					val6.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
					val6.transform.localPosition = new Vector3(0.56f, -0.37f, 0.541f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, 0.215f);
					if ((num == 2) | (num == 4))
					{
						val3.transform.localPosition = new Vector3(0.56f, 0.37f, 0.71f);
						((Transform)component).localPosition = new Vector3(0.064f, 0.11f, 0.28f);
						val6.transform.localPosition = new Vector3(0.56f, -0.37f, 0.71f);
						((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, 0.28f);
					}
				}
				if (num4 == 3)
				{
					val3.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
					val3.transform.localPosition = new Vector3(0.56f, 0.657f, 0.0063f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.2f, 0.0063f);
					val6.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
					val6.transform.localPosition = new Vector3(0.56f, -0.657f, 0.0063f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.2f, 0.0063f);
					if ((num == 3) | (num == 4))
					{
						val3.transform.localPosition = new Vector3(0.56f, 0.8f, 0.0063f);
						((Transform)component).localPosition = new Vector3(0.064f, 0.244f, 0.0063f);
						val6.transform.localPosition = new Vector3(0.56f, -0.8f, 0.0063f);
						((Transform)component2).localPosition = new Vector3(0.064f, -0.244f, 0.0063f);
					}
				}
				if (num4 == 4)
				{
					val3.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
					val3.transform.localPosition = new Vector3(0.56f, 0.37f, -0.541f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.11f, -0.215f);
					val6.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
					val6.transform.localPosition = new Vector3(0.56f, -0.37f, -0.541f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, -0.215f);
					if ((num == 2) | (num == 4))
					{
						val3.transform.localPosition = new Vector3(0.56f, 0.37f, -0.71f);
						((Transform)component).localPosition = new Vector3(0.064f, 0.11f, -0.28f);
						val6.transform.localPosition = new Vector3(0.56f, -0.37f, -0.71f);
						((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, -0.28f);
					}
				}
				if (num2 == 0 && (Object)(object)val6.GetComponent<Renderer>() != (Object)null)
				{
					Object.Destroy((Object)(object)val3.GetComponent<Renderer>());
					Object.Destroy((Object)(object)val6.GetComponent<Renderer>());
				}
				if (num2 == 1)
				{
					ColorChanger colorChanger = val3.AddComponent<ColorChanger>();
					colorChanger.colors = Mods.RainbowG();
					colorChanger.Start();
					ColorChanger colorChanger2 = val6.AddComponent<ColorChanger>();
					colorChanger2.colors = Mods.RainbowG();
					colorChanger2.Start();
				}
			}
			if ((bool)o[19])
			{
				GameObject val9 = Mods.CreatePrimitive((PrimitiveType)3);
				((Object)val9).name = "dis";
				Object.Destroy((Object)(object)val9.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val9.GetComponent<BoxCollider>());
				val9.transform.parent = val.transform;
				val9.transform.rotation = Quaternion.identity;
				val9.GetComponent<Renderer>().material.color = Mods.AllColors().Values.ToList()[num5];
				GameObject val10 = new GameObject
				{
					name = "dt"
				};
				val10.transform.parent = val2.transform;
				Text val11 = val10.AddComponent<Text>();
				val11.font = font;
				val11.text = "Disconnect";
				val11.fontSize = 1;
				val11.alignment = (TextAnchor)4;
				val11.resizeTextForBestFit = true;
				val11.resizeTextMinSize = 0;
				((Graphic)val11).color = Mods.AllColors("").Values.ToList()[index];
				RectTransform component3 = ((Component)val11).GetComponent<RectTransform>();
				((Transform)component3).localPosition = Vector3.zero;
				component3.sizeDelta = new Vector2(0.2f, 0.03f);
				((Transform)component3).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
				if (num3 == 1)
				{
					float num9 = 0.26f;
					val9.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
					val9.transform.localPosition = new Vector3(0.56f, -0.8f, 0.35f - num9);
					((Transform)component3).localPosition = new Vector3(0.06f, -0.24f, 0.14f - num9 / 2.55f);
					if (num4 == 3)
					{
						val9.transform.localPosition = new Vector3(0.56f, -1.1f, 0.35f - num9);
						((Transform)component3).localPosition = new Vector3(0.06f, -0.33f, 0.14f - num9 / 2.55f);
					}
					if ((num == 3) | (num == 4))
					{
						val9.transform.localPosition = new Vector3(0.56f, -0.95f, 0.35f - num9);
						((Transform)component3).localPosition = new Vector3(0.06f, -0.281f, 0.14f - num9 / 2.55f);
					}
					if (((num == 3) | (num == 4)) && num4 == 3)
					{
						val9.transform.localPosition = new Vector3(0.56f, -1.25f, 0.35f - num9);
						((Transform)component3).localPosition = new Vector3(0.06f, -0.371f, 0.14f - num9 / 2.55f);
					}
				}
				if (num3 == 2)
				{
					float num10 = 0.26f;
					val9.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
					val9.transform.localPosition = new Vector3(0.56f, 0.8f, 0.35f - num10);
					((Transform)component3).localPosition = new Vector3(0.06f, 0.24f, 0.14f - num10 / 2.55f);
					if (num4 == 3)
					{
						val9.transform.localPosition = new Vector3(0.56f, 1.1f, 0.35f - num10);
						((Transform)component3).localPosition = new Vector3(0.06f, 0.33f, 0.14f - num10 / 2.55f);
					}
					if ((num == 3) | (num == 4))
					{
						val9.transform.localPosition = new Vector3(0.56f, 0.95f, 0.35f - num10);
						((Transform)component3).localPosition = new Vector3(0.06f, 0.281f, 0.14f - num10 / 2.55f);
					}
					if (((num == 3) | (num == 4)) && num4 == 3)
					{
						val9.transform.localPosition = new Vector3(0.56f, 1.25f, 0.35f - num10);
						((Transform)component3).localPosition = new Vector3(0.06f, 0.371f, 0.14f - num10 / 2.55f);
					}
				}
				if (num3 == 3)
				{
					val9.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
					val9.transform.localPosition = new Vector3(0.56f, 0f, 0.6f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.24f);
					if (num4 == 2)
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, 0.7f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.28f);
					}
					if ((num == 2) | (num == 4))
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, 0.76f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.305f);
					}
					if (((num == 2) | (num == 4)) && num4 == 2)
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, 0.83f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.33f);
					}
				}
				if (num3 == 4)
				{
					val9.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
					val9.transform.localPosition = new Vector3(0.56f, 0f, -0.6f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.24f);
					if (num4 == 4)
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, -0.7f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.28f);
					}
					if ((num == 2) | (num == 4))
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, -0.76f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.305f);
					}
					if (((num == 2) | (num == 4)) && num4 == 4)
					{
						val9.transform.localPosition = new Vector3(0.56f, 0f, -0.83f);
						((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.33f);
					}
				}
				if (num5 == 0 && (Object)(object)val9.GetComponent<Renderer>() != (Object)null)
				{
					Object.Destroy((Object)(object)val9.GetComponent<Renderer>());
				}
				if (num5 == 1)
				{
					ColorChanger colorChanger3 = val9.AddComponent<ColorChanger>();
					colorChanger3.colors = Mods.RainbowG();
					colorChanger3.Start();
				}
			}
			if (flag)
			{
				GameObject val12 = Mods.CreatePrimitive((PrimitiveType)3);
				((Object)val12).name = "search";
				Object.Destroy((Object)(object)val12.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val12.GetComponent<BoxCollider>());
				val12.transform.parent = val.transform;
				val12.transform.rotation = Quaternion.identity;
				val12.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val12.transform.localPosition = new Vector3(0.56f, -0.657f, 0.53f);
				val12.GetComponent<Renderer>().material.color = Mods.AllColors().Values.ToList()[num6];
				GameObject val13 = new GameObject();
				val13.transform.parent = val2.transform;
				Text val14 = val13.AddComponent<Text>();
				val14.font = font;
				val14.text = "Search";
				val14.fontSize = 1;
				val14.alignment = (TextAnchor)4;
				val14.resizeTextForBestFit = true;
				val14.resizeTextMinSize = 0;
				((Graphic)val14).color = Mods.AllColors("").Values.ToList()[(int)o[14]];
				RectTransform component4 = ((Component)val14).GetComponent<RectTransform>();
				((Transform)component4).localPosition = Vector3.zero;
				component4.sizeDelta = new Vector2(0.18f, 0.02f);
				((Transform)component4).localPosition = new Vector3(0.064f, -0.195f, 0.212f);
				((Transform)component4).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
				if ((num == 3) | (num == 4))
				{
					val12.transform.localPosition = new Vector3(0.56f, -0.8f, 0.53f);
					((Transform)component4).localPosition = new Vector3(0.064f, -0.24f, 0.212f);
				}
				if (num6 == 0 && (Object)(object)val12.GetComponent<Renderer>() != (Object)null)
				{
					Object.Destroy((Object)(object)val12.GetComponent<Renderer>());
				}
				if (num6 == 1)
				{
					ColorChanger colorChanger4 = val12.AddComponent<ColorChanger>();
					colorChanger4.colors = Mods.RainbowG();
					colorChanger4.Start();
				}
			}
			if ((bool)o[15] && (int)o[4] != 0 && !(bool)o[18])
			{
				GameObject val15 = Mods.CreatePrimitive((PrimitiveType)3);
				((Object)val15).name = "home";
				Object.Destroy((Object)(object)val15.GetComponent<Rigidbody>());
				Object.Destroy((Object)(object)val15.GetComponent<BoxCollider>());
				val15.transform.parent = val.transform;
				val15.transform.rotation = Quaternion.identity;
				val15.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val15.transform.localPosition = new Vector3(0.56f, -0.657f, flag ? 0.617f : 0.53f);
				val15.GetComponent<Renderer>().material.color = Mods.AllColors().Values.ToList()[num7];
				GameObject val16 = new GameObject();
				val16.transform.parent = val2.transform;
				Text val17 = val16.AddComponent<Text>();
				val17.font = font;
				val17.text = "Home";
				val17.fontSize = 1;
				val17.alignment = (TextAnchor)4;
				val17.resizeTextForBestFit = true;
				val17.resizeTextMinSize = 0;
				((Graphic)val17).color = Mods.AllColors("").Values.ToList()[(int)o[17]];
				RectTransform component5 = ((Component)val17).GetComponent<RectTransform>();
				((Transform)component5).localPosition = Vector3.zero;
				component5.sizeDelta = new Vector2(0.18f, 0.02f);
				((Transform)component5).localPosition = new Vector3(0.064f, -0.195f, flag ? 0.245f : 0.212f);
				((Transform)component5).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
				if ((num == 3) | (num == 4))
				{
					val15.transform.localPosition = new Vector3(0.56f, -0.8f, flag ? 0.617f : 0.53f);
					((Transform)component5).localPosition = new Vector3(0.064f, -0.24f, flag ? 0.245f : 0.212f);
				}
				if (num7 == 0 && (Object)(object)val15.GetComponent<Renderer>() != (Object)null)
				{
					Object.Destroy((Object)(object)val15.GetComponent<Renderer>());
				}
				if (num7 == 1)
				{
					ColorChanger colorChanger5 = val15.AddComponent<ColorChanger>();
					colorChanger5.colors = Mods.RainbowG();
					colorChanger5.Start();
				}
			}
		}
	}

	public static string MenuTitle = "Malachis Menu Reborn V9.1";

	public static Font MenuFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

	public static List<ButtonInfo> buttons = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Settings",
			method = () =>
			{
				Mods.Settings();
			},
			enabled = false,
			toolTip = "Go to Settings"
		},
		new ButtonInfo
		{
			buttonText = "Enabled Mods",
			method = () =>
			{
				Mods.Cat1();
			},
			enabled = false,
			toolTip = "Go to Enabled Mods"
		},
		new ButtonInfo
		{
			buttonText = "Favorite Mods",
			method = () =>
			{
				Mods.Cat12();
			},
			enabled = false,
			toolTip = "To favoriate a mod, hold both grips and press a mod"
		},
		new ButtonInfo
		{
			buttonText = "OP Mods",
			method = () =>
			{
				Mods.Cat2();
			},
			enabled = false,
			toolTip = "Go to OP Mods"
		},
		new ButtonInfo
		{
			buttonText = "Rig Mods",
			method = () =>
			{
				Mods.Cat3();
			},
			enabled = false,
			toolTip = "Go to Rig Mods"
		},
		new ButtonInfo
		{
			buttonText = "Movement Mods",
			method = () =>
			{
				Mods.Cat4();
			},
			enabled = false,
			toolTip = "Go to Movement Mods"
		},
		new ButtonInfo
		{
			buttonText = "SuperTag Mods",
			method = () =>
			{
				Mods.Cat5();
			},
			enabled = false,
			toolTip = "Go to SuperTag Mods"
		},
		new ButtonInfo
		{
			buttonText = "Block Mods",
			method = () =>
			{
				Mods.Cat6();
			},
			enabled = false,
			toolTip = "Go to Block Mods"
		},
		new ButtonInfo
		{
			buttonText = "Spammer Mods",
			method = () =>
			{
				Mods.Cat7();
			},
			enabled = false,
			toolTip = "Go to Spammer Mods"
		},
		new ButtonInfo
		{
			buttonText = "Visual Mods",
			method = () =>
			{
				Mods.Cat8();
			},
			enabled = false,
			toolTip = "Go to Visual Mods"
		},
		new ButtonInfo
		{
			buttonText = "Tag Mods",
			method = () =>
			{
				Mods.Cat9();
			},
			enabled = false,
			toolTip = "Go to Tag Mods"
		},
		new ButtonInfo
		{
			buttonText = "Other Mods",
			method = () =>
			{
				Mods.Cat10();
			},
			enabled = false,
			toolTip = "Go to Other Mods"
		}
	};

	public static List<ButtonInfo> settingsbuttons = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Settings",
			n = "Exit Settings",
			method = () =>
			{
				Mods.Settings();
			},
			enabled = false,
			toolTip = "Go To Main"
		},
		new ButtonInfo
		{
			buttonText = "Save Settings",
			n = "Save Settings",
			method = () =>
			{
				Mods.Save();
			},
			enabled = false,
			toolTip = "Save Your Settings"
		},
		new ButtonInfo
		{
			buttonText = "Load Settings",
			n = "Load Settings",
			method = () =>
			{
				Mods.Load();
			},
			enabled = false,
			toolTip = "Load Your Settings"
		},
		new ButtonInfo
		{
			buttonText = "Anti Report Disconnect",
			n = "Anti Report Disconnect",
			method = () =>
			{
				Mods.AntiReport();
			},
			enabled = false,
			toolTip = "Disconnects you when someone tries to report you"
		},
		new ButtonInfo
		{
			buttonText = "Anti Report Join Random",
			n = "Anti Report Join Random",
			method = () =>
			{
				Mods.AntiReport(1);
			},
			enabled = false,
			toolTip = "Disconnects you when someone tries to report you, then makes you join a random lobby in your current map"
		},
		new ButtonInfo
		{
			buttonText = "Anti Report Reconnect",
			n = "Anti Report Reconnect",
			method = () =>
			{
				Mods.AntiReport(2);
			},
			enabled = false,
			toolTip = "Disconnects you when someone tries to report you, then makes you join back"
		},
		new ButtonInfo
		{
			buttonText = "Anti Mod Checker",
			n = "Anti Mod Checker",
			method = () =>
			{
				Mods.AntiPep();
			},
			enabled = false,
			toolTip = "Shows that you have no mods on mod checkers"
		},
		new ButtonInfo
		{
			buttonText = "Anti Deep Check (This Menu)",
			n = "Anti Deep Check (This Menu)",
			method = () =>
			{
				Mods.AntiDeep();
			},
			enabled = false,
			toolTip = "Hides the mod menu from your plugins folder"
		},
		new ButtonInfo
		{
			buttonText = "Anti Deep Check (All Plugins)",
			n = "Anti Deep Check (All Plugins)",
			method = () =>
			{
				Mods.AntiDeep1();
			},
			enabled = false,
			toolTip = "Hides all the files inside your plugins folder"
		},
		new ButtonInfo
		{
			buttonText = "Anti Deep Check (BepInEx)",
			n = "Anti Deep Check (BepInEx)",
			method = () =>
			{
				Mods.AntiDeep2();
			},
			enabled = false,
			toolTip = "Hides the BepInEx folder from your Gorilla Tag game folder"
		},
		new ButtonInfo
		{
			buttonText = "Clear Notis",
			n = "Clear Notis",
			method = () =>
			{
				NotifiLib.Clear();
				Mods.DisableButton("Clear Notis");
			},
			enabled = false,
			toolTip = "Clears all notis"
		},
		new ButtonInfo
		{
			buttonText = "Mod Sided Menu",
			n = "Mod Sided Menu",
			method = () =>
			{
				Mods.ModSided();
			},
			enabled = false,
			toolTip = "When True, you can see the menu in other malachi menu users hands"
		},
		new ButtonInfo
		{
			buttonText = "Auto Save Settings",
			n = "Auto Save Settings",
			method = () =>
			{
				Mods.AutoSave();
			},
			enabled = false,
			toolTip = "When True, when you exit settings it will save your settings"
		},
		new ButtonInfo
		{
			buttonText = "Hide Visuals On PC Screen",
			n = "Hide Visuals On PC Screen",
			method = () =>
			{
				Mods.Changehide();
			},
			enabled = false,
			toolTip = "Changes wether visuals are hidden on camera or not"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Animated Menu Title",
			n = "Toggle Animated Menu Title",
			method = () =>
			{
				Mods.ToggleTyName();
			},
			enabled = false,
			toolTip = "Turn off or on the animated menu title"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Gui",
			n = "Toggle Gui",
			method = () =>
			{
				Mods.EnabledOrDisableGUI();
			},
			enabled = false,
			toolTip = "Turn off or on the gui"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Gun Line",
			n = "Toggle Gun Line",
			method = () =>
			{
				Mods.ChangeGunLiner();
			},
			enabled = false,
			toolTip = "Turn off or on the gun liner"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Iron Monke Sounds",
			n = "Toggle Iron Monke Sounds",
			method = () =>
			{
				Mods.IronSounds();
			},
			enabled = false,
			toolTip = "Turn off or on the Iron Monke sound effects"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Menu Collision",
			n = "Toggle Menu Collision",
			method = () =>
			{
				Mods.Togglecoll();
			},
			enabled = false,
			toolTip = "Turn off or on the menu collision"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Search Button",
			n = "Toggle Search Button",
			method = () =>
			{
				Mods.ToggleSearch();
			},
			enabled = false,
			toolTip = "Turn off or on the Search button"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Home Button",
			n = "Toggle Home Button",
			method = () =>
			{
				Mods.ToggleHome();
			},
			enabled = false,
			toolTip = "Turn off or on the Home button"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Disconnect Button",
			n = "Toggle Disconnect Button",
			method = () =>
			{
				Mods.ToggleDis();
			},
			enabled = false,
			toolTip = "Turn off or on the Disconnect button"
		},
		new ButtonInfo
		{
			buttonText = "Toggle Join/Leave Notis",
			n = "Toggle Join/Leave Notis",
			method = () =>
			{
				Mods.ToggleJoin();
			},
			enabled = false,
			toolTip = "Turn off or on the join and leave notis"
		},
		new ButtonInfo
		{
			buttonText = "Change Click Cooldown",
			n = "Change Click Cooldown",
			method = () =>
			{
				Mods.ChangeClickCool();
			},
			enabled = false,
			toolTip = "Change how fast you can click the menu buttons"
		},
		new ButtonInfo
		{
			buttonText = "Change Tracers Position",
			n = "Change Tracers Position",
			method = () =>
			{
				Mods.ChangeTracerPos();
			},
			enabled = false,
			toolTip = "Change the position tracers are at on your body"
		},
		new ButtonInfo
		{
			buttonText = "Change Anti Report Radius",
			n = "Change Anti Report Radius",
			method = () =>
			{
				Mods.ChangeAnti();
			},
			enabled = false,
			toolTip = "Change the radius of Anti Report"
		},
		new ButtonInfo
		{
			buttonText = "Change Aura Distance",
			n = "Change Aura Distance",
			method = () =>
			{
				Mods.ChangeTagAura();
			},
			enabled = false,
			toolTip = "Change the distance of aura for all Aura mods"
		},
		new ButtonInfo
		{
			buttonText = "Change Platform Keybind",
			n = "Change Platform Keybind",
			method = () =>
			{
				Mods.Changeplat();
			},
			enabled = false,
			toolTip = "Change how you use platform mods"
		},
		new ButtonInfo
		{
			buttonText = "Change ESP Color",
			n = "Change ESP Color",
			method = () =>
			{
				Mods.ChangeVisualColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Gun Hand",
			n = "Change Gun Hand",
			method = () =>
			{
				Mods.ChangeGunHand();
			},
			enabled = false,
			toolTip = "Change the hand you use for gun mods"
		},
		new ButtonInfo
		{
			buttonText = "Change Gun Pointer Size",
			n = "Change Gun Pointer Size",
			method = () =>
			{
				Mods.ChangePS();
			},
			enabled = false,
			toolTip = "Change the size of the pointer for gun mods"
		},
		new ButtonInfo
		{
			buttonText = "Change Gun Pointer Shape",
			n = "Change Gun Pointer Shape",
			method = () =>
			{
				Mods.ChangePointerShape();
			},
			enabled = false,
			toolTip = "Change the shape of the pointer for gun mods"
		},
		new ButtonInfo
		{
			buttonText = "Change Gun Line Size",
			n = "Change Gun Line Size",
			method = () =>
			{
				Mods.ChangeLS();
			},
			enabled = false,
			toolTip = "Change the size of the line for gun mods"
		},
		new ButtonInfo
		{
			buttonText = "Change Gun Color",
			n = "Change Gun Color",
			method = () =>
			{
				Mods.ChangeOrbColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Noti Delay",
			n = "Change Noti Delay",
			method = () =>
			{
				Mods.ChangeNotiDelay();
			},
			enabled = false,
			toolTip = "Change how fast notis go away"
		},
		new ButtonInfo
		{
			buttonText = "Change Noti Size",
			n = "Change Noti Size",
			method = () =>
			{
				Mods.ChangeNotiSize();
			},
			enabled = false,
			toolTip = "Change the size of notis"
		},
		new ButtonInfo
		{
			buttonText = "Change Wall Walk Strength",
			n = "Change Wall Walk Strength",
			method = () =>
			{
				Mods.ChangeWallWalk();
			},
			enabled = false,
			toolTip = "Change the strength of wall walk"
		},
		new ButtonInfo
		{
			buttonText = "Change Wall Walk Hand",
			n = "Change Wall Walk Hand",
			method = () =>
			{
				Mods.ChangeWallHand();
			},
			enabled = false,
			toolTip = "Change the hand you use for wall walk"
		},
		new ButtonInfo
		{
			buttonText = "Change GhostView Color",
			n = "Change GhostView Color",
			method = () =>
			{
				Mods.ChangeGVColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change GhostView Transparency",
			n = "Change GhostView Transparency",
			method = () =>
			{
				Mods.ChangeGVTrans();
			},
			enabled = false,
			toolTip = "Change the transparency of the GhostView"
		},
		new ButtonInfo
		{
			buttonText = "Change GhostView Type",
			n = "Change GhostView Type",
			method = () =>
			{
				Mods.ChangeGVType();
			},
			enabled = false,
			toolTip = "Change the type of the GhostView"
		},
		new ButtonInfo
		{
			buttonText = "Change Disconnect Location",
			n = "Change Disconnect Location",
			method = () =>
			{
				Mods.Changedisconnect();
			},
			enabled = false,
			toolTip = "Change the location of the disconnect button"
		},
		new ButtonInfo
		{
			buttonText = "Change Disconnect Color",
			n = "Change Disconnect Color",
			method = () =>
			{
				Mods.ChangeDisColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Disconnect Text Color",
			n = "Change Disconnect Text Color",
			method = () =>
			{
				Mods.ChangeDisTColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Next & Prev Location",
			n = "Change Next & Prev Location",
			method = () =>
			{
				Mods.Changepagebutton();
			},
			enabled = false,
			toolTip = "Change the location of the next and prev page buttons"
		},
		new ButtonInfo
		{
			buttonText = "Change Next & Prev Color",
			n = "Change Next & Prev Color",
			method = () =>
			{
				Mods.ChangeNPColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Next & Prev Text Color",
			n = "Change Next & Prev Text Color",
			method = () =>
			{
				Mods.ChangeNPTColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Next & Prev Arrow",
			n = "Change Next & Prev Arrow",
			method = () =>
			{
				Mods.ChangeArrow();
			},
			enabled = false,
			toolTip = "Change the arrows on the next and prev page buttons"
		},
		new ButtonInfo
		{
			buttonText = "Change Menu Location",
			n = "Change Menu Location",
			method = () =>
			{
				Mods.Changemenu();
			},
			enabled = false,
			toolTip = "Change the location of the menu"
		},
		new ButtonInfo
		{
			buttonText = "Change Menu Size",
			n = "Change Menu Size",
			method = () =>
			{
				Mods.ChangeMenuSize();
			},
			enabled = false,
			toolTip = "Change the size of the menu"
		},
		new ButtonInfo
		{
			buttonText = "Change Menu Title Color",
			n = "Change Menu Title Color",
			method = () =>
			{
				Mods.ChangeTitleColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Menu 1st Color",
			n = "Change Menu 1st Color",
			method = () =>
			{
				Mods.ThemeChangerV1(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Menu 2nd Color",
			n = "Change Menu 2nd Color",
			method = () =>
			{
				Mods.ThemeChangerV2(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Disabled Button Color",
			n = "Change Disabled Button Color",
			method = () =>
			{
				Mods.ThemeChangerV3(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Enabled Button Color",
			n = "Change Enabled Button Color",
			method = () =>
			{
				Mods.ThemeChangerV4(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Enabled Text Color",
			n = "Change Enabled Text Color",
			method = () =>
			{
				Mods.ThemeChangerV5(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Disabled Text Color",
			n = "Change Disabled Text Color",
			method = () =>
			{
				Mods.ThemeChangerV6(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Button Sound",
			n = "Change Button Sound",
			method = () =>
			{
				Mods.ThemeChangerV7();
			},
			enabled = false,
			toolTip = "Change the sound of buttons"
		},
		new ButtonInfo
		{
			buttonText = "Change Menu Font",
			n = "Change Menu Font",
			method = () =>
			{
				Mods.ChangeFont();
			},
			enabled = false,
			toolTip = "Change the font of the menu"
		},
		new ButtonInfo
		{
			buttonText = "Change Home Button Color",
			n = "Change Home Button Color",
			method = () =>
			{
				Mods.ChangeHomeBColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Home Button Text Color",
			n = "Change Home Button Text Color",
			method = () =>
			{
				Mods.ChangeHomeTColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Button Color",
			n = "Change Search Button Color",
			method = () =>
			{
				Mods.ChangeSearchBColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Button Text Color",
			n = "Change Search Button Text Color",
			method = () =>
			{
				Mods.ChangeSearchTColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Input Color",
			n = "Change Search Input Color",
			method = () =>
			{
				Mods.ChangeInputColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Keys Color",
			n = "Change Search Keys Color",
			method = () =>
			{
				Mods.ChangeKeyColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Keys Text Color",
			n = "Change Search Keys Text Color",
			method = () =>
			{
				Mods.ChangeKeyTColor(l: false);
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Change Search Keys Size",
			n = "Change Search Keys Size",
			method = () =>
			{
				Mods.ChangeKeySize();
			},
			enabled = false,
			toolTip = "Changes the size of the keys for the searching keyboard"
		},
		new ButtonInfo
		{
			buttonText = "Clear Search On Disable",
			n = "Clear Search On Disable",
			method = () =>
			{
				Mods.ClearDis();
			},
			enabled = false,
			toolTip = "If True, when you close search, your searched mods will disappear"
		},
		new ButtonInfo
		{
			buttonText = "Recenter While Searching",
			n = "Recenter While Searching",
			method = () =>
			{
				Mods.Recen();
			},
			enabled = false,
			toolTip = "Toggle the keyboard recentering itself if you move to far away from it"
		}
	};

	public static List<ButtonInfo> CatButtons1 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Enabled Mods",
			method = () =>
			{
				Mods.Cat1();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Save Mods",
			method = () =>
			{
				Mods.Save1();
			},
			enabled = false,
			toolTip = "Saves all enabled mods",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Disable All Mods",
			method = () =>
			{
				Mods.DisableAll();
			},
			enabled = false,
			toolTip = "Disables all of your enabled mods",
			n = "n"
		}
	};

	public static List<ButtonInfo> CatButtons2 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit OP Mods",
			method = () =>
			{
				Mods.Cat2();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Block Crash All (M)",
			method = () =>
			{
				Mods.Master("Block Crash All (M)", () =>
				{
					if (gripDownR)
					{
						Mods.DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
					}
				});
			},
			enabled = false,
			toolTip = "Crash everyone in monke blocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Crash All Except (M)",
			method = () =>
			{
				Mods.Master("Block Crash All Except (M)", () =>
				{
					Mods.MakeGun(l: true, () =>
					{
						Mods.DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to crash everyones game besides them"
		},
		new ButtonInfo
		{
			buttonText = "Block Crash Gun (M)",
			method = () =>
			{
				Mods.Master("Block Crash Gun (M)", () =>
				{
					Mods.MakeGun(l: true, () =>
					{
						Mods.DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone and crash their game"
		},
		new ButtonInfo
		{
			buttonText = "Block Crash Touch (M)",
			method = () =>
			{
				Mods.Master("Block Crash Touch (M)", () =>
				{
					foreach (VRRig r in RigUtils.AllRigs(i: false))
					{
						Mods.OnTouchMethod(r, o: false, () =>
						{
							Mods.lockedrig = r;
							Mods.DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
						});
					}
				});
			},
			enabled = false,
			toolTip = "Touch someone to crash them in monkeblocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Crash On Touch (M)",
			method = () =>
			{
				Mods.Master("Block Crash On Touch (M)", () =>
				{
					foreach (VRRig r in RigUtils.AllRigs(i: false))
					{
						Mods.OnTouchMethod(r, o: true, () =>
						{
							Mods.lockedrig = r;
							Mods.DropBlock("crash", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
						});
					}
				});
			},
			enabled = false,
			toolTip = "When someone touches you in monkeblocks, they will crash"
		},
		new ButtonInfo
		{
			buttonText = "Give Block Crash Touch (M)",
			method = () =>
			{
				Mods.Master("Give Block Crash Touch (M)", () =>
				{
					Mods.gc = true;
					Mods.Gi();
				});
			},
			disableMethod = () =>
			{
				if (Mods.gc)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gc = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to crash whoever they want in monkeblocks"
		},
		new ButtonInfo
		{
			buttonText = "Give Block Crash On Touch (M)",
			method = () =>
			{
				Mods.Master("Give Block Crash On Touch (M)", () =>
				{
					Mods.gc2 = true;
					Mods.Gi(5);
				});
			},
			disableMethod = () =>
			{
				if (Mods.gc2)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gc2 = false;
				}
			},
			enabled = false,
			toolTip = "Give the ability to crash someone when they touch the selected player in monkeblocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Trap Gun (M)",
			method = () =>
			{
				Mods.Master("Block Trap Gun (M)", () =>
				{
					Mods.BreakMove();
				});
			},
			enabled = false,
			toolTip = "Shoot someone to spawn ice around them"
		},
		new ButtonInfo
		{
			buttonText = "Create Public Lobby",
			method = () =>
			{
				Mods.CreateLobby();
			},
			enabled = false,
			toolTip = "Creates a public lobby",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Fling Rope Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.RopeLock();
					if ((Object)(object)Mods.lockedrope != (Object)null)
					{
						Mods.FlingRope(Mods.lockedrope, Mods.RandomV(50f));
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				}, "n", r: false);
			},
			enabled = false,
			toolTip = "Flings any rope you shoot in a random direction"
		},
		new ButtonInfo
		{
			buttonText = "Fling Rope Up Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.RopeLock();
					if ((Object)(object)Mods.lockedrope != (Object)null)
					{
						Mods.FlingRope(Mods.lockedrope, new Vector3(0.001f, 75f, 0.001f));
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				}, "n", r: false);
			},
			enabled = false,
			toolTip = "Flings any rope you shoot upwards"
		},
		new ButtonInfo
		{
			buttonText = "Freeze Rope Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.RopeLock();
					if ((Object)(object)Mods.lockedrope != (Object)null)
					{
						Mods.FlingRope(Mods.lockedrope, Vector3.zero);
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				}, "n", r: false);
			},
			enabled = false,
			toolTip = "Freezes any rope you shoot"
		},
		new ButtonInfo
		{
			buttonText = "Control Rope Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.RopeLock();
					if ((Object)(object)Mods.lockedrope != (Object)null)
					{
						Mods.FlingRope(Mods.lockedrope, Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up * 50f);
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				}, "n", r: false);
			},
			enabled = false,
			toolTip = "Flings any rope you shoot in the direction your hand is pointing"
		},
		new ButtonInfo
		{
			buttonText = "Fling Held Rope Up",
			method = () =>
			{
				if (triggerDownR && Mods.OwnRig().grabbedRopeIndex != -1)
				{
					GorillaRopeSwing val = Mods.Held();
					Mods.FlingRope(val, new Vector3(0f, 35f, 0f), val.GetBone(Mods.OwnRig().grabbedRopeBoneIndex), h: true);
				}
			},
			enabled = false,
			toolTip = "Flings the rope you are holding upwards"
		},
		new ButtonInfo
		{
			buttonText = "Fling Held Rope Forwards",
			method = () =>
			{
				if (triggerDownR && Mods.OwnRig().grabbedRopeIndex != -1)
				{
					GorillaRopeSwing val = Mods.Held();
					Vector3 forward = ((Component)Mods.Loco().headCollider).transform.forward;
					Mods.FlingRope(val, forward.normalized * 30f, val.GetBone(Mods.OwnRig().grabbedRopeBoneIndex), h: true);
				}
			},
			enabled = false,
			toolTip = "Flings the rope you are holding in the direction your head is facing"
		},
		new ButtonInfo
		{
			buttonText = "Freeze Held Rope",
			method = () =>
			{
				if (triggerDownR && Mods.OwnRig().grabbedRopeIndex != -1)
				{
					GorillaRopeSwing val = Mods.Held();
					Mods.FlingRope(val, Vector3.zero, val.GetBone(Mods.OwnRig().grabbedRopeBoneIndex), h: true);
				}
			},
			enabled = false,
			toolTip = "Freezes the rope you are holding"
		},
		new ButtonInfo
		{
			buttonText = "Glider Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.Gl(Mods.pointer.transform.position + new Vector3(0f, 1.6f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and move all the gliders where you want"
		},
		new ButtonInfo
		{
			buttonText = "Glider Annoy Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Gl(((Component)Mods.lockedrig).transform.position + ((Component)Mods.lockedrig).transform.forward * 1f, s: true);
				});
			},
			enabled = false,
			toolTip = "Shoot someone and annoy them with gliders"
		},
		new ButtonInfo
		{
			buttonText = "Grab Gliders",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Gl(Mods.Loco().RightHand.controllerTransform.position + new Vector3(0f, 1.5f, 0f));
				}
				else if (gripDownL)
				{
					Mods.Gl(Mods.Loco().LeftHand.controllerTransform.position + new Vector3(0f, 1.5f, 0f));
				}
			},
			enabled = false,
			toolTip = "Grab all the gliders"
		},
		new ButtonInfo
		{
			buttonText = "Glider Orbit",
			method = () =>
			{
				Mods.Gl(new Mods.Delay().Orbit("glid", ((Component)Mods.OwnRig()).transform.position, 3.5f, 0f, 3.5f));
			},
			enabled = false,
			toolTip = "Makes all the gliders orbit aroud you"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Gliders",
			method = () =>
			{
				Mods.Gl(Vector3.zero, s: true, c: false);
			},
			enabled = false,
			toolTip = "Spaz out all the gliders"
		},
		new ButtonInfo
		{
			buttonText = "Glider Minigun",
			method = () =>
			{
				Mods.GliderMinigun();
			},
			enabled = false,
			toolTip = "Shoot gliders like a minigun"
		},
		new ButtonInfo
		{
			buttonText = "Spawn Lava (M)",
			method = () =>
			{
				Mods.Lava((RisingLavaState)3, "Spawn");
			},
			enabled = false,
			toolTip = "Spawn the lava in the Lava map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Despawn Lava (M)",
			method = () =>
			{
				Mods.Lava((RisingLavaState)0, "Despawn");
			},
			enabled = false,
			toolTip = "Despawn the lava in the Lava map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Erupt Lava (M)",
			method = () =>
			{
				Mods.Lava((RisingLavaState)1, "Erupt");
			},
			enabled = false,
			toolTip = "Erupt the lava in the Lava map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Drain Lava (M)",
			method = () =>
			{
				Mods.Lava((RisingLavaState)4, "Drain");
			},
			enabled = false,
			toolTip = "Drain the lava in the Lava map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Break Tryon",
			method = () =>
			{
				if (!Mods.ruu)
				{
					Mods.ruu = true;
					CoroutineRunner.Start(Mods.q());
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Something Else Is Running, Please Wait Till Its Done!");
				}
				Mods.DisableButton("Break Tryon");
			},
			enabled = false,
			toolTip = "Breaks the try on stand and allows you to steal cosmetics, or wear multiple at the same time",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Fix Tryon",
			method = () =>
			{
				if (!Mods.ruu)
				{
					Mods.ruu = true;
					CoroutineRunner.Start(Mods.q(-1));
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Something Else Is Running, Please Wait Till Its Done!");
				}
				Mods.DisableButton("Fix Tryon");
			},
			enabled = false,
			toolTip = "Fixes the try on stand",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Get All Tryon Cosmetics",
			method = () =>
			{
				if (!Mods.ruu)
				{
					Mods.ruu = true;
					CoroutineRunner.Start(Mods.q(1));
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Something Else Is Running, Please Wait Till Its Done!");
				}
				Mods.DisableButton("Get All Tryon Cosmetics");
			},
			enabled = false,
			toolTip = "Equips every tryon cosmetic, all server sided"
		},
		new ButtonInfo
		{
			buttonText = "Remove All Tryon Cosmetics",
			method = () =>
			{
				if (!Mods.ruu)
				{
					Mods.ruu = true;
					CoroutineRunner.Start(Mods.q(2));
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Something Else Is Running, Please Wait Till Its Done!");
				}
				Mods.DisableButton("Remove All Tryon Cosmetics");
			},
			enabled = false,
			toolTip = "Remove all the try on cosmetics"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All",
			method = () =>
			{
				Mods.los = true;
				((Behaviour)Mods.OwnRig()).enabled = false;
				Mods.DrawHandOrbs();
				GorillaNetworkJoinTrigger[] array = Mods.F<GorillaNetworkJoinTrigger>(5f);
				((Component)Mods.OwnRig()).transform.position = ((Component)array[Random.Range(0, array.Length)]).transform.position;
			},
			disableMethod = () =>
			{
				if (Mods.los)
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
					Mods.los = false;
				}
			},
			enabled = false,
			toolTip = "Spam everyones UnityLog"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All V2 (M)",
			method = () =>
			{
				Mods.Log2();
			},
			enabled = false,
			toolTip = "Slowly spam everyones UnityLog with errors"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All V3",
			method = () =>
			{
				if (PhotonNetwork.InRoom)
				{
					LoadBalancingTransport client = ((VoiceConnection)PhotonVoiceNetwork.Instance).Client;
					object[] array = new object[3]
					{
						(byte)0,
						(byte)1,
						new object[1]
						{
							new Dictionary<byte, object>
							{
								{ 1, 0 },
								{ 2, 0 },
								{ 3, 0 },
								{ 4, "MALACHIS MENU ON TOP\ud83d\udd25" },
								{ 5, 0 }
							}
						}
					};
					RaiseEventOptions val = new RaiseEventOptions
					{
						Receivers = (ReceiverGroup)1
					};
					SendOptions val2 = default;
					val2.Reliability = false;
					val2.Channel = 0;
					((LoadBalancingClient)client).OpRaiseEvent((byte)202, (object)array, val, val2);
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be In A Room!");
					Mods.DisableButton("Log Spam All V3");
				}
			},
			enabled = false,
			toolTip = "Spam everyones UnityLog with errors"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All V4 (M)",
			method = () =>
			{
				if (Mods.Local().IsMasterClient)
				{
					if ((from e in RigUtils.AllRigs(i: false)
						where RigUtils.Map(e) == "monkeBlocks"
						select e).ToList().Count != 0)
					{
						Mods.DropBlock("log", Vector3.zero, Vector3.zero, Quaternion.identity, r: false);
					}
					else
					{
						Mods.SendNoti(Mods.Er + "Someone Must Be In MonkeBlocks!");
						Mods.DisableButton("Log Spam All V4 (M)");
					}
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be Master!");
					Mods.DisableButton("Log Spam All V4 (M)");
				}
			},
			enabled = false,
			toolTip = "Spam everyones UnityLog with errors if they are in MonkeBlocks"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All V5",
			method = () =>
			{
				Mods.Crr(((Component)Mods.OwnRig()).transform.position - new Vector3(0f, 10f, 0f), Vector3.zero, new Quaternion[1] { Quaternion.identity }, l: true, new int[1] { -69 });
			},
			enabled = false,
			toolTip = "Spam everyones UnityLog with errors"
		},
		new ButtonInfo
		{
			buttonText = "Log Spam All V6",
			method = () =>
			{
				Mods.Crr(((Component)Mods.OwnRig()).transform.position - new Vector3(0f, 10f, 0f), Vector3.zero, new Quaternion[1] { Quaternion.identity }, l: true, new int[1] { -70 });
			},
			enabled = false,
			toolTip = "Spam everyones UnityLog with errors"
		},
		new ButtonInfo
		{
			buttonText = "Mute All (SS)",
			method = () =>
			{
				Mods.MuteP(1);
			},
			enabled = false,
			toolTip = "Makes everyone not be able to hear anyone"
		},
		new ButtonInfo
		{
			buttonText = "Deafen All",
			method = () =>
			{
				Mods.MuteP(0);
			},
			enabled = false,
			toolTip = "Makes everyone except you not be able to hear anyone"
		},
		new ButtonInfo
		{
			buttonText = "Deafen Master",
			method = () =>
			{
				Mods.MuteP(2);
			},
			enabled = false,
			toolTip = "Makes the current master not be able to hear anyone"
		},
		new ButtonInfo
		{
			buttonText = "Break Guardian All (M)",
			n = "Break Guardian All (M)",
			method = () =>
			{
				RigUtils.AllRigs(i: false).ForEach((VRRig e) =>
				{
					Mods.Break(e.Creator.ActorNumber, o: true);
				});
				Mods.DisableButton("Break Guardian All (M)");
			},
			enabled = false,
			toolTip = "Breaks the guardian ball for everyone"
		},
		new ButtonInfo
		{
			buttonText = "Break Guardian Gun (M)",
			n = "Break Guardian Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(1f, () =>
					{
						Mods.Break(Mods.lockedrig.Creator.ActorNumber, o: true);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to break the guardian ball for them"
		},
		new ButtonInfo
		{
			buttonText = "Fix Guardian All (M)",
			n = "Fix Guardian All (M)",
			method = () =>
			{
				RigUtils.AllRigs(i: false).ForEach((VRRig e) =>
				{
					Mods.Break(e.Creator.ActorNumber, o: false);
				});
				Mods.DisableButton("Fix Guardian All (M)");
			},
			enabled = false,
			toolTip = "Fixes the guardian ball for everyone"
		},
		new ButtonInfo
		{
			buttonText = "Fix Guardian Gun (M)",
			n = "Fix Guardian Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(1f, () =>
					{
						Mods.Break(Mods.lockedrig.Creator.ActorNumber, o: false);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to fix the guardian ball for them"
		},
		new ButtonInfo
		{
			buttonText = "Kick All Near Computer",
			method = () =>
			{
				Mods.K();
			},
			enabled = false,
			toolTip = "Kicks everyone near a computer in a private lobby",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Destroy Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(1f, () =>
					{
						Mods.Des(Mods.lockedrig);
					});
				});
			},
			enabled = false,
			toolTip = "Any new players to the lobby wont see the person you shoot"
		},
		new ButtonInfo
		{
			buttonText = "Destroy All Players",
			method = () =>
			{
				RigUtils.AllRigs(i: false).ForEach((VRRig e) =>
				{
					Mods.Des(e);
				});
				Mods.DisableButton("Destroy All Players");
			},
			enabled = false,
			toolTip = "Any new players to the lobby will only see you"
		},
		new ButtonInfo
		{
			buttonText = "Break Critters (M)",
			method = () =>
			{
				if (Mods.Local().IsMasterClient)
				{
					Mods.DV(((NetworkView)CrittersManager.instance).GetView);
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be Master!");
				}
				Mods.DisableButton("Break Critters (M)");
			},
			enabled = false,
			toolTip = "Breaks critters equipment"
		},
		new ButtonInfo
		{
			buttonText = "Break Fossil Hunt (M)",
			method = () =>
			{
				if (Mods.Local().IsMasterClient)
				{
					Mods.DV((from e in Mods.F<PhotonView>(5f)
						where ((Object)e).name.Contains("VIMExperience3")
						select e).FirstOrDefault());
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be Master!");
				}
				Mods.DisableButton("Break Fossil Hunt (M)");
			},
			enabled = false,
			toolTip = "Makes fossils not spawn and unable to deposit fossils",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Max Fossils Collected (M)",
			method = () =>
			{
				Mods.Dep(int.MaxValue, "Max");
			},
			enabled = false,
			toolTip = "Sets the fossils collected count to max"
		},
		new ButtonInfo
		{
			buttonText = "Reset Fossils Collected (M)",
			method = () =>
			{
				Mods.Dep(0, "Reset");
			},
			enabled = false,
			toolTip = "Sets the fossils collected count to 0"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Fossils Collected (M)",
			method = () =>
			{
				Mods.Dep(Random.Range(0, int.MaxValue), "Spaz");
			},
			enabled = false,
			toolTip = "Spazs the fossils collected count"
		},
		new ButtonInfo
		{
			buttonText = "Add 1 Fossils Collected (M)",
			method = () =>
			{
				Mods.Dep(1, "Add 1");
			},
			enabled = false,
			toolTip = "Adds 1 to the fossils collected count"
		},
		new ButtonInfo
		{
			buttonText = "Remove 1 Fossils Collected (M)",
			method = () =>
			{
				Mods.Dep(1, "Remove 1");
			},
			enabled = false,
			toolTip = "Removes 1 from the fossils collected count"
		},
		new ButtonInfo
		{
			buttonText = "Set Fossils Collected (M)",
			method = () =>
			{
				Mods.Master("Set Fossils Collected (M)", () =>
				{
					Mods.SpawnBoard(new List<ButtonInfo>
					{
						new ButtonInfo
						{
							buttonText = "Confirm Set",
							method = () =>
							{
								if (Mods.typed.Length > 0)
								{
									int.TryParse(Mods.typed, out var result);
									if (Mods.typed.Length >= 10 && result == 0)
									{
										Mods.SendNoti(Mods.Er + "Value Is To High, Look At Tooltip!");
									}
									else
									{
										Mods.Dep(result, "Set");
										Mods.DeleteBoard();
									}
								}
								else
								{
									Mods.SendNoti(Mods.Er + "Please Enter A Number First!");
								}
								Mods.DisableButton("Confirm Set");
							},
							enabled = false
						}
					}, "numonly", 10, "Enter A Number And Press Confirm Set (max is 2147483647)|Set Fossils Collected (M)");
				});
			}
		},
		new ButtonInfo
		{
			buttonText = "Hit Target Gun (M)",
			method = () =>
			{
				Mods.TargetGun(6767);
			},
			enabled = false,
			toolTip = "Shoot a target to hit it"
		},
		new ButtonInfo
		{
			buttonText = "Hit All Targets (M)",
			method = () =>
			{
				LinqUtils.ForEach<HitTargetNetworkState>((IEnumerable<HitTargetNetworkState>)Mods.F<HitTargetNetworkState>(5f), (Action<HitTargetNetworkState>)((HitTargetNetworkState e) =>
				{
					Mods.SetTarget(e, 6767);
				}));
			},
			enabled = false,
			toolTip = "Hit all targets"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Target Gun (M)",
			method = () =>
			{
				Mods.TargetGun(Random.Range(0, 999));
			},
			enabled = false,
			toolTip = "Shoot a target to spaz it"
		},
		new ButtonInfo
		{
			buttonText = "Spaz All Targets (M)",
			method = () =>
			{
				LinqUtils.ForEach<HitTargetNetworkState>((IEnumerable<HitTargetNetworkState>)Mods.F<HitTargetNetworkState>(5f), (Action<HitTargetNetworkState>)((HitTargetNetworkState e) =>
				{
					Mods.SetTarget(e, Random.Range(0, 999));
				}));
			},
			enabled = false,
			toolTip = "Spaz all targets"
		},
		new ButtonInfo
		{
			buttonText = "Break Target Gun (M)",
			method = () =>
			{
				Mods.TargetGun(int.MinValue);
			},
			enabled = false,
			toolTip = "Shoot a target to break it"
		},
		new ButtonInfo
		{
			buttonText = "Break All Targets (M)",
			method = () =>
			{
				LinqUtils.ForEach<HitTargetNetworkState>((IEnumerable<HitTargetNetworkState>)Mods.F<HitTargetNetworkState>(5f), (Action<HitTargetNetworkState>)((HitTargetNetworkState e) =>
				{
					Mods.SetTarget(e, int.MinValue);
				}));
				Mods.DisableButton("Break All Targets (M)");
			},
			enabled = false,
			toolTip = "Break all targets"
		},
		new ButtonInfo
		{
			buttonText = "Reset Target Gun (M)",
			method = () =>
			{
				Mods.TargetGun(0);
			},
			enabled = false,
			toolTip = "Shoot a target to break it"
		},
		new ButtonInfo
		{
			buttonText = "Reset All Targets (M)",
			method = () =>
			{
				LinqUtils.ForEach<HitTargetNetworkState>((IEnumerable<HitTargetNetworkState>)Mods.F<HitTargetNetworkState>(5f), (Action<HitTargetNetworkState>)((HitTargetNetworkState e) =>
				{
					Mods.SetTarget(e, 0);
				}));
				Mods.DisableButton("Reset All Targets (M)");
			},
			enabled = false,
			toolTip = "Break all targets"
		},
		new ButtonInfo
		{
			buttonText = "Break Ranked Countdown (M)",
			method = () =>
			{
				Mods.Rank(1E+09f);
				Mods.DisableButton("Break Ranked Countdown (M)");
			},
			enabled = false,
			toolTip = "Make the ranked countdown have a huge number and never go down"
		},
		new ButtonInfo
		{
			buttonText = "Fix Ranked Countdown (M)",
			method = () =>
			{
				Mods.Rank(3f);
				Mods.DisableButton("Fix Ranked Countdown (M)");
			},
			enabled = false,
			toolTip = "Fix the ranked countdown"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Ranked Countdown (M)",
			method = () =>
			{
				new Mods.Delay().D(0.149f, () =>
				{
					Mods.Rank(Random.Range(3f, 1E+09f));
				});
			},
			enabled = false,
			toolTip = "Make the ranked countdown be a bunch of random numbers"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Ranked State (M)",
			method = () =>
			{
				new Mods.Delay().D(0.149f, () =>
				{
					Mods.Rank(0.001f);
				});
			},
			enabled = false,
			toolTip = "Spaz the state of ranked"
		},
		new ButtonInfo
		{
			buttonText = "Change Ranked State (M)",
			method = () =>
			{
				Mods.Rank(0.001f);
				Mods.DisableButton("Change Ranked State (M)");
			},
			enabled = false,
			toolTip = "Changes the state of ranked"
		},
		new ButtonInfo
		{
			buttonText = "Start Ranked (M)",
			method = () =>
			{
				Mods.Rank(-67f);
				Mods.DisableButton("Start Ranked (M)");
			},
			enabled = false,
			toolTip = "Starts ranked even if you only have 1-3 players"
		},
		new ButtonInfo
		{
			buttonText = "Spawn Blue Lucy (M)",
			method = () =>
			{
				Mods.Lucy(new bool[2] { true, false });
				Mods.DisableButton("Spawn Blue Lucy (M)");
			},
			enabled = false,
			toolTip = "Spawns blue lucy"
		},
		new ButtonInfo
		{
			buttonText = "Spawn Red Lucy (M)",
			method = () =>
			{
				Mods.Lucy(new bool[2] { true, true });
				Mods.DisableButton("Spawn Red Lucy (M)");
			},
			enabled = false,
			toolTip = "Spawns red lucy"
		},
		new ButtonInfo
		{
			buttonText = "Insta Spawn Blue Lucy (M)",
			method = () =>
			{
				Mods.Lucy(new bool[3] { true, false, true });
				Mods.DisableButton("Insta Spawn Blue Lucy (M)");
			},
			enabled = false,
			toolTip = "Instantly spawns blue lucy"
		},
		new ButtonInfo
		{
			buttonText = "Insta Spawn Red Lucy (M)",
			method = () =>
			{
				Mods.Lucy(new bool[3] { true, true, true });
				Mods.DisableButton("Insta Spawn Red Lucy (M)");
			},
			enabled = false,
			toolTip = "Instantly spawns red lucy"
		},
		new ButtonInfo
		{
			buttonText = "Despawn Lucy (M)",
			method = () =>
			{
				Mods.Lucy(new bool[1]);
				Mods.DisableButton("Despawn Lucy (M)");
			},
			enabled = false,
			toolTip = "Despawns lucy"
		},
		new ButtonInfo
		{
			buttonText = "Fast Lucy (M)",
			method = () =>
			{
				Mods.Lucy(11f);
				Mods.DisableButton("Fast Lucy (M)");
			},
			enabled = false,
			toolTip = "Makes lucys speed fast"
		},
		new ButtonInfo
		{
			buttonText = "Slow Lucy (M)",
			method = () =>
			{
				Mods.Lucy(1f);
				Mods.DisableButton("Slow Lucy (M)");
			},
			enabled = false,
			toolTip = "Makes lucys speed slow"
		},
		new ButtonInfo
		{
			buttonText = "Normal Lucy (M)",
			method = () =>
			{
				Mods.Lucy(2f);
				Mods.DisableButton("Normal Lucy (M)");
			},
			enabled = false,
			toolTip = "Makes lucys speed normal"
		},
		new ButtonInfo
		{
			buttonText = "Freeze Lucy (M)",
			method = () =>
			{
				Mods.Lucy(0f);
				Mods.DisableButton("Freeze Lucy (M)");
			},
			enabled = false,
			toolTip = "Freezes lucy in place"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Float Self (M)",
			method = () =>
			{
				Mods.Lucy(Mods.OwnRig());
			},
			enabled = false,
			toolTip = "Makes lucy grab you"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Float Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Lucy(Mods.lockedrig);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to make lucy grab them"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Float Touch (M)",
			method = () =>
			{
				foreach (VRRig r in RigUtils.AllRigs(i: false))
				{
					Mods.OnTouchMethod(r, o: false, () =>
					{
						Mods.Lucy(r);
					});
				}
			},
			enabled = false,
			toolTip = "Touch someone to make lucy grab them"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Float On Touch (M)",
			method = () =>
			{
				foreach (VRRig r in RigUtils.AllRigs(i: false))
				{
					Mods.OnTouchMethod(r, o: true, () =>
					{
						Mods.Lucy(r);
					});
				}
			},
			enabled = false,
			toolTip = "When someone touches you, lucy will grab them"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.Lucy(Mods.pointer.transform.position + new Vector3(0f, 0.5f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot to move lucy to where you are shooting"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Orbit (M)",
			method = () =>
			{
				Mods.Lucy(new Mods.Delay().Orbit("lucy", ((Component)Mods.OwnRig()).transform.position, 1.7f, 0f, 1.7f, 2.5f));
			},
			enabled = false,
			toolTip = "Makes lucy orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Target Self (M)",
			method = () =>
			{
				Mods.Lucy(Mods.OwnRig().Creator);
				Mods.DisableButton("Lucy Target Self (M)");
			},
			enabled = false,
			toolTip = "Makes lucy target you"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Target Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Lucy(Mods.lockedrig.Creator);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to make lucy target them"
		},
		new ButtonInfo
		{
			buttonText = "Lucy Target Spaz (M)",
			method = () =>
			{
				new Mods.Delay().D(0.05f, () =>
				{
					Mods.Lucy(RigUtils.AllNets()[Random.Range(0, RigUtils.AllNets().Count)]);
				});
			},
			enabled = false,
			toolTip = "Spaz out lucys target"
		},
		new ButtonInfo
		{
			buttonText = "Lurker Haunt Self (M)",
			method = () =>
			{
				Mods.Lurk(Mods.OwnRig().Creator);
				Mods.DisableButton("Lurker Haunt Self (M)");
			},
			enabled = false,
			toolTip = "Make the lurker haunt you"
		},
		new ButtonInfo
		{
			buttonText = "Lurker Haunt Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Lurk(Mods.lockedrig.Creator);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to make the lurker haunt them"
		},
		new ButtonInfo
		{
			buttonText = "Beach Score Spaz (M)",
			method = () =>
			{
				new Mods.Delay().D(0.1f, () =>
				{
					Mods.beachspaz++;
					if (Mods.beachspaz > 2)
					{
						Mods.beachspaz = 0;
					}
					Mods.Beach(Mods.beachspaz);
				});
			},
			enabled = false,
			toolTip = "Make the score board in beach spaz out"
		},
		new ButtonInfo
		{
			buttonText = "Beach Win Orange (M)",
			method = () =>
			{
				for (int i = 0; i < 3; i++)
				{
					Mods.Beach(1);
				}
				Mods.DisableButton("Beach Win Orange (M)");
			},
			enabled = false,
			toolTip = "Make orange team in beach win"
		},
		new ButtonInfo
		{
			buttonText = "Beach Win Blue (M)",
			method = () =>
			{
				for (int i = 0; i < 3; i++)
				{
					Mods.Beach();
				}
				Mods.DisableButton("Beach Win Blue (M)");
			},
			enabled = false,
			toolTip = "Make blue team in beach win"
		},
		new ButtonInfo
		{
			buttonText = "Beach Reset Score (M)",
			method = () =>
			{
				Mods.Beach(2);
				Mods.DisableButton("Beach Reset Score (M)");
			},
			enabled = false,
			toolTip = "Reset the scores in beach"
		},
		new ButtonInfo
		{
			buttonText = "Break Movement All",
			method = () =>
			{
				if (!Mods.ruuu)
				{
					Mods.ruuu = true;
					CoroutineRunner.Start(Mods.q(69));
				}
				Mods.DisableButton("Break Movement All");
			},
			enabled = false,
			toolTip = "Break everyones movement"
		},
		new ButtonInfo
		{
			buttonText = "Break Movement All Except",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (!Mods.ruuu)
					{
						Mods.ruuu = true;
						Mods.brevr = Mods.lockedrig;
						CoroutineRunner.Start(Mods.q(70));
					}
				});
			},
			enabled = false,
			toolTip = "Shoot someone to break everyones movement except theirs"
		},
		new ButtonInfo
		{
			buttonText = "Break Movement Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(3f, () =>
					{
						Mods.Breakm(Mods.lockedrig);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to break their movement"
		},
		new ButtonInfo
		{
			buttonText = "Break Movement Touch",
			method = () =>
			{
				foreach (VRRig r in RigUtils.AllRigs(i: false))
				{
					Mods.OnTouchMethod(r, o: false, () =>
					{
						new Mods.Delay().D(2f, () =>
						{
							Mods.Breakm(r);
						});
					});
				}
			},
			enabled = false,
			toolTip = "Touch someone to break their movement"
		},
		new ButtonInfo
		{
			buttonText = "Break Movement On Touch",
			method = () =>
			{
				foreach (VRRig r in RigUtils.AllRigs(i: false))
				{
					Mods.OnTouchMethod(r, o: true, () =>
					{
						new Mods.Delay().D(2f, () =>
						{
							Mods.Breakm(r);
						});
					});
				}
			},
			enabled = false,
			toolTip = "When someone touches you, their movement will break"
		},
		new ButtonInfo
		{
			buttonText = "Give Break Movement Touch",
			method = () =>
			{
				Mods.gb = true;
				Mods.Gi(6);
			},
			disableMethod = () =>
			{
				if (Mods.gb)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gb = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to break someones movement"
		},
		new ButtonInfo
		{
			buttonText = "Give Break Movement On Touch",
			method = () =>
			{
				Mods.gbm = true;
				Mods.Gi(7);
			},
			disableMethod = () =>
			{
				if (Mods.gbm)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gbm = false;
				}
			},
			enabled = false,
			toolTip = "Give the ability to break someones movement when they touch the selected player"
		},
		new ButtonInfo
		{
			buttonText = "Fix Movement All",
			method = () =>
			{
				Mods.DAC(1);
			},
			enabled = false,
			toolTip = "Fixes everyones movement"
		},
		new ButtonInfo
		{
			buttonText = "Break Audio All",
			method = () =>
			{
				Mods.Breakm(RigUtils.AllPlayers(), b: true);
				Mods.DisableButton("Break Audio All");
			},
			enabled = false,
			toolTip = "Breaks everyones audio in ghostreactor"
		},
		new ButtonInfo
		{
			buttonText = "Break Audio Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(1f, () =>
					{
						Mods.Breakm(Mods.lockedrig, b: true);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to break their audio"
		},
		new ButtonInfo
		{
			buttonText = "Fix Audio All",
			method = () =>
			{
				Mods.DAC(2);
			},
			enabled = false,
			toolTip = "Fixes everyones audio"
		},
		new ButtonInfo
		{
			buttonText = "Crash All",
			method = () =>
			{
				if ((from e in RigUtils.AllRigs(i: false)
					where Mods.InGR(e)
					select e).ToList().Count == 0)
				{
					Mods.SendNoti(Mods.Er + "People Must Be In GhostReactor");
					Mods.DisableButton("Crash All");
				}
				else
				{
					if (!Mods.ruu)
					{
						Mods.ruu = true;
						Mods.crashac = -1;
						CoroutineRunner.Start(Mods.q(88));
					}
					Mods.DisableButton("Crash All");
				}
			},
			enabled = false,
			toolTip = "Crash everyones games"
		},
		new ButtonInfo
		{
			buttonText = "Crash Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (!Mods.ruu)
					{
						Mods.crashac = Mods.lockedrig.Creator.ActorNumber;
						CoroutineRunner.Start(Mods.q(88));
						Mods.ruu = true;
					}
				}, () =>
				{
					Mods.ruu = false;
				});
			},
			enabled = false,
			toolTip = "Shoot someone to crash them"
		},
		new ButtonInfo
		{
			buttonText = "Spawn Mines Lucy (M)",
			method = () =>
			{
				Mods.Skel(true);
				Mods.DisableButton("Spawn Mines Lucy (M)");
			},
			enabled = false,
			toolTip = "Spawn the lucy thats in mines"
		},
		new ButtonInfo
		{
			buttonText = "Despawn Mines Lucy (M)",
			method = () =>
			{
				Mods.Skel(false);
				Mods.DisableButton("Despawn Mines Lucy (M)");
			},
			enabled = false,
			toolTip = "Despawn the lucy thats in mines"
		},
		new ButtonInfo
		{
			buttonText = "App Quit Gun (M)",
			method = () =>
			{
				Mods.Qu();
			},
			enabled = false,
			toolTip = "Shoot someone to app quit them"
		},
		new ButtonInfo
		{
			buttonText = "Grab Monsters (M)",
			method = () =>
			{
				Mods.GrabMonsters();
			},
			enabled = false,
			toolTip = "Grab the basement monsters"
		},
		new ButtonInfo
		{
			buttonText = "Monsters Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Vector3 position = Mods.pointer.transform.position;
					Mods.MoveMonsters(new Vector3[4]
					{
						position + new Vector3(0f, 0.05f, 0f),
						position + new Vector3(0f, 0.1f, 0f),
						position + new Vector3(0f, 0.15f, 0f),
						position + new Vector3(0f, 0.2f, 0f)
					});
				});
			},
			enabled = false,
			toolTip = "Shoot to move the basement monsters"
		},
		new ButtonInfo
		{
			buttonText = "Monster Float Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Transform transform = ((Component)Mods.lockedrig).transform;
					Mods.MoveMonsters(new Vector3[4]
					{
						transform.position + transform.right * 0.07f + transform.up * -0.42f,
						transform.position + transform.right * -0.07f + transform.up * -0.42f,
						transform.position + transform.forward * 0.07f + transform.up * -0.42f,
						transform.position + transform.forward * -0.07f + transform.up * -0.42f
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to float them with monsters"
		},
		new ButtonInfo
		{
			buttonText = "Monster Orbit (M)",
			method = () =>
			{
				Mods.MonsterOrb();
			},
			enabled = false,
			toolTip = "Make the monsters orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Monster Break Movement Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.MonThing(Mods.lockedrig, b: true);
				});
			},
			enabled = false,
			toolTip = "Shoot someone in the maze to break their movement"
		},
		new ButtonInfo
		{
			buttonText = "Drop Player Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.MonThing(Mods.lockedrig, b: false);
				});
			},
			enabled = false,
			toolTip = "Shoot someone in the maze to instantly drop them"
		},
		new ButtonInfo
		{
			buttonText = "Reset Monsters (M)",
			method = () =>
			{
				Mods.MoveMonsters((object)new Vector3(-375.5306f, -2.886f, 355.0327f));
				Mods.DisableButton("Reset Monsters (M)");
			},
			enabled = false,
			toolTip = "Resets the monsters positions"
		}
	};

	public static List<ButtonInfo> CatButtons3 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Rig Mods",
			method = () =>
			{
				Mods.Cat3();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Ghost Monke",
			method = () =>
			{
				Mods.Ghostmonke();
			},
			enabled = false,
			toolTip = "Ghost monke"
		},
		new ButtonInfo
		{
			buttonText = "Invis Monke",
			method = () =>
			{
				Mods.Invis();
			},
			enabled = false,
			toolTip = "Invisable monke"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Monke",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					new Mods.Delay().D(0.02f, () =>
					{
						((Component)Mods.OwnRig()).transform.position = Mods.BC().position + Mods.RandomV(0.4f);
					});
				}
				else
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				}
			},
			enabled = false,
			toolTip = "Spaz your rig"
		},
		new ButtonInfo
		{
			buttonText = "Wacky Monke",
			method = () =>
			{
				Vector3 eulerAngles = new Vector3((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
				Mods.OwnRig().head.rigTarget.eulerAngles = eulerAngles;
				Mods.OwnRig().leftHand.rigTarget.eulerAngles = eulerAngles;
				Mods.OwnRig().rightHand.rigTarget.eulerAngles = eulerAngles;
			},
			enabled = false,
			toolTip = "Spaz your rig but weird"
		},
		new ButtonInfo
		{
			buttonText = "Rig Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					((Component)Mods.OwnRig()).transform.position = Mods.pointer.transform.position + new Vector3(0f, 0.86f, 0f);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot and move your rig"
		},
		new ButtonInfo
		{
			buttonText = "Confuse Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					new Mods.Delay().D(0.02f, () =>
					{
						((Component)Mods.OwnRig()).transform.position = ((Component)Mods.lockedrig).transform.position + Mods.RandomV(0.5f);
					});
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and spaz your rig around them"
		},
		new ButtonInfo
		{
			buttonText = "Chase Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					Transform transform = ((Component)Mods.OwnRig()).transform;
					Vector3 position = ((Component)Mods.OwnRig()).transform.position;
					Vector3 val = ((Component)Mods.lockedrig).transform.position - ((Component)Mods.OwnRig()).transform.position;
					transform.position = position + val.normalized * (2.5f * Time.deltaTime);
					((Component)Mods.OwnRig()).transform.LookAt(((Component)Mods.lockedrig).transform.position);
					((Component)Mods.OwnRig().head.rigTarget).transform.rotation = ((Component)Mods.OwnRig()).transform.rotation;
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and make your rig chase them"
		},
		new ButtonInfo
		{
			buttonText = "Fast Chase Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					Transform transform = ((Component)Mods.OwnRig()).transform;
					Vector3 position = ((Component)Mods.OwnRig()).transform.position;
					Vector3 val = ((Component)Mods.lockedrig).transform.position - ((Component)Mods.OwnRig()).transform.position;
					transform.position = position + val.normalized * (12f * Time.deltaTime);
					((Component)Mods.OwnRig()).transform.LookAt(((Component)Mods.lockedrig).transform.position);
					((Component)Mods.OwnRig().head.rigTarget).transform.rotation = ((Component)Mods.OwnRig()).transform.rotation;
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and make your rig chase them"
		},
		new ButtonInfo
		{
			buttonText = "Orbit Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.DrawHandOrbs();
					((Behaviour)Mods.OwnRig()).enabled = false;
					((Component)Mods.OwnRig()).transform.position = new Mods.Delay().Orbit("play", ((Component)Mods.lockedrig).transform.position, 0.8f, 0f, 0.8f);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and orbit your rig around them"
		},
		new ButtonInfo
		{
			buttonText = "Grab Rig",
			method = () =>
			{
				Mods.GrabRig();
			},
			enabled = false,
			toolTip = "Grab your rig"
		},
		new ButtonInfo
		{
			buttonText = "Long Arms",
			method = () =>
			{
				Mods.longArms = true;
				Mods.Arm(1.09f);
			},
			disableMethod = () =>
			{
				if (Mods.longArms)
				{
					Mods.Arm(1f);
					Mods.longArms = false;
				}
			},
			enabled = false,
			toolTip = "Makes your arms long"
		},
		new ButtonInfo
		{
			buttonText = "Longer Arms",
			method = () =>
			{
				Mods.longgArms = true;
				Mods.Arm(1.2f);
			},
			disableMethod = () =>
			{
				if (Mods.longgArms)
				{
					Mods.Arm(1f);
					Mods.longgArms = false;
				}
			},
			enabled = false,
			toolTip = "Makes your arms longer"
		},
		new ButtonInfo
		{
			buttonText = "Super Long Arms",
			method = () =>
			{
				Mods.longggArms = true;
				Mods.Arm(1.6f);
			},
			disableMethod = () =>
			{
				if (Mods.longggArms)
				{
					Mods.Arm(1f);
					Mods.longggArms = false;
				}
			},
			enabled = false,
			toolTip = "Makes your arms super long"
		},
		new ButtonInfo
		{
			buttonText = "Fake Lag",
			method = () =>
			{
				Mods.fak = true;
				new Mods.Delay().D(0.7f, () =>
				{
					Mods.fake = !Mods.fake;
					((Behaviour)Mods.OwnRig()).enabled = Mods.fake;
				});
			},
			disableMethod = () =>
			{
				if (Mods.fak)
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
					Mods.fak = false;
				}
			},
			enabled = false,
			toolTip = "Makes it look like your lagging"
		},
		new ButtonInfo
		{
			buttonText = "Fast Fake Lag",
			method = () =>
			{
				Mods.fak2 = true;
				new Mods.Delay().D(0.35f, () =>
				{
					Mods.fake = !Mods.fake;
					((Behaviour)Mods.OwnRig()).enabled = Mods.fake;
				});
			},
			disableMethod = () =>
			{
				if (Mods.fak2)
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
					Mods.fak2 = false;
				}
			},
			enabled = false,
			toolTip = "Makes it look like your lagging"
		},
		new ButtonInfo
		{
			buttonText = "Fix Head",
			method = () =>
			{
				((Component)Mods.OwnRig().head.rigTarget).transform.rotation = ((Component)Mods.OwnRig()).transform.rotation;
				Mods.DisableButton("Fix Head");
			},
			enabled = false,
			toolTip = "Fixes your head",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Head Spin V1",
			method = () =>
			{
				Mods.Head1 = true;
				Mods.OwnRig().head.trackingRotationOffset.y += 11f;
			},
			disableMethod = () =>
			{
				if (Mods.Head1)
				{
					Mods.OwnRig().head.trackingRotationOffset.y = 0f;
					Mods.Head1 = false;
				}
			},
			enabled = false,
			toolTip = "Spins your head on the y axis"
		},
		new ButtonInfo
		{
			buttonText = "Head Spin V2",
			method = () =>
			{
				Mods.head2 = true;
				Mods.OwnRig().head.trackingRotationOffset.z += 11f;
			},
			disableMethod = () =>
			{
				if (Mods.head2)
				{
					Mods.OwnRig().head.trackingRotationOffset.z = 0f;
					Mods.head2 = false;
				}
			},
			enabled = false,
			toolTip = "Spins your head on the z axis"
		},
		new ButtonInfo
		{
			buttonText = "Head Spin V3",
			method = () =>
			{
				Mods.head3 = true;
				Mods.OwnRig().head.trackingRotationOffset.x += 11f;
			},
			disableMethod = () =>
			{
				if (Mods.head3)
				{
					Mods.OwnRig().head.trackingRotationOffset.x = 0f;
					Mods.head3 = false;
				}
			},
			enabled = false,
			toolTip = "Spins your head on the x axis"
		},
		new ButtonInfo
		{
			buttonText = "Fake Grey Screen (RJ)",
			method = () =>
			{
				Mods.OwnRig().head.trackingRotationOffset.y = (Mods.joy ? 180f : 0f);
				if (rjoy && !Mods.last4)
				{
					Mods.joy = !Mods.joy;
				}
				Mods.last4 = rjoy;
			},
			enabled = false,
			toolTip = "Makes your head flick backwards when you click your right joystick"
		},
		new ButtonInfo
		{
			buttonText = "Backwards Head",
			method = () =>
			{
				Mods.headback = true;
				Mods.OwnRig().head.trackingRotationOffset.y = 180f;
			},
			disableMethod = () =>
			{
				if (Mods.headback)
				{
					Mods.OwnRig().head.trackingRotationOffset.y = 0f;
					Mods.headback = false;
				}
			},
			enabled = false,
			toolTip = "Makes your head backwards"
		},
		new ButtonInfo
		{
			buttonText = "Backwards Body",
			method = () =>
			{
				RigPosePatch.backrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.backrig = false;
			},
			enabled = false,
			toolTip = "Makes your body backwards"
		},
		new ButtonInfo
		{
			buttonText = "Backwards Rig",
			method = () =>
			{
				RigPosePatch.fullbackrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.fullbackrig = false;
			},
			enabled = false,
			toolTip = "Makes your rig backwards"
		},
		new ButtonInfo
		{
			buttonText = "Upside Down Head",
			method = () =>
			{
				Mods.downhead = true;
				Mods.OwnRig().head.trackingRotationOffset.z = 180f;
			},
			disableMethod = () =>
			{
				if (Mods.downhead)
				{
					Mods.OwnRig().head.trackingRotationOffset.z = 0f;
					Mods.downhead = false;
				}
			},
			enabled = false,
			toolTip = "Makes your head upside down"
		},
		new ButtonInfo
		{
			buttonText = "Upside Down Body",
			method = () =>
			{
				RigPosePatch.upsiderig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.upsiderig = false;
			},
			enabled = false,
			toolTip = "Makes your body upside down"
		},
		new ButtonInfo
		{
			buttonText = "Upside Down Rig",
			method = () =>
			{
				RigPosePatch.fullupsiderig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.fullupsiderig = false;
			},
			enabled = false,
			toolTip = "Makes your rig upside down"
		},
		new ButtonInfo
		{
			buttonText = "Forwards Body",
			method = () =>
			{
				RigPosePatch.forwardrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.forwardrig = false;
			},
			enabled = false,
			toolTip = "Makes your body point forwards"
		},
		new ButtonInfo
		{
			buttonText = "Forwards Rig",
			method = () =>
			{
				RigPosePatch.forrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.forrig = false;
			},
			enabled = false,
			toolTip = "Makes your rig point forwards"
		},
		new ButtonInfo
		{
			buttonText = "Crawling Body",
			method = () =>
			{
				RigPosePatch.crawl = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.crawl = false;
			},
			enabled = false,
			toolTip = "Makes your body point backwards"
		},
		new ButtonInfo
		{
			buttonText = "Crawling Rig",
			method = () =>
			{
				RigPosePatch.crawlrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.crawlrig = false;
			},
			enabled = false,
			toolTip = "Makes your rig point backwards"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Head",
			method = () =>
			{
				Mods.OwnRig().head.rigTarget.eulerAngles = new Vector3((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
			},
			enabled = false,
			toolTip = "Makes your head spaz"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Body",
			method = () =>
			{
				RigPosePatch.spaz = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spaz = false;
			},
			enabled = false,
			toolTip = "Makes your body spaz"
		},
		new ButtonInfo
		{
			buttonText = "Spaz Rig",
			method = () =>
			{
				RigPosePatch.spazrig = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spazrig = false;
			},
			enabled = false,
			toolTip = "Makes your rig spaz"
		},
		new ButtonInfo
		{
			buttonText = "Fake Full Body Tracking",
			method = () =>
			{
				RigPosePatch.track = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.track = false;
			},
			enabled = false,
			toolTip = "Makes your body move like you have full body tracking"
		},
		new ButtonInfo
		{
			buttonText = "Spin Bot V1",
			method = () =>
			{
				RigPosePatch.spin = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spin = false;
			},
			enabled = false,
			toolTip = "Makes your body spin on the y axis"
		},
		new ButtonInfo
		{
			buttonText = "Spin Bot V2",
			method = () =>
			{
				RigPosePatch.spin2 = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spin2 = false;
			},
			enabled = false,
			toolTip = "Makes your body spin on the z axis"
		},
		new ButtonInfo
		{
			buttonText = "Spin Bot V3",
			method = () =>
			{
				RigPosePatch.spin3 = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spin3 = false;
			},
			enabled = false,
			toolTip = "Makes your body spin on the x axis"
		},
		new ButtonInfo
		{
			buttonText = "Spin Bot V4",
			method = () =>
			{
				RigPosePatch.spin4 = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spin4 = false;
			},
			enabled = false,
			toolTip = "Makes your body spin horizontally on your back"
		},
		new ButtonInfo
		{
			buttonText = "Spin Bot V5",
			method = () =>
			{
				RigPosePatch.spin5 = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.spin5 = false;
			},
			enabled = false,
			toolTip = "Makes your body spin horizontally on your front"
		},
		new ButtonInfo
		{
			buttonText = "Upside Down Spin Bot",
			method = () =>
			{
				RigPosePatch.upspin = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.upspin = false;
			},
			enabled = false,
			toolTip = "Makes your body spin but upside down"
		},
		new ButtonInfo
		{
			buttonText = "Full Spin Bot",
			method = () =>
			{
				RigPosePatch.fullspin = !RigPosePatch.fullspin;
				Mods.NewName("Full Spin Bot: " + RigPosePatch.fullspin);
			},
			enabled = false,
			toolTip = "Toggle if the Spin Bot mods spin your rig instead of your body",
			n = "Full Spin Bot"
		},
		new ButtonInfo
		{
			buttonText = "Backflip (RJ)",
			method = () =>
			{
				RigPosePatch.flip = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.flip = false;
			},
			enabled = false,
			toolTip = "Makes your body do a back flip when you click your right joystick"
		},
		new ButtonInfo
		{
			buttonText = "Frontflip (LJ)",
			method = () =>
			{
				RigPosePatch.fflip = true;
			},
			disableMethod = () =>
			{
				RigPosePatch.fflip = false;
			},
			enabled = false,
			toolTip = "Makes your body do a front flip when you click your left joystick"
		}
	};

	public static List<ButtonInfo> CatButtons4 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Movement Mods",
			method = () =>
			{
				Mods.Cat4();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Platforms",
			method = () =>
			{
				Mods.Platforms();
			},
			enabled = false,
			toolTip = "Platforms"
		},
		new ButtonInfo
		{
			buttonText = "Invis Platforms",
			method = () =>
			{
				Mods.Platforms(i: true);
			},
			enabled = false,
			toolTip = "Platforms but invisable"
		},
		new ButtonInfo
		{
			buttonText = "Sticky Platforms",
			method = () =>
			{
				Mods.Platforms(i: false, s: true);
			},
			enabled = false,
			toolTip = "Platforms but sticky"
		},
		new ButtonInfo
		{
			buttonText = "Sticky Invis Platforms",
			method = () =>
			{
				Mods.Platforms(i: true, s: true);
			},
			enabled = false,
			toolTip = "Platforms but sticky and invisable"
		},
		new ButtonInfo
		{
			buttonText = "NoClip",
			method = () =>
			{
				MeshCollider[] array = Mods.F<MeshCollider>(5f);
				foreach (MeshCollider val in array)
				{
					((Collider)val).enabled = !triggerDownR;
				}
			},
			enabled = false,
			toolTip = "Go through anything"
		},
		new ButtonInfo
		{
			buttonText = "Change Fly Direction",
			n = "Change Fly Direction",
			method = () =>
			{
				Mods.Changefly();
			},
			enabled = false,
			toolTip = "Change which part of your body controlls the fly mods"
		},
		new ButtonInfo
		{
			buttonText = "Fly",
			method = () =>
			{
				Mods.FlyMeth(11f);
			},
			enabled = false,
			toolTip = "Fly like a bird"
		},
		new ButtonInfo
		{
			buttonText = "Fast Fly",
			method = () =>
			{
				Mods.FlyMeth(20f);
			},
			enabled = false,
			toolTip = "Fly like a bird but fast"
		},
		new ButtonInfo
		{
			buttonText = "Really Fast Fly",
			method = () =>
			{
				Mods.FlyMeth(50f);
			},
			enabled = false,
			toolTip = "Fly like a bird but really fast"
		},
		new ButtonInfo
		{
			buttonText = "Slow Fly",
			method = () =>
			{
				Mods.FlyMeth(5f);
			},
			enabled = false,
			toolTip = "Fly like a bird but slow"
		},
		new ButtonInfo
		{
			buttonText = "Velocity Fly",
			method = () =>
			{
				Mods.FlyMeth(0f, new object[1] { Mods.Flyus * Time.deltaTime * 40f }, v: false);
			},
			enabled = false,
			toolTip = "Fly around but with lots of velocity"
		},
		new ButtonInfo
		{
			buttonText = "Super Monke",
			method = () =>
			{
				if (Mods.super)
				{
					Mods.Grav(0.935f);
				}
				Mods.FlyMeth(15f);
				if (bbuttonDown && !Mods.last3)
				{
					Mods.super = !Mods.super;
				}
				Mods.last3 = bbuttonDown;
			},
			enabled = false,
			toolTip = "Hold A to fly and press B to have no gravity"
		},
		new ButtonInfo
		{
			buttonText = "Iron Monke",
			method = () =>
			{
				Mods.IronMonke();
			},
			enabled = false,
			toolTip = "Fly like iron man"
		},
		new ButtonInfo
		{
			buttonText = "Fast Swim",
			method = () =>
			{
				if (Mods.Loco().InWater)
				{
					Rigidbody component = ((Component)Mods.Loco()).gameObject.GetComponent<Rigidbody>();
					component.linearVelocity *= 1.04f;
				}
			},
			enabled = false,
			toolTip = "Swim fast"
		},
		new ButtonInfo
		{
			buttonText = "Solid Water",
			method = () =>
			{
				Mods.solwa = true;
				LinqUtils.ForEach<WaterVolume>((IEnumerable<WaterVolume>)Mods.F<WaterVolume>(5f), (Action<WaterVolume>)((WaterVolume w) =>
				{
					((Component)w).gameObject.layer = LayerMask.NameToLayer("Default");
				}));
			},
			disableMethod = () =>
			{
				if (Mods.solwa)
				{
					LinqUtils.ForEach<WaterVolume>((IEnumerable<WaterVolume>)Mods.F<WaterVolume>(5f), (Action<WaterVolume>)((WaterVolume w) =>
					{
						((Component)w).gameObject.layer = LayerMask.NameToLayer("Water");
					}));
					Mods.solwa = false;
				}
			},
			enabled = false,
			toolTip = "Allows you to walk on water like its the ground"
		},
		new ButtonInfo
		{
			buttonText = "Frozone",
			method = () =>
			{
				Mods.Frozone();
			},
			enabled = false,
			toolTip = "Move around like frozone"
		},
		new ButtonInfo
		{
			buttonText = "Up & Down",
			method = () =>
			{
				Mods.UpnDown(((Component)Mods.Loco()).transform.up);
			},
			enabled = false,
			toolTip = "Right trigger to go up, left to go down"
		},
		new ButtonInfo
		{
			buttonText = "Right & Left",
			method = () =>
			{
				Mods.UpnDown(Mods.BC().right);
			},
			enabled = false,
			toolTip = "Right trigger to go right, left to go left"
		},
		new ButtonInfo
		{
			buttonText = "Forward & Backward",
			method = () =>
			{
				Mods.UpnDown(Mods.BC().forward);
			},
			enabled = false,
			toolTip = "Right trigger to go forward, left to go backward"
		},
		new ButtonInfo
		{
			buttonText = "Ride Player Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Loco().TeleportTo(Mods.W(((Component)Mods.lockedrig).transform.position) + new Vector3(0f, 1f, 0f), ((Component)Mods.Loco()).transform.rotation, false, false);
				});
			},
			enabled = false,
			toolTip = "Shoot and ride someone"
		},
		new ButtonInfo
		{
			buttonText = "TP Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					if (!Mods.tpg)
					{
						Mods.Loco().TeleportTo(Mods.W(Mods.pointer.transform.position + new Vector3(0f, 0.65f, 0f)), ((Component)Mods.Loco()).transform.rotation, false, false);
						Mods.tpg = true;
					}
				}, () =>
				{
					if (Mods.tpg)
					{
						Mods.tpg = false;
					}
				});
			},
			enabled = false,
			toolTip = "Shoot and teleport to where you shoot"
		},
		new ButtonInfo
		{
			buttonText = "TP To Random Player",
			method = () =>
			{
				if (RigUtils.AllPlayers().ToArray().Length > 1)
				{
					Mods.Loco().TeleportTo(Mods.W(((Component)RigUtils.RigFromPlayer(RigUtils.AllPlayers(i: false)[Random.Range(0, RigUtils.AllPlayers(i: false).ToArray().Length)])).transform.position + new Vector3(0f, 0.3f, 0f)), Quaternion.identity, false, false);
				}
				Mods.DisableButton("TP To Random Player");
			},
			enabled = false,
			toolTip = "Teleports you to a random player"
		},
		new ButtonInfo
		{
			buttonText = "TP To Maps",
			method = () =>
			{
				Mods.TPMaps();
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Anti Planet Pull",
			n = "Anti Planet Pull",
			method = () =>
			{
				Mods.antigrav = !Mods.antigrav;
				LinqUtils.ForEach<BasicGravityZone>((IEnumerable<BasicGravityZone>)Mods.F<BasicGravityZone>(5f), (Action<BasicGravityZone>)((BasicGravityZone e) =>
				{
					((Behaviour)e).enabled = !Mods.antigrav;
				}));
				Mods.NewName("Anti Planet Pull: " + Mods.antigrav);
			},
			enabled = false,
			toolTip = "Allows you to not be pulled by planets in the space map"
		},
		new ButtonInfo
		{
			buttonText = "Set Alarm Clock",
			method = () =>
			{
				Mods.SpawnPoint();
			},
			enabled = false,
			toolTip = "Sets your spawn points with the different alarm clocks",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Wall Walk",
			method = () =>
			{
				Mods.WallWalk();
			},
			enabled = false,
			toolTip = "Hold right grip and walk on walls"
		},
		new ButtonInfo
		{
			buttonText = "Mosa Speed",
			method = () =>
			{
				Mods.Speed(7.55f);
			},
			enabled = false,
			toolTip = "Get a slight speed boost (7.55)"
		},
		new ButtonInfo
		{
			buttonText = "Malachi Speed",
			method = () =>
			{
				Mods.Speed(7.903f);
			},
			enabled = false,
			toolTip = "Get a good speed boost (7.903)"
		},
		new ButtonInfo
		{
			buttonText = "TTT Speed",
			method = () =>
			{
				Mods.Speed(8.9f);
			},
			enabled = false,
			toolTip = "Get a great speed boost (9)"
		},
		new ButtonInfo
		{
			buttonText = "Speed Boost",
			method = () =>
			{
				Mods.Speed(16f);
			},
			enabled = false,
			toolTip = "Get a huge speed boost (16/max)"
		},
		new ButtonInfo
		{
			buttonText = "Grip Speed Boost",
			method = () =>
			{
				if (gripDownL)
				{
					Mods.Speed(16f);
				}
			},
			enabled = false,
			toolTip = "Hold left grip and get a huge speed boost"
		},
		new ButtonInfo
		{
			buttonText = "Custom Speed Boost Changer",
			method = () =>
			{
				Mods.ChangeSpeed();
			},
			enabled = false,
			toolTip = "Hold triggers to change how fast you go with the Custom Speed Boost mod"
		},
		new ButtonInfo
		{
			buttonText = "Custom Speed Boost",
			method = () =>
			{
				Mods.Speed(Mods.customspeed);
			},
			enabled = false,
			toolTip = "Get a speed boost depending on what you set the Custom Speed Boost Changer to"
		},
		new ButtonInfo
		{
			buttonText = "No Tag Freeze",
			method = () =>
			{
				Mods.Loco().disableMovement = false;
			},
			enabled = false,
			toolTip = "Be able to move when you get tagged"
		},
		new ButtonInfo
		{
			buttonText = "PBBV Walk",
			method = () =>
			{
				Mods.Speed(0f);
			},
			enabled = false,
			toolTip = "Limits your movement speed"
		},
		new ButtonInfo
		{
			buttonText = "No Gravity",
			method = () =>
			{
				Mods.Grav(0.935f);
			},
			enabled = false,
			toolTip = "Gives you no gravity"
		},
		new ButtonInfo
		{
			buttonText = "High Gravity",
			method = () =>
			{
				Mods.Grav(-0.8f);
			},
			enabled = false,
			toolTip = "Gives you high gravity"
		},
		new ButtonInfo
		{
			buttonText = "Low Gravity",
			method = () =>
			{
				Mods.Grav(0.65f);
			},
			enabled = false,
			toolTip = "Gives you low gravity"
		},
		new ButtonInfo
		{
			buttonText = "Punch Mod",
			method = () =>
			{
				Mods.PunchMod();
			},
			enabled = false,
			toolTip = "When someone punches you, you get knocked back"
		},
		new ButtonInfo
		{
			buttonText = "No Slippery Walls",
			method = () =>
			{
				Mods.NoSlipWalls();
			},
			disableMethod = () =>
			{
				Mods.OffNoSlipWalls();
			},
			enabled = false,
			toolTip = "Makes the slippery walls in forest non slippery"
		},
		new ButtonInfo
		{
			buttonText = "Slide Control",
			method = () =>
			{
				Mods.sli = true;
				Mods.sl = ((Mods.sl == 0f) ? Mods.Loco().slideControl : Mods.sl);
				Mods.Loco().slideControl = 1f;
			},
			disableMethod = () =>
			{
				if (Mods.sli)
				{
					Mods.Loco().slideControl = Mods.sl;
					Mods.sl = 0f;
					Mods.sli = false;
				}
			},
			enabled = false,
			toolTip = "Be able to have control over where youre sliding on ice"
		},
		new ButtonInfo
		{
			buttonText = "Spider Walk",
			method = () =>
			{
				Mods.Sw();
			},
			disableMethod = () =>
			{
				if (Mods.sww)
				{
					GTPlayerTransform.ResetRotationPositionOffset();
					Mods.Loco().UnsetGravityOverride((Object)(object)Mods.Loco());
					Mods.stt = Quaternion.identity;
					Mods.rr = Quaternion.identity;
					Mods.sww = false;
				}
			},
			enabled = false,
			toolTip = "Walk on anything and your gravity will change to face it"
		}
	};

	public static List<ButtonInfo> CatButtons5 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit SuperTag Mods",
			method = () =>
			{
				Mods.Cat5();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Break SuperGameMode (M)",
			method = () =>
			{
				if (Mods.Local().IsMasterClient)
				{
					Mods.DV(GameEntityManager.GetManagerForZone(Mods.OwnRig().zoneEntity.currentZone).photonView);
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be Master!");
				}
				Mods.DisableButton("Break SuperGameMode (M)");
			},
			enabled = false,
			toolTip = "Makes the terminals unusable, items wont spawn, and cant claim resources",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Unlock All Gadgets (M)(CS)",
			method = () =>
			{
				if (Mods.Local().IsMasterClient)
				{
					bool[][] unlockedTechTreeData = SIProgression.Instance.unlockedTechTreeData;
					foreach (bool[] array in unlockedTechTreeData)
					{
						for (int j = 0; j < array.Length; j++)
						{
							array[j] = true;
						}
					}
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be Master!");
				}
				Mods.DisableButton("Unlock All Gadgets (M)(CS)");
			},
			enabled = false,
			toolTip = "Unlock all gadgets"
		},
		new ButtonInfo
		{
			buttonText = "Disable Map Barriers (CS)",
			method = () =>
			{
				Mods.DisableBarr();
			},
			enabled = false,
			toolTip = "Removes the map barriers in SuperInfection",
			n = "Disable Map Barriers (CS)"
		},
		new ButtonInfo
		{
			buttonText = "Item ESP",
			method = () =>
			{
				Mods.CollectESP();
			},
			disableMethod = () =>
			{
				Mods.OffCollectESP();
			},
			enabled = false,
			toolTip = "Find items through walls"
		},
		new ButtonInfo
		{
			buttonText = "Change Item",
			method = () =>
			{
				Mods.ChangeItem();
			},
			enabled = false,
			n = "Change Item"
		},
		new ButtonInfo
		{
			buttonText = "Grab Item",
			method = () =>
			{
				Mods.GrabColl(Mods.collect);
			},
			enabled = false,
			toolTip = "Grab the chosen resource"
		},
		new ButtonInfo
		{
			buttonText = "Item Spammer",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Crr(Mods.Loco().RightHand.controllerTransform.position + new Vector3(0f, 0.15f, 0f), Vector3.zero, Quaternion.identity);
				}
			},
			enabled = false,
			toolTip = "Spam create the chosen resource"
		},
		new ButtonInfo
		{
			buttonText = "Item Minigun",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Crr(Mods.Loco().RightHand.controllerTransform.position, Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up * 15f, Quaternion.identity);
				}
			},
			enabled = false,
			toolTip = "Spam shoot the chosen resource"
		},
		new ButtonInfo
		{
			buttonText = "Item Orbit",
			method = () =>
			{
				if (gripDownR)
				{
					Vector3 val = new Mods.Delay().Orbit("res", ((Component)Mods.OwnRig()).transform.position, 0.9f, 0f, 0.9f);
					Mods.Crr(val, Vector3.zero, Quaternion.LookRotation(Vector3.up, ((Component)Mods.OwnRig()).transform.position - val));
				}
			},
			enabled = false,
			toolTip = "Spam the chosen resource around you"
		},
		new ButtonInfo
		{
			buttonText = "Item Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.Crr(Mods.rayCur.point + new Vector3(0f, 0.14f, 0f), Vector3.zero, Quaternion.identity, l: true);
				});
			},
			enabled = false,
			toolTip = "Shoot and spam the chosen resource"
		},
		new ButtonInfo
		{
			buttonText = "Item Sphere",
			method = () =>
			{
				Vector3 val = Mods.Sphere(((Component)Mods.OwnRig()).transform.position, 1.2f);
				Mods.Crr(val, Vector3.zero, Quaternion.LookRotation(((Component)Mods.OwnRig()).transform.position - val));
			},
			enabled = false,
			toolTip = "Spawn resources in a ball around you"
		},
		new ButtonInfo
		{
			buttonText = "Item Sphere Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					if (Mods.ogpp == Vector3.zero)
					{
						Mods.ogpp = Mods.rayCur.point + new Vector3(0f, 0.9f, 0f);
					}
					Vector3 val = Mods.Sphere(Mods.ogpp, 0.85f);
					Mods.Crr(val, Vector3.zero, Quaternion.LookRotation(Mods.ogpp - val), l: true);
				}, () =>
				{
					Mods.ogpp = Vector3.zero;
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn items in a sphere"
		},
		new ButtonInfo
		{
			buttonText = "Item Insta Sphere Gun",
			method = () =>
			{
				Mods.InstaSphere();
			},
			enabled = false,
			toolTip = "Shoot and instantly an item sphere"
		},
		new ButtonInfo
		{
			buttonText = "Item Ellipsoid Gun",
			method = () =>
			{
				Mods.ItemElli();
			},
			enabled = false,
			toolTip = "Shoot and make circles along each axis with items"
		},
		new ButtonInfo
		{
			buttonText = "Destroy Item Gun",
			method = () =>
			{
				Mods.DCr();
			},
			enabled = false,
			toolTip = "Shoot a item to destroy it"
		},
		new ButtonInfo
		{
			buttonText = "Destroy All Items",
			method = () =>
			{
				Mods.DAC();
			},
			enabled = false,
			toolTip = "Destroys all the items",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Complete All Quests",
			method = () =>
			{
				foreach (RotatingQuest quest in SIProgression.Instance.questSourceList.quests)
				{
					if (quest.isQuestActive)
					{
						quest.ApplySavedProgress(quest.requiredOccurenceCount);
					}
				}
				Mods.DisableButton("Complete All Quests");
			},
			enabled = false,
			toolTip = "Completes all of your quests",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Complete Bonus Points",
			method = () =>
			{
				for (int i = 0; i < 10; i++)
				{
					SIPlayer.LocalPlayer.GetBonusProgress(SuperInfectionManager.activeSuperInfectionManager);
				}
				Mods.DisableButton("Complete Bonus Points");
			},
			enabled = false,
			toolTip = "Instantly completes your bonus points progress",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Takeover All Terminals",
			method = () =>
			{
				Mods.ScAll();
			},
			enabled = false,
			toolTip = "Scans your hand on all of the gadget terminal"
		},
		new ButtonInfo
		{
			buttonText = "Infinite Thruster Fuel",
			method = () =>
			{
				SIGadgetWristJetOnUpdateAuthorityPatch.e = !SIGadgetWristJetOnUpdateAuthorityPatch.e;
				Mods.NewName("Infinite Thruster Fuel: " + SIGadgetWristJetOnUpdateAuthorityPatch.e);
			},
			enabled = false,
			toolTip = "Never run out of fuel for the thrusters",
			n = "Infinite Thruster Fuel"
		},
		new ButtonInfo
		{
			buttonText = "Change Thrusters Speed",
			method = () =>
			{
				yots++;
				if (yots > 3)
				{
					yots = 0;
				}
				float[] array = new float[4] { 0f, 25f, 50f, 6.5f };
				SIGadgetWristJetOnUpdateAuthorityPatch.PhotonNetworkRunViewUpdatePatch = array[yots];
				string[] array2 = new string[4] { "Normal", "Fast", "Super Fast", "Slow" };
				Mods.NewName("Thrusters Speed: " + array2[yots]);
			},
			enabled = false,
			toolTip = "Change how fast the thrusters will send you when used",
			n = "Change Thrusters Speed"
		},
		new ButtonInfo
		{
			buttonText = "Disable Yo-Yo Cooldown",
			method = () =>
			{
				SIGadgetDashYoyoOnUpdateAuthorityPatch.e = !SIGadgetDashYoyoOnUpdateAuthorityPatch.e;
				Mods.NewName("Disable Yo-Yo Cooldown: " + SIGadgetDashYoyoOnUpdateAuthorityPatch.e);
			},
			enabled = false,
			toolTip = "Be able to use the Yo-Yo without having a cooldown",
			n = "Disable Yo-Yo Cooldown"
		},
		new ButtonInfo
		{
			buttonText = "Change Yo-Yo Strength",
			method = () =>
			{
				yost++;
				if (yost > 2)
				{
					yost = 0;
				}
				string[] array = new string[3] { "Normal", "Strong", "Super Strong" };
				Mods.NewName("Yo-Yo Strength: " + array[yost]);
			},
			enabled = false,
			toolTip = "Change how far the Yo-Yo will launch you when used",
			n = "Change Yo-Yo Strength"
		},
		new ButtonInfo
		{
			buttonText = "Change Yo-Yo Distance",
			method = () =>
			{
				yofs++;
				if (yofs > 2)
				{
					yofs = 0;
				}
				string[] array = new string[3] { "Normal", "Far", "Super Far" };
				Mods.NewName("Yo-Yo Distance: " + array[yofs]);
			},
			enabled = false,
			toolTip = "Change how hard the Yo-Yo will get thrown when used",
			n = "Change Yo-Yo Distance"
		},
		new ButtonInfo
		{
			buttonText = "Infinite Platforms (CS)",
			method = () =>
			{
				SIGadgetPlatformDeployerOnUpdateAuthorityPatch.e = !SIGadgetPlatformDeployerOnUpdateAuthorityPatch.e;
				Mods.NewName("Infinite Platforms (CS): " + SIGadgetPlatformDeployerOnUpdateAuthorityPatch.e);
			},
			enabled = false,
			toolTip = "Never run out of charges for the platform spawner",
			n = "Infinite Platforms (CS)"
		},
		new ButtonInfo
		{
			buttonText = "Change Stilt Length (CS)",
			method = () =>
			{
				yola++;
				if (yola > 3)
				{
					yola = 0;
				}
				string[] array = new string[4] { "Normal", "Far", "Super Far", "Max" };
				Mods.NewName("Stilt Length (CS): " + array[yola]);
			},
			enabled = false,
			toolTip = "Change how far the stilts extend",
			n = "Change Stilt Length (CS)"
		},
		new ButtonInfo
		{
			buttonText = "Fast Stilt Extension",
			method = () =>
			{
				SIGadgetStiltOnUpdateAuthorityPatch.r = !SIGadgetStiltOnUpdateAuthorityPatch.r;
				Mods.NewName("Fast Stilt Extension: " + SIGadgetStiltOnUpdateAuthorityPatch.r);
			},
			enabled = false,
			toolTip = "Makes the extendo stilts extend faster",
			n = "Fast Stilt Extension"
		},
		new ButtonInfo
		{
			buttonText = "Fast Motorized Stilt",
			method = () =>
			{
				SIGadgetStiltOnUpdateAuthorityPatch.f = !SIGadgetStiltOnUpdateAuthorityPatch.f;
				Mods.NewName("Fast Motorized Stilt: " + SIGadgetStiltOnUpdateAuthorityPatch.f);
			},
			enabled = false,
			toolTip = "Makes the motorized stilt spin a lot faster",
			n = "Fast Motorized Stilt"
		},
		new ButtonInfo
		{
			buttonText = "Anti Blaster GTPlayerApplyKnockbackPatch",
			method = () =>
			{
				SIPlayerPlayerKnockbackPatch.e = !SIPlayerPlayerKnockbackPatch.e;
				Mods.NewName("Anti Blaster GTPlayerApplyKnockbackPatch: " + SIPlayerPlayerKnockbackPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so where no one can fling you around with the blaster",
			n = "Anti Blaster GTPlayerApplyKnockbackPatch"
		},
		new ButtonInfo
		{
			buttonText = "No Blaster Cooldown",
			method = () =>
			{
				SIGadgetChargeBlasterOnUpdateAuthorityPatch.e = !SIGadgetChargeBlasterOnUpdateAuthorityPatch.e;
				Mods.NewName("No Blaster Cooldown: " + SIGadgetChargeBlasterOnUpdateAuthorityPatch.e);
			},
			enabled = false,
			toolTip = "Makes the blasters have no cooldown",
			n = "No Blaster Cooldown"
		},
		new ButtonInfo
		{
			buttonText = "Move Player Forwards",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Move(((Component)Mods.lockedrig).transform.position, Mods.lockedrig.headMesh.transform.forward);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot a player to move them forwards with a blaster"
		},
		new ButtonInfo
		{
			buttonText = "Move Player Backwards",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Move(((Component)Mods.lockedrig).transform.position, -Mods.lockedrig.headMesh.transform.forward);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot a player to move them backwards with a blaster"
		},
		new ButtonInfo
		{
			buttonText = "Move Player Upwards",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Move(((Component)Mods.lockedrig).transform.position - new Vector3(0f, 0.25f, 0f), Vector3.up);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot a player to move them upwards with a blaster"
		},
		new ButtonInfo
		{
			buttonText = "Move Player Random",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Move(((Component)Mods.lockedrig).transform.position, Mods.RandomV(360f));
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot a player to move them randomly with a blaster"
		},
		new ButtonInfo
		{
			buttonText = "Move Player Controllable",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Move(((Component)Mods.lockedrig).transform.position, Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot a player to move them wherever your hand points"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Spam",
			method = () =>
			{
				Mods.BlasterSpam(Mods.Loco().RightHand.controllerTransform.position, Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up);
			},
			enabled = false,
			toolTip = "Spam the blaster projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Orbit",
			method = () =>
			{
				Mods.BlasterSpam(new Mods.Delay().Orbit("bl1", ((Component)Mods.OwnRig()).transform.position), Vector3.up);
			},
			enabled = false,
			toolTip = "Make the blaster shoot bullets up around you"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Orbit V2",
			method = () =>
			{
				Mods.BlasterSpam(new Mods.Delay().Orbit("bl2", ((Component)Mods.OwnRig()).transform.position, 1f, 0.5f), -Vector3.up);
			},
			enabled = false,
			toolTip = "Make the blaster shoot bullets down around you"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Rain",
			method = () =>
			{
				Mods.BlasterSpam(Mods.RainVector(3.5f), -Vector3.up);
			},
			enabled = false,
			toolTip = "Make it rain bullets"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Puke",
			method = () =>
			{
				Mods.BlasterSpam(((Component)Mods.GIn().headCollider).transform.position + ((Component)Mods.GIn().headCollider).transform.forward * 0.1f + ((Component)Mods.GIn().headCollider).transform.up * -0.125f, ((Component)Mods.GIn().headCollider).transform.rotation);
			},
			enabled = false,
			toolTip = "Puke out the blaster projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Piss",
			method = () =>
			{
				Mods.BlasterSpam(((Component)Mods.GIn().bodyCollider).transform.position + new Vector3(0f, -0.15f, 0f), ((Component)Mods.GIn().bodyCollider).transform.rotation);
			},
			enabled = false,
			toolTip = "Piss out the blaster projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Protector",
			method = () =>
			{
				List<VRRig> list = RigUtils.AllRigs(i: false).Where((VRRig e) =>
				{
					return Vector3.Distance(((Component)e).transform.position, ((Component)Mods.OwnRig()).transform.position) < 5f;
				}).ToList();
				if (list.Count > 0)
				{
					Mods.BlasterSpam(((Component)list[Random.Range(0, list.Count)]).transform.position - new Vector3(0f, 0.25f, 0f), Vector3.up);
				}
			},
			enabled = false,
			toolTip = "When someone comes near you it will shoot them upwards with the blaster projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Blaster Protector V2",
			method = () =>
			{
				List<VRRig> list = RigUtils.AllRigs(i: false).Where((VRRig e) =>
				{
					return Vector3.Distance(((Component)e).transform.position, ((Component)Mods.OwnRig()).transform.position) < 5f;
				}).ToList();
				if (list.Count > 0)
				{
					VRRig val = list[Random.Range(0, list.Count)];
					Mods.BlasterSpam(((Component)val).transform.position, -val.headMesh.transform.forward);
				}
			},
			enabled = false,
			toolTip = "When someone comes near you it will shoot them upwards with the blaster projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Infinite Tentacle Fuel",
			method = () =>
			{
				SIGadgetTentacleArmOnUpdateAuthorityPatch.e = !SIGadgetTentacleArmOnUpdateAuthorityPatch.e;
				Mods.NewName("Infinite Tentacle Fuel: " + SIGadgetTentacleArmOnUpdateAuthorityPatch.e);
			},
			enabled = false,
			toolTip = "Never run out of fuel for the tentacles",
			n = "Infinite Tentacle Fuel"
		},
		new ButtonInfo
		{
			buttonText = "Change Tentacle Length",
			method = () =>
			{
				yolaa++;
				if (yolaa > 4)
				{
					yolaa = 0;
				}
				string[] array = new string[5] { "Normal", "Long", "Super Long", "Very Long", "Max" };
				Mods.NewName("Change Tentacle Length: " + array[yolaa]);
			},
			enabled = false,
			toolTip = "Change how far the tentacles go",
			n = "Change Tentacle Length"
		},
		new ButtonInfo
		{
			buttonText = "Change Tentacle Speed",
			method = () =>
			{
				yolaaa++;
				if (yolaaa > 2)
				{
					yolaaa = 0;
				}
				string[] array = new string[3] { "Normal", "Fast", "Super Fast" };
				Mods.NewName("Change Tentacle Speed: " + array[yolaaa]);
			},
			enabled = false,
			toolTip = "Change how fast you move with the tentacles",
			n = "Change Tentacle Speed"
		},
		new ButtonInfo
		{
			buttonText = "Tentacle Size Changer",
			method = () =>
			{
				Mods.TentacleChanger();
			},
			enabled = false,
			toolTip = "Grip to grow, primary button to shrink",
			n = "Tentacle Size Changer"
		},
		new ButtonInfo
		{
			buttonText = "Infinite Zipline Uses",
			method = () =>
			{
				SIGadgetLaserZiplineOnUpdateAuthorityPatch.c = !SIGadgetLaserZiplineOnUpdateAuthorityPatch.c;
				Mods.NewName("Infinite Zipline Uses: " + SIGadgetLaserZiplineOnUpdateAuthorityPatch.c);
			},
			enabled = false,
			toolTip = "Gives you infinite uses on the Zipline gadget",
			n = "Infinite Zipline Uses"
		},
		new ButtonInfo
		{
			buttonText = "Fast Zipline",
			method = () =>
			{
				SIGadgetLaserZiplineOnUpdateAuthorityPatch.b = !SIGadgetLaserZiplineOnUpdateAuthorityPatch.b;
				Mods.NewName("Fast Zipline: " + SIGadgetLaserZiplineOnUpdateAuthorityPatch.b);
			},
			enabled = false,
			toolTip = "Makes the zipline gadget a lot faster",
			n = "Fast Zipline"
		},
		new ButtonInfo
		{
			buttonText = "Infinite AirGrab Uses",
			method = () =>
			{
				SIGadgetAirGrabFixedUpdatePatch.a = !SIGadgetAirGrabFixedUpdatePatch.a;
				Mods.NewName("Infinite AirGrab Uses: " + SIGadgetAirGrabFixedUpdatePatch.a);
			},
			enabled = false,
			toolTip = "Gives you infinite uses on the AirGrab gadget",
			n = "Infinite AirGrab Uses"
		},
		new ButtonInfo
		{
			buttonText = "No AirGrab Decay",
			method = () =>
			{
				SIGadgetAirGrabFixedUpdatePatch.b = !SIGadgetAirGrabFixedUpdatePatch.b;
				Mods.NewName("No AirGrab Decay: " + SIGadgetAirGrabFixedUpdatePatch.b);
			},
			enabled = false,
			toolTip = "Gives you unlimited time to be in the air with AirGrab",
			n = "No AirGrab Decay"
		},
		new ButtonInfo
		{
			buttonText = "Infinite AirJuke Uses",
			method = () =>
			{
				SIGadgetAirJukeFixedUpdatePatch.a = !SIGadgetAirJukeFixedUpdatePatch.a;
				Mods.NewName("Infinite AirJuke Uses: " + SIGadgetAirJukeFixedUpdatePatch.a);
			},
			enabled = false,
			toolTip = "Gives you infinite uses on the AirJuke gadget",
			n = "Infinite AirJuke Uses"
		},
		new ButtonInfo
		{
			buttonText = "Stronger AirJuke",
			method = () =>
			{
				SIGadgetAirJukeFixedUpdatePatch.b = !SIGadgetAirJukeFixedUpdatePatch.b;
				Mods.NewName("Stronger AirJuke: " + SIGadgetAirJukeFixedUpdatePatch.b);
			},
			enabled = false,
			toolTip = "Makes the AirJuke gadget a lot more stronger",
			n = "Stronger AirJuke"
		}
	};

	public static List<ButtonInfo> CatButtons6 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Block Mods",
			method = () =>
			{
				Mods.Cat6();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Make Block Spammers Float",
			method = () =>
			{
				Mods.floatblockb = !Mods.floatblockb;
				Mods.NewName("Make Block Spammers Float: " + Mods.floatblockb);
			},
			enabled = false,
			toolTip = "Makes the blocks you spawn float in place",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Change Block",
			method = () =>
			{
				Mods.ChangeBlock();
			},
			enabled = false,
			n = "Change Block"
		},
		new ButtonInfo
		{
			buttonText = "Block Selector",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					if ((Object)(object)((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<BuilderPiece>() != (Object)null && !Mods.blse)
					{
						string name = ((Object)((Component)((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<BuilderPiece>()).gameObject).name;
						if (!name.Contains("AtticFloor01") && !name.Contains("PrivatePlot"))
						{
							Mods.block = ((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<BuilderPiece>().pieceType.ToString();
							name = name.Substring(9).Split('(')[0].Trim();
							if (name.StartsWith("_"))
							{
								name = name.Remove(0, 1);
							}
							Mods.SendNoti("[<color=#00FFFF>BLOCK</color>] Selected Block: " + name);
							Mods.blse = true;
							Mods.NewName("Change Block: " + name);
						}
						else
						{
							Mods.blse = true;
						}
					}
				}, () =>
				{
					Mods.blse = false;
				});
			},
			enabled = false,
			toolTip = "Shoot a block to select it for block mods"
		},
		new ButtonInfo
		{
			buttonText = "Destroy Block (M)",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					if (Mods.Local().IsMasterClient)
					{
						if ((Object)(object)((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<BuilderPiece>() != (Object)null)
						{
							new Mods.Delay().D(0.0455f, () =>
							{
								Mods.bt().RequestRecyclePiece(((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<BuilderPiece>(), false, 2);
							});
						}
					}
					else
					{
						Mods.DelayedNoti(Mods.Er + "You Must Be Master To Use This!");
					}
				});
			},
			enabled = false,
			toolTip = "Shoot a block to destroy it"
		},
		new ButtonInfo
		{
			buttonText = "Block Spammer",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.DropBlock(Mods.block, Mods.OwnRig().rightHandTransform.position + new Vector3(0f, -0.15f, 0f), Vector3.zero, Quaternion.identity, Mods.floatblockb);
				}
			},
			enabled = false,
			toolTip = "Spam blocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Minigun",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.DropBlock(Mods.block, Mods.OwnRig().rightHandTransform.position + new Vector3(0f, -0.2f, 0f), Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up * 9f, Quaternion.identity, r: false);
				}
			},
			enabled = false,
			toolTip = "Shoot out blocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.DropBlock(Mods.block, Mods.pointer.transform.position + new Vector3(0f, 0.3f, 0f), Vector3.zero, Quaternion.identity, Mods.floatblockb);
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn blocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Rain",
			method = () =>
			{
				Mods.DropBlock(Mods.block, Mods.RainVector(2f), Vector3.zero, Quaternion.identity, Mods.floatblockb);
			},
			enabled = false,
			toolTip = "Make it rain blocks"
		},
		new ButtonInfo
		{
			buttonText = "Block Orbit",
			method = () =>
			{
				Vector3 val = new Mods.Delay().Orbit("block", ((Component)Mods.OwnRig()).transform.position);
				Mods.DropBlock(Mods.block, val, Vector3.zero, Quaternion.LookRotation(((Component)Mods.OwnRig()).transform.position - val), Mods.floatblockb);
			},
			enabled = false,
			toolTip = "Make blocks orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Block Sphere (M)",
			method = () =>
			{
				Mods.Master("Block Sphere (M)", () =>
				{
					Vector3 val = Mods.Sphere(((Component)Mods.OwnRig()).transform.position, 1f);
					Mods.DropBlock(Mods.block, val, Vector3.zero, Quaternion.LookRotation(((Component)Mods.OwnRig()).transform.position - val), r: true);
				});
			},
			enabled = false,
			toolTip = "Make blocks spawn in a sphere around you"
		},
		new ButtonInfo
		{
			buttonText = "Block Sphere Gun (M)",
			method = () =>
			{
				Mods.Master("Block Sphere Gun (M)", () =>
				{
					Mods.MakeGun(l: false, () =>
					{
						if (Mods.ogpp == Vector3.zero)
						{
							Mods.ogpp = Mods.rayCur.point + new Vector3(0f, 1f, 0f);
						}
						Vector3 val = Mods.Sphere(Mods.ogpp, 0.55f);
						Mods.DropBlock(Mods.block, val, Vector3.zero, Quaternion.LookRotation(Mods.ogpp - val), r: true);
					}, () =>
					{
						Mods.ogpp = Vector3.zero;
					});
				});
			},
			enabled = false,
			toolTip = "Shoot and make blocks spawn in a sphere"
		},
		new ButtonInfo
		{
			buttonText = "Block Igloo Gun (M)",
			method = () =>
			{
				Mods.Master("Block Igloo Gun (M)", () =>
				{
					Mods.BreakMove(i: false);
				});
			},
			enabled = false,
			toolTip = "Shoot to spawn an igloo"
		},
		new ButtonInfo
		{
			buttonText = "Block Ellipsoid Gun (M)",
			method = () =>
			{
				Mods.Elli();
			},
			enabled = false,
			toolTip = "Shoot and make circles along each axis with blocks"
		},
		new ButtonInfo
		{
			buttonText = "Suffocate Gun (M)",
			method = () =>
			{
				Mods.Master("Suffocate Gun (M)", () =>
				{
					Mods.MakeGun(l: true, () =>
					{
						Mods.DropBlock("-566818631", ((Component)Mods.lockedrig).transform.position, Vector3.zero, Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))), r: true, -1, Mods.lockedrig);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot and suffocate someone with blocks"
		},
		new ButtonInfo
		{
			buttonText = "Float Gun (M)",
			method = () =>
			{
				Mods.Master("Float Gun (M)", () =>
				{
					Mods.MakeGun(l: true, () =>
					{
						Mods.DropBlock("fling", ((Component)Mods.lockedrig).transform.position + Vector3.down * 0.345f, Vector3.up * 10f, Quaternion.Euler(0f, Random.Range(0f, 359f), 0f), r: false);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone and float them with blocks"
		},
		new ButtonInfo
		{
			buttonText = "Cover Private Plots (M)",
			method = () =>
			{
				Mods.Master("Cover Private Plots (M)", () =>
				{
					Mods.BlockObject(5);
				});
			},
			enabled = false,
			toolTip = "Block off all private plots"
		},
		new ButtonInfo
		{
			buttonText = "Cover Entrance (M)",
			method = () =>
			{
				Mods.Master("Cover Entrance (M)", () =>
				{
					Mods.BlockObject(0);
				});
			},
			enabled = false,
			toolTip = "Block off the exit so no one can leave"
		},
		new ButtonInfo
		{
			buttonText = "Cover Board (M)",
			method = () =>
			{
				Mods.Master("Cover Board (M)", () =>
				{
					Mods.BlockObject(1);
				});
			},
			enabled = false,
			toolTip = "Block off the board so no one can get access to it"
		},
		new ButtonInfo
		{
			buttonText = "Cover Computer (M)",
			method = () =>
			{
				Mods.Master("Cover Computer (M)", () =>
				{
					Mods.BlockObject(2);
				});
			},
			enabled = false,
			toolTip = "Block off the computer so no one can get access to it"
		},
		new ButtonInfo
		{
			buttonText = "Cover GameMode (M)",
			method = () =>
			{
				Mods.Master("Cover GameMode (M)", () =>
				{
					Mods.BlockObject(3);
				});
			},
			enabled = false,
			toolTip = "Block off the gamemode changer so no one can get access to it"
		},
		new ButtonInfo
		{
			buttonText = "Cover SizeChangers (M)",
			method = () =>
			{
				Mods.Master("Cover SizeChangers (M)", () =>
				{
					Mods.BlockObject(4);
				});
			},
			enabled = false,
			toolTip = "Block off the size changers so no one can get access to it"
		},
		new ButtonInfo
		{
			buttonText = "Cover Elevator (M)",
			method = () =>
			{
				Mods.Master("Cover Elevator (M)", () =>
				{
					Mods.BlockObject(6);
				});
			},
			enabled = false,
			toolTip = "Block off the elevator so no one can get access to it"
		},
		new ButtonInfo
		{
			buttonText = "Cover ShareMyBlocks (M)",
			method = () =>
			{
				Mods.Master("Cover ShareMyBlocks (M)", () =>
				{
					Mods.BlockObject(7);
				});
			},
			enabled = false,
			toolTip = "Block off the entrance to ShareMyBlocks"
		},
		new ButtonInfo
		{
			buttonText = "Cover All (M)",
			method = () =>
			{
				Mods.Master("Cover All (M)", () =>
				{
					Mods.BlockObject(Random.Range(0, 8));
				});
			},
			enabled = false,
			toolTip = "Block off everything so no one can get access to it"
		}
	};

	public static List<ButtonInfo> CatButtons7 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Spammer Mods",
			method = () =>
			{
				Mods.Cat7();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "HoverBoard Spammer",
			method = () =>
			{
				Mods.HoverBoardSpam(Vector3.down * 2f);
			},
			enabled = false,
			toolTip = "Makes you spam hoverboards"
		},
		new ButtonInfo
		{
			buttonText = "HoverBoard Minigun",
			method = () =>
			{
				Mods.HoverBoardSpam(Mods.Loco().RightHand.controllerTransform.forward - Mods.GIn().rightHandTransform.up * 50f);
			},
			enabled = false,
			toolTip = "Makes you shoot hoverboards"
		},
		new ButtonInfo
		{
			buttonText = "Randomize Cosmetics",
			method = () =>
			{
				Mods.RanCos();
			},
			enabled = false,
			toolTip = "Equips random cosmetics",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Change Cosmetic Outfit",
			n = "Change Cosmetic Outfit",
			method = () =>
			{
				Mods.fit++;
				if (Mods.fit > 9)
				{
					Mods.fit = 0;
				}
				Mods.Con().LoadSavedOutfit(Mods.fit);
				Mods.NewName("Change Cosmetic Outfit: " + (Mods.fit + 1));
			},
			enabled = false,
			toolTip = "Load your saved cosmetic outfits"
		},
		new ButtonInfo
		{
			buttonText = "Change Cosmetic Spam Category",
			n = "Change Cosmetic Spam Category",
			method = () =>
			{
				Mods.ChangeCosCat();
			},
			enabled = false,
			toolTip = "Changes the category of cosmetics you spam"
		},
		new ButtonInfo
		{
			buttonText = "Change Cosmetic Spam Delay",
			n = "Change Cosmetic Spam Delay",
			method = () =>
			{
				Mods.ChangeCosSpamDelay();
			},
			enabled = false,
			toolTip = "Changes how fast the cosmetics spam"
		},
		new ButtonInfo
		{
			buttonText = "Spam Cosmetics",
			method = () =>
			{
				Mods.SpamCosmetics();
			},
			enabled = false,
			toolTip = "Spams a bunch of different cosmetics from the category you selected"
		},
		new ButtonInfo
		{
			buttonText = "Change Current Projectile",
			method = () =>
			{
				Mods.ChangeProj();
			},
			enabled = false,
			n = "Change Current Projectile"
		},
		new ButtonInfo
		{
			buttonText = "Change Projectile Hand",
			n = "Change Projectile Hand",
			method = () =>
			{
				Mods.prr++;
				if (Mods.prr > 3)
				{
					Mods.prr = 0;
				}
				string[] array = new string[3] { "Right", "Left", "Both" };
				Mods.NewName("Change Projectile Hand: " + array[Mods.prr]);
			},
			enabled = false,
			toolTip = "Changes the hand you use for projectile mods"
		},
		new ButtonInfo
		{
			buttonText = "Projectile Spammer",
			method = () =>
			{
				if (Mods.ProjHand())
				{
					Mods.LaunchProj(Vector3.zero, Vector3.zero, Mods.curprojb);
				}
			},
			enabled = false,
			toolTip = "Spam projectiles from your hand"
		},
		new ButtonInfo
		{
			buttonText = "Projectile Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.LaunchProj(Mods.pointer.transform.position + new Vector3(0f, 0.1f, 0f), Vector3.zero, Mods.curprojb);
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot to spawn a projectile where you shot"
		},
		new ButtonInfo
		{
			buttonText = "Projectile MiniGun",
			method = () =>
			{
				if (Mods.ProjHand())
				{
					Mods.LaunchProj(Vector3.zero, Vector3.zero, Mods.curprojb, 25f);
				}
			},
			enabled = false,
			toolTip = "Shoot projectiles from your hand"
		},
		new ButtonInfo
		{
			buttonText = "Projectile Strong MiniGun",
			method = () =>
			{
				if (Mods.ProjHand())
				{
					Mods.LaunchProj(Vector3.zero, Vector3.zero, Mods.curprojb, 50f);
				}
			},
			enabled = false,
			toolTip = "Shoot projectiles from your hand super fast"
		},
		new ButtonInfo
		{
			buttonText = "Projectile Ultra Strong MiniGun",
			method = () =>
			{
				if (Mods.ProjHand())
				{
					Mods.LaunchProj(Vector3.zero, Vector3.zero, Mods.curprojb, 100f);
				}
			},
			enabled = false,
			toolTip = "Shoot projectiles from your hand ultra fast"
		},
		new ButtonInfo
		{
			buttonText = "Get Projectile",
			method = () =>
			{
				if (Mods.ProjHand())
				{
					new Mods.Delay().D(0.2f, () =>
					{
						Mods.GetSnow(Mods.curprojb);
					});
				}
			},
			enabled = false,
			toolTip = "Grab a projectile"
		},
		new ButtonInfo
		{
			buttonText = "Give Projectile Spammer",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (((VRMap)Mods.lockedrig.rightMiddle).calcT > 0.4f)
					{
						Mods.LaunchProj(Mods.lockedrig.rightHandTransform.position, Vector3.zero, Mods.curprojb);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT > 0.4f)
					{
						Mods.LaunchProj(Mods.lockedrig.leftHandTransform.position, Vector3.zero, Mods.curprojb);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT < 0.4f && ((VRMap)Mods.lockedrig.rightMiddle).calcT < 0.4f)
					{
						((Behaviour)Mods.OwnRig()).enabled = true;
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone then when they hold their grips they will spam projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Give Projectile MiniGun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (((VRMap)Mods.lockedrig.rightMiddle).calcT > 0.4f)
					{
						Mods.LaunchProj(Mods.lockedrig.rightHandTransform.position, -Mods.lockedrig.rightHandTransform.up, Mods.curprojb, 30f);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT > 0.4f)
					{
						Mods.LaunchProj(Mods.lockedrig.leftHandTransform.position, -Mods.lockedrig.leftHandTransform.up, Mods.curprojb, 30f);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT < 0.4f && ((VRMap)Mods.lockedrig.rightMiddle).calcT < 0.4f)
					{
						((Behaviour)Mods.OwnRig()).enabled = true;
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone then when they hold their grips they will shoot projectiles"
		},
		new ButtonInfo
		{
			buttonText = "Custom Sound Spam",
			method = () =>
			{
				Mods.CustomSpam();
			},
			disableMethod = () =>
			{
				Mods.sou = -1;
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Random Sound Spam",
			method = () =>
			{
				Mods.Sound(Random.Range(0, Mods.Loco().materialData.Count));
			},
			enabled = false,
			toolTip = "Spam random sounds"
		},
		new ButtonInfo
		{
			buttonText = "Jman Scream Spam",
			method = () =>
			{
				Mods.Sound(337);
			},
			enabled = false,
			toolTip = "Spam the Jmancurly scream sound"
		},
		new ButtonInfo
		{
			buttonText = "Jman \"Ok\" Spam",
			method = () =>
			{
				Mods.Sound(336);
			},
			enabled = false,
			toolTip = "Spam the Jmancurly \"Ok\" sound"
		},
		new ButtonInfo
		{
			buttonText = "Jman Slap Spam",
			method = () =>
			{
				Mods.Sound(338);
			},
			enabled = false,
			toolTip = "Spam the Jmancurly butt slap sound"
		},
		new ButtonInfo
		{
			buttonText = "Jman Random Spam",
			method = () =>
			{
				Mods.Sound(Random.Range(336, 338));
			},
			enabled = false,
			toolTip = "Spam random Jmancurly sounds"
		},
		new ButtonInfo
		{
			buttonText = "Schizo Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					new Mods.Delay().D(0.4f, () =>
					{
						Mods.GIn().myVRRig.SendRPC(Mods.RPC(3), NetPlayer.op_Implicit(RigUtils.PlayerFromRig(Mods.lockedrig)), new object[6]
						{
							((Component)Mods.OwnRig()).transform.position,
							Quaternion.LookRotation(Vector3.up),
							4f,
							100f,
							true,
							false
						});
					});
				});
			},
			enabled = false,
			toolTip = "Make the selected player think they have schizophrenia"
		},
		new ButtonInfo
		{
			buttonText = "Water Spam",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Water(Mods.Loco().RightHand.controllerTransform.position, Mods.Loco().RightHand.controllerTransform.rotation);
				}
				if (gripDownL)
				{
					Mods.Water(Mods.Loco().LeftHand.controllerTransform.position, Mods.Loco().LeftHand.controllerTransform.rotation);
				}
			},
			enabled = false,
			toolTip = "Spam water out of your hands"
		},
		new ButtonInfo
		{
			buttonText = "Water Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.Water(Mods.pointer.transform.position, Quaternion.LookRotation(Vector3.up));
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot and spam water"
		},
		new ButtonInfo
		{
			buttonText = "Water Orbit",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Water(new Mods.Delay().Orbit("water", ((Component)Mods.OwnRig()).transform.position), Quaternion.LookRotation(Vector3.up));
				}
			},
			enabled = false,
			toolTip = "Spam water around you"
		},
		new ButtonInfo
		{
			buttonText = "Shower",
			method = () =>
			{
				if (gripDownR)
				{
					Mods.Water(((Component)Mods.OwnRig()).transform.position + new Vector3(0f, 0.6f, 0f), Quaternion.LookRotation(Vector3.down));
				}
			},
			enabled = false,
			toolTip = "Take a shower"
		},
		new ButtonInfo
		{
			buttonText = "Give Water Spam",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (((VRMap)Mods.lockedrig.rightMiddle).calcT > 0.4f)
					{
						Mods.Water(Mods.lockedrig.rightHandTransform.position, Mods.lockedrig.rightHandTransform.rotation);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT > 0.4f)
					{
						Mods.Water(Mods.lockedrig.leftHandTransform.position, Mods.lockedrig.leftHandTransform.rotation);
					}
					if (((VRMap)Mods.lockedrig.leftMiddle).calcT < 0.4f && ((VRMap)Mods.lockedrig.rightMiddle).calcT < 0.4f)
					{
						((Behaviour)Mods.OwnRig()).enabled = true;
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone then when they hold their grips they will spam water"
		},
		new ButtonInfo
		{
			buttonText = "Give Water Orbit",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Water(new Mods.Delay().Orbit("wat", ((Component)Mods.lockedrig).transform.position), Quaternion.LookRotation(Vector3.up));
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and make water orbit around them"
		},
		new ButtonInfo
		{
			buttonText = "Give Shower",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.Water(((Component)Mods.lockedrig).transform.position + new Vector3(0f, 0.6f, 0f), Quaternion.LookRotation(Vector3.down));
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and give them a shower"
		},
		new ButtonInfo
		{
			buttonText = "Spawn Bracelet",
			method = () =>
			{
				Mods.Brac(e: true, 2);
				Mods.DisableButton("Spawn Bracelet");
			},
			enabled = false,
			toolTip = "Gives you the friendship bracelet",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Remove Bracelet",
			method = () =>
			{
				Mods.Brac(e: false, 2);
				Mods.DisableButton("Remove Bracelet");
			},
			enabled = false,
			toolTip = "Removes the friendship bracelet",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Spam Bracelet",
			method = () =>
			{
				if (gripDownR && !gripDownL)
				{
					new Mods.Delay().D(0.09f, () =>
					{
						Mods.bra = !Mods.bra;
						Mods.Brac(Mods.bra, 0);
					});
				}
				if (gripDownL && !gripDownR)
				{
					new Mods.Delay().D(0.09f, () =>
					{
						Mods.bra = !Mods.bra;
						Mods.Brac(Mods.bra, 1);
					});
				}
				if (gripDownR && gripDownL)
				{
					new Mods.Delay().D(0.09f, () =>
					{
						Mods.bra = !Mods.bra;
						Mods.Brac(Mods.bra, 2);
					});
				}
			},
			enabled = false,
			toolTip = "Spam the friendship bracelet"
		}
	};

	public static List<ButtonInfo> CatButtons8 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Visual Mods",
			method = () =>
			{
				Mods.Cat8();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "FPS Boost",
			method = () =>
			{
				Mods.FPSboost();
			},
			disableMethod = () =>
			{
				Mods.fixFPS();
			},
			enabled = false,
			toolTip = "Boost Your FPS"
		},
		new ButtonInfo
		{
			buttonText = "Tracers",
			method = () =>
			{
				Mods.Tracers();
			},
			disableMethod = () =>
			{
				if (Mods.TracerLines.Count > 0)
				{
					foreach (LineRenderer tracerLine in Mods.TracerLines)
					{
						Object.Destroy((Object)(object)tracerLine);
					}
					Mods.TracerLines.Clear();
				}
			},
			enabled = false,
			toolTip = "Lines towards everyone"
		},
		new ButtonInfo
		{
			buttonText = "Box ESP V1",
			method = () =>
			{
				Mods.BoxESP();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(0);
			},
			enabled = false,
			toolTip = "Make solid boxes on everyone"
		},
		new ButtonInfo
		{
			buttonText = "Box ESP V2",
			method = () =>
			{
				Mods.BoxESPV2();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(2);
			},
			enabled = false,
			toolTip = "Make hollow boxes around everyone"
		},
		new ButtonInfo
		{
			buttonText = "Box ESP V3",
			method = () =>
			{
				Mods.BoxESPV3();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(3);
			},
			enabled = false,
			toolTip = "Make corners around everyone"
		},
		new ButtonInfo
		{
			buttonText = "Ring ESP",
			method = () =>
			{
				Mods.RingESP();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(6);
			},
			enabled = false,
			toolTip = "Makes a vertical ring around everyone"
		},
		new ButtonInfo
		{
			buttonText = "Ellipsoid ESP",
			method = () =>
			{
				Mods.ElliESP();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(7);
			},
			enabled = false,
			toolTip = "Makes a ellipsoids around everyone"
		},
		new ButtonInfo
		{
			buttonText = "Sphere ESP",
			method = () =>
			{
				Mods.SphereESP();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(1);
			},
			enabled = false,
			toolTip = "Make spheres on everyone"
		},
		new ButtonInfo
		{
			buttonText = "Beacons",
			method = () =>
			{
				Mods.Beacons();
			},
			disableMethod = () =>
			{
				Mods.OffESPS(4);
			},
			enabled = false,
			toolTip = "Make poles on everyone"
		},
		new ButtonInfo
		{
			buttonText = "Chams",
			method = () =>
			{
				Mods.Chams();
			},
			disableMethod = () =>
			{
				Mods.OffChams();
			},
			enabled = false,
			toolTip = "Make everyone a certain color"
		},
		new ButtonInfo
		{
			buttonText = "Skelly ESP V1",
			method = () =>
			{
				Mods.SkellyESP();
			},
			disableMethod = () =>
			{
				if (Mods.skl)
				{
					foreach (LineRenderer line in Mods.lines)
					{
						Object.Destroy((Object)(object)line);
					}
					Mods.lines.Clear();
					Mods.skl = false;
				}
			},
			enabled = false,
			toolTip = "See everyones skeleton"
		},
		new ButtonInfo
		{
			buttonText = "Skelly ESP V2",
			method = () =>
			{
				if (!Mods.IsEnabled("Chams"))
				{
					Mods.SkellyV2();
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Cannot Use Chams And Skelly V2 At The Same Time!");
					Mods.DisableButton("Chams");
				}
			},
			disableMethod = () =>
			{
				Mods.OffSkellyV2();
			},
			enabled = false,
			toolTip = "See everyones actual skeleton"
		},
		new ButtonInfo
		{
			buttonText = "Platform Labels",
			method = () =>
			{
				Mods.PlatformLabel();
			},
			disableMethod = () =>
			{
				Mods.OffPlatform();
			},
			enabled = false,
			toolTip = "Tells you if someone is on standalone or pcvr"
		},
		new ButtonInfo
		{
			buttonText = "FPS Labels",
			method = () =>
			{
				Mods.PlatformLabel();
			},
			disableMethod = () =>
			{
				Mods.OffPlatform();
			},
			enabled = false,
			toolTip = "Tells you the persons current fps"
		},
		new ButtonInfo
		{
			buttonText = "Special Cosmetic Labels",
			method = () =>
			{
				Mods.PlatformLabel();
			},
			disableMethod = () =>
			{
				Mods.OffPlatform();
			},
			enabled = false,
			toolTip = "Tells you if someone has a special/rare cosmetic"
		},
		new ButtonInfo
		{
			buttonText = "NameTags",
			method = () =>
			{
				Mods.PlatformLabel();
			},
			disableMethod = () =>
			{
				Mods.OffPlatform();
			},
			enabled = false,
			toolTip = "Tells you the persons name"
		},
		new ButtonInfo
		{
			buttonText = "Wide Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone wide"
		},
		new ButtonInfo
		{
			buttonText = "Tall Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone tall"
		},
		new ButtonInfo
		{
			buttonText = "Short Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone short"
		},
		new ButtonInfo
		{
			buttonText = "Thin Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone thin"
		},
		new ButtonInfo
		{
			buttonText = "Long Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone long"
		},
		new ButtonInfo
		{
			buttonText = "Flat Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone flat"
		},
		new ButtonInfo
		{
			buttonText = "2D Monkes (CS)",
			method = () =>
			{
				Mods.Monk();
			},
			disableMethod = () =>
			{
				Mods.OffMonk();
			},
			enabled = false,
			toolTip = "Makes everyone 2D"
		},
		new ButtonInfo
		{
			buttonText = "GhostReactor Core ESP",
			method = () =>
			{
				Mods.CoreESP();
			},
			disableMethod = () =>
			{
				Mods.OffCoreESP();
			},
			enabled = false,
			toolTip = "Find cores for GhostReactor through walls"
		},
		new ButtonInfo
		{
			buttonText = "Change Time Of Day (CS)",
			method = () =>
			{
				Mods.ChangeDay();
			},
			enabled = false,
			toolTip = "Change the time of day",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "RGB (NC)",
			method = () =>
			{
				new Mods.Delay().D(0.075f, () =>
				{
					Mods.ChangeName("", Mods.Rainbow(), "RGB (NC)");
				});
			},
			enabled = false,
			toolTip = "Makes you rainbow"
		},
		new ButtonInfo
		{
			buttonText = "Normal Color Strobe (NC)",
			method = () =>
			{
				new Mods.Delay().D(0.075f, () =>
				{
					Color val = Color.white;
					Mods.st++;
					if (Mods.st > 9)
					{
						Mods.st = 1;
					}
					if (Mods.st == 1)
					{
						val = Color.blue;
					}
					if (Mods.st == 2)
					{
						val = Color.red;
					}
					if (Mods.st == 3)
					{
						val = Color.white;
					}
					if (Mods.st == 4)
					{
						val = Color.green;
					}
					if (Mods.st == 5)
					{
						val = Color.magenta;
					}
					if (Mods.st == 6)
					{
						val = Color.cyan;
					}
					if (Mods.st == 7)
					{
						val = Color.yellow;
					}
					if (Mods.st == 8)
					{
						val = Color.black;
					}
					if (Mods.st == 9)
					{
						val = Color.grey;
					}
					Mods.ChangeName("", val, "Normal Color Strobe (NC)");
				});
			},
			enabled = false,
			toolTip = "Makes you flash normal colors"
		},
		new ButtonInfo
		{
			buttonText = "Random Color Strobe (NC)",
			method = () =>
			{
				new Mods.Delay().D(0.075f, () =>
				{
					Mods.ChangeName("", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), "Random Color Strobe (NC)");
				});
			},
			enabled = false,
			toolTip = "Makes you flash random colors"
		},
		new ButtonInfo
		{
			buttonText = "Black & White Strobe (NC)",
			method = () =>
			{
				new Mods.Delay().D(0.075f, () =>
				{
					Color val = Color.white;
					Mods.st++;
					if (Mods.st > 2)
					{
						Mods.st = 1;
					}
					if (Mods.st == 1)
					{
						val = Color.white;
					}
					if (Mods.st == 2)
					{
						val = Color.black;
					}
					Mods.ChangeName("", val, "Black & White Strobe (NC)");
				});
			},
			enabled = false,
			toolTip = "Makes you flash black and white"
		},
		new ButtonInfo
		{
			buttonText = "Animate Name (NC)",
			method = () =>
			{
				Mods.AnimateName();
			},
			disableMethod = () =>
			{
				Mods.DisableAnimateName();
			},
			enabled = false,
			toolTip = "Types out your name, making it look animated"
		},
		new ButtonInfo
		{
			buttonText = "Random Color (NC)",
			method = () =>
			{
				Mods.ChangeName("", Mods.ranc(), "Random Color (NC)");
				Mods.DisableButton("Random Color (NC)");
			},
			enabled = false,
			toolTip = "Gives you a random color"
		},
		new ButtonInfo
		{
			buttonText = "Random Name (NC)",
			method = () =>
			{
				Mods.ChangeName(Mods.RandomString(), Color.clear, "Random Name (NC)");
				Mods.DisableButton("Random Name (NC)");
			},
			enabled = false,
			toolTip = "Gives you a random name"
		},
		new ButtonInfo
		{
			buttonText = "Random Name Spaz (NC)",
			method = () =>
			{
				new Mods.Delay().D(0.21f, () =>
				{
					Mods.ChangeName(Mods.RandomString(), Color.clear, "Random Name Spaz (NC)");
				});
			},
			enabled = false,
			toolTip = "Spazs your name to random things"
		},
		new ButtonInfo
		{
			buttonText = "Random Color & Name (NC)",
			method = () =>
			{
				Mods.ChangeName(Mods.RandomString(), Mods.ranc(), "Random Color & Name (NC)");
				Mods.DisableButton("Random Color & Name (NC)");
			},
			enabled = false,
			toolTip = "Gives you a random color & name"
		},
		new ButtonInfo
		{
			buttonText = "Become Gtag Ghosts (NC)",
			method = () =>
			{
				Mods.BecomeGhost();
			},
			enabled = false,
			toolTip = "Become different gtag ghosts"
		},
		new ButtonInfo
		{
			buttonText = "Change Name",
			method = () =>
			{
				Mods.SpawnBoard(new List<ButtonInfo>
				{
					new ButtonInfo
					{
						buttonText = "Confirm Name Change",
						method = () =>
						{
							if (Mods.typed.Length > 0)
							{
								Mods.ChangeName(Mods.typed, Mods.OwnRig().playerColor, "Confirm Name Change");
								Mods.DisableButton("Change Name");
								Mods.DeleteBoard();
							}
							else
							{
								Mods.SendNoti(Mods.Er + "Please Enter A Name First!");
							}
							Mods.DisableButton("Confirm Name Change");
						},
						enabled = false
					}
				}, "nospace", 12, "Type A Name Then Press Confirm Name Change|Change Name");
			}
		},
		new ButtonInfo
		{
			buttonText = "Change Color",
			method = () =>
			{
				Mods.SpawnBoard(new List<ButtonInfo>
				{
					new ButtonInfo
					{
						buttonText = "Confirm Color Change",
						method = () =>
						{
							if (Mods.typed.Length == 3)
							{
								Mods.ChangeName(Mods.Local().NickName, new Color(float.Parse(Mods.typed[0].ToString()) / 9f, float.Parse(Mods.typed[1].ToString()) / 9f, float.Parse(Mods.typed[2].ToString()) / 9f), "Confirm Color Change");
								Mods.DisableButton("Change Color");
								Mods.DeleteBoard();
							}
							else
							{
								Mods.SendNoti(Mods.Er + "Please Enter A 3 Digit Color Code!");
							}
							Mods.DisableButton("Confirm Color Change");
						},
						enabled = false
					}
				}, "numonly", 3, "Type A 3 Digit Color Code Then Press Confirm Color Change|Change Color");
			}
		},
		new ButtonInfo
		{
			buttonText = "Spoof Self (Become Players)",
			method = () =>
			{
				Mods.Spoof("(Become Players)");
			},
			enabled = false,
			toolTip = "For everyone, you have their name and color"
		},
		new ButtonInfo
		{
			buttonText = "Spoof Self (Random)",
			method = () =>
			{
				Mods.Spoof("(Random)");
			},
			enabled = false,
			toolTip = "For everyone, you have a random name and color"
		},
		new ButtonInfo
		{
			buttonText = "Spoof Self (Gtag Ghosts)",
			method = () =>
			{
				Mods.Spoof("(Gtag Ghosts)");
			},
			enabled = false,
			toolTip = "For everyone, you are random gtag ghosts"
		},
		new ButtonInfo
		{
			buttonText = "Spoof Self (Player IDS)",
			method = () =>
			{
				Mods.Spoof("(Player IDS)");
			},
			enabled = false,
			toolTip = "For everyone, your name is their player id"
		},
		new ButtonInfo
		{
			buttonText = "UnSpoof Self",
			method = () =>
			{
				Mods.Spoof("");
			},
			enabled = false,
			toolTip = "Resets your name and color for everyone"
		},
		new ButtonInfo
		{
			buttonText = "Spoof Cosmetics",
			method = () =>
			{
				Mods.SpoofCos(u: false);
			},
			enabled = false,
			toolTip = "For everyone else, you have random cosmetics (works best if you own a lot of cosmetics)"
		},
		new ButtonInfo
		{
			buttonText = "UnSpoof Cosmetics",
			method = () =>
			{
				Mods.SpoofCos(u: true);
			},
			enabled = false,
			toolTip = "Resets your cosmetics for everyone"
		},
		new ButtonInfo
		{
			buttonText = "Comp Label",
			method = () =>
			{
				Mods.CompLabel();
			},
			disableMethod = () =>
			{
				if ((Object)(object)Mods.label != (Object)null && Mods.ve1)
				{
					Object.Destroy((Object)(object)Mods.label);
					Mods.ve1 = false;
				}
			},
			enabled = false,
			toolTip = "Shows you a bunch of stuff useful for comp or ranked lobbys"
		},
		new ButtonInfo
		{
			buttonText = "Player Info Gun",
			method = () =>
			{
				Mods.InfoGun();
			},
			enabled = false,
			toolTip = "Gets the info about the player you shoot"
		},
		new ButtonInfo
		{
			buttonText = "Basic Mod Checker Gun",
			method = () =>
			{
				Mods.ModGun();
			},
			enabled = false,
			toolTip = "Gets the players custom properties and displays them on your hand"
		},
		new ButtonInfo
		{
			buttonText = "Show Network Triggers",
			method = () =>
			{
				Mods.ShowTriggers();
			},
			disableMethod = () =>
			{
				if (Mods.nett)
				{
					Object.Destroy((Object)(object)Mods.Find("ShowTriggers"));
					Mods.nett = false;
				}
			},
			enabled = false,
			toolTip = "Highlights all the network triggers"
		},
		new ButtonInfo
		{
			buttonText = "Show Anti Report Radius",
			method = () =>
			{
				Mods.ShowAntiReport();
			},
			disableMethod = () =>
			{
				if (Mods.ant)
				{
					Object.Destroy((Object)(object)Mods.Find("ShowAntiReport"));
					Mods.bs.Clear();
					Mods.ant = false;
				}
			},
			enabled = false,
			toolTip = "Highlights how far a player has to be near the report button for Anti Report to activate"
		},
		new ButtonInfo
		{
			buttonText = "Fix Shaders",
			method = () =>
			{
				ZoneShaderSettings.ActivateDefaultSettings();
				Mods.DisableButton("Fix Shaders");
			},
			enabled = false,
			toolTip = "Fixes your shaders if they're messed up",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Mat Spam Self (CS)",
			method = () =>
			{
				Mods.CSMat();
			},
			disableMethod = () =>
			{
				if ((Object)(object)Mods.ogmat != (Object)null)
				{
					((Renderer)Mods.OwnRig().mainSkin).material = Mods.ogmat;
					Mods.ogmat = null;
				}
			},
			enabled = false,
			toolTip = "Material spam yourself"
		}
	};

	public static List<ButtonInfo> CatButtons9 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Tag Mods",
			method = () =>
			{
				Mods.Cat9();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Tag Self",
			method = () =>
			{
				Mods.TagSelf();
			},
			enabled = false,
			toolTip = "Tag yourself",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Tag All",
			method = () =>
			{
				Mods.TagAll();
			},
			enabled = false,
			toolTip = "Tag everyone"
		},
		new ButtonInfo
		{
			buttonText = "Tag All Except",
			method = () =>
			{
				Mods.TagAllEx();
			},
			enabled = false,
			toolTip = "Shoot someone to tag everyone besides them"
		},
		new ButtonInfo
		{
			buttonText = "Tag Others (M)",
			method = () =>
			{
				foreach (VRRig item in from e in RigUtils.AllRigs(i: false)
					where !Mods.ActualTagged(e)
					select e)
				{
					if (Mods.Local().IsMasterClient)
					{
						if (Mods.IsGameMode((GameModeType)8))
						{
							Mods.DisableButton("Tag Others (M)");
						}
						else
						{
							Mods.Tagged(item.Creator);
						}
					}
					else
					{
						Mods.SendNoti(Mods.Er + "You Must Be Master To Use This!");
					}
				}
				Mods.DisableButton("Tag Others (M)");
			},
			enabled = false,
			toolTip = "Tag everyone besides you",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Tag Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (Mods.IsGameMode((GameModeType)3) ? Mods.CanHit(Mods.lockedrig) : (!Mods.ActualTagged(Mods.lockedrig)))
					{
						if (Mods.Local().IsMasterClient)
						{
							Mods.Tagged(Mods.lockedrig.Creator);
						}
						else if (!Mods.IsGameMode((GameModeType)3))
						{
							if (Mods.ActualTagged(Mods.OwnRig()) || ((GorillaHuntManager)GorillaGameManager.instance).GetTargetOf(NetworkSystem.Instance.LocalPlayer) == Mods.lockedrig.Creator)
							{
								Mods.SetSP();
								Mods.GIn().maxTagDistance = float.MaxValue;
								Mods.DrawHandOrbs();
								((Behaviour)Mods.OwnRig()).enabled = false;
								((Component)Mods.OwnRig()).transform.position = ((Component)Mods.lockedrig).transform.position + Mods.RandomV(0.05f);
								((Component)Mods.GIn().myVRRig).transform.position = ((Component)Mods.lockedrig).transform.position + Mods.RandomV(0.05f);
								if (Vector3.Distance(Mods.SP, ((Component)Mods.lockedrig).transform.position) < 5f)
								{
									new Mods.Delay().D(0.2f, () =>
									{
										GameMode.ReportTag(Mods.lockedrig.Creator);
										Mods.RPCFlush();
									});
								}
								Mods.GIn().StartVibration(Mods.change23 != 1, Mods.GIn().tapHapticStrength / 6f, Mods.GIn().tapHapticDuration);
							}
						}
						else
						{
							Mods.FixHit(Mods.lockedrig);
						}
					}
				}, () =>
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
				});
			},
			enabled = false,
			toolTip = "Shoot someone and tag them"
		},
		new ButtonInfo
		{
			buttonText = "Tag Aura",
			method = () =>
			{
				Mods.TagAura();
			},
			enabled = false,
			toolTip = "Hold grip and tag someone nearby"
		},
		new ButtonInfo
		{
			buttonText = "UnTag Self (M)",
			method = () =>
			{
				Mods.Master("UnTag Self (M)", () =>
				{
					Mods.UnTag(NetworkSystem.Instance.LocalPlayer);
					Mods.DisableButton("UnTag Self (M)");
				});
			},
			enabled = false,
			toolTip = "Untag yourself"
		},
		new ButtonInfo
		{
			buttonText = "UnTag All (M)",
			method = () =>
			{
				Mods.UnTagAll(0);
			},
			enabled = false,
			toolTip = "Untag everyone",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "UnTag Others (M)",
			method = () =>
			{
				Mods.Master("UnTag Others (M)", () =>
				{
					foreach (VRRig item2 in RigUtils.AllRigs(i: false))
					{
						Mods.UnTag(item2.Creator);
						Mods.DisableButton("UnTag Others (M)");
					}
				});
			},
			enabled = false,
			toolTip = "Untag everyone besides you",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "UnTag Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					if (!Mods.IsEnabled("Tag Gun"))
					{
						if (Mods.Local().IsMasterClient)
						{
							if (Mods.ActualTagged(Mods.lockedrig))
							{
								new Mods.Delay().D(0.4f, () =>
								{
									Mods.UnTag(Mods.lockedrig.Creator);
								});
							}
						}
						else
						{
							Mods.DelayedNoti(Mods.Er + "You Must Be Master!");
						}
					}
				});
			},
			enabled = false,
			toolTip = "Shoot someone to untag them"
		},
		new ButtonInfo
		{
			buttonText = "UnTag Aura (M)",
			method = () =>
			{
				Mods.Master("UnTag Aura (M)", () =>
				{
					if (gripDownR)
					{
						foreach (VRRig rig in RigUtils.AllRigs(i: false).Where((VRRig r) =>
						{
							return Vector3.Distance(((Component)r).transform.position, ((Component)Mods.OwnRig()).transform.position) < Mods.AURA;
						}).ToList())
						{
							new Mods.Delay().D(0.4f, () =>
							{
								Mods.UnTag(rig.Creator);
							});
						}
					}
				});
			},
			enabled = false,
			toolTip = "Hold grip and untag someone nearby"
		},
		new ButtonInfo
		{
			buttonText = "Mat Self (M)",
			method = () =>
			{
				Mods.Master("Mat Self (M)", () =>
				{
					Mods.Mat(NetworkSystem.Instance.LocalPlayer);
				});
			},
			enabled = false,
			toolTip = "Spam tag and untag yourself"
		},
		new ButtonInfo
		{
			buttonText = "Mat All V1 (M)",
			method = () =>
			{
				Mods.Master("Mat All V1 (M)", () =>
				{
					if (Mods.IsGameMode((GameModeType)8))
					{
						Mods.DisableButton("Mat All V1 (M)");
					}
					else if (triggerDownR)
					{
						if (Mods.IsGameMode((GameModeType)3))
						{
							Mods.PaintMat(Mods.OwnRig(), 0);
						}
						else
						{
							new Mods.Delay().D(Mods.matd, () =>
							{
								Mods.mat++;
								if (Mods.mat > 2)
								{
									Mods.mat = 1;
								}
								if (Mods.mat == 1)
								{
									Mods.TagAll();
								}
								if (Mods.mat == 2)
								{
									Mods.UnTagAll(69420);
								}
							});
						}
					}
				});
			},
			enabled = false,
			toolTip = "Spam tag and untag everyone"
		},
		new ButtonInfo
		{
			buttonText = "Mat All V2 (M)",
			method = () =>
			{
				Mods.Master("Mat All V2 (M)", () =>
				{
					if (triggerDownR)
					{
						new Mods.Delay().D(0.2f, () =>
						{
							NetPlayer val = RigUtils.AllNets(i: false)[Random.Range(0, RigUtils.AllNets(i: false).Count)];
							if (Mods.ActualTagged(RigUtils.RigFromPlayer(val.GetPlayerRef())))
							{
								Mods.UnTag(val);
							}
							else
							{
								Mods.Tagged(val);
							}
						});
					}
				});
			},
			enabled = false,
			toolTip = "Spam tag and untag everyone"
		},
		new ButtonInfo
		{
			buttonText = "Mat Gun (M)",
			method = () =>
			{
				Mods.Master("Mat Gun (M)", () =>
				{
					Mods.MakeGun(l: true, () =>
					{
						Mods.Mat(Mods.lockedrig.Creator);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot and spam tag and untag someone"
		},
		new ButtonInfo
		{
			buttonText = "Mat Aura (M)",
			method = () =>
			{
				Mods.Master("Mat Aura (M)", () =>
				{
					if (gripDownR)
					{
						foreach (VRRig item3 in RigUtils.AllRigs(i: false).Where((VRRig r) =>
						{
							return Vector3.Distance(((Component)r).transform.position, ((Component)Mods.OwnRig()).transform.position) < Mods.AURA;
						}).ToList())
						{
							Mods.Mat(item3.Creator);
						}
					}
				});
			},
			enabled = false,
			toolTip = "Hold grip and spam tag and untag everyone nearby"
		},
		new ButtonInfo
		{
			buttonText = "Give Mat All (M)",
			method = () =>
			{
				Mods.Master("Give Mat All (M)", () =>
				{
					if (Mods.IsGameMode((GameModeType)8))
					{
						Mods.SendNoti(Mods.Er + "You Cant Use This In Guardian!");
						Mods.DisableButton("Give Mat All (M)");
					}
					else
					{
						Mods.gm = true;
						Mods.Gi(4);
					}
				});
			},
			disableMethod = () =>
			{
				if (Mods.gm)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gm = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to spam tag and untag everyone"
		},
		new ButtonInfo
		{
			buttonText = "Flick Tag Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					if (!Object.op_Implicit((Object)(object)((Component)Mods.rayCur.ReflectionEnums).GetComponentInParent<VRRig>()) || RigUtils.AllPlayers().ToArray().Length <= 3)
					{
						Mods.GetGunHand().position = Mods.pointer.transform.position;
					}
				});
			},
			enabled = false,
			toolTip = "Shoot and flick tag someone"
		},
		new ButtonInfo
		{
			buttonText = "Get Guardian",
			method = () =>
			{
				Mods.Shit();
			},
			disableMethod = () =>
			{
				if (Mods.re)
				{
					((Behaviour)Mods.OwnRig()).enabled = true;
					Mods.re = false;
				}
			},
			enabled = false,
			toolTip = "Get guardian in the Guardian gamemode",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Undo Guardian",
			method = () =>
			{
				if (PhotonNetwork.InRoom)
				{
					if (Mods.IsGameMode((GameModeType)8))
					{
						if (Mods.ActualTagged(Mods.OwnRig()))
						{
							((GorillaGuardianManager)GorillaGameManager.instance).RequestEjectGuardian(Mods.OwnRig().Creator);
						}
						else
						{
							Mods.SendNoti(Mods.Er + "You Are Not Currently Guardian!");
						}
					}
					else
					{
						Mods.SendNoti(Mods.Er + "You Must Be In A Guardian GameMode!");
					}
				}
				Mods.DisableButton("Undo Guardian");
			},
			enabled = false,
			toolTip = "Unsets yourself as guardian",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Fling All",
			method = () =>
			{
				Mods.GuardMeth("Fling All", () =>
				{
					foreach (VRRig item4 in RigUtils.AllRigs(i: false))
					{
						Mods.Throw(item4, new Vector3(0f, 5555f, 0f));
					}
				});
			},
			enabled = false,
			toolTip = "Fling everyone up"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Fling Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.GuardMeth("Fling Gun", () =>
					{
						Mods.Throw(Mods.lockedrig, new Vector3(0f, 5555f, 0f));
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to fling them up"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Spaz All",
			method = () =>
			{
				Mods.GuardMeth("Spaz All", () =>
				{
					foreach (VRRig item5 in RigUtils.AllRigs(i: false))
					{
						Mods.Throw(item5, Mods.RandomV(1000f));
					}
				});
			},
			enabled = false,
			toolTip = "Spaz everyone around"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Spaz Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.GuardMeth("Spaz Gun", () =>
					{
						Mods.Throw(Mods.lockedrig, Mods.RandomV(1000f));
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to spaz them around"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Orbit All",
			method = () =>
			{
				Mods.GuardMeth("Orbit All", () =>
				{
					foreach (VRRig item6 in RigUtils.AllRigs(i: false))
					{
						Vector3 val = ((Component)Mods.OwnRig()).transform.position - ((Component)item6).transform.position;
						Mods.Throw(item6, val.normalized * 300f);
					}
				});
			},
			enabled = false,
			toolTip = "Fling everyone around you"
		},
		new ButtonInfo
		{
			buttonText = "Guardian Orbit Gun",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.GuardMeth("Orbit Gun", () =>
					{
						VRRig lockedrig = Mods.lockedrig;
						Vector3 val = ((Component)Mods.OwnRig()).transform.position - ((Component)Mods.lockedrig).transform.position;
						Mods.Throw(lockedrig, val.normalized * 300f);
					});
				});
			},
			enabled = false,
			toolTip = "Shoot someone to make them fling around you"
		},
		new ButtonInfo
		{
			buttonText = "Give Guardian Fling All",
			method = () =>
			{
				Mods.gf = true;
				Mods.Gi(1);
			},
			disableMethod = () =>
			{
				if (Mods.gf)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gf = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to fling everyone in the guardian gamemode"
		},
		new ButtonInfo
		{
			buttonText = "Give Guardian Spaz All",
			method = () =>
			{
				Mods.gs = true;
				Mods.Gi(2);
			},
			disableMethod = () =>
			{
				if (Mods.gs)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.gs = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to spaz everyone in the guardian gamemode"
		},
		new ButtonInfo
		{
			buttonText = "Give Guardian Orbit All",
			method = () =>
			{
				Mods.go = true;
				Mods.Gi(3);
			},
			disableMethod = () =>
			{
				if (Mods.go)
				{
					if ((Object)(object)Mods.gl != (Object)null)
					{
						Object.Destroy((Object)(object)((Component)Mods.gl).gameObject);
					}
					Mods.giv = null;
					Mods.go = false;
				}
			},
			enabled = false,
			toolTip = "Give someone the ability to orbit everyone in the guardian gamemode"
		},
		new ButtonInfo
		{
			buttonText = "Guardian To Point All",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.GuardMeth("To Point All", () =>
					{
						foreach (VRRig item7 in RigUtils.AllRigs(i: false))
						{
							Vector3 val = Mods.pointer.transform.position - ((Component)item7).transform.position;
							Mods.Throw(item7, val.normalized * 300f);
						}
					});
				});
			},
			enabled = false,
			toolTip = "Fling everyone to where you point your gun to"
		},
		new ButtonInfo
		{
			buttonText = "PaintBrawl Hard AimBot",
			method = () =>
			{
				if (!Mods.IsEnabled("PaintBrawl Soft AimBot"))
				{
					Mods.Aimbot(float.MaxValue);
				}
				else
				{
					Mods.SendNoti(Mods.Er + "Cannot Use Both AimBots At The Same Time!");
					Mods.DisableButton("PaintBrawl Hard AimBot");
				}
			},
			enabled = false,
			toolTip = "Shoot people through walls anywhere"
		},
		new ButtonInfo
		{
			buttonText = "PaintBrawl Soft AimBot",
			method = () =>
			{
				Mods.Aimbot(1.9f);
			},
			enabled = false,
			toolTip = "When the projectile gets near someone in a 2 meter radius, itll teleport the ball to the player and hit them"
		},
		new ButtonInfo
		{
			buttonText = "No PaintBrawl Cooldown (M)",
			n = "No PaintBrawl Cooldown (M)",
			method = () =>
			{
				string s2 = "No PaintBrawl Cooldown (M):";
				Mods.Paint(() =>
				{
					GorillaPaintbrawlManager val = (GorillaPaintbrawlManager)GorillaGameManager.instance;
					Mods.cool = !Mods.cool;
					val.stunGracePeriod = (Mods.cool ? 0f : 2f);
					val.hitCooldown = (Mods.cool ? 0f : 3f);
					Mods.NewName(s2 + " " + Mods.cool);
				}, () =>
				{
					Mods.cool = false;
					Mods.NewName(s2);
				});
			},
			enabled = false,
			toolTip = "Removes the hit cooldown in PaintBrawl for everyone"
		},
		new ButtonInfo
		{
			buttonText = "PaintBrawl Teams (M)",
			n = "PaintBrawl Teams (M)",
			method = () =>
			{
				string s2 = "PaintBrawl Teams (M):";
				Mods.Paint(() =>
				{
					GorillaPaintbrawlManager val = (GorillaPaintbrawlManager)GorillaGameManager.instance;
					val.teamBattle = !val.teamBattle;
					if (!val.teamBattle)
					{
						((GorillaGameManager)val).ResetGame();
					}
					Mods.NewName(s2 + " " + val.teamBattle);
				}, () =>
				{
					Mods.NewName(s2);
				});
			},
			enabled = false,
			toolTip = "Brings back og teams for PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Randomize Teams (M)",
			n = "Randomize Teams (M)",
			method = () =>
			{
				Mods.Paint(() =>
				{
					GorillaPaintbrawlManager val = (GorillaPaintbrawlManager)GorillaGameManager.instance;
					if (val.teamBattle)
					{
						val.RandomizeTeams();
					}
					else
					{
						Mods.SendNoti(Mods.Er + "You Must Have PaintBrawl Teams On!");
					}
				});
				Mods.DisableButton("Randomize Teams (M)");
			},
			enabled = false,
			toolTip = "Puts everyone of a random team"
		},
		new ButtonInfo
		{
			buttonText = "Set Orange Team All (M)",
			n = "Set Orange Team All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 1);
				});
				Mods.DisableButton("Set Orange Team All (M)");
			},
			enabled = false,
			toolTip = "Sets everyone to orange team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Orange Team Gun (M)",
			n = "Set Orange Team Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 1);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to set them to orange team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Orange Team Self (M)",
			n = "Set Orange Team Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 1);
				Mods.DisableButton("Set Orange Team Self (M)");
			},
			enabled = false,
			toolTip = "Sets yourself to orange team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Blue Team All (M)",
			n = "Set Blue Team All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 2);
				});
				Mods.DisableButton("Set Blue Team All (M)");
			},
			enabled = false,
			toolTip = "Sets everyone to blue team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Blue Team Gun (M)",
			n = "Set Blue Team Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 2);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to set them to blue team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Blue Team Self (M)",
			n = "Set Blue Team Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 2);
				Mods.DisableButton("Set Blue Team Self (M)");
			},
			enabled = false,
			toolTip = "Sets yourself to blue team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set No Team All (M)",
			n = "Set No Team All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 4);
				});
				Mods.DisableButton("Set No Team All (M)");
			},
			enabled = false,
			toolTip = "Sets everyone to no team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set No Team Gun (M)",
			n = "Set No Team Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 4);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to set them to no team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set No Team Self (M)",
			n = "Set No Team Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 4);
				Mods.DisableButton("Set No Team Self (M)");
			},
			enabled = false,
			toolTip = "Sets yourself to no team in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Stunned All (M)",
			n = "Set Stunned All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 67);
				});
			},
			enabled = false,
			toolTip = "Sets everyone to be stunned in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Stunned Gun (M)",
			n = "Set Stunned Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 67);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to set them to be stunned in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Stunned Self (M)",
			n = "Set Stunned Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 67);
			},
			enabled = false,
			toolTip = "Sets yourself to be stunned in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Hit All (M)",
			n = "Set Hit All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 69);
				});
			},
			enabled = false,
			toolTip = "Sets everyone to be hit in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Hit Gun (M)",
			n = "Set Hit Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 69);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to set them to be hit in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "Set Hit Self (M)",
			n = "Set Hit Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 69);
			},
			enabled = false,
			toolTip = "Sets yourself to be hit in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "GodMode All (M)",
			n = "GodMode All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 999);
				});
				Mods.DisableButton("GodMode All (M)");
			},
			enabled = false,
			toolTip = "Gives everyone god mode in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "GodMode Gun (M)",
			n = "GodMode Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 999);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to give them god mode in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "GodMode Self (M)",
			n = "GodMode Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 999);
				Mods.DisableButton("GodMode Self (M)");
			},
			enabled = false,
			toolTip = "Gives yourself god mode in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "UnGodMode All (M)",
			n = "UnGodMode All (M)",
			method = () =>
			{
				RigUtils.AllRigs().ForEach((VRRig e) =>
				{
					Mods.ChangeTeam(e, 3);
				});
				Mods.DisableButton("UnGodMode All (M)");
			},
			enabled = false,
			toolTip = "Undoes god mode for everyone in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "UnGodMode Gun (M)",
			n = "UnGodMode Gun (M)",
			method = () =>
			{
				Mods.MakeGun(l: true, () =>
				{
					Mods.ChangeTeam(Mods.lockedrig, 3);
				});
			},
			enabled = false,
			toolTip = "Shoot someone to undo god mode for them in PaintBrawl"
		},
		new ButtonInfo
		{
			buttonText = "UnGodMode Self (M)",
			n = "UnGodMode Self (M)",
			method = () =>
			{
				Mods.ChangeTeam(Mods.OwnRig(), 3);
				Mods.DisableButton("UnGodMode Self (M)");
			},
			enabled = false,
			toolTip = "Undoes god mode for yourself in PaintBrawl"
		}
	};

	public static List<ButtonInfo> CatButtons10 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Other Mods",
			method = () =>
			{
				Mods.Cat10();
			},
			enabled = false,
			toolTip = "Go to Main"
		},
		new ButtonInfo
		{
			buttonText = "Hear Server Mic",
			method = () =>
			{
				bool flag = false;
				if (PhotonNetwork.InRoom)
				{
					LocalVoice voice = Mods.Create(((Component)Mods.OwnRig()).GetComponent<GorillaSpeakerLoudness>()).Field(Mods.Field(42)).GetValue<Recorder>()
						.Voice;
					voice.DebugEchoMode = !voice.DebugEchoMode;
					flag = voice.DebugEchoMode;
				}
				else
				{
					Mods.SendNoti(Mods.Er + "You Must Be In A Room!");
				}
				Mods.NewName("Hear Server Mic: " + flag);
			},
			enabled = false,
			toolTip = "Lets you hear how you sound to others",
			n = "Hear Server Mic"
		},
		new ButtonInfo
		{
			buttonText = "TryOn Cosmetic Browser",
			method = () =>
			{
				Mods.CosBro();
			},
			enabled = false
		},
		new ButtonInfo
		{
			buttonText = "Join Room",
			method = () =>
			{
				Mods.SpawnBoard(new List<ButtonInfo>
				{
					new ButtonInfo
					{
						buttonText = "Confirm Room Join",
						method = () =>
						{
							if (Mods.typed.Length > 0)
							{
								GuiPlugin.Join(0, Mods.typed);
								Mods.DisableButton("Join Room");
								Mods.DeleteBoard();
							}
							else
							{
								Mods.SendNoti(Mods.Er + "Please Enter A Room Code First!");
							}
							Mods.DisableButton("Confirm Join Room");
						},
						enabled = false
					}
				}, "nospace", 10, "Type The Room Name Then Press Confirm Room Join|Join Room");
			}
		},
		new ButtonInfo
		{
			buttonText = "Mute All (CS)",
			method = () =>
			{
				if (PhotonNetwork.InRoom)
				{
					Mods.mue = !Mods.mue;
					foreach (GorillaPlayerScoreboardLine item8 in GorillaScoreboardTotalUpdater.allScoreboardLines.Where((GorillaPlayerScoreboardLine e) => e.linePlayer != NetworkSystem.Instance.LocalPlayer))
					{
						item8.muteButton.isOn = Mods.mue;
						item8.PressButton(Mods.mue, (ButtonType)3);
					}
				}
				Mods.NewName("Mute All (CS): " + Mods.mue);
			},
			enabled = false,
			toolTip = "Mute everyone"
		},
		new ButtonInfo
		{
			buttonText = "RJ Disconnect",
			method = () =>
			{
				if (rjoy)
				{
					PhotonNetwork.Disconnect();
				}
			},
			enabled = false,
			toolTip = "Press your right joystick to disconnect from the lobby"
		},
		new ButtonInfo
		{
			buttonText = "Disconnect",
			method = () =>
			{
				PhotonNetwork.Disconnect();
				Mods.DisableButton("Disconnect");
			},
			enabled = false,
			toolTip = "Disconnect from the lobby",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Normal Voxel Dig Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					ChunkComponent c2 = ((Component)Mods.rayCur.ReflectionEnums).GetComponent<ChunkComponent>();
					new Mods.Delay().D(PhotonNetwork.InRoom ? 0.05f : 0f, () =>
					{
						ChunkComponent val = c2;
						if (val != null)
						{
							VoxelExtensions.Mine(val.World, Mods.rayCur, new VoxelAction
							{
								strength = 1f,
								radius = 0.5f,
								operation = (OperationType)0
							});
						}
					});
				});
			},
			enabled = false,
			toolTip = "Shoot voxels to dig them in a normal radius"
		},
		new ButtonInfo
		{
			buttonText = "Big Voxel Dig Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					ChunkComponent c2 = ((Component)Mods.rayCur.ReflectionEnums).GetComponent<ChunkComponent>();
					new Mods.Delay().D(PhotonNetwork.InRoom ? 0.09f : 0f, () =>
					{
						ChunkComponent val = c2;
						if (val != null)
						{
							VoxelExtensions.Mine(val.World, Mods.rayCur, new VoxelAction
							{
								strength = 1f,
								radius = 4.67f,
								operation = (OperationType)0
							});
						}
					});
				});
			},
			enabled = false,
			toolTip = "Shoot voxels to dig them in a big radius"
		},
		new ButtonInfo
		{
			buttonText = "No Mining Cooldown",
			method = () =>
			{
				VoxelPickaxeUpdateInteractionPointPatch.e = !VoxelPickaxeUpdateInteractionPointPatch.e;
				Mods.NewName("No Mining Cooldown: " + VoxelPickaxeUpdateInteractionPointPatch.e);
			},
			enabled = false,
			toolTip = "Removes the cooldown for the mining pickaxe",
			n = "No Mining Cooldown"
		},
		new ButtonInfo
		{
			buttonText = "Big Mining Radius",
			method = () =>
			{
				VoxelPickaxeUpdateInteractionPointPatch.b = !VoxelPickaxeUpdateInteractionPointPatch.b;
				Mods.NewName("Big Mining Radius: " + VoxelPickaxeUpdateInteractionPointPatch.b);
			},
			enabled = false,
			toolTip = "Makes the mining pickaxe dig voxels in a big radius",
			n = "Big Mining Radius"
		},
		new ButtonInfo
		{
			buttonText = "Unlock SubscriptionManagerIsLocalSubscribedPatch",
			method = () =>
			{
				SubscriptionManagerIsLocalSubscribedPatch.e = !SubscriptionManagerIsLocalSubscribedPatch.e;
				Mods.NewName("Unlock SubscriptionManagerIsLocalSubscribedPatch: " + SubscriptionManagerIsLocalSubscribedPatch.e);
			},
			enabled = false,
			toolTip = "Unlock access to SubscriptionManagerIsLocalSubscribedPatch",
			n = "Unlock SubscriptionManagerIsLocalSubscribedPatch"
		},
		new ButtonInfo
		{
			buttonText = "Add SubscriptionManagerIsLocalSubscribedPatch Cosmetics To Cart",
			method = () =>
			{
				Mods.Vim();
			},
			enabled = false,
			toolTip = "Adds the SubscriptionManagerIsLocalSubscribedPatch cosmetics to your cart",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "First Person Camera",
			method = () =>
			{
				GuiPlugin.fpsc = !GuiPlugin.fpsc;
				GuiPlugin.fpsb = true;
				Mods.NewName("First Person Camera: " + GuiPlugin.fpsc);
			},
			enabled = false,
			toolTip = "Puts your pc screen into first person",
			n = "First Person Camera"
		},
		new ButtonInfo
		{
			buttonText = "Join Random Room",
			method = () =>
			{
				GuiPlugin.Join(1, "");
				Mods.DisableButton("Join Random Room");
			},
			enabled = false,
			toolTip = "Makes you join a random room",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Join Random Room In Current Map",
			method = () =>
			{
				GuiPlugin.Join(2, "");
				Mods.DisableButton("Join Random Room In Current Map");
			},
			enabled = false,
			toolTip = "Makes you join a random room in the current map your in",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Join Random Priv Room",
			method = () =>
			{
				GuiPlugin.Join(0, Mods.RandomString(s: true));
				Mods.DisableButton("Join Random Priv Room");
			},
			enabled = false,
			toolTip = "Makes you join a random private room",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Complete Scavenger Hunt",
			method = () =>
			{
				ScavengerManager.Instance.Hunts.Where((Hunt e) => !e.IsCompleted).ToList().ForEach((Hunt h) =>
				{
					h.Targets.Where((ScavengerTarget o) => !h.IsCollected(o)).ToList().ForEach((ScavengerTarget o) =>
					{
						h.Collect(o, false);
					});
				});
				Mods.DisableButton("Complete Scavenger Hunt");
			},
			enabled = false,
			toolTip = "Completes the current active scavenger hunt",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Complete MonkeBiz Quests",
			method = () =>
			{
				try
				{
					for (int i = 0; i < 2; i++)
					{
						foreach (QuestDisplay item9 in (List<QuestDisplay>)Mods.Create(Mods.F<MonkeBusinessStation>(5f).FirstOrDefault()).Field(Mods.Field(35)).GetValue())
						{
							item9.quest.ApplySavedProgress(item9.quest.requiredOccurenceCount);
							item9.quest.RemoveEventListener();
							item9.quest.questManager.HandleQuestCompleted(item9.quest.questID);
							item9.UpdateDisplay();
						}
					}
				}
				catch
				{
				}
				Mods.DisableButton("Complete MonkeBiz Quests");
			},
			enabled = false,
			toolTip = "Completes all of the MonkeBiz quests for you",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Kill All GhostReactor Enemies (CS)",
			method = () =>
			{
				try
				{
					foreach (GameEntity item10 in Mods.GhostShi("GhostReactorEnemy"))
					{
						Object.Destroy((Object)(object)((Component)item10).gameObject);
					}
				}
				catch
				{
				}
			},
			enabled = false,
			toolTip = "Kills all the enemies in GhostReactor only for you"
		},
		new ButtonInfo
		{
			buttonText = "Grab GhostReactor Cores",
			method = () =>
			{
				Mods.GrabColl("GhostReactorCollectibleCore");
			},
			enabled = false,
			toolTip = "Grab cores for the GhostReactor"
		},
		new ButtonInfo
		{
			buttonText = "Grab GhostReactor Seeds",
			method = () =>
			{
				Mods.GrabColl("GhostReactorCollectibleSentientCore(Clone)");
			},
			enabled = false,
			toolTip = "Grab seeds for the GhostReactor"
		},
		new ButtonInfo
		{
			buttonText = "Bug Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.MoveObj(Mods.GetObj("Floating Bug Holdable", "Bug"), Mods.pointer.transform.position + new Vector3(0f, 0.25f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn doug the bug"
		},
		new ButtonInfo
		{
			buttonText = "FireFly Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.MoveObj(Mods.GetObj("Floating Bug Holdable", "FireFly"), Mods.pointer.transform.position + new Vector3(0f, 0.25f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn the firefly"
		},
		new ButtonInfo
		{
			buttonText = "Bat Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.MoveObj(Mods.GetObj("Cave Bat Holdable", ""), Mods.pointer.transform.position + new Vector3(0f, 0.25f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn mat the bat"
		},
		new ButtonInfo
		{
			buttonText = "BeachBall Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.MoveObj(Mods.GetObj("Ball", ""), Mods.pointer.transform.position + new Vector3(0f, 0.38f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn the beach ball"
		},
		new ButtonInfo
		{
			buttonText = "ID Card Gun",
			method = () =>
			{
				Mods.MakeGun(l: false, () =>
				{
					Mods.MoveObj(Mods.GetObj("ID Card Holdable", ""), Mods.pointer.transform.position + new Vector3(0f, 0.38f, 0f));
				});
			},
			enabled = false,
			toolTip = "Shoot and spawn the id card"
		},
		new ButtonInfo
		{
			buttonText = "Grab Bug",
			method = () =>
			{
				Mods.GrabObj(Mods.GetObj("Floating Bug Holdable", "Bug"));
			},
			enabled = false,
			toolTip = "Grab doug the bug"
		},
		new ButtonInfo
		{
			buttonText = "Grab FireFly",
			method = () =>
			{
				Mods.GrabObj(Mods.GetObj("Floating Bug Holdable", "FireFly"));
			},
			enabled = false,
			toolTip = "Grab the firefly"
		},
		new ButtonInfo
		{
			buttonText = "Grab Bat",
			method = () =>
			{
				Mods.GrabObj(Mods.GetObj("Cave Bat Holdable", ""));
			},
			enabled = false,
			toolTip = "Grab mat the bat"
		},
		new ButtonInfo
		{
			buttonText = "Grab BeachBall",
			method = () =>
			{
				Mods.GrabObj(Mods.GetObj("Ball", ""));
			},
			enabled = false,
			toolTip = "Grab the beach ball"
		},
		new ButtonInfo
		{
			buttonText = "Grab ID Card",
			method = () =>
			{
				Mods.GrabObj(Mods.GetObj("ID Card Holdable", ""));
			},
			enabled = false,
			toolTip = "Grab the id card"
		},
		new ButtonInfo
		{
			buttonText = "Orbit Bug",
			method = () =>
			{
				Mods.MoveObj(Mods.GetObj("Floating Bug Holdable", "Bug"), new Mods.Delay().Orbit("bug", ((Component)Mods.OwnRig()).transform.position, 1f, 0f, 1f, 3f));
			},
			enabled = false,
			toolTip = "Make doug the bug orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Orbit FireFly",
			method = () =>
			{
				Mods.MoveObj(Mods.GetObj("Floating Bug Holdable", "FireFly"), new Mods.Delay().Orbit("fire", ((Component)Mods.OwnRig()).transform.position, 1f, 0f, 1f, 3f));
			},
			enabled = false,
			toolTip = "Make the firefly orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Orbit Bat",
			method = () =>
			{
				Mods.MoveObj(Mods.GetObj("Cave Bat Holdable", ""), new Mods.Delay().Orbit("bat", ((Component)Mods.OwnRig()).transform.position, 1f, 0f, 1f, 3f));
			},
			enabled = false,
			toolTip = "Make mat the bat orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Orbit BeachBall",
			method = () =>
			{
				Mods.MoveObj(Mods.GetObj("Ball", ""), new Mods.Delay().Orbit("ball", ((Component)Mods.OwnRig()).transform.position, 1f, 0f, 1f, 3f));
			},
			enabled = false,
			toolTip = "Make the beach ball orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Orbit ID Card",
			method = () =>
			{
				Mods.MoveObj(Mods.GetObj("ID Card Holdable", ""), new Mods.Delay().Orbit("idcard", ((Component)Mods.OwnRig()).transform.position, 1f, 0f, 1f, 3f));
			},
			enabled = false,
			toolTip = "Make the id card orbit you"
		},
		new ButtonInfo
		{
			buttonText = "Snowball Forest Ground",
			method = () =>
			{
				Mods.snowground = true;
				try
				{
					Mods.Sur("pit ground bottom").overrideIndex = Mods.d["Growing Snowball"];
				}
				catch
				{
				}
			},
			disableMethod = () =>
			{
				if (Mods.snowground)
				{
					try
					{
						Mods.Sur("pit ground bottom").overrideIndex = Mods.over[1];
					}
					catch
					{
					}
					Mods.snowground = false;
				}
			},
			enabled = false,
			toolTip = "Be able to pick up snowballs from the forest ground"
		},
		new ButtonInfo
		{
			buttonText = "Random Color Snowballs (SS)",
			method = () =>
			{
				Mods.rais = true;
				SnowballThrowable[] array = Mods.F<SnowballThrowable>(5f);
				foreach (SnowballThrowable val in array)
				{
					if (!val.randomizeColor)
					{
						val.randomizeColor = true;
					}
				}
			},
			disableMethod = () =>
			{
				if (Mods.rais)
				{
					SnowballThrowable[] array = Mods.F<SnowballThrowable>(5f);
					foreach (SnowballThrowable val in array)
					{
						if (val.randomizeColor)
						{
							val.randomizeColor = false;
						}
					}
					Mods.rais = false;
				}
			},
			enabled = false,
			toolTip = "Makes snowballs be random colors"
		},
		new ButtonInfo
		{
			buttonText = "Auto Big Snowballs",
			method = () =>
			{
				Mods.AutoBigSnowballs();
			},
			enabled = false,
			toolTip = "VoxelPickaxeUpdateInteractionPointPatch up a snowball and instantly make it big"
		},
		new ButtonInfo
		{
			buttonText = "Get All IDS",
			method = () =>
			{
				Mods.GetAllIDs();
			},
			enabled = false,
			toolTip = "Saves everyones player id to a txt file on your computer",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Disable Network Triggers",
			method = () =>
			{
				Mods.network = true;
				Mods.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(false);
			},
			disableMethod = () =>
			{
				if (Mods.network)
				{
					Mods.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(true);
					Mods.network = false;
				}
			},
			enabled = false,
			toolTip = "Allows you to go into other maps without disconnecting"
		},
		new ButtonInfo
		{
			buttonText = "Join Ghost Codes",
			method = () =>
			{
				Mods.JoinRandomGhostCode();
			},
			enabled = false,
			toolTip = "Join random ghost codes",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Set Quest Score To Max (SS)",
			method = () =>
			{
				Mods.OwnRig().SetQuestScore(99999);
				Mods.DisableButton("Set Quest Score To Max (SS)");
			},
			enabled = false,
			toolTip = "Change your quest badge score to 99999 (which is the max)",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Disable WeatherStatus Text",
			method = () =>
			{
				Mods.wee = !Mods.wee;
				GorillaScoreboardTotalUpdater.allScoreboards.ForEach((GorillaScoreBoard g) =>
				{
					((Component)g.weatherControlsText).gameObject.SetActive(!Mods.wee);
				});
				Mods.NewName("Disable WeatherStatus Text: " + Mods.wee);
			},
			enabled = false,
			n = "Disable WeatherStatus Text",
			toolTip = "Disable the WeatherStatus text on the leaderboard"
		},
		new ButtonInfo
		{
			buttonText = "Mute All TVs",
			method = () =>
			{
				bool flag = false;
				try
				{
					PlayerPrefFlagButton val = Mods.F<PlayerPrefFlagButton>(5f).Where((PlayerPrefFlagButton e) =>
					{
						return (int)Mods.Create(e).Field(Mods.Field(70)).GetValue<Flag>() == 8;
					}).FirstOrDefault();
					((GorillaPressableButton)val).Click(true);
					flag = ((GorillaPressableButton)val).isOn;
				}
				catch
				{
				}
				Mods.NewName("Mute All TVs: " + flag);
			},
			enabled = false,
			n = "Mute All TVs",
			toolTip = "Mutes all the tvs"
		},
		new ButtonInfo
		{
			buttonText = "Open Elevator",
			method = () =>
			{
				Mods.Ele(4, "Open Elevator");
			},
			enabled = false,
			toolTip = "Open the elevator",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Close Elevator",
			method = () =>
			{
				Mods.Ele(5, "Close Elevator");
			},
			enabled = false,
			toolTip = "Close the elevator",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To Mall",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To Mall");
			},
			enabled = false,
			toolTip = "Sends the elevator to Mall",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To City",
			method = () =>
			{
				Mods.Ele(2, "Send Elevator To City");
			},
			enabled = false,
			toolTip = "Sends the elevator to City",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To GhostReactor",
			method = () =>
			{
				Mods.Ele(3, "Send Elevator To GhostReactor");
			},
			enabled = false,
			toolTip = "Sends the elevator to GhostReactor",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To GhostEntrance",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To GhostEntrance", "GRMallEntranceElevator");
			},
			enabled = false,
			toolTip = "Sends the elevator to GhostEntrance",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To MonkeBlocks",
			method = () =>
			{
				Mods.Ele(7, "Send Elevator To MonkeBlocks");
			},
			enabled = false,
			toolTip = "Sends the elevator to MonkeBlocks",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To Lava",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To Lava", "VIMForestLavaElevator");
			},
			enabled = false,
			toolTip = "Sends the elevator to the Lava map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To Fossil Hunt",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To Fossil Hunt", "VIMExperience3Elevator");
			},
			enabled = false,
			toolTip = "Sends the elevator to the Fossil Hunt map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To GravDash",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To GravDash", "VIMExperience4Elevator");
			},
			enabled = false,
			toolTip = "Sends the elevator to the GravDash map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Send Elevator To OG Basement",
			method = () =>
			{
				Mods.Ele(1, "Send Elevator To OG Basement", "VIMExperience2Elevator");
			},
			enabled = false,
			toolTip = "Sends the elevator to the OG Basement map",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Anti Snowball GTPlayerApplyKnockbackPatch",
			method = () =>
			{
				GTPlayerApplyKnockbackPatch.e = !GTPlayerApplyKnockbackPatch.e;
				Mods.NewName("Anti Snowball GTPlayerApplyKnockbackPatch: " + GTPlayerApplyKnockbackPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so where no one can fling you around with snowballs",
			n = "Anti Snowball GTPlayerApplyKnockbackPatch"
		},
		new ButtonInfo
		{
			buttonText = "Anti Guardian Fling",
			method = () =>
			{
				VRRigGrabbedByPlayerPatch.e = !VRRigGrabbedByPlayerPatch.e;
				Mods.NewName("Anti Guardian Fling: " + VRRigGrabbedByPlayerPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so where you dont get flung by modders using guardian fling mods",
			n = "Anti Guardian Fling"
		},
		new ButtonInfo
		{
			buttonText = "Anti Block Crash",
			method = () =>
			{
				BuilderTableNetworkingPieceCreatedByShelfRPCPatch.e = !BuilderTableNetworkingPieceCreatedByShelfRPCPatch.e;
				Mods.NewName("Anti Block Crash: " + BuilderTableNetworkingPieceCreatedByShelfRPCPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so you cant get crashed in monke blocks if someone tries to crash you",
			n = "Anti Block Crash"
		},
		new ButtonInfo
		{
			buttonText = "Anti Break Movement",
			method = () =>
			{
				GameEntityManagerCreateItemRPCPatch.e = !GameEntityManagerCreateItemRPCPatch.e;
				Mods.NewName("Anti Break Movement: " + GameEntityManagerCreateItemRPCPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so your movement wont break by modders using break movement mods",
			n = "Anti Break Movement"
		},
		new ButtonInfo
		{
			buttonText = "Anti Crash",
			method = () =>
			{
				GameEntityManagerCreateItemRPCPatch.a = !GameEntityManagerCreateItemRPCPatch.a;
				Mods.NewName("Anti Crash: " + GameEntityManagerCreateItemRPCPatch.a);
			},
			enabled = false,
			toolTip = "Makes it so you cant get crashed from modders using crash mods",
			n = "Anti Crash"
		},
		new ButtonInfo
		{
			buttonText = "Anti App Quit",
			method = () =>
			{
				SecondLookSkeletonFloatPlayerPatch.e = !SecondLookSkeletonFloatPlayerPatch.e;
				Mods.NewName("Anti App Quit: " + SecondLookSkeletonFloatPlayerPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so you cant get app quitted by modders using app quit mods",
			n = "Anti App Quit"
		},
		new ButtonInfo
		{
			buttonText = "Anti Lucy Float",
			method = () =>
			{
				HalloweenGhostChaserRiseGrabbedLocalPlayerPatch.e = !HalloweenGhostChaserRiseGrabbedLocalPlayerPatch.e;
				Mods.NewName("Anti Lucy Float: " + HalloweenGhostChaserRiseGrabbedLocalPlayerPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so you wont float when lucy grabs you",
			n = "Anti Lucy Float"
		},
		new ButtonInfo
		{
			buttonText = "Anti Lurker",
			method = () =>
			{
				LurkerGhostSeekPlayerPatch.e = !LurkerGhostSeekPlayerPatch.e;
				Mods.NewName("Anti Lurker: " + LurkerGhostSeekPlayerPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so you dont get haunted by the lurker",
			n = "Anti Lurker"
		},
		new ButtonInfo
		{
			buttonText = "Anti Computer Kick",
			method = () =>
			{
				PhotonNetworkControllerAttemptToFollowIntoPubPatch.e = !PhotonNetworkControllerAttemptToFollowIntoPubPatch.e;
				Mods.NewName("Anti Computer Kick: " + PhotonNetworkControllerAttemptToFollowIntoPubPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so you cant get kicked from Kick Near Computer mods",
			n = "Anti Computer Kick"
		},
		new ButtonInfo
		{
			buttonText = "Anti Deafen",
			method = () =>
			{
				LoadBalancingTransportonEventActionVoiceClientPatch.e = !LoadBalancingTransportonEventActionVoiceClientPatch.e;
				Mods.NewName("Anti Deafen: " + LoadBalancingTransportonEventActionVoiceClientPatch.e);
			},
			enabled = false,
			toolTip = "Makes it so if someone trys to deafen all, you wont get deafend",
			n = "Anti Deafen"
		},
		new ButtonInfo
		{
			buttonText = "Anti AFK Kick",
			method = () =>
			{
				PhotonNetworkController.Instance.disableAFKKick = !PhotonNetworkController.Instance.disableAFKKick;
				Mods.NewName("Anti AFK Kick: " + PhotonNetworkController.Instance.disableAFKKick);
			},
			enabled = false,
			toolTip = "Disables AFK Kick so you wont get kicked for being AFK",
			n = "Anti AFK Kick"
		},
		new ButtonInfo
		{
			buttonText = "Join Discord Server",
			method = () =>
			{
				Process.Start("https://discord.gg/2AmFSzYeAV");
				Mods.DisableButton("Join Discord Server");
			},
			enabled = false,
			toolTip = "Opens your broswer and opens the discord invite link",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Clear Menu Preset",
			method = () =>
			{
				Mods.ClearPreset();
			},
			enabled = false,
			toolTip = "Applys all the clear settings to the menu"
		},
		new ButtonInfo
		{
			buttonText = "Rainbow Menu Preset",
			method = () =>
			{
				Mods.RainbowPreset();
			},
			enabled = false,
			toolTip = "Applys all the rainbow settings to the menu",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Random Menu Preset",
			method = () =>
			{
				Mods.RandomPreset();
			},
			enabled = false,
			toolTip = "Applys random settings to the menu",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Malachis Personal Preset",
			method = () =>
			{
				Mods.MalachiPreset();
			},
			enabled = false,
			toolTip = "Applys all the settings the owner of the menu uses",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Reset Menu Appearance",
			method = () =>
			{
				Mods.Load();
				Mods.DisableButton("Reset Menu Appearance");
			},
			enabled = false,
			toolTip = "Just loads your saved settings",
			n = "n"
		},
		new ButtonInfo
		{
			buttonText = "Keyboard",
			method = () =>
			{
				Mods.SpawnBoard((from e in Mods.GetAllButtons(i: true, ii: false, c: true)
					where !e.buttonText.Contains("Exit") && e.buttonText != "Keyboard"
					select e).ToList());
			},
			enabled = false,
			toolTip = " "
		}
	};

	public static List<ButtonInfo> CatButtons11 = new List<ButtonInfo>();

	public static List<ButtonInfo> CatButtons12 = new List<ButtonInfo>
	{
		new ButtonInfo
		{
			buttonText = "Exit Favorite Mods",
			method = () =>
			{
				Mods.Cat12();
			},
			enabled = false,
			toolTip = "Go to Main"
		}
	};

	public static Color FirstColor = Color.blue;

	public static Color SecondColor = Color.black;

	public static Color ButtonColorDisable = Color.yellow;

	public static Color ButtonColorEnabled = Color.magenta;

	public static Color EnableTextColor = Color.black;

	public static Color DIsableTextColor = Color.black;

	public static Color DICo = Color.red;

	public static Color DITCo = Color.white;

	public static Color NPCo = Color.black;

	public static Color NPTC = Color.white;

	public static Color MenuTC = Color.white;

	public static Color Searchb = Color.black;

	public static Color SearchT = Color.white;

	public static Color Homeb = Color.black;

	public static Color HomeT = Color.white;

	public static bool gripDownR;

	public static bool triggerDownR;

	public static bool abuttonDown;

	public static bool bbuttonDown;

	public static bool xbuttonDown;

	public static bool ybuttonDown;

	public static bool gripDownL;

	public static bool triggerDownL;

	public static bool rjoy;

	public static bool ljoy;

	public static VRRig ch = null;

	public static int clickcool = 10;

	public static int yost = 0;

	public static int yofs = 0;

	public static int yots = 0;

	public static int yola = 0;

	public static int yolaa = 0;

	public static int yolaaa = 0;

	public static int lx = -1;

	public static GameObject menu = null;

	public static GameObject canvasObj = null;

	public static GameObject reference = null;

	public static int pageNumber = 0;

	public static WristMenu instance = new WristMenu();

	public static GameObject menuObj;

	public static Text tooltipText;

	public static string tooltipString;

	public static bool toggle = false;

	public static bool toggle1 = false;

	public static bool bu;

	public static bool ru;

	public static int pageSize = 4;

	public static bool flush = true;

	public static bool was = false;

	public static bool cle = false;

	public static bool alreadyma = false;

	public static bool dgv = false;

	public static string LetterMeanings = "SS = SERVER SIDED | CS = CLIENT SIDED\nM = MASTER (FIRST ON THE LEADERBOARD IN THE LOBBY)\nNC = NEAR COMPUTER/WARDROBE\nRJ = RIGHT JOYSTICK CLICK\nLJ = LEFT JOYSTICK CLICK";

	public static string Annc = "";

	public static string fcc = "Shoutout To My Friends Cuz Y Not:\nS-VESTER\nRADER\nBLU";

	public static bool savc = false;

	public static bool coss = false;

	public static bool customboards = false;

	public static Shader esp = null;

	public static List<Player> lea = new List<Player>();

	public static string arrow = "< >";

	public static Dictionary<Player, HashSet<string>> allcos = new Dictionary<Player, HashSet<string>>();

	public static string[] ownids;

	public static bool owner = false;

	public static bool usingm = false;

	public static string ss = "67ur actually so weird for making a mod checker LMAO get a life kid";

	public static object[] cus = new object[2] { ss, 67 };

	public static bool check = false;

	public static Player own = null;

	public static int trys = 0;

	public static float ver = -0.9f;

	public static bool check1 = false;

	public static Text titiel;

	public static GameObject Button;

	public static Text text2;

	public static Dictionary<string, List<ButtonInfo>> bll;

	public static Vector3 Sp;

	public static Dictionary<Player, GameObject> modsidedmenus = new Dictionary<Player, GameObject>();

	public static Dictionary<Player, object[]> modsidedextras = new Dictionary<Player, object[]>();

	public static Dictionary<Player, object[]> sidedbuttons = new Dictionary<Player, object[]>();

	public static Dictionary<Player, object[]> sidedkeyboards = new Dictionary<Player, object[]>();

	public static bool RJoy()
	{
		try
		{
			bool flag = false;
			try
			{
				flag = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState((SteamVR_Input_Sources)2);
			}
			catch
			{
				ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, ref flag);
			}
			if (flag && !Mods.last6)
			{
				ru = !ru;
			}
			Mods.last6 = flag;
			return ru;
		}
		catch
		{
			return false;
		}
	}

	public static bool LJoy()
	{
		try
		{
			bool flag = false;
			try
			{
				flag = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState((SteamVR_Input_Sources)1);
			}
			catch
			{
				ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, ref flag);
			}
			if (flag && !Mods.last5)
			{
				bu = !bu;
			}
			Mods.last5 = flag;
			return bu;
		}
		catch
		{
			return false;
		}
	}

	public static void UseAnyways()
	{
		string s2 = "Use Menu Anyways (<color=red>USE WITH CAUTION</color>";
		if (CatButtons11.Any((ButtonInfo e) => e.buttonText == s2))
		{
			return;
		}
		Mods.searching = true;
		CatButtons11.Insert(0, new ButtonInfo
		{
			buttonText = s2,
			method = () =>
			{
				ver = 1f;
				Mods.searching = false;
				Mods.DisableButton(s2);
				CatButtons1.RemoveAll((ButtonInfo x) => x.buttonText == s2);
			}
		});
		Refresh();
	}

	private async void Update()
	{
		try
		{
			string mn = MenuTitle;
			if (Mods.tyname)
			{
				int l = (int)Mathf.PingPong(Time.time / 0.167f, (float)(MenuTitle.Length + 1));
				mn = ((l > 1) ? MenuTitle.Substring(0, l) : "M");
			}
			if (Time.time > Mods.bal + 0.1f)
			{
				Mods.bal = Time.time;
				titiel.text = mn + $"\n Fps: {Mathf.RoundToInt(1f / Time.deltaTime)} | Page: {pageNumber + 1}";
			}
			ControllerInputPoller ci = ControllerInputPoller.instance;
			gripDownL = ci.leftGrab;
			gripDownR = ci.rightGrab;
			rjoy = RJoy();
			ljoy = LJoy();
			triggerDownL = ci.leftControllerIndexFloat > 0.6f;
			triggerDownR = ci.rightControllerIndexFloat > 0.6f;
			abuttonDown = ci.rightControllerPrimaryButton;
			bbuttonDown = ci.rightControllerSecondaryButton;
			xbuttonDown = ci.leftControllerPrimaryButton;
			ybuttonDown = ci.leftControllerSecondaryButton;
			if ((Mods.change7 == 5 || Mods.change7 == 6) && (!Object.op_Implicit((Object)(object)menu.GetComponent<Rigidbody>()) || Mods.searching))
			{
				if ((Mods.change7 == 6) ? gripDownL : triggerDownL)
				{
					if (!toggle)
					{
						Toggle("Prev");
						GuiPlugin.PT(b: true);
						toggle = true;
					}
				}
				else
				{
					toggle = false;
				}
				if ((Mods.change7 == 6) ? gripDownR : triggerDownR)
				{
					if (!toggle1)
					{
						Toggle("Next");
						GuiPlugin.PT();
						toggle1 = true;
					}
				}
				else
				{
					toggle1 = false;
				}
			}
			if (!owner)
			{
				if (ver > -1f)
				{
					if (Version() < ver)
					{
						Mods.DelayedNoti(Mods.Er + "YOU ARE USING AN OUTDATED VERSION OF THE MENU. PLEASE UPDATE BY DOWNLOADING THE NEWEST VERSION!");
					}
				}
				else
				{
					Mods.DelayedNoti(Mods.Er + "MENU IS CURRENTLY DOWN. READ THE COC BOARD IN STUMP!");
					UseAnyways();
				}
			}
			else
			{
				string s2 = "Use Menu Anyways (<color=red>USE WITH CAUTION</color>";
				if (CatButtons11.Any((ButtonInfo buttonInfo) => buttonInfo.buttonText == s2))
				{
					Mods.searching = false;
					CatButtons11.RemoveAll((ButtonInfo x) => x.buttonText == s2);
					Refresh();
				}
			}
			if (ybuttonDown && !Mods.right)
			{
				if ((Object)(object)menu == (Object)null)
				{
					instance.Draw();
				}
				else
				{
					menu.transform.position = Mods.Loco().LeftHand.controllerTransform.position;
					menu.transform.rotation = Mods.Loco().LeftHand.controllerTransform.rotation;
					menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * Mods.Loco().scale;
					if ((Object)(object)reference == (Object)null)
					{
						reference = Mods.CreatePrimitive((PrimitiveType)0);
						((Object)reference).name = "bp";
						reference.layer = 2;
						reference.transform.parent = Mods.Loco().RightHand.controllerTransform;
						reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
						reference.GetComponent<Renderer>().material.color = FirstColor;
						ModSidedMenu.Send("spawn");
					}
					reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
				}
				if (Object.op_Implicit((Object)(object)menu.GetComponent<Rigidbody>()))
				{
					Object.Destroy((Object)(object)menu.GetComponent<Rigidbody>());
				}
			}
			else if (!ybuttonDown && !Mods.right && !Object.op_Implicit((Object)(object)menu.GetComponent<Rigidbody>()))
			{
				Object.Destroy((Object)(object)reference);
				reference = null;
				menu.AddComponent<Rigidbody>();
				menu.GetComponent<Rigidbody>().isKinematic = false;
				menu.GetComponent<Rigidbody>().useGravity = true;
				menu.GetComponent<Rigidbody>().linearVelocity = Mods.Loco().LeftHand.velocityTracker.GetAverageVelocity(true, 0f, false);
			}
			if (bbuttonDown && Mods.right)
			{
				if ((Object)(object)menu == (Object)null)
				{
					instance.Draw();
				}
				else
				{
					menu.transform.position = Mods.Loco().RightHand.controllerTransform.position;
					menu.transform.rotation = Mods.Loco().RightHand.controllerTransform.rotation;
					menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
					menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * Mods.Loco().scale;
					if ((Object)(object)reference == (Object)null)
					{
						reference = Mods.CreatePrimitive((PrimitiveType)0);
						((Object)reference).name = "bp";
						reference.layer = 2;
						reference.transform.parent = Mods.Loco().LeftHand.controllerTransform;
						reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
						reference.GetComponent<Renderer>().material.color = FirstColor;
						ModSidedMenu.Send("spawn");
					}
					reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
				}
				if (Object.op_Implicit((Object)(object)menu.GetComponent<Rigidbody>()))
				{
					Object.Destroy((Object)(object)menu.GetComponent<Rigidbody>());
				}
			}
			else if (!abuttonDown && Mods.right && !Object.op_Implicit((Object)(object)menu.GetComponent<Rigidbody>()))
			{
				Object.Destroy((Object)(object)reference);
				reference = null;
				menu.AddComponent<Rigidbody>();
				menu.GetComponent<Rigidbody>().isKinematic = false;
				menu.GetComponent<Rigidbody>().useGravity = true;
				menu.GetComponent<Rigidbody>().linearVelocity = Mods.Loco().RightHand.velocityTracker.GetAverageVelocity(true, 0f, false);
			}
			if (Mods.searching && CatButtons11.Count > 1)
			{
				foreach (ButtonInfo b2 in CatButtons11)
				{
					try
					{
						if (!bll.TryGetValue(b2.buttonText, out var m))
						{
							continue;
						}
						bool t = b2.enabled == true;
						foreach (ButtonInfo bb in m)
						{
							if (bb.enabled != t)
							{
								bb.enabled = t;
							}
						}
						m = null;
					}
					catch (Exception ex)
					{
						Debug.Log((object)("Searching: " + ex));
					}
				}
			}
			if (Mods.inCat12)
			{
				List<ButtonInfo> v = CatButtons12.Where((ButtonInfo buttonInfo) => buttonInfo.buttonText != "Exit Favorite Mods").ToList();
				if (v.Count > 0)
				{
					foreach (ButtonInfo b3 in v)
					{
						try
						{
							if (!bll.TryGetValue(b3.buttonText, out var m2))
							{
								continue;
							}
							bool t2 = b3.enabled == true;
							foreach (ButtonInfo bb2 in m2)
							{
								if (bb2.enabled != t2)
								{
									bb2.enabled = t2;
								}
							}
							m2 = null;
						}
						catch (Exception ex2)
						{
							Debug.Log((object)("favs: " + ex2));
						}
					}
				}
			}
			try
			{
				foreach (ButtonInfo b4 in buttons)
				{
					if (b4.enabled == true)
					{
						b4.method();
					}
					if (b4.enabled == false && b4.disableMethod != null)
					{
						b4.disableMethod();
					}
				}
				foreach (ButtonInfo b5 in CatButtons1)
				{
					if (b5.enabled == true)
					{
						b5.method();
					}
					if (b5.enabled == false && b5.disableMethod != null)
					{
						b5.disableMethod();
					}
				}
				foreach (ButtonInfo b6 in Mods.GetAllButtons())
				{
					try
					{
						if (b6.enabled == true)
						{
							b6.method();
						}
						if (b6.enabled == false && b6.disableMethod != null)
						{
							b6.disableMethod();
						}
						if (b6.enabled == true && !CatButtons1.Any((ButtonInfo x) => x.buttonText == b6.buttonText))
						{
							ButtonInfo bu = new ButtonInfo
							{
								buttonText = b6.buttonText,
								method = () =>
								{
									if (CatButtons1.Any((ButtonInfo x) => x.buttonText == b6.buttonText))
									{
										Mods.DisableButton(b6.buttonText);
									}
								},
								enabled = false,
								toolTip = b6.toolTip
							};
							CatButtons1.Insert(3, bu);
							Refresh();
						}
						if (b6.enabled == false && CatButtons1.Any((ButtonInfo x) => x.buttonText == b6.buttonText && x.buttonText != "Disable All Mods" && x.buttonText != "Save Mods"))
						{
							CatButtons1.RemoveAll((ButtonInfo x) => x.buttonText == b6.buttonText);
							Refresh();
						}
					}
					catch (Exception ex3)
					{
						Debug.Log((object)("Mod: " + b6.buttonText + " | " + ex3));
					}
				}
			}
			catch (Exception ex4)
			{
				Debug.Log((object)("Mod Invoking: " + ex4));
			}
			if (!savc)
			{
				if (!Directory.Exists("Malachis_Menu_Reborn"))
				{
					Directory.CreateDirectory("Malachis_Menu_Reborn");
				}
				if (!File.Exists("Malachis_Menu_Reborn\\Saved_Settings.txt") || !File.Exists("Malachis_Menu_Reborn\\Saved_Settings2.txt"))
				{
					Mods.Save();
				}
				string p = "Malachis_Menu_Reborn/Special_Players.txt";
				if (!File.Exists(p))
				{
					File.WriteAllText(p, "SPECIAL PLAYERS LOGGED USING MALACHIS MENU REBORN.\nWHEN THERES A PERSON WITH A SPECIAL COSMETIC IN YOUR LOBBY, THEY WILL AUTOMATICALLY GET PUT INTO THIS TXT.\nPLAYERS LOGGED: 0 { MOD STICK: 0 | FINGER PAINTERS: 0 | ILLUSTRATORS: 0 | FIRE STICKS: 0 | AA BADGES/HATS: 0 | GT RIBBONS: 0 | GOLD PHONE: 0 | MMH STAFF: 0 | GOLD LEMMING PLUSHIES: 0 }\n\n");
				}
				Mods.SaveMenuFeatures();
				try
				{
					((Behaviour)Mods.F<Playspace>(5f).FirstOrDefault()).enabled = false;
				}
				catch
				{
				}
				if (!GorillaComputer.instance.allowedInCompetitive)
				{
					GorillaComputer.instance.CompQueueUnlockButtonPress();
				}
				try
				{
					Mods.GIn().maxTagDistance = float.MaxValue;
				}
				catch
				{
				}
				GameObject g = Mods.CreatePrimitive((PrimitiveType)3);
				((Object)g).name = "barr";
				g.transform.localScale = new Vector3(float.MaxValue, 0f, float.MaxValue);
				g.transform.position = new Vector3(0f, 1000f, 0f);
				Object.Destroy((Object)(object)g.GetComponent<Renderer>());
				esp = Shader.Find("GUI/Text Shader");
				Object.Destroy((Object)(object)Mods.Find("Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnInForestAndHoverboard/ForestDome_CollisionOnly"));
				savc = true;
			}
			if (PhotonNetwork.InRoom && flush)
			{
				GuiPlugin.Last = PhotonNetwork.CurrentRoom.Name;
				Mods.RPCFlush();
				if (Mods.jb)
				{
					Mods.jb = false;
				}
				try
				{
					foreach (Player p2 in from item in RigUtils.AllPlayers(i: false)
						where !Mods.usings.Contains(item)
						select item)
					{
						IsUsing(p2, out var s3);
						if (s3 == "<color=#FF00FF>")
						{
							Mods.SendNoti(s3 + "The Owner Of The Menu Is Here</color>");
						}
						if (s3 == "<color=#00FFFF>")
						{
							Mods.SendNoti(s3 + "A Malachi Menu User Is Here</color>");
						}
						Mods.usings.Add(p2);
						s3 = null;
					}
				}
				catch
				{
				}
				alreadyma = Mods.Local().IsMasterClient;
				flush = false;
			}
			else if (!PhotonNetwork.InRoom && !flush)
			{
				LinqUtils.ForEach<Player>(from val3 in allcos.Keys.ToList()
					where val3 != Mods.Local()
					select val3, (Action<Player>)((Player key) =>
				{
					allcos.Remove(key);
				}));
				Mods.masternoti = false;
				if (Mods.jb)
				{
					((GorillaTriggerBox)PhotonNetworkController.Instance.currentJoinTrigger).OnBoxTriggered();
				}
				alreadyma = false;
				if (Mods.usings.Count > 0)
				{
					Mods.usings.Clear();
				}
				if (Mods.usingmenu.Count > 0)
				{
					Mods.usingmenu.Clear();
				}
				if (modsidedmenus.Keys.Count > 0)
				{
					LinqUtils.ForEach<KeyValuePair<Player, GameObject>>((IEnumerable<KeyValuePair<Player, GameObject>>)modsidedmenus, (Action<KeyValuePair<Player, GameObject>>)((KeyValuePair<Player, GameObject> keyValuePair) =>
					{
						Object.Destroy((Object)(object)keyValuePair.Value);
					}));
					modsidedmenus.Clear();
				}
				if (sidedkeyboards.Keys.Count > 0)
				{
					foreach (object[] e in sidedkeyboards.Values)
					{
						Object.Destroy((Object)(GameObject)e[0]);
						Object.Destroy((Object)(GameObject)e[1]);
					}
					sidedkeyboards.Clear();
				}
				if (sidedbuttons.Keys.Count > 0)
				{
					sidedbuttons.Clear();
				}
				if (modsidedextras.Keys.Count > 0)
				{
					modsidedextras.Clear();
				}
				if ((Object)(object)Mods.lockedrig != (Object)null)
				{
					Mods.lockedrig = null;
				}
				Mods.cool = false;
				flush = true;
			}
			if (PhotonNetwork.InRoom)
			{
				try
				{
					new Mods.Delay().D(10f, () =>
					{
						foreach (Player item2 in RigUtils.AllPlayers())
						{
							HashSet<string> h = Mods.Create(RigUtils.RigFromPlayer(item2)).Field(Mods.Field(0)).GetValue<HashSet<string>>();
							if (!allcos.ContainsKey(item2))
							{
								allcos.Add(item2, h);
							}
							if (Mods.sc.Keys.Any((string k) => h.Contains(k)))
							{
								string text2 = File.ReadAllText("Malachis_Menu_Reborn/Special_Players.txt");
								List<string> list = (from x in text2.Split(new string[1] { "\n\n" }, StringSplitOptions.None)
									where !string.IsNullOrWhiteSpace(x)
									select x).ToList();
								if (!text2.Contains(item2.UserId))
								{
									VRRig val3 = RigUtils.RigFromPlayer(item2);
									string text3 = "";
									foreach (string item3 in Mods.sc.Keys.Where((string item2) => h.Contains(item2)).ToList())
									{
										text3 = text3 + Mods.sc[item3] + ", ";
									}
									Color playerColor = val3.playerColor;
									string text4 = "QUEST";
									if (h.Contains("S. FIRST LOGIN"))
									{
										text4 = "STEAM";
									}
									if (h.Contains("FIRST LOGIN"))
									{
										text4 = "OCULUSPC";
									}
									string item = string.Format("NAME: {0}  |  TIME LOGGED: {1}\n{{\n  <PLAYER INFO> <—>  ID: {2} | COLOR: {3}, {4}, {5} | PLATFORM: {6} | COSMETICS: {7}\n  <LOBBY INFO>  <—>  CODE: {8} | REGION: {9} | GAMEMODE: {10} | QUEUE: {11} | MAP: {12}\n}}", item2.NickName, DateTime.Now, item2.UserId, Mathf.RoundToInt(playerColor.r * 9f), Mathf.RoundToInt(playerColor.g * 9f), Mathf.RoundToInt(playerColor.b * 9f), text4, text3, PhotonNetwork.CurrentRoom.Name, NetworkSystem.Instance.CurrentRegion.Replace("/*", "").ToUpper(), GuiPlugin.GetQueOrGameMode(0).ToUpper(), GuiPlugin.GetQueOrGameMode(1).ToUpper(), PhotonNetworkController.Instance.currentJoinTrigger.networkZone.ToUpper());
									list.Add(item);
									int[] i = new int[10]
									{
										list.Where((string text5) => text5.Contains("<PLAYER INFO>")).ToList().Count,
										list.Where((string text5) => text5.Contains("MOD STICK,")).ToList().Count,
										list.Where((string text5) => text5.Contains("FINGER PAINTER,")).ToList().Count,
										list.Where((string text5) => text5.Contains("ILLUSTRATOR,")).ToList().Count,
										list.Where((string text5) => text5.Contains("FIRE STICK,")).ToList().Count,
										list.Where((string text5) => text5.Contains("AA BADGE,")).ToList().Count,
										list.Where((string text5) => text5.Contains("GT RIBBON,")).ToList().Count,
										list.Where((string text5) => text5.Contains("GOLD PHONE,")).ToList().Count,
										list.Where((string text5) => text5.Contains("MAYHEM STAFF,")).ToList().Count,
										list.Where((string text5) => text5.Contains("PLUSH,")).ToList().Count
									};
									list[0] = string.Join("\n", from x in list[0].Split('\n')
										select x.StartsWith("PLAYERS") ? $"PLAYERS LOGGED: {i[0]} {{ MOD STICK: {i[1]} | FINGER PAINTERS: {i[2]} | ILLUSTRATORS: {i[3]} | FIRE STICKS: {i[4]} | AA BADGES/HATS: {i[5]} | GT RIBBONS: {i[6]} | GOLD PHONE: {i[7]} | MMH STAFF: {i[8]} | GOLD LEMMING PLUSHIES: {i[9]} }}" : x);
									File.WriteAllText("Malachis_Menu_Reborn/Special_Players.txt", string.Join("\n\n", list));
								}
							}
						}
					});
				}
				catch (Exception ex5)
				{
					Exception e2 = ex5;
					Debug.LogError((object)e2);
				}
				if (lea.ToList().Count > 0)
				{
					try
					{
						foreach (Player p3 in lea.ToList())
						{
							if (p3 == own)
							{
								own = null;
							}
							if (modsidedmenus.Keys.ToList().Contains(p3))
							{
								modsidedextras.Remove(p3);
								sidedbuttons.Remove(p3);
								Object.Destroy((Object)(object)modsidedmenus[p3]);
								modsidedmenus.Remove(p3);
							}
							if (Mods.usingmenu.ToList().Contains(p3))
							{
								Mods.usingmenu.Remove(p3);
							}
							if (allcos.Keys.ToList().Contains(p3))
							{
								allcos.Remove(p3);
							}
							if (Mods.usings.ToList().Contains(p3))
							{
								Mods.usings.Remove(p3);
							}
							if (sidedkeyboards.Keys.ToList().Contains(p3))
							{
								Object.Destroy((Object)(GameObject)sidedkeyboards[p3][0]);
								Object.Destroy((Object)(GameObject)sidedkeyboards[p3][1]);
								sidedkeyboards.Remove(p3);
							}
						}
						lea.Clear();
					}
					catch
					{
					}
				}
				if (Mods.usingmenu.Count > 0)
				{
					foreach (Player p4 in Mods.usingmenu)
					{
						if (!RigUtils.AllPlayers().Contains(p4))
						{
							continue;
						}
						try
						{
							VRRig r = RigUtils.RigFromPlayer(p4);
							if (p4 == own && ((Graphic)r.playerText1).color != Color.magenta)
							{
								((Graphic)r.playerText1).color = Color.magenta;
							}
							else if (p4 != own && ((Graphic)r.playerText1).color != Color.cyan)
							{
								((Graphic)r.playerText1).color = Color.cyan;
							}
						}
						catch
						{
						}
					}
					if (modsidedmenus.Keys.Count > 0)
					{
						ModSidedMenu.TrackMenus();
					}
				}
				if (PhotonNetwork.CurrentRoom.IsVisible && Mods.ki)
				{
					CoroutineRunner.Start(Mods.Re());
					Mods.ki = false;
				}
			}
			if (Mods.platl)
			{
				try
				{
					Mods.DoPlatformLabel();
				}
				catch
				{
				}
			}
			if (Mods.monk)
			{
				Mods.DoMonke();
			}
			if (!dgv)
			{
				lgv();
			}
			if (((Behaviour)Mods.OwnRig()).enabled && (Object)(object)Mods.gv != (Object)null && dgv)
			{
				Mods.DisableGV();
			}
			if (!customboards)
			{
				Extra();
				IsOwner();
				TextMeshPro t3 = Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>();
				TextMeshPro t4 = Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/WelcomeToGorilllaTagHeadingText").GetComponent<TextMeshPro>();
				Object.Destroy((Object)(object)Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText"));
				Object.Destroy((Object)(object)Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData"));
				TextMeshPro t5 = Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/HeadOutside").GetComponent<TextMeshPro>();
				TextMeshPro t6 = Mods.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>();
				((TMP_Text)t3).text = "<color=white>" + MenuTitle.ToUpper() + "</color>";
				((TMP_Text)t4).text = "<color=white>LETTER MEANINGS</color>";
				((TMP_Text)t5).richText = true;
				((TMP_Text)t5).text = LetterMeanings.Replace("(", "<size=60>(").Replace(")", ")</size>") + "\n----------------------------------------------\n" + ((Annc == "null") ? "<size=70>NO ANNOUNCEMENT FROM MALACHI</size> <size=60>(THE MENU OWNER)</size>\n<color=grey><size=59>MAKE SURE TO JOIN THE DISCORD SERVER TO STAY UPDATED WITH THE MENU AND COMMUNITY.</size></color>" : ("<color=yellow>ANNOUNCEMENT FROM MALACHI <size=60>(THE MENU OWNER)</size></color>\n" + Annc));
				bool b7 = PhotonNetwork.IsConnectedAndReady || Mods.CosDone();
				string text;
				if (check)
				{
					if (trys >= 6)
					{
						text = "<color=red>FAILED CHECK. LOOK AT COC BOARD FOR INFO</color>";
					}
					else
					{
						text = (owner ? "<color=#FF00FF>OWNER</color>" : "<color=#00FFFF>NORMAL USER</color>");
					}
				}
				else
				{
					text = "<color=white>CHECKING STATUS NOW...</color>";
				}
				string mes = "\n<color=white>MENU USER STATUS: </color>" + text;
				string motd = "<color=white>THANKS FOR CHOOSING MY MENU, MORE TO COME SOON\nJOIN MY DISCORD SERVER: </color><color=yellow>DISCORD.GG/2AmFSzYeAV</color>\n<color=red>I (MALACHI) AM NOT RESPONSIBLE FOR ANY BANS</color>\n<color=white>PLS LOOK AT THE TOOLTIPS THEY ARE THERE FOR A REASON</color>\n\n<color=#00FFFF>CYAN NAME TAG</color><color=white> = USING THE MENU ASWELL</color>\n<color=#FF00FF>PINK NAME TAG</color><color=white> = OWNER OF THE MENU</color>\n\n<color=grey>BTW THE MENU IS PRONOUCED MAL-UH-KAI</color>\n<color=yellow>!!!</color><color=white> IF YOU PAID FOR THIS MENU, YOU GOT SCAMMED </color><color=yellow>!!!</color>";
				((TMP_Text)((Component)t6).GetComponent<TextMeshPro>()).text = motd + "\n\n<color=white>ACCOUNT STATUS: </color><color=" + (b7 ? "green>UNBANNED" : "red>BANNED OR TRYING TO CONNECT TO PHOTON") + "</color>" + mes;
				customboards = b7 && check;
			}
			if (!customboards || coss)
			{
				return;
			}
			if (!cus[0].Equals(ss + GitVersion()))
			{
				cus[0] = ss + GitVersion();
			}
			if (!usingm && !owner && trys < 6)
			{
				Player val = Mods.Local();
				Hashtable val2 = new Hashtable();
				((Dictionary<object, object>)val2).Add(cus[0], cus[1]);
				val.SetCustomProperties(val2, (Hashtable)null, (WebFlags)null);
				usingm = true;
			}
			if (!Mods.CosDone() || coss)
			{
				return;
			}
			if (!allcos.ContainsKey(Mods.Local()))
			{
				allcos.Add(Mods.Local(), Mods.Create(Mods.OwnRig()).Field(Mods.Field(0)).GetValue<HashSet<string>>());
			}
			new Mods.Delay().D(0.9f, () =>
			{
				List<CosmeticItem> list = new List<CosmeticItem>();
				List<CosmeticItem> list2 = new List<CosmeticItem>();
				List<CosmeticItem> list3 = new List<CosmeticItem>();
				List<CosmeticItem> list4 = new List<CosmeticItem>();
				List<CosmeticItem> list5 = new List<CosmeticItem>();
				CosmeticsController val3 = Mods.Con();
				List<CosmeticItem> allCosmetics = val3.allCosmetics;
				foreach (CosmeticItem item4 in allCosmetics)
				{
					if (!list4.Contains(item4) && item4.canTryOn && (int)item4.itemCategory == 24 && !Mods.GetCos(Mods.OwnRig()).Contains(item4.itemName))
					{
						list4.Add(item4);
					}
				}
				foreach (CosmeticItem item5 in allCosmetics)
				{
					if (item5.cost == 0 && !list2.Contains(item5) && item5.canTryOn && !Mods.GetCos(Mods.OwnRig()).Contains(item5.itemName))
					{
						list2.Add(item5);
					}
				}
				foreach (CosmeticItem item6 in list4)
				{
					string[] bundledItems = item6.bundledItems;
					foreach (string text2 in bundledItems)
					{
						if (!list5.Contains(Mods.Cos(text2)))
						{
							list5.Add(Mods.Cos(text2));
						}
					}
				}
				foreach (CosmeticItem item7 in allCosmetics)
				{
					if (item7.cost == 0 && !list.Contains(item7) && !list3.Contains(item7) && list2.Contains(item7) && !list5.Contains(item7) && !item7.isNullItem && item7.canTryOn && !Mods.GetCos(Mods.OwnRig()).Contains(item7.itemName))
					{
						list.Add(item7);
					}
				}
				if (list.Count < list2.Count || list.Count == list2.Count)
				{
					foreach (CosmeticItem item8 in list)
					{
						if (!list3.Contains(item8))
						{
							val3.itemToBuy = item8;
							val3.itemToPurchase = item8.itemName;
							val3.PurchaseItem();
							if ((int)val3.currentPurchaseItemStage == 7)
							{
								val3.itemToBuy = val3.nullItem;
								val3.itemToPurchase = "";
								list3.Add(item8);
							}
							else if ((int)val3.currentPurchaseItemStage == 6)
							{
								val3.UpdateWardrobeModelsAndButtons();
								val3.UpdateMyCosmetics();
								list3.Add(item8);
							}
						}
					}
					foreach (CosmeticItem item9 in list3)
					{
						if (list.Contains(item9))
						{
							list.Remove(item9);
						}
					}
					if (list.Count == 0)
					{
						coss = true;
					}
				}
			});
		}
		catch (Exception ex6)
		{
			Debug.Log((object)("Update: " + ex6));
		}
	}

	public static async void lgv()
	{
		await Task.Delay(8000);
		try
		{
			if ((((Behaviour)Mods.OwnRig()).isActiveAndEnabled || ((Component)Mods.OwnRig()).gameObject.activeSelf) && (Object)(object)Mods.gv == (Object)null)
			{
				GameObject g = new GameObject("g");
				g.SetActive(false);
				Mods.gv = Object.Instantiate<VRRig>(Mods.OwnRig(), ((Component)Mods.Loco()).transform.position, ((Component)Mods.Loco()).transform.rotation, g.transform);
				Mods.gv.headBodyOffset = Vector3.zero;
				((Component)Mods.gv).gameObject.SetActive(false);
				((Component)Mods.gv).transform.SetParent(((Component)Mods.OwnRig()).transform.parent);
				Object.Destroy((Object)(object)g);
				((Component)((Component)Mods.gv).transform.Find("VR Constraints/LeftArm/Left Arm IK/SlideAudio")).gameObject.SetActive(false);
				((Component)((Component)Mods.gv).transform.Find("VR Constraints/RightArm/Right Arm IK/SlideAudio")).gameObject.SetActive(false);
				((Component)((Component)Mods.gv).transform.Find("rig/body_pivot/SlideAudio")).gameObject.SetActive(false);
			}
			if ((Object)(object)Mods.gv != (Object)null)
			{
				dgv = true;
			}
		}
		catch
		{
		}
	}

	public static float Version()
	{
		string text = MenuTitle.Substring(22);
		return float.Parse((text.Count((char c2) => c2 == '.') > 1) ? text.Remove(text.LastIndexOf('.'), 1) : text);
	}

	public static string GitVersion()
	{
		string text = ver.ToString();
		int num = text.IndexOf('.');
		if (num != -1 && num + 2 < text.Length)
		{
			text = text.Insert(num + 2, ".");
		}
		return text;
	}

	public static bool IsUsing(Player p, out string PhotonNetworkRunViewUpdatePatch)
	{
		if (RigUtils.AllPlayers().Contains(p))
		{
			Hashtable customProperties = p.CustomProperties;
			if (ownids.Contains(p.UserId))
			{
				if (!Mods.usingmenu.Contains(p))
				{
					Mods.usingmenu.Add(p);
				}
				own = p;
				PhotonNetworkRunViewUpdatePatch = "<color=#FF00FF>";
				return true;
			}
			if (((Dictionary<object, object>)(object)customProperties).ContainsKey(cus[0]) && ((Dictionary<object, object>)(object)customProperties).ContainsValue(cus[1]))
			{
				if (!Mods.usingmenu.Contains(p))
				{
					Mods.usingmenu.Add(p);
				}
				PhotonNetworkRunViewUpdatePatch = "<color=#00FFFF>";
				return true;
			}
		}
		PhotonNetworkRunViewUpdatePatch = "<color=white>";
		return false;
	}

	public static void Extra()
	{
		if (check1)
		{
			return;
		}
		new Mods.Delay().D(1f, () =>
		{
			try
			{
				string[] array = (from x in GitString("ids.txt").Split(new string[1] { "\n" }, StringSplitOptions.None)
					where !string.IsNullOrWhiteSpace(x)
					select x).ToArray();
				if (array.Length != 0)
				{
					ownids = array.Skip(3).ToArray();
					Annc = array[1];
					ver = float.Parse(array[0]);
					string[] n = array[2].Split(':');
					if (array[2] != "null")
					{
						CatButtons8.Add(new ButtonInfo
						{
							buttonText = "Start LiveEvent (CS)",
							method = () =>
							{
								if (RigUtils.Map(Mods.OwnRig()) == n[1].Trim())
								{
									GorillaEventAnimationController component = Mods.Find(n[0].Trim()).GetComponent<GorillaEventAnimationController>();
									if (Mods.Create(component).Field(Mods.Field(79)).GetValue<bool>())
									{
										Mods.SendNoti(Mods.Er + "Live Event Is Already Playing!");
									}
									else
									{
										component.StartPlaying();
									}
								}
								else
								{
									Mods.SendNoti(Mods.Er + "You Must Be In The " + n[1].Trim() + " Map!");
								}
								Mods.DisableButton("Start LiveEvent (CS)");
							},
							enabled = false,
							toolTip = "Starts the live event early",
							n = "n"
						});
					}
					check1 = true;
				}
			}
			catch
			{
				trys++;
			}
		});
		if (trys >= 6)
		{
			Annc = "<color=red>FAILED TO GET ANNOUNCEMENT! MAKE SURE YOUR WIFI ISNT BLOCKING GITHUB REQUESTS, OR JUST CHECK THE DISCORD SERVER FOR THE ANNOUNCEMENT!</color>";
			owner = false;
			check = true;
			ownids = new string[1] { "hi" };
			check1 = true;
		}
	}

	public static void IsOwner()
	{
		if (!check && !string.IsNullOrEmpty(Mods.Local().UserId) && ownids.Count() != 0)
		{
			owner = ownids.Contains(Mods.Local().UserId);
			check = true;
		}
	}

	public static string GitString(string u)
	{
		return new WebClient().DownloadString("https://raw.githubusercontent.com/Malachi-the-modder/ownerids/main/" + u);
	}

	private static string GetButtonTooltip(int i)
	{
		ButtonInfo buttonInfo = Mods.LocalCB()[i];
		return ((buttonInfo.n == "" || buttonInfo.n == "n") ? buttonInfo.buttonText : buttonInfo.n) + ": " + buttonInfo.toolTip;
	}

	public void Draw()
	{
		menu = Mods.CreatePrimitive((PrimitiveType)3);
		Object.Destroy((Object)(object)menu.GetComponent<Rigidbody>());
		Object.Destroy((Object)(object)menu.GetComponent<BoxCollider>());
		Object.Destroy((Object)(object)menu.GetComponent<Renderer>());
		menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f);
		menuObj = Mods.CreatePrimitive((PrimitiveType)3);
		Object.Destroy((Object)(object)menuObj.GetComponent<Rigidbody>());
		Object.Destroy((Object)(object)menuObj.GetComponent<BoxCollider>());
		Transform transform = menuObj.transform;
		transform.parent = menu.transform;
		transform.rotation = Quaternion.identity;
		if (Mods.change20 == 1)
		{
			transform.localScale = new Vector3(0.1f, 1f, 1f) * 1f;
		}
		if (Mods.change20 == 2)
		{
			transform.localScale = new Vector3(0.1f, 1f, 1.3f) * 1f;
		}
		if (Mods.change20 == 3)
		{
			transform.localScale = new Vector3(0.1f, 1.25f, 1f) * 1f;
		}
		if (Mods.change20 == 4)
		{
			transform.localScale = new Vector3(0.1f, 1.25f, 1.3f) * 1f;
		}
		Gradient val = new Gradient();
		if (Mods.change10 == 1)
		{
			val = Mods.RainbowG();
		}
		else
		{
			GradientColorKey[] array = new GradientColorKey[4];
			array[0].color = FirstColor;
			array[0].time = 0f;
			array[1].color = FirstColor;
			array[1].time = 0.3f;
			array[2].color = SecondColor;
			array[2].time = 0.6f;
			array[3].color = FirstColor;
			array[3].time = 1f;
			val = new Gradient
			{
				colorKeys = array
			};
		}
		ColorChanger colorChanger = menuObj.AddComponent<ColorChanger>();
		colorChanger.colors = val;
		colorChanger.Start();
		if (Mods.change10 == 0)
		{
			Object.Destroy((Object)(object)menuObj.GetComponent<Renderer>());
			Object.Destroy((Object)(object)menuObj.GetComponent<ColorChanger>());
		}
		transform.position = new Vector3(0.05f, 0f, 0f) * 1f;
		canvasObj = new GameObject();
		canvasObj.transform.parent = menu.transform;
		Canvas val2 = canvasObj.AddComponent<Canvas>();
		CanvasScaler val3 = canvasObj.AddComponent<CanvasScaler>();
		canvasObj.AddComponent<GraphicRaycaster>();
		val2.renderMode = (RenderMode)2;
		val3.dynamicPixelsPerUnit = 2800f;
		GameObject val4 = new GameObject();
		val4.transform.parent = canvasObj.transform;
		Text val5 = val4.AddComponent<Text>();
		((Object)((Component)val5).gameObject).name = "name";
		titiel = val5;
		val5.font = MenuFont;
		val5.text = MenuTitle;
		val5.fontSize = 1;
		val5.alignment = (TextAnchor)4;
		((Graphic)val5).color = MenuTC;
		val5.resizeTextForBestFit = true;
		val5.resizeTextMinSize = 0;
		RectTransform component = ((Component)val5).GetComponent<RectTransform>();
		((Transform)component).localPosition = Vector3.zero;
		component.sizeDelta = new Vector2(0.28f, 0.05f);
		if ((Mods.change20 == 2) | (Mods.change20 == 4))
		{
			((Transform)component).position = new Vector3(0.06f, 0f, 0.225f);
		}
		if ((Mods.change20 == 1) | (Mods.change20 == 3))
		{
			((Transform)component).position = new Vector3(0.06f, 0f, 0.1751f);
		}
		((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
		GameObject val6 = new GameObject();
		val6.transform.parent = canvasObj.transform;
		Text val7 = val6.AddComponent<Text>();
		((Object)((Component)val7).gameObject).name = "creds";
		val7.font = MenuFont;
		val7.text = fcc;
		val7.fontSize = 4;
		val7.alignment = (TextAnchor)4;
		((Graphic)val7).color = MenuTC;
		val7.resizeTextForBestFit = true;
		val7.resizeTextMinSize = 0;
		RectTransform component2 = ((Component)val7).GetComponent<RectTransform>();
		((Transform)component2).localPosition = Vector3.zero;
		component2.sizeDelta = new Vector2(0.28f, 0.05f);
		((Transform)component2).position = new Vector3(0.03f, 0f, 0f);
		((Transform)component2).rotation = Quaternion.Euler(new Vector3(0f, 90f, 90f));
		AddPageButtons();
		string[] array2 = (from buttonInfo in Mods.LocalCB().Skip(pageNumber * pageSize).Take(pageSize)
			select buttonInfo.buttonText).ToArray();
		for (int num = 0; num < array2.Length; num++)
		{
			AddButton((float)num * 0.13f + 0.26f, array2[num]);
		}
		GameObject val8 = new GameObject();
		val8.transform.SetParent(canvasObj.transform);
		val8.transform.localPosition = new Vector3(0f, 0f, 1f) * 1f;
		tooltipText = val8.GetComponent<Text>();
		if ((Object)(object)tooltipText == (Object)null)
		{
			tooltipText = val8.AddComponent<Text>();
		}
		tooltipText.font = MenuFont;
		tooltipText.text = tooltipString;
		tooltipText.fontSize = 20;
		tooltipText.alignment = (TextAnchor)4;
		tooltipText.resizeTextForBestFit = true;
		tooltipText.resizeTextMinSize = 0;
		((Graphic)tooltipText).color = MenuTC;
		RectTransform component3 = val8.GetComponent<RectTransform>();
		((Transform)component3).localPosition = Vector3.zero;
		component3.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
		if ((Mods.change20 == 1) | (Mods.change20 == 3))
		{
			((Transform)component3).position = new Vector3(0.06f, 0f, -0.18f) * 1f;
		}
		if ((Mods.change20 == 2) | (Mods.change20 == 4))
		{
			((Transform)component3).position = new Vector3(0.06f, 0f, -0.23f) * 1f;
		}
		((Transform)component3).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
	}

	private static void AddPageButtons()
	{
		if (Mods.change7 < 5)
		{
			GameObject val = Mods.CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val.GetComponent<Rigidbody>());
			((Collider)val.GetComponent<BoxCollider>()).isTrigger = true;
			val.transform.parent = menu.transform;
			val.layer = ((!Mods.menucol) ? 2 : 0);
			val.transform.rotation = Quaternion.identity;
			val.AddComponent<BtnCollider>().relatedText = "Prev";
			val.GetComponent<Renderer>().material.color = NPCo;
			GameObject val2 = new GameObject();
			val2.transform.parent = canvasObj.transform;
			Text val3 = val2.AddComponent<Text>();
			val3.font = MenuFont;
			val3.text = arrow[0].ToString();
			val3.fontSize = 1;
			val3.alignment = (TextAnchor)4;
			val3.resizeTextForBestFit = true;
			((Graphic)val3).color = NPTC;
			val3.resizeTextMinSize = 0;
			RectTransform component = ((Component)val3).GetComponent<RectTransform>();
			((Transform)component).localPosition = Vector3.zero;
			component.sizeDelta = new Vector2(0.2f, 0.03f);
			((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			GameObject val4 = Mods.CreatePrimitive((PrimitiveType)3);
			Object.Destroy((Object)(object)val4.GetComponent<Rigidbody>());
			((Collider)val4.GetComponent<BoxCollider>()).isTrigger = true;
			val4.transform.parent = menu.transform;
			val4.layer = ((!Mods.menucol) ? 2 : 0);
			val4.transform.rotation = Quaternion.identity;
			val4.AddComponent<BtnCollider>().relatedText = "Next";
			val4.GetComponent<Renderer>().material.color = NPCo;
			GameObject val5 = new GameObject();
			val5.transform.parent = canvasObj.transform;
			Text val6 = val5.AddComponent<Text>();
			val6.font = MenuFont;
			val6.text = arrow[2].ToString();
			val6.fontSize = 1;
			val6.alignment = (TextAnchor)4;
			val6.resizeTextForBestFit = true;
			((Graphic)val6).color = NPTC;
			val6.resizeTextMinSize = 0;
			RectTransform component2 = ((Component)val6).GetComponent<RectTransform>();
			((Transform)component2).localPosition = Vector3.zero;
			component2.sizeDelta = new Vector2(0.2f, 0.03f);
			((Transform)component2).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			if (Mods.change7 == 1)
			{
				float num = 0f;
				val.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
				val.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num);
				((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.111f - num / 2.55f);
				num = 0.13f;
				val4.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
				val4.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num);
				((Transform)component2).localPosition = new Vector3(0.064f, 0f, 0.111f - num / 2.55f);
				pageSize = 4;
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					pageSize = 6;
					val.transform.localPosition = new Vector3(0.56f, 0f, 0.45f);
					((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.17701961f);
					val4.transform.localPosition = new Vector3(0.56f, 0f, 0.45f - num);
					((Transform)component2).localPosition = new Vector3(0.064f, 0f, 0.128f);
				}
				if ((Mods.change20 == 3) | (Mods.change20 == 4))
				{
					val.transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
					val4.transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
				}
			}
			if (Mods.change7 == 2)
			{
				val.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val.transform.localPosition = new Vector3(0.56f, 0.37f, 0.541f);
				((Transform)component).localPosition = new Vector3(0.064f, 0.11f, 0.215f);
				val4.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val4.transform.localPosition = new Vector3(0.56f, -0.37f, 0.541f);
				((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, 0.215f);
				pageSize = 6;
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					pageSize = 8;
					val.transform.localPosition = new Vector3(0.56f, 0.37f, 0.71f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.11f, 0.28f);
					val4.transform.localPosition = new Vector3(0.56f, -0.37f, 0.71f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, 0.28f);
				}
			}
			if (Mods.change7 == 3)
			{
				val.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
				val.transform.localPosition = new Vector3(0.56f, 0.657f, 0.0063f);
				((Transform)component).localPosition = new Vector3(0.064f, 0.2f, 0.0063f);
				val4.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
				val4.transform.localPosition = new Vector3(0.56f, -0.657f, 0.0063f);
				((Transform)component2).localPosition = new Vector3(0.064f, -0.2f, 0.0063f);
				pageSize = 6;
				if ((Mods.change20 == 3) | (Mods.change20 == 4))
				{
					val.transform.localPosition = new Vector3(0.56f, 0.8f, 0.0063f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.244f, 0.0063f);
					val4.transform.localPosition = new Vector3(0.56f, -0.8f, 0.0063f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.244f, 0.0063f);
				}
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					pageSize = 8;
				}
			}
			if (Mods.change7 == 4)
			{
				val.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val.transform.localPosition = new Vector3(0.56f, 0.37f, -0.541f);
				((Transform)component).localPosition = new Vector3(0.064f, 0.11f, -0.215f);
				val4.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
				val4.transform.localPosition = new Vector3(0.56f, -0.37f, -0.541f);
				((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, -0.215f);
				pageSize = 6;
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					pageSize = 8;
					val.transform.localPosition = new Vector3(0.56f, 0.37f, -0.71f);
					((Transform)component).localPosition = new Vector3(0.064f, 0.11f, -0.28f);
					val4.transform.localPosition = new Vector3(0.56f, -0.37f, -0.71f);
					((Transform)component2).localPosition = new Vector3(0.064f, -0.11f, -0.28f);
				}
			}
			if (Mods.change31 == 0 && (Object)(object)val4.GetComponent<Renderer>() != (Object)null)
			{
				Object.Destroy((Object)(object)val.GetComponent<Renderer>());
				Object.Destroy((Object)(object)val4.GetComponent<Renderer>());
			}
			if (Mods.change31 == 1)
			{
				ColorChanger colorChanger = val.AddComponent<ColorChanger>();
				colorChanger.colors = Mods.RainbowG();
				colorChanger.Start();
				ColorChanger colorChanger2 = val4.AddComponent<ColorChanger>();
				colorChanger2.colors = Mods.RainbowG();
				colorChanger2.Start();
			}
		}
		if (Mods.change7 == 5 || Mods.change7 == 6)
		{
			pageSize = 6;
			if ((Mods.change20 == 2) | (Mods.change20 == 4))
			{
				pageSize = 8;
			}
		}
		if (Mods.dis)
		{
			GameObject val7 = Mods.CreatePrimitive((PrimitiveType)3);
			((Object)val7).name = "dis";
			Object.Destroy((Object)(object)val7.GetComponent<Rigidbody>());
			((Collider)val7.GetComponent<BoxCollider>()).isTrigger = true;
			val7.transform.parent = menu.transform;
			val7.layer = ((!Mods.menucol) ? 2 : 0);
			val7.transform.rotation = Quaternion.identity;
			val7.AddComponent<BtnCollider>().relatedText = "Dis";
			val7.GetComponent<Renderer>().material.color = DICo;
			GameObject val8 = new GameObject
			{
				name = "dt"
			};
			val8.transform.parent = canvasObj.transform;
			Text val9 = val8.AddComponent<Text>();
			val9.font = MenuFont;
			val9.text = "Disconnect";
			val9.fontSize = 1;
			val9.alignment = (TextAnchor)4;
			val9.resizeTextForBestFit = true;
			val9.resizeTextMinSize = 0;
			((Graphic)val9).color = DITCo;
			RectTransform component3 = ((Component)val9).GetComponent<RectTransform>();
			((Transform)component3).localPosition = Vector3.zero;
			component3.sizeDelta = new Vector2(0.2f, 0.03f);
			((Transform)component3).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			if (Mods.change4 == 1)
			{
				float num2 = 0.26f;
				val7.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
				val7.transform.localPosition = new Vector3(0.56f, -0.8f, 0.35f - num2);
				((Transform)component3).localPosition = new Vector3(0.06f, -0.24f, 0.14f - num2 / 2.55f);
				if (Mods.change7 == 3)
				{
					val7.transform.localPosition = new Vector3(0.56f, -1.1f, 0.35f - num2);
					((Transform)component3).localPosition = new Vector3(0.06f, -0.33f, 0.14f - num2 / 2.55f);
				}
				if ((Mods.change20 == 3) | (Mods.change20 == 4))
				{
					val7.transform.localPosition = new Vector3(0.56f, -0.95f, 0.35f - num2);
					((Transform)component3).localPosition = new Vector3(0.06f, -0.281f, 0.14f - num2 / 2.55f);
				}
				if (((Mods.change20 == 3) | (Mods.change20 == 4)) && Mods.change7 == 3)
				{
					val7.transform.localPosition = new Vector3(0.56f, -1.25f, 0.35f - num2);
					((Transform)component3).localPosition = new Vector3(0.06f, -0.371f, 0.14f - num2 / 2.55f);
				}
			}
			if (Mods.change4 == 2)
			{
				float num3 = 0.26f;
				val7.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
				val7.transform.localPosition = new Vector3(0.56f, 0.8f, 0.35f - num3);
				((Transform)component3).localPosition = new Vector3(0.06f, 0.24f, 0.14f - num3 / 2.55f);
				if (Mods.change7 == 3)
				{
					val7.transform.localPosition = new Vector3(0.56f, 1.1f, 0.35f - num3);
					((Transform)component3).localPosition = new Vector3(0.06f, 0.33f, 0.14f - num3 / 2.55f);
				}
				if ((Mods.change20 == 3) | (Mods.change20 == 4))
				{
					val7.transform.localPosition = new Vector3(0.56f, 0.95f, 0.35f - num3);
					((Transform)component3).localPosition = new Vector3(0.06f, 0.281f, 0.14f - num3 / 2.55f);
				}
				if (((Mods.change20 == 3) | (Mods.change20 == 4)) && Mods.change7 == 3)
				{
					val7.transform.localPosition = new Vector3(0.56f, 1.25f, 0.35f - num3);
					((Transform)component3).localPosition = new Vector3(0.06f, 0.371f, 0.14f - num3 / 2.55f);
				}
			}
			if (Mods.change4 == 3)
			{
				val7.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
				val7.transform.localPosition = new Vector3(0.56f, 0f, 0.6f);
				((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.24f);
				if (Mods.change7 == 2)
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, 0.7f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.28f);
				}
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, 0.76f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.305f);
				}
				if (((Mods.change20 == 2) | (Mods.change20 == 4)) && Mods.change7 == 2)
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, 0.83f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, 0.33f);
				}
			}
			if (Mods.change4 == 4)
			{
				val7.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
				val7.transform.localPosition = new Vector3(0.56f, 0f, -0.6f);
				((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.24f);
				if (Mods.change7 == 4)
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, -0.7f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.28f);
				}
				if ((Mods.change20 == 2) | (Mods.change20 == 4))
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, -0.76f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.305f);
				}
				if (((Mods.change20 == 2) | (Mods.change20 == 4)) && Mods.change7 == 4)
				{
					val7.transform.localPosition = new Vector3(0.56f, 0f, -0.83f);
					((Transform)component3).localPosition = new Vector3(0.064f, 0f, -0.33f);
				}
			}
			if (Mods.change29 == 0 && (Object)(object)val7.GetComponent<Renderer>() != (Object)null)
			{
				Object.Destroy((Object)(object)val7.GetComponent<Renderer>());
			}
			if (Mods.change29 == 1)
			{
				ColorChanger colorChanger3 = val7.AddComponent<ColorChanger>();
				colorChanger3.colors = Mods.RainbowG();
				colorChanger3.Start();
			}
		}
		if (Mods.searchB && (!Mods.searching || Mods.actsearch))
		{
			GameObject val10 = Mods.CreatePrimitive((PrimitiveType)3);
			((Object)val10).name = "search";
			Object.Destroy((Object)(object)val10.GetComponent<Rigidbody>());
			((Collider)val10.GetComponent<BoxCollider>()).isTrigger = true;
			val10.transform.parent = menu.transform;
			val10.layer = ((!Mods.menucol) ? 2 : 0);
			val10.transform.rotation = Quaternion.identity;
			val10.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
			val10.transform.localPosition = new Vector3(0.56f, -0.657f, 0.53f);
			val10.AddComponent<BtnCollider>().relatedText = "sear";
			val10.GetComponent<Renderer>().material.color = Searchb;
			GameObject val11 = new GameObject();
			val11.transform.parent = canvasObj.transform;
			Text val12 = val11.AddComponent<Text>();
			val12.font = MenuFont;
			val12.text = "Search";
			val12.fontSize = 1;
			val12.alignment = (TextAnchor)4;
			val12.resizeTextForBestFit = true;
			val12.resizeTextMinSize = 0;
			((Graphic)val12).color = SearchT;
			RectTransform component4 = ((Component)val12).GetComponent<RectTransform>();
			((Transform)component4).localPosition = Vector3.zero;
			component4.sizeDelta = new Vector2(0.18f, 0.02f);
			((Transform)component4).localPosition = new Vector3(0.064f, -0.195f, 0.212f);
			((Transform)component4).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			if ((Mods.change20 == 3) | (Mods.change20 == 4))
			{
				val10.transform.localPosition = new Vector3(0.56f, -0.8f, 0.53f);
				((Transform)component4).localPosition = new Vector3(0.064f, -0.24f, 0.212f);
			}
			if (Mods.change34 == 0 && (Object)(object)val10.GetComponent<Renderer>() != (Object)null)
			{
				Object.Destroy((Object)(object)val10.GetComponent<Renderer>());
			}
			if (Mods.change34 == 1)
			{
				ColorChanger colorChanger4 = val10.AddComponent<ColorChanger>();
				colorChanger4.colors = Mods.RainbowG();
				colorChanger4.Start();
			}
		}
		if (Mods.homeB && !Mods.InMain() && pageNumber != 0 && !Mods.searching)
		{
			GameObject val13 = Mods.CreatePrimitive((PrimitiveType)3);
			((Object)val13).name = "home";
			Object.Destroy((Object)(object)val13.GetComponent<Rigidbody>());
			((Collider)val13.GetComponent<BoxCollider>()).isTrigger = true;
			val13.transform.parent = menu.transform;
			val13.layer = ((!Mods.menucol) ? 2 : 0);
			val13.transform.rotation = Quaternion.identity;
			val13.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
			val13.transform.localPosition = new Vector3(0.56f, -0.657f, Mods.searchB ? 0.617f : 0.53f);
			val13.AddComponent<BtnCollider>().relatedText = "Home";
			val13.GetComponent<Renderer>().material.color = Homeb;
			GameObject val14 = new GameObject();
			val14.transform.parent = canvasObj.transform;
			Text val15 = val14.AddComponent<Text>();
			val15.font = MenuFont;
			val15.text = "Home";
			val15.fontSize = 1;
			val15.alignment = (TextAnchor)4;
			val15.resizeTextForBestFit = true;
			val15.resizeTextMinSize = 0;
			((Graphic)val15).color = HomeT;
			RectTransform component5 = ((Component)val15).GetComponent<RectTransform>();
			((Transform)component5).localPosition = Vector3.zero;
			component5.sizeDelta = new Vector2(0.18f, 0.02f);
			((Transform)component5).localPosition = new Vector3(0.064f, -0.195f, Mods.searchB ? 0.245f : 0.212f);
			((Transform)component5).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
			if ((Mods.change20 == 3) | (Mods.change20 == 4))
			{
				val13.transform.localPosition = new Vector3(0.56f, -0.8f, Mods.searchB ? 0.617f : 0.53f);
				((Transform)component5).localPosition = new Vector3(0.064f, -0.24f, Mods.searchB ? 0.245f : 0.212f);
			}
			if (Mods.change42 == 0 && (Object)(object)val13.GetComponent<Renderer>() != (Object)null)
			{
				Object.Destroy((Object)(object)val13.GetComponent<Renderer>());
			}
			if (Mods.change42 == 1)
			{
				ColorChanger colorChanger5 = val13.AddComponent<ColorChanger>();
				colorChanger5.colors = Mods.RainbowG();
				colorChanger5.Start();
			}
		}
	}

	public static void DestroyMenu()
	{
		Object.Destroy((Object)(object)menu);
		Object.Destroy((Object)(object)canvasObj);
		Object.Destroy((Object)(object)reference);
		reference = null;
		menu = null;
		canvasObj = null;
	}

	private static void AddButton(float offset, string text)
	{
		if (text == "Keyboard")
		{
			return;
		}
		Button = Mods.CreatePrimitive((PrimitiveType)3);
		Object.Destroy((Object)(object)Button.GetComponent<Rigidbody>());
		((Collider)Button.GetComponent<BoxCollider>()).isTrigger = true;
		Button.transform.parent = menu.transform;
		Button.transform.rotation = Quaternion.identity;
		Button.layer = ((!Mods.menucol) ? 2 : 0);
		Button.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
		Transform transform = Button.transform;
		if ((Mods.change20 == 3) | (Mods.change20 == 4))
		{
			transform.localScale = new Vector3(0.09f, 1.05f, 0.08f);
		}
		if (Mods.change7 == 1)
		{
			transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);
		}
		if ((Mods.change7 == 2) | (Mods.change7 == 3) | (Mods.change7 == 4) | (Mods.change7 == 5) | (Mods.change7 == 6))
		{
			transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset);
		}
		if ((Mods.change20 == 2) | (Mods.change20 == 4))
		{
			transform.localPosition = new Vector3(0.56f, 0f, 0.7f - offset);
		}
		if (((Mods.change20 == 2) | (Mods.change20 == 4)) && Mods.change7 == 1)
		{
			transform.localPosition = new Vector3(0.56f, 0f, 0.48f - offset);
		}
		Button.AddComponent<BtnCollider>().relatedText = text;
		int index = -1;
		List<ButtonInfo> list = Mods.LocalCB();
		for (int i = 0; i < list.Count; i++)
		{
			if (text == list[i].buttonText)
			{
				index = i;
				break;
			}
		}
		GameObject val = new GameObject();
		val.transform.parent = canvasObj.transform;
		text2 = val.AddComponent<Text>();
		text2.font = MenuFont;
		text2.text = text;
		text2.fontSize = 1;
		text2.alignment = (TextAnchor)4;
		text2.resizeTextForBestFit = true;
		text2.resizeTextMinSize = 0;
		RectTransform component = ((Component)text2).GetComponent<RectTransform>();
		((Transform)component).localPosition = Vector3.zero;
		component.sizeDelta = new Vector2(((Mods.change20 == 3) | (Mods.change20 == 4)) ? 0.27f : 0.2f, 0.03f) * 1f;
		if (Mods.change7 == 1)
		{
			((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.111f - offset / 2.55f);
		}
		if ((Mods.change7 == 2) | (Mods.change7 == 3) | (Mods.change7 == 4) | (Mods.change7 == 5) | (Mods.change7 == 6))
		{
			((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.237f - offset / 2.55f);
		}
		if ((Mods.change20 == 2) | (Mods.change20 == 4))
		{
			((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.275f - offset / 2.55f);
		}
		if (((Mods.change20 == 2) | (Mods.change20 == 4)) && Mods.change7 == 1)
		{
			((Transform)component).localPosition = new Vector3(0.064f, 0f, 0.19f - offset / 2.55f);
		}
		((Transform)component).rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
		Renderer component2 = Button.GetComponent<Renderer>();
		if (Mods.inCat1)
		{
			if (!Mods.searching)
			{
				ButtonInfo buttonInfo = CatButtons1[index];
				if (!Mods.IsEnabled(buttonInfo))
				{
					bool flag = buttonInfo.buttonText.Contains("Exit") || buttonInfo.buttonText == "Disable All Mods" || buttonInfo.buttonText == "Save Mods";
					component2.material.color = (flag ? ButtonColorDisable : ButtonColorEnabled);
					((Graphic)text2).color = (flag ? DIsableTextColor : EnableTextColor);
					if (flag ? (Mods.change12 == 0) : (Mods.change13 == 0))
					{
						Object.Destroy((Object)(object)component2);
					}
				}
			}
			else
			{
				bool flag2 = Mods.IsEnabled(CatButtons11[index]);
				component2.material.color = (flag2 ? ButtonColorEnabled : ButtonColorDisable);
				((Graphic)text2).color = (flag2 ? EnableTextColor : DIsableTextColor);
				if (flag2 ? (Mods.change13 == 0) : (Mods.change12 == 0))
				{
					Object.Destroy((Object)(object)component2);
				}
			}
		}
		else
		{
			bool flag3 = Mods.IsEnabled(list[index]);
			component2.material.color = (flag3 ? ButtonColorEnabled : ButtonColorDisable);
			((Graphic)text2).color = (flag3 ? EnableTextColor : DIsableTextColor);
			if (flag3 ? (Mods.change13 == 0) : (Mods.change12 == 0))
			{
				Object.Destroy((Object)(object)component2);
			}
		}
	}

	public static void l()
	{
		bll = new Dictionary<string, List<ButtonInfo>>();
		foreach (ButtonInfo allButton in Mods.GetAllButtons(i: true, ii: false, c: true))
		{
			if (!bll.TryGetValue(allButton.buttonText, out var value))
			{
				value = new List<ButtonInfo>();
				bll.Add(allButton.buttonText, value);
			}
			value.Add(allButton);
		}
	}

	public void Start()
	{
		Draw();
		PhotonNetworkRunViewUpdatePatch.o += O;
		l();
		PhotonNetwork.NetworkingClient.EventReceived += EventReceived;
	}

	public static void Refresh()
	{
		DestroyMenu();
		instance.Draw();
	}

	public static void Toggle(string r)
	{
		string text = "Next";
		string text2 = "Prev";
		string n = "Keyboard";
		if (r == "sear")
		{
			if ((Object)(object)Mods.keyboard != (Object)null && !Mods.IsEnabled(n))
			{
				return;
			}
			bool? enabled = Mods.GetButtonO(n).enabled;
			Mods.GetButtonO(n).enabled = !enabled;
			Mods.doser = true;
			DestroyMenu();
			if (enabled == true)
			{
				Mods.DeleteBoard();
			}
			else
			{
				instance.Draw();
			}
		}
		if (r == "Home")
		{
			if (Mods.inSettings && Mods.autosave)
			{
				Mods.Save(n: false);
			}
			Mods.inSettings = false;
			Mods.inCat1 = false;
			Mods.inCat2 = false;
			Mods.inCat3 = false;
			Mods.inCat4 = false;
			Mods.inCat5 = false;
			Mods.inCat6 = false;
			Mods.inCat7 = false;
			Mods.inCat8 = false;
			Mods.inCat9 = false;
			Mods.inCat10 = false;
			Mods.inCat12 = false;
			pageNumber = 0;
			Refresh();
		}
		if (r == "Dis")
		{
			PhotonNetwork.Disconnect();
		}
		else
		{
			if (r == "sear" || r == "Home")
			{
				return;
			}
			List<ButtonInfo> s2 = Mods.LocalCB();
			int num = (s2.Count + pageSize - 1) / pageSize;
			if (r == text)
			{
				if (pageNumber < num - 1)
				{
					pageNumber++;
				}
				else
				{
					pageNumber = 0;
				}
				Refresh();
				if (Mods.searching && Mods.usingmenu.Count > 0 && Mods.sided)
				{
					ModSidedMenu.Send("spawn");
				}
				return;
			}
			if (r == text2)
			{
				if (pageNumber > 0)
				{
					pageNumber--;
				}
				else
				{
					pageNumber = num - 1;
				}
				Refresh();
				if (Mods.searching && Mods.usingmenu.Count > 0 && Mods.sided)
				{
					ModSidedMenu.Send("spawn");
				}
				return;
			}
			int num2 = -1;
			for (int i = 0; i < s2.Count; i++)
			{
				if (r == s2[i].buttonText)
				{
					num2 = i;
					break;
				}
			}
			bool flag = !Mods.actsearch && Mods.searching;
			if (!s2[num2].enabled.HasValue)
			{
				return;
			}
			if (gripDownL && gripDownR && !s2[num2].buttonText.StartsWith("Exit") && s2[num2].buttonText != "Favorite Mods" && !flag)
			{
				if (CatButtons12.Any((ButtonInfo e) => e.buttonText == s2[num2].buttonText))
				{
					CatButtons12.RemoveAll((ButtonInfo e) => e.buttonText == s2[num2].buttonText);
				}
				else
				{
					CatButtons12.Insert(1, s2[num2]);
				}
				Mods.SaveFavs();
			}
			else
			{
				s2[num2].enabled = !s2[num2].enabled;
			}
			lx = num2;
			if (lx != -1 && lx < s2.Count && s2[num2].buttonText != "Search" && !s2[num2].buttonText.StartsWith("Use Menu Anyways") && !flag)
			{
				tooltipString = GetButtonTooltip(lx);
				tooltipText.text = tooltipString;
				lx = -1;
			}
			if (s2[num2].n == "" && Mods.searching && Mods.usingmenu.Count > 0 && Mods.sided)
			{
				ModSidedMenu.Send("spawn");
			}
			Refresh();
		}
	}

	public static Vector3 S3(Vector3 v)
	{
		return (GTExt.IsNaN(ref v) || GTExt.IsInfinity(ref v)) ? Vector3.zero : Vector3.ClampMagnitude(v, 5000f);
	}

	public static void O()
	{
		VRRig val = Mods.OwnRig();
		Sp = ((val != null) ? ((Component)val).transform.position : Sp);
	}

	public static void EventReceived(EventData d)
	{
		try
		{
			Player player = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(d.Sender, false);
			if (Mods.sided && Mods.usingmenu.Count > 0 && d.Code == 72)
			{
				try
				{
					if (sidedbuttons.ContainsKey(player))
					{
						sidedbuttons.Remove(player);
					}
					object[] array = (object[])d.CustomData;
					sidedbuttons.Add(player, (object[])array[0]);
				}
				catch
				{
				}
			}
			if (Mods.sided && Mods.usingmenu.Count > 0 && d.Code == 71 && sidedbuttons.ContainsKey(player))
			{
				ModSidedMenu.DrawMenu(d);
			}
		}
		catch
		{
		}
	}

	static WristMenu()
	{
	}
}
