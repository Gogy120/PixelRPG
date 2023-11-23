using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector2 dir;
    public float speed;

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
