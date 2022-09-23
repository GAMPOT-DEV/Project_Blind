using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class tutorialTest : MonoBehaviour
    {
        AbyssSceneManager abyssSceneManager = new AbyssSceneManager();
        // Start is called before the first frame update
        private void OnTriggerEnter2D(Collider2D collision)
        {
            abyssSceneManager.MoveNextStage();
        }
    }
}

