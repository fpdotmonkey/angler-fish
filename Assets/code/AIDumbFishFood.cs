using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Pick a position on the left of the screen and swim to the right
/// until eaten or there's no more screen left.
public class AIDumbFishFood : MonoBehaviour
{
    [SerializeField]
    private float minSpeed = 0.5f;
    [SerializeField]
    private float maxSpeed = 1.5f;

    private Vector2 velocity;
    private float maxX;

    private Vector2 InitialPosition()
    {
        // This all assumes a static camera with the origin in the center
        float maxY = Camera.main.orthographicSize;
        float minX = -Camera.main.aspect * maxY;
        maxX = -minX;

        return new Vector2(minX - 2.0f, Random.Range(-maxY, maxY));
    }

    private void Start()
    {
        transform.position = InitialPosition();
        velocity = Random.Range(minSpeed, maxSpeed) * Vector2.right;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3) velocity * Time.fixedDeltaTime;
        if (transform.position.x > (maxX + 2.0f))
            Destroy(gameObject);
    }
}
