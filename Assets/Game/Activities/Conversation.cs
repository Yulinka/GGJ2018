using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Activity : MonoBehaviour
{
    public int MaxParticipants = 10;
    public List<AiPerson> Participants;

    public Activity()
    {
        Participants = new List<AiPerson>();
    }

    public virtual bool CanJoin(AiPerson participant)
    {
        return (Participants.Count < MaxParticipants) && !Participants.Contains(participant);
    }

    public virtual bool Join(AiPerson participant)
    {
        if (CanJoin(participant))
        {
            Participants.Add(participant);
            return true;
        }
        return false;
    }

    public virtual bool Leave(AiPerson participant)
    {
        return Participants.Remove(participant);
    }

    public virtual float GetApproachDistance() { return 0; }
}

public class Conversation : Activity
{
    float actorSize = 1.5f;

    public override float GetApproachDistance()
    {
        var theta = (1f / Participants.Count) * Mathf.PI * 2;
        return ((actorSize + 0.5f * 2) / theta) + actorSize;
    }

    void DistributeParticipants()
    {
        var theta = (1f / Participants.Count) * Mathf.PI * 2;
        var radius = (actorSize + 0.5f * 2) / theta;

        for (int i = 0; i < Participants.Count; ++i)
        {
            Participants[i].ConversationMove(transform.position + radius * new Vector3(
                Mathf.Cos(i * theta),
                0,
                Mathf.Sin(i * theta)
            ));
        }
    }

    public override bool Join(AiPerson participant)
    {
        if (base.Join(participant))
        {
            DistributeParticipants();
            return true;
        }
        return false;
    }

    public override bool Leave(AiPerson participant)
    {
        if (base.Leave(participant))
        {
            DistributeParticipants();
            return true;
        }
        return false;
    }
}
