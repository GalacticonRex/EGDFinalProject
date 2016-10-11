using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    public float buildRadius = 10f;

	// Use this for initialization
	void Start () {
        // cost = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
        EnergyCost = 0;
        FoodCost = 1;
        PopulationRequirement = 0;
        initCosts();
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + 0.5f, transform.rotation.z));
	}
    void Bounce()
    { 

    }
    public bool withinRadius(Vector3 checkPos)
    {
        if (Vector3.Distance(transform.position, checkPos) < buildRadius)
        {
            return true;
        }
        return false;
    }
}
