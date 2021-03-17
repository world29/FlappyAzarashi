using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WaitPlayableBehaviour : PlayableBehaviour
{
    public PlayableDirector m_playableDirector;

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (playable.GetTime() != 0)
        {
            m_playableDirector.Pause();
        }
    }
}
