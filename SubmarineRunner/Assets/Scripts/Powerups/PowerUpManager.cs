using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoSingleton<PowerUpManager>
{
       [SerializeField] private PowerUpSpawnData[] m_powerUpSpawnDatas;
        [SerializeField] private List<PowerUp> m_powerUpPrefabs;
        [SerializeField] private ParticleSystem m_collectParticle;

        public ParticleSystem CollectParticle => m_collectParticle;
        
        protected override void Init()
        {
            //No use
        }
        private  void Start()
        {
            SpawnPowerUps();
        }

        private void SpawnPowerUps()
        {
            PowerUp powerUpToSpawn;
            foreach (PowerUpSpawnData powerUpSpawnData in m_powerUpSpawnDatas)
            {
                powerUpToSpawn =
                    m_powerUpPrefabs.Find(powerUp => powerUp.PowerUpType == powerUpSpawnData.TypeOfPowerUp);
                powerUpToSpawn = Instantiate(powerUpToSpawn, powerUpSpawnData.PowerUpSpawnPoint);
                powerUpToSpawn.transform.localPosition = Vector3.zero;
            }
        }
}


