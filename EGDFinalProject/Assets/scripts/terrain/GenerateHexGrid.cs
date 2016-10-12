using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 0 = 000
// 1 = 001
// 2 = 010
// 3 = 011
// 4 = 100
// 5 = 101

public class GraphMap<T> where T : HexStack
{
    public class GraphIndexer
    {
        private Dictionary<int, T> mapping;
        public GraphIndexer(Dictionary<int, T> n)
        {
            mapping = n;
        }
        public T this[int key]
        {
            get
            {
                if (mapping == null)
                    return null;
                T output;
                if (!mapping.TryGetValue(key, out output))
                    return null;
                return output;
            }
            set
            {
                if (mapping == null)
                    return;
                mapping[key] = value;
            }
        }
    }

    private Dictionary<int, Dictionary<int, T>> mapping;
    public GraphIndexer this[int key]
    {
        get
        {
            Dictionary<int, T> dict;
            if ( !mapping.TryGetValue(key, out dict) )
            {
                dict = new Dictionary<int, T>();
                mapping[key] = dict;
            }
            return new GraphIndexer(dict);
        }
    }
    public GraphMap()
    {
        mapping = new Dictionary<int, Dictionary<int, T>>();
    }
}

public class vertex_surface
{
    List<Vector3> vertices;
    List<vertex_surface> linked;
}

public class HexStack
{
    private static readonly int[] adjacentX = { 1, 0, -1, -1, 0, 1 };
    private static readonly int[] adjacentY = { 1, 2, 1, -1, -2, -1 };
    private static int creation_index = 0;
    public static int reverse(int index)
    {
        return (index + 3) % 6;
    }
    public static int succ(int index)
    {
        return (index + 1) % 6;
    }
    public static int pred(int index)
    {
        return (index + 5) % 6;
    }

    public int locationX;
    public int locationY;
    public int vertical_offset;
    public float radius;
    public Vector2 location;
    public List<Hexagon> layers;
    public HexStack[] adjacent;
    public bool isFlagged;
    public int index_created;
    public int vertex_start; // initial vertex in parent buffer

    public float generation_veloc = 0f;
    public float generation_accel = 0f;
    public float generation_value = 0f;

    public HexStack(int lx, int ly, float x, float y, float r, int v = 0)
    {
        locationX = lx;
        locationY = ly;
        vertical_offset = v;
        radius = r;
        location = new Vector2(x, y);
        layers = new List<Hexagon>();
        adjacent = new HexStack[6] { null, null, null, null, null, null };
        vertex_start = -1;
        index_created = creation_index ++ ;
        isFlagged = false;
    }
    public HexStack(int lx, int ly, Vector2 loc, float r, int v = 0)
    {
        locationX = lx;
        locationY = ly;
        vertical_offset = v;
        radius = r;
        location = loc;
        layers = new List<Hexagon>();
        adjacent = new HexStack[6] { null, null, null, null, null, null };
        vertex_start = -1;
        index_created = creation_index ++ ;
        isFlagged = false;
    }

    public void UnsetAllFlags()
    {
        if (isFlagged)
        {
            isFlagged = false;
            foreach (HexStack hex in adjacent)
                hex.UnsetAllFlags();
        }
    }
    public void AddFloor(float value)
    {
        Hexagon newlayer = new Hexagon(this, value);
        int match0 = layers.Count;
        for ( int i=0;i<6;i++ )
        {
            HexStack adj = adjacent[i];
            if (adj == null)
                continue;
            int match1 = match0 + vertical_offset - adj.vertical_offset;
            if (match1 < 0 || match1 >= adj.layers.Count)
                continue;

            Hexagon hex = adj.layers[match1];
            int other = reverse(i);
            newlayer.connections[i] = hex;
            hex.connections[other] = newlayer;
            if (Mathf.Abs(hex.surface - newlayer.surface) > 1.5f)
            {
                newlayer.isRamp[i] = false;
                hex.isRamp[other] = false;
            }
            else
            {
                newlayer.isRamp[i] = true;
                hex.isRamp[other] = true;
            }
        }
        layers.Add(newlayer);
    }
    public HexStack CreateAdjacent(int direction, int offset = 0)
    {
        if (adjacent[direction] != null)
        {
            return null;
        }
        else
        {
            Vector2 added = Quaternion.AngleAxis(60.0f * direction + 30, Vector3.forward) * new Vector2(radius*Mathf.Sqrt(3), 0);
            HexStack ret = new HexStack(locationX + adjacentX[direction],
                                        locationY + adjacentY[direction],
                                        added + location, radius, vertical_offset + offset);

            Connect(ret, direction);

            SearchAdjacentCC(adjacent[succ(direction)], ret, direction, pred(direction));
            SearchAdjacentCCW(adjacent[pred(direction)], ret, direction, succ(direction));

            return ret;
        }
    }
    private void Connect(HexStack other, int direction)
    {
        adjacent[direction] = other;
        other.adjacent[reverse(direction)] = this;
    }
    private void SearchAdjacentCC(HexStack current, HexStack bind, int search, int arc)
    {
        if (current == null || current.adjacent[arc] != null)
            return;
        current.Connect(bind, arc);
        SearchAdjacentCC(current.adjacent[search], bind, pred(search), pred(arc));
    }
    private void SearchAdjacentCCW(HexStack current, HexStack bind, int search, int arc)
    {
        if (current == null || current.adjacent[arc] != null)
            return;
        current.Connect(bind, arc);
        SearchAdjacentCCW(current.adjacent[search], bind, succ(search), succ(arc));
    }
}

public class Hexagon {
    public HexStack parent;
    public float surface;

    public Hexagon[] connections = new Hexagon[6] { null, null, null, null, null, null };
    public bool[] isRamp = new bool[6] { false, false, false, false, false, false };
    private bool[] hasCliff = new bool[6] { false, false, false, false, false, false };

    public int vertex_start = -1; // initial vertex in parent buffer

    // search algorithm
    public int gScore = -1;
    public int fScore = -1;

    // placement values
    public int buildAccess = 0;
    public int connectionAccess = 0;

    public Vector3 Position
    { get { return new Vector3(parent.location.x, surface, parent.location.y); } }

    public Hexagon(HexStack stack, float layer)
    {
        parent = stack;
        surface = layer;
    }
    public void ResetScore()
    {
        if (gScore < 0) return;

        gScore = -1;
        fScore = -1;

        for (int i = 0; i < 6; i++)
            if (connections[i] != null)
                connections[i].ResetScore();
    }
    public int DistanceTo(Hexagon hex)
    {
        return Mathf.FloorToInt(Vector2.Distance(parent.location, hex.parent.location));
    }
}

public class GenerateHexGrid : MonoBehaviour {

    public float hexRadius = 1.0f;
    private HexStack root;
    private GraphMap<HexStack> map = new GraphMap<HexStack>();
    private GameObject surface;
    private GameObject cliffs;

    public float hexAnchor { get { return hexRadius * Mathf.Sqrt(3.0f) / 2.0f; } }
    public float hexInnerRadius { get { return hexRadius * 0.75f; } }

    public float hexXOffset { get { return hexRadius * 1.50f; } }
    public float hexYOffset { get { return hexRadius * Mathf.Sqrt(3.0f); } }

    public Collider Ground { get { return surface.GetComponent<MeshCollider>() as Collider; } }
    public GraphMap<HexStack>.GraphIndexer this[int key] { get { return map[key]; } }
    public HexStack Root {  get { return root; } }

    public HexStack GetTile(Vector3 location)
    {
        int x = Mathf.RoundToInt(location.x / hexXOffset);
        int y = Mathf.RoundToInt(location.z / hexYOffset) * 2 - (System.Math.Abs(x)%2);
        return map[x][y];
    }

    private void generateFloorSurface(List<Vector3> vertices, List<int> indices, HexStack hexes)
    {
        if (hexes.vertex_start != -1)
            return;
        hexes.vertex_start = vertices.Count;
        foreach (Hexagon hex in hexes.layers)
            generateFloorSurface(vertices, indices, hex);
        foreach (HexStack hex in hexes.adjacent)
            if( hex != null )
                generateFloorSurface(vertices, indices, hex);
    }
    private void generateFloorSurface(List<Vector3> vertices, List<int> indices, Hexagon hex)
    {
        Vector3[] local_vertices = new Vector3[7];
        int e = vertices.Count;
        int[] local_indices = new int[18]
        {
            e, e+2, e+1,
            e, e+3, e+2,
            e, e+4, e+3,
            e, e+5, e+4,
            e, e+6, e+5,
            e, e+1, e+6
        };
        hex.vertex_start = e;

        local_vertices[0] = new Vector3(hex.parent.location.x, 0, hex.parent.location.y);

        Quaternion rotation = Quaternion.AngleAxis(60, -Vector3.up);
        Vector3 rotating = new Vector3(hexRadius, 0, 0);

        for (int i=0;i<6;i++)
        {
            local_vertices[i + 1] = local_vertices[0] + rotating;
            rotating = rotation * rotating;
        }

        float total = 0.0f;
        for (int i=0;i<6;i+=1)
        {
            int a = i;
            int b = (i + 5) % 6;

            Hexagon hex0 = hex.connections[a];
            Hexagon hex1 = hex.connections[b];

            if (hex0 != null && hex.isRamp[a])
            {
                if (hex1 != null && hex.isRamp[b])
                    local_vertices[i + 1].y = (hex.surface + hex0.surface + hex1.surface) / 3.0f;
                else
                    local_vertices[i + 1].y = (hex.surface + hex0.surface) / 2.0f;
            }
            else if (hex1 != null && hex.isRamp[b])
                local_vertices[i + 1].y = (hex.surface + hex1.surface) / 2.0f;
            else
                local_vertices[i + 1].y = hex.surface;

            total += local_vertices[i + 1].y;
        }
        local_vertices[0].y = total / 6.0f;

        vertices.AddRange(local_vertices);
        indices.AddRange(local_indices);
    }

    private void generateCliffSurface(List<Vector3> vertices, List<int> indices, List<Vector3> surf_vertices, List<int> surf_indices, HexStack hexes)
    {
        if (hexes.isFlagged)
            return;
        hexes.isFlagged = true;
        foreach (Hexagon hex in hexes.layers)
            generateCliffSurface(vertices, indices, surf_vertices, surf_indices, hex);
        foreach (HexStack hex in hexes.adjacent)
            if (hex != null)
                generateCliffSurface(vertices, indices, surf_vertices, surf_indices, hex);
    }
    private void generateCliffSurface(List<Vector3> vertices, List<int> indices, List<Vector3> surf_vertices, List<int> surf_indices, Hexagon hex)
    {
        int st = hex.vertex_start;
        for (int i = 0; i < 6; i++)
        {
            // if there is no ramp then we need a cliff
            if (hex.isRamp[i])
                continue;

            int a = i;
            int b = HexStack.reverse(i);
            Vector3 selfA = surf_vertices[hex.vertex_start + a + 1];
            Vector3 selfB = surf_vertices[hex.vertex_start + HexStack.succ(a) + 1];
            Vector3 otherA;
            Vector3 otherB;
            if (hex.connections[i] == null)
            {
                otherA = new Vector3(selfA.x, -100, selfA.z);
                otherB = new Vector3(selfB.x, -100, selfB.z);
            }
            else
            {
                Hexagon other = hex.connections[i];
                otherA = surf_vertices[other.vertex_start + HexStack.succ(b) + 1];
                otherB = surf_vertices[other.vertex_start + b + 1];
            }

            Vector3[] local_vertices = new Vector3[4];
            int[] local_indices = new int[6];

            local_vertices[0] = selfA;
            local_vertices[1] = selfB;

            local_vertices[2] = otherA;
            local_vertices[3] = otherB;

            int e = vertices.Count;
            local_indices[0] = e + 0;
            local_indices[1] = e + 1;
            local_indices[2] = e + 2;

            local_indices[3] = e + 1;
            local_indices[4] = e + 3;
            local_indices[5] = e + 2;

            vertices.AddRange(local_vertices);
            indices.AddRange(local_indices);
        }
    }

    private GameObject generateMesh(List<Vector3> vertices, List<int> indices)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        GameObject go = new GameObject("GeneratedMesh");
        go.transform.parent = transform;
        go.layer = LayerMask.NameToLayer("Ground");

        MeshFilter mfilter = go.AddComponent<MeshFilter>();
        mfilter.mesh = mesh;

        MeshRenderer mrender = go.AddComponent<MeshRenderer>();
        mrender.material = new Material(Shader.Find("Diffuse"));
        mrender.material.color = Color.gray;

        MeshCollider mcollid = go.AddComponent<MeshCollider>();
        mcollid.sharedMesh = mesh;

        return go;
    }

    private void generateTerrain(HexStack src, int remaining, float scale)
    {
        Queue<HexStack> queue = new Queue<HexStack>();
        float initial = (float)remaining;
        float effect = 1f / scale;
        float offset = 1f / (scale * scale);

        queue.Enqueue(src);
        int count = 0;

        while ( queue.Count > 0 && count < remaining)
        {
            HexStack hex = queue.Dequeue();
            int empty = 1;
            for (int i = 0; i < 6; i++)
            {
                HexStack s0 = hex.CreateAdjacent(i);
                if (s0 == null)
                {
                    if (hex.adjacent[i].layers.Count == 0)
                        empty++;
                    continue;
                }

                map[s0.locationX][s0.locationY] = s0;

                s0.generation_accel = hex.generation_accel * 0.99f + Random.Range(-1,1) * offset;
                s0.generation_veloc = hex.generation_veloc + s0.generation_accel;
                s0.generation_value = hex.generation_value + s0.generation_veloc;

                queue.Enqueue(s0);
            }
            float val = Random.value * count * empty * offset * offset;
            if ( val <= Mathf.Abs(hex.generation_value))
            {
                if (hex.location.y < -10000 && Random.value * count * count > initial)
                    hex.AddFloor(100.0f);
                else
                    hex.AddFloor(Random.Range(-4.0f * hex.generation_value / scale - 0.2f,
                                              10.0f * hex.generation_value / scale + 0.2f));
            }
            count++;
        }
    }

    void Start()
    {
        map[0][0] = root = new HexStack(0,0, 0,0, hexRadius);
        generateTerrain(root, 1000, 10.0f);

        List<Vector3> surface_vertices = new List<Vector3>();
        List<int> surface_indices = new List<int>();
        generateFloorSurface(surface_vertices, surface_indices, root);
        surface = generateMesh(surface_vertices, surface_indices);

        List<Vector3> cliff_vertices = new List<Vector3>();
        List<int> cliff_indices = new List<int>();
        generateCliffSurface(cliff_vertices, cliff_indices, surface_vertices, surface_indices, root);
        cliffs = generateMesh(cliff_vertices, cliff_indices);
    }
}
