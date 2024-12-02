using System;
using UnityEngine;
public abstract class PowerUp : MonoBehaviour
    {
        [SerializeField] protected PowerUpTypes m_powerUpType;
        [SerializeField] protected bool m_canDisable = true;
        [SerializeField] protected bool m_canPlayCollectionParticle = true;
        
        [SerializeField] private bool m_disableMovementOnUse = true;
        
        protected PlayerSubmarine m_playerSubmarine;
        protected Submarine m_targetSubmarine;

        public PowerUpTypes PowerUpType
        {
            get { return m_powerUpType; }
        }

        protected virtual void UsePowerUp()
        {
            MyGameplayEvents.SendOnPowerUpUsed(m_powerUpType, m_targetSubmarine, m_disableMovementOnUse);
        }
        
        private void Awake()
        {
            m_playerSubmarine = SubmarineManager.Instance.PlayerSubmarine;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_playerSubmarine.gameObject != other.gameObject)
            {
                return;
            }

            CollectPickup();
        }

        private void CollectPickup()
        {
            m_targetSubmarine = m_playerSubmarine;
            ParticleSystem collectParticle = PowerUpManager.Instance.CollectParticle;
            if (m_canPlayCollectionParticle)
            {
                collectParticle.Stop();
                collectParticle.transform.position = transform.position;
                collectParticle.Play();
            }
            float speedIncreaseOnPickupCollected = SurvivalConfig.Instance.PlayerSubmarine.PlayerSpeedIncreaseOnPickupCollected;
            m_playerSubmarine.SetMoveSpeed(m_playerSubmarine.CurrentSpeed + speedIncreaseOnPickupCollected
                                            , true);
            UsePowerUp();
            if (m_canDisable)
            {
                gameObject.SetActive(false);
            }
        }
    }
