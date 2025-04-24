using System.Collections.Generic;
using System.ComponentModel;
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
    public CellState GetNextState(Node node, List<Node> neighbors)
    {
        return CellState.dead;
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
