using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public enum ButtonType
    {
        Main,
        Sub,
    }

    public ButtonState m_buttonMain;
    public ButtonState m_buttonSub;

    public bool GetButtonDown(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.Main:
                return m_buttonMain.isDown() || Input.GetButtonDown("Fire1");
            case ButtonType.Sub:
                return m_buttonSub.isDown() || Input.GetButtonDown("Fire2");
        }
        return false;
    }
}
