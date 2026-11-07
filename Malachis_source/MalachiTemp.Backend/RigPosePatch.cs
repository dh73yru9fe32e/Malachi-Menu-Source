using HarmonyLib;
using MalachiTemp.UI;
using UnityEngine;

namespace MalachiTemp.Backend;

[HarmonyPatch(typeof(VRRig), "PostTick")]
public static class RigPosePatch
{
	public static bool upsiderig;

	public static bool fullupsiderig;

	public static bool backrig;

	public static bool fullbackrig;

	public static bool forwardrig;

	public static bool crawl;

	public static bool spin;

	public static bool spaz;

	public static bool track;

	public static bool fullspin;

	public static bool upspin;

	public static bool forrig;

	public static bool crawlrig;

	public static bool spazrig;

	public static bool spin2;

	public static bool spin3;

	public static bool spin4;

	public static bool spin5;

	public static bool flip;

	public static bool fflip;

	public static void Postfix(VRRig __instance)
	{
		if (!__instance.isLocal || (!upsiderig && ((!fflip && !flip) || !((Behaviour)Mods.OwnRig()).enabled) && !spazrig && !crawlrig && !forrig && !fullupsiderig && !backrig && !fullbackrig && !forwardrig && !crawl && !spin && !spin2 && !spin4 && !spin5 && !spin3 && !spaz && !track && !upspin && (!Mods.IsEnabled("Grab Rig") || (!WristMenu.gripDownL && !WristMenu.gripDownR))))
		{
			return;
		}
		VRRig val = Mods.OwnRig();
		if (Mods.IsEnabled("Grab Rig") && (WristMenu.gripDownL || WristMenu.gripDownR))
		{
			if (WristMenu.gripDownR && !WristMenu.gripDownL)
			{
				((Component)val).transform.rotation = Mods.Loco().RightHand.controllerTransform.rotation;
			}
			if (!WristMenu.gripDownR && WristMenu.gripDownL)
			{
				((Component)val).transform.rotation = Mods.Loco().LeftHand.controllerTransform.rotation;
			}
			return;
		}
		Quaternion rotation = ((Component)Mods.GIn().headCollider).transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		Quaternion rotation2 = Quaternion.Euler(0f, eulerAngles.y, 0f);
		bool flag = false;
		bool flag2 = false;
		if (upsiderig || fullupsiderig)
		{
			rotation2 = Quaternion.Euler(0f, eulerAngles.y, 180f);
		}
		if (backrig || fullbackrig)
		{
			rotation2 = Quaternion.Euler(0f, eulerAngles.y + 180f, 0f);
		}
		if (forwardrig || forrig)
		{
			rotation2 = Quaternion.Euler(-86f, eulerAngles.y, 0f);
		}
		if (crawl || crawlrig)
		{
			rotation2 = Quaternion.Euler(87f, eulerAngles.y, 0f);
		}
		if (spaz || spazrig)
		{
			rotation2 = Random.rotationUniform;
		}
		if (track)
		{
			rotation2 = ((Component)Camera.main).transform.rotation;
			((Component)Mods.GIn().bodyCollider).transform.rotation = rotation2;
			((Component)Mods.GIn().headCollider).transform.rotation = rotation2;
			((Component)Mods.Loco().headCollider).transform.rotation = rotation2;
			((Component)Mods.Loco().bodyCollider).transform.rotation = rotation2;
		}
		if (spin || upspin || spin2 || spin3 || spin4 || spin5)
		{
			flag2 = true;
			if (Time.time > Mods.orbit)
			{
				Mods.orbit = Time.time;
				Mods.angle += 280f * Time.deltaTime;
			}
			rotation2 = Quaternion.Euler(spin3 ? Mods.angle : 0f, spin ? Mods.angle : eulerAngles.y, spin2 ? Mods.angle : 0f);
			if (upspin)
			{
				rotation2 = Quaternion.Euler(0f, Mods.angle, 180f);
			}
			if (spin4 || spin5)
			{
				rotation2 = Quaternion.Euler(spin4 ? (-86f) : 87f, Mods.angle, 0f);
			}
		}
		if (flip || fflip)
		{
			flag = (flip || fflip) && (WristMenu.rjoy || WristMenu.ljoy);
			if (flip && WristMenu.rjoy && Mods.bangle < 360f)
			{
				WristMenu.ljoy = false;
				WristMenu.bu = false;
				Mods.bangle += 350f * Time.deltaTime;
				rotation2 = Quaternion.Euler(0f - Mods.bangle, eulerAngles.y, 0f);
			}
			if (flip && (Mods.bangle >= 360f || !WristMenu.rjoy))
			{
				Mods.bangle = 0f;
				WristMenu.rjoy = false;
				WristMenu.ru = false;
			}
			if (fflip && WristMenu.ljoy && Mods.fangle < 360f)
			{
				WristMenu.rjoy = false;
				WristMenu.ru = false;
				Mods.fangle += 350f * Time.deltaTime;
				rotation2 = Quaternion.Euler(Mods.fangle, eulerAngles.y, 0f);
			}
			if (fflip && (Mods.fangle >= 360f || !WristMenu.ljoy))
			{
				Mods.fangle = 0f;
				WristMenu.ljoy = false;
				WristMenu.bu = false;
			}
		}
		((Component)val).transform.rotation = rotation2;
		float value = Mods.Create(val).Field(Mods.Field(66)).GetValue<float>();
		Transform playerOffsetTransform = val.playerOffsetTransform;
		if ((!fullspin || !flag2) && !forrig && !crawlrig && !fullbackrig && !fullupsiderig && !spazrig && !flag)
		{
			val.head.MapMine(value, playerOffsetTransform);
		}
		if (!flag)
		{
			val.leftHand.MapMine(value, playerOffsetTransform);
			val.rightHand.MapMine(value, playerOffsetTransform);
		}
	}
}
