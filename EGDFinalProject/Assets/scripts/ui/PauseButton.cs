using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickEventPause()
    {
        Globals.isPaused = true;
        //Debug.LogError("isPaused = true");
    }

    public void ClickEventResume()
    {
        Globals.isPaused = false;
    }
}
