using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Activity : MonoBehaviour
{
    public int MaxParticipants = 10;
    public List<MonoBehaviour> Participants;

    public Activity()
    {
        Participants = new List<MonoBehaviour>();
    }

    public virtual bool CanJoin(MonoBehaviour participant)
    {
        return (Participants.Count < MaxParticipants) && !Participants.Contains(participant);
    }

    public virtual bool Join(MonoBehaviour participant)
    {
        if (CanJoin(participant))
        {
            Participants.Add(participant);
            return true;
        }
        return false;
    }

    public virtual bool Leave(MonoBehaviour participant)
    {
        return Participants.Remove(participant);
    }
}

public class Conversation : Activity
{
    void DistributeParticipants()
    {
        var theta = (1f / Participants.Count) * Mathf.PI * 2;
        var radius = (1 + 0.5f * 2) / theta;

        for (int i = 0; i < Participants.Count; ++i)
        {
            Participants[i].transform.localPosition = radius * new Vector3(
                Mathf.Cos(i * theta),
                0,
                Mathf.Sin(i * theta)
            );
        }
    }

    public override bool Join(MonoBehaviour participant)
    {
        if (base.Join(participant))
        {
            DistributeParticipants();
            return true;
        }
        return false;
    }

    public override bool Leave(MonoBehaviour participant)
    {
        if (base.Leave(participant))
        {
            DistributeParticipants();
            return true;
        }
        return false;
    }
}
