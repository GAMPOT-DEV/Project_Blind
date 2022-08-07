using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TestClue : MonoBehaviour
    {
        [SerializeField] private int _clueId;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;

            Debug.Log("?");

            bool result = DataManager.Instance.GetClueItem(_clueId);
            if (result == true)
            {
                DataManager.Instance.SaveGameData();
                Destroy(this.gameObject);
            }
        }
    }
}
