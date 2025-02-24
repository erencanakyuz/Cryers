using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public float rightLimit; // Sağ sınır

    public float leftLimit; // Sağ sınır
    public float bottomLimit; // Alt sınır (genellikle y ekseni için sabit)
    public float topLimit; // Üst sınır (genellikle y ekseni için sabit)
    public Transform target;
    public float xMin = -53.3f, xMax = 53.3f, zMin, zMax;

    void LateUpdate()
    {
        float x = Mathf.Clamp(target.position.x, xMin, xMax);
        float z = Mathf.Clamp(target.position.z, zMin, zMax);
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, z);
    }


    void OnDrawGizmos()
    {
        // Sınırları görselleştirmek için gizmos çiz
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftLimit, topLimit, 0), new Vector3(rightLimit, topLimit, 0));
        Gizmos.DrawLine(new Vector3(rightLimit, topLimit, 0), new Vector3(rightLimit, bottomLimit, 0));
        Gizmos.DrawLine(new Vector3(rightLimit, bottomLimit, 0), new Vector3(leftLimit, bottomLimit, 0));
        Gizmos.DrawLine(new Vector3(leftLimit, bottomLimit, 0), new Vector3(leftLimit, topLimit, 0));
    }
}
