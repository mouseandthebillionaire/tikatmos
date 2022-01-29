using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantCareManager : MonoBehaviour
{
    public Image waterLevel;
    private float currentLevel;
    public float full = 1f;
    
    
    void Start()
    {
        currentLevel = full;
        StartCoroutine(WaterLevelDepleating());
    }

    
    void Update()
    {
        
    }

    IEnumerator WaterLevelDepleating()
    {
        // if the water level isn't zero, depleat some water after a bit
        if (currentLevel > 0)
        {
            // wait some seconds
            yield return new WaitForSeconds(5f);
            
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
            Debug.Log("water depleated");
            
            yield return new WaitForSeconds(5f);
        }
        
        // re-enter the coroutine 
        StartCoroutine(WaterLevelDepleating());
        
    }
    
}
