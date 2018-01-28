using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundWave : MonoBehaviour
{
    public const float decayRate = 0.1f;

    public float speed;
    public float intensity;
    public AudioSource waveAudioSource;
    public AudioClip[] waveImpactSound;

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
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SoundObstacle soundObstacle = other.gameObject.GetComponent<SoundObstacle>();
        if (soundObstacle == null)
            return;

        intensity += soundObstacle.IntensityEffect;
        switch (soundObstacle.type) {
            case SoundObstacleType.Water:
                Refract(other);
                break;
            case SoundObstacleType.Microphone:
                Amplify(soundObstacle.target.transform);
                break;
            default:
            case SoundObstacleType.Amplifier:
                Amplify(soundObstacle.transform);
                break;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    SoundObstacle soundObstacle = other.gameObject.GetComponent<SoundObstacle>();
    //    if (soundObstacle == null || soundObstacle.type != SoundObstacleType.Water)
    //        return;
    //    Refract(other);
    //}

    public void Reflect(ContactPoint contact)
    {
        Vector3 curDir = transform.TransformDirection(Vector3.forward);
        Vector3 newDir = Vector3.Reflect(curDir, contact.normal);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        GetComponent<Rigidbody>().velocity = newDir.normalized * speed;
    }

    public void Amplify(Transform target)
    {
        transform.position = target.position;
        Vector3 forward = target.forward;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, forward);
        GetComponent<Rigidbody>().velocity = forward * speed;
    }

    public void Refract(Collider collider)
    {
        Vector3 closestPoint = collider.ClosestPointOnBounds(transform.position);
        closestPoint.y = 2;
        Vector3 normal = (collider.transform.position - closestPoint).normalized;
        float right = closestPoint.x - collider.transform.position.x;
        float front = closestPoint.z - collider.transform.position.z;
        if (Mathf.Abs(right) > Mathf.Abs(front)) { // sides
            if (right > 0)
                normal = Vector3.right;
            else
                normal = Vector3.left;
        } else { // faces
            if (front > 0)
                normal = Vector3.forward;
            else
                normal = Vector3.back;
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, normal);
        GetComponent<Rigidbody>().velocity = normal * speed;
    }

    private void PlayImpactSound()
    {
        if (waveImpactSound.Length != 0)
        {
            waveAudioSource.clip = waveImpactSound[Random.Range(0, waveImpactSound.Length)];
            waveAudioSource.Play();
        }
    }

    void Update()
    {
        intensity -= decayRate * Time.deltaTime;
        if (intensity <= 0)
            Destroy(gameObject);
    }
}
