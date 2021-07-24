using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GraphBuilder : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject edgePrefab;

    public Transform nodesFather;
    public Transform edgesFather;

    public Transform inversdeNodesFather;
    public Transform inversedEdgesFather;

    public static GraphBuilder instance;

    public float radius;

    List<UnityNode> unityNodes;
    List<UnityEdge> unityEdges;

    public List<UnityNode> inversedUnityNodes;
    public List<UnityEdge> inversedUnityEdges;

    private bool proceed;

    public Toggle toggle;

    private bool[] visited;
    Queue<UnityNode> queue;

    private bool wait;

    private int visitedNotes;

    private Stack<int> finishedStack;

    public InverseButton inverseButton;

    public List<Color> colors;

    private void Awake()
    {
        instance = this;
        UpdateWaitValue();

        colors.AddRange(colors);
        colors.AddRange(colors);
    }

    public void GenerateGraph(Graph graph)
    {
        List<GraphNode> nodes = graph.GetNodes();

        unityNodes = new List<UnityNode>();
        unityEdges = new List<UnityEdge>();

        float radiansBetweenEachPoint = (2 * Mathf.PI) / nodes.Count;

        for (int i = 0; i < nodes.Count; i++)
        {
            UnityNode unityNode = Instantiate(nodePrefab, nodesFather).GetComponent<UnityNode>();
            unityNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(radius * Mathf.Cos(i * radiansBetweenEachPoint), radius * Mathf.Sin(i * radiansBetweenEachPoint));
            unityNode.SetNode(nodes[i]);
            unityNodes.Add(unityNode);
        }

        GraphEdge[,] adjacencyMatrix = graph.GetAdjacencyMatrix();
        int totalNodes = graph.GetNodes().Count;

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = (graph.IsDirected() ? 0 : i + 1); j < nodes.Count; j++)
            {
                if(adjacencyMatrix[i,j] != null)
                {
                    UnityEdge unityEdge = Instantiate(edgePrefab, edgesFather).GetComponent<UnityEdge>();
                    RectTransform nodeARect = unityNodes[adjacencyMatrix[i, j].GetNodeAIndex()].GetComponent<RectTransform>();
                    RectTransform nodeBRect = unityNodes[adjacencyMatrix[i, j].GetNodeBIndex()].GetComponent<RectTransform>();

                    unityEdge.SetEdge(adjacencyMatrix[i, j], nodeARect, nodeBRect, graph.IsDirected());

                    unityNodes[adjacencyMatrix[i, j].GetNodeAIndex()].AddConnection(unityEdge);
                    if (!graph.IsDirected())
                    {
                        unityNodes[adjacencyMatrix[i, j].GetNodeBIndex()].AddConnection(unityEdge);
                    }

                    unityEdges.Add(unityEdge);
                }
            }
        }

        if(graph.IsDirected())
        {
            GenerateInverseGraph(graph);
        }
        else
        {
            inverseButton.gameObject.SetActive(false);
        }
    }

    public void GenerateInverseGraph(Graph graph)
    {
        List<GraphNode> nodes = graph.GetNodes();

        inversedUnityNodes = new List<UnityNode>();
        inversedUnityEdges = new List<UnityEdge>();

        float radiansBetweenEachPoint = (2 * Mathf.PI) / nodes.Count;

        for (int i = 0; i < nodes.Count; i++)
        {
            UnityNode unityNode = Instantiate(nodePrefab, inversdeNodesFather).GetComponent<UnityNode>();
            unityNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(radius * Mathf.Cos(i * radiansBetweenEachPoint), radius * Mathf.Sin(i * radiansBetweenEachPoint));
            unityNode.SetNode(nodes[i]);
            inversedUnityNodes.Add(unityNode);
        }

        GraphEdge[,] adjacencyMatrix = graph.GetAdjacencyMatrix();
        int totalNodes = graph.GetNodes().Count;

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (adjacencyMatrix[i, j] != null)
                {
                    UnityEdge unityEdge = Instantiate(edgePrefab, inversedEdgesFather).GetComponent<UnityEdge>();
                    RectTransform nodeARect = inversedUnityNodes[adjacencyMatrix[i, j].GetNodeBIndex()].GetComponent<RectTransform>();
                    RectTransform nodeBRect = inversedUnityNodes[adjacencyMatrix[i, j].GetNodeAIndex()].GetComponent<RectTransform>();

                    unityEdge.SetEdge(adjacencyMatrix[i, j], nodeARect, nodeBRect, graph.IsDirected());

                    inversedUnityNodes[adjacencyMatrix[i, j].GetNodeBIndex()].AddConnection(unityEdge);

                    inversedUnityEdges.Add(unityEdge);
                }
            }
        }
    }

    public IEnumerator Prim()
    {
        List<UnityNode> selectedNodes = new List<UnityNode>();
        List<UnityEdge> selectedEdges = new List<UnityEdge>();
        List<UnityEdge> candidates = new List<UnityEdge>();

        int[] selectedNodesAux = new int[unityNodes.Count];

        for (int i = 0; i < unityEdges.Count; i++)
        {
            unityEdges[i].SetColor(Color.white);
        }

        int randomNode = Random.Range(0, unityNodes.Count);

        unityNodes[randomNode].SetColor(Color.yellow);
        selectedNodesAux[randomNode] = 1;
        selectedNodes.Add(unityNodes[randomNode]);
        candidates.AddRange(unityNodes[randomNode].GetConnections());

        while(selectedNodes.Count < unityNodes.Count)
        {
            if (wait)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                proceed = false;
                yield return new WaitUntil(() => proceed == true);
                proceed = false;
            }

            float minValue = float.PositiveInfinity;
            int selectedCandidate = 0;

            for (int j = 0; j < candidates.Count; j++)
            {
                if (selectedNodesAux[candidates[j].GetGraphEdge().GetNodeAIndex()] == 0 || selectedNodesAux[candidates[j].GetGraphEdge().GetNodeBIndex()] == 0)
                {
                    candidates[j].SetColor(Color.red);

                    if (candidates[j].GetDistance() < minValue)
                    {
                        selectedCandidate = j;
                        minValue = candidates[j].GetDistance();
                    }
                }
            }

            if (wait)
            {
                yield return new WaitForSeconds(3);
            }
            else
            {
                proceed = false;
                yield return new WaitUntil(() => proceed == true);
                proceed = false;
            }

            candidates[selectedCandidate].SetColor(Color.yellow);
            selectedEdges.Add(candidates[selectedCandidate]);

            if (selectedNodesAux[candidates[selectedCandidate].GetGraphEdge().GetNodeAIndex()] == 0)
            {
                selectedNodesAux[candidates[selectedCandidate].GetGraphEdge().GetNodeAIndex()] = 1;
                unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeAIndex()].SetColor(Color.yellow);
                selectedNodes.Add(unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeAIndex()]);
                candidates.AddRange(unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeAIndex()].GetConnections());
            }
            else
            {
                selectedNodesAux[candidates[selectedCandidate].GetGraphEdge().GetNodeBIndex()] = 1;
                unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeBIndex()].SetColor(Color.yellow);
                selectedNodes.Add(unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeBIndex()]);
                candidates.AddRange(unityNodes[candidates[selectedCandidate].GetGraphEdge().GetNodeBIndex()].GetConnections());
            }

            candidates.RemoveAt(selectedCandidate);
        }

        if (wait)
        {
            yield return new WaitForSeconds(2);
        }
        else
        {
            proceed = false;
            yield return new WaitUntil(() => proceed == true);
            proceed = false;
        }

        for (int i = 0; i < selectedEdges.Count; i++)
        {
            unityEdges.Remove(selectedEdges[i]);
        }
        for (int i = 0; i < unityEdges.Count; i++)
        {
            unityEdges[i].gameObject.SetActive(false);
        }
    }


    private IEnumerator Kruskal()
    {
        List<UnityEdge> selectedEdges = new List<UnityEdge>();
        List<UnityEdge> orederedEdges = unityEdges.OrderBy(edge => edge.GetDistance()).ToList();
        int[] currentTree = new int[unityNodes.Count];
        
        for (int i = 0; i < unityEdges.Count; i++)
        {
            unityEdges[i].SetColor(Color.white);
        }

        for (int i = 0; i < currentTree.Length; i++)
        {
            unityNodes[i].SetColor(colors[i]);
            currentTree[i] = i;
        }

        

        for (int i = 0; i < orederedEdges.Count; i++)
        {
            if (currentTree[orederedEdges[i].GetGraphEdge().GetNodeAIndex()] != currentTree[orederedEdges[i].GetGraphEdge().GetNodeBIndex()])
            {
                if (wait)
                {
                    yield return new WaitForSeconds(2);
                }
                else
                {
                    proceed = false;
                    yield return new WaitUntil(() => proceed == true);
                    proceed = false;
                }
                
                orederedEdges[i].SetColor(Color.yellow);
                selectedEdges.Add(orederedEdges[i]);

                int originalValue = currentTree[orederedEdges[i].GetGraphEdge().GetNodeAIndex()];
                int newValue = currentTree[orederedEdges[i].GetGraphEdge().GetNodeBIndex()];

                for (int j = 0; j < currentTree.Length; j++)
                {
                    if(currentTree[j] == originalValue)
                    {
                        currentTree[j] = newValue;
                        unityNodes[j].SetColor(colors[newValue]);
                    }
                }
            }
        }

        if (wait)
        {
            yield return new WaitForSeconds(2);
        }
        else
        {
            proceed = false;
            yield return new WaitUntil(() => proceed == true);
            proceed = false;
        }

        for (int i = 0; i < selectedEdges.Count; i++)
        {
            unityEdges.Remove(selectedEdges[i]);
        }
        for (int i = 0; i < unityEdges.Count; i++)
        {
            unityEdges[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator StartDepthSearchCoroutine(List<UnityNode> unityNodes, bool useStack = false)
    {
        visited = new bool[unityNodes.Count];

        if(!useStack)
        {
            finishedStack = new Stack<int>();
        }

        for (int i = 0; i < unityNodes.Count; i++)
        {
            unityNodes[i].SetColor(Color.white);
            unityNodes[i].SetWeight(0);
        }

        int currentColor = 0;

        visitedNotes = 1;

        if(!useStack)
        {
            for (int i = 0; i < unityNodes.Count; i++)
            {
                if (!visited[unityNodes[i].GetGraphNode().GetId()])
                {
                    yield return StartCoroutine(DepthSearch(unityNodes[i], colors[currentColor++], true, unityNodes));
                }
            }
        }
        else
        {
            while(finishedStack.Count > 0)
            {
                UnityNode node = unityNodes[finishedStack.Pop()];

                if (!visited[node.GetGraphNode().GetId()])
                {
                    yield return StartCoroutine(DepthSearch(node, colors[currentColor++], false, unityNodes));
                }
            }
        }
        
    }

    public IEnumerator StartDepthSearchSpecialCoroutine()
    {
        yield return StartCoroutine(StartDepthSearchCoroutine(unityNodes));

        inverseButton.Inverse();

        yield return StartCoroutine(StartDepthSearchCoroutine(inversedUnityNodes, true));

        for (int i = 0; i < unityNodes.Count; i++)
        {
            unityNodes[i].SetColor(inversedUnityNodes[i].GetColor());
        }

        inverseButton.Disinverse();
    }


    public IEnumerator DepthSearch(UnityNode currentNode, Color color, bool useStack, List<UnityNode> unityNodes)
    {
        visited[currentNode.GetGraphNode().GetId()] = true;

        currentNode.SetColor(color);

        currentNode.SetWeight(visitedNotes++);

        List<UnityEdge> nodeConnections = currentNode.GetConnections();

        if (wait)
        {
            yield return new WaitForSeconds(2);
        }
        else
        {
            proceed = false;
            yield return new WaitUntil(() => proceed == true);
            proceed = false;
        } 

        for (int i = 0; i < nodeConnections.Count; i++)
        {
            if (nodeConnections[i].GetGraphEdge().GetNodeAIndex() != currentNode.GetGraphNode().GetId() && !visited[nodeConnections[i].GetGraphEdge().GetNodeAIndex()])
            {
                yield return StartCoroutine(DepthSearch(unityNodes[nodeConnections[i].GetGraphEdge().GetNodeAIndex()], color, useStack, unityNodes));
            }
            else if (nodeConnections[i].GetGraphEdge().GetNodeBIndex() != currentNode.GetGraphNode().GetId() && !visited[nodeConnections[i].GetGraphEdge().GetNodeBIndex()])
            {
                yield return StartCoroutine(DepthSearch(unityNodes[nodeConnections[i].GetGraphEdge().GetNodeBIndex()], color, useStack, unityNodes));
            }
        }

        if(useStack)
        {
            finishedStack.Push(currentNode.GetGraphNode().GetId());
        }
            
    }

    private IEnumerator BreadthSearchCoroutine()
    {
        visited = new bool[unityNodes.Count];
        Queue<UnityNode> queue = new Queue<UnityNode>();

        for (int i = 0; i < unityNodes.Count; i++)
        {
            unityNodes[i].SetColor(Color.white);
            unityNodes[i].SetWeight(0);
        }

        int currentColor = 0;
        visitedNotes = 1;

        for (int i = 0; i < unityNodes.Count; i++)
        {
            if (!visited[unityNodes[i].GetGraphNode().GetId()])
            {
                queue.Enqueue(unityNodes[i]);

                while(queue.Count > 0)
                {
                    UnityNode currentNode = queue.Dequeue();
                    visited[currentNode.GetGraphNode().GetId()] = true;
                    currentNode.SetColor(colors[currentColor]);
                    currentNode.SetWeight(visitedNotes++);
                    List<UnityEdge> nodeConnections = currentNode.GetConnections();

                    for (int j = 0; j < nodeConnections.Count; j++)
                    {
                        if (nodeConnections[j].GetGraphEdge().GetNodeAIndex() != currentNode.GetGraphNode().GetId() && !visited[nodeConnections[j].GetGraphEdge().GetNodeAIndex()])
                        {
                            queue.Enqueue(unityNodes[nodeConnections[j].GetGraphEdge().GetNodeAIndex()]);
                            visited[nodeConnections[j].GetGraphEdge().GetNodeAIndex()] = true;
                        }
                        else if (nodeConnections[j].GetGraphEdge().GetNodeBIndex() != currentNode.GetGraphNode().GetId() && !visited[nodeConnections[j].GetGraphEdge().GetNodeBIndex()])
                        {
                            queue.Enqueue(unityNodes[nodeConnections[j].GetGraphEdge().GetNodeBIndex()]);
                            visited[nodeConnections[j].GetGraphEdge().GetNodeBIndex()] = true;
                        }
                    }

                    if (wait)
                    {
                        yield return new WaitForSeconds(2);
                    }
                    else
                    {
                        proceed = false;
                        yield return new WaitUntil(() => proceed == true);
                        proceed = false;
                    }
                }

                currentColor++;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            proceed = true;
        }
    }

    public void StartPrim()
    {
        StartCoroutine(Prim());
    }

    public void StartKruskal()
    {
        StartCoroutine(Kruskal());
    }

    public void StartDepthSearch()
    {
        StartCoroutine(StartDepthSearchCoroutine(unityNodes));
    }

    public void StartBreadthSearch()
    {
        StartCoroutine(BreadthSearchCoroutine());
    }

    public void UpdateWaitValue()
    {
        wait = toggle.isOn;
    }

    public void StartDepthSpecial()
    {
        StartCoroutine(StartDepthSearchSpecialCoroutine());
    }

}
