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
    public bool fightFlag = false;
    public bool idleFlag = true;

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
            CellState avgNeighbor = GetMajorityNode(n);

            if (idleFlag == true)
            {
                if (behaviorMap.TryGetValue(avgNeighbor, out var behavior))
                {
                    // Birth turn
                    currentState = behavior.GetNextState(n, neighbors);

                    if (currentState == CellState.blue)
                    {
                        RandomNodeChange(n);
                    }

                    if (currentState == CellState.red && neighbors.Count(o => o.cellState == CellState.green) > 0)
                    {
                        RandomNodeChange(n);
                    }
                }
            }

            if (fightFlag == true)
            {
                if (behaviorMap.TryGetValue(currentState, out var behavior))
                {
                    // Fighting turn
                    if (currentState != avgNeighbor && currentState != CellState.dead)
                    {
                        currentState = behavior.ReplaceState(n, avgNeighbor);
                    }
                    // Yellow x Red Exception:
                    if (currentState == CellState.red && neighbors.Count(n => n.cellState == CellState.yellow) > 0)
                    {
                        currentState = behavior.ReplaceState(n, CellState.yellow);
                    }
                    // Green x Blue Exception
                    if (currentState == CellState.blue && neighbors.Count(n => n.cellState == CellState.green) > 1)
                    {
                        currentState = behavior.ReplaceState(n, CellState.green);
                    }
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
        fightFlag = !fightFlag;
        idleFlag = !idleFlag;
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

    public CellState GetMajorityNode(Node node)
    {
        Dictionary<CellState, int> frequency = new Dictionary<CellState, int>
        {
            { CellState.red, 0},
            { CellState.blue, 0},
            { CellState.yellow, 0},
            { CellState.green, 0}
        };

        foreach (Node n in node.neighbors)
        {
            if (n.cellState != CellState.dead)
            {
                frequency[n.cellState]++;
            }
        }

        CellState majority = CellState.dead;
        int mode = 0;

        foreach (var colorKey in frequency)
        {
            if (colorKey.Value > mode)
            {
                mode = colorKey.Value;
                majority = colorKey.Key;
            }
        }

        return majority;
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