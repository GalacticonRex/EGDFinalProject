using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour {
    public GenerateHexGrid parent;
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
        buildings = GameObject.Find("BuildingController").GetComponent<BuildingController>();
        gameObject.layer = Globals.PLACEMENT_LAYER;
        parent = FindObjectOfType<GenerateHexGrid>();
        if (parent != null)
        {
            ground = parent.Ground;
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

        Hexagon hex = lockToNearsetHex(result.point);

        validPosition = (hex != null && result.collider == ground);
        if (validPosition)
        {
            placeAt = new Vector3(hex.parent.location.x, hex.surface, hex.parent.location.y);
            self.enabled = true;
            transform.position = placeAt;
        }
        else
        {
            self.enabled = false;
        }
        if (!Input.GetMouseButton(0))
        {
            bool no_overlap = (overlapping.Count == 0);
            bool cost = Globals.SpendResources(1, Globals.resourceTypes.FOOD) && Globals.SpendResources(1, Globals.resourceTypes.ENERGY);
            if (no_overlap && validPosition && cost && findNearestPylon())
            {
                addBuilding(hex);
            }

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
    bool findNearestPylon()
    {
        bool withinRadius = false;
        if (toGenerate.name == "Pylon")
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
