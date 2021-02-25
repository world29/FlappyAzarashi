using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTrigger : MonoBehaviour
{
    GameObject gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.transform.root.CompareTag("Player"))
        {
            return;
        }

        gameController.SendMessage("GameClear");
    }
}
