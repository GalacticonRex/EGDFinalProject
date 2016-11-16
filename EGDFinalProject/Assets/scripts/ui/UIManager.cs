using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    RaycastHit myhit = new RaycastHit();
    Ray myray = new Ray();
    public GameObject inspector;
    GameObject population, energy, food, water, gold, populationText, energyText, foodText, waterText, goldText;
    public Sprite aNewImage;
    GameObject building_image;
    public Sprite[] newImages;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();

        building_image = inspector.transform.Find("ObjectImage").gameObject;
        population = inspector.transform.Find("CurrentPopulation").gameObject;
        energy = inspector.transform.Find("EnergyProduction").gameObject;
        food = inspector.transform.Find("FoodProduction").gameObject;
        water = inspector.transform.Find("WaterProduction").gameObject;
        gold = inspector.transform.Find("GoldProduction").gameObject;
        populationText = population.transform.Find("Text").gameObject;
        energyText = energy.transform.Find("Text").gameObject;
        foodText = food.transform.Find("Text").gameObject;
        waterText = water.transform.Find("Text").gameObject;
        goldText = gold.transform.Find("Text").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //Pause controller
	    if (Globals.isPaused)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else
        {
            Time.timeScale = 1;
            hidePaused();
        }

        //Inspector handler
        if (Input.GetMouseButtonDown(0))
        {
            var myray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myray, out myhit))
            {
                Image img = building_image.GetComponentInChildren(typeof(Image)) as Image;
                UnityEngine.UI.Text textObject;
                if (myhit.transform.name == "Pylon 1(Clone)")
                {
                    //Image img = building_image.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = true;
                    img.sprite = newImages[0];

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = myhit.transform.GetComponent<PylonInstance>().getProductionRate().ToString();

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = "0";

                    //Debug.Log("My object has been clicked");
                }
                else if (myhit.transform.name == "House(Clone)")
                {
                    img.enabled = true;
                    img.sprite = newImages[1];

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = "0";


                }
                else if (myhit.transform.name == "Farm(Clone)")
                {
                    img.enabled = true;
                    img.sprite = newImages[2];

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = myhit.transform.GetComponent<FarmInstance>().getProductionRate().ToString();

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = "0";
                }
                else if (myhit.transform.name == "Geyser_prefab(Clone)")
                {
                    img.enabled = true;
                    img.sprite = newImages[3];

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = myhit.transform.GetComponent<GeyserFarmInstance>().getProductionRate().ToString();

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = "0";
                }
                else if (myhit.transform.name == "Mine(Clone)")
                {
                    img.enabled = true;
                    img.sprite = newImages[4];

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = myhit.transform.GetComponent<MineInstance>().getProductionRate().ToString();
                }
                else
                {
                    Debug.Log("The object has not been clicked.");

                    img.enabled = false;

                    textObject = energyText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = populationText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = foodText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = waterText.GetComponent<Text>();
                    textObject.text = "0";

                    textObject = goldText.GetComponent<Text>();
                    textObject.text = "0";
                }
            }
        }
	}

    public void Reload()
    {
        //Application.LoadLevel(Application.loadedLevel);
    }

    public void showPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void pauseControl()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }
}
