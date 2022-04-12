using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficPerson : MonoBehaviour
{
    public float yOffset;

    public float xBounds;

    public float[] initialWait = new float[2];

    public float[] timeOnFloor = new float[2];
    public Color[] colors;

    private Vector3 startPosition;

    private int startFloor;
    private int currentFloor;

    private bool goingDown = false;

    private bool leftStartFloor = false;

    private bool changedFloors = false;

    private bool firstSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        
        // Determine what floor the person started on
        for (int i = 0; i < FloorManager.floors.Count; i++) {
            if (startPosition.y == FloorManager.floors[i].transform.position.y) startFloor = i;
        }

        currentFloor = startFloor;

        // Randomly choose a color for the person
        int randomIndex = UnityEngine.Random.Range(0, colors.Length);
        this.GetComponent<SpriteRenderer>().color = colors[randomIndex];


        // Wait before the person leaves the floor
        float waitTime = UnityEngine.Random.Range(initialWait[0], initialWait[1]);
        StartCoroutine(ChangeFloors(waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFloor == startFloor) {
            // Check if the person has travelled up or down a floor
            if (transform.position.y >= startPosition.y + FloorManager.distBetweenFloors) {
                currentFloor++;  
                goingDown = false; 
            }
            if (transform.position.y <= startPosition.y - FloorManager.distBetweenFloors) {
                currentFloor--; 
                goingDown = true;
            }

            changedFloors = false;
        }  else {
            // Move the person to another floor
            if (!changedFloors) {
                // Determine how long the person stays on the floor
                float waitTime = UnityEngine.Random.Range(timeOnFloor[0], timeOnFloor[1]);
                StartCoroutine(ChangeFloors(waitTime));
            }
        }
        
        

        // Check if the person has left their starting floor
        if (!leftStartFloor) {
            if (transform.position.y < startPosition.y  - yOffset || transform.position.y > startPosition.y + yOffset) {
                // Update the floor's counter
                FloorManager.peopleOnFloors[startFloor]--;
                leftStartFloor = true;
            }
        }

        // Check if the person has gone out of bounds
        // if (transform.position.y < FloorManager.floors[0].transform.position.y) {
        //     FloorManager.peopleOnFloors[0]++;
        //     this.gameObject.SetActive(false);
        // }
        // if (transform.position.y > FloorManager.floors[FloorManager.floors.Count - 1].transform.position.y) {
        //     FloorManager.peopleOnFloors[FloorManager.floors.Count - 1]++;
        //     this.gameObject.SetActive(false);
        // }
    }

    IEnumerator ChangeFloors(float waitTime) {

        // Update the floor's counter
        if (!firstSpawn) FloorManager.peopleOnFloors[currentFloor]++;
        
        firstSpawn = false;
        changedFloors = true;

        // Set the person's velocity to 0
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        
        // Stay on the floor for a while before moving on
        yield return new WaitForSeconds(waitTime);


        List<GameObject> availableEscalators = new List<GameObject>(0);
        // Find which escalators the person can go onto
        for (int i = 0; i < FloorManager.escalators.Count; i++) {

            float floorHeight = FloorManager.floors[currentFloor].transform.position.y;

            // Check to see what escalators are available
            if (FloorManager.escalators[i].transform.position.y == floorHeight + FloorManager.distBetweenFloors/2) {
                if (!FloorManager.escalatorDirectionDown[i]) availableEscalators.Add(FloorManager.escalators[i]);
            }
            else if (FloorManager.escalators[i].transform.position.y == floorHeight - FloorManager.distBetweenFloors/2) {
                if (FloorManager.escalatorDirectionDown[i]) availableEscalators.Add(FloorManager.escalators[i]);
            }
        }


        // If there are any available escalators
        if (availableEscalators.Count >= 1) {
            // Randomly choose an escalator to go onto
            int randomEscalator = UnityEngine.Random.Range(0, availableEscalators.Count);

            // Check to see if the escalator is going up or down
            int index = 0;
            for (int i = 0; i < FloorManager.escalators.Count; i++) { 
                if (availableEscalators[randomEscalator].transform.position == FloorManager.escalators[i].transform.position) index = i;
            }

            if (FloorManager.escalatorDirectionDown[index]) goingDown = true;
            else goingDown = false;

            // Check to see if the floor can hold more people
            bool floorFull = false;
            bool floorEmpty = false;
            bool currentFloorEmpty = false;
            int nextFloor;
            if (goingDown) nextFloor = currentFloor - 1;
            else nextFloor = currentFloor + 1;
            if (FloorManager.peopleOnFloors[nextFloor] >= FloorManager.maxPeopleOnFloor - 1) floorFull = true;
            if (FloorManager.peopleOnFloors[nextFloor] <= 0) floorEmpty = true;
            if (FloorManager.peopleOnFloors[currentFloor] <= 0) currentFloorEmpty = true;

            // Check if there is already a person on the escalator
            bool anotherPersonOnEscalator = false;
            float xPos = availableEscalators[randomEscalator].transform.position.x;
            // float yPos = availableEscalators[randomEscalator].transform.position.y;
            // float personSize = FloorManager.people[0].GetComponent<SpriteRenderer>().bounds.extents.y * 2;
            // float yEscalator;

            // for (int i = 0; i < FloorManager.people.Count; i++) {
            //     float otherPersonX = FloorManager.people[i].transform.position.x;
            //     float otherPersonY = FloorManager.people[i].transform.position.y;

            //     if (otherPersonX == xPos) {
            //         if (goingDown) {
            //             yEscalator = yPos - 2;
            //             if (otherPersonY >= yEscalator && otherPersonY < yPos) anotherPersonOnEscalator = true;
            //         } else {
            //             yEscalator = yPos + 2;
            //             if (otherPersonY <= yEscalator && otherPersonY > yPos) anotherPersonOnEscalator = true;
            //         }
            //     }
            // }

            // Change floors if these conditions are met
            if (!floorEmpty && !floorFull && !currentFloorEmpty && !anotherPersonOnEscalator) {

                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

                startFloor = currentFloor;
                startPosition = transform.position;
                leftStartFloor = false;

                // Add velocity to the person
                if (goingDown) this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -FloorManager.personSpeed, 0);
                else this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, FloorManager.personSpeed, 0);

            } else {
                // Restart the wait time
                waitTime = UnityEngine.Random.Range(timeOnFloor[0], timeOnFloor[1]);
                StartCoroutine(ChangeFloors(waitTime));
            }
        } else {
            // Restart the wait time
            waitTime = UnityEngine.Random.Range(timeOnFloor[0], timeOnFloor[1]);
            StartCoroutine(ChangeFloors(waitTime));
        }
    }
}
