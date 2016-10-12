using UnityEngine;
using System.Collections;

public class EnvironmentInstance : MonoBehaviour {
    public GenerateHexGrid parent;
    private Collider ground;
    private Material source;
    private MeshRenderer self;
    // Use this for initialization
    void Start () {
        parent = FindObjectOfType<GenerateHexGrid>();
        if (parent != null)
        {
            ground = parent.Ground;
        }
        self = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
