using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform Target;
    public bool Animate = false;

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
