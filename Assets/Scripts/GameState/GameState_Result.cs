using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class GameState_Result : IGameState
{
    class GameEventListener : IGameEventListener
    {
        public bool Raised { get; private set; } = false;

        public void OnEventRaised()
        {
            Raised = true;
        }
    }

    GameEventListener m_surrenderListener;

    private bool m_isAdvertisementCompleted = false;

    private bool m_isShowingAds = false;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_surrenderListener = new GameEventListener();

        ctx.m_PressSurrenderEvent.RegisterListener(m_surrenderListener);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_PressSurrenderEvent.UnregisterListener(m_surrenderListener);

        m_surrenderListener = null;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_isAdvertisementCompleted)
        {
            ReloadScene();

            return new GameState_Title();
        }

        if (m_surrenderListener.Raised)
        {
            if (!Advertisement.IsReady())
            {
                ReloadScene();

                return new GameState_Title();
            }

            ShowAds();
        }

        return this;
    }

    private void ShowAds()
    {
#if UNITY_IOS
        if (m_isShowingAds) return;

        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        Debug.LogFormat("ATTrackingAuthrizationStatus={0}", status);

        switch (status)
        {
            case ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED:
            case ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED:
                m_isAdvertisementCompleted = true;
                break;
            case ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED:
                ShowAdsImpl();
                m_isShowingAds = true;
                break;
        }
#else
        m_isAdvertisementCompleted = true;
#endif
    }

    private void ShowAdsImpl()
    {
        var options = new ShowOptions
        {
            resultCallback = OnAdvertisementCompleted
        };

        Advertisement.Show(options);
    }

    public void OnAdvertisementCompleted(ShowResult result)
    {
        m_isAdvertisementCompleted = true;
    }

    private void ReloadScene()
    {
        //todo: 本来はステートマシンの外部でシーン遷移を呼び出す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
