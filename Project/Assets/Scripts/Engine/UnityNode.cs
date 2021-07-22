using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UnityNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_InputField nameText;
    public TMP_InputField weightText;
    public Image image;

    GraphNode node;

    Vector2 difference;

    private bool dragging;
    private Vector2 offset;

    public List<UnityEdge> connections;

    public void SetNode(GraphNode node)
    {
        connections = new List<UnityEdge>();
        this.node = node;
        UpdateValues();
    }

    public void UpdateValues()
    {
        nameText.text = node.GetName();
        weightText.text = node.GetWeight().ToString();
    }

    public void UpdateWeight(float newWeight)
    {
        node.SetWeight(newWeight);
        UpdateValues();
    }

    public List<UnityEdge> GetConnections()
    {
        return connections;
    }

    public void AddConnection(UnityEdge connection)
    {
        connections.Add(connection);
    }

    private void OnMouseDown()
    {
        print("ue");
        difference = new Vector2(transform.position.x, transform.position.y) - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseDrag()
    {
        print("eu");
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + difference;
    }

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public Color GetColor()
    {
        return image.color;
    }

    public GraphNode GetGraphNode()
    {
        return node;
    }

    public void SetWeight(int value)
    {
        weightText.text = value.ToString();
    }
}
