using UnityEngine;

public class PowerUpSpawnpoint : MonoBehaviour
{
    public float Powerup1SpawnRate = 0.5f;
    public float Powerup2SpawnRate = 0.5f;
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
        GameObject archetypePrefab = null;
        if (random < Powerup1SpawnRate)
        {
            archetypePrefab = ResourceManager.GetPrefab("Powerup1");
        }
        else
        {
            archetypePrefab = ResourceManager.GetPrefab("Powerup2");
        }

        Instantiate(archetypePrefab, transform.position, Quaternion.identity).GetComponent<Powerup>().Spawnpoint = this;

        m_SpawnTimer = SpawnCooldown;
        enabled = false;
    }
}