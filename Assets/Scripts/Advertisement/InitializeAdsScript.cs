using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour
{
#if UNITY_IOS
    static readonly string kGameID = "4112076";
#elif UNITY_ANDROID
    static readonly string kGameID = "4112077";
#endif

    public bool testMode;

    void Start()
    {
        Advertisement.Initialize(kGameID, testMode);
    }
}
