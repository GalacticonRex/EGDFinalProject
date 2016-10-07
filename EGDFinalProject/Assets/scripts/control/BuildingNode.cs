using UnityEngine;
using System.Collections;  

public class BuildingNode : Node {
    private int[] connected;
    public GameObject[] paths;
	// Use this for initialization
	void Start () {
       
    }

    // Update is called once per frame
    void Update () {
    }
    void setIndex(int newIndex)
    {
        index = newIndex;
    }
}
