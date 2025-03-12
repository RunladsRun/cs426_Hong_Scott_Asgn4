using UnityEngine;

public class BouncePlayerBack : MonoBehaviour
{
    public float bounceForce = 10f; // Adjust the bounce strength

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 bounceDirection = collision.transform.position - transform.position;
            bounceDirection.Normalize();

            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}