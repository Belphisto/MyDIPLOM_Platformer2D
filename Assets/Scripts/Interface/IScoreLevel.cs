using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.IInterface
{
    public interface IScoreLevel 
    {
        int CurrentScore {get;}
        int TotalScore {get;}
    }

}
