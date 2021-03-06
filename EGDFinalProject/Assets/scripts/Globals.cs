﻿using UnityEngine;
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
        WATER = 3,
        GOLD = 4,
        OBSTACLE = 100,
        DIRT = 101,
    }
    public static Dictionary<resourceTypes, int> resources = new Dictionary<resourceTypes, int>();
    /* ENVIRONMENT VARIABLES */
    public static Dictionary<resourceTypes, int> environment = new Dictionary<resourceTypes, int>();
    static public int numEnergyNodes = 0;
    static public int numFoodNodes = 0;
    static public int numWaterNodes = 0;
    static public int numGoldNodes = 0;
    /*                       */
    static public bool EnoughResources(int cost, resourceTypes type)
    {
        return (resources[type] >= cost);
    }

    static public bool isPaused;
    static public int energyNodes = 100, farmNodes  = 50, obstacleNodes = 20, waterNodes = 20, goldNodes = 25;
    static public int currentEnergy = 0;
    static public int currentPopulation = 0;
    static public int characters = 0;
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
        else if (resources[type] < cost)
            return false;
        resources[type] -= cost;
        return true;
    }
    static public bool sufficientResources(int cost, resourceTypes type)
    {
        if (type == resourceTypes.POPULATION)
        {
            if (currentPopulation + cost > resources[resourceTypes.POPULATION])
            {
                return false;
            }
            return true;
        }
        if (type == resourceTypes.ENERGY)
        {
            if (currentEnergy + cost > resources[resourceTypes.ENERGY])
            {
                return false;
            }
            return true;
        }
        else if (resources[type] < cost)
            return false;
        return true;
    }
    static public void GainResource(int amount, resourceTypes type, EnvironmentInstance env)
    {
        if (env != null)
        {
        //    Debug.Log(type);
            env.harvestResource(amount);
        }
         resources[type] += amount;
       
    }
    static public void GainResource(int amount, resourceTypes type)
    {
        resources[type] += amount;
    }

    // Use this for initialization
    void Start () {
        
    }
    public static void initEnvironment()
    {
        environment.Add(resourceTypes.ENERGY, energyNodes);
        environment.Add(resourceTypes.FOOD, farmNodes);
        environment.Add(resourceTypes.WATER, waterNodes);
        environment.Add(resourceTypes.GOLD, goldNodes);
        environment.Add(resourceTypes.OBSTACLE, obstacleNodes);
    }
    public static void addEnv(resourceTypes res)
    {
        if (!environment.ContainsKey(res))
        {
            environment.Add(res, 20);
        }
    }
    public static void initResources()
    {
        resources.Add(resourceTypes.POPULATION, 1);
        resources.Add(resourceTypes.GOLD, 20);
        resources.Add(resourceTypes.ENERGY, 10);
        resources.Add(resourceTypes.FOOD, 100);
        resources.Add(resourceTypes.WATER, 100);
    }
    // Update is called once per frame
    void Update () {
	}

}
