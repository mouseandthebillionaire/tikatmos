using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficPerson : MonoBehaviour
{
    public int despawnChance;

    public float yOffset;

    public float xBounds;

    private Vector3 startPosition;

    private int startFloor;
    private int currentFloor;

    private bool goingDown = false;

    private bool leftStartFloor = false;

    private float wandertimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        
        // Determine what floor the person started on
        for (int i = 0; i < FloorManager.floors.Count; i++) {
            if (startPosition.y == FloorManager.floors[i].transform.position.y) startFloor = i;
        }

        currentFloor = startFloor;
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
        }  
        else NewFloor();

        // Check if the person has left their starting floor
        if (!leftStartFloor) {
            if (transform.position.y < startPosition.y - yOffset || transform.position.y > startPosition.y + yOffset) {
                // Update the floor's counter
                FloorManager.peopleOnFloors[startFloor]--;
                leftStartFloor = true;
            }
        }

        // Check if the person has gone out of bounds
        if (transform.position.y < FloorManager.floors[0].transform.position.y) {
            FloorManager.peopleOnFloors[0]++;
            this.gameObject.SetActive(false);
        }
        if (transform.position.y > FloorManager.floors[FloorManager.floors.Count - 1].transform.position.y) {
            FloorManager.peopleOnFloors[FloorManager.floors.Count - 1]++;
            this.gameObject.SetActive(false);
        }
    }

    void NewFloor() {
        // Update the floor's counter
        FloorManager.peopleOnFloors[currentFloor]++;
        leftStartFloor = false;

        // Determine whether the person stays on the floor or leaves
        int choice = UnityEngine.Random.Range(0, despawnChance);
        if (choice == 0) this.gameObject.SetActive(false);

        List<GameObject> availableEscalators = new List<GameObject>(0);

        // Find which escalators the person can go onto
        for (int i = 0; i < FloorManager.escalators.Count; i++) {

            // Check to see if the person is going up or down
            float comparison;
            if (goingDown) comparison = FloorManager.floors[currentFloor - 1].transform.position.y;
            else comparison = FloorManager.floors[currentFloor].transform.position.y;

            // Check to see if any escalators are deactivated
            if (FloorManager.escalators[i].transform.position.y == comparison + FloorManager.distBetweenFloors/2) {
                if (goingDown && FloorManager.downIsActive[i]) availableEscalators.Add(FloorManager.escalators[i]);
                if (!goingDown && FloorManager.upIsActive[i]) availableEscalators.Add(FloorManager.escalators[i]);
            }
        }

        // If there are any available escalators
        if (availableEscalators.Count >= 1) {
            // Randomly choose an escalator to go onto
            int randomEscalator = UnityEngine.Random.Range(0, availableEscalators.Count);

            // Check to see if the person is going up or down
            float xPos = availableEscalators[randomEscalator].transform.position.x;
            if (goingDown) xPos -= FloorManager.distBetweenEscalators/2;
            else xPos += FloorManager.distBetweenEscalators/2;
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            startFloor = currentFloor;
            startPosition = transform.position;
        } 
        else this.gameObject.SetActive(false);
    }
}
