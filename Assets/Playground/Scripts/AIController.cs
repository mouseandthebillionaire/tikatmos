using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    public GameObject person;
    public Text goalMessage;
    private GameObject[] people;

    public int numberOfPeople;

    public bool isWaldo;

    public static bool goalCompleted = true;

    private int lastGoalPerson = 0;

    public Color[] goals;
    public string[] goalMessages;

    public static List<int> goalsCompleted = new List<int>(0);

    public static float goalX, goalY;

    public Image[] badges;

    // Start is called before the first frame update
    void Start()
    {
        people = new GameObject[numberOfPeople];

        for (int i = 0; i < numberOfPeople; i++) {

            // Choose a random color for the person
            int randomColor = Random.Range(0, goals.Length);
            person.gameObject.GetComponentInChildren<SpriteRenderer>().color = goals[randomColor];

            people[i] = Instantiate(person);
        }

        person.SetActive(false);
        
        for (int i = 0; i < badges.Length; i++) goalsCompleted.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (goalCompleted) {
            // Determine who was the last person to be a goal
            for (int i = 0; i < people.Length; i++) {
                if (people[i].gameObject.transform.GetChild(0).tag == "Goal") lastGoalPerson = i;
            }

            // Show the goal was completed
            Color personColor = people[lastGoalPerson].gameObject.GetComponentInChildren<SpriteRenderer>().color;
            for (int i = 0; i < badges.Length; i++) if (badges[i].color == personColor) goalsCompleted[i]++;

            CreateGoal();
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

        // Update the badges
        for (int i = 0; i < badges.Length; i++) {
            badges[i].color = goals[i];
            badges[i].gameObject.GetComponentInChildren<Text>().text = "" + goalsCompleted[i];
        }
    }

    void CreateGoal() {
        // Generate a random goal
        int randomGoal = Random.Range(0, goals.Length);

        // Determine the number of people who are the goal's color
        List<GameObject> somePeople = new List<GameObject>(0);

        for (int i = 0; i < people.Length; i++) {
            if (people[i].gameObject.GetComponentInChildren<SpriteRenderer>().color == goals[randomGoal]) somePeople.Add(people[i]);
        }

        // Randomly choose a person
        int goalPerson = Random.Range(0, somePeople.Count);

        if (somePeople[goalPerson].gameObject.transform.GetChild(0) == people[lastGoalPerson].gameObject.transform.GetChild(0)) {
            if (lastGoalPerson < people.Length - 1) goalPerson++;
            else goalPerson--;
        }

        // Change the tag of the object
        somePeople[goalPerson].gameObject.transform.GetChild(0).tag = "Goal";

        // Display the pointer
        Transform person = somePeople[goalPerson].gameObject.transform.GetChild(0);
        person.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

        // Change the goal text
        goalMessage.color = goals[randomGoal];
        goalMessage.text = goalMessages[randomGoal];
    }
}
