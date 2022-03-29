using System.Collections;
using System.Text.RegularExpressions;
using Facebook.WitAi;
using Facebook.WitAi.Lib;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] public string value;

    public Text dialogue;
    
    public void ProcessInformation(WitResponseNode response)
    {
        string _tempValue = WitResultUtilities.GetFirstEntityValue(response, "wit_store_code:wit_store_code");
        value = Regex.Replace(_tempValue, "[^\\w\\._] ", "");
        
        Debug.Log(value);
        
        if (value == "five") dialogue.text = "Oh, really? That doesn't sound right?";
        if (value == "77queue" || 
            value =="seventy-sevenQ" || 
            value =="seven seven Q" ||
            value =="seven seven queue")
        {
            Debug.Log("Bang");
            dialogue.text = "Oh. Perfect! Thanks!";
        }
    }
    
}
