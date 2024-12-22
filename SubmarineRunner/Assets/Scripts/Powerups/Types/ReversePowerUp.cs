using UnityEngine;

namespace KWCreative
{
    public class ReversePowerUp : PowerUp
    {
        [SerializeField] private ReverseControlProjectile m_projectile;
        protected override void UsePowerUp()
        {
            m_playerSubmarine.ConfigProjectile(m_projectile);
        }
    }
}

