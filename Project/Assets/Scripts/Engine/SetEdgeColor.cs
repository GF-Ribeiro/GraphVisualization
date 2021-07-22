using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEdgeColor : MonoBehaviour
{
    UnityEdge unityEdge;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        unityEdge = GetComponentInParent<UnityEdge>();
    }

    public void Set()
    {
        unityEdge.SetColor(image.color);
    }
}
