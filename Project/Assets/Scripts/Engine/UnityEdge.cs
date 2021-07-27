using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using DG.Tweening;

public class UnityEdge : MonoBehaviour
{
    public RectTransform origin;
    public RectTransform destination;

    public UILineTextureRenderer lineRenderer;
    public RectTransform distanceContainer;
    public TextMeshProUGUI distanceText;
    public TMP_InputField distanceInput;

    Vector2 offset;

    GraphEdge edge;

    public Transform optionsPanel;

    public RectTransform directionArrow;

    // Start is called before the first frame update
    void Awake()
    {
        optionsPanel.localScale = Vector3.zero;
        offset = new Vector2(Screen.width/2, Screen.height/2);
    }

    private void OnEnable()
    {
        UpdateLine();
    }

    public void SetEdge(GraphEdge edge, RectTransform origin, RectTransform destination, bool directed)
    {
        this.edge = edge;
        this.origin = origin;
        this.destination = destination;

        lineRenderer.color = edge.GetColor();
        distanceText.text = edge.GetDistance().ToString();
        distanceInput.text = distanceText.text;

        if(!directed)
        {
            directionArrow.gameObject.SetActive(false);
        }

        UpdateLine();
    }

    private void UpdateLine()
    {
        if (!origin)
            return;

        Vector2[] points = new Vector2[2];
        points[0] = origin.anchoredPosition + offset;
        points[1] = destination.anchoredPosition + offset;
        lineRenderer.Points = points;
        
        distanceContainer.anchoredPosition = (origin.anchoredPosition + destination.anchoredPosition) / 2;
        directionArrow.anchoredPosition = (origin.anchoredPosition + 2 * destination.anchoredPosition) / 3;
        directionArrow.up = (destination.anchoredPosition - origin.anchoredPosition).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
    }

    public void SetColor(Color color)
    {
        lineRenderer.color = color;
    }

    public void OpenOptions()
    {
        optionsPanel.gameObject.SetActive(true);
        optionsPanel.DOScale(Vector3.one, 0.3f);
    }

    public void CloseOptions()
    {
        optionsPanel.DOScale(Vector3.zero, 0.3f);
    }

    public void UpdateDistance()
    {
        distanceText.text = distanceInput.text;
    }

    public GraphEdge GetGraphEdge()
    {
        return edge;
    }

    public float GetDistance()
    {
        return float.Parse(distanceText.text);
    }
}
