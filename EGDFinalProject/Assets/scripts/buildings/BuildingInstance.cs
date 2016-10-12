using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildingInstance : MonoBehaviour
{
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

    //= new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
    public static Dictionary<Globals.resourceTypes, int> costs;
    protected int EnergyCost;
    protected int FoodCost;
    protected int PopulationRequirement;

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
    protected void Update()
    {
        if (UnityEngine.Random.value < 0.001f)
        {
            int j = UnityEngine.Random.Range(0, connections.Count);
            int i = 0;
            foreach( KeyValuePair<PathInstance, BuildingInstance> kv in connections )
            {
                if (i == j)
                {

                    break;
                }
                i++;
            }
        }
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
        BuildingController buildings;
        if (transform.parent == null)
        {
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
    protected void initCosts()
    {
        costs = new Dictionary<Globals.resourceTypes, int>();
        foreach (Globals.resourceTypes type in Enum.GetValues(typeof(Globals.resourceTypes)))
        {
            switch (type)
            {
                case Globals.resourceTypes.ENERGY:
                    costs.Add(type, EnergyCost);
                    break;
                case Globals.resourceTypes.FOOD:
                    costs.Add(type, FoodCost);
                    break;
                case Globals.resourceTypes.POPULATION:
                    costs.Add(type, PopulationRequirement);
                    break;
            }
        }
    }
}
