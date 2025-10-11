using TMPro;
using UnityEngine;

namespace Game.Source
{
    public class PopupText : MonoBehaviour
    {
        public TMP_Text popupText;
        public float LifeTime;
        public void Start()
        {
        }
        public void Update()
        {
            LifeTime -= Time.deltaTime;
            if(LifeTime <= 0)
                Destroy(gameObject);
        }
    }
}