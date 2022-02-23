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
    public Slider filter;

    public static MusicManip S;

    public void Start() {
        S = this;
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        volume.onValueChanged.AddListener(delegate {ValueChangeCheckVolume(); });
        filter.onValueChanged.AddListener(delegate {ValueChangeCheckFilter(); });
        
        // MRB - Let's see if we can dynamically find our PD instances in the Main scene
        ambientPatch = GameObject.Find("Ambient_Background").GetComponent<LibPdInstance>();
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

     public void ValueChangeCheckFilter()
    {
        trackPatch.SendFloat("filterValue", filter.value);
        ambientPatch.SendFloat("filterValue2", filter.value);
    }

    void OnMouseDown () {

        MusicShift();
    }

    public void MusicShift() {
        ambientPatch.SendBang("knobOn");
        trackPatch.SendBang("knobOn");
    }
}
