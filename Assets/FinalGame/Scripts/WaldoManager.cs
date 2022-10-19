using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaldoManager : MonoBehaviour
{
    public GameObject magnifyingGlass, temperature;
    public GameObject[] batteryBars = new GameObject[5];

    public Canvas canvas;

    public Image cameraPosition;

    public Image cameraZoom;

    public Text goalCounter;

    public float[] speed = new float[2];

    public float xBounds, yBounds;
    private float cameraPos_x, cameraPos_y;
    public float lerpSpeed;

    public float zoomMax, zoomMin;
    public float zoomSpeed;
    public float minGlassSize, maxGlassSize;

    private Camera waldoCamera;

    private float recordingTimer = 0f;
    public float blinkRate;
    private float startTime = 0f;
    private int currentBar;

    public float gridBoundsX, gridBoundsY;
    public float zoomedOut, zoomedIn;

    public Color hiPower, midPower, lowPower;

    public static float timer;

    void Awake() {
        magnifyingGlass.gameObject.SetActive(true);
        magnifyingGlass.transform.localScale = new Vector3(.2f, .2f, 0);
    }
    
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

        waldoCamera = GameObject.Find("Camera_Device").GetComponent<Camera>();
        waldoCamera.orthographicSize = zoomMin + (zoomMax - zoomMin)/2;
        cameraPos_x = waldoCamera.transform.position.x;
        cameraPos_y = waldoCamera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the score counter
        goalCounter.text = "" + WaldoTaskManager.goalsCompleted;
        if (WaldoTaskManager.goalsCompleted > 0) goalCounter.color = Color.white;

        float zoom = waldoCamera.orthographicSize;
        // make sure the zoom is within range
        if (zoom > 3) zoom = 3;

        // Zoom OUT the camera
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) {
            if (zoom + zoomSpeed < zoomMax) zoom += zoomSpeed;
            else zoom = zoomMax;
        }

        // Zoom IN the camera
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) {
            if (zoom - zoomSpeed > zoomMin) zoom -= zoomSpeed;
            else zoom = zoomMin;
        }

        waldoCamera.orthographicSize = zoom;

        // Change the size of the player based on the camera's zoom
        float newSize = map(zoom, zoomMin, zoomMax, maxGlassSize, minGlassSize);
        magnifyingGlass.transform.localScale = new Vector3(newSize, newSize, 1);

        // Change the zoom indicator's position
        float zoomLevel = map(zoom, zoomMin, zoomMax, zoomedOut, zoomedIn);
        cameraZoom.rectTransform.anchoredPosition = new Vector3(cameraZoom.rectTransform.anchoredPosition.x, zoomLevel, 1);
        


        // Change the camera speed based on the zoom level
        float cameraSpeed = map(zoom, zoomMin, zoomMax, speed[0], speed[1]);

        // TODO - change GetKey to GetKeyDown in build
        // Move the camera UP
        if (Input.GetKey(GlobalVariables.S.knob0_left)) {
            if (cameraPos_y + cameraSpeed <= yBounds) cameraPos_y += cameraSpeed;
            else cameraPos_y = yBounds;
        }
        // Move the camera DOWN
        if (Input.GetKey(GlobalVariables.S.knob0_right)) {
            if (cameraPos_y - cameraSpeed > -yBounds) cameraPos_y -= cameraSpeed;
            else cameraPos_y = -yBounds;
        }

        // Move the camera LEFT
        if (Input.GetKey(GlobalVariables.S.knob1_left)) {
            if (cameraPos_x - cameraSpeed > -xBounds) cameraPos_x -= cameraSpeed;
            else cameraPos_x = -xBounds;
        }
        // Move the camera RIGHT
        if (Input.GetKey(GlobalVariables.S.knob1_right)) {
            if (cameraPos_x + cameraSpeed < xBounds) cameraPos_x += cameraSpeed;
            else cameraPos_x = xBounds;
        }    

        // Change the position of the camera
        Vector3 targetPosition = new Vector3(cameraPos_x, cameraPos_y, -10);
        waldoCamera.transform.position = Vector3.Lerp(waldoCamera.transform.position, targetPosition, lerpSpeed);

        // Move the position of the camera indicator to reflect the camera position
        float xLoc = map(cameraPos_x, -xBounds, xBounds, -gridBoundsX, gridBoundsX);
        float yLoc = map(cameraPos_y, -yBounds, yBounds, -gridBoundsY, gridBoundsY);
        Vector3 oldPosition = cameraPosition.rectTransform.anchoredPosition;
        Vector3 newPosition = new Vector3(xLoc, yLoc, 1);
        cameraPosition.rectTransform.anchoredPosition = Vector3.Lerp(oldPosition, newPosition, lerpSpeed);

        CheckOverlap();
        CameraUI();
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;

        return(newValue);
    }

    void CameraUI() {

        // Reposition the canvas
        canvas.GetComponent<RectTransform>().position = waldoCamera.transform.position;

        // Display the timer
        float currentTime = timer - (Time.time - startTime);
        if (currentTime > 0) {
            int minutes = (int)(currentTime / 60);
            int seconds  = (int)(currentTime % 60);
            int fraction = (int)((currentTime * 100) % 100);

            //catchTime.GetComponent<Text>().text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
        } 

        // Blink the recording light
        recordingTimer += Time.fixedDeltaTime;

        if (recordingTimer >= blinkRate) {
            recordingTimer = 0;

            // Turn the recording indicator on or off
            //recording.GetComponent<Image>().enabled = !recording.GetComponent<Image>().enabled;

            // If low battery, start blinking the battery
            if (currentTime <= (timer/batteryBars.Length)/2) {
                //batteryBars[0].GetComponent<Image>().enabled = !batteryBars[0].GetComponent<Image>().enabled;
                //battery.GetComponent<Image>().enabled = !battery.GetComponent<Image>().enabled;
            }
        }

        // Change the temperature level
        float xDistance = waldoCamera.transform.position.x - WaldoTaskManager.goalX;
        float yDistance = waldoCamera.transform.position.y - WaldoTaskManager.goalY;
        float distance = Mathf.Sqrt((xDistance*xDistance) + (yDistance*yDistance));
        float maxDistance = Mathf.Sqrt(((xBounds*2)*(xBounds*2)) + ((yBounds*2)*(yBounds*2)));
        temperature.GetComponent<Image>().fillAmount = map(distance, 0, maxDistance, 1, 0);

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
            //cameraOff.gameObject.SetActive(true);
            WaldoTaskManager.goalsCompleted = 0;
            goalCounter.color = lowPower;
        }
    }

    // Check to see if the magnifying glass is on a person
    void CheckOverlap()
    {
        Vector2 position = new Vector2(waldoCamera.transform.position.x, waldoCamera.transform.position.y);

        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(position, magnifyingGlass.GetComponent<CircleCollider2D>().bounds.extents.x);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject.layer == 6 && colliders[i].gameObject.tag == "Goal") {
                if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) {
                    // Goal was completed
                    colliders[i].gameObject.GetComponentInParent<WaldoTargets>().StartCoroutine("FoundReaction");

                    // Play a sound effect
                    AudioSource audio = GetComponentInChildren<AudioSource>();
                    audio.Play();

                    WaldoTaskManager.goalCompleted = true;

                    // Reset the timer
                    startTime = Time.time;

                    // Reset the battery
                    for (int j = 0; j < batteryBars.Length; j++) {
                        batteryBars[j].GetComponent<Image>().enabled = true;
                        batteryBars[j].GetComponent<Image>().color = hiPower;
                        currentBar = batteryBars.Length;
                    }
                }
            }
        }
    }
}
