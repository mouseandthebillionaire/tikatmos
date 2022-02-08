using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MusicManip : MonoBehaviour
{
    public LibPdInstance pdPatch;
    public Slider mainSlider;

    public static MusicManip S;

    public void Start() {
        S = this;
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        pdPatch.SendFloat("sliderValue", mainSlider.value);
    }

    void OnMouseDown () {

        MusicShift();
    }

    public void MusicShift() {
        pdPatch.SendBang("knobOn");
    }
}
