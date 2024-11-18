using Dreamteck.Splines;
using UnityEngine;

public class PersonNPCFactory : NPCFactory
{
    public void CreatePerson(Vector3 position, SplineComputer personSplineComputer, double startPercent, float speedPerson)
    {
        CreateNPC(position, personSplineComputer, startPercent, speedPerson);
    }
}
