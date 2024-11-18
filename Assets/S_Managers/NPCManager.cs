using UnityEngine;
using Dreamteck.Splines;

public class NPCManager : MonoBehaviour
{
    public CarFactory carFactory;      // Fábrica de coches
    public PersonNPCFactory personFactory; // Fábrica de NPCs

    public int numberOfCars = 3;       // Número de coches
    public int numberOfPeople = 3;     // Número de personas

    public SplineComputer carSpline;   // Spline para los coches
    public SplineComputer personSpline; // Spline para las personas

    public float speedCar = 6;    // Velocidad de los coches
    public float speedPerson = 1.3f;  // Velocidad de las personas

    void Start()
    {
        // Generamos los coches
        for (int i = 0; i < numberOfCars; i++)
        {
            Vector3 carPosition = new Vector3(i * 20, 0, 0);  // Posición de cada coche
            double carStartPercent = (double)Random.Range(0f, 1f);  // Genera un porcentaje aleatorio en el spline
            carFactory.CreateCar(carPosition, carSpline, carStartPercent, speedCar);
        }

        // Generamos las personas
        for (int i = 0; i < numberOfPeople; i++)
        {
            Vector3 personPosition = new Vector3(i * 20, 0, 10);  // Posición de cada persona
            double personStartPercent = (double)Random.Range(0f, 1f);  // Genera un porcentaje aleatorio en el spline
            personFactory.CreatePerson(personPosition, personSpline, personStartPercent, speedPerson);
        }
    }
}


