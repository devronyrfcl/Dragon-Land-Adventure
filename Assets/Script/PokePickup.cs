using UnityEngine;

public class PokePickup : MonoBehaviour
{
    public int pokeValue = 1; // Amount of Pokes to be added on pickup

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player character has the "Player" tag
        {
            CurrencyManager.Instance.AddPokeCurrency(pokeValue);
            Destroy(gameObject); // Destroy the pickup object after collecting
        }
    }
}
