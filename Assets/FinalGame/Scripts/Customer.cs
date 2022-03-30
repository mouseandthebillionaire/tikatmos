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
    private string[] customerList = new string[2];
    private string[] customerRequests = new string[2];
    private string[] info = new string[2];
    private string[] customerSuccessResponses = new string[2];
    private string[] customerSorries = new string[2];

    public static Customer S;

    public void Awake() {
        S = this;
    }

    public void Start() {
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
    public void InitializeCustomer() {
        currCustomer        = 0;
        Sprite s = Resources.Load ("CharacterSprites/" + currCustomer + "/0", typeof(Sprite)) as Sprite;
        customerImage.GetComponent<Image>().sprite = s;
        dialogue.GetComponentInChildren<Text>().text = customerRequests[currCustomer];

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
            
            if (value == info[currCustomer]) {
                // Success
                dialogue.GetComponentInChildren<Text>().text = customerSuccessResponses[currCustomer];
                StartCoroutine(CustomerServed());
            }
            else {
                dialogue.GetComponentInChildren<Text>().text = customerSorries[currCustomer];
            }
        }
    }
    
    private void GetCustomerFile()
    {
        TextAsset customer_file = Resources.Load("Characters") as TextAsset;
        customerList = customer_file.text.Split('\n');

        for (int i = 0; i < customerList.Length; i++)
        {
            string[] temp = customerList[i].Split('\t');
            customerRequests[i] = temp[2];
            info[i] = temp[3];
            customerSuccessResponses[i] = temp[4];
            customerSorries[i] = temp[5];
        }
    }

}
