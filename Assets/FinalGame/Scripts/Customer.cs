using System.Collections;
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
    private int currCustomer;
    private string[] customerScript = new string[6];
    private string[] customerRequests = new string[6];
    private string[] responseNeeded = new string[6];
    private string[] info = new string[6];
    private string[] customerSuccessResponses = new string[6];
    private string[] customerSorries = new string[6];

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

    public void Reset() {
        GlobalVariables.S.customerActive = false;
        customerNotification.SetActive(false);
        customerImage.SetActive(false);
        dialogue.SetActive(false);
    }

    // Load the customer to be served and display the tuning dialogue
    public void InitializeCustomer()
    {
        //currCustomer = Random.Range(0, customerScript.Length);
        currCustomer = 3;
        Sprite s = Resources.Load ("CharacterSprites/" + currCustomer + "/0", typeof(Sprite)) as Sprite;
        customerImage.GetComponent<Image>().sprite = s;
        dialogue.GetComponentInChildren<Text>().text = customerRequests[currCustomer];
        
        // Reset the Vocal Tuner
        //Tuner.S.Reset();
        
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

    private IEnumerator CustomerServed() {
        yield return new WaitForSeconds(2);
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
                    dialogue.GetComponentInChildren<Text>().text = customerSuccessResponses[currCustomer];
                    StartCoroutine(CustomerServed());
                }
                else
                {
                    dialogue.GetComponentInChildren<Text>().text = customerSorries[currCustomer];
                }
            }
        }
    }
    
    private void GetCustomerFile()
    {
        TextAsset customer_file = Resources.Load("Characters") as TextAsset;
        customerScript = customer_file.text.Split('\n');
        Debug.Log(customerScript[3]);

        for (int i = 0; i < customerScript.Length; i++)
        {
            string[] temp = customerScript[i].Split('\t');
            customerRequests[i] = temp[2];
            responseNeeded[i] = temp[3];
            info[i] = temp[4];
            customerSuccessResponses[i] = temp[5];
            customerSorries[i] = temp[6];
        }
    }

}
