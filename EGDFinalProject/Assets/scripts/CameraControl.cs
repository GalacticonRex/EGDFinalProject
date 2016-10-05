using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public float CamSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 cf = Camera.main.transform.forward;
        Vector3 forward = Vector3.Normalize(new Vector3(cf.x, 0, cf.z));
        Vector3 right = Camera.main.transform.right;
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A))
            position -= right * Time.deltaTime * CamSpeed;
        if (Input.GetKey(KeyCode.D))
            position += right * Time.deltaTime * CamSpeed;
        if (Input.GetKey(KeyCode.S))
            position -= forward * Time.deltaTime * CamSpeed;
        if (Input.GetKey(KeyCode.W))
            position += forward * Time.deltaTime * CamSpeed;

        transform.position = position;
    }
}
