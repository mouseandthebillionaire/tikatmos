using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float xBounds, yBounds;
    public float speed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;

    public GameObject player;

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
            if (yPos <= yBounds && !CheckOverlap(Vector2.up)) yPos += speed;
        }
        // Move the camera DOWN
        if (Input.GetKey(KeyCode.DownArrow)) {
            if (yPos >= -yBounds && !CheckOverlap(Vector2.down)) yPos -= speed;
        }

        // Move the camera LEFT
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (xPos >= -xBounds && !CheckOverlap(Vector2.left)) xPos -= speed;
        }
        // Move the camera RIGHT
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (xPos <= xBounds && !CheckOverlap(Vector2.right)) xPos += speed;
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

        // Change the position of the player and the camera
        player.transform.position = new Vector3 (xPos, yPos, 0);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    bool CheckOverlap(Vector2 direction) {
        float rayDistance = player.GetComponent<CircleCollider2D>().bounds.extents.x;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.TransformDirection(direction), rayDistance);

        for (int i = 0; i < hit.Length; i++) {
            if (hit[i].collider.gameObject.layer != 6) return true;
        }
        return false;
    }
}
