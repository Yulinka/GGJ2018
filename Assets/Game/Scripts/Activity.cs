using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity : MonoBehaviour
{
    public int MaxParticipants = 10;
    public List<AiPerson> Participants;
    public List<AiPerson> Reserved;
    public Color Color { get; set; }

    public Activity()
    {
        Participants = new List<AiPerson>();
        Reserved = new List<AiPerson>();
    }

    private void Awake()
    {
        Color = Random.ColorHSV();
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
        if (!Reserved.Remove(participant))
            throw new System.IndexOutOfRangeException("Participant's spot was not reserved");
        Participants.Add(participant);
    }

    public virtual bool Leave(AiPerson participant)
    {
        return Participants.Remove(participant);
    }

    public virtual float GetApproachDistance() { return 0; }
    public abstract string GetName();
}
