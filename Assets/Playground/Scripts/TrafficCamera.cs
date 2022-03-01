using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficCamera : MonoBehaviour
{
    public float cameraSpeed;
    public float lerpSpeed;
    public float[] cameraBounds = new float[2];

    public Image systemPower, customerHappiness;

    private Vector3 systemStart, customerStart;

    // Start is called before the first frame update
    void Start()
    {
        systemStart = systemPower.transform.position - transform.position;
        systemStart.z = 100;
        customerStart =  customerHappiness.transform.position - transform.position;
        customerStart.z = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.position.y;

        if (SerialScript.S.crankUp && yPos + cameraSpeed <= cameraBounds[1]) yPos += cameraSpeed;
        if (SerialScript.S.crankDown && yPos - cameraSpeed >= cameraBounds[0]) yPos -= cameraSpeed;

        // Change the position of the camera
        Vector3 targetPosition = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);

        // Reposition the system power and customer happiness indicators
        systemPower.transform.position = transform.position + systemStart;
        systemPower.transform.SetAsLastSibling();
        customerHappiness.transform.position = transform.position + customerStart;
        customerHappiness.transform.SetAsLastSibling();

        UpdateUI();
    }

    void UpdateUI() {
        // Display the total number of happy customers
        int maxPeople = FloorManager.maxPeopleOnFloor;
        float threshold = FloorManager.floorThreshold;
        int happyFloors = 0;
        for (int i = 0; i < FloorManager.floors.Count; i++) {
            if (FloorManager.peopleOnFloors[i] <= maxPeople - threshold && FloorManager.peopleOnFloors[i] >= threshold) happyFloors++;
        }
        customerHappiness.transform.GetChild(0).GetComponentInChildren<Image>().fillAmount = (float) happyFloors/FloorManager.floors.Count;

        // Display the total number of active escalators
        systemPower.transform.GetChild(0).GetComponentInChildren<Image>().fillAmount = FloorManager.percentActivated;
    }
}
