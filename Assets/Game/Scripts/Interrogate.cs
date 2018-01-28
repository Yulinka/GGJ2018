using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrogate : MonoBehaviour
{
    public Color HighlightColor = Color.yellow;
    public float SearchRadius = 2;

    void DrawTriangle(Vector3 origin, Color color)
    {
        var a = new Vector3(0, 0, 0.1f);
        var b = new Vector3(0.1f, 0, -0.1f);
        var c = new Vector3(-0.1f, 0, -0.1f);
        Debug.DrawRay(origin + a, b - a, color);
        Debug.DrawRay(origin + c, a - c, color);
        Debug.DrawRay(origin + b, c - b, color);
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var nearby = Physics.OverlapSphere(transform.position, SearchRadius);
        foreach (var obj in nearby)
        {
            var ai = obj.GetComponent<AiPerson>();
            if (ai != null)
            {
                if (Vector2.Distance(ai.transform.position, transform.position) < SearchRadius)
                {
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit);
                    bool isHovering = hit.transform == ai.transform;
                    DrawTriangle(ai.transform.position,
                        isHovering ? Color.red : HighlightColor);

                    if (isHovering && Input.GetMouseButtonDown(0))
                    {
                        ai.Interrogate();
                        break;
                    }
                }
            }
        }
    }
}
