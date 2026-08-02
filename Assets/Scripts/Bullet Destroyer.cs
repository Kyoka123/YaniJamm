using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    public int damage = 10; // Merminin vereceði hasar miktarý
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Çarptýðýmýz obje Player mý kontrol et (Tag üzerinden)
        if (other.CompareTag("Player"))
        {
            // 2. Player üzerindeki PlayerHealth script'ine ulaþ
            Boy playerHealth = other.GetComponent<Boy>();

            // 3. Eðer script bulunduysa canýný azalt
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // 4. Mermiyi yok et
            Destroy(gameObject);
        }
        // Duvara çarptýðýnda da yok olmasý için:
        else if (other.CompareTag("boundary"))
        {
            Destroy(gameObject);
        }
    }
    
}
