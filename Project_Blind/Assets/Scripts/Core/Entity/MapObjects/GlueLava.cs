using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class GlueLava : MonoBehaviour
    {
        private PlayerCharacter character;
        private void Start()
        {
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!character.isOnLava)
                {
                    Debug.Log("점액질을 밟음");
                    character.DebuffOn();

                }
            }
        }
    }
}
