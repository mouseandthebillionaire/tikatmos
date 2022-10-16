using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficPerson : MonoBehaviour
{
    public GameObject sprite;
    public float yOffset;
    private float speed;

    public float[] timeOnFloor = new float[2];

    public float xVariation;
    public Color[] colors;

    private Vector3 startPosition;

    private int startFloor;
    private int currentFloor;

    private bool goingDown = false;

    private bool leftStartFloor = false;

    private bool changedFloors = false;

    private bool firstSpawn = true;

    private float currentY, previousY;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        // Default to half-speed
        speed = FloorManager.personSpeedMax + (FloorManager.personSpeedMax - FloorManager.personSpeedMin)/2;
        
        // Determine what floor the person started on
        for (int i = 0; i < FloorManager.floors.Count; i++) {
            if (startPosition.y == FloorManager.floors[i].transform.position.y) startFloor = i;
        }

        currentFloor = startFloor;


        // Wait before the person leaves the floor
        float waitTime = UnityEngine.Random.Range(timeOnFloor[0]/2, timeOnFloor[1]);
        StartCoroutine(ChangeFloors(waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFloor == startFloor) {
            // Check if the person has travelled up or down a floor
            if (transform.position.y >= startPosition.y + FloorManager.distBetweenFloors) currentFloor++;  
            if (transform.position.y <= startPosition.y - FloorManager.distBetweenFloors) currentFloor--;

            changedFloors = false;
        }  else {
            // Move the person to another floor
            if (!changedFloors) {
                // Set the person's velocity to 0
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
                this.gameObject.transform.position = FloorManager.floors[currentFloor].transform.position;

                // Determine how long the person stays on the floor
                float waitTime = UnityEngine.Random.Range(timeOnFloor[0], timeOnFloor[1]);
                StartCoroutine(ChangeFloors(waitTime));

                // Update the floor's counter
                if (!firstSpawn) FloorManager.peopleOnFloors[currentFloor]++;
            }
        }
        

        // Check if the person has left their starting floor
        if (!leftStartFloor) {
            if (transform.position.y < startPosition.y - yOffset || transform.position.y > startPosition.y + yOffset) {
                // Update the floor's counter
                FloorManager.peopleOnFloors[startFloor]--;
                leftStartFloor = true;
            }
        }

        // Check to see if the person has changed direction
        CheckDirection();
    }

    void CheckDirection() {
        // Determine if the person is going up or down
        currentY = this.transform.position.y;
        if (currentY > previousY) goingDown = false;
        else if (currentY < previousY) goingDown = true;
        
        // Find out which escalator the person is on
        for (int i = 0; i < FloorManager.escalators.Count; i++) {
            float xPos = FloorManager.escalators[i].transform.position.x;
            float yTop = FloorManager.escalators[i].transform.position.y + FloorManager.distBetweenFloors/2 - (yOffset/2);
            float yBottom = FloorManager.escalators[i].transform.position.y - FloorManager.distBetweenFloors/2 + (yOffset/2);

            if (this.transform.position.x == xPos) {
                if (currentY < yTop && currentY > yBottom) {

                    // Change person's speed to match the escalator's speed
                    float speedRange = FloorManager.personSpeedMax - FloorManager.personSpeedMin;
                    speed = FloorManager.personSpeedMin + (FloorManager.escalatorSpeed[i] * speedRange);

                    // If the person's direction doesn't match their escalator's direction
                    if (goingDown && !FloorManager.escalatorDirectionDown[i]) {
                        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
                        goingDown = false;
                        startFloor--;
                    } 
                    else if (!goingDown && FloorManager.escalatorDirectionDown[i]) {
                        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
                        goingDown = true;
                        startFloor++;
                    }

                    if (goingDown) this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -speed, 0);
                    else this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, speed, 0);

                    currentFloor = startFloor;
                    startPosition = FloorManager.floors[startFloor].transform.position;
                }
            }
        }

        previousY = currentY;
    }

    IEnumerator ChangeFloors(float waitTime) {

        // Vary the x location of the person so they appear more natural on the escalators
        float randomX = UnityEngine.Random.Range(-xVariation, xVariation);
        sprite.transform.localPosition = new Vector3(randomX, 0, 0);
        
        firstSpawn = false;
        changedFloors = true;
        
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
            int nextFloor;
            if (goingDown) nextFloor = currentFloor - 1;
            else nextFloor = currentFloor + 1;
            if (FloorManager.peopleOnFloors[nextFloor] >= FloorManager.maxPeopleOnFloor) floorFull = true;
            if (FloorManager.peopleOnFloors[currentFloor] <= 0) floorEmpty = true;

            // Check if there is already a person nearby on the escalator
            bool anotherPersonOnEscalator = false;
            float xPos = availableEscalators[randomEscalator].transform.position.x;
            float yPos;
            for (int i = 0; i < FloorManager.people.Count; i++) {
                if (FloorManager.people[i].transform.position.x == xPos) {
                    float personY = FloorManager.people[i].transform.position.y;
                    float yRange = yOffset * 3f;

                    if (goingDown) {
                        yPos = availableEscalators[randomEscalator].transform.position.y + (FloorManager.distBetweenFloors/2);
                        if (personY < yPos && personY > yPos - yRange) anotherPersonOnEscalator = true;
                    }
                    else {
                        yPos = availableEscalators[randomEscalator].transform.position.y - (FloorManager.distBetweenFloors/2);
                        if (personY > yPos && personY < yPos + yRange) anotherPersonOnEscalator = true;
                    }
                }
            }

            // Check to see if the escalator has already been used by another person
            bool escalatorUsed = false;
            for (int i = 0; i < FloorManager.usedEscalators.Count; i++) {
                if (FloorManager.usedEscalators[i] == randomEscalator) escalatorUsed = true;
            }

            // Change floors if these conditions are met
            if (!floorEmpty && !floorFull && !anotherPersonOnEscalator && !escalatorUsed) {

                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

                startFloor = currentFloor;
                startPosition = transform.position;
                leftStartFloor = false;

                // Add velocity to the person
                if (goingDown) this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -speed, 0);
                else this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, speed, 0);

                FloorManager.usedEscalators.Add(randomEscalator);

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
