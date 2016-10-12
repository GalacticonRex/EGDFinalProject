using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreComparer : IComparer<int>
{
    public int Compare(int x, int y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return 1;   // Handle equality as beeing greater
        else
            return result;
    }
}

public class PathPlacer : MonoBehaviour {
    public BuildingInstance source;
    public Color Invalid;
    public Color Valid;
    public float Weight = 1.0f;
    public GameObject instance;
    public AudioClip PlayOnPlace;
    private Hexagon goal;
    private bool pathValid = false;
    private GenerateHexGrid parent;
    private LineRenderer liner;
    private Material material;
    private List<Vector3> positions = new List<Vector3>();
    private List<GameObject> elements = new List<GameObject>();

    static public Hexagon[] pathToPoint(Hexagon start, Hexagon goal)
    {
        if (start == goal)
            return new Hexagon[1] { start };

        HashSet<Hexagon> closed = new HashSet<Hexagon>();
        SortedList<int, Hexagon> open = new SortedList<int, Hexagon>(new ScoreComparer());
        Dictionary<Hexagon, Hexagon> efficient = new Dictionary<Hexagon, Hexagon>();

        open.Add(0, start);
        start.gScore = 0;
        start.fScore = goal.DistanceTo(start);

        bool found = false;
        while (open.Count > 0)
        {
            Hexagon hex = open.Values[0];
            if (hex == goal)
            {
                found = true;
                break;
            }
            open.RemoveAt(0);
            closed.Add(hex);
            for ( int i=0;i<6;i++ )
            {
                if (!hex.isRamp[i])
                    continue;

                Hexagon adj = hex.connections[i];
                if (adj == null || closed.Contains(adj))
                    continue;

                int score = hex.gScore + 1;
                if (!open.ContainsValue(adj))
                    open.Add(score, adj);
                else if (score >= adj.gScore)
                    continue;

                efficient[adj] = hex;
                adj.gScore = score;
                adj.fScore = score + goal.DistanceTo(adj);
            }
        }

        start.ResetScore();

        if (found)
        {
            List<Hexagon> path = new List<Hexagon>();
            Hexagon current = goal;

            path.Add(current);
            while ( efficient.TryGetValue(current, out current) )
                path.Add(current);

            path.Reverse();
            return path.ToArray();
        }
        else
        {
            return null;
        }
    }
    private void BuildPath(Hexagon[] path)
    {
        pathValid = (path != null);
        if (path == null)
            liner.enabled = false;
        else
        {
            positions.Clear();
            for (int i = 0; i < path.Length; i++)
            {
                float hgt = path[i].surface + 0.5f;

                Vector2 current = path[i].parent.location;
                Vector2 fromLast, toNext;
                Vector3 blend0, blend1;

                if (i == 0)
                {
                    fromLast = source.ground.parent.location;
                    blend0 = new Vector3(fromLast.x, path[i].surface, fromLast.y);
                }
                else
                {
                    Vector2 dif = path[i].parent.location - path[i - 1].parent.location;
                    fromLast = path[i - 1].parent.location + dif * Weight / dif.magnitude;
                    blend0 = new Vector3(fromLast.x, hgt, fromLast.y);
                }
                if (i + 1 == path.Length)
                {
                    toNext = goal.parent.location;
                    blend1 = new Vector3(toNext.x, path[i].surface, toNext.y);
                }
                else
                {
                    Vector2 dif = path[i + 1].parent.location - path[i].parent.location;
                    toNext = path[i].parent.location + dif * (1.0f - Weight) / dif.magnitude;
                    blend1 = new Vector3(toNext.x, hgt, toNext.y);
                }

                positions.Add(blend0);
                positions.Add(blend1);
            }
            liner.enabled = true;
            liner.SetVertexCount(positions.Count);
            liner.SetPositions(positions.ToArray());
        }
    }

    void Start()
    {
        parent = FindObjectOfType<GenerateHexGrid>();
        liner = FindObjectOfType<LineRenderer>();
        material = liner.material;
        material.color = Invalid;
    }

    // Update is called once per frame
    void Update()
    {
        BuildingInstance inst = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray, out result, float.PositiveInfinity, Globals.GROUND_LAYER | Globals.BUILDING_LAYER);

        if (result.collider != null)
        {
            inst = result.collider.GetComponent<BuildingInstance>();
            if (inst != null)
            {
                material.color = Valid;
                Hexagon hex = inst.ground;
                if (goal != hex)
                {
                    goal = hex;
                    Hexagon[] path = pathToPoint(source.ground, goal);
                    BuildPath(path);
                }
            }
            else
            {
                material.color = Invalid;
                HexStack stack = parent.GetTile(result.point);
                if (stack != null && stack.layers.Count != 0)
                {
                    Hexagon hex = stack.layers[0];
                    if (hex != goal)
                    {
                        goal = hex;
                        Hexagon[] path = pathToPoint(source.ground, goal);
                        BuildPath(path);
                    }
                }
            }
        }

        if( Input.GetMouseButtonUp(0) )
        {
            if( pathValid && inst != null && inst != source && !inst.ConnectedTo(source) )
            {
                if( PlayOnPlace != null )
                    AudioSource.PlayClipAtPoint(PlayOnPlace, Camera.main.transform.position);
                GameObject go = Instantiate(instance);
                PathInstance p = go.GetComponent<PathInstance>();
                p.Build(source, inst, positions.ToArray());
            }
            Destroy(gameObject);
        }
    }
}
