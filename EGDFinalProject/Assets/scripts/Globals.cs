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
        OBSTACLE = 100
    }
    public static Dictionary<resourceTypes, int> resources = new Dictionary<resourceTypes, int>();
    public static Dictionary<resourceTypes, int> environment = new Dictionary<resourceTypes, int>();
    static public int numEnergyNodes = 0;
    static public bool EnoughResources(int cost, resourceTypes type)
    {
        return (resources[type] >= cost);
    }

    static public int currentEnergy = 0;
    static public int energyNodes = 10, farmNodes  = 10, obstacleNodes = 10;
    static public int currentPopulation = 0;
    static public bool SpendResources(int cost, resourceTypes type)
    {
        //have required energy amount for energy resource
        if (type == resourceTypes.POPULATION)
        {
            if (currentPopulation + cost > resources[resourceTypes.POPULATION])
            {
                return false;
            }
            else currentPopulation += cost;
            return true;
        }
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
        //Debug.Log(amount);
        resources[type] += amount;
    }

    // Use this for initialization
    void Start () {
        
    }
    public static void initEnvironment()
    {
        environment.Add(resourceTypes.ENERGY, energyNodes);
        environment.Add(resourceTypes.FOOD, farmNodes);
        environment.Add(resourceTypes.OBSTACLE, obstacleNodes);
    }
    public static void initResources()
    {
        resources.Add(resourceTypes.POPULATION, 100);
        resources.Add(resourceTypes.ENERGY, 100);
        resources.Add(resourceTypes.FOOD, 100);

    }
    // Update is called once per frame
    void Update () {
	}

}
