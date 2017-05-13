﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Bomb : MonoBehaviour
{
    public float ExpandTime = 2.0f;
    public float MaxRadius = 3.0f;

    private float m_Timer;
    private List<GameObject> m_PushBackObjs;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        m_Timer = 0;
        m_PushBackObjs = new List<GameObject>();
    }

    void FixedUpdate()
    {
        m_Timer += Time.deltaTime;

        float scale = m_Timer * MaxRadius;
        transform.localScale = new Vector3(scale, scale, scale);

        if (m_Timer > ExpandTime)
        {
            m_PushBackObjs.ForEach(p =>
            {
                if (p == null)
                    return;

                Rigidbody rigidbody = p.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            });
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!m_PushBackObjs.Contains(collision.gameObject))
            m_PushBackObjs.Add(collision.gameObject);
    }
}