using System;
using UnityEngine;

namespace Game.Source
{
    public class Feel : MonoBehaviour
    {
        [SerializeField] private PopupText _basicTextPopup;
        private void Awake()
        {
            G.feel = this;
        }
        public void CreateBasicPopup(Vector3 position, string text, Color color)
        {
            var popup = Instantiate(_basicTextPopup, position, Quaternion.identity);
            popup.popupText.text = text;
            popup.popupText.color = color;
        }
    }
}