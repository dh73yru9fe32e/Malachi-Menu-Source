using MalachiTemp.Backend;
using MalachiTemp.UI;
using UnityEngine;
using MalachiTemp.Gui;

internal class BtnCollider : MonoBehaviour
{
	public static int framePressCooldown;

	public string relatedText;

	public static bool La(string s)
	{
		return s != "sear" && s != "Dis" && s != "Next" && s != "Prev" && s != "Home" && !s.StartsWith("Exit ") && s != "Favorite Mods";
	}

	private void OnTriggerEnter(Collider ReflectionEnums)
	{
		if (Time.frameCount < framePressCooldown + WristMenu.clickcool || (!(((Object)ReflectionEnums).name == "bp") && !(((Object)ReflectionEnums).name == "bp1")))
		{
			return;
		}
		bool flag = ((Object)ReflectionEnums).name == "bp";
		float num = Mods.GIn().tapHapticStrength / 10f;
		if (!Mods.searching)
		{
			int s = ((WristMenu.gripDownL && WristMenu.gripDownR && La(relatedText)) ? 213 : (-1));
			if (!Mods.right)
			{
				Mods.GIn().StartVibration(false, num, 0.001f);
				GuiPlugin.PT(b: false, s);
			}
			else
			{
				Mods.GIn().StartVibration(true, num, 0.001f);
				GuiPlugin.PT(b: true, s);
			}
		}
		else
		{
			Mods.GIn().StartVibration(!flag, num, 0.001f);
			GuiPlugin.PT(!flag, (Mods.actsearch && WristMenu.gripDownL && WristMenu.gripDownR && La(relatedText)) ? 213 : (-1));
		}
		WristMenu.Toggle(relatedText);
		framePressCooldown = Time.frameCount;
	}
}
