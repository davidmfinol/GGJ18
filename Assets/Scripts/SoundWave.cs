using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundWave : MonoBehaviour
{
    public const float decayRate = 0.1f;

    public float speed;
    public float intensity;

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
            default:
            case SoundObstacleType.Amplifier:
                Amplify(soundObstacle.transform);
                break;
        }
    }

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

    void Update()
    {
        intensity -= decayRate * Time.deltaTime;
        if (intensity <= 0)
            Destroy(gameObject);
    }
}
