using System;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 플레이어를 포함한 모든 캐릭터들의 움직임등을 처리하는 클래스입니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterController2D : MonoBehaviour,IGameManagerObj
    {
        
        public LayerMask groundedLayerMask;
        public float groundedRaycastDistance = 0.1f;


        private Rigidbody2D _rigidBody2D;
        protected Collider2D _collider2D;

        private Vector2 _nextMovement;
        private Vector2 _previousPosition;
        private Vector2 _currentPosition;
        
        private Vector2 _boxCastSize = new Vector2(0.4f,0.3f);
        private float _boxCastMaxDistance = 1f;
        private int _groundMask;
        CapsuleCollider2D m_Capsule;
        ContactFilter2D m_ContactFilter;
        RaycastHit2D[] m_HitBuffer = new RaycastHit2D[5];
        RaycastHit2D[] m_FoundHits = new RaycastHit2D[3];
        Collider2D[] m_GroundColliders = new Collider2D[3];
        Vector2[] m_RaycastPositions = new Vector2[3];
        
        public Vector2 Velocity { get; private set; }
        public bool IsGrounded { get; protected set; }
        public ContactFilter2D ContactFilter { get { return m_ContactFilter; } }
        public Collider2D[] GroundColliders { get { return m_GroundColliders; } }

        
        public bool IsCeilinged { get; protected set; }

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            m_Capsule = GetComponent<CapsuleCollider2D>();

            _previousPosition = _rigidBody2D.position;
            _currentPosition = _rigidBody2D.position;
            
            m_ContactFilter.layerMask = groundedLayerMask;
            m_ContactFilter.useLayerMask = true;
            m_ContactFilter.useTriggers = false;
            
            _groundMask = LayerMask.GetMask("Floor");
        }

        public void OnFixedUpdate()
        {
            _previousPosition = _rigidBody2D.position;
            _currentPosition = _previousPosition + _nextMovement;
            Velocity = (_currentPosition - _previousPosition) / Time.deltaTime;
            
            _rigidBody2D.MovePosition(_currentPosition);
            _nextMovement = Vector2.zero;
            CheckCapsuleEndCollisions();
            CheckCapsuleEndCollisions(false);
        }

        public void OnDrawGizmos() {
            var _FootPos = transform.position - new Vector3(0,1f,0);
            RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, _boxCastSize, 0f, Vector2.down, _boxCastMaxDistance,_groundMask); // 다리 밑으로 레이캐스트를 쏴 바닥을 체크합니다.
            Gizmos.color = Color.cyan;
            if (raycastHit.collider != null)
            {
                Gizmos.DrawRay(_FootPos, Vector2.down * raycastHit.distance);
                Gizmos.DrawWireCube(_FootPos + Vector3.down * raycastHit.distance, _boxCastSize);
            }
            else
            {
                Gizmos.DrawRay(_FootPos, Vector2.down * _boxCastMaxDistance);
            }
        }

        public void Move(Vector2 movement)
        {
            _nextMovement += movement;
        }

         public void CheckCapsuleEndCollisions(bool bottom = true)
        {
            Vector2 raycastDirection;
            Vector2 raycastStart;
            float raycastDistance;
            if (m_Capsule == null)
            {
                raycastStart = _rigidBody2D.position + Vector2.up;
                raycastDistance = 1f + groundedRaycastDistance;

                if (bottom)
                {
                    raycastDirection = Vector2.down;

                    m_RaycastPositions[0] = raycastStart + Vector2.left * 0.4f;
                    m_RaycastPositions[1] = raycastStart;
                    m_RaycastPositions[2] = raycastStart + Vector2.right * 0.4f;
                }
                else
                {
                    raycastDirection = Vector2.up;

                    m_RaycastPositions[0] = raycastStart + Vector2.left * 0.4f;
                    m_RaycastPositions[1] = raycastStart;
                    m_RaycastPositions[2] = raycastStart + Vector2.right * 0.4f;
                }
            }
            else
            {
                raycastStart = _rigidBody2D.position + m_Capsule.offset;
                raycastDistance = m_Capsule.size.x * 0.5f + groundedRaycastDistance * 2f;

                if (bottom)
                {   
                    raycastDirection = Vector2.down;
                    Vector2 raycastStartBottomCentre = raycastStart + Vector2.down * (m_Capsule.size.y * 0.5f - m_Capsule.size.x * 0.5f);

                    m_RaycastPositions[0] = raycastStartBottomCentre + Vector2.left * m_Capsule.size.x * 0.5f;
                    m_RaycastPositions[1] = raycastStartBottomCentre;
                    m_RaycastPositions[2] = raycastStartBottomCentre + Vector2.right * m_Capsule.size.x * 0.5f;
                }
                else
                {
                    raycastDirection = Vector2.up;
                    Vector2 raycastStartTopCentre = raycastStart + Vector2.up * (m_Capsule.size.y * 0.5f - m_Capsule.size.x * 0.5f);

                    m_RaycastPositions[0] = raycastStartTopCentre + Vector2.left * m_Capsule.size.x * 0.5f;
                    m_RaycastPositions[1] = raycastStartTopCentre;
                    m_RaycastPositions[2] = raycastStartTopCentre + Vector2.right * m_Capsule.size.x * 0.5f;
                }
            }

            for (int i = 0; i < m_RaycastPositions.Length; i++)
            {
                int count = Physics2D.Raycast(m_RaycastPositions[i], raycastDirection, m_ContactFilter, m_HitBuffer, raycastDistance);

                if (bottom)
                {
                    m_FoundHits[i] = count > 0 ? m_HitBuffer[0] : new RaycastHit2D();
                    m_GroundColliders[i] = m_FoundHits[i].collider;
                }
                else
                {
                    IsCeilinged = false;

                    for (int j = 0; j < m_HitBuffer.Length; j++)
                    {
                        if (m_HitBuffer[j].collider != null)
                        {
                            if (!PhysicHelper.ColliderHasPlatformEffector(m_HitBuffer[j].collider))
                            {
                                IsCeilinged = true;
                            }
                        }
                    }
                }
            }

            if (bottom)
            {
                Vector2 groundNormal = Vector2.zero;
                int hitCount = 0;

                for (int i = 0; i < m_FoundHits.Length; i++)
                {
                    if (m_FoundHits[i].collider != null)
                    {
                        groundNormal += m_FoundHits[i].normal;
                        hitCount++;
                    }
                }

                if (hitCount > 0)
                {
                    groundNormal.Normalize();
                }

                Vector2 relativeVelocity = Velocity;

                if (Mathf.Approximately(groundNormal.x, 0f) && Mathf.Approximately(groundNormal.y, 0f))
                {
                    IsGrounded = false;
                }
                else
                {
                    IsGrounded = relativeVelocity.y <= 0f;
                    if (m_Capsule != null)
                    {
                        if (m_GroundColliders[1] != null)
                        {
                            float capsuleBottomHeight = _rigidBody2D.position.y + m_Capsule.offset.y - m_Capsule.size.y * 0.5f;
                            float middleHitHeight = m_FoundHits[1].point.y;
                            IsGrounded &= middleHitHeight < capsuleBottomHeight + groundedRaycastDistance;
                        }
                    }
                }
            }

            for (int i = 0; i < m_HitBuffer.Length; i++)
            {
                m_HitBuffer[i] = new RaycastHit2D();
            }
        }
    }
}