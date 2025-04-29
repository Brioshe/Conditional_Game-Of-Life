using System.Collections.Generic;
using UnityEngine;

public class GraphView : MonoBehaviour
{
    public GameObject nodeviewPrefab;
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color yellowColor = Color.yellow;
    public Color greenColor = new Color32(45, 115, 54, 255);
    public Color deadColor = Color.white;
    public NodeView[,] nodeViews;
    public void Init(GraphClass graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("Graphview error! No graph to init!");
            return;
        }

        nodeViews = new NodeView[graph.m_width, graph.m_height];
        foreach(Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeviewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeview = instance.GetComponent<NodeView>();
            // Debug.Log("Position: " + n.position.x + ", " + n.position.z);
            if (nodeview != null)
            {
                nodeview.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeview;
                
                nodeview.ColorNode(deadColor);
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }
}


