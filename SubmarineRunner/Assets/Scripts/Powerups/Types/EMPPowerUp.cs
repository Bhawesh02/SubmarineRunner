public class EMPPowerUp : PowerUp
{
    protected override void UsePowerUp()
    {
        m_playerSubmarine.ConfigEMP();
    } 
}