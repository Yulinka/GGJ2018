using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraSystem : MonoBehaviour
{
    public GameObject Target;
    private float offsetZ;
    private bool animate;

    public void LookAtAnimate(GameObject target)
    {
        Target = target;
        animate = true;
    }

	private void Start ()
    {

        offsetZ = Target.transform.position.z - transform.position.z;
	}
	
	private void Update()
    {
        if (animate)
        {
            float step = (Mathf.PI * 2) / 20;

            Vector3 targetDir = Target.transform.position - Camera.main.transform.position;

            Vector3 newDir = Vector3.RotateTowards(
                Camera.main.transform.forward,
                targetDir,
                step * Time.deltaTime,
                0.0F);
            
            Camera.main.transform.rotation = Quaternion.LookRotation(newDir);
        }
        else
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
}
