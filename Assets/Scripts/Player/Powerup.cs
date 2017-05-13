using UnityEngine;

public class Powerup : MonoBehaviour
{
    public int Id;
    public PowerUpSpawnpoint Spawnpoint { get; set; }
    public float UpDownSpeed;
    public float RotationSpeed;

    private float m_Timer;

    void Update()
    {
        Transform graphics = transform.Find("Graphics");

        m_Timer += Time.deltaTime;
        Vector3 position = graphics.position;
        position.y = (Mathf.Sin(m_Timer * UpDownSpeed * Time.deltaTime) + 1.0f) / 2.0f;
        transform.position = position;

        float angle = m_Timer * RotationSpeed * Time.deltaTime;
        Debug.Log(angle);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Helpers.CheckObjectTag(collider.gameObject, "Player"))
        {
            collider.GetComponent<PlayerController>().ApplyPowerup(Id);
            Spawnpoint.enabled = true;
        }
    }
}