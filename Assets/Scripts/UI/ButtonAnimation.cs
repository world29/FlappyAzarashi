using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    public string m_actionName;
    public float m_scaleEndValue = 0.8f;
    public float m_duration = 0.25f;

    private RectTransform m_rectTransform;
    private Vector3 m_baseScale;
    private PlayerAction.PlatformActionActions m_action;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_baseScale = m_rectTransform.localScale;

        m_action = InputManager.Input.PlatformAction;
    }

    private void LateUpdate()
    {
        if (m_action.Jump.triggered)
        {
            OnJump();
        }

        if (m_action.Dash.triggered)
        {
            OnDash();
        }
    }

    public void OnJump()
    {
        if (m_actionName == "Jump")
        {
            PlayAnimation();
        }
    }

    public void OnDash()
    {
        if (m_actionName == "Dash")
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        m_rectTransform
            .DOScale(m_scaleEndValue, m_duration)
            .SetEase(Ease.OutBounce)
            .SetLink(gameObject)
            .OnComplete(() => { m_rectTransform.localScale = m_baseScale; });
    }
}
