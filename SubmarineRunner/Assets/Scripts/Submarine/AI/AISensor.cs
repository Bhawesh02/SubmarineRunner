using System;
using UnityEngine;

[Serializable]
    public class AISensor
    {
        [SerializeField] private Vector2 m_sensorOffset;
        [SerializeField] private float m_sensorLenght;
        private RaycastHit2D m_sensorHit;

        public Vector2 SensorOffset => m_sensorOffset;
        public float SensorLenght => m_sensorLenght;
        
        public RaycastHit2D SensorHit
        {
            get { return m_sensorHit; }
            set { m_sensorHit = value; }
        }
    } 

