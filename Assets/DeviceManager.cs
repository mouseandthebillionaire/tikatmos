using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour {
    public Camera deviceCamera;
    
    // Start is called before the first frame update
    void Start() {
        deviceCamera.aspect = 2;
        Display.displays[1].SetRenderingResolution(1200, 800);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
