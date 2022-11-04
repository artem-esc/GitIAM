using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMovement : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

