using UnityEngine;
using System.Collections;

public class PathInstance : MonoBehaviour {
    private Vector3[] points;
    private float total_distance;
    private float[] distances;
    private float[] cumulative;
    private LineRenderer liner;
    private BuildingInstance source;
    private BuildingInstance dest;

    public Vector3 this[int key] { get { return points[key]; } }
    public Vector3 start { get { return points[0];  } }
    public Vector3 end { get { return points[points.Length-1]; } }
    public float Length { get { return total_distance; } }

    public void Build(BuildingInstance a, BuildingInstance b, Vector3[] path)
    {
        source = a;
        dest = b;

        a.AddConnection(this, b);
        b.AddConnection(this, a);

        liner = GetComponent<LineRenderer>();
        liner.SetVertexCount(path.Length);  
        liner.SetPositions(path);

        points = path;
        total_distance = 0.0f;
        distances = new float[points.Length-1];
        cumulative = new float[points.Length-1];

        for (int i=0;i<distances.Length;i++)
        {
            float dist = (points[i + 1] - points[i]).magnitude;
            distances[i] = dist;
            cumulative[i] = total_distance;
            total_distance += dist;
        }
        Debug.Log("Total Distance: " + total_distance.ToString());
    }
    public float GetDirectionFromSource(BuildingInstance b)
    {
        if (b == source)
            return 1.0f;
        else if (b == dest)
            return -1.0f;
        else
            return 0.0f;
    }
    public Vector3 GetPositionAlongPath(float travelled)
    {
        if (travelled < 0)
            return points[0];
        else if (travelled >= total_distance)
            return points[points.Length - 1];

        for (int i=0;i<cumulative.Length;i++)
        {
            if (travelled >= cumulative[i] && travelled < cumulative[i] + distances[i])
            {
                float scaled = (travelled - cumulative[i]) / distances[i];
                return points[i] * (1-scaled) + points[i + 1] * scaled;
            }
        }
        throw new System.Exception("Should not get to this point");
    }
}
