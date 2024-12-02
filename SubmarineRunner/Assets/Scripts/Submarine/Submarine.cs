
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


    public abstract class Submarine : MonoBehaviour
    {
        private const float GRAVITY_IN_AIR = 0.6f;
        private const float GRAVITY_IN_WATER = 0;
        private const float GRAVITY_IN_WATER_TO_FALL_DOWN = 2f;
        private const float DRAG_IN_AIR = 0f;
        private const float DRAG_IN_WATER = 1f;
        private const float WATER_LEVEL = -6.5f;
        
        [Header("Speed")]
        [SerializeField, ReadOnly] private float m_currentMoveSpeed;
        
        private bool m_isInAir;
        private bool m_isMovementAllowed = true;
        private Coroutine m_speedCoroutine;
        private Tween m_rotationTween;

        [Header("References")]
        [SerializeField] protected Rigidbody2D m_rigidbody2D;
        [SerializeField] protected ParticleSystem m_explosionParticle;
        [SerializeField] protected GameObject m_wholeSubmarine;
        [SerializeField] protected GameObject m_brokenSubmarine;
        [SerializeField] private SpriteRenderer m_angleArrow;
        [SerializeField] protected ParticleSystem m_defaultTrail;
        [SerializeField] protected GameObject m_targetGlow;
        [SerializeField] private Rigidbody2D[] m_brokenPartRigidBodies;
        
        protected Vector3 m_directionVector;
        protected RotationType m_currentRotationType;
        protected Transform m_submarineTransform;
        protected SurvivalConfig.SubmarineRotationData m_submarineRotationData;
        protected float m_originalSpeed;

        public Transform SubmarineTransform => m_submarineTransform;
        public float CurrentSpeed => m_currentMoveSpeed;
        
        public bool IsInAir()
        {
            return m_isInAir;
        }

        public void SetMoveSpeed(float newSpeed, float activeDuration)
        {
            if (m_speedCoroutine != null)
            {
                StopCoroutine(m_speedCoroutine);
            }
            SetMoveSpeed(newSpeed);
            m_speedCoroutine = StartCoroutine(CoroutineUtils.Delay(activeDuration, () =>
            {
                SetMoveSpeed(m_originalSpeed);
            }));
        }

        public void SetMoveSpeed(float newSpeed, bool overrideOriginalSpeed = false)
        {
            m_currentMoveSpeed = newSpeed;
            if (overrideOriginalSpeed)
            {
                m_originalSpeed = m_currentMoveSpeed;
            }
        }
        
        private void Start()
        {
            Init();
        }
        

        protected virtual void Init()
        {
            m_submarineTransform = transform;
            SetRotationData();
        }

        protected abstract void SetRotationData();
        
        protected virtual void Update()
        {
            CheckInAir();
            HandlePhysics();
            if (!m_isMovementAllowed)
            {
                return;
            }
            MoveSubmarine();
            HandelRotation();
        }

        private void CheckInAir()
        {
            m_isInAir = transform.position.y > WATER_LEVEL;
        }

        protected virtual void HandlePhysics()
        {
            if (m_isInAir)
            {
                m_rigidbody2D.gravityScale = GRAVITY_IN_AIR;
                m_rigidbody2D.drag = DRAG_IN_AIR;
            }
            else
            {
                m_rigidbody2D.gravityScale = GRAVITY_IN_WATER;
                m_rigidbody2D.drag = DRAG_IN_WATER;
            }
            m_directionVector = transform.right;
        }
        
        protected virtual void MoveSubmarine()
        {
            if (m_isInAir)
            {
                return;
            }
            m_rigidbody2D.velocity = m_directionVector * m_currentMoveSpeed;
        }
        
        protected void RotateSubmarine(RotationType rotationType)
        {
            if (m_currentRotationType == rotationType)
            {
                return;
            }
            m_currentRotationType = rotationType;
            if (m_isInAir)
            {
                m_currentRotationType = m_rigidbody2D.velocity.y > 0f ? RotationType.UP : RotationType.DOWN;
            }
            Vector3 rotateAngle = m_submarineTransform.eulerAngles;
            rotateAngle.z = rotationType == RotationType.UP ? m_submarineRotationData.RotationAngle : -m_submarineRotationData.RotationAngle;
            m_rotationTween?.Kill();
            m_rotationTween = transform.DORotate(rotateAngle, m_submarineRotationData.RotationSpeed).SetSpeedBased(true);
        }
        
        protected abstract void HandelRotation();
        
        public void DisableMovement(bool goDown)
        {
            m_isMovementAllowed = false;
            m_rotationTween?.Kill();
            if (!goDown)
            {
                m_rigidbody2D.velocity = Vector2.zero;
                return;
            }
            m_rigidbody2D.velocity = Vector2.down;
            m_rigidbody2D.gravityScale = GRAVITY_IN_WATER_TO_FALL_DOWN;
        }
        public void EnableMovement()
        {
            m_isMovementAllowed = true;
            m_rigidbody2D.gravityScale = 0;
        }

        public virtual void Eaten()
        {
            m_explosionParticle.Play();
            SurvivalConfig survivalConfig = SurvivalConfig.Instance;
            foreach (Rigidbody2D brokenPartRigidBody in m_brokenPartRigidBodies)
            {
                brokenPartRigidBody.gravityScale = survivalConfig.GravityForSubmarineBrokenPart;
                brokenPartRigidBody.transform
                    .DORotate(survivalConfig.RotationForSubmarineBrokenPart,
                        survivalConfig.RotationDelayForSubmarineBrokenPart, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
            }
            StopAllCoroutines();
            DisableMovement(true);
            m_wholeSubmarine.SetActive(false);
            m_brokenSubmarine.SetActive(true);
            m_defaultTrail.Stop();
            m_angleArrow.enabled = false;
            if (m_targetGlow)
            {
                m_targetGlow.SetActive(false);
            }
        }
    }

