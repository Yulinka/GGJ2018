using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraSystem : MonoBehaviour
{
    public GameObject Target;
    private float offsetZ;


	void Start ()
    {

        offsetZ = Target.transform.position.z - transform.position.z;
	}
	
	void Update()
    {
        float newZ = Mathf.Lerp(
            Target.transform.position.z,
            Target.transform.position.z - offsetZ,
            0.50f);

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            newZ
        );

        Camera.main.transform.LookAt(Target.transform);
	}
}
