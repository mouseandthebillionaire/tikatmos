using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeviceManager : MonoBehaviour
{
    // NOTE: the device manager doesn't have a camera since all of the subscenes have their own cameras
    
    public int currApp;
    // Scene Names for the Device Apps
    public string[] appNames;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currApp = 0;
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(appNames[currApp]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currApp = (currApp + 1) % appNames.Length;
            SceneManager.LoadScene(appNames[currApp]);
        }
    }
}
