using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 maximumTranslationShake = Vector3.one * 0.5f;
    [SerializeField] float frequency = 5;

    private float seed;

    void Start()
    {
        seed = Random.value; // random seed to have unique shake values
    }

    private void Update()
    {
        TransformWithNoise();
    }

    void TransformWithNoise()
    {
        transform.localPosition = new Vector3(
        maximumTranslationShake.x * Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1, // horizontal
        maximumTranslationShake.y * Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1, // vertical
        maximumTranslationShake.z * Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1 // a bit of zoom
        );
    }

    void ShakeZoom()
    {
        // shake camera in/out using Perlin noise   
    }
}