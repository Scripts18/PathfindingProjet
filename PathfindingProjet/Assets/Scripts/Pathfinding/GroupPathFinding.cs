using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathfinding : MonoBehaviour 
{
    //Already Evaluated Nodes
	private List<Node> closedSet = new List<Node>();

    //Nodes to be evaluated, including start node
    private List<Node> openSet = new List<Node>();

    //Navigated Nodes
	private Stack<Node> exploredSet = new Stack<Node>();

    private Map currentMap;

    public Stack<Node> PathFinding(Node start, Node goal)
    {
        Node currentNode;

        this.openSet.Add(start);
        start.bestKnownPathCost = 0;

        start.totalNodePathCost = start.bestKnownPathCost + start.CalculateHeuristic(goal.transform.position);

        while (this.openSet.Count != 0)
        {
            currentNode = this.findLowestTotalCost();

            if (currentNode == goal)
            {
                return reconstructPath(currentNode);
            }

            this.openSet.Remove(currentNode);
            this.closedSet.Add(currentNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                float tempPathCost = currentNode.bestKnownPathCost + distanceBetween(currentNode, neighbor);

                if (!this.openSet.Contains(neighbor) || tempPathCost < neighbor.bestKnownPathCost)
                {
                    this.exploredSet.Push(neighbor);
                    neighbor.bestKnownPathCost = tempPathCost;
					neighbor.CalculateHeuristic(goal.transform.position);
					
					if (!this.openSet.Contains(neighbor))
                    {
                        this.openSet.Add(neighbor);
                    }
                }
            }

        }

        return null;
    }

    private Node findLowestTotalCost()
    {
        Node templowestNodeCost = null;

        foreach (Node node in this.openSet)
        {
            if (templowestNodeCost == null || templowestNodeCost.totalNodePathCost > node.totalNodePathCost)
            {
                templowestNodeCost = node;
            }
        }

        return templowestNodeCost;
    }

    private float distanceBetween(Node startNode, Node goalNode)
    {
		return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }

    private Stack<Node> reconstructPath(Node currentNode)
    {
        Stack<Node> totalPath = new Stack<Node>();

        totalPath.Push(currentNode);

        while (this.exploredSet.Count != 0)
        {
            totalPath.Push(exploredSet.Pop());
        }

        return totalPath;
    }
}
