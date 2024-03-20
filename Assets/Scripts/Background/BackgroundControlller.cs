using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;

namespace Platformer2D.Background
{
    public class BackgroundControlller
    {
        private BackgroundModel model;
        private BackgroundView view;

        private IScoreLevel leveltarget;
    
        public BackgroundControlller(BackgroundModel model, BackgroundView view, IScoreLevel scoreLevel)
        {
            this.model = model;
            this.view = view;
            this.leveltarget = scoreLevel;

        }
    
        public void HandleScoreUpdate(int score)
        {
            float progress = (float)score /leveltarget.TotalScore;
    
            if (progress >= 1f)
            {
                progress = 1f;
            }
    
            view.T = progress;
    
            Color newColor = new Color(
                Mathf.Lerp(view.CurrentColor.r, model.TargetColor.r, view.T),
                Mathf.Lerp(view.CurrentColor.g, model.TargetColor.g, view.T),
                Mathf.Lerp(view.CurrentColor.b, model.TargetColor.b, view.T),
                view.CurrentColor.a
            );
    
            view.ChangeColor(newColor);
        }
    }
    
}
