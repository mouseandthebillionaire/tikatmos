using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaldoCamera : MonoBehaviour
{
    public GameObject magnifyingGlass, recording, battery;
    public GameObject[] batteryBars = new GameObject[5];
    public GameObject catchTime;

    public Image cameraOff;

    public Image cameraPosition;

    public Image cameraZoom;

    public float xBounds, yBounds;
    [SerializeField] [Range(0f, 5f)] float lerpSpeed;
    public float cameraSpeed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;
    public float minGlassSize, maxGlassSize;

    private float recordingTimer = 0f;
    public float blinkRate;
    public float timer;
    private float startTime = 0f;
    private int currentBar;

    public float xMin, xMax;
    public float yMin, yMax;

    public float zoomedOut, zoomedIn;

    public Color hiPower, midPower, lowPower;

    public static bool catchingPerson = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        // Reset the battery
        for (int i = 0; i < batteryBars.Length; i++) {
            batteryBars[i].GetComponent<Image>().enabled = true;
            batteryBars[i].GetComponent<Image>().color = hiPower;
            currentBar = batteryBars.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Move the camera UP
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) {
            if (yPos <= yBounds) yPos += cameraSpeed;
        }
        // Move the camera DOWN
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) {
            if (yPos >= -yBounds) yPos -= cameraSpeed;
        }

        // Move the camera LEFT
        if (Input.GetKeyDown(GlobalVariables.S.knobLeft)) {
            if (xPos >= -xBounds) xPos -= cameraSpeed;
        }
        // Move the camera RIGHT
        if (Input.GetKeyDown(GlobalVariables.S.knobRight)) {
            if (xPos <= xBounds) xPos += cameraSpeed;
        }

        float zoom = GetComponent<Camera>().orthographicSize;

        // Zoom OUT the camera
        if (Input.GetKeyDown(GlobalVariables.S.leftSlider)) {
            if (zoom <= zoomMax) zoom += zoomSpeed;
        }

        // Zoom IN the camera
        if (Input.GetKeyDown(GlobalVariables.S.rightSlider)) {
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
        Vector3 targetPosition = new Vector3(xPos, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

        // Move the position of the camera indicator to reflect the camera position
        float xLoc = map(xPos, -xBounds, xBounds, xMin, xMax);
        float yLoc = map(yPos, -yBounds, yBounds, yMin, yMax);
        Vector3 oldPosition = cameraPosition.rectTransform.anchoredPosition;
        Vector3 newPosition = new Vector3(xLoc, yLoc, 1);
        cameraPosition.rectTransform.anchoredPosition = Vector3.Lerp(oldPosition, newPosition, lerpSpeed * Time.deltaTime);

        CameraUI();
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
    
        return(newValue);
    }

    void CameraUI() {
        // Display the timer
        float currentTime = timer - (Time.time - startTime);
        if (currentTime > 0) {
            int minutes = (int)(currentTime / 60);
            int seconds  = (int)(currentTime % 60);
            int fraction = (int)((currentTime * 100) % 100);

            catchTime.GetComponent<Text>().text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        } 
        CheckOverlap();


        // Blink the recording light
        recordingTimer += Time.fixedDeltaTime;

        if (recordingTimer >= blinkRate) {
            recordingTimer = 0;

            // Turn the recording indicator on or off
            recording.GetComponent<Image>().enabled = !recording.GetComponent<Image>().enabled;

            // If low battery, start blinking the battery
            if (currentTime <= 10f) {
                batteryBars[0].GetComponent<Image>().enabled = !batteryBars[0].GetComponent<Image>().enabled;
                battery.GetComponent<Image>().enabled = !battery.GetComponent<Image>().enabled;
            }
        }

        // Decrease the battery life
        float barLife = timer/batteryBars.Length;

        for (int i = batteryBars.Length - 1; i >= 0; i--) {
            if (currentTime <= barLife*i) {
                Image bar = batteryBars[i].GetComponent<Image>();

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

        // The battery has run out of power
        if (currentTime < 0) {
            cameraOff.gameObject.SetActive(true);
        }
    }

    // Check to see if the magnifying glass is on a person
    void CheckOverlap()
    {
        Vector2 position = new Vector2(transform.position.x,transform.position.y);

        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(position, magnifyingGlass.GetComponent<CircleCollider2D>().bounds.extents.x);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject.layer == 6 && colliders[i].gameObject.tag == "Goal") {
                if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) {
                    // Goal was completed

                }
            }
        }
    }
}
