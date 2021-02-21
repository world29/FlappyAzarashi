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
            return;
        }

        gameController.SendMessage("IncreaseScore");

        gameObject.SetActive(false);
    }
}
