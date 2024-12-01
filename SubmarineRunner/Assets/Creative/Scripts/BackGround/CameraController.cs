using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup m_cinemachineTargetGroup;
        [SerializeField] private Vector2 m_cameraTargetWeightRadius;
        
        private void Awake()
        {
            MyGameplayEvents.OnPowerUpUsed += HandelOnPowerUpUsed;
        }

        private void HandelOnPowerUpUsed(PowerUpTypes powerUpType ,Submarine submarine, bool disableMovement)
        {
            if (submarine == SubmarineManager.Instance.PlayerSubmarine)
            {
                return;
            }
            AddMemberToTargetGroup(submarine.SubmarineTransform, SubmarineManager.Instance.SubmarineSpeedDeclineDelay);
        }
        
        private void AddMemberToTargetGroup(Transform newMemberTransform, float removeDelay)
        {
            if (m_cinemachineTargetGroup.FindMember(newMemberTransform) >= 0)
            {
                return;
            }
            m_cinemachineTargetGroup.AddMember(newMemberTransform,m_cameraTargetWeightRadius.x,m_cameraTargetWeightRadius.y);
            StartCoroutine(
                CoroutineUtils.Delay(removeDelay, () =>
                {
                    m_cinemachineTargetGroup.RemoveMember(newMemberTransform);
                }));
        }
}

