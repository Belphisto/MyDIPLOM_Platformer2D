using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Platformer2D.IInterface;

namespace Platformer2D.Background
{
    public class BackgroundView : MonoBehaviour
    {
        private Image _image;
        private Color _currentColor;
        private float _t = 0f;

        public Image Image {get => _image; set => _image = value;}
        public Color CurrentColor {get => _currentColor; set => _currentColor = value;}
        public float T {get => _t; set => _t = value;}
        private BackgroundControlller controller;
    
        void Start()
        {
            _image = GetComponent<Image>(); 
            _currentColor = _image.color;
            IScoreLevel scoreLevel =
            controller = new BackgroundControlller(new BackgroundModel(), this);
            Hero.OnScoreUpdate += controller.HandleScoreUpdate;
        }
    
        public void ChangeColor(Color newColor)
        {
            _image.color = newColor;
        }
    }
    
}
