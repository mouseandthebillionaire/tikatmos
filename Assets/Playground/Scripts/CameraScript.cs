using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float xBounds, yBounds;
    public float speed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Move the camera UP
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (yPos <= yBounds) yPos += speed;
        }
        // Move the camera DOWN
        if (Input.GetKey(KeyCode.DownArrow)) {
            if (yPos >= -yBounds) yPos -= speed;
        }

        // Move the camera LEFT
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (xPos >= -xBounds) xPos -= speed;
        }
        // Move the camera RIGHT
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (xPos <= xBounds) xPos += speed;
        }

        float zoom = GetComponent<Camera>().orthographicSize;

        // Zoom OUT the camera
        if (Input.GetKey(KeyCode.Z)) {
            if (zoom <= zoomMax) zoom += zoomSpeed;
        }

        // Zoom IN the camera
        if (Input.GetKey(KeyCode.X)) {
            if (zoom >= zoomMin) zoom -= zoomSpeed;
        }

        GetComponent<Camera>().orthographicSize = zoom;

        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
