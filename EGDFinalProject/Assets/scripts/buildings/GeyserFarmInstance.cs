using UnityEngine;
using System.Collections;

public class GeyserFarmInstance : BuildingInstance
{
    int foodValue = 0;
    int waterValue = 2;
    int energyValue = 1;
    int waterProductionRate = 5;
    envGeyserInstance resource;
    public Character[] characters = new Character[5];
    public int charactersSize;
    public GameObject ToGenerate;
    public GameObject[] Proficiency = new GameObject[5];
    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 2;
        FoodCost = 2;
        WaterCost = 0;
        GoldCost = 0;
        PopulationRequirement = 0;
        resource = GetComponentInChildren<envGeyserInstance>();
        base.setEnvironment(resource);
        base.Start();
        StartCoroutine(base.ActiveProduction(waterProductionRate));
        charactersSize = 0;
        waterProductionRate = charactersSize + 1;
       
        //   produceResources();
    }

    // Update is called once per frame
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
        // envFoodInstance e = GetComponentInChildren<envFoodInstance>();
    }

    //protected void produceResources()
    //{
    //    // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
    //    if (environmentInstance.resourceAmount > 0)
    //    {
    //        Globals.GainResource(5, Globals.resourceTypes.WATER, environmentInstance);
    //    }
    //}
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
    public int getProductionRate()
    {
        return waterProductionRate;
    }
}
