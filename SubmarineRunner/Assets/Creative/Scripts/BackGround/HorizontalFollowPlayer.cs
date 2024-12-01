using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KWCreative
{
    public class HorizontalFollowPlayer : MonoBehaviour
    {
        [SerializeField] private Transform m_playerTransform;
        private void Update()
        {
            if(!m_playerTransform)
                return;
            Vector3 targetPosition = transform.position;
            targetPosition.x = m_playerTransform.position.x;
            transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
        }
    }
}

