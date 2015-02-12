﻿using UnityEngine;
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
        closedSet.Clear();
        openSet.Clear();

	    Node currentNode;

	    openSet.Add(start);

	    start.actualCostInitialState = 0;

        ReversePathFinding.CalculateHeuristicReversePathFinding(start, goal);
		start.estimatedTotalNodePathCost = ReversePathFinding.abstracDist(start, goal);

        Debug.Log(goal.transform.position);

		if(start.estimatedTotalNodePathCost != Mathf.Infinity)
		{
		    while (openSet.Count != 0)
		    {
		        currentNode = findLowestTotalCost();

		        if (currentNode == goal)
		        {
					//goal.timeReservation.Add(Mathf.Infinity);
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

                    float tempPathCost = ReversePathFinding.abstracDist(currentNode, goal) + currentNode.CalculateHeuristic(neighbor.transform.position);

					if(neighbor.IsObstacle() || neighbor.timeReservation.Contains(tempPathCost) || neighbor.isReserved)
					{
						tempPathCost = -1;
					}

		            if (!openSet.Contains(neighbor) || (tempPathCost < neighbor.actualCostInitialState))
		            {
		                neighbor.parent = currentNode;
		                neighbor.actualCostInitialState = tempPathCost;

                        neighbor.estimatedTotalNodePathCost = neighbor.CalculateHeuristic(goal.transform.position);
                        Debug.Log(tempPathCost);
						neighbor.timeReservation.Add(tempPathCost);

						if (!openSet.Contains(neighbor))
		                {
		                    openSet.Add(neighbor);
		                }
		            }
		        }

		    }
		}


        return null;
    }

    protected Node findLowestTotalCost()
    {
        Node templowestNodeCost = null;

        foreach (Node node in openSet)
        {
            if ((templowestNodeCost == null || templowestNodeCost.estimatedTotalNodePathCost > node.estimatedTotalNodePathCost) && node.estimatedTotalNodePathCost != -1 && !node.IsObstacle())
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
            Debug.Log(currentNode.transform.position);
            totalPath.Push(currentNode);
            parent = currentNode;
            currentNode = currentNode.parent;
            parent.ClearParent();
        }

        return totalPath;
    }
}
