﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour {
    public GenerateHexGrid parent;
    public GameObject toGenerate;
    public Color Active;
    public Color Invalid;
    public AudioClip PlayOnPlace;
    private Collider ground;
    private SpriteRenderer self;
    private bool validPosition;
    private HashSet<BuildingInstance> overlapping;
    private Vector3 placeAt;
    private Material source;
    private bool isPylon;
    private bool isFarm;
    private bool isGeyser;
    private bool isMine;
    public ResourceController rc;
    private Hexagon lockToNearsetHex(Vector3 pt)
    {
        HexStack stack = parent.GetTile(pt);
        if (stack == null || stack.layers.Count == 0)
            return null;
        return stack.layers[0];
    }

    void Start()
    {
        rc = GameObject.Find("ResourceController").GetComponent<ResourceController>();
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        gameObject.layer = LayerMask.NameToLayer("Placement");
        parent = FindObjectOfType<GenerateHexGrid>();
        if (parent != null)
        {
            ground = parent.Ground;
        }
        self = GetComponent<SpriteRenderer>();
        source = self.material;
        overlapping = new HashSet<BuildingInstance>();
        if (toGenerate.GetComponent<PylonInstance>() != null) isPylon = true;
        if (toGenerate.GetComponent<FarmInstance>() != null) isFarm = true;
        if (toGenerate.GetComponent<GeyserFarmInstance>() != null) isGeyser = true;
        if (toGenerate.GetComponent<MineInstance>() != null) isMine = true;
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, 1<<LayerMask.NameToLayer("Ground"));
        //Debug.Log(result.distance.ToString());

        Hexagon hex = lockToNearsetHex(result.point);
        validPosition = (hex != null && result.collider == ground);

        if (validPosition)
        {
            placeAt = hex.Position;
            self.enabled = true;
            transform.position = placeAt;
        }
        else
        {
            self.enabled = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            bool no_overlap = (overlapping.Count == 0);
            BuildingInstance building = toGenerate.GetComponent<BuildingInstance>();
            bool cost = checkResources(building);
            if (isGeyser)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment.GetComponent<envGeyserInstance>() != null);
            }
            else if (isMine)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment.GetComponent<envGoldInstance>() != null);
            }
            else if (isFarm)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment.GetComponent<envFoodInstance>() != null);
            }
            else if (isPylon)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment.GetComponent<envEnergyInstance>() != null);
            }
            else if (!isPylon && !isFarm)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment == null);
            }
            if (no_overlap && validPosition && cost && findNearestPylon())
            {
               // spendResources(building);
                if ( PlayOnPlace != null )
                    AudioSource.PlayClipAtPoint(PlayOnPlace, Camera.main.transform.position);
                GameObject newBuilding = addBuilding(hex);
                if (hex.environment != null)
                {
                    hex.environment.transform.parent = newBuilding.transform;
                    hex.environment.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }

            }
            Destroy(gameObject);
        }
        else if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            Destroy(gameObject);
        }

        if (overlapping.Count == 0 && validPosition && findNearestPylon())
            source.color = Active;
        else
            source.color = Invalid;
    }
    void spendResources(BuildingInstance building)
    {
        Globals.SpendResources(building.getCost(Globals.resourceTypes.FOOD), Globals.resourceTypes.FOOD);
        Globals.SpendResources(building.getCost(Globals.resourceTypes.ENERGY), Globals.resourceTypes.ENERGY);
        Globals.SpendResources(building.getCost(Globals.resourceTypes.WATER), Globals.resourceTypes.WATER);
        Globals.SpendResources(building.getCost(Globals.resourceTypes.GOLD), Globals.resourceTypes.GOLD);

    }
    bool checkResources(BuildingInstance b)
    {
        int foodCost = b.getCost(Globals.resourceTypes.FOOD);
        int energyCost = b.getCost(Globals.resourceTypes.ENERGY);
        int waterCost = b.getCost(Globals.resourceTypes.WATER);
        int goldCost = b.getCost(Globals.resourceTypes.GOLD);

        //&& Globals.sufficientResources(1, Globals.resourceTypes.GOLD);

        return Globals.sufficientResources(foodCost, Globals.resourceTypes.FOOD)
            && Globals.sufficientResources(energyCost, Globals.resourceTypes.ENERGY)
            && Globals.sufficientResources(waterCost, Globals.resourceTypes.WATER)
            && Globals.sufficientResources(goldCost, Globals.resourceTypes.GOLD);
    }
    GameObject addBuilding(Hexagon hex)
    {
        GameObject building = Instantiate(toGenerate);
        building.transform.parent = GameObject.Find("BuildingController").transform;
        BuildingInstance bb = building.GetComponent<BuildingInstance>();
        if(bb != null)
        {
            bb.ground = hex;
        }

        building.tag = "building";
        building.transform.position = placeAt;
            return building;
    }
    bool findNearestPylon()
    {
        bool withinRadius = false;
        if (toGenerate.GetComponent<PylonInstance>() != null)
        {
            return true;
        }
        else
        {
            PylonInstance[] pylons = GameObject.FindObjectsOfType<PylonInstance>();

            for (int i = 0; i < pylons.Length; i++)
            {
                if (pylons[i].withinRadius(transform.position))
                {
                    withinRadius = true;
                    return withinRadius;
                }
            }

        }
        return withinRadius;
    }
    bool findNearestBuilding()
    {
        bool withinRadius = false;
        
            BuildingInstance[] buildings = GameObject.FindObjectsOfType<BuildingInstance>();

            for (int i = 0; i < buildings.Length; i++)
            {
                if (buildings[i].withinRadius(transform.position))
                {
                    withinRadius = true;
                    return withinRadius;
                }
            }
        return withinRadius;
    }
    void OnTriggerEnter(Collider collid)
    {
        if (collid == ground)
            return;
        BuildingInstance bi = collid.GetComponent<BuildingInstance>();
        if (bi != null)
            overlapping.Add(bi);
    }
    void OnTriggerExit(Collider collid)
    {
        if (collid == ground)
            return;
        BuildingInstance bi = collid.GetComponent<BuildingInstance>();
        if (bi != null)
            overlapping.Remove(bi);
    }
}
