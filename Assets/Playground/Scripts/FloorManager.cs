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
    public float distBetweenFloors;

    public float distBetweenEscalators;

    public float personSpeed;

    public float[] personSpawnRate = new float[2];

    public int[] escalatorsPerFloor = new int[2];
    public int[] peoplePerFloor = new int[2];

    private GameObject[] floors = new GameObject[0];
    private GameObject[] escalators = new GameObject[0];
    private GameObject[] people = new GameObject[0];

    private bool[] escalatorsActive = new bool[0];

    private int[] peopleOnFloors;

    private float spawnWaitTime;
    private float time;

    private int currentPerson;

    private int[] startFloor = new int[0];
    private int[] escalatorsOnFloor = new int[0];

    // Start is called before the first frame update
    void Start()
    {
        // Spawn Floors
        Array.Resize(ref floors, numberOfFloors);
        Array.Resize(ref escalatorsOnFloor, numberOfFloors);

        for (int i = 0; i < numberOfFloors; i++) {
            float yLoc = distBetweenFloors * i;
            Vector3 loc = new Vector3(0, yLoc, 0);

            floors[i] = Instantiate(floor, loc, Quaternion.identity);
        }

        // Spawn escalators 
        int currentEscalator = 0;
        for (int i = 0; i < floors.Length - 1; i++) {
            int numberOfEscalators = UnityEngine.Random.Range(escalatorsPerFloor[0], escalatorsPerFloor[1] + 1);
            escalatorsOnFloor[i] = numberOfEscalators;

            // Generate the escalators for each floor
            for (int j = 0; j < numberOfEscalators; j++) {
                float xDistance = floors[i].GetComponent<SpriteRenderer>().bounds.extents.x;
                float xDiff = xDistance * 2 / (numberOfEscalators + 1);
                float xLoc = -xDistance + (xDiff * (j+1));
                float yLoc = (distBetweenFloors * i) + distBetweenFloors/2;
                Vector3 loc = new Vector3(xLoc, yLoc, 0);

                if (j == 0) {
                    int newSize = escalators.Length + numberOfEscalators;
                    Array.Resize(ref escalators, newSize);
                }

                escalators[currentEscalator] = Instantiate(escalator, loc, Quaternion.identity); 
                currentEscalator++;
            }
        }
        Array.Resize(ref escalatorsActive, escalators.Length);
        for (int i = 0; i < escalatorsActive.Length; i++) escalatorsActive[i] = true;

        // Determine how many people are on each floor
        Array.Resize(ref peopleOnFloors, numberOfFloors);
        for (int i = 0; i < numberOfFloors; i++) {
            int randomInt = UnityEngine.Random.Range(peoplePerFloor[0], peoplePerFloor[1] + 1);
            peopleOnFloors[i] = randomInt;
        }

        floor.SetActive(false);
        escalator.SetActive(false);
        //person.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (time == 0) spawnWaitTime = UnityEngine.Random.Range(personSpawnRate[0], personSpawnRate[1]);
        time += Time.deltaTime;
        if (time >= spawnWaitTime) {
            // Spawn a person moving to another floor
            int randomEscalator = UnityEngine.Random.Range(0, escalators.Length);

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
            
            Vector3 spawnLoc = new Vector3(xLoc, yLoc, 0);
            Array.Resize(ref people, people.Length + 1);
            people[currentPerson] = Instantiate(person, spawnLoc, Quaternion.identity);
            people[currentPerson].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed), ForceMode2D.Impulse);

            // Keep track of what floor the person started on
            Array.Resize(ref startFloor, people.Length);
            int start = 0;
            for(int i = 0; i < numberOfFloors; i++) if (floors[i].gameObject.transform.position.y == yLoc) start = i;
            startFloor[currentPerson] = start;

            currentPerson++;
            time = 0;
        }

        // If the person has moved beyond their start floor
        for (int i = 0; i < people.Length; i++) {
            if (people[i] != null) {
                // Check to see what floor the person is currently on
                float currentY = people[i].transform.position.y;
                int currentFloor = startFloor[i];

                float startY = floors[startFloor[i]].gameObject.transform.position.y;

                // If the person has travelled up one floor
                if (currentY >= startY + distBetweenFloors) currentFloor++;
                // If the person has travelled down one floor
                else if (currentY <= startY - distBetweenFloors) currentFloor--;

                // The person is out of bounds
                if (currentFloor < 0 || currentFloor > numberOfFloors) people[i].SetActive(false);
                else {
                    // Decide whether to remove the person or let them keep going
                    if (currentFloor != startFloor[i]) {
                        int choice = UnityEngine.Random.Range(0,2);

                        people[i].SetActive(false);

                        /*
                        if (choice == 0);
                        else {
                            int floorNumber = currentFloor;
                            if (currentFloor < startFloor[i] && currentFloor >= 1) floorNumber = currentFloor - 1;

                            // Determine the x location of the person based on the number of escalators
                            float xLoc = people[i].transform.position.x;
                            float xStart = -floors[0].GetComponent<SpriteRenderer>().bounds.extents.x;
                            float xJump = Mathf.Abs((xStart * 2) / (escalatorsOnFloor[floorNumber] + 1));

                            float minDistance = Mathf.Abs(xLoc - xStart);
                            int timesMultiplied = 0;

                            for (int j = 1; j <= escalatorsOnFloor[floorNumber]; j++) {
                                if (Mathf.Abs(xLoc - (xStart + (xJump * i))) <= minDistance) timesMultiplied = i;
                            }

                            Vector3 loc = new Vector3(xStart + (xJump * timesMultiplied), people[i].transform.position.y, 0);
                            people[i].transform.position = loc;
                        }
                        */
                    }
                }
            }
        }
    }
}
