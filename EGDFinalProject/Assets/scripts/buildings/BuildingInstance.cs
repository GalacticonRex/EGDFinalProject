using UnityEngine;
using System.Collections;

public class BuildingInstance : MonoBehaviour {
    public int index;
    public GameObject[] paths;
    private Vector3 screenPoint;
    private Vector3 offset;
    RaycastHit hit;
    float dist;
    Vector3 dir;
    // Use this for initialization
    void Start()
    {
        gameObject.layer = Globals.BUILDING_LAYER;
    }	
    void setIndex(int newIndex)
    {
        index = newIndex;
    }
	// Update is called once per frame
	void Update () {

    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    //Drag and move building
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        //locking y axis until raycast function implemented

        gameObject.transform.position = new Vector3(curPosition.x, gameObject.transform.position.y, curPosition.z);

        hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Vector3 p = hit.point;
            if (hit.distance > 0)
            {
                p.y += 0.5f;
                transform.position = p;
            }
        }
        else if (Physics.Raycast(transform.position, Vector3.up, out hit))
        {
            transform.position = hit.point;
        }
        else
        {

        }
    }
}
