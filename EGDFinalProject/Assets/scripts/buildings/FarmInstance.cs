using UnityEngine;
using System.Collections;

public class FarmInstance : BuildingInstance
{
    int foodValue = 2;
    int energyValue = 1;

    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.SpendResources(1, Globals.resourceTypes.POPULATION);
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("produceResources");
    }

    protected IEnumerator produceResources()
    {
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        Globals.GainResource(1, Globals.resourceTypes.FOOD);
        yield return new WaitForSeconds(10f);


    }
}
