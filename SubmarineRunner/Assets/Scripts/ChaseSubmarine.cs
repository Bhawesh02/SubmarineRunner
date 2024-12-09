using System;
using UnityEngine;


    public abstract class ChaseSubmarine : MonoBehaviour
    {
        
        
        [SerializeField] protected Rigidbody2D m_rigidbody2D;
        
        protected float m_chaseSpeed;
        protected float m_rotationSpeed;
        protected Transform m_transform;
        protected Submarine m_targetSubmarine;
        protected float m_distanceToTargetSub;
        protected Vector2 m_hitDirection;
        protected float m_speedRecovery;
        
        protected bool m_isMovementPause;
        
        private void Awake()
        {
            Init();
        }
        
        protected virtual void Init()
        {
            m_transform = transform;
        }
        
        protected void SetTargetSubmarine(Submarine targetSubmarine)
        {
            m_targetSubmarine = targetSubmarine;
        }

        protected virtual void Update()
        {
            if (m_isMovementPause)
            {
                return;
            }
            Chase();
        }

        private void Chase()
        {
            if (!m_targetSubmarine)
            {
                return;
            }
            Vector2 targetPosition = m_targetSubmarine.SubmarineTransform.position;
            Vector2 currentPosition = m_transform.position;
            m_distanceToTargetSub = Vector2.Distance(currentPosition, targetPosition);
            Vector2 direction = targetPosition - currentPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            m_transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
            m_hitDirection = Vector2.Lerp(m_hitDirection, Vector2.zero, m_speedRecovery * Time.deltaTime);
            m_rigidbody2D.velocity =( m_transform.right + (Vector3)m_hitDirection) * m_chaseSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject != m_targetSubmarine.gameObject)
            {
                return;
            }
            OnHit();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != m_targetSubmarine.gameObject)
            {
                return;
            }
            OnHit();
        }
        
        protected void StopMovement()
        {
            m_isMovementPause = true;
            m_rigidbody2D.velocity = Vector2.zero;
        }
        
        protected abstract void OnHit();
        public virtual void Config(Submarine targetSubmarine){}
    }

