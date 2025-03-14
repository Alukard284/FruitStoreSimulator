using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель (персонаж)
    public Vector3 offset = new Vector3(0, 2, -5); // Смещение камеры

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}