public class TurboPowerUp : PowerUp
    {
        protected override void UsePowerUp()
        {
            base.UsePowerUp();
            m_playerSubmarine.SetMoveSpeed(SurvivalConfig.Instance.TurboSpeed,SurvivalConfig.Instance.TurboActiveDuration);
            m_playerSubmarine.SwitchToTurboTrail(SurvivalConfig.Instance.TurboActiveDuration);
        }
    }

