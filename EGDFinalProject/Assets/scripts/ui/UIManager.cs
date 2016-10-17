using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
	}
	
	// Update is called once per frame
	void Update () {
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
