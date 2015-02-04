using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ReversePathFinding : GroupPathFinding
{
    public override Stack<Node> PathFinding(Node start, Node goal)
    {
        Node currentNode;

        this.openSet.Add(goal);

        goal.bestKnownPathCost = 0;

        goal.totalNodePathCost = goal.bestKnownPathCost + goal.CalculateHeuristic(start.transform.position);

        while (this.openSet.Count != 0)
        {
            currentNode = this.findLowestTotalCost();

            if (currentNode == start)
            {
                return this.reconstructPath(currentNode);
            }

            this.openSet.Remove(currentNode);
            this.closedSet.Add(currentNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tempPathCost = currentNode.bestKnownPathCost + distanceBetween(currentNode, neighbor);

                if ((!this.openSet.Contains(neighbor) || (tempPathCost < neighbor.bestKnownPathCost && tempPathCost != -1)) && tempPathCost != currentNode.timeReservation.BinarySearch(tempPathCost) && !currentNode.isUnitWaiting)
                {
                    neighbor.parent = currentNode;
                    neighbor.bestKnownPathCost = tempPathCost;
                    neighbor.totalNodePathCost = neighbor.CalculateHeuristic(goal.transform.position);
                    neighbor.timeReservation.Add(neighbor.bestKnownPathCost);

                    if (!this.openSet.Contains(neighbor))
                    {
                        this.openSet.Add(neighbor);
                    }
                }
            }

        }

        return new Stack<Node>();
    }

    protected override Stack<Node> reconstructPath(Node currentNode)
    {
        List<Node> totalPath = new List<Node>();
        Stack<Node> finalPath = new Stack<Node>();
        Node parent;

        while (currentNode.parent != null)
        {
            totalPath.Add(currentNode);
            parent = currentNode;
            currentNode = currentNode.parent;
            parent.ClearParent();
        }

        finalPath.Push(null);

        foreach (Node node in totalPath.Reverse<Node>())
        {
            finalPath.Push(node);
        }

        return finalPath;
    }
}
