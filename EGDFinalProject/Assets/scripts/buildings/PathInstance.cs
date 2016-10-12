using UnityEngine;
using System.Collections;

public class PathInstance : MonoBehaviour {
    private LineRenderer liner;
    private BuildingInstance source;
    private BuildingInstance dest;

    public void Build(BuildingInstance a, BuildingInstance b, Vector3[] path)
    {
        a.AddConnection(this, b);
        b.AddConnection(this, a);
        liner = GetComponent<LineRenderer>();
        liner.SetVertexCount(path.Length);  
        liner.SetPositions(path);
    }
}
