using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFishController : MonoBehaviour
{
    private Vector2 setpoint_position;

    [SerializeField]
    private float speed = 1.0f;

    private Vector2 CalculateControlPosition(Vector2 current_position)
    {
        if ((setpoint_position - current_position).magnitude < 0.01f)
        {
            return current_position;
        }
        Vector2 control_position =
            current_position + speed * Time.fixedDeltaTime * (setpoint_position - current_position).normalized;
        return control_position;
    }

    private void Update()
    {
        if (Input.GetButton("Move"))
        {
            setpoint_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void FixedUpdate()
    {
        transform.position = CalculateControlPosition(transform.position);
    }
}
