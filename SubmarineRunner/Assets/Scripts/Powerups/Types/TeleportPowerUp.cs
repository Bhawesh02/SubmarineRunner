using System;
using UnityEngine;

namespace KWCreative
{
    public class TeleportPowerUp : PowerUp
    {
        [SerializeField] private GameObject m_teleportVisual;

        private bool m_isUsed;
        
        protected override void UsePowerUp()
        {
            if (m_isUsed)
            {
                return;
            }
            m_isUsed = true;
            base.UsePowerUp();
            AISubmarine targetSubmarine = SubmarineManager.Instance.GetTargetSubmarine();
            m_targetSubmarine = targetSubmarine;
            Vector2 newPosition = targetSubmarine.SubmarineTransform.position;
            newPosition.x += SurvivalConfig.Instance.TeleportDistance;
            Instantiate(m_teleportVisual, newPosition, Quaternion.identity, transform);
            m_playerSubmarine.StartCoroutine(
                m_playerSubmarine.Teleport(newPosition,SurvivalConfig.Instance.TeleportScaleDuration));
        }
    }
}

