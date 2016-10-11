using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {
    public const int GROUND_LAYER = 0x01;
    public const int PLACEMENT_LAYER = 0x02;
    public const int BUILDING_LAYER = 0x04;
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
