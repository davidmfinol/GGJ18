using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public GameObject soundPrefab;
    public Vector3 startPosition = new Vector3(-2.5f, 2, 5);
    public Vector3 startLookDirection = new Vector3(0, 0, -1);
    public Vector3 initialVelocity = new Vector3(0, 0, -10f);
    public float initialIntensity = 1.0f;

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
            StartSoundWave();
    }

    public void StartSoundWave()
    {
        Quaternion startRotation = Quaternion.LookRotation(startLookDirection);
        GameObject newSoundObject = Instantiate(soundPrefab, startPosition, startRotation);
        newSoundObject.GetComponent<Rigidbody>().velocity = initialVelocity;
        newSoundObject.GetComponent<SoundWave>().intensity = initialIntensity;
    }
}
