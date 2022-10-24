using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Manager differs from MusicManip in that it uses the knobs/button/crank to control
    
    public LibPdInstance trackPatch;
    public LibPdInstance ambientPatch;

    public float blendVal,      filterVal,      timeVal;
    public float leftKnobSpeed, rightKnobSpeed, crankSpeed;

    public GameObject leftKnob, rightKnob, middleButton, needle;

    public static MusicManager S;

    public void Start() {
        S = this;

        // MRB - Let's see if we can dynamically find our PD instances in the Main scene
        ambientPatch = GameObject.Find("Ambient_Background").GetComponent<LibPdInstance>();
        trackPatch = GameObject.Find("Tracks").GetComponent<LibPdInstance>();
        
        // initialize values to something more interesting later
        filterVal = 100; //  1-100
        blendVal = 0.5f; //  0-1
        timeVal = 100; // -200 to 200
    }

    public void Update()
    {
        // Filter
        if (Input.GetKey(GlobalVariables.S.knob0_left) && (filterVal > 1))
        {
            filterVal -= leftKnobSpeed;
            ValueChangeCheckFilter(filterVal);
        }

        if (Input.GetKey(GlobalVariables.S.knob0_right) && (filterVal < 100))
        {
            filterVal += leftKnobSpeed;
            ValueChangeCheckFilter(filterVal);
        }

        leftKnob.transform.rotation = Quaternion.Euler(0, 0, filterVal * 3f);

        // Time
        if (Input.GetKey(GlobalVariables.S.upCrank) && (timeVal > -200))
        {
            timeVal -= crankSpeed;
            ValueChangeCheckTime(timeVal);
        }

        if (Input.GetKey(GlobalVariables.S.downCrank) && (timeVal < 200))
        {
            timeVal += crankSpeed;
            ValueChangeCheckTime(timeVal);
        }

        needle.transform.rotation = Quaternion.Euler(0, 0, timeVal / -5f);

        // Blend
        if (Input.GetKey(GlobalVariables.S.knob1_left) && (blendVal > 0))
        {
            blendVal -= rightKnobSpeed;
            ValueChangeCheckVolume(blendVal);
        }

        if (Input.GetKey(GlobalVariables.S.knob1_right) && (blendVal < 1))
        {
            blendVal += rightKnobSpeed;
            ValueChangeCheckVolume(blendVal);
        }
        rightKnob.transform.rotation = Quaternion.Euler(0, 0, blendVal * 360f);

        // Change Track
        if (Input.GetKey(GlobalVariables.S.deviceButton))
        {
            MusicShift();
            StartCoroutine(ColorPulse());
        }
    }



    public void ValueChangeCheckFilter(float _value)
    {
        trackPatch.SendFloat("filterValue", _value);
        ambientPatch.SendFloat("filterValue2", _value);
    }

    public void ValueChangeCheckVolume(float _value)
    {
        trackPatch.SendFloat("sliderValue", _value);
        ambientPatch.SendFloat("sliderValue2", _value);
    }

    public void ValueChangeCheckTime(float _value)
    {
        trackPatch.SendFloat("crankValue", _value);
        ambientPatch.SendFloat("crankValue2", _value);
    }

    public void MusicShift() {
        ambientPatch.SendBang("knobOn");
        trackPatch.SendBang("knobOn");
    }

    private IEnumerator ColorPulse() {
        middleButton.GetComponent<SpriteRenderer>().color = new Color (1f, 0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(1);
        middleButton.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
    }
}
