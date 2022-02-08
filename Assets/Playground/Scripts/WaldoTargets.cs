using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaldoTargets : MonoBehaviour
{
    public GameObject person;
    public GameObject target;
    public GameObject path;

    private Rigidbody2D rb;

    public float xBounds, yBounds;
    public float minVelocity, maxVelocity;
    public float pathDistance;
    public float pathRadius;
    public float minTargetTime, maxTargetTime;

    private float time = 0.0f;

    private float previousTheta;

    private bool stopped;

    // Start is called before the first frame update
    void Start()
    {
        rb = person.gameObject.GetComponent<Rigidbody2D>();

        // Choose a random location for the person
        StartCoroutine(RandomSpawn());

        // Choose a random location for the target
        target.transform.position = new Vector3(Random.Range(xBounds, -xBounds), Random.Range(yBounds, -yBounds), 0);

        // Choose a random velocity for the person
        rb.velocity = new Vector3(Random.Range(-maxVelocity, maxVelocity), Random.Range(-maxVelocity, maxVelocity), 0);
    }

    

    // Choose a new random location for the person to spawn in
    IEnumerator RandomSpawn() {
        yield return new WaitForSeconds(0);

        // Determine a random spawn location
        float newX = Random.Range(xBounds, -xBounds);
        float newY = Random.Range(yBounds, -yBounds);

        // Check to see if the person will collide with something else
        if (CheckOverlap(person, newX, newY)) StartCoroutine(RandomSpawn());
        else {
            // Move the person to that new location
            person.transform.position = new Vector3(newX, newY, 0);

            // Turn on that person's sprite
            person.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        // Calculate the distance between the person and the target
        float xDistance = target.transform.position.x - person.transform.position.x;
        float yDistance = target.transform.position.y - person.transform.position.y;
        float hypotenuse = Mathf.Sqrt(xDistance*xDistance + yDistance*yDistance);

        VelocityToTarget(xDistance, yDistance, hypotenuse);

        // Set a maximum velocity for the person
        float xVelocity = rb.velocity.x;
        float yVelocity = rb.velocity.y;

        // Max X velocity
        if (rb.velocity.x > maxVelocity) xVelocity = maxVelocity;
        if (rb.velocity.x < -maxVelocity) xVelocity = -maxVelocity;

        // Max Y velocity
        if (rb.velocity.y > maxVelocity) yVelocity = maxVelocity;
        if (rb.velocity.y < -maxVelocity) yVelocity = -maxVelocity;

        rb.velocity = new Vector3(xVelocity, yVelocity, 0);

        // Stop the person for a while if their velocity is slow enough
        if (Mathf.Abs(xVelocity) < minVelocity && Mathf.Abs(yVelocity) < minVelocity) stopped = true;
        if (time == 0) stopped = false;

        if (stopped) rb.velocity = new Vector3(0, 0, 0);
    }

    void VelocityToTarget(float xDistance, float yDistance, float hypotenuse) {
    
        // A prediction of where the person will move to based on its current velocity
        float scaleFactor = pathDistance/hypotenuse;

        float xPrediction = person.transform.position.x + xDistance*scaleFactor;
        float yPrediction = person.transform.position.y + yDistance*scaleFactor;

        // Display the "path" object at the predicted location
        Vector3 predictedLocation = new Vector3(xPrediction, yPrediction, 0);
        path.transform.position = predictedLocation;

        // Update time counter
        time = time + Time.fixedDeltaTime;

        float targetTime = Random.Range(minTargetTime,maxTargetTime);

        // Determine if a new target location needs to be chosen
        if (time >= targetTime) {
            StartCoroutine(RandomLocation(xPrediction, yPrediction));
            time = 0;
        }

        // Change the velocity of the person to aim for the target
        xDistance = target.transform.position.x - person.transform.position.x;
        yDistance = target.transform.position.y - person.transform.position.y;

        Vector2 newVelocity = new Vector2(xDistance, yDistance);

        rb.AddForce(newVelocity);
    }

    IEnumerator RandomLocation(float xPrediction, float yPrediction) {
        float xTarget, yTarget;
        yield return new WaitForSeconds(0);

        // Determine a random location that is a fixed radius away from the "path" object
        float theta = Random.Range(previousTheta - Mathf.PI/4, previousTheta + Mathf.PI/4);
        previousTheta = theta;

        xTarget = Mathf.Cos(theta) * pathRadius;
        yTarget = Mathf.Sin(theta) * pathRadius;

        // Check to see if the target will collide with something else
        if (CheckOverlap(target, xPrediction + xTarget, yPrediction + yTarget)) StartCoroutine(RandomLocation(xPrediction, yPrediction));
        else {
            // Move the target to that new location
            Vector3 targetLocation = new Vector3(xPrediction + xTarget, yPrediction + yTarget, 0);
            target.transform.position = targetLocation;
        }
    }

    // Check to see if any targets are overlapping with terrain
    public bool CheckOverlap(GameObject target, float x, float y)
    {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(new Vector2(x,y),target.GetComponent<SpriteRenderer>().bounds.extents.x/2);
        if (colliders.Length >= 1) return true;
        else return false;
    }
}
