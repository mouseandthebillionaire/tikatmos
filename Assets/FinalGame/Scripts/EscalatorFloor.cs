using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EscalatorFloor : MonoBehaviour
{
    public  int   numCitizens;
    public  Image citizenUI;
    public  Image citizenMeter;
    private int   status; // 0 = good, 1 = fine, 2 = bad;

    public int      floorNum;

    public GameObject escalator;


    // Start is called before the first frame update
    void Start()
    {
        // How many people are on the floor?
        numCitizens = Random.Range(25, 75);
        
        // Spawn escalators 
        // Make it random later
        // int numberOfEscalators = Random.Range(2, 4);
        // But for now
        int numberOfEscalators;
        if (floorNum == 7) numberOfEscalators = 0;
        else numberOfEscalators = 3;
        
        GameObject floorBG = transform.GetChild(0).gameObject;
        for (int i = 0; i < numberOfEscalators; i++) {
            float xDistance = floorBG.GetComponent<SpriteRenderer>().bounds.extents.x;
            float xDiff = xDistance * 2 / (numberOfEscalators + 1);
            float xLoc = -xDistance + (xDiff * (i+1)) + floorBG.transform.position.x;
            Vector3 loc = new Vector3(xLoc, this.transform.position.y + 1.25f, 0);

            EscalatorManager.S.escalators.Add(Instantiate(escalator, loc, Quaternion.identity, this.transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float visual = numCitizens / 100f;
        EscalatorManager.S.statusUIs[floorNum].transform.GetChild(0).GetComponent<Image>().fillAmount = visual;
        citizenMeter.GetComponent<Image>().fillAmount = visual;
        
        // Get Warning
        if (numCitizens > 90 || numCitizens < 10) status = 2;
        else if (numCitizens > 70 || numCitizens < 30) status = 1;
        else status = 0;

        EscalatorManager.S.statusUIs[floorNum].transform.GetChild(2).GetComponent<Image>().color =
            EscalatorManager.S.statusColors[status];

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "ExitPoint" && numCitizens > 0) numCitizens -= 1;
        if (other.name == "EntryPoint" && numCitizens < 100) numCitizens += 1;
        
    }
}
