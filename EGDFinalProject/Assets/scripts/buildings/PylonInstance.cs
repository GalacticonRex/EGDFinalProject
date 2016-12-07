using UnityEngine;
using System.Collections;
using System;

public class PylonInstance : BuildingInstance {
    //   public float buildRadius = 10f;
    // Use this for initialization
    envEnergyInstance resource;
    int energyProductionRate = 5;
    public Character[] characters = new Character[5];
    public int charactersSize;
    public GameObject ToGenerate;
    public GameObject[] Proficiency = new GameObject[5];

    void Start () {
        EnergyCost = 0;
        FoodCost = 2;
        GoldCost = 2;
        WaterCost = 2;
        PopulationRequirement = 0;
        charactersSize = 0;
        base.initCosts();
       // transform.localScale = new Vector2(0.2f, 0.2f);
        
        resource = GetComponentInChildren<envEnergyInstance>();
        base.setEnvironment(resource);
        Globals.GainResource(5, Globals.resourceTypes.ENERGY);
        StartCoroutine(base.ActiveProduction(energyProductionRate));
        base.Start();
        energyProductionRate = charactersSize + 1;
    }
    void Update()
    {
        if (UnityEngine.Random.value < 0.01f && charactersSize < 5)
        {
            GameObject go = Instantiate(ToGenerate);
            GameObject child = Instantiate(Proficiency[0]);
            child.transform.parent = go.transform;
            child.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            go.transform.position = this.transform.position;
            go.transform.localScale = new Vector3(0, 0, 0);
            Character ch = go.GetComponent<Character>();
            Globals.currentPopulation += 1;
            ch.proficiency = 1;
            characters[charactersSize] = ch;
            charactersSize += 1;
        }
        base.Update();
    }
    public bool withinRadius(Vector3 checkPos)
    {
        return base.withinRadius(checkPos);
    }
    public int getCost(Globals.resourceTypes type)
    {
        return base.getCost(type);
    }
    public int getProductionRate()
    {
        return energyProductionRate;
    }
}
