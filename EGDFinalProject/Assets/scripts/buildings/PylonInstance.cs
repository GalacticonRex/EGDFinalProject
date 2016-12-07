using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    //   public float buildRadius = 10f;
    // Use this for initialization
    envEnergyInstance resource;
    int energyProductionRate = 5;
    public Character[] characters = new Character[5];
    public int charactersSize;

    void Start () {
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        charactersSize = 0;
       // transform.localScale = new Vector2(0.2f, 0.2f);
        
        resource = GetComponentInChildren<envEnergyInstance>();
        base.setEnvironment(resource);
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);
        StartCoroutine(base.ActiveProduction(energyProductionRate));
        base.Start();
        energyProductionRate = charactersSize + 1;
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
