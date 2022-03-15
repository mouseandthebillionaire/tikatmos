using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficCamera : MonoBehaviour
{
    public float cameraSpeed;
    public float lerpSpeed;
    public float[] cameraBounds = new float[2];

    public Image systemPower;

    public Image UIBackground;

    public Image[] levelFill;

    public float distBetweenLevels;

    public Color danger, medium, safe;

    private Vector3 systemStart, customerStart, backgroundStart;

    public float dangerThreshold, mediumThreshold;

    public static TrafficCamera S;

    void Awake()
    {
        // Create Singleton
        if (S == null) S = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Determine the starting positions of the UI elements
        systemStart = systemPower.transform.position - transform.position;
        systemStart.z = 100;
        customerStart =  levelFill[0].transform.position - transform.position;
        customerStart.z = 100;
        backgroundStart = UIBackground.transform.position - transform.position;
        backgroundStart.z = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.position.y;

        if (SerialScript.S.crankUp && yPos + cameraSpeed <= cameraBounds[1]) yPos += cameraSpeed;
        if (SerialScript.S.crankDown && yPos - cameraSpeed >= cameraBounds[0]) yPos -= cameraSpeed;

        // Change the position of the camera
        Vector3 targetPosition = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);

        // Reposition the system power indicator
        systemPower.transform.position = transform.position + systemStart;
        systemPower.transform.SetAsLastSibling();


        // Reposition the level fill indicators
        for (int i = 0; i < levelFill.Length; i++) {
            yPos = transform.position.y + customerStart.y + (distBetweenLevels*i);
            levelFill[i].transform.position = new Vector3(transform.position.x + customerStart.x, yPos, 10);
            levelFill[i].transform.SetAsLastSibling();
        }

        // Change the position of the UI background
        UIBackground.transform.position = transform.position + backgroundStart;

        UpdateUI();
    }

    void UpdateUI() {
        // Display the total number of people per floor
        int maxPeople = FloorManager.maxPeopleOnFloor;
        for (int i = 0; i < levelFill.Length; i++) {
            Color levelColor;
            if (FloorManager.peopleOnFloors[i] <= maxPeople * dangerThreshold) levelColor = danger;
            else if (FloorManager.peopleOnFloors[i] <= maxPeople * mediumThreshold) levelColor = medium;
            else levelColor = safe;

            levelFill[i].transform.GetChild(0).GetComponentInChildren<Image>().fillAmount = (float) FloorManager.peopleOnFloors[i]/maxPeople;
            levelFill[i].transform.GetChild(0).GetComponentInChildren<Image>().color = levelColor;
        }

        // Display the total number of active escalators
        systemPower.transform.GetChild(0).GetComponentInChildren<Image>().fillAmount = FloorManager.percentActivated;
        if (FloorManager.percentActivated >= 1 - dangerThreshold) systemPower.transform.GetChild(0).GetComponentInChildren<Image>().color = danger;
        else if (FloorManager.percentActivated >= mediumThreshold) systemPower.transform.GetChild(0).GetComponentInChildren<Image>().color = medium;
        else systemPower.transform.GetChild(0).GetComponentInChildren<Image>().color = safe;
    }
}
