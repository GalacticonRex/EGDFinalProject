using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Globals : MonoBehaviour {
    public static int GROUND_LAYER
    { get { return 1 << LayerMask.NameToLayer("Ground"); } }
    public static int PLACEMENT_LAYER
    { get { return 1 << LayerMask.NameToLayer("Placement"); } }
    public static int BUILDING_LAYER
    { get { return 1 << LayerMask.NameToLayer("Buildings"); } }

    static public int Resources = int.MaxValue;

    /*
     * POP:0,ENERGY:1,FOOD:2
     */
    public enum resourceTypes
    {
        POPULATION = 0,
        ENERGY = 1,
        FOOD = 2,
    }
    public static Dictionary<resourceTypes, int> resources = new Dictionary<resourceTypes, int>();

    static public bool EnoughResources(int cost, resourceTypes type)
    {
        return (resources[type] >= cost);
    }
    static public bool SpendResources(int cost, resourceTypes type)
    {
        if (resources[type] < cost)
            return false;
        resources[type] -= cost;
        return true;
    }
	// Use this for initialization
	void Start () {
        
    }
    public static void initResources()
    {
      //  resourceValues = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
        resources.Add(resourceTypes.POPULATION, 3);
        resources.Add(resourceTypes.ENERGY, 3);
        resources.Add(resourceTypes.FOOD, 3);

    }
    // Update is called once per frame
    void Update () {
	}

}
