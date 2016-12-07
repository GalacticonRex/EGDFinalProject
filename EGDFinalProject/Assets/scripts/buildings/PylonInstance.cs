using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    //   public float buildRadius = 10f;
    // Use this for initialization
    envEnergyInstance resource;
    int energyProductionRate = 5;

    void Start () {
        EnergyCost = 0;
        FoodCost = 2;
        WaterCost = 2;
        PopulationRequirement = 0;
        base.initCosts();
       // transform.localScale = new Vector2(0.2f, 0.2f);
        
        resource = GetComponentInChildren<envEnergyInstance>();
        base.setEnvironment(resource);
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);
        StartCoroutine(base.ActiveProduction(energyProductionRate));
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
    public int getProductionRate()
    {
        return energyProductionRate;
    }
}
