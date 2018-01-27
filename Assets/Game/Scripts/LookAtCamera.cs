using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour
{
	void Start ()
    {
	}
	
	void Update ()
    {
        transform.LookAt(Camera.main.transform);	
	}
}
