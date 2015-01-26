using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathfinding : MonoBehaviour 
{
    //Already Evaluated Nodes
    private List<Node> closedSet;

    //Nodes to be evaluated, including start node
    private List<Node> openSet;

    //Navigated Nodes
    private Stack<Node> exploredSet;

    private Map currentMap;

    public Stack<Node> PathFinding(Node start, Node goal)
    {
        Node currentNode;

        this.openSet.Add(start);
        start.bestKnownPathCost = 0;

        start.totalNodePathCost = start.bestKnownPathCost + this.heuristicEstimation(start, goal);

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
                    neighbor.totalNodePathCost = neighbor.bestKnownPathCost + heuristicEstimation(neighbor, goal);

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
        foreach (Node node in currentMap)
        {

        }

        return new Node(0,0);
    }

    private float heuristicEstimation(Node start, Node goal)
    {


        return 0;
    }

    private float distanceBetween(Node startNode, Node goalNode)
    {


        return 0;
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
