using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BuildingButton : MonoBehaviour, IPointerDownHandler {

    public int Index;
    public GameObject Building;
    public float Delay = 0.25f;
    private bool isActive;
    private UnityEngine.UI.Image self;
    private float initPos;
    private float timeAccum;
    private bool inPosition;

    public bool Active {
        set
        {
            isActive = value;
            if( value )
                self.enabled = true;
        }
        get
        {
            return isActive;
        }
    }

    float GetCurrentPosition()
    {
        if (isActive && timeAccum < Index)
        {
            timeAccum += Time.deltaTime / Delay;
            if (timeAccum > Index)
                timeAccum = Index;
        }
        else if (!isActive && timeAccum > 0)
        {
            timeAccum -= Time.deltaTime / Delay;
            if (timeAccum < 0)
            {
                timeAccum = 0;
                self.enabled = false;
            }
        }
        return initPos + (timeAccum-Index+1) * 96.0f;
    }

    void Start()
    {
        initPos = transform.position.x;
        self = GetComponent<UnityEngine.UI.Image>();
        self.enabled = false;
        isActive = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = transform.position;
        position.x = GetCurrentPosition();
        transform.position = position;
    }

    public void OnPointerDown(PointerEventData pt)
    {
        Debug.Log("MOO");
        Instantiate(Building);
    }
}
