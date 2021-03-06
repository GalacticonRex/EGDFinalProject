﻿using UnityEngine;
using System.Collections;

public class EnvironmentInstance : MonoBehaviour {
    protected GenerateHexGrid parent;
    protected Collider ground;
    protected Material source;
    protected MeshRenderer self;
    public Globals.resourceTypes resource;
    public int resourceAmount;
    // Use this for initialization
    protected void Start () {
        
    }

    // Update is called once per frame
    protected void Update () {
	    
	}
    public int harvestResource(int amount)
    {
        if (resourceAmount - amount <= 0)
        {
            return resourceAmount;
        }
        else
        {
            resourceAmount -= amount;
            return amount;
        }
    }
}
