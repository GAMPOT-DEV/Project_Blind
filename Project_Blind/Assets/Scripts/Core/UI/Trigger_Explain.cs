using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blind
{
    public class Trigger_Explain : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.LogWarning("?");
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            ResourceManager.Instance.Instantiate($"UI/Explains/UI_Explain_Item");
            Destroy(this.gameObject);
        }
    }
}

