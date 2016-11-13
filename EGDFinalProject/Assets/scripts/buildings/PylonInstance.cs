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
       // transform.localScale = new Vector2(0.2f, 0.2f);
        base.Start();
        resource = GetComponentInChildren<envEnergyInstance>();
        base.setEnvironment(resource);
        StartCoroutine(base.ActiveProduction());
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
}
