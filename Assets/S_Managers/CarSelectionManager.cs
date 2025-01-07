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
        // Load the previously selected car index (default to 0 if none saved)
        selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", 1);

        // Instantiate the selected car prefab at the spawn position and rotation
        GameObject selectedCar = Instantiate(carPrefabs[selectedCarIndex], spawnPosition, spawnRotation);

        // Get the CameraFollow component from the main camera
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            // Set the target of the camera to the selected car's transform
            cameraFollow.objetivo = selectedCar.transform;
        }
        else
        {
            Debug.LogError("CameraFollow script not found on the main camera!");
        }

        // Get the Rigidbody of the selected car
        Rigidbody carRigidbody = selectedCar.GetComponent<Rigidbody>();


        if (speedmeter != null && carRigidbody != null)
        {
            // Set the Speedometer's target Rigidbody to the selected car's Rigidbody
            speedmeter.target = carRigidbody;
        }
        else
        {
            if (carRigidbody == null)
            {
                Debug.LogError("The selected car prefab does not have a Rigidbody component!");
            }
        }
    }
}
