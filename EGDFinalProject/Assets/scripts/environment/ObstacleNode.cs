using UnityEngine;
using System.Collections;

public class ObstacleNode : EnvironmentInstance {

	// Use this for initialization
	void Start () {
        base.Start();
        resource = Globals.resourceTypes.OBSTACLE;
        resourceAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
