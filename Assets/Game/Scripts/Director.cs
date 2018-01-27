using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public Activity FindActivity(AiPerson participant, System.Type typeFilter)
    {
        Component[] activities = GetComponentsInChildren(typeFilter);

        int start = Random.Range(0, activities.Length);

        for (int i = start, n = 0; n < activities.Length; i = (i + 1) % activities.Length, ++n)
        {
            Activity activity = activities[i] as Activity;

            if (activity.CanJoin(participant))
                return activity;
        }

        return null;
    }
}
