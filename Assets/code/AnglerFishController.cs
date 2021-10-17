using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFishController : MonoBehaviour
{
    private Vector2 setpointPosition;

    [SerializeField]
    private float speed = 1.0f;

    private Vector2 CalculateControlPosition(Vector2 currentPosition)
    {
        if ((setpointPosition - currentPosition).magnitude < 0.01f)
        {
            return currentPosition;
        }
        Vector2 control_position =
            currentPosition + speed * Time.fixedDeltaTime * (setpointPosition - currentPosition).normalized;
        return control_position;
    }

    private void Start()
    {
        setpointPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetButton("Move"))
        {
            setpointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        transform.position = CalculateControlPosition(transform.position);
    }
}
