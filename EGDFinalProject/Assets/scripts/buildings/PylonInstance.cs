using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    public float buildRadius;
    public int[] cost;

	// Use this for initialization
	void Start () {
        cost = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public bool withinRadius(Vector3 targetPos)
    {
        if (Vector3.Distance(transform.position, targetPos) < buildRadius)
        {
            return true;
        }
        return false;
    }
}
