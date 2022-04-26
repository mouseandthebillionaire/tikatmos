using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource channelChange;

    public static AudioManager S;
    
    // Start is called before the first frame update
    void Start() {
        S = this;
    }

    public void ChangeChannel() {
        channelChange.Play();
    }
    
}
