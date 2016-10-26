﻿using UnityEngine;
using System.Collections;

public class GeyserFarmInstance : BuildingInstance
{
    int foodValue = 0;
    int waterValue = 2;
    int energyValue = 1;
    envGeyserInstance resource;
    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.SpendResources(1, Globals.resourceTypes.POPULATION);
        base.Start();
        resource = GetComponentInChildren<envGeyserInstance>();
        base.setEnvironment(resource);
        produceResources();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        // envFoodInstance e = GetComponentInChildren<envFoodInstance>();
    }

    protected void produceResources()
    {
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        if (environmentInstance.resourceAmount > 0)
        {
            Globals.GainResource(resource.harvestResource(5), Globals.resourceTypes.WATER);
        }
    }
    public void setEnvironment(EnvironmentInstance env)
    {
        base.setEnvironment(env);
    }
    void OnMouseEnter()
    {
        base.OnMouseEnter();
    }
    void OnMouseExit()
    {
        base.OnMouseExit();
    }
}