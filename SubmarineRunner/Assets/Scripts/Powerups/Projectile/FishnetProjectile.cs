using UnityEngine;

    public class FishnetProjectile : ChaseSubmarine
    {
        [SerializeField] private SpriteRenderer m_netSprite;

        protected override void Init()
        {
            base.Init();
            m_chaseSpeed = SurvivalConfig.Instance.FishnetData.ChaseSpeed;
            m_rotationSpeed = SurvivalConfig.Instance.FishnetData.RotationSpeed;
        }

        public override void Config(Submarine targetSubmarine)
        {
            SetTargetSubmarine(targetSubmarine);
        }
        
        protected override void OnHit()
        {
            AISubmarine aiSubmarine = (AISubmarine) m_targetSubmarine;
            aiSubmarine.DisableMovement(true);
            aiSubmarine.ShowNet();
            StopMovement();
            m_netSprite.enabled = false;
        }
        
    }


