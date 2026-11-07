using System.Collections;
using UnityEngine;

namespace MalachiTemp.Backend;

public class CoroutineRunner : MonoBehaviour
{
	public static CoroutineRunner instance;

	private void Awake()
	{
		instance = this;
	}

	public static Coroutine Start(IEnumerator e)
	{
		return ((MonoBehaviour)instance).StartCoroutine(e);
	}
}
