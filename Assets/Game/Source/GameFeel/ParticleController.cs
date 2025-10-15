using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Source
{
    [Serializable]
    public struct ParticleT
    {
        public ParticleType ParticleType;
        public GameObject ParticlePfb;
    }
    public enum ParticleType
    {
        Slash,
        Shoot
    }
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] public List<ParticleT> Particles = new List<ParticleT>();
        [SerializeField] public GameObject FollowParticle;
        private void Awake()
        {
            G.ParticleController = this;
        }

        public void Spawn(Vector3 position, ParticleType particleType)
        {
            var particle = Particles.Find(x => x.ParticleType == particleType).ParticlePfb;
            var particleGO = Instantiate(particle, position, Quaternion.identity);
            var dirDifference = transform.position.x - position.x;

            var scale = particleGO.transform.localScale;
            if (dirDifference < 0)
            {
                particleGO.transform.localScale = new Vector3(-1f * scale.x, scale.y * 1f, scale.z * 1f);
            }
        }

        public void SpawnAndMoveToWithDuration(float duration, Vector3 startingPosition, Vector3 endPosition)
        {
            StartCoroutine(SequenceRoutine(duration, startingPosition, endPosition));
        }
        public IEnumerator SequenceRoutine(float duration, Vector3 startingPosition, Vector3 endPosition)
        {
            var particle = Instantiate(FollowParticle, startingPosition, Quaternion.identity);
            particle.transform.DOMove(endPosition, duration);
            yield return new WaitUntil(G.Ticker.CreatePr(duration));
            Destroy(particle.gameObject);
        }
    }
}