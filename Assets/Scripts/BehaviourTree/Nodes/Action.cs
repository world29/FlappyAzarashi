using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Action : Node
    {
        public virtual float GetProgress()
        {
            return 0;
        }
    }
}
