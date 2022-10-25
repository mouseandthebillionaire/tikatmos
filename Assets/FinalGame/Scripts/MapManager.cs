using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public float zoomMax, zoomMin;
    public float zoomSpeed;
    private float zoom = 1.5f;
    
    public float xBounds, yBounds;
    [SerializeField] [Range(0f, 5f)] float lerpSpeed;
    public float cameraSpeed;
    
    public GameObject[] floorPlans;
    public GameObject[] floors;
    public Color[]      floorColors;

    public  GameObject map;
    public  GameObject floor;
    private int        floorCounter = 0;

    private TextAsset storeCodes;

    void Start()
    {
        floorPlans[0].SetActive(true);
    }
    
    void Update()
    {
        float xPos = map.transform.position.x;
        float yPos = map.transform.position.y;

        // Move the camera UP
        if (Input.GetKeyDown(GlobalVariables.S.knob0_left)) {
            if (yPos <= yBounds) yPos += cameraSpeed;
        }
        // Move the camera DOWN
        if (Input.GetKeyDown(GlobalVariables.S.knob0_right)) {
            if (yPos >= -yBounds) yPos -= cameraSpeed;
        }

        // Move the camera LEFT
        if (Input.GetKeyDown(GlobalVariables.S.knob1_left)) {
            if (xPos >= -xBounds) xPos -= cameraSpeed;
        }
        // Move the camera RIGHT
        if (Input.GetKeyDown(GlobalVariables.S.knob1_right)) {
            if (xPos <= xBounds) xPos += cameraSpeed;
        }
        
        // Zoom OUT the camera
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) {
            if (zoom <= zoomMax) zoom += zoomSpeed;
        }

        // Zoom IN the camera
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) {
            if (zoom >= zoomMin) zoom -= zoomSpeed;
        }

        map.transform.localScale = new Vector3(zoom, zoom, map.transform.localScale.z);
        
        //Change floor plan displayed
        //Highlight ISO floor
        if (Input.GetKeyDown(GlobalVariables.S.deviceButton))
        {
            //Hide current displayed map
            floorPlans[floorCounter].SetActive(false);
            
            floorCounter++; //increase count
            //If our new number is greater than the number of floors, loop around
            if (floorCounter > floorPlans.Length-1) floorCounter = 0;
            
            //Show new floor plan 
            floorPlans[floorCounter].SetActive(true);
            
            //NEED TO IMPLEMENT
            for (int i = 0; i < floors.Length; i++) {
                if (i != floorCounter) {
                    floors[i].transform.localPosition = new Vector2(
                        -55,
                        floors[i].transform.localPosition.y);
                }

                if (i == floorCounter) {
                    floors[i].transform.localPosition = new Vector2(
                        -50,
                        floors[i].transform.localPosition.y);
                }            
            }
        }
        
        // Change the position of the camera
        Vector3 targetPosition = new Vector3(xPos, yPos, map.transform.position.z);
        map.transform.position = Vector3.Lerp(map.transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
