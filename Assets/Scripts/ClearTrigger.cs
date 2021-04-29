using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    private PlayerController m_player;

    void Start()
    {
        var go = GameObject.FindWithTag("Player");
        m_player = go.GetComponent<PlayerController>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_player.IsDead()) return;

        GameDataAccessor.Score++;
    }
}
