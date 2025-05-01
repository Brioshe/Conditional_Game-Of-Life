using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public interface CellBehaviorInterface
{   
    CellState GetNextState(Node node, List<Node> neighbors);
    CellState? GetReplacement(Node node, List<Node> neighbors);
    float GetWinChanceAgainst(CellState opponent);
}

public class RedCellBehavior : CellBehaviorInterface
{
    public CellMechanics cellMechanics; 

    public CellState GetNextState(Node node, List<Node> neighbors)
    {
        int redNeighbors = neighbors.Count(n => n.cellState == CellState.red);
    
        if (node.cellState == CellState.red)
        {
            return (redNeighbors >= 2 && redNeighbors <= 7) ? CellState.red : CellState.dead;
        }
        else if (node.cellState == CellState.dead)
        {
            return (redNeighbors >= 3) ? CellState.red : CellState.dead;
        }
        else
        {
            return node.cellState;
        }
    }
    public CellState? GetReplacement(Node node, List<Node> neighbors)
    {
        return CellState.dead;
    }
    public float GetWinChanceAgainst(CellState opponent)
    {
        return 0;
    }
}

public class BlueCellBehavior : CellBehaviorInterface
{
    public CellMechanics cellMechanics; 

    public CellState GetNextState(Node node, List<Node> neighbors)
    {
        int blueNeighbors = neighbors.Count(n => n.cellState == CellState.blue);
    
        if (node.cellState == CellState.blue)
        {
            return (blueNeighbors >= 2 && blueNeighbors <= 3) ? CellState.blue : CellState.dead;
        }
        else if (node.cellState == CellState.dead)
        {
            return (blueNeighbors == 2) ? CellState.blue : CellState.dead;
        }
        else
        {
            return node.cellState;
        }
    }
    public CellState? GetReplacement(Node node, List<Node> neighbors)
    {
        return CellState.dead;
    }
    public float GetWinChanceAgainst(CellState opponent)
    {
        return 0;
    }
}

public class YellowCellBehavior : CellBehaviorInterface
{
    public CellMechanics cellMechanics; 

    public CellState GetNextState(Node node, List<Node> neighbors)
    {
        int yellowNeighbors = neighbors.Count(n => n.cellState == CellState.yellow);
    
        if (node.cellState == CellState.yellow)
        {
            return (yellowNeighbors >= 2 && yellowNeighbors <= 3) ? CellState.yellow : CellState.dead;
        }
        else if (node.cellState == CellState.dead)
        {
            return (yellowNeighbors == 3) ? CellState.yellow : CellState.dead;
        }
        else
        {
            return CellState.dead;
        }
    }
    public CellState? GetReplacement(Node node, List<Node> neighbors)
    {
        return CellState.dead;
    }
    public float GetWinChanceAgainst(CellState opponent)
    {
        return 0;
    }
}

public class GreenCellBehavior : CellBehaviorInterface
{
    public CellMechanics cellMechanics; 

    public CellState GetNextState(Node node, List<Node> neighbors)
    {
        int greenNeighbors = neighbors.Count(n => n.cellState == CellState.green);
    
        if (node.cellState == CellState.green)
        {
            return (greenNeighbors >= 2 && greenNeighbors <= 3) ? CellState.green : CellState.dead;
        }
        else if (node.cellState == CellState.dead)
        {
            return (greenNeighbors == 3) ? CellState.green : CellState.dead;
        }
        else
        {
            return CellState.dead;
        }
    }
    public CellState? GetReplacement(Node node, List<Node> neighbors)
    {
        return CellState.dead;
    }
    public float GetWinChanceAgainst(CellState opponent)
    {
        return 0;
    }
}