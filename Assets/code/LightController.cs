using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Vector2 setpoint_direction = Vector2.zero;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float max_distance = 10.0f;

    private Vector2 CalculateControlPosition(Vector2 current_position)
    {
        if (setpoint_direction == Vector2.zero)
            return current_position;
        Vector2 control_position = current_position + speed * Time.fixedDeltaTime * setpoint_direction;
        if (control_position.magnitude > max_distance)
            return current_position;
        return control_position;
    }

    private void Update()
    {
        setpoint_direction = new Vector2(Input.GetAxis("LightHorizontal"), Input.GetAxis("LightVertical")).normalized;
    }

    private void FixedUpdate()
    {
        transform.localPosition = CalculateControlPosition(transform.localPosition);
    }
}
