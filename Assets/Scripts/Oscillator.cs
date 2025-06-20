using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
    
    private Vector3 startingPos;
    private Vector3 targetPos;
    private float movementFactor;
    private void Start()
    {
        startingPos = transform.position;
        targetPos = startingPos + movementVector;
    }
    private void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startingPos, targetPos, movementFactor);
    }
}
