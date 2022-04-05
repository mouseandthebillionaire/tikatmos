using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Manager differs from MusicManip in that it uses the knobs/button/crank to control
    
    public LibPdInstance trackPatch;
    public LibPdInstance ambientPatch;

    public float hKnobVal, vKnobVal, crankVal;

    public static MusicManager S;

    public void Start() {
        S = this;

        // MRB - Let's see if we can dynamically find our PD instances in the Main scene
        ambientPatch = GameObject.Find("Ambient_Background").GetComponent<LibPdInstance>();
        trackPatch = GameObject.Find("Tracks").GetComponent<LibPdInstance>();
        
        // initialize values to something more interesting later
        hKnobVal = 0; //  1-100
        vKnobVal = 0; //  0-1
        crankVal = 0; // -200 to 200
    }

    public void Update()
    {
        if (Input.GetKey(GlobalVariables.S.leftSlider) && (hKnobVal > 1))
        {
            hKnobVal -= .1f;
            ValueChangeCheckFilter(hKnobVal);
        }

        if (Input.GetKey(GlobalVariables.S.rightSlider) && (hKnobVal < 100))
        {
            hKnobVal += .1f;
            ValueChangeCheckFilter(hKnobVal);
        }


        if (Input.GetKey(GlobalVariables.S.upCrank) && (crankVal > -200))
        {
            crankVal -= .1f;
            ValueChangeCheckTime(crankVal);
        }

        if (Input.GetKey(GlobalVariables.S.downCrank) && (crankVal < 200))
        {
            crankVal += .1f;
            ValueChangeCheckTime(crankVal);
        }

        if (Input.GetKey(GlobalVariables.S.knobLeft) && (vKnobVal > 0))
        {
            vKnobVal -= .001f;
            ValueChangeCheckVolume(vKnobVal);
        }

        if (Input.GetKey(GlobalVariables.S.knobRight) && (vKnobVal < 1))
        {
            vKnobVal += .001f;
            ValueChangeCheckVolume(vKnobVal);
        }

        if (Input.GetKey(GlobalVariables.S.deviceButton))
        {
            MusicShift();
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
}
