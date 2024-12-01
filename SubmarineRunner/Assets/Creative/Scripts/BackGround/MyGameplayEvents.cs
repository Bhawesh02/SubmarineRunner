using System;
public partial class MyGameplayEvents
{
    public static event Action<PowerUpTypes, Submarine, bool> OnPowerUpUsed;

    public static void SendOnPowerUpUsed(PowerUpTypes powerUpTypes,Submarine targetSubmarine, bool disableMovement)
    {
        OnPowerUpUsed?.Invoke(powerUpTypes, targetSubmarine, disableMovement);
    }
}
