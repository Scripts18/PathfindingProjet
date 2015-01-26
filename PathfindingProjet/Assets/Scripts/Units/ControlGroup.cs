﻿using UnityEngine;
using System.Collections.Generic;

abstract class ControlGroup : MonoBehaviour
{
    public abstract void ComputePathfinding();
    public abstract void moveToPosition(Vector2 _position);
    public abstract void moveToPosition(int _x, int _y);
}