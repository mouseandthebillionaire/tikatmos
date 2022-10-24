using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Facebook.WitAi;
using Facebook.WitAi.Lib;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] public string value;

    public GameObject customerNotification, customerImage, dialogue;
    
    // Customer stufffffff
    private int          currCustomer;
    private string[]     customerScript           = new string[10];
    public  Sprite[]     customerSprites          = new Sprite[10];
    private string[]     customerRequests         = new string[10];
    private string[]     responseNeeded           = new string[10];
    private string[]     info                     = new string[10];
    private string[]     customerSuccessResponses = new string[10];
    private List<string> customerSorries          = new List<string>();
    private int          currentSorry; // to keep track and loop current sorries,
                                       // alternatively w could have the conversation end if you run through all of the sorries
                                       // ex: "Um, okay, I guess I'll come back later?"

    public KeyCode           scriptAdvance;
                                       
    public static Customer S;

    public void Awake() {
        S = this;
    }

    public void Start()
    {
        currCustomer = 0;
        GetCustomerFile();
        Reset();
    }

    public void Update() {
        if (Input.GetKeyDown(GlobalVariables.S.customerServedCorrectly)) CorrectInformation();
        if (Input.GetKeyDown(GlobalVariables.S.loadNextCustomer)) InitializeCustomer();
    }

    public void CompleteReset() {
        Reset();
        currCustomer = 0;
    }

    public void Reset() {
        GlobalVariables.S.customerActive = false;
        customerNotification.SetActive(false);
        customerImage.SetActive(false);
        dialogue.SetActive(false);
        TunerManager.S.ResetTuning();
    }

    // Load the customer to be served and display the tuning dialogue
    public void InitializeCustomer() {
        customerImage.GetComponent<Image>().sprite = customerSprites[currCustomer];
        dialogue.GetComponentInChildren<Text>().text = customerRequests[currCustomer];

        // Take it Away!
        StartCoroutine(StartCustomer());
    }

    public IEnumerator StartCustomer() {
        
        GlobalVariables.S.customerActive = true;
        
        // show the Customer
        customerImage.SetActive(true);
        
        // show the Customer Notification
        customerNotification.SetActive(true);
        
        // wait for the tuner to be tuned
        while (!GlobalVariables.S.tuned)
        {
            yield return null;
        }
        // And once it has been tuned, present the customer's request
        Request();

        yield return null;
    }
    
    public void Request() {
        customerNotification.SetActive(false);
        dialogue.SetActive(true);
    }

    private void CorrectInformation() {
        dialogue.GetComponentInChildren<Text>().text = customerSuccessResponses[currCustomer];
        StartCoroutine(CustomerServed());
    }

    private IEnumerator CustomerServed() {
        yield return new WaitForSeconds(2);
        currCustomer = (currCustomer + 1) % customerScript.Length;
        Reset();
    }
    
    // Function called from the Wit object
    public void ProcessInformation(WitResponseNode response)
    {
        if (GlobalVariables.S.tuned) {
            string _tempValue = WitResultUtilities.GetFirstEntityValue(response, "wit_store_code:wit_store_code");
            value = Regex.Replace(_tempValue, @"[^\w\s]", "");

            Debug.Log(value);

            if (responseNeeded[currCustomer] == "FALSE")
            {
                dialogue.GetComponentInChildren<Text>().text = customerSuccessResponses[currCustomer];
                StartCoroutine(CustomerServed());
            }
            else
            {

                if (value == info[currCustomer])
                {
                    // Success
                    CorrectInformation();
                }
                else
                {
                    dialogue.GetComponentInChildren<Text>().text = customerSorries[currentSorry];
                    currentSorry++;
                }
            }
        }
    }
    
    private void GetCustomerFile()
    {
        // Delete sorries and reset the counter
        customerSorries.Clear();
        currentSorry = 0;
        
        TextAsset customer_file = Resources.Load("Characters") as TextAsset;
        customerScript = customer_file.text.Split('\n');

        for (int i = 0; i < customerScript.Length; i++)
        {
            string[] temp = customerScript[i].Split('\t');
            customerSprites[i] = Resources.Load ("CharacterSprites/" + temp[0] + "/0", typeof(Sprite)) as Sprite;
            customerRequests[i] = temp[2];
            responseNeeded[i] = temp[3];
            info[i] = temp[4];
            customerSuccessResponses[i] = temp[5];
            // Get Array of Sorries
            string[] tempSorries = temp[6].Split('/');
            for (int j = 0; j < tempSorries.Length; j++)
            {
                customerSorries.Add(tempSorries[j % tempSorries.Length]);
            }
        }
    }

}
