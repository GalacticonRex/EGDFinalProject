using UnityEngine;
using System.Collections;

public class envFoodInstance : EnvironmentInstance {

	// Use this for initialization
	void Start () {
        base.Start();
        resource = Globals.resourceTypes.FOOD;
        resourceAmount = 100;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
