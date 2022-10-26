using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MapFloor : MonoBehaviour
{
    private List<string> RandomStoreCodes;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform rscFolder = this.transform.GetChild(1);
        foreach (Transform t in rscFolder)
        {
            int r = Random.Range(0, GlobalVariables.S.StoreCodes.Count);
            t.gameObject.GetComponent<Text>().text = GlobalVariables.S.StoreCodes[r];
        }
    }
    
}
