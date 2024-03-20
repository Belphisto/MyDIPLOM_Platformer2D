using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using UnityEngine.Animations;

namespace Platformer2D.Level
{
    public class LevelController
    {
        private LevelModel model;
        private LevelView view;
        public LevelController(LevelModel model)
        {
            this.model = model;
            SpawnCrystals();
        }

        public LevelController(LevelModel model, LevelView view)
        {
            this.model = model;
            this.view = view;
            SpawnCrystals();
            SpawnColorChangeBackground();
        }

        private void SpawnCrystals()
        {
            int scorePerCrystal = model.TotalScore / model.CrystalCount;
            foreach (var position in model.Positions)
            {
                var crystal = Object.Instantiate(model.СrystalPrefab, position, Quaternion.identity);
                var crystalModel = new CrystalModel { Score = scorePerCrystal };
                new CrystalController(crystalModel, crystal);
            }
        }
        private void SpawnColorChangeBackground()
        {
            var colorChangeBackground = Object.Instantiate(model.BackgroundPrefab);
            new BackgroundControlller(new BackgroundModel(), colorChangeBackground);
        }

        public void HandleScoreUpdate(int score)
        {
            model.IncrementScore(score);
            
        }
    }
}

