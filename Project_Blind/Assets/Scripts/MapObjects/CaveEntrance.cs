using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class CaveEntrance : MonoBehaviour
    {
        [SerializeField] private int isCaveClear;
        // Start is called before the first frame update
        void Start()
        {
            isCaveClear = DataManager.Instance.GetClearCave();
            if(isCaveClear == 1)
            {
                gameObject.SetActive(false);
            }

        }
    }
}

