using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerSubmarine : Submarine
    {
        [SerializeField] 
        private ParticleSystem m_turboTrail;
        [SerializeField] 
        private ParticleSystem m_empBlast;
        [Header("Player Config")]
        [SerializeField] 
        private Vector3 m_projectileFireOffset;

        private bool m_isInputDown;
        private bool m_canFireProjectile;
        private ChaseSubmarine m_projectileInstance;
        private bool m_canFireEMP = false;
        private AISubmarine m_targetSubmarine;
        
        public bool IsInputDown => m_isInputDown;

        #region Projectile
        private void FireProjectile()
        {
            if (!m_canFireProjectile)
            {
                return;
            }
            if (Vector2.Distance(SubmarineTransform.position, m_targetSubmarine.SubmarineTransform.position) > SurvivalConfig.Instance.FishnetData.FireDistance)
            {
                return;
            }
            MyGameplayEvents.SendOnPowerUpUsed(PowerUpTypes.FISHNET ,m_targetSubmarine, true);
            m_projectileInstance = Instantiate(m_projectileInstance, GetProjectileFirePosition(), Quaternion.identity);
            m_projectileInstance.Config(m_targetSubmarine);
            m_canFireProjectile = false;
        }

        private Vector3 GetProjectileFirePosition()
        {
            return SubmarineTransform.position + (m_projectileFireOffset.x * transform.right);
        }
        #endregion

        private void FireEMPBlast()
        {
            if (!m_canFireEMP)
            {
                return;
            }
            float empRadius = SurvivalConfig.Instance.EMPRadius;
            if (Vector2.Distance(SubmarineTransform.position, m_targetSubmarine.SubmarineTransform.position) > empRadius)
            {
                return;
            }
            m_empBlast.transform.localScale = new Vector2(empRadius,empRadius);
            m_empBlast.Play();
            m_targetSubmarine.PlayEmpPfx();
            m_targetSubmarine.DisableMovement(true);
            MyGameplayEvents.SendOnPowerUpUsed(PowerUpTypes.EMP ,m_targetSubmarine, true);
            m_canFireEMP = false;
        }
        
        protected override void Init()
        {
            base.Init();
            m_originalSpeed = SurvivalConfig.Instance.PlayerSubmarine.MoveSpeed;
            SetMoveSpeed(m_originalSpeed);
        }

        protected override void SetRotationData()
        {
            m_submarineRotationData = SurvivalConfig.Instance.PlayerSubmarine.SubmarineRotationData;
        }

        protected override void Update()
        {
            base.Update();
            FireProjectile();
            FireEMPBlast();
        }
        
        protected override void HandelRotation()
        {
            m_isInputDown = Input.GetKey(KeyCode.Space);
            if (!m_isInputDown)
            {
                RotateSubmarine(RotationType.DOWN);
                return;
            }
            RotateSubmarine(RotationType.UP);
        }

        public IEnumerator Teleport(Vector2 newPosition, float scaleDuration = 0.5f)
        {
            Vector3 originalScale = m_submarineTransform.localScale;
            m_defaultTrail.Pause();
            yield return transform.DOScale(Vector3.zero, scaleDuration).WaitForCompletion();
            yield return transform.DOMove(newPosition, SurvivalConfig.Instance.TeleportCameraTransitionDelay).WaitForCompletion();
            m_defaultTrail.Play();
            transform.DOScale(originalScale, scaleDuration);
        }

        public void ConfigProjectile(ChaseSubmarine projectile)
        {
            m_projectileInstance = projectile;
            m_canFireProjectile = true;
            m_targetSubmarine = SubmarineManager.Instance.GetTargetSubmarine();
        }

        public void ConfigEMP()
        {
            m_canFireEMP = true;
            m_targetSubmarine = SubmarineManager.Instance.GetTargetSubmarine();
        }
        
        public void SwitchToTurboTrail(float duration)
        {
            m_defaultTrail.Stop();
            m_turboTrail.Play();
            StartCoroutine(CoroutineUtils.Delay(duration, () =>
            {
                m_turboTrail.Stop();
                m_defaultTrail.Play();
            }));
        }
        #if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetProjectileFirePosition(), 0.1f);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(SubmarineTransform.position , SurvivalConfig.Instance.FishnetData.FireDistance);
        }
        
        #endif
    }

