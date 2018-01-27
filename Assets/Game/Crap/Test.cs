using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var activity = FindObjectOfType<Activity>();
        if (activity != null)
            activity.Join(GetComponent<AiPerson>());
	}

	// Update is called once per frame
	void Update () {

	}

    private void OnDestroy()
    {
        var activity = FindObjectOfType<Activity>();
        if (activity != null)
            activity.Leave(GetComponent<AiPerson>());
    }
}
