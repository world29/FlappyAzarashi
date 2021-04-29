using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : SingletonMonoBehaviour<TimelineManager>
{
    public PlayableDirector m_timelineTitleToGameplay;

    public PlayableDirector m_timelinePlayerSpawn;

    public PlayableDirector m_timelinePlayerDead;

    public PlayableDirector m_timelinePlayerDeadAndGameOver;
}
