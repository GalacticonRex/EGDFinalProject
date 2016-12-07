using UnityEngine;
using System.Collections;

public class FarmInstance : BuildingInstance
{
    int foodValue = 2;
    int energyValue = 1;
    int foodProductionRate = 5;
    envFoodInstance resource;
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
        base.Start();
        resource = GetComponentInChildren<envFoodInstance>();
        base.setEnvironment(resource);
        StartCoroutine(base.ActiveProduction(foodProductionRate));
        charactersSize = 0;
        foodProductionRate = charactersSize + 1;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
       // envFoodInstance e = GetComponentInChildren<envFoodInstance>();
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
    public int getProductionRate()
    {
        return foodProductionRate;
    }
}
