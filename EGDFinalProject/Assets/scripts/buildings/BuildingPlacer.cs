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

    void Start()
    {
        gameObject.layer = Globals.PLACEMENT_LAYER;
        GenerateHexGrid g = FindObjectOfType<GenerateHexGrid>();
        if (g != null)
        {
            ground = (Collider)g.GetComponent<MeshCollider>();
        }
        self = GetComponent<MeshRenderer>();
        source = self.material;
        overlapping = new HashSet<BuildingInstance>();
    }
	
	// Update is called once per frame
	void Update () {
        if (overlapping.Count == 0)
            source.color = Active;
        else
            source.color = Invalid;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, Globals.GROUND_LAYER);
        validPosition = (result.collider == ground);
        if (result.collider != null)
        {
            placeAt = result.point;
            transform.position = placeAt;
        }
        if (!Input.GetMouseButton(0))
        {
            if (overlapping.Count == 0 && validPosition && Globals.SpendResources(100))
            {
                GameObject building = Instantiate(toGenerate);
                building.transform.position = placeAt;
            }
            Destroy(gameObject);
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
