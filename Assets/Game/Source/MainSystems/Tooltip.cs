using TMPro;
using UnityEngine;

namespace Game.Source
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private TMP_Text _description;

        [SerializeField]
        GameObject _holder;
        
        public void Show(string itemName, string description)
        {
            var mousePos = Input.mousePosition;
            var position = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            _name.text = itemName;
            _description.text = description;
            _holder.SetActive(true);
        }
        public void Hide()
        {
            _holder.SetActive(false);
        }
    }
}