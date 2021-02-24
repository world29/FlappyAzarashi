using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonState : MonoBehaviour
{
    bool m_isPressed = false;
    bool m_isDown = false;
    bool m_isUp = false;

    public void OnPointerDown()
    {
        if (m_isPressed == false)
        {
            m_isDown = true;
        }
        m_isPressed = true;
    }

    public void OnPointerUp()
    {
        if (m_isPressed == true)
        {
            m_isUp = true;
        }
        m_isPressed = false;
    }

    public bool IsPressed()
    {
        return m_isPressed;
    }

    public bool isDown()
    {
        bool result = m_isDown;
        m_isDown = false;
        return result;
    }

    public bool isUp()
    {
        bool result = m_isUp;
        m_isUp = false;
        return result;
    }
}
