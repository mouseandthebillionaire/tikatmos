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
    public static int maxPeopleOnFloor = 100;

    public static float personSpeed = 1;

    public float[] personSpawnRate = new float[2];

    public int[] escalatorsPerFloor = new int[2];


    public static List<GameObject> floors = new List<GameObject>();
    public static List<GameObject> escalators = new List<GameObject>();
    public static List<GameObject> people = new List<GameObject>();
    public static List<int> peopleOnFloors = new List<int>();

    private float spawnWaitTime;
    public int despawnChance;

    private float time;
    private int currentPerson;

    public Canvas canvas;
    public Image floorNumber;

    public static List<Image> floorNumbers = new List<Image>();

    private int currentEscalator, currentFloor;
    private int[] escalatorsOnFloor = new int[0];

    public static int selectedFloor;

    public static bool[] escalatorDirectionDown = new bool[0];

    public Color active, inactive, selected;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn Floors
        for (int i = 0; i < numberOfFloors; i++) {
            float yLoc = distBetweenFloors * i;
            Vector3 loc = new Vector3(floor.transform.position.x, yLoc, 0);

            floors.Add(Instantiate(floor, loc, Quaternion.identity));

            // Spawn the numbers for each floor
            float floorNumberY = floorNumber.transform.position.y + (distBetweenFloors * i);
            Vector3 floorNumberPosition = new Vector3(floorNumber.transform.position.x, floorNumberY, 0);
            floorNumbers.Add(Instantiate(floorNumber, floorNumberPosition, Quaternion.identity));

            floorNumbers[i].transform.SetParent(canvas.transform);
            floorNumbers[i].GetComponentInChildren<Text>().text = "Level " + (i+1);

            // Determine how many people are on each floor
            peopleOnFloors.Add(UnityEngine.Random.Range(0, maxPeopleOnFloor));
            if (i != 0) { 
                if (peopleOnFloors[i - 1] < maxPeopleOnFloor/2) peopleOnFloors[i] = UnityEngine.Random.Range(maxPeopleOnFloor/2, maxPeopleOnFloor);
                else peopleOnFloors[i] = UnityEngine.Random.Range(maxPeopleOnFloor/6, maxPeopleOnFloor/2);
            }

            // Spawn the people
            for (int j = 0; j < peopleOnFloors[i]; j++) {
                Vector3 spawnLoc = new Vector3(floors[i].transform.position.x, floors[i].transform.position.y, 0);
                people.Add(Instantiate(person, spawnLoc, Quaternion.identity));
            }
        }

        // Spawn escalators 
        Array.Resize(ref escalatorsOnFloor, floors.Count - 1);
        for (int i = 0; i < floors.Count - 1; i++) {
            int numberOfEscalators = UnityEngine.Random.Range(escalatorsPerFloor[0], escalatorsPerFloor[1] + 1);

            // Generate the escalators for each floor
            for (int j = 0; j < numberOfEscalators; j++) {
                float xDistance = floors[i].GetComponent<SpriteRenderer>().bounds.extents.x;
                float xDiff = xDistance * 2 / (numberOfEscalators + 1);
                float xLoc = -xDistance + (xDiff * (j+1)) + floor.transform.position.x;
                float yLoc = (distBetweenFloors * i) + distBetweenFloors/2;
                Vector3 loc = new Vector3(xLoc, yLoc, 0);

                escalators.Add(Instantiate(escalator, loc, Quaternion.identity)); 
            }

            // Keep track of how many escalators are on each floor
            escalatorsOnFloor[i] = numberOfEscalators;
        }

        Array.Resize(ref escalatorDirectionDown, escalators.Count);

        // Randomly choose whether the escalators are facing up or down
        for (int i = 0; i < escalators.Count; i++) {
            int binaryChoice = UnityEngine.Random.Range(0, 2);
            if (binaryChoice == 0) {
                escalators[i].transform.rotation = Quaternion.Euler(0, 0, 180);
                escalatorDirectionDown[i] = true;
            } else {
                escalators[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                escalatorDirectionDown[i] = false;
            }
        }

        floor.SetActive(false);
        escalator.SetActive(false);
        floorNumber.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ManageEscalators();

        // Change the color of the floor depending on how many people it has
        for (int i = 0; i < peopleOnFloors.Count; i++)
        {
            floorNumbers[i].GetComponentInChildren<Text>().text = "Level " + i;
            floorNumbers[i].fillAmount = (float)peopleOnFloors[i] / 100;

            if (peopleOnFloors[i] <= maxPeopleOnFloor * TrafficCamera.S.dangerThreshold) floorNumbers[i].color = TrafficCamera.S.danger;
            else if (peopleOnFloors[i] <= maxPeopleOnFloor * TrafficCamera.S.mediumThreshold) floorNumbers[i].color = TrafficCamera.S.medium;
            else floorNumbers[i].color = TrafficCamera.S.safe;
        }

        // Set Level 0 to "Garage"
        floorNumbers[0].GetComponentInChildren<Text>().text = "Garage";
        //floorNumbers[0].fillAmount = 100;
        //floorNumbers[0].color = Color.gray;

        // Increment the timer
        if (time == 0) spawnWaitTime = UnityEngine.Random.Range(personSpawnRate[0], personSpawnRate[1]);
        time += Time.deltaTime;

        //SpawnPerson();
    }

    void ManageEscalators() {

        // Check to see if the selected floor was changed
        if (currentFloor != selectedFloor) {
            currentEscalator = 0;
            for (int i = 0; i < selectedFloor; i++) {
                currentEscalator += escalatorsOnFloor[i];
            }

            currentFloor = selectedFloor;
        }

        // Check to see what the range of escalators that can be selected is
        int lowerLimit = 0, upperLimit = 0;
        for (int i = 0; i < selectedFloor; i++) lowerLimit += escalatorsOnFloor[i];
        for (int i = 0; i <= selectedFloor; i++) upperLimit += escalatorsOnFloor[i];


        // Check to see if the selected escalator was changed
        if (SerialScript.S.knobLeft) {
            if (currentEscalator > lowerLimit) {
                currentEscalator--;
            }
        }
        if (SerialScript.S.knobRight) {
            if (currentEscalator < upperLimit - 1) {
                currentEscalator++;
            }
        }

        // Deselect all escalators
        for (int i = 0; i < escalators.Count; i++) escalators[i].GetComponentInChildren<SpriteRenderer>().color = inactive;

        // Highlight the currently selected escalator
        escalators[currentEscalator].GetComponentInChildren<SpriteRenderer>().color = selected;


        // Change the direction of the selected escalator
        if (SerialScript.S.deviceButton) {
            if (escalators[currentEscalator].transform.rotation.z == 0) {
                escalators[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 180);
                escalatorDirectionDown[currentEscalator] = true;
            } else {
                escalators[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 0);
                escalatorDirectionDown[currentEscalator] = false;
            }
        }
    }


    void SpawnPerson() {
        // Randomly spawn a person on the garage level
        if (time >= spawnWaitTime) {
            if (peopleOnFloors[0] < maxPeopleOnFloor) {
                // Spawn a person
                Vector3 spawnLoc = new Vector3(floors[0].transform.position.x, floors[0].transform.position.y, 0);
                people.Add(Instantiate(person, spawnLoc, Quaternion.identity));
                peopleOnFloors[0]++;
            }

            // Occasionally despawn a person if they are on the Garage level
            bool despawnedPerson = false;
            for (int i = 0; i < people.Count; i++)
            if (people[i].transform.position.y == FloorManager.floors[0].transform.position.y && !despawnedPerson) {
                int randomChance = UnityEngine.Random.Range(0, despawnChance);
                if (randomChance == 0 && peopleOnFloors[0] > 0) {
                    // Despawn a person
                    people[i].gameObject.SetActive(false);
                    peopleOnFloors[0]--;
                    despawnedPerson = true;
                }
            }

            time = 0;
        }
        

        // Spawn a person moving to another floor
        // if (time >= spawnWaitTime)
        // {
        //     int randomEscalator = UnityEngine.Random.Range(0, escalators.Count);
        //     float xLoc = escalators[randomEscalator].gameObject.transform.position.x;
        //     float yLoc = escalators[randomEscalator].gameObject.transform.position.y;
        //     float speed = personSpeed;

        //     // The person is going up
        //     if (!escalatorDirectionDown[randomEscalator])
        //     {
        //         yLoc -= distBetweenFloors / 2;
        //     }
        //     // The person is going down
        //     else
        //     {
        //         yLoc += distBetweenFloors / 2;
        //         speed = -speed;
        //     }

        //     int spawnFloor = 0;
        //     // Determine what floor the person will spawn on
        //     for (int i = 0; i < floors.Count; i++)
        //     {
        //         if (floors[i].transform.position.y == yLoc) spawnFloor = i;
        //     }

        //     // Check to see if the floor can hold more people
        //     bool floorFull = false;
        //     bool floorEmpty = false;
        //     if (peopleOnFloors[spawnFloor] >= maxPeopleOnFloor - 1) floorFull = true;
        //     if (peopleOnFloors[spawnFloor] <= 0) floorEmpty = true;

        //     if (!floorFull && !floorEmpty)
        //     {
        //         Vector3 spawnLoc = new Vector3(xLoc, yLoc, 0);
        //         people.Add(Instantiate(person, spawnLoc, Quaternion.identity));
        //         people[currentPerson].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed), ForceMode2D.Impulse);

        //         currentPerson++;
        //     }

        //     time = 0;
        // }
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
    
        return(newValue);
    }
}
