using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseButton : MonoBehaviour
{
    bool inversed = false;

    public GameObject inversedGraph;

    private void Start()
    {
        inversedGraph.SetActive(false);
    }

    public void Click()
    {
        if (inversed)
        {
            inversed = false;
            inversedGraph.SetActive(false);
        }
        else
        {
            inversed = true;
            inversedGraph.SetActive(true);
        }
    }

    public void Inverse()
    {
        inversed = true;
        inversedGraph.SetActive(true);
    }

    public void Disinverse()
    {
        inversed = false;
        inversedGraph.SetActive(false);
    }
}
