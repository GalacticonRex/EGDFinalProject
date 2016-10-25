using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    //   public float buildRadius = 10f;
    // Use this for initialization
    envEnergyInstance resource;

    void Start () {
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        base.Start();
        resource = GetComponentInChildren<envEnergyInstance>();
        base.setEnvironment(resource);
        produceResources();

    }
    void Update()
    {
        base.Update();
    }
    public bool withinRadius(Vector3 checkPos)
    {
        return base.withinRadius(checkPos);
    }
    protected void produceResources()
    {
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        if (environmentInstance.resourceAmount > 0)
        {
            Globals.GainResource(resource.harvestResource(5), Globals.resourceTypes.ENERGY);
        }
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
