using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour
{
    static readonly string kGameID_iOS = "4112076";
    static readonly string kGameID_Android = "4112077";

    string gameId = kGameID_iOS;
    bool testMode = true;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
    }
}
