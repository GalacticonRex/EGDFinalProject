using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 0 = 000
// 1 = 001
// 2 = 010
// 3 = 011
// 4 = 100
// 5 = 101

public class vertex_surface
{
    List<Vector3> vertices;
    List<vertex_surface> linked;
}

public class HexStack
{
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

    public int vertical_offset;
    public float radius;
    public Vector2 location;
    public List<Hexagon> layers;
    public HexStack[] adjacent;
    public int vertex_start; // initial vertex in parent buffer
    public int index_created;

    public HexStack(float x, float y, float r, int v = 0)
    {
        vertical_offset = v;
        radius = r;
        location = new Vector2(x, y);
        layers = new List<Hexagon>();
        adjacent = new HexStack[6] { null, null, null, null, null, null };
        vertex_start = -1;
        index_created = creation_index ++ ;
    }
    public HexStack(Vector2 loc, float r, int v = 0)
    {
        vertical_offset = v;
        radius = r;
        location = loc;
        layers = new List<Hexagon>();
        adjacent = new HexStack[6] { null, null, null, null, null, null };
        vertex_start = -1;
        index_created = creation_index ++ ;
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
            HexStack ret = new HexStack(added + location, radius, vertical_offset + offset);

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
    public Hexagon[] connections;
    public bool[] isRamp;
    private bool searchFlag;

    public Hexagon(HexStack stack, float layer)
    {
        parent = stack;
        surface = layer;

        connections = new Hexagon[6] { null, null, null, null, null, null };
        isRamp = new bool[6] { true, true, true, true, true, true };

        searchFlag = false;
    }

    //------------- Modify Flags ---------------------------------------------
    public void UnsetAllFlags()
    {
        if (searchFlag)
        {
            searchFlag = false;
            foreach (Hexagon hex in connections)
                hex.UnsetAllFlags();
        }
    }
}

public class GenerateHexGrid : MonoBehaviour {

    public float hexRadius = 1.0f;
    HexStack root;

    private void generateStackSurface(List<Vector3> vertices, List<int> indices, HexStack hexes)
    {
        if (hexes.vertex_start != -1)
            return;
        hexes.vertex_start = vertices.Count;
        foreach (Hexagon hex in hexes.layers)
            generateFloorSurface(vertices, indices, hex);
        foreach(HexStack hex in hexes.adjacent)
            if( hex != null )
                generateStackSurface(vertices, indices, hex);
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

    private void generateMesh(List<Vector3> vertices, List<int> indices)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        MeshFilter mfilter = gameObject.AddComponent<MeshFilter>();
        mfilter.mesh = mesh;

        MeshRenderer mrender = gameObject.AddComponent<MeshRenderer>();
        mrender.material = new Material(Shader.Find("Diffuse"));
        mrender.material.color = Color.gray;

        MeshCollider mcollid = gameObject.AddComponent<MeshCollider>();
        mcollid.sharedMesh = mesh;
    }

    private void GenerateTerrain(HexStack src, int remaining)
    {
        Queue<HexStack> queue = new Queue<HexStack>();

        queue.Enqueue(src);
        int count = 0;

        while ( queue.Count > 0 && count < remaining)
        {
            HexStack hex = queue.Dequeue();
            hex.AddFloor(Random.Range(-1.0f, 1.0f));
            for (int i = 0; i < 6; i++)
            {
                HexStack s0 = hex.CreateAdjacent(i);
                if (s0 == null)
                    continue;
                queue.Enqueue(s0);
            }
            count++;
        }
    }

    void Start()
    {
        root = new HexStack(0, 0, hexRadius);
        GenerateTerrain(root, 200);
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        generateStackSurface(vertices, indices, root);
        generateMesh(vertices, indices);
    }
}
