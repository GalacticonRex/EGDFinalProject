using UnityEngine;
using System.Collections;

public class MineInstance : BuildingInstance
{
    int foodValue = 2;
    int waterValue = 10;
    int energyValue = 1;
    envGoldInstance resource;
    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 5;
        WaterCost = 5;
        FoodCost = 5;
        PopulationRequirement = 5;
        base.Start();
        resource = GetComponentInChildren<envGoldInstance>();
        base.setEnvironment(resource);
        produceResources();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        // envFoodInstance e = GetComponentInChildren<envFoodInstance>();
    }

    protected void produceResources()
    {
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        if (environmentInstance.resourceAmount > 0)
        {
            //harvest at the rate of workers on the farm
            Globals.GainResource(resource.harvestResource(5), Globals.resourceTypes.GOLD);
        }
    }
    public void setEnvironment(EnvironmentInstance env)
    {
        base.setEnvironment(env);
    }
    void OnMouseEnter()
    {
        base.OnMouseEnter();
    }
    void OnMouseExit()
    {
        base.OnMouseExit();
    }
}
