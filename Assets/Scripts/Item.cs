using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        var playerObject = collision.gameObject;

        playerObject.SendMessage("PowerUpShotThreeWay");

        gameObject.SetActive(false);
    }
}
