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
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, 1<<LayerMask.NameToLayer("Ground"));
        Debug.Log(result.distance.ToString());

        Hexagon hex = lockToNearsetHex(result.point);

        validPosition = (hex != null && result.collider == ground);
        if (toGenerate.GetComponent<PylonInstance>() != null)
        {
            Debug.Log("This is a pylon");

        }
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

         //   if (overlapping.Count > 0) Debug.Log(overlapping[0]);
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
