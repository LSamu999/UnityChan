using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float smoothSpeed = 5f;
    public float rotationSpeed = 3f;

    private float _yaw;

    void LateUpdate()
    {
        if (target == null) return;

        _yaw = target.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(15f, _yaw, 0f);
        Vector3 desiredPos = target.position + rot * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
