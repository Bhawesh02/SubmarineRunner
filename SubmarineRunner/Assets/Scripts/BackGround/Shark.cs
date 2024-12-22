
using UnityEngine;
public class Shark : ChaseSubmarine
    {
        private const string ANIM_SHARK_BITE = "Shark_Bite";
        private const string ANIM_SHARK_HURT = "Shark Hurt";
        
        [SerializeField] private Animator m_animator;

        private bool m_biteAnimPlayed;
        private SurvivalConfig m_survivalConfig;
        
        protected override void Init()
        {
            base.Init();
            m_survivalConfig = SurvivalConfig.Instance;
            SetChaseSpeed(m_survivalConfig.SharkSpeed);
            m_rotationSpeed = m_survivalConfig.SharkRotationSpeed;
            m_speedRecovery = m_survivalConfig.SharkSpeedRecoveryAfterSpikeCollision;
            MyGameplayEvents.OnPowerUpUsed += HandelOnPickupUsed;
        }

        private void HandelOnPickupUsed(PowerUpTypes powerUpTypes, Submarine obj, bool disableMovement)
        {
            SetChaseSpeed(m_chaseSpeed + m_survivalConfig.SharkSpeedIncreaseOnPickupCollected);
            if (!disableMovement)
            {
                return;
            }
            StopMovement();
            StartCoroutine(CoroutineUtils.Delay(SubmarineManager.Instance.SubmarineSpeedDeclineDelay, () =>
            {
                m_isMovementPause = false;
            }));
        }

        private void SetChaseSpeed(float newSpeed)
        {
            m_chaseSpeed = newSpeed;
        }
        
        protected override void Update()
        {
            SetTargetSubmarine(SubmarineManager.Instance.GetClosestSubmarine(m_transform.position));
            base.Update();
            if (m_biteAnimPlayed ||
                !(m_distanceToTargetSub <= SurvivalConfig.Instance.SharkDistanceToPlayBiteAnimation))
            {
                return;
            }

            m_biteAnimPlayed = true;
            m_animator.Play(ANIM_SHARK_BITE);
            StartCoroutine(CoroutineUtils.Delay(1f, () =>
            {
                m_biteAnimPlayed = false;
            }));
        }

        protected override void OnHit()
        {
             m_targetSubmarine.Eaten();
             float originalSpeed = m_chaseSpeed;
             SetChaseSpeed(m_survivalConfig.SharkSpeedAfterSubEaten);
             StartCoroutine(CoroutineUtils.Delay(m_survivalConfig.SharkSpeedDecreaseDelay, ()=>
             {
                 SetChaseSpeed(originalSpeed);
             }));
        }

        public void SpikeHit(Vector2 hitPosition)
        {
            m_animator.Play(ANIM_SHARK_HURT);
            m_hitDirection = (Vector2)transform.position - hitPosition;
            m_hitDirection = m_hitDirection.normalized * SurvivalConfig.Instance.SharkPushbackForceOnSpikeCollision;
        }
    }
