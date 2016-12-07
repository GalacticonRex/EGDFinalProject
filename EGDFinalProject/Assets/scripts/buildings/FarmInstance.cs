using UnityEngine;
using System.Collections;

public class FarmInstance : BuildingInstance
{
    int foodValue = 2;
    int energyValue = 1;
    int foodProductionRate = 5;
    envFoodInstance resource;
    public Character[] characters = new Character[5];
    public int charactersSize;
    public GameObject ToGenerate;
    public GameObject[] Proficiency = new GameObject[5];
    // Use this for initialization
    void Start()
    {
        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        GoldCost = 1;
        PopulationRequirement = 0;
        Globals.SpendResources(1, Globals.resourceTypes.POPULATION);
        base.Start();
        resource = GetComponentInChildren<envFoodInstance>();
        base.setEnvironment(resource);
        StartCoroutine(base.ActiveProduction(foodProductionRate));
        charactersSize = 0;
        foodProductionRate = charactersSize + 1;
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
        return foodProductionRate;
    }
}
