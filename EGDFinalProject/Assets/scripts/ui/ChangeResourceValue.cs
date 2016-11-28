using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeResourceValue : MonoBehaviour {
    UnityEngine.UI.Text value;
    public int resourceType;
    private int aRandomInt = 0;

	// Use this for initialization
	void Start () {
        value = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        if (resourceType == 0)
            value.text = Globals.currentPopulation.ToString();
        else if (resourceType == 1)
            value.text = Globals.resources[Globals.resourceTypes.ENERGY].ToString();
        else if (resourceType == 2)
            value.text = Globals.resources[Globals.resourceTypes.FOOD].ToString(); /* Should be equal to current food */
        else if (resourceType == 3)
            value.text = Globals.resources[Globals.resourceTypes.WATER].ToString();
        else if (resourceType == 4)
            value.text = Globals.resources[Globals.resourceTypes.GOLD].ToString();
        else
            Debug.LogError("Invalid resourceType value");
	}
}
