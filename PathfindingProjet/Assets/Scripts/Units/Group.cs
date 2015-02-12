using UnityEngine;
using System.Collections.Generic;

public class Group : ControlGroup 
{
    [SerializeField]private List<ControlGroup> listUnits = new List<ControlGroup>();

    [SerializeField]private GroupPathFinding pathFinding;
    [SerializeField]private Map currentMap;

    private Stack<Node> movementOrders = new Stack<Node>();

    public override void ComputePathfinding()
    {

    }

    public void SetMap(Map _map)
    {
        this.currentMap = _map;
    }

    private void calculateCenter()
    {
        Vector3 sum = new Vector3(0, 0, 0);

        foreach (ControlGroup controlGroup in this.listUnits)
        {
            sum += controlGroup.transform.position;
        }

        if (this.listUnits.Count > 0)
        {
            Vector3 center = sum / this.listUnits.Count;
            this.transform.position = new Vector3((int)center.x, (int)center.y, 0);
        }
    }

    public void SetSquareFormation(int _range)
    {

        this.calculateCenter();

        Vector3 positionInFormation = new Vector3(-1, 1, 0);
        Vector3 formationOffset = new Vector3(1, -1, 0);
        Vector3 magicMultiplicator = new Vector3(-1, -1, 0);

        if (_range < 1)
        {
            _range = 1;
        }

        positionInFormation *= _range;

        for (int i = 0; i < 4; ++i)
        {
            this.listUnits[i].moveToPosition((this.transform.position + positionInFormation));
            this.listUnits[i].offsetFromCenter = positionInFormation;



            positionInFormation = Vector3.Scale(positionInFormation, formationOffset);
            formationOffset = Vector3.Scale(formationOffset, magicMultiplicator);
        }   
    }

    public void SetCircleFormation()
    {
        this.calculateCenter();

        float angle = 360.0f / this.listUnits.Count;
        float currentAngle = 0;
        float radiansAngle = 0;

        float constRadians = (Mathf.PI / 180);

        int posX = 0;
        int posY = 0;

        int radius = (this.listUnits.Count / 4) + 1;

        List<Vector3> listPosition = new List<Vector3>();


        for (int i = 0; i < this.listUnits.Count; ++i)
        {
            radiansAngle = currentAngle * constRadians;

            posX = (int)((float)(System.Math.Cos(radiansAngle) * radius));
            posY = (int)((float)(System.Math.Sin(radiansAngle) * radius));
            Vector3 positionInFormation = new Vector3(posX, posY, 0);

            listPosition.Add(positionInFormation + this.transform.position);

            currentAngle += angle;
        }

        List<ControlGroup> tempList = new List<ControlGroup>();

        foreach(ControlGroup controlGroup in this.listUnits)
        {
            Vector3 tempPosition = this.getClosestPointToUnit(controlGroup, listPosition);
            listPosition.Remove(tempPosition);
            controlGroup.moveToPosition(tempPosition);
            controlGroup.offsetFromCenter = (tempPosition - this.transform.position);
        }
    }

    private Vector3 getClosestPointToUnit(ControlGroup _unit, List<Vector3> _points)
    {
        Vector3 selectedPoint = Vector3.zero;
        double distance = 2048;
        double tempDistance = 0;
        foreach (Vector3 vector in _points)
        {
            tempDistance = Vector3.Distance(_unit.transform.position, vector);
            if (tempDistance < distance)
            {
                selectedPoint = vector;
                distance = tempDistance;
            }
        }

        return selectedPoint;
    }

    public void SetLineFormation(bool _isVertical)
    {
        this.calculateCenter();

        int numberUnits = this.listUnits.Count;

        Vector3 formationOffset = new Vector3(0, 0, 0);
        Vector3 positionInFormation = new Vector3(0, 0, 0);
        if (_isVertical)
        {
            formationOffset.y += 1;
        }
        else
        {
            formationOffset.x += 1;
        }

        int half = numberUnits / 2;

        positionInFormation = -half * formationOffset;

        for (int i = 0; i < numberUnits; ++i)
        {
            this.listUnits[i].moveToPosition((this.transform.position + positionInFormation));
            positionInFormation += formationOffset;
            this.listUnits[i].offsetFromCenter = positionInFormation;
        }
    }

    public void CancelAnyFormation()
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {       
            controlGroup.offsetFromCenter = Vector3.zero;
        }
    }

    public void AddUnit(ControlGroup _unit)
    {
        this.listUnits.Add(_unit);
    }

    public void Remove(ControlGroup _unit)
    {
        this.listUnits.Remove(_unit);
    }

    public override void moveToPosition(Vector3 _position)
    {
        moveAllUnitsTo(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        moveAllUnitsTo(new Vector3(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        moveAllUnitsTo(_gameobject.transform.position);
    }

    private void moveAllUnitsTo(Vector3 _position)
    {
        if (_position.x < 0)
        {
            _position.Set(0, _position.y, 0);
        }
        else if (_position.x > (this.currentMap.getMapSize().x - 1))
        {
            _position.Set((this.currentMap.getMapSize().x - 1), _position.y, 0);
        }
        if (_position.y < 0)
        {
            _position.Set(_position.x, 0, 0);
        }
        else if (_position.y > (this.currentMap.getMapSize().y - 1))
        {
            _position.Set(_position.x, (this.currentMap.getMapSize().y - 1), 0);
        }

        this.movementOrders = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)_position.x][(int)_position.y].GetComponent<Node>());
        
        int stackCount = 0;

        if (this.movementOrders != null)
        {
            stackCount = this.movementOrders.Count;
        }

        int numberMoves = (int)Mathf.Sqrt(stackCount) + 1;

        Debug.Log("Move group with " + (stackCount / numberMoves) + " moves.");
        for (int i = 0; i < stackCount; ++i)
        {
            Node target = this.movementOrders.Pop();
            if (i % numberMoves == 0 && target != null)
            {
                foreach (ControlGroup controlGroup in this.listUnits)
                {
                    controlGroup.queuePath(target.transform.position + controlGroup.offsetFromCenter);
                }
            }
        }
    }

    public override void changePath(Vector3 newOrder)
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.changePath(newOrder);
        }
    }

    public override void queuePath(Vector3 _newOrder)
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.queuePath(_newOrder);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	}
}
