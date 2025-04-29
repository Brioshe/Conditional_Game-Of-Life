using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CellMechanics : MonoBehaviour
{
    GraphClass Graph;
    GraphView GraphView;
    public CellState[,] nextStates;

    private Dictionary<CellState, CellBehaviorInterface> behaviorMap;

    public void Init(GraphClass Graph, GraphView GraphView)
    {
        if (Graph == null || GraphView == null)
        {
            Debug.LogWarning("CellMechanics Init error: missing components.");
            return;
        }

        this.Graph = Graph;
        this.GraphView = GraphView;
        nextStates = new CellState[Graph.m_width, Graph.m_height];

        behaviorMap = new Dictionary<CellState,CellBehaviorInterface>
        {
            { CellState.red, new RedCellBehavior()},
            { CellState.blue, new BlueCellBehavior()},
            { CellState.yellow, new YellowCellBehavior()},
            { CellState.green, new GreenCellBehavior()}
        };

        UpdateAliveNodes();
    }

    public void UpdateAliveNodes()
    {
        foreach (Node n in Graph.nodes)
        {
            if (n.cellState == CellState.red)
            {
                GraphView.nodeViews[n.xIndex, n.yIndex].ColorNode(GraphView.redColor);
            }
            if (n.cellState == CellState.blue)
            {
                GraphView.nodeViews[n.xIndex, n.yIndex].ColorNode(GraphView.blueColor);
            }
            if (n.cellState == CellState.dead)
            {
                GraphView.nodeViews[n.xIndex, n.yIndex].ColorNode(GraphView.deadColor);
            }
            if (n.cellState == CellState.yellow)
            {
                GraphView.nodeViews[n.xIndex, n.yIndex].ColorNode(GraphView.yellowColor);
            }
            if (n.cellState == CellState.green)
            {
                GraphView.nodeViews[n.xIndex, n.yIndex].ColorNode(GraphView.greenColor);
            }
        }
    }

    public void UpdateCellStates()
    {
        int aliveNeighborCount = 0;

        foreach (Node n in Graph.nodes)
        {
            aliveNeighborCount = CountAliveNeighbors(n);
            var neighbors = n.neighbors;
            CellState currentState = n.cellState; 
            CellState avgNeighbor = AvgCellState(n);

            if (behaviorMap.TryGetValue(avgNeighbor, out var behavior))
            {
                currentState = behavior.GetNextState(n, neighbors);

                if (currentState == CellState.blue)
                {
                    RandomNodeChange(n);
                }

                if (currentState == CellState.red && neighbors.Count(n => n.cellState == CellState.green) > 1)
                {
                    RandomNodeChange(n);
                }
            }

            if (n.nextStateFlag == false)
            {
                nextStates[n.xIndex, n.yIndex] = currentState;
                n.nextStateFlag = true;
            }
        }

        foreach (Node n in Graph.nodes)
        {
            n.cellState = nextStates[n.xIndex, n.yIndex];
            n.nextStateFlag = false;
        }

        UpdateAliveNodes();
    }

    public int CountAliveNeighbors(Node node)
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

    public CellState AvgCellState(Node node)
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

        if (avgDivValue == 0)
        {
            return node.cellState;
        }
        
        return (CellState)Math.Ceiling(sum);
    }

    private void RandomNodeChange(Node node)
    {
        int randomInt = (int)UnityEngine.Random.Range(0, node.neighbors.Count);

        if (node.neighbors[randomInt] != null && node.neighbors[randomInt].cellState == CellState.dead)
        {
            nextStates[node.neighbors[randomInt].xIndex, node.neighbors[randomInt].yIndex] = node.cellState;
            node.neighbors[randomInt].nextStateFlag = true;
        } 
    }
}