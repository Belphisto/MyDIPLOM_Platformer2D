using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class PlatformController 
    {
        private PlatformModel model;
        private PlatformView view;
        
        public PlatformController(PlatformModel model, PlatformView view, IScoreUpdate scoreUpdate)
        {
            this.model = model;
            this.view = view;
            scoreUpdate.OnScoreUpdate += HandleScoreUpdate; // Подписка на событие обновления счета
        }
        
        private void HandleScoreUpdate(int score)
        {
            if (score >= model.TargetScore) view.ChangeState();
        }
    }

}
