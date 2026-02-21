using BulletsSoul.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullets : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var pc = collision.GetComponent<PlayerController>();

            if (pc != null)
            {
                pc.TakeDamage(damage);
            }

            //Destroy(gameObject);
        }
    }
}
