using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildingInstance : MonoBehaviour
{
    public int index;
    protected bool active;
    public Hexagon ground;
    public GameObject pathPlacer;
    public GameObject[] paths;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Dictionary<PathInstance, BuildingInstance> connections;
    private PathPlacer current;
    private MeshRenderer render;
    private SpriteRenderer sprite;
    private CharacterFactory factory;
    protected Material material;
    //= new int[Enum.GetNames(typeof(Globals.resourceTypes)).Length];
    public static Dictionary<Globals.resourceTypes, int> costs;
    protected int EnergyCost;
    protected int FoodCost;
    protected int WaterCost;
    protected int GoldCost;
    protected int PopulationRequirement;
    protected float buildRadius = 10f;

    protected EnvironmentInstance environmentInstance;

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
    protected void setActive()
    {
        active = !active;
    }
    // Use this for initialization
    protected void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Buildings");
        current = null;
        connections = new Dictionary<PathInstance, BuildingInstance>();
        render = GetComponent<MeshRenderer>();
        if (render!=null) material = render.material;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.sortingOrder = -500;
        }
        if (sprite == null)
        {
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in sprites)
            {
                s.sortingOrder = (int)Camera.main.WorldToScreenPoint(s.bounds.min).y * -1;
            }
        }
        initCosts();
        setActive();
        WaterCost = 0;
        EnergyCost = 0;
        FoodCost = 0;
        PopulationRequirement = 0;
        factory = FindObjectOfType<CharacterFactory>();
    }
    protected void Update()
    {
        if (UnityEngine.Random.value < 0.01f)
        {
            int j = UnityEngine.Random.Range(0, connections.Count);
            int i = 0;
            foreach( KeyValuePair<PathInstance, BuildingInstance> kv in connections )
            {
                if (i == j)
                {
                    Character ch = factory.Create(UnityEngine.Random.Range(1,5), this, kv.Key);
                    ch.speed *= 3.0f;
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
      //  material.color = Color.green;
    }
    protected void OnMouseExit()
    {
       // material.color = Color.white;
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
                case Globals.resourceTypes.GOLD:
                    costs.Add(type, GoldCost);
                    break;
                case Globals.resourceTypes.FOOD:
                    costs.Add(type, FoodCost);
                    break;
                case Globals.resourceTypes.POPULATION:
                    costs.Add(type, PopulationRequirement);
                    break;
                case Globals.resourceTypes.WATER:
                    costs.Add(type, WaterCost);
                    break;
            }
        }
    }
    public int getCost(Globals.resourceTypes type)
    {
        if (costs == null) initCosts();
        return costs[type];
    }
    public bool withinRadius(Vector3 checkPos)
    {
        if (Vector3.Distance(transform.position, checkPos) < buildRadius)
        {
            return true;
        }
        return false;
    }
    protected void setEnvironment(EnvironmentInstance env)
    {
        environmentInstance = env;
    }
    public void produceResources(int resourceProductionRate)
    {
        // base.produceResources();
        // Globals.SpendResources(1, Globals.resourceTypes.FOOD);
        if (environmentInstance != null && environmentInstance.resourceAmount > 0)
        {
            Globals.GainResource(resourceProductionRate, environmentInstance.resource, environmentInstance);
        }
    }
    protected IEnumerator ActiveProduction(int resourceProductionRate)
    {
        while (active)
        {
            produceResources(resourceProductionRate);
            yield return new WaitForSeconds(5f);
        }
    }
}
