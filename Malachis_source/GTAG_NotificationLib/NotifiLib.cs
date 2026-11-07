using System;
using System.Linq;
using MalachiTemp.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GTAG_NotificationLib;

public class NotifiLib : MonoBehaviour
{
	public static GameObject HUDObj;

	private GameObject HUDObj2;

	private GameObject MainCamera;

	public static Text Testtext;

	private Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

	public static int NotificationDecayTime = 150;

	private int N = 200;

	public static int NoticationThreshold = 5;

	private string[] Notifilines;

	private string newtext;

	public static string PreviousNotifi;

	private bool HasInit = false;

	public static Text NotifiText;

	public static float r;

	private void Init()
	{
		MainCamera = GameObject.Find("Main Camera");
		if ((Object)(object)MainCamera == (Object)null)
		{
			Debug.LogError((object)"no cam");
			return;
		}
		HUDObj = new GameObject("NOTIFICATIONLIB_HUD_OBJ");
		HUDObj2 = new GameObject("NOTIFICATIONLIB_HUD_OBJ2");
		HUDObj.AddComponent<Canvas>();
		HUDObj.AddComponent<CanvasScaler>();
		HUDObj.AddComponent<GraphicRaycaster>();
		if ((Object)(object)HUDObj.GetComponent<Canvas>() == (Object)null)
		{
			Debug.LogError((object)"no Can");
			return;
		}
		((Behaviour)HUDObj.GetComponent<Canvas>()).enabled = true;
		HUDObj.GetComponent<Canvas>().renderMode = (RenderMode)2;
		HUDObj.GetComponent<Canvas>().worldCamera = MainCamera.GetComponent<Camera>();
		HUDObj.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 2.2f;
		HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
		((Transform)HUDObj.GetComponent<RectTransform>()).position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
		HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z - 4.6f);
		HUDObj.transform.SetParent(HUDObj2.transform);
		((Transform)HUDObj.GetComponent<RectTransform>()).localPosition = new Vector3(0f, 0f, 1.6f);
		Quaternion rotation = ((Transform)HUDObj.GetComponent<RectTransform>()).rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.y = -270f;
		HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
		((Transform)HUDObj.GetComponent<RectTransform>()).rotation = Quaternion.Euler(eulerAngles);
		GameObject val = new GameObject();
		val.transform.parent = HUDObj.transform;
		Testtext = val.AddComponent<Text>();
		if ((Object)(object)Testtext == (Object)null)
		{
			Debug.LogError((object)"no text");
			return;
		}
		Testtext.text = "";
		Testtext.fontSize = 9;
		Testtext.font = WristMenu.MenuFont;
		((Graphic)Testtext).rectTransform.sizeDelta = new Vector2(260f, 70f);
		Testtext.alignment = (TextAnchor)6;
		((Transform)((Graphic)Testtext).rectTransform).localScale = new Vector3(0.01f, 0.01f, 1f);
		((Transform)((Graphic)Testtext).rectTransform).localPosition = new Vector3(-1.2f, -0.7f, -0.6f);
		if ((Object)(object)AlertText == (Object)null)
		{
			Debug.LogError((object)"no mat");
			return;
		}
		((Graphic)Testtext).material = AlertText;
		NotifiText = Testtext;
	}

	private void FixedUpdate()
	{
		if (!HasInit && (Object)(object)GameObject.Find("Main Camera") != (Object)null)
		{
			Init();
			HasInit = true;
		}
		HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
		HUDObj2.transform.rotation = MainCamera.transform.rotation;
		if (Testtext.text != "")
		{
			N++;
			if (N <= NotificationDecayTime)
			{
				return;
			}
			Notifilines = null;
			newtext = "";
			N = 0;
			Notifilines = Testtext.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
			string[] notifilines = Notifilines;
			foreach (string text in notifilines)
			{
				if (text != "")
				{
					newtext = newtext + text + "\n";
				}
			}
			Testtext.text = newtext;
		}
		else
		{
			N = 0;
		}
	}

	public static void Send(string n)
	{
		if (!((Object)(object)NotifiText == (Object)null) && r < Time.time)
		{
			r = Time.time + 0.05f;
			if (!n.Contains(Environment.NewLine))
			{
				n += Environment.NewLine;
			}
			NotifiText.text += n;
			PreviousNotifi = n;
		}
	}

	public static void Clear()
	{
		NotifiText.text = "";
	}

	public static void ClearPast(int a)
	{
		string[] array = null;
		string text = "";
		array = NotifiText.text.Split(Environment.NewLine.ToCharArray()).Skip(a).ToArray();
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2 != "")
			{
				text = text + text2 + "\n";
			}
		}
		NotifiText.text = text;
	}

	public NotifiLib()
	{
	}
}
