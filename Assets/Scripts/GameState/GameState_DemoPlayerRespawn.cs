using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_DemoPlayerRespawn : IGameState
{
    private PlayerController m_player;
    private Camera m_camera;

    public GameState_DemoPlayerRespawn()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        m_player = go.GetComponent<PlayerController>();

        m_camera = Camera.main;
    }

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_player.transform.position = ctx.m_playerRespawnPosition;

        m_player.Respawn();

        BroadcastExecuteEvents.Execute<IStageEvents>(null /* eventData */,
            (handler, eventData) => handler.OnStageRetry(StageId.Stage1));
    }

    public void OnExit(GameStateMachineContext ctx)
    {
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        // プレイヤーが画面内にはいるまでカメラが移動したら次ステートへ遷移する
        var viewportPoint = m_camera.WorldToViewportPoint(m_player.transform.position);
        if (viewportPoint.x > 0 && viewportPoint.x < 1)
        {
            return new GameState_GameplayReady();
        }

        return this;
    }
}
