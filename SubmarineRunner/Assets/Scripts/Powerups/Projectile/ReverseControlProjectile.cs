
using UnityEngine;


    public class ReverseControlProjectile : ChaseSubmarine
    {
        [SerializeField] private GameObject m_projectileVisual;

        protected override void Init()
        {
            base.Init();
            m_chaseSpeed = SurvivalConfig.Instance.ReverseControlProjectileData.ChaseSpeed;
            m_rotationSpeed = SurvivalConfig.Instance.ReverseControlProjectileData.RotationSpeed;
        }

        public override void Config(Submarine targetSubmarine)
        {
            SetTargetSubmarine(targetSubmarine);
        }
        
        protected override void OnHit()
        {
            AISubmarine aiSubmarine = (AISubmarine) m_targetSubmarine;
            aiSubmarine.FlipSubmarine();
            StopMovement();
            m_projectileVisual.SetActive(false);
        }

    }

