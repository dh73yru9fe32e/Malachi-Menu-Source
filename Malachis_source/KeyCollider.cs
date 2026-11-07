using System.Collections.Generic;
using System.Linq;
using MalachiTemp.Backend;
using MalachiTemp.UI;
using UnityEngine;
using MalachiTemp.Gui;

internal class KeyCollider : MonoBehaviour
{
	public int framePressCooldownn = 0;

	public string ch;

	public List<ButtonInfo> but = new List<ButtonInfo>();

	public int limit = 30;

	public string mo = "d";

	private void OnTriggerEnter(Collider ReflectionEnums)
	{
		if (Time.frameCount >= framePressCooldownn + 8 && (((Object)ReflectionEnums).name == "bp" || ((Object)ReflectionEnums).name == "bp1"))
		{
			bool flag = ((Object)ReflectionEnums).name == "bp";
			Mods.GIn().StartVibration(!flag, Mods.GIn().tapHapticStrength / 9f, 0.001f);
			GuiPlugin.PT(!flag, 66);
			if (ch == "delete")
			{
				if (Mods.typed != "")
				{
					Mods.typed = Mods.typed.Substring(0, Mods.typed.Length - 1);
				}
			}
			else if (ch == "enter")
			{
				Mods.typed += " ";
			}
			else
			{
				Mods.typed += ch;
			}
			if (Mods.typed.Length > limit)
			{
				Mods.typed = Mods.typed.Substring(0, limit);
			}
			if (WristMenu.pageNumber != 0)
			{
				WristMenu.pageNumber = 0;
			}
			if ((but != null && mo == "d") || mo == "ed")
			{
				List<ButtonInfo> c2 = WristMenu.CatButtons11;
				foreach (ButtonInfo b2 in but)
				{
					if (b2.buttonText.ToUpper().Contains(Mods.typed) && !c2.Any((ButtonInfo x) => x.buttonText == b2.buttonText) && Mods.typed != "")
					{
						c2.Insert(c2.Count, b2);
					}
					if (!b2.buttonText.ToUpper().Contains(Mods.typed) && c2.Any((ButtonInfo x) => x.buttonText == b2.buttonText))
					{
						c2.RemoveAll((ButtonInfo x) => x.buttonText == b2.buttonText);
					}
				}
				if (Mods.typed == "")
				{
					c2.RemoveAll((ButtonInfo x) => x.buttonText != "Cancel");
					but.ForEach((ButtonInfo e) =>
					{
						c2.Insert(c2.Count, e);
					});
				}
				else if (c2.Count == ((mo == "ed") ? 1 : 0))
				{
					c2.Insert((mo == "ed") ? 1 : 0, new ButtonInfo
					{
						buttonText = "No Results Found!",
						method = () =>
						{
							c2.Find((ButtonInfo e) => e.buttonText == "No Results Found!").enabled = false;
							WristMenu.Refresh();
						},
						enabled = false,
						toolTip = " "
					});
				}
				else if (c2.Any((ButtonInfo x) => x.buttonText == "No Results Found!") && c2.Count > ((!(mo == "ed")) ? 1 : 2))
				{
					c2.RemoveAll((ButtonInfo x) => x.buttonText == "No Results Found!");
				}
				WristMenu.Refresh();
				WristMenu.ModSidedMenu.FixLag("spawn");
			}
		}
		framePressCooldownn = Time.frameCount;
	}
}
