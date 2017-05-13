using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 4.0f;
    public Vector3 GunOffset;
    public float FireCooldown = 0.25f;
    public bool PS4;
    public float ShieldTime = 5.0f;
    public float DoubleGunTime = 5.0f;

    public bool IsInteracting { get; private set; }
    private Rigidbody m_Rigidbody;
    private float m_FireTimer;
    private bool m_Shielded;
    private bool m_DoubleGun;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Shielded = false;
        m_DoubleGun = false;
    }

    void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    void Update()
    {
        UpdateShooting();
        UpdateInteractions();
    }

    public void ApplyPowerup(int id)
    {
        if (id == 0)
        {
            if (!m_Shielded)
                StartCoroutine(Shield());
        }
        else
        {
            if (!m_DoubleGun)
                StartCoroutine(DoubleGun());
        }
    }

    private IEnumerator Shield()
    {
        Transform shield = transform.Find("Shield");
        shield.gameObject.SetActive(true);
        m_Shielded = true;

        yield return new WaitForSeconds(ShieldTime);

        shield.gameObject.SetActive(false);
        m_Shielded = false;
    }

    private IEnumerator DoubleGun()
    {
        m_DoubleGun = true;

        yield return new WaitForSeconds(DoubleGunTime);

        m_DoubleGun = false;
    }

    private void UpdatePosition()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            m_Rigidbody.velocity = Vector3.zero;
            return;
        }

        Vector3 movement = (horizontal * Vector3.right + vertical * Vector3.forward) * Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(transform.position + movement);
    }

    private void UpdateRotation()
    {
        float rightHorizontal = PS4 ? Input.GetAxis("RightHorizontalPS4") : Input.GetAxis("RightHorizontal");
        float rightVertical = PS4 ? Input.GetAxis("RightVerticalPS4") : Input.GetAxis("RightVertical");

        if (rightHorizontal == 0 && rightVertical == 0)
        {
            m_Rigidbody.angularVelocity = Vector3.zero;
            return;
        }

        Vector3 direction = (rightHorizontal * Vector3.right + rightVertical * Vector3.forward).normalized;

        m_Rigidbody.MoveRotation(Quaternion.LookRotation(direction));
    }

    private void UpdateShooting()
    {
        m_FireTimer = Mathf.Max(m_FireTimer - Time.deltaTime, 0.0f);

        float rightHorizontal = PS4 ? Input.GetAxis("RightHorizontalPS4") : Input.GetAxis("RightHorizontal");
        float rightVertical = PS4 ? Input.GetAxis("RightVerticalPS4") : Input.GetAxis("RightVertical");

        if (Mathf.Abs(rightHorizontal) <= 0.5f && Mathf.Abs(rightVertical) <= 0.5f)
            return;

        if (m_FireTimer > 0)
            return;

        Vector3 gunPosition = transform.position + transform.TransformVector(GunOffset);

        if (m_DoubleGun)
        {
            Vector3 forward = transform.forward;
            Quaternion rot = Quaternion.AngleAxis(10, Vector3.up);
            Quaternion invRot = Quaternion.AngleAxis(-10, Vector3.up);

            Instantiate(ResourceManager.GetPrefab("Bullet"), gunPosition, Quaternion.LookRotation(rot * forward));
            Instantiate(ResourceManager.GetPrefab("Bullet"), gunPosition, Quaternion.LookRotation(invRot * forward));
        }
        else
        {
            Instantiate(ResourceManager.GetPrefab("Bullet"), gunPosition, transform.rotation);
        }

        m_FireTimer = FireCooldown;
    }

    private void UpdateInteractions()
    {
        IsInteracting = PS4 ? Input.GetButtonDown("Fire2") : Input.GetButtonDown("Fire1");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "AI"))
        {
            AI ai = collision.gameObject.GetComponent<AI>();
            if (ai.IsShrunk)
            {
                ai.Die();
                GetComponent<Player>().Gold += ai.GoldReward;
            }
        }
    }
}
