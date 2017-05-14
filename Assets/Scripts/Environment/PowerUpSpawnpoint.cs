using UnityEngine;

public class PowerUpSpawnpoint : MonoBehaviour
{
    public float Powerup1SpawnRate = 0.33f;
    public float Powerup2SpawnRate = 0.33f;
    public float Powerup3SpawnRate = 0.33f;
    public float SpawnCooldown = 10.0f;

    private float m_SpawnTimer;

    void Awake()
    {
        DestroyImmediate(transform.Find("HelperGraphic").gameObject);
    }

    void Start()
    {
        m_SpawnTimer = SpawnCooldown;
    }

    void Update()
    {
        m_SpawnTimer -= Time.deltaTime;
        if (m_SpawnTimer > 0)
            return;

        float random = UnityEngine.Random.value;
        GameObject powerupPrefab = null;
        if (random < Powerup1SpawnRate)
        {
            powerupPrefab = ResourceManager.GetPrefab("Powerup1");
        }
        else if (random < (Powerup1SpawnRate + Powerup2SpawnRate))
        {
            powerupPrefab = ResourceManager.GetPrefab("Powerup2");
        }
        else
        {
            powerupPrefab = ResourceManager.GetPrefab("Powerup3");
        }

        Instantiate(powerupPrefab, transform.position, Quaternion.identity).GetComponent<Powerup>().Spawnpoint = this;

        m_SpawnTimer = SpawnCooldown;
        enabled = false;
    }
}