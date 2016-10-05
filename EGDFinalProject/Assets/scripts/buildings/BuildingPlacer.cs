using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour {
    public GameObject toGenerate;
    private Collider ground;
    private MeshRenderer self;
    private bool validPosition;
    private Vector3 placeAt;

    void Start()
    {
        gameObject.layer = Globals.PLACEMENT_LAYER;
        GenerateHexGrid g = FindObjectOfType<GenerateHexGrid>();
        if (g != null)
        {
            ground = (Collider)g.GetComponent<MeshCollider>();
        }
        self = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, Globals.GROUND_LAYER);
        validPosition = (result.collider == ground);
        Debug.Log(validPosition);
        if (result.collider != null)
        {
            placeAt = result.point;
            transform.position = placeAt;
        }
        if (!Input.GetMouseButton(0))
        {
            if (validPosition && Globals.SpendResources(100))
            {
                GameObject building = Instantiate(toGenerate);
                building.transform.position = placeAt;
            }
            Destroy(gameObject);
        }
	}
}
