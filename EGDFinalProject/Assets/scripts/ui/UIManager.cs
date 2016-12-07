using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    RaycastHit myhit = new RaycastHit();
    RaycastHit2D my2Dhit = new RaycastHit2D();
    Ray myray = new Ray();
    public GameObject inspector;
    GameObject population, energy, food, water, gold, populationText, energyText, foodText, waterText, goldText, char1, char2, char3, char4, char5;
    public Sprite aNewImage;
    GameObject building_image;
    public Sprite[] newImages;
    Character[] currentlyShowing;
    Character selectedCharacter;
    int selectedCharacterInt;
    bool characterSelected;
    

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
        char1 = inspector.transform.Find("Char1").gameObject;
        char2 = inspector.transform.Find("Char2").gameObject;
        char3 = inspector.transform.Find("Char3").gameObject;
        char4 = inspector.transform.Find("Char4").gameObject;
        char5 = inspector.transform.Find("Char5").gameObject;
        characterSelected = false;
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
            //my2Dhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //int layerMask = 1 << 5;
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> rayresult = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, rayresult);
            
            if (Physics.Raycast(myray, out myhit))
            {
                Image img = building_image.GetComponentInChildren(typeof(Image)) as Image;
                UnityEngine.UI.Text textObject;
                if (myhit.transform.name == "Pylon 1(Clone)")
                {
                    if (characterSelected && myhit.transform.GetComponent<PylonInstance>().charactersSize < 5)
                    {
                        myhit.transform.GetComponent<PylonInstance>().characters[myhit.transform.GetComponent<PylonInstance>().charactersSize] = selectedCharacter;
                        currentlyShowing[selectedCharacterInt] = null;
                    }
                    currentlyShowing = myhit.transform.GetComponent<PylonInstance>().characters;
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


                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;

                    int x = myhit.transform.GetComponent<PylonInstance>().charactersSize;
                    if (x != 0)
                    {
                        if (x >= 1)
                        {
                            img = char1.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<PylonInstance>().characters[0].proficiency];
                        }
                        if (x >= 2)
                        {
                            img = char2.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<PylonInstance>().characters[1].proficiency];
                        }
                        if (x >= 3)
                        {
                            img = char3.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<PylonInstance>().characters[2].proficiency];
                        }
                        if (x >= 4)
                        {
                            img = char4.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<PylonInstance>().characters[3].proficiency];
                        }
                        if (x >= 5)
                        {
                            img = char5.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<PylonInstance>().characters[4].proficiency];
                        }
                    }
                }
                else if (myhit.transform.name == "House(Clone)")
                {
                    if (characterSelected && myhit.transform.GetComponent<HouseInstance>().charactersSize < 5)
                    {
                        myhit.transform.GetComponent<HouseInstance>().characters[myhit.transform.GetComponent<HouseInstance>().charactersSize] = selectedCharacter;
                        currentlyShowing[selectedCharacterInt] = null;
                    }
                    currentlyShowing = myhit.transform.GetComponent<HouseInstance>().characters;
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


                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;

                    int x = myhit.transform.GetComponent<HouseInstance>().charactersSize;
                    if (x != 0)
                    {
                        if (x >= 1)
                        {
                            img = char1.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<HouseInstance>().characters[0].proficiency];
                        }
                        if (x >= 2)
                        {
                            img = char2.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<HouseInstance>().characters[1].proficiency];
                        }
                        if (x >= 3)
                        {
                            img = char3.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<HouseInstance>().characters[2].proficiency];
                        }
                        if (x >= 4)
                        {
                            img = char4.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<HouseInstance>().characters[3].proficiency];
                        }
                        if (x >= 5)
                        {
                            img = char5.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<HouseInstance>().characters[4].proficiency];
                        }
                    }
                }
                else if (myhit.transform.name == "Farm(Clone)")
                {
                    if (characterSelected && myhit.transform.GetComponent<FarmInstance>().charactersSize < 5)
                    {
                        myhit.transform.GetComponent<FarmInstance>().characters[myhit.transform.GetComponent<FarmInstance>().charactersSize] = selectedCharacter;
                        currentlyShowing[selectedCharacterInt] = null;
                    }
                    currentlyShowing = myhit.transform.GetComponent<FarmInstance>().characters;
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


                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;

                    int x = myhit.transform.GetComponent<FarmInstance>().charactersSize;
                    if (x != 0)
                    {
                        if (x >= 1)
                        {
                            img = char1.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<FarmInstance>().characters[0].proficiency];
                        }
                        if (x >= 2)
                        {
                            img = char2.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<FarmInstance>().characters[1].proficiency];
                        }
                        if (x >= 3)
                        {
                            img = char3.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<FarmInstance>().characters[2].proficiency];
                        }
                        if (x >= 4)
                        {
                            img = char4.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<FarmInstance>().characters[3].proficiency];
                        }
                        if (x >= 5)
                        {
                            img = char5.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<FarmInstance>().characters[4].proficiency];
                        }
                    }
                }
                else if (myhit.transform.name == "Geyser_prefab(Clone)")
                {
                    if (characterSelected && myhit.transform.GetComponent<GeyserFarmInstance>().charactersSize < 5)
                    {
                        myhit.transform.GetComponent<GeyserFarmInstance>().characters[myhit.transform.GetComponent<GeyserFarmInstance>().charactersSize] = selectedCharacter;
                        currentlyShowing[selectedCharacterInt] = null;
                    }
                    currentlyShowing = myhit.transform.GetComponent<GeyserFarmInstance>().characters;
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


                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;

                    int x = myhit.transform.GetComponent<GeyserFarmInstance>().charactersSize;
                    if (x != 0)
                    {
                        if (x >= 1)
                        {
                            img = char1.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<GeyserFarmInstance>().characters[0].proficiency];
                        }
                        if (x >= 2)
                        {
                            img = char2.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<GeyserFarmInstance>().characters[1].proficiency];
                        }
                        if (x >= 3)
                        {
                            img = char3.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<GeyserFarmInstance>().characters[2].proficiency];
                        }
                        if (x >= 4)
                        {
                            img = char4.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<GeyserFarmInstance>().characters[3].proficiency];
                        }
                        if (x >= 5)
                        {
                            img = char5.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<GeyserFarmInstance>().characters[4].proficiency];
                        }
                    }
                }
                else if (myhit.transform.name == "Mine(Clone)")
                {
                    if (characterSelected && myhit.transform.GetComponent<MineInstance>().charactersSize < 5)
                    {
                        myhit.transform.GetComponent<MineInstance>().characters[myhit.transform.GetComponent<MineInstance>().charactersSize] = selectedCharacter;
                        currentlyShowing[selectedCharacterInt] = null;
                    }
                    currentlyShowing = myhit.transform.GetComponent<MineInstance>().characters;
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


                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;

                    int x = myhit.transform.GetComponent<MineInstance>().charactersSize;
                    if (x != 0)
                    {
                        if (x >= 1)
                        {
                            img = char1.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<MineInstance>().characters[0].proficiency];
                        }
                        if (x >= 2)
                        {
                            img = char2.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<MineInstance>().characters[1].proficiency];
                        }
                        if (x >= 3)
                        {
                            img = char3.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<MineInstance>().characters[2].proficiency];
                        }
                        if (x >= 4)
                        {
                            img = char4.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<MineInstance>().characters[3].proficiency];
                        }
                        if (x >= 5)
                        {
                            img = char5.GetComponentInChildren(typeof(Image)) as Image;
                            img.enabled = true;
                            img.sprite = newImages[myhit.transform.GetComponent<MineInstance>().characters[4].proficiency];
                        }
                    }
                }
                else if (myhit.transform.name == "Char1")
                {
                    selectedCharacter = currentlyShowing[0];
                    characterSelected = true;
                    selectedCharacterInt = 0;
                }
                else if (myhit.transform.name == "Char2")
                {
                    selectedCharacter = currentlyShowing[1];
                    characterSelected = true;
                    selectedCharacterInt = 1;
                }
                else if (myhit.transform.name == "Char3")
                {
                    selectedCharacter = currentlyShowing[2];
                    characterSelected = true;
                    selectedCharacterInt = 2;
                }
                else if (myhit.transform.name == "Char4")
                {
                    selectedCharacter = currentlyShowing[3];
                    characterSelected = true;
                    selectedCharacterInt = 3;
                }
                else if (myhit.transform.name == "Char5")
                {
                    selectedCharacter = currentlyShowing[4];
                    characterSelected = true;
                    selectedCharacterInt = 4;
                }
                else
                {
                    currentlyShowing = null;
                    //Debug.Log("The object has not been clicked.");

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

                    img = char1.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char2.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char3.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char4.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                    img = char5.GetComponentInChildren(typeof(Image)) as Image;
                    img.enabled = false;
                }
            }
            if (rayresult.Count > 0)
            {
                Debug.Log("2D Raycast Hit");
                for (int i = 0; i < rayresult.Count; i++)
                {
                    if (rayresult[i].gameObject.transform.name == "Char1")
                    {
                        selectedCharacter = currentlyShowing[0];
                        characterSelected = true;
                        selectedCharacterInt = 0;
                    }
                    else if (rayresult[i].gameObject.transform.name == "Char2")
                    {
                        selectedCharacter = currentlyShowing[1];
                        characterSelected = true;
                        selectedCharacterInt = 1;
                    }
                    else if (rayresult[i].gameObject.transform.name == "Char3")
                    {
                        selectedCharacter = currentlyShowing[2];
                        characterSelected = true;
                        selectedCharacterInt = 2;
                    }
                    else if (rayresult[i].gameObject.transform.name == "Char4")
                    {
                        selectedCharacter = currentlyShowing[3];
                        characterSelected = true;
                        selectedCharacterInt = 3;
                    }
                    else if (rayresult[i].gameObject.transform.name == "Char5")
                    {
                        selectedCharacter = currentlyShowing[4];
                        characterSelected = true;
                        selectedCharacterInt = 4;
                    }
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
