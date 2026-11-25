using UnityEngine;

public class WheelFlapper : MonoBehaviour
{
    [Header("Configuración de Rebote")]
    [SerializeField] private float kickAngle = -35f;    
    [SerializeField] private float springSpeed = 18f;   
    [SerializeField] private float damping = 10f;       

    [Header("Audio (Opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tickClip;

    private float currentAngle = 0f;
    private float angularVelocity = 0f;

    private void Update()
    {
        //Damped Harmonic Oscillator
        float force = -springSpeed * currentAngle;
        angularVelocity += force * Time.deltaTime * 60f;
        angularVelocity *= Mathf.Exp(-damping * Time.deltaTime);
        currentAngle += angularVelocity * Time.deltaTime;

        
        transform.localEulerAngles = new Vector3(0f, currentAngle, 0f);
    }


    public void TriggerHit()
    {
        currentAngle = kickAngle;
        angularVelocity = 0f;

        if (audioSource != null && tickClip != null)
        {
            audioSource.pitch = Random.Range(0.92f, 1.08f);
            audioSource.PlayOneShot(tickClip);
        }
    }
}
