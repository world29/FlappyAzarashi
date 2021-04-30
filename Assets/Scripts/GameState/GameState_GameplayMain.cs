using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_GameplayMain : IGameState
{
    private PlayerController m_player;

    public GameState_GameplayMain()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        m_player = go.GetComponent<PlayerController>();
    }

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_player.SetSteerActive(true);

        m_player.Flap();
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        m_player.SetSteerActive(false);
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_player.IsDead())
        {
            return new GameState_GameplayDead();
        }

        return this;
    }
}
