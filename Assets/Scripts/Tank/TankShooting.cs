using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;
    public MeshRenderer tankRenderer;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    private float nextFireTime;
    private Color originalColor = Color.clear;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire(m_CurrentLaunchForce, 1);
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            Fire(m_CurrentLaunchForce, 1);
        }
    }


    public void Fire(float launchForce, float fireRate)
    {
        if (Time.time <= nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        m_Fired = true;

        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public void setChaseUI() {
        Material[] materials = tankRenderer.materials;
        originalColor = materials[0].color;
        for (int i=0; i < materials.Length; i++) {
            materials[i].color = Color.magenta;
        }
        tankRenderer.materials = materials;
    }

    public void setPatrolUI() {
        Material[] materials = tankRenderer.materials;
        for (int i=0; i < materials.Length; i++) {
            if (originalColor != Color.clear) materials[i].color = originalColor;
        }
        tankRenderer.materials = materials;
    }

    public void setShootUI() {
        Material[] materials = tankRenderer.materials;
        originalColor = materials[0].color;
        for (int i=0; i < materials.Length; i++) {
            materials[i].color = Color.black;
        }
        tankRenderer.materials = materials;
    }
    
}