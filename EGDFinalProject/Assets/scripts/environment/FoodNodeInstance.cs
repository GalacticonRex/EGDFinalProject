using UnityEngine;
using System.Collections;

public class FoodNodeInstance : EnvironmentInstance
{
    // Use this for initialization
    protected void Start()
    {
        base.Start();
        resource = Globals.resourceTypes.FOOD;
        resourceAmount = 200;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
