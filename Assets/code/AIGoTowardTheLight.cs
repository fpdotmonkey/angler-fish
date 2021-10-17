using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Pick a position on the left of the screen and swim to the right.
/// If the fish is to the left of the center of the light, then try to
/// swim toward it.  If the fish is in the light, swim back and forth
/// within the light.
public class AIGoTowardTheLight : MonoBehaviour
{
    [SerializeField]
    private float minSpeed = 0.5f;
    [SerializeField]
    private float maxSpeed = 1.5f;
    [SerializeField]
    private float verticalApproachFactor = 0.01f;
    [SerializeField]
    private float threatRange = 2.0f;
    [SerializeField]
    private GameObject lamp;
    [SerializeField]
    private GameObject anglerFish;

    private Vector2 velocity;
    private float minX;
    private float maxX;
    private bool isInTheLight = false;
    private bool hasBeenThreatened = false;

    private Vector2 InitialPosition()
    {
        // This all assumes a static camera with the origin in the center
        float maxY = Camera.main.orthographicSize;
        minX = -Camera.main.aspect * maxY;
        maxX = -minX;

        return new Vector2(minX - 2.0f, Random.Range(-maxY, maxY));
    }

    private Vector2 TowardTheLightTrajectory(Vector2 currentPosition)
    {
        if (!isInTheLight && transform.position.x > lamp.transform.position.x)
            return currentPosition + velocity * Time.fixedDeltaTime;
        Vector2 distanceToLight = (Vector2) lamp.transform.position - currentPosition;
        float averageSpeed = (maxSpeed + minSpeed) / 2.0f;

        Vector2 horizontalDisplacement = velocity * Time.fixedDeltaTime;
        Vector2 verticalDisplacement = averageSpeed * Vector2.up * verticalApproachFactor * distanceToLight.y;
        if (isInTheLight)
            horizontalDisplacement = verticalApproachFactor * distanceToLight.x * Vector2.right;

        return currentPosition + horizontalDisplacement + verticalDisplacement;
    }

    private Vector2 RunAwayTrajectory(Vector2 currentPosition)
    {
        return currentPosition + maxSpeed * Vector2.left * Time.fixedDeltaTime;
    }

    private bool AnglerFishIsInThreatRange(Vector2 currentPosition)
    {
        return ((Vector2) anglerFish.transform.position - currentPosition).magnitude < threatRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("lamp"))
            isInTheLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("lamp"))
            isInTheLight = false;
    }

    private void Start()
    {
        if (lamp == null)
            throw new System.Exception($"`{gameObject.name}.AIGoTowardTheLight.lamp` is not defined");
        if (anglerFish == null)
            throw new System.Exception($"`{gameObject.name}.AIGoTowardTheLight.anglerFish` is not defined");
        transform.position = InitialPosition();
        velocity = Random.Range(minSpeed, maxSpeed) * Vector2.right;
    }

    private void FixedUpdate()
    {
        if (!AnglerFishIsInThreatRange(transform.position) && !hasBeenThreatened)
        {
            transform.position = TowardTheLightTrajectory(transform.position);
        }
        else
        {
            transform.position = RunAwayTrajectory(transform.position);
            if (!hasBeenThreatened)
                Debug.Log(
                    $"Smart fish '{gameObject.name} ({gameObject.GetInstanceID()})' has seen fear and is swimming away.");
            hasBeenThreatened = true;
        }
        if (transform.position.x > (maxX + 2.0f) || transform.position.x < (minX - 2.1f))
            Destroy(gameObject);
    }
}
