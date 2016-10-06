using UnityEngine;
using System.Collections;

public class BuildingInstance : MonoBehaviour {
    public int index;
    public GameObject[] paths;
	// Use this for initialization
	void Start () {
        gameObject.layer = Globals.BUILDING_LAYER;
	}
	
    void setIndex(int newIndex)
    {
        index = newIndex;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
