using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    static int count = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            count++;

            ScreenCapture.CaptureScreenshot("screenshot" + count.ToString() + ".png");
        }
    }
}
