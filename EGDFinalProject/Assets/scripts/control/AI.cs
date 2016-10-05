using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AI : MonoBehaviour {
    public float speed, maxSpeed;
    public bool alive;
    public Transform targetNode;
    public BuildingNode[] currentRoute;
    public int routeIndex;
    private Rigidbody rigidBdy;
    public enum State
    {
        TRAVELLING,
        WORKING
    }
    public State state;
    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.TRAVELLING:
                    //travel();
                    move(targetNode.position);
                    break;
                case State.WORKING:
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }
	// Use this for initialization
	void Start () {
        alive = true;
        rigidBdy = GetComponent<Rigidbody>();
        speed = 5f;
        routeIndex = 0;
        StartCoroutine("FSM");
    }

    // Update is called once per frame
    void Update () {
        if (routeIndex > currentRoute.Length - 1)
        {
            routeIndex = 0;
        }
        targetNode = currentRoute[routeIndex].transform;
    }
    public void travel()
    {
        Debug.Log(routeIndex);
        if (currentRoute[routeIndex] == null) return;
        if (Vector3.Distance(currentRoute[routeIndex].transform.position, transform.position) < 0.2f)
        {
            routeIndex++;
            if (routeIndex > currentRoute.Length) routeIndex = 0;
        }
        if (currentRoute.Length > 0)
        {
            targetNode = currentRoute[routeIndex].transform;
        }
    }
    public void move(Vector3 dest)
    {
        Vector3 dir = (dest - transform.position).normalized;
        if (speed > maxSpeed) speed = maxSpeed;
        rigidBdy.velocity = dir * speed;
    }
}
