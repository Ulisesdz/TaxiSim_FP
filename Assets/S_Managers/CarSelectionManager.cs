using UnityEngine;

public class CarSelectionManager : MonoBehaviour
{
    public GameObject[] carPrefabs;  // Array of car prefabs (assign in the Inspector)
    public Vector3 spawnPosition = new Vector3(-52, 2.14f, 25.2f); // Position for spawning the car
    public Quaternion spawnRotation = Quaternion.Euler(0, 0, 0); // Rotation for the car
    public Camera mainCamera; // Reference to the main camera
    public Speedometer speedmeter; // Reference to the Speedometer script

    private int selectedCarIndex;

    void Start()
    {
        // Load the previously selected car index
        selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", 0);

        // Validate the car index and prefabs array
        if (carPrefabs == null || carPrefabs.Length == 0)
        {
            Debug.LogError("No car prefabs assigned!");
            return;
        }
        selectedCarIndex = Mathf.Clamp(selectedCarIndex, 0, carPrefabs.Length - 1);

        // Instantiate the selected car prefab
        GameObject selectedCar = Instantiate(carPrefabs[selectedCarIndex], spawnPosition, spawnRotation);

        // Set up camera and speedometer
        if (mainCamera != null)
        {
            CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.objetivo = selectedCar.transform;
            }
            else
            {
                Debug.LogError("CameraFollow script not found on the main camera!");
            }
        }

        if (speedmeter != null)
        {
            Rigidbody carRigidbody = selectedCar.GetComponent<Rigidbody>();
            if (carRigidbody != null)
            {
                speedmeter.target = carRigidbody;
            }
            else
            {
                Debug.LogError("The selected car prefab does not have a Rigidbody component!");
            }
        }
    }
}