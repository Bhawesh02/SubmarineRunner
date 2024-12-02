using System.Collections;
using UnityEngine;

    public class SubmarineManager : MonoSingleton<SubmarineManager>
    {
        private const float CONVERT_TO_PERCENTAGE = 0.01f;
        
        [SerializeField] private PlayerSubmarine m_playerSubmarine;
        [SerializeField] private AISubmarine[] m_aiSubmarines;
        [SerializeField] private float m_submarineSpeedDeclineDelay = 1.5f;
        
        private Submarine m_closestSubmarine;
        private float m_closestDistance;
        private int m_aiSubIndex;
        private SurvivalConfig m_survivalConfig;
        
        public PlayerSubmarine PlayerSubmarine => m_playerSubmarine;
        public float SubmarineSpeedDeclineDelay => m_submarineSpeedDeclineDelay;

        protected override void Init()
        {
            m_aiSubIndex = 0;
            m_survivalConfig = SurvivalConfig.Instance;
            MyGameplayEvents.OnPowerUpUsed += HandelOnPickupCollected;
        }

        public Submarine GetClosestSubmarine(Vector2 currentPosition, bool isPlayer = false)
        {
            m_closestSubmarine = null;
            m_closestDistance = float.PositiveInfinity;
            foreach (AISubmarine aiSubmarine in m_aiSubmarines)
            {
                CheckForClosestSubmarine(currentPosition, aiSubmarine);
            }
            if (!isPlayer)
            {
                CheckForClosestSubmarine(currentPosition, m_playerSubmarine);
            }
            return m_closestSubmarine;
        }

        private void CheckForClosestSubmarine(Vector2 currentPosition, Submarine targetSubmarine)
        {
            Vector2 targetSubmarinePosition = targetSubmarine.transform.position;
            if (targetSubmarinePosition.x < currentPosition.x)
            {
                return;
            }
            float submarineDistance = targetSubmarinePosition.x - currentPosition.x;
            if (submarineDistance < m_closestDistance)
            {
                m_closestDistance = submarineDistance;
                m_closestSubmarine = targetSubmarine;
            }
        }

        private void HandelOnPickupCollected(PowerUpTypes powerUpType, Submarine targetSubmarine, bool decreaseSpeed)
        {
            if (!decreaseSpeed)
            {
                return;
            }
            float subSpeed;
            subSpeed = (m_survivalConfig.PlayerSubmarine.PlayerSpeedPercentageOnPickupUse * m_playerSubmarine.CurrentSpeed * CONVERT_TO_PERCENTAGE);
            m_playerSubmarine.SetMoveSpeed(subSpeed, m_submarineSpeedDeclineDelay); 
            foreach (AISubmarine aiSubmarine in m_aiSubmarines)
            {
                subSpeed = (m_survivalConfig.AISubmarine.AISpeedPercentangeOnPickupUse * aiSubmarine.CurrentSpeed * CONVERT_TO_PERCENTAGE);
                aiSubmarine.SetMoveSpeed(subSpeed, m_submarineSpeedDeclineDelay);
            }
        }
        
        public AISubmarine GetTargetSubmarine()
        {
            Vector2 playerPosition = m_playerSubmarine.SubmarineTransform.position;
            AISubmarine targetSub = (AISubmarine)GetClosestSubmarine(playerPosition, true);
            return targetSub;
        }

        public float GetAiSubSpeed()
        {
            m_aiSubIndex++;
            SurvivalConfig.AISubmarineData aiSubData= m_survivalConfig.AISubmarine;
            return aiSubData.MinMoveSpeed + (aiSubData.MoveSpeedIncrement * m_aiSubIndex);
        }
    }

