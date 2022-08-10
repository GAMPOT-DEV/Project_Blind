using System;
using UnityEngine;

namespace Blind
{
    public class WaveBullet : MonoBehaviour
    {
        private Rigidbody2D rigid;
        private bool facing;
        [SerializeField] private int speed;
        private bool isFire = false;
        public void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        public void GetFacing(bool _playerfacing)
        {
            facing = _playerfacing;
            isFire = true;
        }

        public void Update()
        {
            if (isFire)
            {
                if (facing) rigid.velocity = transform.right * speed;
                else rigid.velocity = -transform.right * speed;
            }
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == 6 || col.gameObject.layer == 7)
            {
                var waveSense = ResourceManager.Instance.Instantiate("WaveSense").GetComponent<WaveSense>();
                waveSense.transform.position = transform.position;
                waveSense.StartSpread();
                Destroy(gameObject);
            }
        }
    }
}