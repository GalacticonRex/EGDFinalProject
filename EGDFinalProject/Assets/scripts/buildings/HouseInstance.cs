                                                                                                                                                                                                                    using UnityEngine;
using System.Collections;

public class HouseInstance : BuildingInstance {
    int foodValue = 1;
    int energyValue = 1;
    public Character[] characters = new Character[5];
    public int charactersSize;
    public GameObject ToGenerate;
    public GameObject[] Proficiency = new GameObject[5];
    
	// Use this for initialization
	void Start () {
        GameObject go = Instantiate(ToGenerate);
        GameObject child = Instantiate(Proficiency[Random.Range(0, 5)]);
        child.transform.parent = go.transform;
        child.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = this.transform.position;
        //go.transform.localScale = new Vector3(0, 0, 0);
        Character ch = go.GetComponent<Character>();
        Globals.currentPopulation += 1;
        ch.proficiency = Random.Range(1,5);
        characters[0] = ch;
        charactersSize = 1;

        int[] res = new int[2];

        Globals.GainResource(1, Globals.resourceTypes.POPULATION);
        base.Start();
        EnergyCost = 1;
        FoodCost = 1;
        WaterCost = 1;
        GoldCost = 1;
        PopulationRequirement = 1;
        produceResources();
        spendResources2();
    }

    // Update is called once per frame
    void Update () {
        //produceResources();
    }
    protected void spendResources2()
    {
        Globals.SpendResources(FoodCost, Globals.resourceTypes.FOOD);
        Globals.SpendResources(WaterCost, Globals.resourceTypes.WATER);
        Globals.SpendResources(GoldCost, Globals.resourceTypes.GOLD);
        Globals.SpendResources(EnergyCost, Globals.resourceTypes.ENERGY);
    }
    protected void produceResources()
    {
       // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        Globals.GainResource(1, Globals.resourceTypes.POPULATION);



    }
}
