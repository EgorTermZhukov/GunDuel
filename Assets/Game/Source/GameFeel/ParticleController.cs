using System;
using System.Collections.Generic;
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
    }
}