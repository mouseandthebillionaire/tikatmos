using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlantCareManager : MonoBehaviour
{
    [Header("Light Direction Stuff")]
    public GameObject arrow;
    public float arrowSpeed;
    private float arrowMovement;
    public GameObject sun;
    private float sunRot;
    private float sunRotGoal;
    private float anglePadding = 5f;
    public GameObject plantHeart1;
    private bool keepRotating = true;
    
    [Header("Water Level Stuff")]
    public Image waterLevel;
    private float currentLevel;
    private float full = 1f;
    public GameObject plantHeart2;
    public float waterRefillAmount;
    public float waterDrainStep;
    public float waterDrainDelay;
    private bool keepDraining = true;

    [Header("Misc.")] 
    public GameObject[] lights;
    private bool keepBlinking = true;
    
    void Start()
    {
        // set the water level to be full and start the water draining
        currentLevel = full;
        StartCoroutine(WaterLevelDrain());
        
        // set the light source to a random location and the arrow to start in the middle
        sunRot = Random.Range(-80f, 80f);
        sun.transform.Rotate(0f, 0f, sunRot);
        Debug.Log("sun start rotation = " + sunRot);
        arrow.transform.Rotate(0f, 0f, 0f);

        StartCoroutine(RotateSun());
        
        // start the plant hearts off initially
        plantHeart1.SetActive(false);
        plantHeart2.SetActive(false);
        
        // start the blinking lights off 
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        StartCoroutine(BlinkLights());
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
            currentLevel += waterRefillAmount;
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

    IEnumerator WaterLevelDrain()
    {
        if (keepDraining)
        {
            // if the water level isn't zero, drain some water after a bit
            if (currentLevel > 0)
            {
                // wait some seconds
                yield return new WaitForSeconds(waterDrainDelay);
            
                // subtract a bit from the current water level
                currentLevel -= waterDrainStep;
            
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
            
                yield return new WaitForSeconds(waterDrainDelay);
            }
        
            // re-enter the coroutine 
            StartCoroutine(WaterLevelDrain());    
        }

    }

    IEnumerator BlinkLights()
    {
        if (keepBlinking)
        {
            // turn on a random light
            int onlight = Random.Range(0, lights.Length);
            lights[onlight].SetActive(true);
            //Debug.Log("light on = " + lights[onlight]);
            
            // keep it on for a random time
            float ontime = Random.Range(0, 1f);
            yield return new WaitForSeconds(ontime);
            
            // turn off the light
            lights[onlight].SetActive(false);
            
            // wait a little bit with it off
            float offtime = Random.Range(0, 0.5f);
            yield return new WaitForSeconds(offtime);
            
            // re-enter the coroutine
            StartCoroutine(BlinkLights());
        }
    }

    IEnumerator RotateSun()
    {
         if (keepRotating)
         {
             // set a rotation for the sun to go to
             sunRotGoal = Random.Range(-80, 80);
             Debug.Log("sun goal rotation = " + sunRotGoal);
             
             bool stillRotating = true;
             
             // rotate the sun's position till it's at the destination point (sunRotGoal)
             while (stillRotating)
             {
                 // if we've made it, exit the while loop
                 if (sunRot >= sunRotGoal - anglePadding && sunRot <= sunRotGoal + anglePadding)
                 {
                     Debug.Log("we made it!");
                     stillRotating = false;
                 }
                 // if the rotation goal is > current rotation
                 else if (sunRotGoal > sunRot)
                 {
                     sun.transform.Rotate(0f, 0f, 0.2f);
                     sunRot = sun.transform.rotation.eulerAngles.z;
                     if (sunRot > 180)
                     {
                         sunRot -= 360f;
                     }
                     Debug.Log(sunRotGoal +" > "+ sunRot);
                     
                     yield return new WaitForSeconds(0.01f);
                 }
                 // if the rotation goal is < current rotation
                 else if (sunRotGoal < sunRot)
                 {
                     sun.transform.Rotate(0f, 0f, -0.2f);
                     sunRot = sun.transform.rotation.eulerAngles.z;
                     if (sunRot > 180)
                     {
                         sunRot -= 360f;
                     }
                     Debug.Log(sunRotGoal +" < "+ sunRot);
                     
                     yield return new WaitForSeconds(0.01f);
                 }
             }
             
             Debug.Log("waiting to move the sun again");
             yield return new WaitForSeconds(5f);

             // re-enter the coroutine
             StartCoroutine(RotateSun()); 
         }
    }
    
}
