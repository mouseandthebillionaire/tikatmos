using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantCareManager : MonoBehaviour
{
    [Header("Light Direction Stuff")]
    public GameObject arrow;
    public float arrowSpeed;
    private float arrowMovement;
    public GameObject sun;
    private float sunRot;
    private float anglePadding = 5f;
    public GameObject plantHeart1;
    
    [Header("Water Level Stuff")]
    public Image waterLevel;
    private float currentLevel;
    private float full = 1f;
    private float waterDelay = 2f;
    public GameObject plantHeart2;
    
    
    void Start()
    {
        // set the water level to be full and start the water draining
        currentLevel = full;
        StartCoroutine(WaterLevelDepleating());
        
        // set the light source to a random location and the arrow to start in the middle
        sunRot = Random.Range(-80f, 80f);
        sun.transform.Rotate(0f, 0f, sunRot);
        Debug.Log("sun start rotation = " + sunRot);
        arrow.transform.Rotate(0f, 0f, 0f);
        
        // start the plant hearts off initially
        plantHeart1.SetActive(false);
        plantHeart2.SetActive(false);
    }

    
    void Update()
    {
        // update the arrows direction based on player input
        arrowMovement = Input.GetAxisRaw("Horizontal")*-1;
        arrow.transform.Rotate(0, 0, arrowMovement*arrowSpeed*Time.deltaTime);
        float arrowRotation = arrow.transform.rotation.eulerAngles.z;
        if (arrowRotation > 180)
        {
            arrowRotation -= 360f;
        }
        //Debug.Log("arrow rotation = "+arrowRotation);
        
        // if the arrow lines up with the sun, turn on a plant heart
        if (arrowRotation >= sunRot - anglePadding && arrowRotation <= sunRot + anglePadding)
        {
            plantHeart1.SetActive(true);
            Debug.Log("Plant lined up to light!");
        }
        else
        {
            plantHeart1.SetActive(false);
        }
        
        // add water when the player presses a button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Add water");
            currentLevel += 0.25f;
            waterLevel.fillAmount = currentLevel;
            Debug.Log("current water level = " + currentLevel);
        }

        // if the water is full, turn on a plant heart
        if (currentLevel > 1)
        {
            currentLevel = 1;
            plantHeart2.SetActive(true);
        }
        else if (currentLevel > 0.75f)
        {
            plantHeart2.SetActive(true);
        }
        else
        {
            plantHeart2.SetActive(false);
        }
        

    }

    IEnumerator WaterLevelDepleating()
    {
        // if the water level isn't zero, depleat some water after a bit
        if (currentLevel > 0)
        {
            // wait some seconds
            yield return new WaitForSeconds(waterDelay);
            
            // subtract a bit from the current water level
            currentLevel -= 0.0625f;
            
            // subtract a bit from the water interface
            waterLevel.fillAmount = currentLevel;

            Debug.Log("current water level = " + currentLevel);

            if (currentLevel < 0)
            {
                currentLevel = 0;
            }
        }
        
        // if the water level is at zero, keep waiting
        else
        {
            Debug.Log("water depleted");
            
            yield return new WaitForSeconds(waterDelay);
        }
        
        // re-enter the coroutine 
        StartCoroutine(WaterLevelDepleating());
        
    }
    
}
