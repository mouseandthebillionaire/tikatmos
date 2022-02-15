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

    private int child;

    public bool isWaldo;

    public Color[] goals;


    // Start is called before the first frame update
    void Start()
    {
        people = new GameObject[numberOfPeople];

        for (int i = 0; i < numberOfPeople; i++) {
            people[i] = person;
            Instantiate(people[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Generate special people for the player to search for
        if (isWaldo) {
            // Generate a random number
            child = Random.Range(0,people.Length - 1);

            // Change the tag of the object
            people[child].gameObject.transform.GetChild(0).tag = "Goal";

            // Change the color of the child person
            people[child].gameObject.GetComponentInChildren<SpriteRenderer>().color = goals[0];

            // Change the text goal
            goalMessage.color = goals[0];
        }
    }
}
