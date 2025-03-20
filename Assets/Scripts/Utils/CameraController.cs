using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 10, -5);
    public float lookAheadFactor = 0.5f;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Vector3 mouseAimDirection;

    void LateUpdate()
    {
        if (target == null) return;

        // Get mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(rayDistance);
            mouseAimDirection = (mouseWorldPosition - target.position).normalized;
        }

        // Calculate desired position with mouse aim offset
        desiredPosition = target.position + offset + (mouseAimDirection * lookAheadFactor);
        
        // Smoothly move camera
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Look at target
        transform.LookAt(target.position);
    }
}
