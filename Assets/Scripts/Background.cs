using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour, IStageEvents, IGameStateEventHandler
{
    public float m_scrollSpeed = 0;
    [Range(0, 1)]
    public float m_parallax = 1;
    public float m_borderRight;
    public float m_nextOffset;

    private bool m_scrollEnabled = true;
    private bool m_skipUpdate = false;

    private Vector3 m_lastCamerPosition;

    private Vector3 m_initialPosition;

    private void Awake()
    {
        m_initialPosition = transform.position;
    }

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IStageEvents>(gameObject);
        BroadcastReceivers.RegisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IStageEvents>(gameObject);
        BroadcastReceivers.UnregisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    private void Start()
    {
        m_lastCamerPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        if (m_skipUpdate)
        {
            m_lastCamerPosition = Camera.main.transform.position;
            m_skipUpdate = false;
            return;
        }

        if (m_scrollEnabled)
        {
            var camPos = Camera.main.transform.position;

            var diff = camPos.x - m_lastCamerPosition.x;
            var offsetX = diff * m_parallax;

            Scroll(offsetX);
        }

        m_lastCamerPosition = Camera.main.transform.position;
    }

    void Scroll(float x)
    {
        transform.Translate(new Vector3(x, 0, 0));

        var cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));

        if ((transform.position.x + m_borderRight) < cameraLeft.x)
        {
            OnScrollEnd();
        }
    }

    void OnScrollEnd()
    {
        transform.Translate(new Vector3(m_nextOffset, 0, 0));
    }

    public void StartParallaxScroll()
    {
        m_scrollEnabled = true;
    }

    public void StopScroll()
    {
        m_scrollEnabled = false;
    }

    public void SkipNextUpdate()
    {
        m_skipUpdate = true;
    }

    public void OnStageRetry(StageId id)
    {
        transform.position = m_initialPosition;
    }

    public void OnGameStateChanged(IGameState prevState, IGameState nextState)
    {
        // リスポーン演出時はカメラ移動に応じた自身の座標更新を行わない
        if (nextState is GameState_DemoPlayerRespawn)
        {
            m_scrollEnabled = false;
        }

        if (prevState is GameState_DemoPlayerRespawn)
        {
            m_scrollEnabled = true;
        }
    }
}
