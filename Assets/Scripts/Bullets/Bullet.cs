using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 5.0f;
    public float DestroyRange = 20.0f;

    void Start()
    {
        DoStart();
    }
    protected virtual void DoStart() { }

    void FixedUpdate()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;

        if (transform.position.magnitude > DestroyRange)
            Destroy(gameObject);
    }
    
    void Update()
    {
        DoUpdate();
    }
    protected virtual void DoUpdate() { }

    void OnCollisionEnter(Collision collision)
    {
        DoOnCollisionEnter(collision);

        if (Helpers.CheckObjectTag(collision.gameObject, "KillBullet"))
        {
            Destroy(gameObject);
        }
    }
    protected virtual void DoOnCollisionEnter(Collision collision) { }
}