using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlerpLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform[] test;
    [SerializeField] private float offset;

    [ContextMenu("TestUpdateLin")]
    public void TestUpdateLine()
    {
        var points = test.Select(x => x.position).ToArray();
        UpdateLine(points);
    }

    private void UpdateLine(Vector3[] points)
    {
        List<Vector3> linePos = new List<Vector3>();
        if (points.Length <= 1)
            return;
        for(int i=0;i<points.Length-1;i++)
        {         
            linePos.AddRange(SlerpPoints(points[i], points[i + 1], -offset, 0.5f).ToArray());
            linePos.Add(points[i + 1]);
        }
        
        lineRenderer.positionCount = linePos.Count;
        lineRenderer.SetPositions(linePos.ToArray());

    }

    private IEnumerable<Vector3> SlerpPoints(Vector3 start,Vector3 end,float offset,float distane)
    {
        List<Vector3> v = new List<Vector3>();
        v.Add(start);
       var lineCentre = (start + end) * 0.5f;
        var lineCentreDis= -Mathf.Abs((start - end).magnitude) * 0.5f;
        var lineWay = (start - end).normalized;
        var center = lineCentre + new Vector3(lineWay.y, -lineWay.x, 0) * (lineCentreDis-offset);
        var _start = start - center;
        var _end = end - center;   
        float dotProduct = Vector3.Dot(_start, _end);

        float magnitudeAX = _start.magnitude;
        float magnitudeBX = _end.magnitude;
        float cosTheta = dotProduct / (magnitudeAX * magnitudeBX);

        float angle = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
          Vector3 cross = Vector3.Cross(_end, _start);
        if (cross.z < 0) 
        {
            angle = 360 - angle;
        }
        float radius = _start.magnitude;

        float arcLength = (angle / 360) * 2 * Mathf.PI * radius;
        float t = 0;
        while (t< arcLength)
        {
            float currentAngle = Mathf.Lerp(0f, angle, t/ arcLength);
            Vector3 interpolatedPosition = Quaternion.AngleAxis(currentAngle,- Vector3.forward) * _start;
            interpolatedPosition += center;

            v.Add(interpolatedPosition);
            t += distane;
        }
        return v;

    }
}
