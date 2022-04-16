using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.WitAi;

public class WitActivation : MonoBehaviour
{
    [SerializeField] private Wit wit;

    private void OnValidate()
    {
        if(!wit) wit = GetComponent<Wit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Listening...");
            wit.Activate();
        }
    }
}
