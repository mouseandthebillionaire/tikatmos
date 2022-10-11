using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -6) Destroy(this.gameObject);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Yes")
        {
            WishManager.S.WishGranted();
            Destroy(this.gameObject);
        }
        
        if (other.gameObject.name == "No")
        {
            WishManager.S.WishDenied();
            Destroy(this.gameObject);
        }
    }
}
