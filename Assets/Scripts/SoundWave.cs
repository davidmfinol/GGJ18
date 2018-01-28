using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundWave : MonoBehaviour
{
    public const float DecayRate = 0.1f;

    public float speed;
    public float intensity;
    public AudioSource waveAudioSource;
    public AudioClip[] waveImpactSound;

    public AudioClip reflectorSound;
    public AudioClip speedBoostSound;
    public AudioClip waterSound;
    public AudioClip microphoneSound;
    public AudioClip amplifierSound;

    void OnCollisionEnter(Collision collision)
    {
        SoundObstacle soundObstacle = collision.gameObject.GetComponent<SoundObstacle>();
         if (soundObstacle == null) {
            Reflect(collision.contacts[0]);
            return;
        }

        intensity += soundObstacle.IntensityEffect;
        switch (soundObstacle.type) {
            default:
            case SoundObstacleType.Reflector:
                Reflect(collision.contacts[0]);
                AudioManager.Play(reflectorSound, intensity * 0.6f);
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SoundObstacle soundObstacle = other.gameObject.GetComponentInParent<SoundObstacle>();
        if (soundObstacle == null)
            return;

        intensity += soundObstacle.IntensityEffect;
        switch (soundObstacle.type) {
            case SoundObstacleType.SpeedBoost:
                speed += soundObstacle.speedAmount;
                Amplify(soundObstacle.transform);
                AudioManager.Play(speedBoostSound, intensity * 0.6f);
                break;
            case SoundObstacleType.Water:
                Refract(other);
                AudioManager.Play(waterSound, intensity * 0.6f);
                break;
            case SoundObstacleType.Microphone:
                Amplify(soundObstacle.microphoneTarget.transform);
                AudioManager.Play(microphoneSound, intensity * 0.6f);
                break;
            default:
            case SoundObstacleType.Amplifier:
                Amplify(soundObstacle.transform);
                AudioManager.Play(amplifierSound, intensity * 0.6f);
                break;
        }
    }

    public void Reflect(ContactPoint contact)
    {
        Vector3 curDir = transform.TransformDirection(Vector3.forward);
        Vector3 newDir = Vector3.Reflect(curDir, contact.normal);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        GetComponent<Rigidbody>().velocity = newDir.normalized * speed;
        PlayImpactSound();
    }

    public void Amplify(Transform target)
    {
        float starty = transform.position.y;
        transform.position = new Vector3(target.position.x, starty, target.position.z);
        Vector3 forward = target.forward;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, forward);
        GetComponent<Rigidbody>().velocity = forward * speed;
        PlayImpactSound();
    }

    public void Refract(Collider collider)
    {
        Quaternion rotate45 = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.right), 45);
        transform.rotation = rotate45;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        PlayImpactSound();
    }

    public void PlayImpactSound()
    {
        if (waveImpactSound.Length != 0)
        {
            waveAudioSource.clip = waveImpactSound[Random.Range(0, waveImpactSound.Length)];
            waveAudioSource.volume = intensity*0.6f;
            waveAudioSource.Play();
        }
    }

    void Update()
    {
        intensity -= DecayRate * Time.deltaTime;
        if (intensity <= 0)
            Destroy(gameObject);
    }
}
