using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind.Abyss
{
    public class DashTutor : MonoBehaviour
    {
        bool isTriggered = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (!isTriggered)
            {
                AbyssSceneManager.Instance.ShowText("Stage2_1");
                isTriggered = true;
            }
        }
    }
}
