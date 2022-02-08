using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject person;
    private GameObject[] people;

    public int numberOfPeople;

    private int child;

    public bool isWaldo;


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

            // Change the color of the child person
            people[child].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255,165,0);
        }
    }
}
