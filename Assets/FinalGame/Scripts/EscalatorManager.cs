using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscalatorManager : MonoBehaviour
{
    private float yPos;
    public  float scrollSpeed = .01f;

    public  List<GameObject> escalators;
    private int              escalatorSelected;
    private int              previousSelected = 0;

    public GameObject floor;
    public Color[]    floorColors;
    
    // UI
    public GameObject[] statusUIs;
    public Color[]      statusColors; // 0 = good, 1=fine, 2=bad


    public static EscalatorManager S;

    void Awake()
    {
        S = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Generate the Floors
        int numFloors = 8;
        for (int i = 0; i < numFloors; i++)
        {
            float yLoc = -3 + (3 * i);
            Vector3 loc = new Vector3(0, yLoc, 0);
            GameObject go = Instantiate(floor, loc, Quaternion.identity, this.transform);
            go.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = floorColors[i];
            Color dimmer = new Color(floorColors[i].r, floorColors[i].g, floorColors[i].b, .75f);
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = dimmer;
            go.GetComponent<EscalatorFloor>().floorNum = i;
            string floorName = "";
            if (i == 0) floorName = "Garage";
            else floorName = "Level " + i;
            go.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = floorName;

        }
        SwitchEscalator(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GlobalVariables.S.upCrank) && yPos < 0) yPos += scrollSpeed;
        if (Input.GetKeyDown(GlobalVariables.S.downCrank) && yPos > -14.4) yPos -= scrollSpeed;

        Debug.Log(yPos);
        this.transform.position = new Vector3(0, yPos, 0);

        if (Input.GetKeyDown(GlobalVariables.S.downCrank) && escalatorSelected < escalators.Count -1) SwitchEscalator(escalatorSelected += 1);
        if (Input.GetKeyDown(GlobalVariables.S.upCrank) && escalatorSelected > 0) SwitchEscalator(escalatorSelected -= 1);
    }

    private void SwitchEscalator(int escalatorNum)
    {
        escalators[previousSelected].GetComponent<Escalator>().MakeInactive();
        escalatorSelected = escalatorNum;
        escalators[escalatorSelected].GetComponent<Escalator>().MakeActive();
        previousSelected = escalatorSelected;
    }
}
