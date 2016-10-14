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
    private MeshRenderer self;
    private bool validPosition;
    private HashSet<BuildingInstance> overlapping;
    private Vector3 placeAt;
    private Material source;
    private bool isPylon;

    private Hexagon lockToNearsetHex(Vector3 pt)
    {
        HexStack stack = parent.GetTile(pt);
        if (stack == null || stack.layers.Count == 0)
            return null;
        return stack.layers[0];
    }

    void Start()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        gameObject.layer = LayerMask.NameToLayer("Placement");
        parent = FindObjectOfType<GenerateHexGrid>();
        if (parent != null)
        {
            ground = parent.Ground;
        }
        self = GetComponent<MeshRenderer>();
        source = self.material;
        overlapping = new HashSet<BuildingInstance>();
        if (toGenerate.GetComponent<PylonInstance>() != null) isPylon = true;
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
            
            bool cost = Globals.sufficientResources(1, Globals.resourceTypes.FOOD) && Globals.sufficientResources(1, Globals.resourceTypes.ENERGY);//Globals.SpendResources(1, Globals.resourceTypes.FOOD) && Globals.SpendResources(1, Globals.resourceTypes.ENERGY);
            if (isPylon)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment != null);
            }
            else if (!isPylon)
            {
                validPosition = (hex != null && result.collider == ground && hex.environment == null);
            }
            //   if (overlapping.Count > 0) Debug.Log(overlapping[0]);
            if (no_overlap && validPosition && cost && findNearestPylon())
            {
                Globals.SpendResources(1, Globals.resourceTypes.FOOD);
                Globals.SpendResources(1, Globals.resourceTypes.ENERGY);
                if ( PlayOnPlace != null )
                    AudioSource.PlayClipAtPoint(PlayOnPlace, Camera.main.transform.position);
                addBuilding(hex);
                if (isPylon)
                {
                    if (hex.environment != null) hex.environment.gameObject.GetComponent<MeshRenderer>().enabled = false;
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

    void addBuilding(Hexagon hex)
    {
        GameObject building = Instantiate(toGenerate);
        BuildingInstance bb = building.GetComponent<BuildingInstance>();
        if(bb != null)
        {
            bb.ground = hex;
        }

        building.tag = "building";
        building.transform.position = placeAt;
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
