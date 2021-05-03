using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ResultSequence : MonoBehaviour
{
    public TextMeshProUGUI m_scoreLabel;

    public TextMeshProUGUI m_scoreValueText;

    public CanvasGroup m_gotoTitleButtonCanvasGroup;

    public void Initialize()
    {
        m_scoreLabel.alpha = 0;

        m_scoreValueText.text = "";

        m_gotoTitleButtonCanvasGroup.alpha = 0;
    }

    public void Animate()
    {
        Initialize();

        var sequence = DOTween.Sequence();

        // score
        int score = GameDataAccessor.Score;

        sequence
            .Append(m_scoreLabel.DOFade(1, 0.25f))
            .Append(DOVirtual.Float(0, score, 1.0f, (value) => m_scoreValueText.text = ((int)value).ToString()))
            .AppendCallback(() => m_scoreValueText.transform.localScale = Vector3.one * 1.25f)
            .Append(m_scoreValueText.transform.DOScale(Vector3.one, 0.5f))
            .AppendInterval(0.5f);

        // goto Title button
        sequence
            .Append(m_gotoTitleButtonCanvasGroup.DOFade(1, 0.25f));

        sequence.Play();
    }
}
