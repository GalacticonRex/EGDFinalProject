using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    public float buildRadius = 10f;

	// Use this for initialization
	void Start () {
        base.Start();
        EnergyCost = 0;
        FoodCost = 1;
        PopulationRequirement = 0;
        initCosts();
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);

    }
    
    public bool withinRadius(Vector3 checkPos)
    {
        if (Vector3.Distance(transform.position, checkPos) < buildRadius)
        {
            return true;
        }
        return false;
    }
}
