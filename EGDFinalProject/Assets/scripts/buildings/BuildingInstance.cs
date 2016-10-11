using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingInstance : MonoBehaviour {
    public int index;
    public Hexagon ground;
    public GameObject pathPlacer;
    public GameObject[] paths;
    private Vector3 screenPoint;
    private Vector3 offset;
    private HashSet<PathInstance> connections;
    private PathPlacer current;
    private MeshRenderer render;
    private Material material;

    public void AddConnection(PathInstance p)
    {
        connections.Add(p);
    }

    // Use this for initialization
    void Start()
    {
        gameObject.layer = Globals.BUILDING_LAYER;
        current = null;
        connections = new HashSet<PathInstance>();
        render = GetComponent<MeshRenderer>();
        material = render.material;
    }
    void setIndex(int newIndex)
    {
        index = newIndex;
    }
    void OnMouseEnter()
    {
        material.color = Color.green;
    }
    void OnMouseExit()
    {
        material.color = Color.white;
    }
    void OnMouseDown()
    {
        GameObject go = Instantiate(pathPlacer);
        PathPlacer pp = go.GetComponent<PathPlacer>();
        pp.source = this;
    }
}
