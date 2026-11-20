using TMPro;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ShowHealth : MonoBehaviour
{
    [SerializeField] private TextMeshPro healthText;
    private Health health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.hp < 0)
        {
            healthText.text = "HP: 0";
        }
        else
        {
            healthText.text = "HP: " + health.hp.ToString();
        }
    }
}
