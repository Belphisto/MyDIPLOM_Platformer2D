using System.Collections;
using System.Collections.Generic;
using Platformer2D.Player;
using UnityEngine;

namespace Platformer2D
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void Awake()
        {
            if (!player)
                player = FindAnyObjectByType<PlayerView>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            pos = player.position;
            pos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime); //
        }
    }
}

