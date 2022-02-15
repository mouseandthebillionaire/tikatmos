using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MusicManip : MonoBehaviour
{
    public LibPdInstance trackPatch;
    public LibPdInstance ambientPatch;
    public Slider mainSlider;
    public Slider volume;

    public static MusicManip S;

    public void Start() {
        S = this;
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        volume.onValueChanged.AddListener(delegate {ValueChangeCheckVolume(); });
    }

    public void ValueChangeCheck()
    {
        trackPatch.SendFloat("crankValue", mainSlider.value);
        ambientPatch.SendFloat("crankValue2", mainSlider.value);
    }

     public void ValueChangeCheckVolume()
    {
        trackPatch.SendFloat("sliderValue", volume.value);
        ambientPatch.SendFloat("sliderValue2", volume.value);
    }

    void OnMouseDown () {

        MusicShift();
    }

    public void MusicShift() {
        ambientPatch.SendBang("knobOn");
        trackPatch.SendBang("knobOn");
    }
}
