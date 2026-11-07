using UnityEngine;

namespace MalachiTemp.Utilities;

public class ColorChanger : TimedBehaviour
{
	public Renderer gameObjectRenderer;

	public Gradient colors = null;

	public Color color;

	public bool timeBased = true;

	public override void Start()
	{
		base.Start();
		if ((Object)(object)((Component)this).GetComponent<Renderer>() != (Object)null)
		{
			gameObjectRenderer = ((Component)this).GetComponent<Renderer>();
		}
	}

	public override void Update()
	{
		base.Update();
		if (colors != null)
		{
			if (timeBased)
			{
				color = colors.Evaluate(progress);
			}
			gameObjectRenderer.material.color = color;
			gameObjectRenderer.material.SetColor("_EmissionColor", color);
		}
	}
}
