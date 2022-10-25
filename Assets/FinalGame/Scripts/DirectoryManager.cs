using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryManager : MonoBehaviour
{
    public Text[] directoryStore_display;
    public Text[] directoryCode_display;

    private bool translated = false;

    private float scrollMovement;
    public GameObject textParent;
    private float textPosition;
    public float textSpeed;

    public float upperLimit, lowerLimit;

    void Start()
    {
        for (int i = 0; i < directoryStore_display.Length; i++)
        {
            directoryStore_display[i].text = "";
            directoryCode_display[i].text = "";
        }
        
        StartCoroutine(LoadStores());
    }

    IEnumerator LoadStores()
    {
        for (int i = 0; i < directoryStore_display.Length; i++)
        {
            for (int j = 0; j < GlobalVariables.S.StoreList.Count; j++)
            {
                directoryStore_display[i].text += GlobalVariables.S.StoreList[j] + '\n';
                directoryCode_display[i].text += GlobalVariables.S.StoreCodes[j] + '\n';
            }
        }

        yield return null;
    }
    
    void Update()
    {
        // toggle the text
        if (Input.GetKey(GlobalVariables.S.deviceButton)) translated = true;
        else translated = false;

        DisplayText();
        
        
        if(Input.GetKeyDown(GlobalVariables.S.upCrank) && (textPosition + textSpeed) < lowerLimit) {
            textPosition += textSpeed;
        }
        
        if(Input.GetKeyDown(GlobalVariables.S.downCrank) && (textPosition  - textSpeed) >= upperLimit) {
            textPosition -= textSpeed;
        }
        
        textParent.transform.localPosition = new Vector3(0, textPosition, 0);
    }

    private void DisplayText()
    {
        // Just going to go on the record that this is a GENIUS solution
        // TIKATMOS Text
        directoryStore_display[0].gameObject.SetActive(!translated);
        directoryCode_display[0].gameObject.SetActive(!translated);
        
        // Roboto Text
        directoryStore_display[1].gameObject.SetActive(translated);
        directoryCode_display[1].gameObject.SetActive(translated);
    }
}
