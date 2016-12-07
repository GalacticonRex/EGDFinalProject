using UnityEngine;
using System.Collections;

public class GeyserFarmInstance : BuildingInstance
{
    int foodValue = 0;
    int waterValue = 2;
    int energyValue = 1;
    int waterProductionRate = 5;
    envGeyserInstance resource;
    public Character[] characters = new Character[5];
    public int charactersSize;
    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.SpendResources(1, Globals.resourceTypes.POPULATION);
        Globals.SpendResources(5, Globals.resourceTypes.FOOD);
        resource = GetComponentInChildren<envGeyserInstance>();
        base.setEnvironment(resource);
        base.Start();
        StartCoroutine(base.ActiveProduction(waterProductionRate));
        charactersSize = 0;
        waterProductionRate = charactersSize + 1;
       
        //   produceResources();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        // envFoodInstance e = GetComponentInChildren<envFoodInstance>();
    }

    //protected void produceResources()
    //{
    //    // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
    //    if (environmentInstance.resourceAmount > 0)
    //    {
    //        Globals.GainResource(5, Globals.resourceTypes.WATER, environmentInstance);
    //    }
    //}
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
    public int getProductionRate()
    {
        return waterProductionRate;
    }
}
