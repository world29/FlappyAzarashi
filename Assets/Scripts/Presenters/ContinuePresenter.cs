using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePresenter : MonoBehaviour, IGameStateEventHandler
{
    public GameObject m_continueUI;

    private Coroutine m_coroutine = null;

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    public void OnGameStateChanged(IGameState prevState, IGameState nextState)
    {
        if (nextState is GameState_Continue)
        {
            OnEnterContinueState();
        }

        if (prevState is GameState_Continue)
        {
            OnExitContinueState();
        }
    }

    void OnEnterContinueState()
    {
        m_continueUI.SetActive(true);

        m_continueUI.GetComponent<ContinueSequence>().Animate();
    }

    void OnExitContinueState()
    {
        m_continueUI.SetActive(false);
    }

    IEnumerator FadeInContinueUI()
    {
        var canvasGroup = m_continueUI.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;

        m_continueUI.SetActive(true);

        const float duration = 0.5f;

        float t = 0;
        while (t < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;
        }

        canvasGroup.alpha = 1;

        m_coroutine = null;
    }
}
