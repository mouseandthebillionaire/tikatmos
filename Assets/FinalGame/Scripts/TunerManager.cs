using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TunerManager : MonoBehaviour
{
    // Variables for the WaveDisplays
    // One for now, but possible add more later
    public LineRenderer[] tuningDisplays;
    public int points;
    public float amplitude = 1;
    public float freq = 1;
    public float moveSpeed;

    // Circle Image
    public SpriteRenderer[] indicators;
    public Image[] indicatorDials;
    public float rAmt, gAmt; 
    
    // Text Sprites
    public SpriteRenderer activeBox;

    // Sound Control Variables
    public AudioSource voice, noise;
    public float pitch, filter, distortion, noiseAmt;
    public float[] noiseLocs = new float[3];
    public float[] noiseAmts = new float[3];
    private AudioHighPassFilter hpf;
    private AudioReverbFilter arf;
    
    // Input Variables
    private float knobVal, sliderVal, crankVal, button;
    
    // Start is called before the first frame update
    void Start() {
        hpf = voice.GetComponent<AudioHighPassFilter>();
        arf = voice.GetComponent<AudioReverbFilter>();
        hpf.cutoffFrequency = 2200;
        noiseAmt = 1;
        noise.volume = 1;
        for (int i = 0; i < noiseLocs.Length; i++) {
            noiseLocs[i] = UnityEngine.Random.Range(0, 1f);
        }
        
        // assign random number to the crank
        crankVal = UnityEngine.Random.Range(0, 1f);
    }

    // Get Inputs from Device
    public void GetInputs()
    {
        if (Input.GetKey(GlobalVariables.S.leftSlider) && (sliderVal > 0)) sliderVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.rightSlider) && (sliderVal < 1)) sliderVal += .001f;

        if (Input.GetKey(GlobalVariables.S.upCrank) && (crankVal > 0)) crankVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.downCrank) && (crankVal < 1)) crankVal += .001f;
        
        if (Input.GetKey(GlobalVariables.S.knobLeft) && (knobVal > 0)) knobVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.knobRight) && (knobVal < 1)) knobVal += .001f;

    }
    
    // Update is called once per frame
    void DrawSine()
    {
        float xStart = 0;
        float Tau = 2 * Mathf.PI;
        float xFinish = Tau;

        for (int i = 0; i < tuningDisplays.Length; i++) {
            tuningDisplays[i].positionCount = points;
            for (int j = 0; j < points; j++) {
                float progress = (float) j / (points - 1);
                float x        = Mathf.Lerp(xStart, xFinish, progress);
                float y        = amplitude * Mathf.Sin((Tau * freq * x * (i + 1)) + Time.time * moveSpeed );
                tuningDisplays[i].SetPosition(j, new Vector3(x, y, 0));
            }
        }
    }

    void Update()
    {
        GetInputs();
        
        // Update SineWaves
        amplitude = ((sliderVal * .2f) + (crankVal * .3f) + (knobVal * .5f)) * 3f;
        freq = ((sliderVal * .3f) + (crankVal * .5f) + (knobVal * .2f)) * 2f;
        moveSpeed = 1 + (20 * ((sliderVal * .5f) + (crankVal * .2f) + (knobVal * .3f)));
        
        // Update Noise Amounts Via Input Positions
        noiseAmts[0] = Mathf.Abs(sliderVal - noiseLocs[0]);
        noiseAmts[1] = Mathf.Abs(crankVal - noiseLocs[1]);
        noiseAmts[2] = Mathf.Abs(knobVal - noiseLocs[2]);

        // Update Sounds
        hpf.cutoffFrequency = noiseAmts[0] * 2200;
        voice.pitch = 1 + (noiseAmts[1] * 3f);
        arf.dryLevel = 0 - (noiseAmts[2] * 10000);
        arf.reverbLevel = 0 + (noiseAmts[2] * 500);
        arf.room = -10000 + (noiseAmts[2] * 10000);

        noiseAmt = (noiseAmts[0] + noiseAmts[1] + noiseAmts[2]);
        noise.volume = noiseAmt;
        
        // Update the Indicators
        for (int i = 0; i < indicators.Length; i++) {
            indicators[i].color = new Color(1 - noiseAmts[i], 1 - noiseAmts[i], 01 - noiseAmts[i]);
            indicatorDials[i].fillAmount = 1 - noiseAmts[i];
        }

        DrawSine();

        // Text Boxes
        if (noiseAmt < 0.2f) {
            // We are tuned
            activeBox.transform.position = new Vector3(0, 0, 0);
            // Press the button to make it official?
            if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) GlobalVariables.S.tuned = true;
            // Do we want to do any kind of visual change to show that we are tuned? 

        }
        else {
            if (Input.anyKey) activeBox.transform.position = new Vector3(-5, 0, 0);
            else activeBox.transform.position = new Vector3(-10, 0, 0);
        } 

    }
}
