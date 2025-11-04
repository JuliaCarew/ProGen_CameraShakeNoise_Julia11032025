using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Intensity")]
    [Range(0f, 1f)]
    [SerializeField] float horizontalShakeIntensity = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] float verticalShakeIntensity = 0.5f;
    [Range(0f, 10f)]
    [SerializeField] float zoomShakeIntensity = 2f; // FOV change amount
    
    [Header("Shake Settings")]
    [Range(0.1f, 10f)]
    [SerializeField] float frequency = 5f;
    [SerializeField] float baseFOV = 60f; // Base field of view
    
    private float seed;
    private Camera cam;
    private Vector3 originalLocalPosition;
    
    void Start()
    {
        if (cam == null) cam = GetComponentInChildren<Camera>();
        
        // original local position
        originalLocalPosition = transform.localPosition;
        
        // get random seed for each axis 
        seed = Random.value * 100f; 
        
        // set base FOV 
        if (cam != null && baseFOV > 0) cam.fieldOfView = baseFOV;
    }

    private void Update()
    {
        ApplyCameraShake();
    }

    void ApplyCameraShake()
    {
        float time = Time.time * frequency;
        
        // horizontal shake
        float horizontalNoise = Mathf.PerlinNoise(seed, time) * 2f - 1f;
        
        // vertical shake 
        float verticalNoise = Mathf.PerlinNoise(seed + 100f, time + 0.5f) * 2f - 1f;
        
        // apply shake to the camera
        transform.localPosition = originalLocalPosition + new Vector3(
            horizontalNoise * horizontalShakeIntensity,
            verticalNoise * verticalShakeIntensity,
            0f
        );
        
        // apply FOV (zoom in/out)
        if (cam != null)
        {
            float zoomNoise = Mathf.PerlinNoise(seed + 200f, time + 1f) * 2f - 1f;
            float newFOV = baseFOV + (zoomNoise * zoomShakeIntensity);
            cam.fieldOfView = newFOV;
        }
    }
}