using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Variables

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
    [SerializeField] float baseFOV = 60f; // base field of view

    [Header("Rotation")]
    [Range(0f, 1f)]
    [SerializeField] float rotationIntensityX;
    [Range(0f, 1f)]
    [SerializeField] float rotationIntensityY;
    [Range(0f, 10f)]
    [SerializeField] float rotationMagnitude = 2f;
    [Range(0f, 10f)]
    [SerializeField] float rotationShakeDuration = 2f;

    private float seed;
    private Camera cam;
    private Vector3 originalLocalPosition;

    #endregion

    void Start()
    {
        if (cam == null) cam = GetComponentInChildren<Camera>();
        
        // original local position
        originalLocalPosition = transform.localPosition;
        
        // get random seed for each axis 
        seed = UnityEngine.Random.value * 100f; 
        
        // set base FOV 
        if (cam != null && baseFOV > 0) cam.fieldOfView = baseFOV;
    }

    private void Update()
    {
        // ApplyCameraShake();
        StartCoroutine(ApplyCameraRotation());
    }

    public void TestButton()
    {
        // ApplyCameraShake();
        StartCoroutine(ApplyCameraRotation());
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

    IEnumerator ApplyCameraRotation()
    {
        float elapsedTime = 0f;

        while(elapsedTime < rotationShakeDuration)
        {
            float time = Time.time * frequency;

            // perlin noise floats
            float rotationX = Mathf.PerlinNoise(seed + rotationIntensityX, time) * rotationMagnitude;
            float rotationY = Mathf.PerlinNoise(seed + 10 + rotationIntensityY, time) * rotationMagnitude;

            // set position to quaternion euler
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);

            elapsedTime++;
            yield return null;
        }

        // reset rotation
        transform.rotation = transform.localRotation;
    }
}
// animation curve to smoothly decrease intensity over duration
// multiple shake sources
// interface for shakers, heap to manage current shakes with different durations