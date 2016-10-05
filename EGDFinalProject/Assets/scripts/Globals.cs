using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {
    public const int GROUND_LAYER = 1;
    public const int PLACEMENT_LAYER = 4;
    public const int BUILDING_LAYER = 5;
    static public int Resources = int.MaxValue;

    static public bool SpendResources(int cost)
    {
        if (Resources < cost)
            return false;
        Resources -= cost;
        return true;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
