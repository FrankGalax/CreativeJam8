using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 5.0f;
    public float DestroyRange = 20.0f;

    void FixedUpdate()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;

        if (transform.position.magnitude > DestroyRange)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "KillBullet"))
        {
            Destroy(gameObject);
        }
    }
}