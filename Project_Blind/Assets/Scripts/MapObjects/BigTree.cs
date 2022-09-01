using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BigTree : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
          
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null)
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    Debug.Log("¡¢√À«‘");
                }
            }
        }
    }

}

