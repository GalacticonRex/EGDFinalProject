using UnityEngine;
using System.Collections;

public class BlenderInstance : BuildingInstance
{
    int foodValue = 1;
    int energyValue = 1;

    // Use this for initialization
    void Start()
    {
        EnergyCost = 1;
        FoodCost = 1;
        WaterCost = 1;
        GoldCost = 1;
        PopulationRequirement = 1;
        base.Start();
     //   produceResources();
    }

    // Update is called once per frame
    void Update()
    {
        //produceResources();
    }

    protected void produceResources()
    {
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        Globals.GainResource(1, Globals.resourceTypes.POPULATION);



    }
}
