using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Eating : MonoBehaviour
{
    [SerializeField]
    private bool canEatPlayer = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("fish") ||
            (canEatPlayer && other.gameObject.layer == LayerMask.NameToLayer("player")))
        {
            Debug.Log($"Eating fish '{other.gameObject.name} ({other.gameObject.GetInstanceID()})'");
            Destroy(other.gameObject);
        }
    }
}
