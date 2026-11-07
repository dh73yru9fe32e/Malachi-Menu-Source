using BepInEx;
using HarmonyLib;

namespace Malachis_Temp;

[BepInPlugin("malachisreborn", "malachis.reborn", "1.0")]
public class Plugin : BaseUnityPlugin
{
	public const string Name = "malachisreborn";

	public const string GUID = "malachis.reborn";

	public const string Version = "1.0";

	private bool p = false;

	private void Awake()
	{
		if (!p)
		{
			new Harmony("malachis.reborn").PatchAll();
			p = true;
		}
	}
}
