using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BallScript : MonoBehaviour {

    public float               ballSpeed = 2f;

    private Rigidbody2D        rb;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(GlobalVariables.S.deviceButton)) StartCoroutine(Launch());
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "bounds") {
            StartCoroutine("Launch");
        }
    }

    public IEnumerator Launch() {
        this.transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        rb.AddForce(transform.right * -1);
        rb.AddForce(transform.up);

        yield return null;


    }
}
