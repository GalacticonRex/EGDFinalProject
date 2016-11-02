                                                                                                                                                                                                                    using UnityEngine;
using System.Collections;

public class HouseInstance : BuildingInstance {
    int foodValue = 1;
    int energyValue = 1;
    Character[] characters = new Character[5];
    int charactersSize;
    public GameObject ToGenerate;
    public GameObject[] Proficiency = new GameObject[5];
    
	// Use this for initialization
	void Start () {
        GameObject go = Instantiate(ToGenerate);
        GameObject child = Instantiate(Proficiency[0]);
        child.transform.parent = go.transform;
        child.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = this.transform.position;
        Character ch = go.GetComponent<Character>();
        Globals.currentPopulation += 1;
        ch.proficiency = 1;
        characters[0] = ch;

        int[] res = new int[2];
        EnergyCost = 1;
        FoodCost = 1;
        PopulationRequirement = 0;
        Globals.GainResource(1, Globals.resourceTypes.POPULATION);
        base.Start();
        produceResources();
    }

    // Update is called once per frame
    void Update () {
        //produceResources();
    }
    
    protected void produceResources()
    {
       // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        Globals.GainResource(1, Globals.resourceTypes.POPULATION);



    }
}
