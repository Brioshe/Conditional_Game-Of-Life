using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public interface CellBehaviorInterface
{   
    CellState GetNextState(Node node, List<Node> neighbors);
    CellState ReplaceState(Node node, CellState majorityNeighbor);
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
    public CellState ReplaceState(Node node, CellState majorityNeighbor)
    {
        int randomInt = (int)UnityEngine.Random.Range(1, 100);

        if (majorityNeighbor == CellState.blue && randomInt >= 30)
        {
            return CellState.blue;
        }
        else if (majorityNeighbor == CellState.yellow && randomInt >= 20)
        {
            return CellState.yellow;
        }
        else if (majorityNeighbor == CellState.green)
        {
            return node.cellState;
        }

        return node.cellState;
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
    public CellState ReplaceState(Node node, CellState majorityNeighbor)
    {
        int randomInt = (int)UnityEngine.Random.Range(1, 100);
        if (majorityNeighbor == CellState.red && randomInt >= 70)
        {
            return CellState.red;
        }
        else if (majorityNeighbor == CellState.yellow && randomInt >= 55)
        {
            return CellState.yellow;
        }
        else if (majorityNeighbor == CellState.green && randomInt >= 10)
        {
            return CellState.green;
        }

        return node.cellState;
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
    public CellState ReplaceState(Node node, CellState majorityNeighbor)
    {
        int randomInt = (int)UnityEngine.Random.Range(1, 100);
        if (majorityNeighbor == CellState.red && randomInt >= 80)
        {
            return CellState.red;
        }
        else if (majorityNeighbor == CellState.blue && randomInt >= 45)
        {
            return CellState.blue;
        }
        else if (majorityNeighbor == CellState.green && randomInt >= 65)
        {
            return CellState.green;
        }

        return node.cellState;
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
    public CellState ReplaceState(Node node, CellState majorityNeighbor)
    {
        int randomInt = (int)UnityEngine.Random.Range(1, 100);
        if (majorityNeighbor == CellState.red)
        {
            return node.cellState;
        }
        else if (majorityNeighbor == CellState.blue && randomInt >= 90)
        {
            return CellState.blue;
        }
        else if (majorityNeighbor == CellState.yellow && randomInt >= 35)
        {
            return CellState.yellow;
        }

        return node.cellState;
    }
}