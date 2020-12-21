using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MovementOrder : Command
{
    public Vector2 Destination;
    public float Tolerance;

    public MovementOrder(Vector2 destination, float tolerance)
    {
        Destination = destination;
        Tolerance = tolerance;
    }
}
