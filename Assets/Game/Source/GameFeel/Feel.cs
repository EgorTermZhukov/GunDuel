using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Source
{
    [Serializable]
    public struct IconSprite
    {
        public Icon Icon;
        public Sprite Sprite;
    }

    public enum Icon
    {
        None,
        Health,
        HealthDamaged,
        Damage,
        Defense
    }

    public class Feel : MonoBehaviour
    {
        [SerializeField] private PopupText _basicTextPopup;
        [SerializeField] private List<IconSprite> _iconSprites;
        private void Awake()
        {
            G.feel = this;
        }
        public void CreateBasicPopup(string text,float duration, Vector3 position, Color color, Icon icon)
        {
            var sprite = _iconSprites.Find(x=> x.Icon == icon).Sprite;
            var popup = Instantiate(_basicTextPopup, position, Quaternion.identity) as PopupText;
            popup.StartPop(text, duration, color, sprite);
        }
    }
}