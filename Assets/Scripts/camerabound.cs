using UnityEngine;

public class camerabound : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // Kameranýn yumuþak takip hýzý
    Transform target;

    [Header("Kamera Sýnýrlarý")]
    public float minX; // Haritanýn en sol sýnýrý
    public float maxX; // Haritanýn en sað sýnýrý
    public float minY; // Haritanýn en alt sýnýrý
    public float maxY; // Haritanýn en üst sýnýrý

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Kameranýn gitmek istediði ideal pozisyon
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Yumuþak geçiþ (Lerp)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kameranýn X ve Y koordinatlarýný belirlediðimiz sýnýrlar arasýnda tutuyoruz
        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        // Yeni pozisyonu uygula
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
