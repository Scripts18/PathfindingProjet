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
        Debug.Log(goal.transform.position);

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

				if(neighbor.IsObstacle())
				{
					tempPathCost = -1;
				}

                if ((!openSet.Contains(neighbor) || (tempPathCost < neighbor.bestKnownPathCost && tempPathCost != -1)) && tempPathCost != currentNode.timeReservation.BinarySearch(tempPathCost) && !currentNode.isUnitWaiting)
                {
                    neighbor.parent = currentNode;
                    neighbor.bestKnownPathCost = tempPathCost;

					ReversePathFinding.CalculateHeuristicReversePathFinding(neighbor, goal);
					neighbor.totalNodePathCost = ReversePathFinding.abstracDist(neighbor, goal);

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
            if (templowestNodeCost == null || templowestNodeCost.totalNodePathCost > node.totalNodePathCost)
            {
                templowestNodeCost = node;
            }
        }

        return templowestNodeCost;
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
