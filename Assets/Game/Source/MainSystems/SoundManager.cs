using System;
using UnityEngine;

namespace Game.Source
{
    public class SoundManager : MonoBehaviour
    {
        private void Awake()
        {
            G.soundManager = this;
        }
    }
}