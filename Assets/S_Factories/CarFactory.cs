using Dreamteck.Splines;
using UnityEngine;

public class CarFactory : NPCFactory
{
    public void CreateCar(Vector3 position, SplineComputer carSplineComputer, double startPercent, float speedCar)
    {
        CreateNPC(position, carSplineComputer, startPercent, speedCar);
    }
}