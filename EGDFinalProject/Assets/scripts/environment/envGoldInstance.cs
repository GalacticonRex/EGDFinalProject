using UnityEngine;
using System.Collections;

public class envGoldInstance : EnvironmentInstance
{

    // Use this for initialization
    void Start()
    {
        base.Start();
        resource = Globals.resourceTypes.GOLD;
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
