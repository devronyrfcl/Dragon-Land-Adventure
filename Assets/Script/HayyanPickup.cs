using UnityEngine;

public class HayyanPickup : MonoBehaviour
{
    public int hayyanValue = 1; // Amount of Hayyans to be added on pickup

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player character has the "Player" tag
        {
            CurrencyManager.Instance.AddHayyanCurrency(hayyanValue);
            Destroy(gameObject); // Destroy the pickup object after collecting
        }
    }
}
