using System;
using UnityEngine;

namespace KWCreative
{
    public class EnvironmentLoopHandler : MonoBehaviour
    {
        [SerializeField] private Transform[] m_environmentTransforms;
        [SerializeField] private float m_distanceThreshold = 28.125f;
        [SerializeField] private float m_offsetToMoveBy = 56.25f;
        [SerializeField] private Transform m_playerTransform;
        [SerializeField] private Rigidbody2D m_playerRigidbody;
        
        private void Update()
        {
            if (!m_playerTransform)
                return;

            int playerMovingDirection = Math.Sign(m_playerRigidbody.velocity.x);
            if (playerMovingDirection == 0)
            {
                return;
            }
            float playerPositionX = m_playerTransform.position.x;
            foreach (Transform environmentTransform in m_environmentTransforms)
            {
                float signedDistanceFromPlayer = environmentTransform.position.x - playerPositionX;

                if (Mathf.Abs(signedDistanceFromPlayer) > m_distanceThreshold &&
                    Math.Sign(signedDistanceFromPlayer) != playerMovingDirection) 
                {
                    RepositionTransform(environmentTransform, playerMovingDirection);
                }
            }
        }
        
        private void RepositionTransform(Transform environmentTransform, float direction)
        {
            Vector3 targetPosition = environmentTransform.position;
            targetPosition.x += direction*m_offsetToMoveBy;
            environmentTransform.position = targetPosition;
        }
    }
}

