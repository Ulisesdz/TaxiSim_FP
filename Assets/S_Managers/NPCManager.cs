using UnityEngine;
using Dreamteck.Splines;
using System.Collections.Generic;  // Necesario para listas

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

    public float minPercentDistance = 0.1f; // Distancia mínima en porcentaje

    void Start()
    {
        // Listas para almacenar los porcentajes ya utilizados
        List<double> usedCarPercents = new List<double>();
        List<double> usedPersonPercents = new List<double>();

        // Generamos los coches
        for (int i = 0; i < numberOfCars; i++)
        {
            Vector3 carPosition = new Vector3(i * 20, 0, 0);  // Posición de cada coche
            double carStartPercent = GetUniquePercent(usedCarPercents);
            carFactory.CreateCar(carPosition, carSpline, carStartPercent, speedCar);
        }

        // Generamos las personas
        for (int i = 0; i < numberOfPeople; i++)
        {
            Vector3 personPosition = new Vector3(i * 20, 0, 10);  // Posición de cada persona
            double personStartPercent = GetUniquePercent(usedPersonPercents);
            personFactory.CreatePerson(personPosition, personSpline, personStartPercent, speedPerson);
        }
    }

    double GetUniquePercent(List<double> usedPercents)
    {
        double newPercent;
        bool isUnique;

        do
        {
            newPercent = (double)Random.Range(0f, 1f);  // Genera un porcentaje aleatorio
            isUnique = true;

            // Verifica que no esté demasiado cerca de los porcentajes ya usados
            foreach (double percent in usedPercents)
            {
                if (Mathf.Abs((float)(newPercent - percent)) < minPercentDistance)
                {
                    isUnique = false;
                    break;
                }
            }
        }
        while (!isUnique);

        usedPercents.Add(newPercent);  // Agrega el porcentaje a la lista de usados
        return newPercent;
    }
}


