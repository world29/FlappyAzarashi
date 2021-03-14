using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]
public class CheckPoint : MonoBehaviour
{
    public AudioClip m_sound;

    AudioSource m_audioSource;
    GameController m_gameController;

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
}
