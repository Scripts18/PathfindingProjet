using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathFinding : MonoBehaviour 
{
    //Already Evaluated Nodes
	protected List<Node> closedSet = new List<Node>();

    //Nodes to be evaluated, including start node
    protected List<Node> openSet = new List<Node>();

    public Stack<Node> PathFinding(Node start, Node goal)
    {
        //Debug.Log(goal.transform.position);

	    Node currentNode;

	    openSet.Add(start);

	    start.bestKnownPathCost = 0;

	    start.totalNodePathCost = start.bestKnownPathCost + start.CalculateHeuristic(goal.transform.position);

		ReversePathFinding.CalculateHeuristicReversePathFinding(start, goal);
		start.totalNodePathCost = ReversePathFinding.abstracDist(start, goal);

	    while (openSet.Count != 0)
	    {
	        currentNode = findLowestTotalCost();

	        if (currentNode == goal)
	        {
				goal.timeReservation.Add(-1);
	            return reconstructPath(currentNode);
	        }

	        openSet.Remove(currentNode);
	        closedSet.Add(currentNode);

	        foreach (Node neighbor in currentNode.neighbors)
	        {
	            if (closedSet.Contains(neighbor))
	            {
	                continue;
	            }

	            float tempPathCost = currentNode.bestKnownPathCost + distanceBetween(currentNode, neighbor);

				if(neighbor.IsObstacle() || neighbor.timeReservation.Contains(tempPathCost) || neighbor.isReserved || (this.findHighestTotalCost(neighbor) != tempPathCost && neighbor.timeReservation.Contains(-1)))
				{
					tempPathCost = -1;
				}

	            if (((!openSet.Contains(neighbor) || (tempPathCost < neighbor.bestKnownPathCost)) && tempPathCost != -1))
	            {
	                neighbor.parent = currentNode;
	                neighbor.bestKnownPathCost = tempPathCost;

					ReversePathFinding.CalculateHeuristicReversePathFinding(neighbor, goal);
					neighbor.totalNodePathCost = ReversePathFinding.abstracDist(neighbor, goal);
					neighbor.timeReservation.Add(tempPathCost);

					if (!openSet.Contains(neighbor))
	                {
	                    openSet.Add(neighbor);
	                }
	            }
	        }

	    }


        return new Stack<Node>();
    }

    protected Node findLowestTotalCost()
    {
        Node templowestNodeCost = null;

        foreach (Node node in openSet)
        {
            if (templowestNodeCost == null || templowestNodeCost.totalNodePathCost > node.totalNodePathCost && !node.IsObstacle())
            {
                templowestNodeCost = node;
            }
        }

        return templowestNodeCost;
    }

	protected float findHighestTotalCost(Node currentNode)
	{
		float tempHighestNodeCost = 0f;
		
		foreach (float cost in currentNode.timeReservation)
		{
			if (cost >= tempHighestNodeCost)
			{
				tempHighestNodeCost = cost;
			}
		}
		
		return tempHighestNodeCost;
	}

    protected float distanceBetween(Node startNode, Node goalNode)
    {
		return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }

    protected Stack<Node> reconstructPath(Node currentNode)
    {
        Stack<Node> totalPath = new Stack<Node>();
        Node parent;

        totalPath.Push(null);

        while (currentNode.parent != null)
        {
            totalPath.Push(currentNode);
            parent = currentNode;
            currentNode = currentNode.parent;
            parent.ClearParent();
        }

        return totalPath;
    }
}
