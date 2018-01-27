using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundWave : MonoBehaviour
{
    public float speed;
    public float intensity;

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 curDir = transform.TransformDirection(Vector3.forward);
        Vector3 newDir = Vector3.Reflect(curDir, contact.normal);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        GetComponent<Rigidbody>().velocity = newDir.normalized * speed;

        SoundObstacle soundObstacle = collision.gameObject.GetComponent<SoundObstacle>();
        if (soundObstacle != null)
            intensity -= soundObstacle.effect;
        if (intensity <= 0)
            Destroy(gameObject);
    }
}
