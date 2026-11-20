using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private string useTag = "Player";
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == useTag || useTag == "")
        {
            onTriggerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == useTag || useTag == "")
        {
            onTriggerExit?.Invoke();
        }
    }
}
