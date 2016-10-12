using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
    public int index;
    public LineRenderer path;
    IEnumerator connectNodes()
    {

        yield return new WaitForSeconds(1f);
    }
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
    }

}
/*
 * 
 * 
 *                 LineRenderer path = nodeToCheck.gameObject.GetComponent<LineRenderer>();
                if (path == null)
                {
                    Debug.Log("not connected");
                }
                else
                {
                    path.SetVertexCount(2);
                    path.SetPosition(0, transform.position);
                    path.SetPosition(0, nodeToCehck.gameObject.transform.position);
                    Debug.Log("connected");
                }
                       for (int i = 0; i < connectedNodes.Length; i++)
        {
            LineRenderer path = connectedNodes[i].GetComponent<LineRenderer>();
            if (path == null && GetComponent<LineRenderer>() == null)
            {
                if (connectedNodes[i].index == index) continue;
                connectedNodes[i].gameObject.AddComponent<LineRenderer>();
                path = connectedNodes[i].GetComponent<LineRenderer>();
                path.SetVertexCount(2);
                path.SetPosition(0, transform.position);
                path.SetPosition(1, connectedNodes[i].gameObject.transform.position);

            }
            else
            {
                path = connectedNodes[i].GetComponent<LineRenderer>();
                if (!path) continue;
                path.SetVertexCount(2);
                path.SetPosition(0, transform.position);
                path.SetPosition(1, connectedNodes[i].gameObject.transform.position);

            }
            */
       
