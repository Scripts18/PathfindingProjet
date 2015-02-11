using UnityEngine;
using System.Collections.Generic;

public abstract class ControlGroup : MonoBehaviour
{
    public abstract void ComputePathfinding();
    public abstract void moveToPosition(Vector3 _position);
    public abstract void moveToPosition(int _x, int _y);
    public abstract void moveToPosition(GameObject _gameobject);
    public abstract void changePath(Vector3 newOrder);
    public abstract void queuePath(Vector3 newOrder);


    public Vector3 offsetFromCenter;
}
