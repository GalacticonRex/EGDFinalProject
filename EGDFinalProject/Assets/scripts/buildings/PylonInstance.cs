using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    //   public float buildRadius = 10f;
	// Use this for initialization
    
	void Start () {
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);
        base.Start();
    }
    void Update()
    {
        base.Update();
    }
    public bool withinRadius(Vector3 checkPos)
    {
        return base.withinRadius(checkPos);
    }
    public int getCost(Globals.resourceTypes type)
    {
        return base.getCost(type);
    }
    //public static bool withinRadius(Vector3 checkPos)
    //{
    //    return base.withinRadius(checkPos);
    //}
}
