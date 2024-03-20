using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Platform
{
    public class PlatformView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer stateColorless;
        [SerializeField] private SpriteRenderer stateColor;
        private PlatformController controller;

        // Start is called before the first frame update
        void Start()
        {
            PlatformModel model = new PlatformModel(20);
            IScoreUpdate scoreUpdate = PlayerController.Instance;
            controller = new PlatformController(model, this, scoreUpdate);
        }

        private void Awake()
        {
            // ссылка на первый дочерний объект с компонентом SpriteRenderer
            stateColorless = transform.GetChild(0).GetComponent<SpriteRenderer>();

            //ссылка на второй дочерний объект с компонентом SpriteRenderer
            stateColor = transform.GetChild(1).GetComponent<SpriteRenderer>();

            stateColor.gameObject.SetActive(false);
        }

        public void ChangeState()
        {
            stateColor.gameObject.SetActive(true);
            stateColorless.gameObject.SetActive(false);
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
