using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Collections;
using System.Collections.Generic;

public class CellMechanics : MonoBehaviour
{
    GraphClass Graph;
    GraphView GraphView;
    List<Node> RedNodes;
    List<Node> BlueNodes;
    CellState[,] nextStates;

    public void Init(GraphClass Graph, GraphView GraphView)
    {
        if (Graph == null || GraphView == null)
        {
            Debug.LogWarning("CellMechanics Init error: missing components.");
            return;
        }

        this.Graph = Graph;
        this.GraphView = GraphView;
        RedNodes = new List<Node>();
        BlueNodes = new List<Node>();
        nextStates = new CellState[Graph.m_width, Graph.m_height];

        UpdateAliveNodes();
    }

    public void UpdateAliveNodes()
    {
        foreach (Node n in Graph.nodes)
        {
            if (n.cellState == CellState.red && !RedNodes.Contains(n))
            {
                RedNodes.Add(n);
            }
            if (n.cellState == CellState.blue && !BlueNodes.Contains(n))
            {
                BlueNodes.Add(n);
            }
            if (n.cellState == CellState.dead && (RedNodes.Contains(n) || BlueNodes.Contains(n)))
            {
                if (RedNodes.Contains(n))
                {
                    RedNodes.Remove(n);
                }
                if (BlueNodes.Contains(n))
                {
                    BlueNodes.Remove(n);
                }

                NodeView deadNode = GraphView.nodeViews[n.xIndex, n.yIndex];
                deadNode.ColorNode(GraphView.deadColor);
            }
        }
        GraphView.ColorNodes(RedNodes, GraphView.redColor);
        GraphView.ColorNodes(BlueNodes, GraphView.blueColor);
    }

    public void UpdateCellStates()
    {
        int aliveNeighborCount = 0;
        foreach (Node n in Graph.nodes)
        {
            aliveNeighborCount = CountAliveNeighbors(n);
            CellState nextAlive = n.cellState; 
            // Game Rules

            // RED RULES
            // Death
            if (n.cellState == CellState.red && (aliveNeighborCount < 3 || aliveNeighborCount > 7))
            {
                nextAlive = CellState.dead;
            }
            // Birth
            if (n.cellState == CellState.dead && aliveNeighborCount >= 3 && AvgCellState(n) == CellState.red)
            {
                nextAlive = CellState.red;
            }

            // BLUE RULES
            // Death
            if (n.cellState == CellState.blue && (aliveNeighborCount < 3 || aliveNeighborCount > 4))
            {
                nextAlive = CellState.dead;
            }
            // Birth
            if (n.cellState == CellState.dead && aliveNeighborCount == 2 && AvgCellState(n) == CellState.blue)
            {
                nextAlive = CellState.blue;
                RandomNode(n).cellState = CellState.blue;
            }

            nextStates[n.xIndex, n.yIndex] = nextAlive;
        }

        foreach (Node n in Graph.nodes)
        {
            n.cellState = nextStates[n.xIndex, n.yIndex];
        }

        UpdateAliveNodes();
    }

    private int CountAliveNeighbors(Node node)
    {
        int aliveCount = 0;
        foreach (Node n in node.neighbors)
        {
            if (n.cellState != CellState.dead)
            {
                aliveCount++;
            }
        }
        return aliveCount;
    }

    private CellState AvgCellState(Node node)
    {
        float sum = 0;
        int avgDivValue = 0;

        foreach (Node n in node.neighbors)
        {
            if(n.cellState != CellState.dead)
            {
                sum += (int)n.cellState;
                avgDivValue++;
            }
        }

        sum /= avgDivValue;
        
        return (CellState)Math.Ceiling(sum);
    }

    private Node RandomNode(Node node)
    {
        int randomInt = (int)UnityEngine.Random.Range(0, 6);

        if (node.neighbors[randomInt] != null)
        {
            Debug.Log("randomInt Index (1st try successful): " + randomInt);
            return node.neighbors[randomInt];
        }
        else
        {
            while (node.neighbors[randomInt] == null)
            {
                Debug.Log("randomInt Index (RETRY): " + randomInt);
                randomInt = (int)UnityEngine.Random.Range(0, 6);
            }
            Debug.Log("randomInt Index (Retried several times): " + randomInt);
            return node.neighbors[randomInt];
        }
    }
}