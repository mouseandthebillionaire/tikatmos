using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficCamera : MonoBehaviour
{
    public float cameraSpeed;
    public float lerpSpeed;
    public float[] cameraBounds = new float[2];
    public float[] escalatorWidth = new float[2];

    public GameObject floor_view;
    
    public GameObject graph;

    public Image UIBackground;

    public Image selectedLevel;

    public Image[] levelFill;
    public Image[] levelFillBackground;
    public float levelStatusOffset;

    public float distBetweenLevels;

    public Color danger, medium, safe;

    private Vector3 systemStart, customerStart, backgroundStart, graphStart;

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
        customerStart =  levelFill[0].transform.position - transform.position;
        customerStart.z = 100;
        backgroundStart = UIBackground.transform.position - transform.position;
        backgroundStart.z = 100;
        graphStart = graph.transform.position - transform.position;
        graphStart.z = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.position.y;

        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) {
            if (yPos + cameraSpeed < cameraBounds[1]) yPos += cameraSpeed;
            else yPos = cameraBounds[1];
        } 
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) {
            if (yPos - cameraSpeed > cameraBounds[0]) yPos -= cameraSpeed;
            else yPos = cameraBounds[0];
        }

        // Change the position of the floor_view
        Vector3 targetPosition = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);


        // Reposition the level fill indicators
        for (int i = 0; i < levelFill.Length; i++) {
            yPos = transform.position.y + customerStart.y + (distBetweenLevels*i);
            levelFill[i].transform.position = new Vector3(transform.position.x + customerStart.x, yPos, 10);
            levelFill[i].transform.SetAsLastSibling();
            levelFillBackground[i].transform.position = new Vector3(levelFill[i].transform.position.x + levelStatusOffset, levelFill[i].transform.position.y, 0);
        }

        // Change the position of the selected level
        float levelY = this.transform.position.y + 4.75f;
        for (int i = 1; i < FloorManager.floorNumbers.Count; i++) {
            if (levelY >= FloorManager.floorNumbers[i].transform.position.y) {
                float bottomY = levelFill[i-1].transform.position.y;
                float topY = levelFill[i].transform.position.y;
                float currentX = selectedLevel.transform.position.x;
                selectedLevel.transform.position = new Vector3(currentX, bottomY + ((topY - bottomY)/2), 1);
                FloorManager.selectedFloor = i-1;
            }
        }

        // Change the position of the UI background
        UIBackground.transform.position = transform.position + backgroundStart;

        // Change the position of the graph
        graph.transform.position = transform.position + graphStart;

        UpdateUI();
    }

    void UpdateUI() {
        // Display the total number of people per floor
        int maxPeople = FloorManager.maxPeopleOnFloor;
        for (int i = 0; i < levelFill.Length; i++) {
            Color levelColor;
            int safeZone = (int) (FloorManager.maxPeopleOnFloor * mediumThreshold);
            int dangerZone = (int) (FloorManager.maxPeopleOnFloor * dangerThreshold);

            if (FloorManager.peopleOnFloors[i] <= dangerZone || FloorManager.peopleOnFloors[i] >= FloorManager.maxPeopleOnFloor - dangerZone) levelColor = danger;
            else if (FloorManager.peopleOnFloors[i] <= safeZone || FloorManager.peopleOnFloors[i] >= FloorManager.maxPeopleOnFloor - safeZone) levelColor = medium;
            else levelColor = safe;

            levelFill[i].transform.GetChild(0).GetComponentInChildren<Image>().fillAmount = (float) FloorManager.peopleOnFloors[i]/maxPeople;
            levelFill[i].transform.GetChild(2).GetComponentInChildren<Image>().color = levelColor;
        }

        // Change the size of the escalators based on their speed
        for (int i = 0; i < FloorManager.escalators.Count; i++) {
            float widthRange = escalatorWidth[1] - escalatorWidth[0];
            float yScale = FloorManager.escalators[i].gameObject.transform.localScale.y;
            float xScale = escalatorWidth[0] + (FloorManager.escalatorSpeed[i] * widthRange);
            FloorManager.escalators[i].gameObject.transform.localScale = new Vector3(xScale, yScale, 0);
        }
    }
}
