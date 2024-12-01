using System;
using DG.Tweening;
using KWCreative;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spike : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spikeSprite;
    [SerializeField] private ParticleSystem m_spikeExplosionPfx;
    [SerializeField] private Collider2D m_collider2D;
    [SerializeField] private Vector3 m_spikeEndPos;
    [SerializeField] private float m_movementDelayMin;
    [SerializeField] private float m_movementDelayMax;
    
    private void Start()
    {
        int randomMultiplier = Random.Range(0, 2) == 0 ? -1 : 1;
        transform.DOMove(transform.position + m_spikeEndPos * randomMultiplier, Random.Range(m_movementDelayMin, m_movementDelayMax))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Shark shark = other.GetComponent<Shark>();
        if (!shark)
        {
            return;
        }
        m_spikeSprite.enabled = false;
        m_spikeExplosionPfx.Play();
        shark.SpikeHit(other.ClosestPoint(shark.transform.position));
        m_collider2D.enabled = false;
    }
}
