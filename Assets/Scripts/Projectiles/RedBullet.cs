using BulletsSoul.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullets : MonoBehaviour
{
    public float damage = 10f;

    private PlayerController _playerController;
    private bool _playerInTrigger;

    private void FixedUpdate()
    {
        if (_playerInTrigger && _playerController != null)
        {
            _playerController.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerController = collision.GetComponent<PlayerController>();
            if (_playerController != null)
            {
                _playerController.TakeDamage(damage);
                _playerInTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInTrigger = false;
            _playerController = null;
        }
    }
}
