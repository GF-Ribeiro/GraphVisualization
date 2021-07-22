using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<GraphNode> nodes;
    List<GraphEdge> edges;

    GraphEdge[,] adjacencyMatrix;

    int[,] incidenceMatrix;

    bool directed;

    public Graph(int totalNodes, bool directed)
    {
        nodes = new List<GraphNode>();
        edges = new List<GraphEdge>();

        this.directed = directed;

        adjacencyMatrix = new GraphEdge[totalNodes, totalNodes];
    }

    public int AddNode(string name, float weight)
    {
        int id = nodes.Count;
        GraphNode newNode = new GraphNode(id, name, weight);
        nodes.Add(newNode);
        return id;
    }

    public List<GraphNode> GetNodes()
    {
        return nodes;
    }

    public List<GraphEdge> GetEdges()
    {
        return edges;
    }

    public void AddDirectionalEdge(GraphEdge edge)
    {
        adjacencyMatrix[edge.GetNodeAIndex(), edge.GetNodeBIndex()] = edge;
        nodes[edge.GetNodeAIndex()].AddConnection(edge);
        edges.Add(edge);
    }
    
    public void AddBidirectionalGraphEdge(GraphEdge edge)
    {
        AddDirectionalEdge(edge);

        GraphEdge newEdge = new GraphEdge(edge.GetNodeBIndex(), edge.GetNodeAIndex(), edge.GetColor(), edge.GetDistance());
        AddDirectionalEdge(newEdge);
    }

    public bool ContainsNode(string nodeId)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetName() == nodeId)
                return true;
        }
        return false;
    }

    public bool IsDirected()
    {
        return directed;
    }

    public int GetNodeIdByName(string nodeName)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetName() == nodeName)
                return i;
        }
        return -1;
    }

    public GraphNode GetNodeIdById(int id)
    {
        return nodes[id];
    }

    public void UpdateNode(GraphNode node)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetName() == node.GetName())
                nodes[i].SetWeight(node.GetWeight());
        }
    }

    public void GenerateIncidenceMatrix(bool directional)
    {
        int limit;

        if(directional)
        {
            limit = edges.Count;
        }
        else
        {
            limit = edges.Count/2;
        }

        incidenceMatrix = new int[nodes.Count, limit];

        for (int i = 0; i < limit; i++)
        {
            incidenceMatrix[edges[i].GetNodeAIndex(), i] = 1;
            incidenceMatrix[edges[i].GetNodeBIndex(), i] = 1;
        }
    }

    public GraphEdge[,] GetAdjacencyMatrix()
    {
        return adjacencyMatrix;
    }

    public int[,] GetIncidenceMatrix()
    {
        return incidenceMatrix;
    }
}


public class GraphNode
{
    int id;
    string name;
    float weight;

    List<GraphEdge> connections;

    public GraphNode(int id, string name, float weight)
    {
        this.id = id;
        this.name = name;
        this.weight = weight;
        connections = new List<GraphEdge>();
    }

    public string GetName()
    {
        return name;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetWeight(float weight)
    {
        this.weight = weight;
    }

    public void AddConnection(GraphEdge edge)
    {
        connections.Add(edge);
    }

    public List<GraphEdge> GetConnections()
    {
        return connections;
    }

    public int GetId()
    {
        return id;
    }
}

public class GraphEdge
{
    int id;
    int nodeAIndex;
    int nodeBIndex;
    Color color;

    float distance;

    public GraphEdge (int nodeA, int nodeB, Color color, float distance)
    {
        this.nodeAIndex = nodeA;
        this.nodeBIndex = nodeB;
        this.color = color;
        this.distance = distance;
    }

    public int GetNodeAIndex()
    {
        return nodeAIndex;
    }

    public int GetNodeBIndex()
    {
        return nodeBIndex;
    }

    public Color GetColor()
    {
        return color;
    }

    public float GetDistance()
    {
        return distance;
    }
}