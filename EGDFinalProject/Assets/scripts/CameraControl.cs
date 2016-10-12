using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public float CamSpeed = 1.0f;
    private Vector2 oldMouse;
    private Camera cam;

    void Start()
    {
        oldMouse = Input.mousePosition;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = transform.rotation;

        if (Input.GetMouseButton(1) && oldMouse.x != Input.mousePosition.x)
        {
            float delta = Input.mousePosition.x - oldMouse.x;
            float scaled = Mathf.Pow(Mathf.Abs(delta), 0.6f) * delta;
            rotation = Quaternion.AngleAxis(scaled / 16.0f, Vector3.up) * rotation;
        }

        Vector3 cf = Camera.main.transform.forward;
        Vector3 forward = Vector3.Normalize(new Vector3(cf.x, 0, cf.z));
        Vector3 right = Camera.main.transform.right;
        Vector3 position = transform.position;

        if (Input.mouseScrollDelta.magnitude != 0)
            cam.orthographicSize = Mathf.Max(2.0f, cam.orthographicSize -    Input.mouseScrollDelta.y);

        float spd = cam.orthographicSize / 5.0f * CamSpeed;

        if (Input.GetKey(KeyCode.A))
            position -= right * Time.deltaTime * spd;
        if (Input.GetKey(KeyCode.D))
            position += right * Time.deltaTime * spd;
        if (Input.GetKey(KeyCode.S))
            position -= forward * Time.deltaTime * spd;
        if (Input.GetKey(KeyCode.W))
            position += forward * Time.deltaTime * spd;

        transform.position = position;
        transform.rotation = rotation;
        oldMouse = Input.mousePosition;
    }
}
