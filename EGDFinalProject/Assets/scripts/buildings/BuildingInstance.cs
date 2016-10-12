using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildingInstance : MonoBehaviour {
    public int index;
    public Hexagon ground;
    public GameObject pathPlacer;
    public GameObject[] paths;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Dictionary<PathInstance, BuildingInstance> connections;
    private PathPlacer current;
    private MeshRenderer render;
    protected Material material;

    RaycastHit hit;
    float dist;
    Vector3 dir;
    //= new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
    public static Dictionary<Globals.resourceTypes, int> costs;
    public static int EnergyCost;
    public static int FoodCost;

    public void AddConnection(PathInstance p, BuildingInstance b)
    {
        connections.Add(p, b);
    }
    public bool ConnectedTo(BuildingInstance inst)
    {
        foreach(BuildingInstance b in connections.Values)
            if (b == inst) return true;
        return false;
    }

    // Use this for initialization
    protected void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Buildings");
        current = null;
        connections = new Dictionary<PathInstance, BuildingInstance>();
        render = GetComponent<MeshRenderer>();
        material = render.material;
        initCosts();
    }	
    void setIndex(int newIndex)
    {
        index = newIndex;
    }
    protected void OnMouseEnter()
    {
        material.color = Color.green;
    }
    protected void OnMouseExit()
    {
        material.color = Color.white;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        BuildingController buildings;
        if (transform.parent == null) {
            transform.parent = GameObject.Find("BuildingController").transform;
        }
        buildings = transform.parent.GetComponent<BuildingController>();
        buildings.selectBuilding(gameObject, index);
    }
    void OnMouseDown()
    {
        GameObject go = Instantiate(pathPlacer);
        PathPlacer pp = go.GetComponent<PathPlacer>();
        pp.source = this;
    }
    static void initCosts() {
        costs = new Dictionary<Globals.resourceTypes, int>();
        foreach (Globals.resourceTypes type in Enum.GetValues(typeof(Globals.resourceTypes)))
        {
            costs.Add(type, 10);
            switch (type)
            {
                case Globals.resourceTypes.ENERGY:
                    EnergyCost = 10;
                    break;
                case Globals.resourceTypes.FOOD:
                    FoodCost = 10;
                    break;
            }
        }
      //  costs.Add(Globals.resourceTypes.FOOD, 10);


    }
}
