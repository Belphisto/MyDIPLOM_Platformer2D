using System.Collections;
using System.Collections.Generic;
using Platformer2D.Player;
using UnityEngine;
using TMPro;

namespace Platformer2D
{
    public class CameraManager : MonoBehaviour
    {
        
        [SerializeField] private Transform player;
        public TextMeshProUGUI textHelp;
        private Vector3 pos;

        public static CameraManager Instance {get;private set;}
        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (!player)
                player = FindAnyObjectByType<PlayerView>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            pos = player.position;
            pos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime); //

            Vector3 labelPos = Camera.main.WorldToScreenPoint(player.position + Vector3.up); 
            textHelp.transform.position = labelPos;
        }

        public void UpadteText(string text)
        {
            textHelp.text = text;
        }
        public void SetActive(bool active)
        {
            textHelp.gameObject.SetActive(active);
        }
    }
}

