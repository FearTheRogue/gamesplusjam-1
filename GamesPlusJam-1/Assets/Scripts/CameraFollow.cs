using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Vector2 minPos;
    [SerializeField] private Vector2 maxPos;

    public bool disableClamp = false;

    private void FixedUpdate()
    {
        if (target == null)
        {
            transform.position = TargetToFollow(new Vector3(0,0,0));
            return;
        }

        transform.position = TargetToFollow(target.position);
    }

    private Vector3 TargetToFollow(Vector3 targetPos)
    {
        Vector3 desiredPosition;
        Vector3 smoothedPosition;

        if (!disableClamp)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        }

        desiredPosition = targetPos + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        return smoothedPosition;
    }
}
