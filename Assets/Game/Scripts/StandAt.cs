using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StandAt : Activity
{       
    public StandAt()
    {
        MaxParticipants = 1;
    }

    public override string GetName()
    {
        return "Stand At";
    }

    public override float GetApproachDistance()
    {
        return 0.5f;
    }
}
