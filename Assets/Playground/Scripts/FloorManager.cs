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

    public float escalatorSpeedIncrease;
    public float defaultSpeed;

    public Canvas canvas;
    public Image floorNumber;

    public GameObject arrow;
    public float arrowLoop, arrowOffset;
    public static float personSpeedMin = 0.5f;
    public static float personSpeedMax = 1.5f;
    public static List<GameObject> arrows = new List<GameObject>();

    public Color[] floorColors = new Color[8];
    public Color[] floorBackgroundColors = new Color[8];

    public static List<Image> floorNumbers = new List<Image>();

    private int currentEscalator, currentFloor;
    private int[] escalatorsOnFloor = new int[0];

    public static int selectedFloor;

    public static bool[] escalatorDirectionDown = new bool[0];
    public static float[] escalatorSpeed = new float[0];

    public static List<GameObject> peopleOnEscalators = new List<GameObject>();
    public static List<int> usedEscalators = new List<int>();

    public Color active, inactive, selected;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn Floors
        for (int i = 0; i < numberOfFloors; i++) {
            float yLoc = distBetweenFloors * i;
            Vector3 loc = new Vector3(floor.transform.position.x, yLoc, 0);

            floors.Add(Instantiate(floor, loc, Quaternion.identity));
            floors[i].transform.parent = this.transform;

            // Spawn the numbers for each floor
            float floorNumberY = floorNumber.transform.position.y + (distBetweenFloors * i);
            Vector3 floorNumberPosition = new Vector3(floorNumber.transform.position.x, floorNumberY, 0);
            floorNumbers.Add(Instantiate(floorNumber, floorNumberPosition, Quaternion.identity));
            floorNumbers[i].transform.parent = this.transform;

            floorNumbers[i].transform.SetParent(canvas.transform);
            floorNumbers[i].GetComponentInChildren<Text>().text = "Level " + (i+1);
            floorNumbers[i].color = floorColors[i];
            floors[i].GetComponent<SpriteRenderer>().color = floorBackgroundColors[i];

            // Determine how many people are on each floor
            peopleOnFloors.Add(UnityEngine.Random.Range(maxPeopleOnFloor/3, maxPeopleOnFloor*2/3));
            if (i != 0) { 
                if (peopleOnFloors[i - 1] < maxPeopleOnFloor/2) peopleOnFloors[i] = UnityEngine.Random.Range(maxPeopleOnFloor/2, maxPeopleOnFloor*5/6);
                else peopleOnFloors[i] = UnityEngine.Random.Range(maxPeopleOnFloor/6, maxPeopleOnFloor/2);
            }

            // Spawn the people
            for (int j = 0; j < peopleOnFloors[i]; j++) {
                // Randomly choose a color for the person
                int randomIndex = UnityEngine.Random.Range(0, floorColors.Length);
                person.GetComponentInChildren<SpriteRenderer>().color = floorColors[randomIndex];

                Vector3 spawnLoc = new Vector3(floors[i].transform.position.x, floors[i].transform.position.y, 0);
                people.Add(Instantiate(person, spawnLoc, Quaternion.identity));
                people[j].transform.parent = this.gameObject.transform;
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
                escalators[i].transform.parent = this.transform;

                // Spawn the arrows 
                arrows.Add(Instantiate(arrow, loc, Quaternion.identity)); 
                arrows[i].transform.parent = this.transform;
            }

            // Keep track of how many escalators are on each floor
            escalatorsOnFloor[i] = numberOfEscalators;
        }

        Array.Resize(ref escalatorDirectionDown, escalators.Count);
        Array.Resize(ref escalatorSpeed, escalators.Count);
        // Default the escalator speed to the default within a range of 0 to 1
        for (int i = 0; i < escalatorSpeed.Length; i++) escalatorSpeed[i] = defaultSpeed;

        // Randomly choose whether the escalators are facing up or down
        for (int i = 0; i < escalators.Count; i++) {
            int binaryChoice = UnityEngine.Random.Range(0, 2);
            if (binaryChoice == 0) {
                escalators[i].transform.rotation = Quaternion.Euler(0, 0, 180);
                arrows[i].transform.rotation = Quaternion.Euler(0, 0, 180);
                escalatorDirectionDown[i] = true;

                // Reposition the arrows
                float yPos = escalators[i].transform.position.y + arrowOffset;
                arrows[i].transform.position = new Vector3(arrows[i].transform.position.x, yPos, 0);
            } else {
                escalators[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                arrows[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                escalatorDirectionDown[i] = false;

                // Reposition the arrows
                float yPos = escalators[i].transform.position.y - arrowOffset;
                arrows[i].transform.position = new Vector3(arrows[i].transform.position.x, yPos, 0);
            }
        }

        floor.SetActive(false);
        escalator.SetActive(false);
        arrow.SetActive(false);
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

            int safeZone = (int) (maxPeopleOnFloor * TrafficCamera.S.mediumThreshold);
            int dangerZone = (int) (maxPeopleOnFloor * TrafficCamera.S.dangerThreshold);

            //if (peopleOnFloors[i] <= dangerZone || peopleOnFloors[i] >= maxPeopleOnFloor - dangerZone) floorNumbers[i].color = TrafficCamera.S.danger;
            //else if (peopleOnFloors[i] <= safeZone || peopleOnFloors[i] >= maxPeopleOnFloor - safeZone) floorNumbers[i].color = TrafficCamera.S.medium;
            //else floorNumbers[i].color = TrafficCamera.S.safe;
        }

        // Set Level 0 to "Garage"
        floorNumbers[0].GetComponentInChildren<Text>().text = "Garage";
        //floorNumbers[0].fillAmount = 100;
        //floorNumbers[0].color = Color.gray;

        // Increment the timer
        if (time == 0) spawnWaitTime = UnityEngine.Random.Range(personSpawnRate[0], personSpawnRate[1]);
        time += Time.deltaTime;

        usedEscalators.Clear();

        // Animate the arrows moving
        for (int i = 0; i < arrows.Count; i++) {
            float speed = escalatorSpeed[i] * (personSpeedMax - personSpeedMin);
            AnimateArrows(i, personSpeedMin + speed);
        }
    }

    void AnimateArrows(int i, float speed) {
        float currentY = arrows[i].transform.position.y;

        if (escalatorDirectionDown[i]) {
            arrows[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0,-speed);
            if (currentY <= escalators[i].transform.position.y - arrowLoop + arrowOffset) currentY = escalators[i].transform.position.y + arrowOffset;
        } else {
            arrows[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0,speed);
            if (currentY >= escalators[i].transform.position.y + arrowLoop - arrowOffset) currentY = escalators[i].transform.position.y - arrowOffset;
        }

        arrows[i].transform.position = new Vector3(arrows[i].transform.position.x, currentY, 0);
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
        if (Input.GetKeyDown(GlobalVariables.S.knob0_left)) {
            if (currentEscalator > lowerLimit) {
                currentEscalator--;
            }
        }
        if (Input.GetKeyDown(GlobalVariables.S.knob0_right)) {
            if (currentEscalator < upperLimit - 1) {
                currentEscalator++;
            }
        }

        // Deselect all escalators
        for (int i = 0; i < arrows.Count; i++) {
            arrows[i].GetComponent<SpriteRenderer>().color = active;
            escalators[i].GetComponentsInChildren<SpriteRenderer>()[1].color = active;
        }

        // Highlight the currently selected escalator
        arrows[currentEscalator].GetComponent<SpriteRenderer>().color = selected;
        escalators[currentEscalator].GetComponentsInChildren<SpriteRenderer>()[1].color = selected;


        // Change the direction of the selected escalator
        if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) {
            if (escalators[currentEscalator].transform.rotation.z == 0) {
                escalators[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 180);
                arrows[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 180);
                escalatorDirectionDown[currentEscalator] = true;

            } else {
                escalators[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 0);
                arrows[currentEscalator].transform.rotation = Quaternion.Euler(0, 0, 0);
                escalatorDirectionDown[currentEscalator] = false;
            }
        }

        // Change the speed of the selected escalator
        if (Input.GetKeyDown(GlobalVariables.S.knob1_right)) {
            if (escalatorSpeed[currentEscalator] < 1) escalatorSpeed[currentEscalator] += escalatorSpeedIncrease;
        }
        if (Input.GetKeyDown(GlobalVariables.S.knob1_left)) {
            if (escalatorSpeed[currentEscalator] > 0) escalatorSpeed[currentEscalator] -= escalatorSpeedIncrease;
        }
    }

    public float map(float value, float oldMin, float oldMax, float newMin, float newMax){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;
    
        return(newValue);
    }
}
