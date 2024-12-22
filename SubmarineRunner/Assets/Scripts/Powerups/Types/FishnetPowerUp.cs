using UnityEngine;

namespace KWCreative
{
    public class FishnetPowerUp : PowerUp
    {
        [SerializeField] private FishnetProjectile m_fishnetProjectile;

        protected override void UsePowerUp()
        {
            m_playerSubmarine.ConfigProjectile(m_fishnetProjectile);   
        }
    }
}

