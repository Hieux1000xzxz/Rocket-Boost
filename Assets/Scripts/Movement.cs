using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustStrength = 100f;
    [SerializeField] private float rotationStrength = 100f;
    private Rigidbody rb;
    private AudioSource audioSource;
    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Debug.Log("Thrusting forward");
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if(rotationInput < 0)
        {
            ApllyRotation(rotationStrength);
        }
        else if (rotationInput > 0)
        {
            ApllyRotation(-rotationStrength);
        }
    }

    private void ApllyRotation(float strength)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * strength * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
