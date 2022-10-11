using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using Facebook.WitAi;
using UnityEngine;

public class WishManager : MonoBehaviour
{
    public  GameObject  coin;
    private bool        running;
    private float       upperRandRange;
    public  AudioSource yes, no;

    public static WishManager S;


    void Awake()
    {
        S = this;
        running = false;
        upperRandRange = 5f;
    }

    public void WishGranted()
    {
        yes.Play();
    }

    public void WishDenied()
    {
        no.Play();
    }

    void Update()
    {
        GameObject c = GameObject.Find("Coin(Clone)");
        if (!running || !c) StartCoroutine(CoinDrop());

        upperRandRange = 5 + (5 * Mathf.Sin(Time.time * .1f));
        Debug.Log(upperRandRange);
    }

    public IEnumerator CoinDrop()
    {
        running = true;
        float randX = Random.Range(-5, 5);
        Instantiate(coin, new Vector3(randX, 6, 0), Quaternion.identity, this.transform);
        float randTime = Random.Range(0, upperRandRange);
        Debug.Log("We will launch a new coin in " + randTime + " seconds");
        yield return new WaitForSeconds(randTime);
        running = false;
    }
}
