using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Globals : MonoBehaviour {
    public const int GROUND_LAYER = 0x01;
    public const int PLACEMENT_LAYER = 0x02;
    public const int BUILDING_LAYER = 0x04;
    static public int Resources = int.MaxValue;
    /*
     * POP:0,ENERGY:1,FOOD:2
     * 
     */
    public enum resourceTypes
    {
        POPULATION = 0,
        ENERGY = 1,
        FOOD = 2,
    }
    public static Dictionary<resourceTypes, int> resources = new Dictionary<resourceTypes, int>();
    static public int currentEnergy = 0;

    static public bool SpendResources(int cost, resourceTypes type)
    {
        //have required energy amount for energy resource
        if (type == resourceTypes.ENERGY)
        {
            if (currentEnergy + cost > resources[resourceTypes.ENERGY])
            {
                return false;
            }
            else currentEnergy += cost;
            return true;
        }
        else if (resources[type] < cost)
            return false;
        resources[type] -= cost;
        return true;
    }
    static public void GainResource(int amount, resourceTypes type)
    {
        resources[type] += amount;
    }

    // Use this for initialization
    void Start () {
        
    }
    public static void initResources()
    {
      //  resourceValues = new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
        resources.Add(resourceTypes.POPULATION, 1);
        resources.Add(resourceTypes.ENERGY, 2);
        resources.Add(resourceTypes.FOOD, 2);

    }
    // Update is called once per frame
    void Update () {
	}

}
