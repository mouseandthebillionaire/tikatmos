using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FloorManager : MonoBehaviour
{
    public GameObject floor;
    public GameObject escalator;
    public GameObject person;

    public int numberOfFloors;

    public static float distBetweenFloors = 5;
    public static float distBetweenEscalators = 1;
    public static int maxPeopleOnFloor = 100;
    public static int floorThreshold = 30;

    public float personSpeed;

    public float[] personSpawnRate = new float[2];

    public int[] escalatorsPerFloor = new int[2];


    public static List<GameObject> floors = new List<GameObject>();
    public static List<GameObject> escalators = new List<GameObject>();
    private List<GameObject> people = new List<GameObject>();
    public static List<int> peopleOnFloors = new List<int>();

    public float maxPercentActivated;
    public static float percentActivated = 0.45f;

    private float spawnWaitTime;
    private float time;
    private int currentPerson;

    public Camera mainCamera;
    public Canvas canvas;
    public Image floorNumber;

    private List<Image> floorNumbers = new List<Image>();

    private int currentEscalator = 0;
    private int currentFloor = 0;
    private bool goingDown = true;
    private int[] escalatorsOnFloor = new int[0];
    public static bool[] upIsActive = new bool[0];
    public static bool[] downIsActive = new bool[0];

    public Color active, inactive, selected;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn Floors
        for (int i = 0; i < numberOfFloors; i++) {
            float yLoc = distBetweenFloors * i;
            Vector3 loc = new Vector3(0, yLoc, 0);

            floors.Add(Instantiate(floor, loc, Quaternion.identity));

            // Spawn the numbers for each floor
            float floorNumberY = floorNumber.transform.position.y + (distBetweenFloors * i);
            Vector3 floorNumberPosition = new Vector3(0, floorNumberY, 0);
            floorNumbers.Add(Instantiate(floorNumber, floorNumberPosition, Quaternion.identity));

            floorNumbers[i].transform.SetParent(canvas.transform);
            floorNumbers[i].GetComponentInChildren<Text>().text = "Level " + (i+1);

            // Determine how many people are on each floor
            peopleOnFloors.Add(UnityEngine.Random.Range(0, maxPeopleOnFloor));
        }

        // Spawn escalators 
        Array.Resize(ref escalatorsOnFloor, floors.Count - 1);
        for (int i = 0; i < floors.Count - 1; i++) {
            int numberOfEscalators = UnityEngine.Random.Range(escalatorsPerFloor[0], escalatorsPerFloor[1] + 1);

            // Generate the escalators for each floor
            for (int j = 0; j < numberOfEscalators; j++) {
                float xDistance = floors[i].GetComponent<SpriteRenderer>().bounds.extents.x;
                float xDiff = xDistance * 2 / (numberOfEscalators + 1);
                float xLoc = -xDistance + (xDiff * (j+1));
                float yLoc = (distBetweenFloors * i) + distBetweenFloors/2;
                Vector3 loc = new Vector3(xLoc, yLoc, 0);

                escalators.Add(Instantiate(escalator, loc, Quaternion.identity)); 
            }

            // Keep track of how many escalators are on each floor
            escalatorsOnFloor[i] = numberOfEscalators;
        }

        // Keep track of all deactive escalators
        Array.Resize(ref upIsActive, escalators.Count);
        Array.Resize(ref downIsActive, escalators.Count);
        for (int i = 0; i < escalators.Count; i++) {
            upIsActive[i] = false;
            downIsActive[i] = false;
        }

        // Randomly activate some of the escalators
        int maxActivated = upIsActive.Length + downIsActive.Length;
        for (int i = 0; i < percentActivated * maxActivated; i++) {
            int randomEscalator = UnityEngine.Random.Range(0, escalators.Count);

            // Randomly choose whether to activate the up or down escalator
            int choice = UnityEngine.Random.Range(0, 2);
            if (choice == 0) {
                if (!upIsActive[randomEscalator]) upIsActive[randomEscalator] = true;
                else downIsActive[randomEscalator] = true;
            } else {
                if (!downIsActive[randomEscalator]) downIsActive[randomEscalator] = true;
                else upIsActive[randomEscalator] = true;
            }
        }

        floor.SetActive(false);
        escalator.SetActive(false);
        floorNumber.gameObject.SetActive(false);
        //person.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        ManageEscalators();

        // Change the color of the floor depending on how many people it has
        for (int i = 0; i < peopleOnFloors.Count; i++) {
            floorNumbers[i].GetComponentInChildren<Text>().text = "Level " + (i+1);
            floorNumbers[i].fillAmount = (float) peopleOnFloors[i]/100;

            if (peopleOnFloors[i] >= maxPeopleOnFloor - floorThreshold) floorNumbers[i].color = Color.green;
            else if (peopleOnFloors[i] <= floorThreshold) floorNumbers[i].color = Color.red;
            else floorNumbers[i].color = Color.yellow;
        }

        // Increment the timer
        if (time == 0) spawnWaitTime = UnityEngine.Random.Range(personSpawnRate[0], personSpawnRate[1]);
        time += Time.deltaTime;

        // Spawn a person moving to another floor
        if (time >= spawnWaitTime) {
            int randomEscalator = UnityEngine.Random.Range(0, escalators.Count);
            float xLoc = escalators[randomEscalator].gameObject.transform.position.x;
            float yLoc = escalators[randomEscalator].gameObject.transform.position.y;
            float speed = personSpeed;

            // Determine whether the person is going up or down
            int choice = UnityEngine.Random.Range(0,2);

            // The person is going up
            if (choice == 0) {
                xLoc += distBetweenEscalators/2;
                yLoc -= distBetweenFloors/2;
            }
            // the person is going down
            else {
                xLoc -= distBetweenEscalators/2;
                yLoc += distBetweenFloors/2;
                speed = -speed;
            }
            
            // Spawn the person if the escalator is active
            if ((choice == 0 && upIsActive[randomEscalator]) || (choice != 0 && downIsActive[randomEscalator])) {
                Vector3 spawnLoc = new Vector3(xLoc, yLoc, 0);
                people.Add(Instantiate(person, spawnLoc, Quaternion.identity));
                people[currentPerson].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed), ForceMode2D.Impulse);            

                currentPerson++;
            }

            time = 0;
        }
    }

    void ManageEscalators() {

        // Display the total number of active escalators
        int upActive = 0, downActive = 0;
        for (int i = 0; i < upIsActive.Length; i++) {
            if (upIsActive[i]) upActive++;
            if (downIsActive[i]) downActive++;
        }
        float maxActivated = (int) ((upIsActive.Length + downIsActive.Length) * maxPercentActivated);
        float current = upActive + downActive;
        percentActivated = (float) current/maxActivated;

        // Activate/Deactivate an (escalator)
        if (SerialScript.S.deviceButton) {

            if (!goingDown) upIsActive[currentEscalator] = !upIsActive[currentEscalator];
            else downIsActive[currentEscalator] = !downIsActive[currentEscalator];

            // Can't activate any more escalators
            if (percentActivated == 1) {
                if (!goingDown && upIsActive[currentEscalator]) upIsActive[currentEscalator] = false;
                if (goingDown && downIsActive[currentEscalator]) downIsActive[currentEscalator] = false;
            }
        }

        // Deselect all escalators
        for (int i = 0; i < escalators.Count; i++) {
            escalators[i].GetComponentsInChildren<SpriteRenderer>()[0].color = active;
            escalators[i].GetComponentsInChildren<SpriteRenderer>()[1].color = active;
        }

        // Check to see which escalators are turned off
        for (int i = 0; i < escalators.Count; i++) {
            // Set an escalator to deactivate
            if (!upIsActive[i]) escalators[i].GetComponentsInChildren<SpriteRenderer>()[1].color = inactive;
            if (!downIsActive[i]) escalators[i].GetComponentsInChildren<SpriteRenderer>()[0].color = inactive;
        }
        
        // Check to see what floor the currently selected escalator is on
        for (int i = 0; i < escalatorsOnFloor.Length; i++) {
            int escalatorsBelow = 0;
            for (int j = 0; j < i; j++) escalatorsBelow += escalatorsOnFloor[j];
            if (currentEscalator >= escalatorsBelow) currentFloor = i;
        }

        // Check to see what the range of escalators that can be selected is
        int lowerLimit = 0, upperLimit = 0;
        for (int i = 0; i < currentFloor; i++) lowerLimit += escalatorsOnFloor[i];
        for (int i = 0; i <= currentFloor; i++) upperLimit += escalatorsOnFloor[i];


        // Check to see if the selected escalator was changed
        if (SerialScript.S.knobLeft) {
            if (!goingDown) goingDown = true;
            else {
                if (currentEscalator > lowerLimit) {
                    currentEscalator--;
                    goingDown = false;
                }
            }
        }
        if (SerialScript.S.knobRight) {
            if (goingDown) goingDown = false;
            else {
                if (currentEscalator < upperLimit - 1) {
                    currentEscalator++;
                    goingDown = true;
                }
            }
        }

        // Check to see if the selected floor was changed
        int oldFloor = currentFloor;
        if (SerialScript.S.knobDown) if (currentFloor > 0) currentFloor--;
        if (SerialScript.S.knobUp) if (currentFloor < escalatorsOnFloor.Length - 1) currentFloor++;

        if (oldFloor != currentFloor) {
            currentEscalator = 0;
            goingDown = true;
            for (int i = 0; i < currentFloor; i++) {
                currentEscalator += escalatorsOnFloor[i];
            }
        }

        // Highlight the currently selected escalator
        if (goingDown) escalators[currentEscalator].GetComponentsInChildren<SpriteRenderer>()[0].color = selected;
        else escalators[currentEscalator].GetComponentsInChildren<SpriteRenderer>()[1].color = selected;
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
    
        return(newValue);
    }
}
