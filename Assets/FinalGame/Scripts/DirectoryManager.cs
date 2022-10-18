using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryManager : MonoBehaviour
{
    public Text directoryStore_display;
    public Text directoryCode_display;
    private TextAsset directory_asset;
    private string[] directoryList;
    private List<string> storeList = new List<string>();
    private List<string> codeList = new List<string>();

    private float scrollMovement;
    public GameObject textParent;
    private float textPosition;
    public float textSpeed;

    public float upperLimit, lowerLimit;

    void Start()
    {
        directoryStore_display.text = "";
        directoryCode_display.text = "";
        StartCoroutine(GetTextFromFile());
    }

    IEnumerator GetTextFromFile()
    {
        directory_asset = Resources.Load("DirectoryCSV") as TextAsset;
        directoryList = directory_asset.text.Split('\n');

        for (int i = 0; i < directoryList.Length; i++)
        {
            string[] temp = directoryList[i].Split(',');
            //Debug.Log(temp[0]+" and "+temp[1]);
            storeList.Add(temp[0]);
            codeList.Add(temp[1]);

            directoryStore_display.text += storeList[i] + '\n';
            directoryCode_display.text += codeList[i] + '\n';
        }

        yield return null;
    }
    
    void Update()
    {
        
        if(Input.GetKeyDown(GlobalVariables.S.upCrank) && (textPosition + textSpeed) < lowerLimit) {
            textPosition += textSpeed;
        }
        
        if(Input.GetKeyDown(GlobalVariables.S.downCrank) && (textPosition  - textSpeed) >= upperLimit) {
            textPosition -= textSpeed;
        }
        
        textParent.transform.localPosition = new Vector3(0, textPosition, 0);
        Debug.Log(textPosition);
    }
}
