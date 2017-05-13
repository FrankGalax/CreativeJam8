using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float Archetype1SpawnRate = 0.34f;
    public float Archetype2SpawnRate = 0.33f;
    public float Archetype3SpawnRate = 0.33f;

    public float SpawnCooldown = 5.0f;

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
        Spawn();
    }

    private void Spawn()
    {
        m_SpawnTimer -= Time.deltaTime;
        if (m_SpawnTimer > 0)
            return;

        float random = UnityEngine.Random.value;
        GameObject archetypePrefab = null;
        if (random < Archetype1SpawnRate)
        {
            archetypePrefab = ResourceManager.GetPrefab("Archetype1");
        }
        else if (random < Archetype2SpawnRate)
        {
            archetypePrefab = ResourceManager.GetPrefab("Archetype2");
        }
        else
        {
            archetypePrefab = ResourceManager.GetPrefab("Archetype3");
        }

        Vector3 direction = -transform.position.normalized;
        Instantiate(archetypePrefab, transform.position, Quaternion.LookRotation(direction));

        m_SpawnTimer = SpawnCooldown;
    }
}