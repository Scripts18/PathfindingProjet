using UnityEngine;
using System.Collections.Generic;

public abstract class ControlGroup : MonoBehaviour
{
    public abstract void ComputePathfinding();
    public abstract void moveToPosition(Vector3 _position);
    public abstract void moveToPosition(int _x, int _y);
    public abstract void moveToPosition(GameObject _gameobject);

    protected Vector3 center;

    // Update is called once per frame
    void Update()
    {
        this.calculateCenter();
    }

    protected abstract void calculateCenter();

    public Vector3 getCenter()
    {
        return this.center;
    }
}
