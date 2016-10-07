using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class BuildingController : MonoBehaviour {
  //  public BuildingNode[] list;
    public BuildingInstance[] list;
    public int[,] indexTable;
    public int rows, columns;
    public GameObject pathPrefab;
    public int numBuildings;

	// Use this for initialization
	void Start () {
        initBuildings();
        createIndexTable(list.Length);
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine("updateAllPaths");
         numBuildings = GameObject.FindGameObjectsWithTag("building").Length;

        //addBuilding();
        // printTable();
    }
    public void initBuildings()    
    {
        GameObject[] b = GameObject.FindGameObjectsWithTag("building");
        if (b.Length > 0)
        {
            list = new BuildingInstance[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                list[i] = addBuilding(b[i], i);
                list[i].SendMessage("setIndex", i);
            }
        }
        else
        {
            list = new BuildingInstance[4];

        }

    }
    //This is a lookup table that uses the index of each building to reference its relationship
    /*      i
     *  j [ 1 0 
     *      0 1 ]
     * 
     * If i == j, this is the relationship between the same node, so we can ignore that in most cases
     * 
     * If i = 0, j = 1, this refers to the relationship between building index 0 and building index 1
     * */
    public void handleIndexTable(int size)
    {
        int[,] copy = copyTable(indexTable);
        printTable(copy);
        createIndexTable(size);
        //testing path connection

        for (int i = 0; i < numBuildings; i++)
        {
            for (int j = 0; j < numBuildings; j++)
            {
                indexTable[i, j] = copy[i, j];
            }
        }

        printTable(indexTable);
    }
    void createIndexTable(int size)
    {
        columns = size;
        rows = columns;
        //  Debug.Log(rows);
        indexTable = new int[size, size];
        //   Debug.Log(list.Length);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                indexTable[i, j] = i == j ? 1 : 0;
            }
        }
        printTable(indexTable);
    }
     int[,] copyTable(int[,] table)
    {
        int length = numBuildings;
        int[,] copyTable = new int[length, length];
        Debug.Log("Rows: " + rows + "Cols: " + columns);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                copyTable[i, j] = indexTable[i, j];
            }
        }
        return copyTable;
    }
    void reallocTable(int[,] table, int size)
    {

    }
    public BuildingInstance addBuilding(GameObject building, int index)
    {
        if (list.Length <= 0) initBuildings();
        BuildingInstance b = building.GetComponent<BuildingInstance>();
        b.paths = new GameObject[10];
        return b;
    }
    void toggleConnection(int i, int j)
    {
        if (i == j) return;
        indexTable[i, j] = (indexTable[i, j] == 0 ? 1 : 0);

    }
    IEnumerator updateAllPaths()
    {
        if (numBuildings <= 1) yield return null;

        for (int i = 0; i < numBuildings; i++)
        {
            GameObject path = list[i].paths[i];
            if (list[i].paths[i] != null) path = list[i].paths[i].gameObject;
            if (list[i].paths[i] == null) yield return null;

            for (int j = 0; j < numBuildings; j++)
            {
                if (i > numBuildings || j > numBuildings) yield return null;
                path = list[i].paths[j];

                if (i == j) continue;
                if (path == null && indexTable[i, j] == 1)
                {
                    //path = Instantiate(, list[i].transform);
                    GameObject pref = (GameObject)Instantiate(pathPrefab, list[i].transform);
                    pref.name = i + "," + j;
                    list[i].paths[j] = pref;
                    updatePath(list[i].paths[j].GetComponent<LineRenderer>(), i, j);

                }
                else if (path != null && indexTable[i, j] == 1)
                {
                    updatePath(list[i].paths[j].GetComponent<LineRenderer>(), i, j);
                }
            }

        }
        yield return new WaitForEndOfFrame();
    }
    void updatePath(LineRenderer path, int i, int j)
    {
        if (path == null) return;
        path.SetVertexCount(2);
        path.SetPosition(0, list[i].transform.position);
        path.SetPosition(1, list[j].transform.position);
        path.SetWidth(0.2f,0.2f);
    }
   /* BuildingNode getBuilding(int index)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].index == index)
            {
                return list[i];
            }
        }
        return null;
    }*/
    void getConnected(int index)
    {
        for (int j = 0; j < columns; j++)
        {
            if (index == j) continue;
            if (indexTable[index, j] == 1)
            {
                //  Debug.Log(indexTable[i, j]);
            }
        }
    }
    public void printTable(int[,] target)
    {
        string table = "";
        for (int i = 0; i < rows; i++)
        {
            table += "[";
            for (int j = 0; j < columns; j++)
            {
                table += " " + indexTable[i, j];
            }
            table += " ] \n";
        }
        Debug.Log(table);
    }

}
