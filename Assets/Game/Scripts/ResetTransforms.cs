using UnityEngine;

[ExecuteInEditMode]
public class ResetTransforms : MonoBehaviour
{
    public bool ResetPosition = false;
    public bool ResetScale = false;
    public bool ResetRotation = false;

    void Start()
    {
        enabled = Application.isEditor;
    }

    void Update()
    {
        if (ResetPosition)
            transform.localPosition = Vector3.zero;
        if (ResetRotation)
            transform.localRotation = Quaternion.identity;
        if (ResetScale)
            transform.localScale = Vector3.one;
    }
}