using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarEvent : MonoBehaviour
{
    private GameObject[] event_title = new GameObject[2];
    private GameObject[] event_description = new GameObject[2];
    
    private bool         translated;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < event_title.Length; i++)
        {
            event_title[i] = transform.GetChild(1).GetChild(i).gameObject;
            event_description[i] = transform.GetChild(1).GetChild(i + 2).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // toggle the text
        if (Input.GetKey(GlobalVariables.S.deviceButton)) translated = true;
        else translated = false;

        DisplayText();
    }

    public void DisplayText()
    {
        
        // TIKATMOS Text
        event_title[0].gameObject.SetActive(!translated);
        event_description[0].gameObject.SetActive(!translated);
        
        // English Text
        event_title[1].gameObject.SetActive(translated);
        event_description[1].gameObject.SetActive(translated);
        
    }
}
