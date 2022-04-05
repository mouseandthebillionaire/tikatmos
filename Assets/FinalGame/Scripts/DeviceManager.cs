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
    
    // Load App GAMEOBJECTS rather than scenes
    public GameObject[] apps;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currApp = 2;
        // DontDestroyOnLoad(this);
        // SceneManager.LoadScene(appNames[currApp]);
        
        LoadNextApp();

        // Activate the default app
        apps[currApp].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currApp = (currApp + 1) % apps.Length;
            LoadNextApp();
            //SceneManager.LoadScene(appNames[currApp]);
        }
    }

    public void LoadNextApp() {
        for (int i = 0; i < apps.Length; i++) {
            if(i==currApp) apps[i].SetActive(true);
            else apps[i].SetActive(false);
        }
    }
}
