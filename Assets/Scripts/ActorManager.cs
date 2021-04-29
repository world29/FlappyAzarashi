using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorManager
{
    public static void DestroyActors()
    {
        var objects = GameObject.FindGameObjectsWithTag("Actor");
        foreach (var o in objects)
        {
            GameObject.Destroy(o);
        }
    }
}
