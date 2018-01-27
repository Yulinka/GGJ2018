using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public Activity FindActivity(MonoBehaviour participant, System.Type typeFilter = null)
    {
        var children = GetComponentsInChildren(typeFilter == null ? typeFilter : typeof(Activity));

        int start = Random.Range(0, children.Length);
        for (int i = start, n = 0; n < children.Length; i = (i + 1) % children.Length, ++n)
        {
            if (((Activity)children[i]).CanJoin(participant))
                return (Activity)children[i];
        }

        return null; //no activities found
    }
}
