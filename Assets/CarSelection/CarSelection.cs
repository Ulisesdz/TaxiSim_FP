using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMesh Pro

public class CarSelection : MonoBehaviour
{
    public GameObject[] cars_list;
    public Button next;
    public Button prev;
    public Button buybotton;
    public TextMeshProUGUI carPriceText; // Updated to TextMeshProUGUI
    public int[] carPrices;

    public CarPointsManager carPointsManager;
        
    int carIndex;

    void Start()
    {
        carIndex = PlayerPrefs.GetInt("carIndex", 0);

        for (int i = 0; i < cars_list.Length; i++)
        {
            cars_list[i].SetActive(false);
        }
        cars_list[carIndex].SetActive(true);

        UpdateCarPriceUI();
    }

    void Update()
    {
        next.interactable = carIndex < cars_list.Length - 1;
        prev.interactable = carIndex > 0;
    }

    public void Next()
    {
        if (carIndex < cars_list.Length - 1)
        {
            carIndex++;
            UpdateCarSelection();
        }
    }

    public void Prev()
    {
        if (carIndex > 0)
        {
            carIndex--;
            UpdateCarSelection();
        }
    }

    public void Jugar()
    {
        PlayerPrefs.SetInt("carIndex", carIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game_scene");
    }

    void UpdateCarSelection()
    {
        carIndex = Mathf.Clamp(carIndex, 0, cars_list.Length - 1);

        for (int i = 0; i < cars_list.Length; i++)
        {
            cars_list[i].SetActive(false);
        }
        cars_list[carIndex].SetActive(true);

        PlayerPrefs.SetInt("carIndex", carIndex);
        PlayerPrefs.Save();

        UpdateCarPriceUI();
    }

    void UpdateCarPriceUI()
    {
        if (carPrices != null && carIndex < carPrices.Length)
        {
            carPriceText.text = "Price: $" + carPrices[carIndex];
        }
        else
        {
            carPriceText.text = "Price: N/A";
        }
    }

    public void BuyCar()
    {
        int carPrice = carPrices[carIndex];
        int currentPoints = carPointsManager.GetCurrentPoints();

        if (currentPoints >= carPrice)
        {
            // Deduct points and save purchase
            carPointsManager.SubtractPoints(carPrice);

            carPointsManager.UpdatePointsDisplay();
            Debug.Log($"Car purchased! Remaining points: {currentPoints - carPrice}");
        }
        else
        {
            Debug.Log("Not enough points to purchase this car.");
        }
    }
}





