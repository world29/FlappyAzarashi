using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class WaitPlayableAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<WaitPlayableBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.m_playableDirector = go.GetComponent<PlayableDirector>();

        return playable;
    }
}
