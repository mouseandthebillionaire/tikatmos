using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficTargets : MonoBehaviour
{
    public GameObject person;
    public GameObject target;

    public GameObject[] targetZones;

    private Rigidbody2D rb;

    public float xBounds, yBounds;
    public float maxVelocity;
    public float minTargetTime, maxTargetTime;
    public float targetDistance;

    public float minZoneTime, maxZoneTime;
    public float zonePadding;

    private float time = 0.0f;

    private float zoneTime = 0.0f;
    private float zoneTimer;

    private int currentZone;

    // Start is called before the first frame update
    void Start()
    {
        rb = person.gameObject.GetComponent<Rigidbody2D>();

        // Choose a random location for the person
        StartCoroutine(RandomSpawn());

        // Choose a random target zone
        currentZone = Random.Range(0, targetZones.Length);

        // Choose a random location for the target
        StartCoroutine(RandomLocation());

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

        VelocityToTarget();

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

        // Stop the person when they've reached their target
        //if (hypotenuse <= targetDistance) rb.velocity = new Vector3(0, 0, 0);
    }

    void VelocityToTarget() {

        // Update time counter
        time += Time.fixedDeltaTime;

        float targetTime = Random.Range(minTargetTime,maxTargetTime);

        // Determine if a new target zone needs to be chosen
        NewZone();

        // Determine if a new target location needs to be chosen
        if (time >= targetTime) {
            StartCoroutine(RandomLocation());
            time = 0;
        }

        // Change the velocity of the person to aim for the target
        float xDistance = target.transform.position.x - person.transform.position.x;
        float yDistance = target.transform.position.y - person.transform.position.y;

        Vector2 newVelocity = new Vector2(xDistance, yDistance);

        rb.AddForce(newVelocity);
    }

    void NewZone() {
        // Determine how long before a new zone is chosen
        if (zoneTime == 0) zoneTimer = Random.Range(minZoneTime, maxZoneTime);

        // Update time counter
        zoneTime += Time.fixedDeltaTime;

        if (zoneTime >= zoneTimer) {
            // Generate a new random target zone
            currentZone = Random.Range(0, targetZones.Length);
            zoneTime = 0;
        }
    }

    IEnumerator RandomLocation() {
        yield return new WaitForSeconds(0);

        GameObject zone = targetZones[currentZone];

        // Determine a random location within the chosen target zone
        float xExtent = zone.GetComponent<SpriteRenderer>().bounds.extents.x;
        float yExtent = zone.GetComponent<SpriteRenderer>().bounds.extents.y;

        float xMin = zone.transform.position.x - xExtent + zonePadding;
        float xMax = zone.transform.position.x + xExtent - zonePadding;
        float yMin = zone.transform.position.y - yExtent + zonePadding;
        float yMax = zone.transform.position.y + yExtent - zonePadding;

        float xTarget = Random.Range(xMin, xMax);
        float yTarget = Random.Range(yMin,yMax);

        // Check to see if the target will collide with something else
        if (CheckOverlap(target, xTarget, yTarget)) StartCoroutine(RandomLocation());
        else {
            // Move the target to that location
            Vector3 targetLocation = new Vector3(xTarget, yTarget, 0);
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