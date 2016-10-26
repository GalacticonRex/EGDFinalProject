using UnityEngine;
using System.Collections;

public class envGeyserInstance : EnvironmentInstance
{

    // Use this for initialization
    void Start()
    {
        base.Start();
        resource = Globals.resourceTypes.WATER;
        resourceAmount = 100;

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
