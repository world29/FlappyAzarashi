using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ContinueSequence : MonoBehaviour
{
    public CanvasGroup m_canvasGroup;

    public CanvasGroup m_lifeCountCanvasGroup;
    public TextMeshProUGUI m_lifeCountText;

    public CanvasGroup m_continueButtonCanvasGroup;

    public void Initialize()
    {
        m_canvasGroup.alpha = 0;

        m_lifeCountCanvasGroup.alpha = 0;
        m_lifeCountText.text = "";

        m_continueButtonCanvasGroup.alpha = 0;
    }

    public void Animate()
    {
        Initialize();

        var sequence = DOTween.Sequence();

        // fade-in
        sequence
            .Append(m_canvasGroup.DOFade(1, 1.0f))
            .AppendInterval(0.5f);

        // life count
        int lifeCountValue = GameDataAccessor.PlayerLifeCount;

        sequence
            .Append(m_lifeCountCanvasGroup.DOFade(1.0f, 0.25f))
            .AppendCallback(() => m_lifeCountText.transform.localScale = Vector3.one * 1.25f)
            .AppendCallback(() => m_lifeCountText.text = $"x {lifeCountValue}")
            .Append(m_lifeCountText.transform.DOScale(Vector3.one, 0.5f))
            .AppendInterval(0.5f);

        // appear button
        sequence
            .Append(m_continueButtonCanvasGroup.DOFade(1.0f, 0.25f));

        sequence.Play();
    }
}
