using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rat
{
    public class RatMonoBehaviour : MonoBehaviour
    {
        public GameEvents Events { get { return GameEvents.Instance; } }
        public GameValues Globals { get { return GameValues.Instance; } }
    }
}