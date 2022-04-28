using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Tuner : MonoBehaviour {
    public AudioSource voice;
    public float pitch, filter, distortion, noiseAmt;
    public float[] noiseLocs = new float[3];
    public float[] noiseAmts = new float[3];
    private AudioHighPassFilter hpf;
    private AudioReverbFilter arf;
    public AudioSource noise;

    public Slider[] sliders;

	public static Tuner S;

	void Awake(){
		if (S == null) {
            S = this;
        } else {
            DestroyObject(gameObject);
        }
	}

	public void Reset(){
        hpf.cutoffFrequency = 2200;
        noiseAmt = 1;
        noise.volume = 1;
        for (int i = 0; i < noiseLocs.Length; i++) {
            noiseLocs[i] = UnityEngine.Random.Range(0, 1f);
            sliders[i].value = UnityEngine.Random.Range(0, 1f);
        }
	} 

    // Start is called before the first frame update
    void Start() {
        hpf = GetComponent<AudioHighPassFilter>();
        arf = GetComponent<AudioReverbFilter>();
        Reset();
    }

    // Update is called once per frame
    void Update() {
        // hpf.cutoffFrequency = fSlider.value;
        // voice.pitch = pSlider.value;
        // noise.volume = dSlider.value;

        for (int i = 0; i < noiseAmts.Length; i++) {
            noiseAmts[i] = Mathf.Abs(sliders[i].value - noiseLocs[i]);
            noiseAmt -= noiseAmts[i];
        }

        hpf.cutoffFrequency = noiseAmts[0] * 2200;
        voice.pitch = 1 + (noiseAmts[1] * 3f);
        arf.dryLevel = 0 - (noiseAmts[2] * 10000);
        arf.reverbLevel = 0 + (noiseAmts[2] * 500);
        arf.room = -10000 + (noiseAmts[2] * 10000);

        noiseAmt = (noiseAmts[0] + noiseAmts[1] + noiseAmts[2]);
        noise.volume = noiseAmt;
    }
}
