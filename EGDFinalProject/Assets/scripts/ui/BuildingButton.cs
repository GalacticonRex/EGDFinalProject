using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    public int Index;
    public GameObject Building;
    public float Delay = 0.25f;
    private bool isActive;
    public UnityEngine.UI.Image self;
    private float initPos;
    private float timeAccum;
    private bool inPosition;
    public GameObject tooltip;
    public Sprite buildableSprite;
    public Image buildableLocation;
    private float tooltipPos;
    private ResourceController rc;
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
        if (Index != 6) return transform.position.x;
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
        return initPos + (timeAccum - Index + 1) * 96.0f;
    }

    void Start()
    {
        initPos = transform.position.x;
        tooltip = GameObject.Find("tooltip");
        tooltipPos = tooltip.transform.position.x;
        tooltip.GetComponent<CanvasGroup>().alpha = 0f;
        self = GetComponent<UnityEngine.UI.Image>();
        self.enabled = false;
        isActive = false;
        rc = GameObject.Find("ResourceController").GetComponent<ResourceController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = transform.position;
        position.x = GetCurrentPosition();
        transform.position = position;
    }
    public void displayCosts(GameObject tt)
    {
        tooltip t = tt.GetComponent<tooltip>();
        string name = null;
        // [population, food, water, energy, gold]
        if (Index == 1)
        {
            t.buildingName.text = "Mine";
            name = "mine";
        }
        if (Index == 2)
        {
            t.buildingName.text = "Energy Plant";
            name = "pylon";
        }
        if (Index == 3)
        {
            t.buildingName.text = "House";
            name = "house";

        }
        if (Index == 4)
        {
            t.buildingName.text = "Farm";
            name = "farm";

        }
        if (Index == 5)
        {
            t.buildingName.text = "Geyser";
            name = "geyser";
        }
        
        if (name == null) return;

        int[] c = (rc.getBuildingCosts(name));
        for (int i = 0; i < c.Length; i++)
        {
            t.costs[i].text = c[i].ToString();
        }
    }
    public void OnPointerDown(PointerEventData pt)
    {
        GameObject go = Instantiate(Building);
        go.transform.position = new Vector3(0, -100000, 0);
    }
    public void OnPointerEnter(PointerEventData pt)
    {
        
        if (tooltip.GetComponent<CanvasGroup>().alpha <= 0) tooltip.GetComponent<CanvasGroup>().alpha = 1;
        GameObject tt = GameObject.Find("tooltip");
        displayCosts(tt);
        tt.transform.position = new Vector2(tooltipPos, tt.transform.position.y);
        buildableLocation.sprite = buildableSprite;
        Debug.Log("OnPointerEnter called.");
    }
    public void OnPointerExit(PointerEventData pt)
    {
        GameObject tt = GameObject.Find("tooltip");
        tt.transform.position = new Vector2(10000, tt.transform.position.y);
        Debug.Log("OnPointerExit called.");
    }
}
