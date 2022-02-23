using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool customer;
    
    [Header("in DevMode we don't autoload the DeviceManager")]
    public bool devMode;
    
    // Screen UI Elements
    public GameObject customerNotification, customerRequest;
    
    
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

        customer = false;
        customerNotification.SetActive(false);
        
        // Start the Game
        StartCoroutine(GameLoop());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GameLoop()
    {
        // not currently helping anyone
        if (!customer)
        {
            // wait between one and three minutes
            // float waitTime = Random.Range(60, 180);
            // Quicker for testing
            float waitTime = Random.Range(6, 18);
            yield return new WaitForSeconds(waitTime);

            // Start the customer interaction
            StartCoroutine(Customer());
        }
    }

    private IEnumerator Customer()
    {
        customer = true;
        // display that a customer is present
        customerNotification.SetActive(true);
        
        // wait for the tuner to be tuned
        while (!GlobalVariables.S.tuned)
        {
            yield return null;
        }
        // Present the customer's request
        Debug.Log("I lost my pet catalog");

        yield return null;
    }

    public void NewCustomer() {
        GlobalVariables.S.tuned = false;
    }
}
