using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class EnemyCharacter : MonoBehaviour
    {
        private enum State
        {
            Roaming,
            ChasePlayer,
            Return,
        }
        private CharacterController2D _characterController2D;
        private Vector2 _moveVector;
        private float _recognitionRange = 5f;
        private Vector2 _roamingPosition;
        private Vector2 _startingPosition;
        private GameObject _player;
        private State state;
        private float _speed = 0.3f;

        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _startingPosition = transform.position;
            _roamingPosition = new Vector2(-_speed, 0f);
            _player = GameObject.Find("Player");
            state = State.Roaming;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            switch (state)
            {
                case State.Roaming:
                    
                    FindTarget();
                    break;

                case State.ChasePlayer:
                    _characterController2D.Move(new Vector2(DirectionVector(_player.transform.position), 0f));
                    MissTarget();
                    break;

                case State.Return:
                    float direction = DirectionVector(_startingPosition);
                    if (direction == 0)
                    {
                        state = State.Roaming;
                        break;
                    }
                    _characterController2D.Move(new Vector2(direction, 0f));
                    FindTarget();
                    break;
            }

            _characterController2D.OnFixedUpdate();
        }

        private float DirectionVector(Vector2 goal)
        {
            if (goal.x - transform.position.x > 0)
                return _speed;
            else if (goal.x - transform.position.x < 0)
                return -_speed;
            else if (Mathf.Abs(goal.x - transform.position.x) < 0.5f)
            {
                Debug.Log(Mathf.Abs(goal.x - transform.position.x));
                return 0;
            }
            return 0;
        }

        private void FindTarget()
        {
            if(Vector2.Distance(transform.position, _player.transform.position) < _recognitionRange)
            {
                state = State.ChasePlayer;
            }
        }

        private void MissTarget()
        {
            if (Vector2.Distance(transform.position, _player.transform.position) > _recognitionRange)
            {
                state = State.Return;
            }
        }
    }
}