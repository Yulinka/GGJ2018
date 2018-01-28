using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour
{
    public bool AxisAligned = false;

	void Update ()
    {
        var target = Camera.main.transform.position;

        if (AxisAligned)
            target = new Vector3(
                target.x,
                transform.position.y,
                target.z);

        transform.LookAt(target);
	}
}
