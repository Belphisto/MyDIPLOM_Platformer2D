using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Bus
{
    public static Bus Instance {get;} = new Bus();

    public event Action <int> Mess;
    public void Send(int inpt)
    {
        Mess?.Invoke(inpt);
    }
    
}
