using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour {
    public GameObject toGenerate;
    public Color Active;
    public Color Invalid;
    private Collider ground;
    private MeshRenderer self;
    private bool validPosition;
    private HashSet<BuildingInstance> overlapping;
    private Vector3 placeAt;
    private Material source;

    public BuildingController buildings;

    void Start()
    {
        buildings = GameObject.Find("BuildingController").GetComponent<BuildingController>();
        gameObject.layer = Globals.PLACEMENT_LAYER;
        GenerateHexGrid g = FindObjectOfType<GenerateHexGrid>();
        if (g != null)
        {
            ground = g.Ground;
        }
        self = GetComponent<MeshRenderer>();
        source = self.material;
        overlapping = new HashSet<BuildingInstance>();
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, Globals.GROUND_LAYER);
        validPosition = (result.collider == ground);
        if (result.collider != null)
        {
            placeAt = result.point;
            placeAt.y += 0.5f;
            transform.position = placeAt;
        }
        if (!Input.GetMouseButton(0))
        {
            bool no_overlap = (overlapping.Count == 0);
            if (no_overlap && validPosition && Globals.SpendResources(100, Globals.resourceTypes.FOOD))
            {
                addBuilding();
            }
            Destroy(gameObject);
        }

        if (overlapping.Count == 0 && validPosition)
            source.color = Active;
        else
            source.color = Invalid;
    }
     
    void addBuilding()
    {
        GameObject building = Instantiate(toGenerate);
        building.tag = "building";
        building.transform.position = placeAt;
        building.transform.parent = buildings.transform;
        int numBuildings = GameObject.FindGameObjectsWithTag("building").Length;//gameObject.GetComponentsInChildren<BuildingInstance>().Length;
        building.GetComponent<BuildingInstance>().index = numBuildings - 1;

        if (buildings.list.Length < numBuildings)
        {
            buildings.list = new BuildingInstance[(numBuildings * 2)-1];
            for (int j = 0; j < numBuildings; j++)
            {
                buildings.list[j] = building.GetComponent<BuildingInstance>();
            }
        }

        buildings.list[numBuildings - 1] = buildings.addBuilding(building, numBuildings);

        //this resizes the index table every time a building is added
        //need a more efficient way to do this...
        if (numBuildings > 1)
        {
            buildings.handleIndexTable(numBuildings);

        }

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
