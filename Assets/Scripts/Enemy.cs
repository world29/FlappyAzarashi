using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var contact = collision.contacts[0];
            gameObject.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);

            gameObject.SetActive(false);

            return;
        }

        gameObject.SetActive(false);
    }
}
