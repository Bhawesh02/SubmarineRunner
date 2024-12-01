using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AISubmarine : Submarine
    {
        private const int MAX_STEER = 1;
        private const string SHOW_NET_ANIMATION_BOOL = "ShowNet";

        [SerializeField] 
        private Collider2D m_submarineCollider;
        [SerializeField] 
        private Animator m_animator;
        [SerializeField]
        private AISensor m_topSensor;
        [SerializeField]
        private AISensor m_centerSensor;
        [SerializeField]
        private AISensor m_bottomSensor;
        [SerializeField] 
        private ParticleSystem m_empPfx;
        [SerializeField] 
        private SpriteRenderer m_subSpriteRenderer;
        [SerializeField] 
        private SpriteRenderer m_netSprite;

        private Coroutine m_movementCoroutine;
        private bool m_isSubmarineFlipped;
        private SurvivalConfig.AISubmarineData m_aiSubData;

        public bool IsSubmarineFlipped => m_isSubmarineFlipped;

        protected override void Init()
        {
            base.Init();
            m_aiSubData = SurvivalConfig.Instance.AISubmarine;
            m_originalSpeed = SubmarineManager.Instance.GetAiSubSpeed();
            SetMoveSpeed(m_originalSpeed);
            MyGameplayEvents.OnPowerUpUsed += PlayGlowAnimation;
        }

        protected override void SetRotationData()
        {
            m_submarineRotationData = SurvivalConfig.Instance.AISubmarine.SubmarineRotationData;
        }

        protected override void HandelRotation()
        {
            int steerValue = 0;
            bool isColliding = false;
            if (IsSensorColliding(m_topSensor))
            {
                isColliding = true;
                steerValue -= MAX_STEER;
            }
            if (IsSensorColliding(m_bottomSensor))
            {
                isColliding = true;
                steerValue += MAX_STEER;
            }
            if (IsSensorColliding(m_centerSensor))
            {
                isColliding = true;
                Transform hitObject = m_centerSensor.SensorHit.transform;
                steerValue += (hitObject.position.y < m_submarineTransform.position.y) ? -MAX_STEER : MAX_STEER;
            }
            if (!isColliding)
            {
                m_movementCoroutine ??= StartCoroutine(SwitchRotation());
                return;
            }
            RotateSubmarine(steerValue < 0 ? RotationType.UP : RotationType.DOWN);
        }

        
        private void PlayGlowAnimation(PowerUpTypes powerUpTypes, Submarine targetSubmarine, bool disableMovement)
        {
            if (targetSubmarine != this)
            {
                return;
            }
            m_targetGlow.SetActive(true);
        }
        
        private IEnumerator SwitchRotation()
        {
            RotateSubmarine(m_currentRotationType == RotationType.UP ? RotationType.DOWN : RotationType.UP);
            yield return new WaitForSeconds(Random.Range(m_aiSubData.MinMovementSwitchDelay, m_aiSubData.MaxMovementSwitchDelay));
            m_movementCoroutine = null;
        }

        private bool IsSensorColliding(AISensor aiSensor)
        {
            float rotationAngel = Mathf.Deg2Rad * m_submarineTransform.eulerAngles.z;
            Vector2 sensorOffset = new(
                aiSensor.SensorOffset.x * Mathf.Cos(rotationAngel) - aiSensor.SensorOffset.y * Mathf.Sin(rotationAngel),
                aiSensor.SensorOffset.x * Mathf.Sin(rotationAngel) + aiSensor.SensorOffset.y * Mathf.Cos(rotationAngel)
            );
            Vector2 origin = (Vector2)m_submarineTransform.position + sensorOffset;
            aiSensor.SensorHit = Physics2D.Raycast(origin, m_directionVector, aiSensor.SensorLenght,m_aiSubData.ObstacleLayer);
            if (!aiSensor.SensorHit.collider && aiSensor.SensorHit.collider != m_submarineCollider)
            {
                Vector2 endPoint = origin;
                endPoint += (Vector2) (aiSensor.SensorLenght * m_directionVector);
                Debug.DrawLine(origin, endPoint, Color.green);
                return false;
            }
            Debug.DrawLine(origin, aiSensor.SensorHit.point, Color.red);
            return true;
        }
        
        public void DisableMovement(float duration)
        {
            DisableMovement(true);
            StartCoroutine(CoroutineUtils.Delay(duration,EnableMovement));
        }
        
        public void FlipSubmarine()
        {
            if (m_isSubmarineFlipped)
            {
                return;
            }
            float spriteBlinkTime = SurvivalConfig.Instance.SubmarineSpriteBlinkDelay;
            m_submarineTransform.DORotate(SurvivalConfig.Instance.SubmarineFlipRotation, SurvivalConfig.Instance.SubmarineFlipSpeed)
                .OnStart(() =>
                {
                    DisableMovement(false);
                })
                .OnUpdate(
                () =>
                {
                    spriteBlinkTime -= Time.deltaTime;
                    if (spriteBlinkTime > 0f)
                    {
                        return;
                    }
                    m_subSpriteRenderer.enabled = !m_subSpriteRenderer.enabled;
                    spriteBlinkTime = SurvivalConfig.Instance.SubmarineSpriteBlinkDelay;
                }).OnComplete(() =>
            {
                m_subSpriteRenderer.enabled = true;
                EnableMovement();
            });
            m_isSubmarineFlipped = true;
        }

        public void ShowNet()
        {
            m_animator.SetBool(SHOW_NET_ANIMATION_BOOL, true);
        }
        
        public void PlayEmpPfx()
        {
            StartCoroutine(CoroutineUtils.Delay(SurvivalConfig.Instance.EMPPfxStartDelay, () =>
            {
                m_empPfx.Play();
            }));
        }

        public override void Eaten()
        {
            base.Eaten();
            m_netSprite.enabled = false;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }

            Gizmos.color = Color.green;
            Vector2 submarinePosition = (Vector2)transform.position;
            Vector2 censorStartPoint = submarinePosition  + m_topSensor.SensorOffset ;
            Vector2 censorEndPoint = censorStartPoint;
            censorEndPoint.x += m_topSensor.SensorLenght;
            Gizmos.DrawLine(censorStartPoint,censorEndPoint);
            censorStartPoint = submarinePosition + m_centerSensor.SensorOffset;
            censorEndPoint = censorStartPoint;
            censorEndPoint.x += m_centerSensor.SensorLenght;
            Gizmos.DrawLine(censorStartPoint,censorEndPoint);
            censorStartPoint = submarinePosition + m_bottomSensor.SensorOffset;
            censorEndPoint = censorStartPoint;
            censorEndPoint.x += m_bottomSensor.SensorLenght;
            Gizmos.DrawLine(censorStartPoint,censorEndPoint);
        }
#endif
    }

