using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    RaycastHit myhit = new RaycastHit();
    Ray myray = new Ray();
    public GameObject inspector;
    public Sprite aNewImage;
    GameObject building_image;
    public Sprite[] newImages;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
        building_image = inspector.transform.Find("ObjectImage").gameObject;
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
                if (myhit.transform.name == "Pylon 1(Clone)")
                {
                    //Image img = building_image.GetComponentInChildren(typeof(Image)) as Image;
                    img.sprite = newImages[0];
                    //Debug.Log("My object has been clicked");
                }
                else if (myhit.transform.name == "House(Clone)")
                {
                    img.sprite = newImages[1];
                }
                else if (myhit.transform.name == "Farm(Clone)")
                {
                    img.sprite = newImages[2];
                }
                else if (myhit.transform.name == "Geyser_prefab(Clone)")
                {
                    img.sprite = newImages[3];
                }
                else if (myhit.transform.name == "Mine(Clone)")
                {
                    img.sprite = newImages[4];
                }
                else
                {
                    Debug.Log("The object has not been clicked.");
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
