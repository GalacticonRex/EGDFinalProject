using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public PathInstance path;
    public float speed;
    private float total;

    void Start()
    {
        if (speed < 0)
            total = path.Length;
        else
            total = 0.0f;
    }

    void Update()
    {
        total += speed * Time.deltaTime;
        Vector3 pt = path.GetPositionAlongPath(total);
        Vector3 move = pt - transform.position;
        if ( Mathf.Abs(move.x) > 0.01f )
        {
            if (move.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        if (pt == path.end || pt == path.start)
            Destroy(gameObject);
        transform.position = pt;
    }
}
