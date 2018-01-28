using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtTarget : MonoBehaviour
{
    public Transform Target;
    public bool Animate = false;

    public Material RenderMaterial;

    void Start()
    {
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderMaterial != null)
            Graphics.Blit(source, destination, RenderMaterial);
        else
            Graphics.Blit(source, destination);
    }

    void Update ()
    {
        if (!Target)
            return;

        float step = Mathf.PI * 2;

        if (Animate)
            step /= 20;

        Vector3 targetDir = Target.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(
            transform.forward, targetDir, step * Time.deltaTime, 0.0F);

        transform.rotation = Quaternion.LookRotation(newDir);
	}
}
