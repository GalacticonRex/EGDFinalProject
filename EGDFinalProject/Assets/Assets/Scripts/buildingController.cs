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
        toggleConnection(0, 1);
        toggleConnection(2, 1);
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine("updateAllPaths");

    }
    void initBuildings()    
    {
        buildings = GetComponentsInChildren<BuildingNode>();
        
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].paths = new GameObject[buildings.Length];
            Debug.Log(buildings[i].name);
            buildings[i].SendMessage("setIndex", i);
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
                if (i == j) indexTable[i, j] = 1;
                else indexTable[i, j] = 0;
            }
        }
    //    printTable();
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
                if (i == j) continue;
                if (path == null && indexTable[i, j] == 1)
                {
                    //path = Instantiate(, buildings[i].transform);
                    GameObject pref = (GameObject)Instantiate(pathPrefab, buildings[i].transform);
                    pref.name = i + "," + j;
                    buildings[i].paths[i] = pref;
                    printTable();
                    // path = buildings[i].gameObject.AddComponent<LineRenderer>();
                }
                if (buildings[i].paths[i]) updatePath(buildings[i].paths[i].GetComponent<LineRenderer>(), i, j);
            }

        }
        yield return new WaitForSeconds(5f);
    }
    void updatePath(LineRenderer path, int i, int j)
    {
        if (path == null) return;
        path.SetVertexCount(2);
        path.SetPosition(0, buildings[i].transform.position);
        path.SetPosition(1, buildings[j].transform.position);
        path.SetWidth(0.2f,0.2f);
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
