using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuoteManager : MonoBehaviour {
    public Text quote;
    private TextAsset quote_asset;
    private string[] quoteList;

    public GameObject bg;
    private float r, g, b;
    
    // Start is called before the first frame update
    void Start()
    {
        quote.text = "Anyone who stops learning is old, whether at twenty or eighty. Anyone who keeps learning stays young. The greatest thing in life is to keep your mind young.";
        StartCoroutine(GetTextFromFile());
    }
    
    IEnumerator GetTextFromFile()
    {
        quote_asset = Resources.Load("quotes") as TextAsset;
        quoteList = quote_asset.text.Split('\n');

        // for (int i = 0; i < directoryList.Length; i++)
        // {
        //     string[] temp = directoryList[i].Split(',');
        //     //Debug.Log(temp[0]+" and "+temp[1]);
        //     storeList.Add(temp[0]);
        //     codeList.Add(temp[1]);
        //
        //     directoryStore_display.text += storeList[i] + '\n';
        //     directoryCode_display.text += codeList[i] + '\n';
        // }

        yield return null;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) ChangeQuote();

        if (Input.GetKeyDown(GlobalVariables.S.knob0_left) && r > 0) r -= .01f;
        if (Input.GetKeyDown(GlobalVariables.S.knob0_right) && r < 1) r += .01f;
        if (Input.GetKeyDown(GlobalVariables.S.knob1_left) && g > 0) g -= .01f;
        if (Input.GetKeyDown(GlobalVariables.S.knob1_right) && g < 1) g += .01f;
        if (Input.GetKeyDown(GlobalVariables.S.upCrank) && b > 0) b -= .01f;
        if (Input.GetKeyDown(GlobalVariables.S.downCrank) && b < 1) b += .01f;

        bg.GetComponent<SpriteRenderer>().color = new Color(r, g, b, 0.6f);
    }

    private void ChangeQuote() {
        int rand = Random.Range(0, quoteList.Length);
        quote.text = quoteList[rand];
        r = Random.Range(0, 1f);
        g = Random.Range(0, 1f);
        b = Random.Range(0, 1f);
        int    rand_Cat = Random.Range(0, 1705);
        Sprite s    = Resources.Load ("Cats/ci_" +  rand_Cat, typeof(Sprite)) as Sprite;
        bg.GetComponent<SpriteRenderer>().sprite = s;
    }
}
