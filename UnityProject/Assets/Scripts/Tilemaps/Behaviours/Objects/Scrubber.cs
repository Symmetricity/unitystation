using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Atmospherics;

public class Scrubber : AdvancedPipe
{
	public float MinimumPressure = 101.325f;
	private MetaDataNode metaNode;
	private MetaDataLayer metaDataLayer;

	private void Start()
	{
		if (anchored)
		{
			LoadTurf();
		}
		UpdateManager.Instance.Add(UpdateMe);
	}

	public override void Attach()
	{
		base.Attach();
		LoadTurf();
	}

	private void LoadTurf()
	{
		metaDataLayer = MatrixManager.AtPoint(Vector3Int.RoundToInt(transform.position), true).MetaDataLayer;
		metaNode = metaDataLayer.Get(Vector3Int.RoundToInt(transform.position), false);
	}

	void UpdateMe()
	{
		if (isServer && anchored)
		{
			CheckAtmos();
		}
	}

	private void CheckAtmos()
	{
		if (metaNode.GasMix.Pressure > MinimumPressure)
		{
			var suckedAir =  metaNode.GasMix / 2;
			pipenet.gasMix += suckedAir;
			metaNode.GasMix -= suckedAir;
			metaDataLayer.UpdateSystemsAt(Vector3Int.RoundToInt(transform.position));
		}
	}

}
