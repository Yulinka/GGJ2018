using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity : MonoBehaviour
{
    public int MaxParticipants = 10;
    public List<AiPerson> Participants;
    public List<AiPerson> Reserved;

    public Activity()
    {
        Participants = new List<AiPerson>();
        Reserved = new List<AiPerson>();
    }

    public virtual bool CanJoin(AiPerson participant)
    {
        return (Participants.Count + Reserved.Count < MaxParticipants) && !Participants.Contains(participant);
    }

    public virtual bool Reserve(AiPerson participant)
    {
        if (!CanJoin(participant))
            return false;

        Reserved.Add(participant);
        return true;
    }

    public virtual void Join(AiPerson participant)
    {
        if (!Reserved.Contains(participant))
            throw new System.Exception("Not reserved tried to join");

        Reserved.Remove(participant);
        Participants.Add(participant);
    }

    public virtual bool Leave(AiPerson participant)
    {
        return Participants.Remove(participant);
    }

    public virtual float GetApproachDistance() { return 0; }
    public abstract string GetName();
}
