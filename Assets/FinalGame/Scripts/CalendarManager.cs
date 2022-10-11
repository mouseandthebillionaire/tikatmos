using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    public GameObject calendar;

    private float yPos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        yPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) yPos += .1f;
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) yPos -= .1f;
        
        calendar.transform.position = new Vector3(calendar.transform.position.x, yPos, 0);

    }
}
