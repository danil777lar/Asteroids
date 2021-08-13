using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLog : MonoBehaviour
{
    public List<Collider2D> collisions = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisions.Add(collision);
    }
}
