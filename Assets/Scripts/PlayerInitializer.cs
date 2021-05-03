using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    private PlayerController m_player;

    void Start()
    {
        var go = GameObject.FindWithTag("Player");
        m_player = go.GetComponent<PlayerController>();

        m_player.SetSteerActive(false);
    }
}
