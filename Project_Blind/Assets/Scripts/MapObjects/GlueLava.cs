using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class GlueLava : MonoBehaviour
    {
        private PlayerCharacter character;
        private void Awake()
        {
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!character.isOnLava)
                {
                    Debug.Log("�������� ����");
                    PlayerCharacter character = collision.gameObject.GetComponent<PlayerCharacter>();
                    character.DebuffOn();

                }
            }
        }
    }
}
