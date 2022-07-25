using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blind
{
    public class TransitionDestination : MonoBehaviour
    {
        public enum DestinationTag
        {
            A,B,C,D,E,F,G,
        }

        public DestinationTag destinationTag;
        public GameObject transformingObject;
        public UnityEvent OnReachDestination;
    }

}
