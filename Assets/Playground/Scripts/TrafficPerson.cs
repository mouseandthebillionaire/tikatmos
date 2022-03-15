using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrafficPerson : MonoBehaviour
{
    public int despawnChance;

    public float yOffset;

    public float xBounds;

    public float[] timeOnFloor = new float[2];

    // This is not racist, I'm only sorting people by color
    public Color[] colors;

    private Vector3 startPosition;

    private int startFloor;
    private int currentFloor;

    private bool goingDown = false;

    private bool leftStartFloor = false;

    private bool changedFloors = false;

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
        }  
        else {
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
        if (transform.position.y < FloorManager.floors[0].transform.position.y) {
            FloorManager.peopleOnFloors[0]++;
            this.gameObject.SetActive(false);
        }
        if (transform.position.y > FloorManager.floors[FloorManager.floors.Count - 1].transform.position.y) {
            FloorManager.peopleOnFloors[FloorManager.floors.Count - 1]++;
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator ChangeFloors(float waitTime) {

        // Update the floor's counter
        FloorManager.peopleOnFloors[currentFloor]++;
        changedFloors = true;

        // Set the person's velocity to 0
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        
        // Stay on the floor for a while before moving on
        yield return new WaitForSeconds(waitTime);

        List<GameObject> availableEscalators = new List<GameObject>(0);

        // Determine whether the person is going up or down
        int choice = UnityEngine.Random.Range(0, 2);
        if (choice == 0) {
            if (currentFloor > 0) goingDown = true;
            else goingDown = false;
        }
        else {
            if (currentFloor < FloorManager.floors.Count - 1) goingDown = false;
            else goingDown = true;
        }

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

        // Check to see if the floor can hold more people
        bool floorFull = false;
        bool floorEmpty = false;
        if (FloorManager.peopleOnFloors[currentFloor] >= FloorManager.maxPeopleOnFloor - 1) floorFull = true;
        if (FloorManager.peopleOnFloors[currentFloor] <= 0) floorEmpty = true;

        // If there are any available escalators
        if (availableEscalators.Count >= 1 && !floorFull && !floorEmpty) {
            // Randomly choose an escalator to go onto
            int randomEscalator = UnityEngine.Random.Range(0, availableEscalators.Count);

            // Check to see if the person is going up or down
            float xPos = availableEscalators[randomEscalator].transform.position.x;
            if (goingDown) xPos -= FloorManager.distBetweenEscalators/2;
            else xPos += FloorManager.distBetweenEscalators/2;
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            startFloor = currentFloor;
            startPosition = transform.position;
            leftStartFloor = false;

            // Add velocity to the person
            if (goingDown) this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -FloorManager.personSpeed, 0);
            else this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, FloorManager.personSpeed, 0);
        } 
    }
}
