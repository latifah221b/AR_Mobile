using UnityEngine;

//Simple script to rotate an object around the Y axis 
public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;  
    public float rotationSpeed = 45f;          

    void Update()
    {
        
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
