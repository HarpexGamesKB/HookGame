using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public LineRenderer LineRenderer;
    [SerializeField] private Transform[] points;
    public int Segments = 10;
    public float lenth = 10f;
    public void Draw(Vector3 a,Vector3 b, float length)
    {
        LineRenderer.enabled = true;
        float interpolant = Vector3.Distance(a, b) / length;
        float offset = Mathf.Lerp(length / 2f, 0f, interpolant);

        Vector3 Adown = a + Vector3.down * offset;
        Vector3 Bdown = b + Vector3.down * offset;

        LineRenderer.positionCount = Segments + 1;
        for (int i = 0; i < Segments+1; i++)
        {
            LineRenderer.SetPosition(i, Bezier.GetPoint(a, Adown, Bdown, b, (float)i / Segments));
        }
    }
    private void Update()
    {
        Draw(points[0].position,points[1].position,lenth);
    }
    public void Hide()
    {
        LineRenderer.enabled = false;
    }
    /*
     * public LineRenderer LineRenderer;
    [SerializeField] private Transform[] points;
    
    private void Update()
    {
        for (int i = 0; i <points.Length; i++)
        {
            LineRenderer.SetPosition(i, points[i].position);
        }
    }
    */
}
