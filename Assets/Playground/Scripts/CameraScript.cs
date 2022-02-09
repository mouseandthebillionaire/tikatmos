using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float xBounds, yBounds;
    public float speed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;
    public float minPlayerSize, maxPlayerSize;

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

        // Change the size of the player based on the camera's zoom
        float newSize = map(zoom, zoomMin, zoomMax, maxPlayerSize, minPlayerSize);
        player.transform.localScale = new Vector3(newSize, newSize, 1);

        // Change the position of the camera
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
    float oldRange = (oldMax - oldMin);
    float newRange = (newMax - newMin);
    float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
 
    return(newValue);
}
}
