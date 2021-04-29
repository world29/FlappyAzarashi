using UnityEngine;
using System.Collections;

public class ClashCamera : MonoBehaviour
{
    public void Clash()
    {
        Flash();
        Shake();
    }

    public void Flash()
    {
        FlashEffect.Play();
    }

    public void Shake()
    {
        GetComponent<Animator>().SetTrigger("shake");
    }
}
