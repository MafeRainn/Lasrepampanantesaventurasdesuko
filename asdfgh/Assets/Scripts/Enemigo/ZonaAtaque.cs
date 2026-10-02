using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaAtaque : Enemy
{
    [SerializeField] private string nombreEscena = "ZonaAtaque";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        }
}
