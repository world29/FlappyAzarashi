using UnityEngine;
using System.Collections;

public class ClashCamera : MonoBehaviour
{
    public void Clash()
    {
        FlashEffect.Play();
        Shake();
    }

    public void Shake()
    {
        GetComponent<Animator>().SetTrigger("shake");
    }
}
