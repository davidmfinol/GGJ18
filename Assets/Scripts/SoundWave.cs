using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float intensity;

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        float dot = Vector3.Dot(contact.normal, (-transform.forward));
        dot *= 2;
        Vector3 reflection = contact.normal * dot;
        reflection = reflection + transform.forward;
        GetComponent<Rigidbody>().velocity = reflection.normalized * 15.0f;
    }
}
