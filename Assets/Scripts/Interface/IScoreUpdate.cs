using System;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.IInterface
{
    public interface IScoreUpdate 
    {
        event Action<int> OnScoreUpdate;
    }
}

