using UnityEngine;
using System.Collections;
using System;
public class ResourceController : MonoBehaviour {
    //THIS SCRIPT IS FOR DEBUGGING USAGE ONLY
    public int[] resourceValues;
    public int food, energy, population;
	// Use this for initialization
	void Start () {
        //    initResource();
        Globals.initResources();
        resourceValues = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];

    }

    // Update is called once per frame
    void Update () {
       //do this: on update values
        StartCoroutine("updateValues");

    }
    IEnumerator updateValues()
    {
        //these variables are for debugging.
        food = resourceValues[(int)Globals.resourceTypes.FOOD] = Globals.resources[Globals.resourceTypes.FOOD];
        population = resourceValues[(int)Globals.resourceTypes.POPULATION] = Globals.resources[Globals.resourceTypes.POPULATION];
        resourceValues[(int)Globals.resourceTypes.ENERGY] = Globals.resources[Globals.resourceTypes.ENERGY];
        energy = (Globals.currentEnergy);
        yield return new WaitForSeconds(5f);
    }
}
