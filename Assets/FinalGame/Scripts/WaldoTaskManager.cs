using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaldoTaskManager : MonoBehaviour
{
    public GameObject person;
    public Text goalMessage;
    public Image goalMessageBorder;
    private GameObject[] people;

    public int numberOfPeople;

    public bool isWaldo;

    public static bool goalCompleted = true;

    private int lastGoalPerson = 0;

    public Color[] goals;
    public string[] goalMessages;
    
    public static int goalsCompleted = -1;

    public static float goalX, goalY;

    public float startTime, timerDecrease, minimumTimer;

    private int lastGoal = -1;
    private int lastLastGoal = -1;

    private bool childFound = false;
    private int[] previousMessage = new int[7];

    // Start is called before the first frame update
    void Start()
    {
        people = new GameObject[numberOfPeople];

        for (int i = 0; i < numberOfPeople; i++) {

            // Choose a random color for the person
            int randomColor = Random.Range(0, goals.Length);
            person.gameObject.GetComponentInChildren<SpriteRenderer>().color = goals[randomColor];

            
            
            people[i] = Instantiate(person);
            people[i].transform.parent = this.transform;
        }

        person.SetActive(false);

        WaldoManager.timer = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (goalCompleted) {
            // Determine who was the last person to be a goal
            for (int i = 0; i < people.Length; i++) {
                if (people[i].gameObject.transform.GetChild(0).tag == "Goal") lastGoalPerson = i;
            }

            // Update the goal counter
            goalsCompleted++;

            StartCoroutine(CreateGoal());
            goalCompleted = false;
        } else {
            // Reset the last person who was a goal
            Transform person = people[lastGoalPerson].gameObject.transform.GetChild(0);
            person.tag = "Untagged";
            person.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

            // Find out the x and y positions of who is currently the goal
            int currentGoal = 0;
            for (int i = 0; i < people.Length; i++) {
                if (people[i].gameObject.transform.GetChild(0).tag == "Goal") currentGoal = i;
            }
            goalX = people[currentGoal].gameObject.transform.GetChild(0).transform.position.x;
            goalY = people[currentGoal].gameObject.transform.GetChild(0).transform.position.y;
        }

        // Decrease the timer each time a goal is completed
        if (goalsCompleted > 0) {
            float newtimer = startTime - (goalsCompleted * timerDecrease);
            if (newtimer > minimumTimer) WaldoManager.timer = newtimer;
            else WaldoManager.timer = minimumTimer;
        }
    }

    IEnumerator CreateGoal() {

        yield return new WaitForSeconds(0f);

        // Generate a random goal
        int randomGoal = Random.Range(0, goals.Length);

        // Check to see if the same goal was generated
        if (randomGoal == lastGoal || randomGoal == lastLastGoal) {
            if (randomGoal > 0) randomGoal--;
            else randomGoal++;
        }

        // Make sure the lost child isn't returned to their parents before they are found
        if (lastGoal == 2) {
            childFound = true;
            randomGoal = 3;
        }
        if (lastGoal == 3) childFound = false;
        if (!childFound && randomGoal == 3) {
            if (lastLastGoal != 2 && lastGoal != 2) randomGoal = 2;
            else randomGoal = 6;
        }

        // Determine the number of people who are the goal's color
        List<GameObject> somePeople = new List<GameObject>(0);

        for (int i = 0; i < people.Length; i++) {
            if (people[i].gameObject.GetComponentInChildren<SpriteRenderer>().color == goals[randomGoal]) somePeople.Add(people[i]);
        }

        // Randomly choose a person
        int goalPerson = Random.Range(0, somePeople.Count);

        if (somePeople[goalPerson].gameObject.transform.GetChild(0) == people[lastGoalPerson].gameObject.transform.GetChild(0)) {
            if (lastGoalPerson < people.Length - 1) goalPerson++;
            else if (lastGoalPerson > 0) goalPerson--;
            else goalPerson = lastGoalPerson++;
        }

        // Change the tag of the object
        somePeople[goalPerson].gameObject.transform.GetChild(0).tag = "Goal";

        // Display the pointer
        Transform person = somePeople[goalPerson].gameObject.transform.GetChild(0);
        //person.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        // OR make them glow?
        GameObject halo = person.transform.GetChild(2).gameObject;
        halo.SetActive(true); 

        // Change the goal text
        goalMessage.color = goals[randomGoal];
        goalMessageBorder.color = goals[randomGoal];

        // Create a random citizen ID
        int i_0 = Random.Range(10, 99);
        int i_1 = Random.Range(10, 99);
        string st = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char c_0 = st[Random.Range(0, st.Length)];
        char c_1 = st[Random.Range(0, st.Length)];

        string cID = i_0.ToString() + c_0 + c_1 + i_1.ToString();
        
        int choice = Random.Range(0,3);
        if (previousMessage[randomGoal] == choice) {
            if (choice > 0) choice--;
            else choice++;
        }

        goalMessage.text = "∂" + cID;

        // if (choice == 0) goalMessage.text = goalMessages[randomGoal];
        // else if (choice == 1) goalMessage.text = goalMessages[randomGoal + goals.Length];
        // else goalMessage.text = goalMessages[randomGoal + (goals.Length*2)];

        previousMessage[randomGoal] = choice;

        lastLastGoal = lastGoal;
        lastGoal = randomGoal;
    }
}
