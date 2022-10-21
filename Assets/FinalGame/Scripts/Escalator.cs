using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class Escalator : MonoBehaviour
{
    public  GameObject arrows;
    public  bool       escalatorDown = false; // 0 = down / 1 = up
    public  float      upperLimit, lowerLimit;
    public  float      speed = .01f;
    public  float      minSpeed, maxSpeed;
    private int        dir;
    private bool       selected;
    
    public Color escalatorInactive, escalatorActive;

    
    // Start is called before the first frame update
    void Start()
    {
        arrows = transform.GetChild(1).gameObject;
        // Randomly assign up or down
        int down = UnityEngine.Random.Range(0, 2);
        if (down == 1) escalatorDown = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (selected && Input.GetKeyDown(GlobalVariables.S.deviceButton))
        {
            if (escalatorDown) escalatorDown = false;
            else escalatorDown = true;
        }

        if (selected && Input.GetKeyDown(GlobalVariables.S.knob0_right) && speed < maxSpeed) speed += .001f;
        if (selected && Input.GetKeyDown(GlobalVariables.S.knob1_left) && speed > minSpeed) speed -= .001f;

        
        AnimateArrows();

    }
    
    void AnimateArrows()
    {
        if (escalatorDown)
        {
            dir = -1;
            arrows.transform.localRotation = Quaternion.Euler(0 ,0 , 180);
        }
        else
        {
            dir = 1;
            arrows.transform.localRotation = Quaternion.Euler(0 ,0 , 0);
        }
        
        float currentY = arrows.transform.localPosition.y;
        float newY = currentY + speed * dir;
        
        if (newY > upperLimit) newY = lowerLimit;
        if (newY < lowerLimit) newY = upperLimit;
        
        arrows.transform.localPosition = new Vector3(arrows.transform.localPosition.x, newY, 0);
    }

    public void MakeActive()
    {
        this.GetComponent<SpriteRenderer>().color = escalatorActive;
        selected = true;
    }
    
    public void MakeInactive()
    {
        this.GetComponent<SpriteRenderer>().color = escalatorInactive;
        selected = false;
    }
}
