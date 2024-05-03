using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Bus
{
    public static Bus Instance {get;} = new Bus();

    public event Action <int> SendScore;

    public void Send(int inpt)
    {
        SendScore?.Invoke(inpt);
    }

    public event Action <int> SendBackgroundScore;
    public void SendBackground(int inpt)
    {
        SendBackgroundScore?.Invoke(inpt);
    }
    
    //Событие для инертирования управления
    public event Action InvertControls;
    public void SendInvertControls()
    {
        InvertControls?.Invoke();
    }

    public event Action <int, LocationType> UpdateCrystal;
    public void SendCrystal(int count, LocationType type)
    {
        UpdateCrystal?.Invoke(count, type);
    }

    public event Action <int> UdateTotalScore;
    public void SendAllScore(int count)
    {
        UdateTotalScore?.Invoke(count);
    }

    public event Action <int> UdateLevel;
    public void SendLevelPercent(int count)
    {
        UdateLevel?.Invoke(count);
    }
}
