using UnityEngine;
using System.Collections;

public class BuildingInstance : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.layer = Globals.BUILDING_LAYER;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
