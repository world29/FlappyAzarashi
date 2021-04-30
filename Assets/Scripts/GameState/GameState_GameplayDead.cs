using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_GameplayDead : IGameState
{
    private PlayableDirector m_playingTimeline;

    private Coroutine m_coroutine = null;

    public void OnEnter(GameStateMachineContext ctx)
    {
        var sound = Sound.GetInstance();

        sound.StopBgm();

        //HACK: コルーチン実行のため、適当な MonoBehaviour を借りている
        m_coroutine = DifficultyBehaviour.Instance.StartCoroutine(DeathCoroutine());
    }

    public void OnExit(GameStateMachineContext ctx)
    {

    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_coroutine == null)
        {
            if (GameDataAccessor.PlayerLifeCount == 0)
            {
                return new GameState_DemoGameOver();
            }
            else
            {
                return new GameState_Continue();
            }
        }

        return this;
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSecondsRealtime(1);

        m_coroutine = null;
    }
}
