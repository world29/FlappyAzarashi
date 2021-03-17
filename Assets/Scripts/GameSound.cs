using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSound : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        m_audioSource.clip = clip;

        m_audioSource.Play();
    }

    public void Stop()
    {
        m_audioSource.Stop();
        m_audioSource.volume = 1;
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    IEnumerator FadeOutCoroutine(float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            m_audioSource.volume = Mathf.Lerp(1, 0, t / duration);

            yield return new WaitForEndOfFrame();
        }

        Stop();
    }
}
