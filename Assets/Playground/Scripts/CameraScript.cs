using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public GameObject magnifyingGlass, recording;
    public GameObject[] batteryBars = new GameObject[5];
    public GameObject catchTime;

    public Image cameraPosition;

    public Image cameraZoom;

    public float xBounds, yBounds;
    public float speed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;
    public float minGlassSize, maxGlassSize;

    private float recordingTimer = 0f;
    public float blinkRate;

    private float batteryTimer = 0f;
    private int currentBar;
    public float batteryLife;

    public float catchTimer;
    public float timerResetRate;

    public float xMin, xMax;
    public float yMin, yMax;

    public float zoomedOut, zoomedIn;

    public Color hiPower, midPower, lowPower;

    public static bool catchingPerson = false;

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
        if (Input.GetKey(GlobalVariables.S.upCrank)) {
            if (yPos <= yBounds) yPos += speed;
        }
        // Move the camera DOWN
        if (Input.GetKey(GlobalVariables.S.downCrank)) {
            if (yPos >= -yBounds) yPos -= speed;
        }

        // Move the camera LEFT
        if (Input.GetKey(GlobalVariables.S.knobLeft)) {
            if (xPos >= -xBounds) xPos -= speed;
        }
        // Move the camera RIGHT
        if (Input.GetKey(GlobalVariables.S.knobRight)) {
            if (xPos <= xBounds) xPos += speed;
        }

        float zoom = GetComponent<Camera>().orthographicSize;

        // Zoom OUT the camera
        if (Input.GetKey(GlobalVariables.S.leftSlider)) {
            if (zoom <= zoomMax) zoom += zoomSpeed;
        }

        // Zoom IN the camera
        if (Input.GetKey(GlobalVariables.S.rightSlider)) {
            if (zoom >= zoomMin) zoom -= zoomSpeed;
        }

        GetComponent<Camera>().orthographicSize = zoom;

        // Change the size of the player based on the camera's zoom
        float newSize = map(zoom, zoomMin, zoomMax, maxGlassSize, minGlassSize);
        magnifyingGlass.transform.localScale = new Vector3(newSize, newSize, 1);

        // Change the zoom indicator's position
        float zoomLevel = map(zoom, zoomMin, zoomMax, zoomedOut, zoomedIn);
        cameraZoom.rectTransform.anchoredPosition = new Vector3(cameraZoom.rectTransform.anchoredPosition.x, zoomLevel, 1);

        // Change the position of the camera
        transform.position = new Vector3(xPos, yPos, transform.position.z);

        // Move the position of the camera indicator to reflect the camera position
        float xLoc = map(xPos, -xBounds, xBounds, xMin, xMax);
        float yLoc = map(yPos, -yBounds, yBounds, yMin, yMax);
        
        cameraPosition.rectTransform.anchoredPosition = new Vector3(xLoc, yLoc, 1);

        CameraUI();
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
    
        return(newValue);
    }

    void CameraUI() {
        // Display the timer for how long you need to have the magnifying glass over a person
        float time = catchTimer*100;
        catchTime.GetComponent<Text>().text = time.ToString("#00:00:00");
        CheckOverlap();

        // When the timer reaches 0
        if (catchTimer <= 0) catchTimer = 0;


        // Blink the recording light
        recordingTimer += Time.fixedDeltaTime;

        if (recordingTimer >= blinkRate) {
            recordingTimer = 0;

            // Turn the recording indicator on or off
            recording.GetComponent<Image>().enabled = !recording.GetComponent<Image>().enabled;
        }


        // Reset the battery
        if (batteryTimer == 0) for (int i = 0; i < batteryBars.Length; i++) {
            batteryBars[i].GetComponent<Image>().enabled = true;
            batteryBars[i].GetComponent<Image>().color = hiPower;
            currentBar = batteryBars.Length;
        }

        // Decrease the battery life
        batteryTimer += Time.fixedDeltaTime;
        float barLife = batteryLife/batteryBars.Length;

        for (int i = 1; i <= batteryBars.Length; i++) {
            if (batteryTimer >= barLife*i) {
                Image bar = batteryBars[batteryBars.Length - i].GetComponent<Image>();

                if (bar.enabled) currentBar--;
                bar.enabled = false;
            }

            // Conditions for changing the battery color
            if (currentBar <= 3) {
                for (int j = 0; j <= 3; j++) batteryBars[j].GetComponent<Image>().color = midPower;
            }
            if (currentBar <= 1) {
                for (int j = 0; j <= 1; j++) batteryBars[j].GetComponent<Image>().color = lowPower;
            }
        }
    }

    // Check to see if the magnifying glass is on a person
    void CheckOverlap()
    {
        Vector2 position = new Vector2(transform.position.x,transform.position.y);

        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(position, magnifyingGlass.GetComponent<CircleCollider2D>().bounds.extents.x);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject.layer == 6 && colliders[i].gameObject.tag == "Goal") catchTimer -= Time.deltaTime;
        }
    }
}
