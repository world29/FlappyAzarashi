using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameStateMachineContext : ScriptableObject
{
    public GameEvent m_titlePressStartEvent;

    public GameEvent m_PressContinueEvent;

    public GameEvent m_PressSurrenderEvent;

    public Vector3 m_playerRespawnPosition;
}
