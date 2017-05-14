using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaveManager : GameSingleton<WaveManager>
{
    public int BossWaveGoldRequirement = 75;

    private List<Wave> m_Waves;
    private int m_CurrentWave;

    enum State
    {
        Cooldown,
        Play,
        End
    }
    private State m_State;
    private float m_CooldownTimer;
    private int m_AIKills;
    private int m_AISpawned;

    void Start()
    {
        m_Waves = new List<Wave>();
        m_Waves.Add(transform.Find("Wave1").GetComponent<Wave>());
        m_Waves.Add(transform.Find("Wave2").GetComponent<Wave>());
        m_Waves.Add(transform.Find("Wave3").GetComponent<Wave>());
        m_Waves.Add(transform.Find("Wave4").GetComponent<Wave>());

        m_CurrentWave = 0;
        m_State = State.Cooldown;
        m_CooldownTimer = m_Waves[m_CurrentWave].CooldownTime;
        m_AIKills = 0;
        m_AISpawned = 0;

        FindObjectsOfType<SpawnPoint>().ToList().ForEach(p => p.enabled = false);
    }

    void Update()
    {
        switch (m_State)
        {
            case State.Cooldown:
                Cooldown();
                break;
            case State.Play:
                Play();
                break;
        }
    }

    private void Cooldown()
    {
        m_CooldownTimer -= Time.deltaTime;
        if (m_CooldownTimer < 0)
        {
            m_State = State.Play;
            m_AIKills = 0;
            m_AISpawned = 0;
            if (m_Waves[m_CurrentWave].SpawnPoints.Count > 0)
                m_Waves[m_CurrentWave].SpawnPoints.ForEach(p => p.enabled = true);
        }
    }

    private void Play()
    {
        if (m_AIKills >= m_Waves[m_CurrentWave].AICount)
        {
            m_CurrentWave++;
            if (m_CurrentWave == m_Waves.Count - 1)
            {
                if (FindObjectOfType<Player>().Gold >= BossWaveGoldRequirement)
                {
                    m_State = State.Cooldown;
                    m_CooldownTimer = m_Waves[m_CurrentWave].CooldownTime;
                }
                else
                {
                    m_State = State.End;
                    End();
                }
            }
            else if (m_CurrentWave == m_Waves.Count)
            {
                m_State = State.End;
                End();
            }
            else
            {
                m_State = State.Cooldown;
                m_CooldownTimer = m_Waves[m_CurrentWave].CooldownTime;
            }
            FindObjectsOfType<SpawnPoint>().ToList().ForEach(p => p.enabled = false);
        }
    }

    public void AddKill()
    {
        m_AIKills += 1;
    }

    public void End()
    {
        FindObjectOfType<PlayerController>().enabled = false;
    }

    public bool RequestSpawn()
    {
        if (m_AISpawned + 1 <= m_Waves[m_CurrentWave].AICount)
        {
            m_AISpawned++;
            return true;
        }
        return false;
    }

    public bool IsCooldown { get { return m_State == State.Cooldown; } }
    public float CooldownTimer { get { return m_CooldownTimer; } }
    public int AIKills { get { return m_AIKills; } }
    public int AICount { get { return m_Waves[m_CurrentWave].AICount; } }
    public int CurrentWave { get { return m_CurrentWave; } }
    public bool IsEnded { get { return m_State == State.End; } }
    public bool IsBossWave { get { return m_CurrentWave == m_Waves.Count - 1 && !IsEnded; } }
}