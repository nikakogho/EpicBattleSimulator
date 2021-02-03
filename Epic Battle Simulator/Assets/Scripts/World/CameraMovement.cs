using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 15;
    float v = 0, h = 0;
    float scale = 0;

    public static bool began = false;

    void Update()
    {
        h += Input.GetAxis("Horizontal") * Time.deltaTime;
        v += Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow)) scale += Time.deltaTime;
        else if (Input.GetKey(KeyCode.DownArrow)) scale -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * h * speed;
        transform.position += Vector3.forward * v * speed;
        transform.position -= Vector3.up * scale * speed;

        h = 0;
        v = 0;
        scale = 0;
    }
}
