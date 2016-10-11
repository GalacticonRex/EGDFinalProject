using UnityEngine;
using System.Collections;

public class HouseInstance : BuildingInstance {
    int foodValue = 1;
    int energyValue = 1;
    
	// Use this for initialization
	void Start () {
        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.GainResource(1, Globals.resourceTypes.POPULATION);
        base.Start();
    }

    // Update is called once per frame
    void Update () {
    }
    
  /*  public override int[] produceResources()
    {
        int[] res = new int[2];
        res[0] = energyValue;
        res[1] = foodValue;
        return res;
        
    }*/
}
