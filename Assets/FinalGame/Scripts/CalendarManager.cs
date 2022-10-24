using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public GameObject calendar;
    public GameObject calEvent;

    public float scrollSpeed = 0.5f;
    
    // Dynamically loaded events
    private TextAsset    calendar_asset;
    private string[]     calendarList;
    private List<string> months       = new List<string>();
    private List<string> days         = new List<string>();
    private List<string> times        = new List<string>();
    private List<string> titles       = new List<string>();
    private List<string> descriptions = new List<string>();

    private float     yPos;
    private Transform t;
    
    
    // Start is called before the first frame update
    void Start()
    {
        yPos = 0;
        t = GameObject.Find("Event Calendar").GetComponent<Transform>();
        
        // Load the events
        StartCoroutine(GetTextFromFile());
        
    }
    
    IEnumerator GetTextFromFile()
    {
        calendar_asset = Resources.Load("calendarEvents") as TextAsset;
        calendarList = calendar_asset.text.Split('\n');

        for (int i = 0; i < calendarList.Length; i++)
        {
            string[] temp = calendarList[i].Split('\t');
            Debug.Log(temp[0]);
            months.Add(temp[0]);
            days.Add(temp[1]);
            times.Add(temp[2]);
            titles.Add(temp[3]);
            descriptions.Add(temp[4]);
            AddEvent(i);
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) yPos += scrollSpeed;
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) yPos -= scrollSpeed;
        
        calendar.transform.position = new Vector3(calendar.transform.position.x, yPos, 0);
        
    }

    public void AddEvent(int itemNumber)
    {
        // Create an event object
        GameObject e = GameObject.Instantiate(calEvent) as GameObject;
        e.transform.position = new Vector3(-4, -4 * itemNumber, 0);
        e.transform.parent = t;
        Text day = e.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        day.text = days[itemNumber];
        Text month = e.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        month.text = months[itemNumber];
        // Get the TIKATMOS versions of the info
        Text title_T = e.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        title_T.text = titles[itemNumber];
        Text description_T = e.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        description_T.text = descriptions[itemNumber];
        // Get the English versions of the info and hide them
        Text title_E = e.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        title_E.text = titles[itemNumber];
        title_E.gameObject.SetActive(false);
        Text description_E = e.transform.GetChild(1).GetChild(3).GetComponent<Text>();
        description_E.text = descriptions[itemNumber];
        description_E.gameObject.SetActive(false);
        // Set the time
        Text time = e.transform.GetChild(1).GetChild(4).GetComponent<Text>();
        time.text = times[itemNumber];
    }
    
    private void DisplayText()
    {
        

    }
}
