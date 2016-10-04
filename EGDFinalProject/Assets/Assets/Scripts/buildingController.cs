using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class buildingController : MonoBehaviour {
    public BuildingNode[] buildings;
    public int[,] indexTable;
    public int rows, columns;
    public GameObject pathPrefab;
	// Use this for initialization
	void Start () {
        initBuildings();
        createIndexTable();
        toggleConnection(0, 2);
        toggleConnection(0, 4);
        toggleConnection(0, 3);
        toggleConnection(0, 1);

    }

    // Update is called once per frame
    void Update () {
        StartCoroutine("updateAllPaths");
        //addBuilding();
    }
    void initBuildings()    
    {
        buildings = GetComponentsInChildren<BuildingNode>();
        
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].paths = new GameObject[buildings.Length];
            buildings[i].SendMessage("setIndex", i);
            Debug.Log(buildings[i].name + " " + i);

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
    void createIndexTable()
    {
        columns = buildings.Length;
        rows = columns;
      //  Debug.Log(rows);
        indexTable = new int[buildings.Length, buildings.Length];

        //   Debug.Log(buildings.Length);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                indexTable[i, j] = i == j ? 1 : 0;
            }
        }
    //    printTable();
    }
    void addBuilding()
    {
        columns = buildings.Length;
        rows = columns;
        //  Debug.Log(rows);
        int[,] copyTable = new int[buildings.Length + 1, buildings.Length + 1];
        //   Debug.Log(buildings.Length);
        int i = 0, j = 0;
        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < columns; j++)
            {
                copyTable[i, j] = indexTable[i, j];
            }
        }
        columns++; rows++;
        Debug.Log(i + " " + j);

    }
    void toggleConnection(int i, int j)
    {
        if (i == j) return;
        indexTable[i, j] = (indexTable[i, j] == 0 ? 1 : 0);

    }
    IEnumerator updateAllPaths()
    {
      //  printTable();
    //    Debug.Log(rows);
        for (int i = 0; i < rows; i++)
        {
            GameObject path = buildings[i].paths[i];
            if (buildings[i].paths[i] != null) path = buildings[i].paths[i].gameObject;

            for (int j = 0; j < columns; j++)
            {
                path = buildings[i].paths[j];
                if (i == j) continue;
                if (path == null && indexTable[i, j] == 1)
                {
                    //path = Instantiate(, buildings[i].transform);
                    GameObject pref = (GameObject)Instantiate(pathPrefab, buildings[i].transform);
                    pref.name = i + "," + j;
                    buildings[i].paths[j] = pref;
                    updatePath(buildings[i].paths[j].GetComponent<LineRenderer>(), i, j);

                }
                else if (path != null && indexTable[i, j] == 1)
                {
                    updatePath(buildings[i].paths[j].GetComponent<LineRenderer>(), i, j);
                }
            }

        }
        yield return new WaitForEndOfFrame();
    }
    void updatePath(LineRenderer path, int i, int j)
    {
        if (path == null) return;
        path.SetVertexCount(2);
        path.SetPosition(0, buildings[i].transform.position);
        path.SetPosition(1, buildings[j].transform.position);
        path.SetWidth(0.2f,0.2f);
    }
    BuildingNode getBuilding(int index)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].index == index)
            {
                return buildings[i];
            }
        }
        return null;
    }
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
    void printTable()
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
