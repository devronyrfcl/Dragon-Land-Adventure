using UnityEngine;

public class CurrencyAddCollider : MonoBehaviour
{
    [SerializeField] private bool addHayyans = true; // Option to add Hayyans or Pokes
    [SerializeField] private int hayyanAmount = 1; // Amount of Hayyans to add
    [SerializeField] private int pokeAmount = 1; // Amount of Pokes to add

    public int HayyanAmount
    {
        get { return hayyanAmount; }
        set { hayyanAmount = value; }
    }

    public int PokeAmount
    {
        get { return pokeAmount; }
        set { pokeAmount = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrencyManager currencyManager = CurrencyManager.Instance;

            if (currencyManager != null)
            {
                if (addHayyans)
                {
                    // Add Hayyans to the CurrencyManager
                    currencyManager.AddHayyanCurrency(hayyanAmount);
                }
                else
                {
                    // Add Pokes to the CurrencyManager
                    currencyManager.AddPokeCurrency(pokeAmount);
                }
            }

            // Destroy the trigger object
            Destroy(gameObject);
        }
    }
}
