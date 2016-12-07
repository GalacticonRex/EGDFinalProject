using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class ResourceController : MonoBehaviour {
    //THIS SCRIPT IS FOR DEBUGGING USAGE ONLY
    public int[] resourceValues;
    public int food, energy, water, gold, population;
    public bool on;
    public Dictionary<string, int[]> costDict;
	// Use this for initialization
	void Start () {
        on = true;
        resourceValues = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
     //   StartCoroutine("updateValues");
        StartCoroutine("calculateHunger");
    }

    // Update is called once per frame
    void Update () {
        updateVals();
    }
    public void initBuildingCost()
    {
        //cost dictionary
        //"house" --> [population, food, water, energy, gold] costs
        costDict = new Dictionary<string, int[]>();
        //int[] costs = new int[5];
        int[] costs = new int[] { 1, -1, -1, -1, -1 };
        costDict.Add("house", costs);
        costs = new int[] { 0, -2, -2, 5, -1 };
        costDict.Add("pylon",costs);
        costs = new int[] { 0, 0, -2, -2, -1 };
        costDict.Add("farm", costs);
        costs = new int[] { 0, -5, -5, -5, -1 };
        costDict.Add("mine", costs);
        costs = new int[] { 0, -2, 0, -2, 0 };
        costDict.Add("geyser", costs);
        costs = new int[] { 0, -4, -5, -5, -10 };
        costDict.Add("blender", costs);

    }
    public int[] getBuildingCosts(string name)
    {
        if (costDict == null) initBuildingCost();
        return costDict[name];
    }
    void updateVals()
    {
        food = resourceValues[(int)Globals.resourceTypes.FOOD] = Globals.resources[Globals.resourceTypes.FOOD];
        population = resourceValues[(int)Globals.resourceTypes.POPULATION] = Globals.resources[Globals.resourceTypes.POPULATION];
        resourceValues[(int)Globals.resourceTypes.ENERGY] = Globals.resources[Globals.resourceTypes.ENERGY];
        gold = Globals.resources[Globals.resourceTypes.GOLD];
        water = Globals.resources[Globals.resourceTypes.WATER];
        energy = (Globals.currentEnergy);
    }
    IEnumerator updateValues()
    {
        while (on)
        {
            //these variables are for debugging.
           // Debug.Log("updating...");
            food = resourceValues[(int)Globals.resourceTypes.FOOD] = Globals.resources[Globals.resourceTypes.FOOD];
            population = resourceValues[(int)Globals.resourceTypes.POPULATION] = Globals.resources[Globals.resourceTypes.POPULATION];
            resourceValues[(int)Globals.resourceTypes.ENERGY] = Globals.resources[Globals.resourceTypes.ENERGY];
            gold = Globals.resources[Globals.resourceTypes.GOLD];
            water = Globals.resources[Globals.resourceTypes.WATER];
            energy = (Globals.currentEnergy);
            
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator calculateHunger()
    {
        while (on)
        {
            Globals.SpendResources(Globals.currentPopulation, Globals.resourceTypes.FOOD);
            Globals.SpendResources(Globals.currentPopulation, Globals.resourceTypes.WATER);
            // Debug.Log(Globals.resources[Globals.resourceTypes.FOOD]);
            yield return new WaitForSeconds(20f);
        }
    }
}
