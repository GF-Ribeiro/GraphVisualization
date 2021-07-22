using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    Graph graph;

    // Start is called before the first frame update
    void Start()
    {
        graph = IO.GenerateGraph();
        graph.GenerateIncidenceMatrix(false);
        IO.PrintAdjacencyMatrix(graph);
        IO.PrintIncidenceMatrix(graph);

        GraphBuilder.instance.GenerateGraph(graph);
    }

    public void ResetApplication()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
