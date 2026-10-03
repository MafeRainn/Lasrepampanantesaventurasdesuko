using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaAtaque : MonoBehaviour
{
    [SerializeField] private string nombreEscena = "zonadeataque";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
