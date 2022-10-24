using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("in DevMode we don't autoload the DeviceManager")]
    public bool devMode;

    // Start is called before the first frame update
    void Start()
    {
        // Activate All Screens
        Debug.Log ("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1
        // Check if additional displays are available and activate each.
        for (int i = 1; i < Display.displays.Length; i++) {
            Display.displays[i].Activate();
        }
        
        // Make this Screen Permanent
        DontDestroyOnLoad(this);

        if (!devMode)
        {
            // Load the Device Manager as a secondary scene (to force on second display)
            SceneManager.LoadScene("DeviceManager", LoadSceneMode.Additive);
        }

        GlobalVariables.S.customerActive = false;
        
        // Start the Game
        StartCoroutine(GameLoop());
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Reset();
    }

    private void Reset() {
        Customer.S.CompleteReset();
    }

    private IEnumerator GameLoop() {
       
        // not currently helping anyone
        if (!GlobalVariables.S.customerActive)
        {
            // wait between one and three minutes
            // float waitTime = Random.Range(60, 180);
            // Quicker for testing
            float waitTime = Random.Range(120, 180);
            yield return new WaitForSeconds(waitTime);

            // Reset the tuner
            GlobalVariables.S.tuned = false;
            
            // And start the customer interaction
            Customer.S.InitializeCustomer();
            StartCoroutine(GameLoop());
        }
        else {
            // We're dealing with a customer. Check back every 30 seconds to see if they've left
            yield return new WaitForSeconds(30);
            StartCoroutine(GameLoop());
        }
        
    }
}
