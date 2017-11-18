using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour {

	// Use this for initialization
	public PrefabsLibrary pl;

    private void Awake()
    {
        GameManager.Instance.FeedbackManager = this;
		if(pl == null)
		{
			pl = Resources.Load("SfxLibrary") as PrefabsLibrary;
		}
    }
	void Start () 
	{
		GameObject go = GameManager.Instance.PlayerBrain.gameObject;
		possessionFX = Instantiate(pl.FX_Host, go.transform.position, Quaternion.identity, go.transform);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	Puppet currentlySelectablePuppet;
	Material baseMaterial;
	GameObject selectableFX;

	public void HighlightDeadPuppet(Puppet p)
	{
		if(currentlySelectablePuppet != null)
		{
			UnHighlight();
		}

		if(p != null)
		{
			Highlight(p);
		}
	}

	void Highlight(Puppet p)
	{
		SkinnedMeshRenderer smr = p.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if(smr != null)
		{
			baseMaterial = smr.material;
			smr.material = pl.MAT_OnDeadSelectable;
			currentlySelectablePuppet = p;
			selectableFX = Instantiate(pl.FX_OnDeadSelectable, p.transform.position, Quaternion.identity, p.transform);
		}
	}

	void UnHighlight()
	{
		SkinnedMeshRenderer smr = currentlySelectablePuppet.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if(smr != null)
		{
			smr.material = baseMaterial;
			baseMaterial = null;
			Destroy(selectableFX);
			currentlySelectablePuppet = null;
		}
	}

	GameObject possessionFX;
    public void PossessBody(Transform target)
    {
        possessionFX.transform.SetParent(target, false);
    }

    public void UnPossessBody(Transform target)
    {		
        possessionFX.transform.SetParent(target, false);
    }
}
