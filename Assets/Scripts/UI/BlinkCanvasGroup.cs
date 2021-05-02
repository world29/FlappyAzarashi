using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkCanvasGroup : MonoBehaviour
{
    public float m_durationSeconds;

    public Ease m_easeType;

    private CanvasGroup m_canvasGroup;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_canvasGroup
            .DOFade(0.0f, m_durationSeconds)
            .SetEase(m_easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
