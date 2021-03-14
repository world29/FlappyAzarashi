using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]
public class CheckPoint : MonoBehaviour
{
    public AudioClip m_sound;

    AudioSource m_audioSource;
    GameController m_gameController;

    Dictionary<GameObject, Vector3> m_propsPosition = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        m_gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!enabled) return;

        FlashEffect.Play();

        m_audioSource.PlayOneShot(m_sound);

        m_gameController.SetRespawnPoint(this);
    }

    public void SavePropsPosition()
    {
        var props = GameObject.FindGameObjectsWithTag("Prop");
        foreach (var obj in props)
        {
            m_propsPosition.Add(obj, obj.transform.position);
        }
    }

    public void RestorePropsPosition()
    {
        foreach (var entry in m_propsPosition)
        {
            var obj = entry.Key;
            var pos = entry.Value;

            obj.transform.position = pos;
        }
    }
}
