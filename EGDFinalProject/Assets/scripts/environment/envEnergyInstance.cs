using UnityEngine;
using System.Collections;

public class envEnergyInstance : EnvironmentInstance
{
    // Use this for initialization
    void Start()
    {
        base.Start();
        resource = Globals.resourceTypes.ENERGY;
        resourceAmount = 100;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public int harvestResource(int amount)
    {
        return base.harvestResource(amount);
    }
}
