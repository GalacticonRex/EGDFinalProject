﻿using UnityEngine;
using System.Collections;

public class envEnergyInstance : EnvironmentInstance
{
    // Use this for initialization
    void Start()
    {
        base.Start();
        resource = Globals.resourceTypes.ENERGY;
        resourceAmount = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
}