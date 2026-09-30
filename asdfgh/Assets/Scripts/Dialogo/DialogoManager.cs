using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoManager : MonoBehaviour
{
    [SerializeField] private DialogoNodo[] nodos;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private Button[] botones;
    [SerializeField] private AudioSource fuenteAudio;
    [SerializeField] private AudioClip sonidoLetra;
    [SerializeField] private float segundosPorLetra = 0.04f;
    [SerializeField] private float variacionPitch = 0.1f;

    private Coroutine escritura;

    private void Start()
    {
        MostrarNodo(0);
    }

    public void MostrarNodo(int id)
    {
        if (id < 0 || id >= nodos.Length)
        {
            Debug.LogError("El nodo " + id + " no existe en " + gameObject.name, this);
            return;
        }

        if (escritura != null)
        {
            StopCoroutine(escritura);
        }

        DialogoNodo nodo = nodos[id];
        textoNombre.text = nodo.nombrePersonaje;
        escritura = StartCoroutine(Escribir(nodo));
    }

    private IEnumerator Escribir(DialogoNodo nodo)
    {
        foreach (Button boton in botones)
        {
            boton.gameObject.SetActive(false);
        }

        textoDialogo.text = nodo.textoDialogo;
        textoDialogo.maxVisibleCharacters = 0;
        textoDialogo.ForceMeshUpdate();
        int total = textoDialogo.textInfo.characterCount;

        for (int i = 1; i <= total; i++)
        {
            textoDialogo.maxVisibleCharacters = i;
            char letra = textoDialogo.textInfo.characterInfo[i - 1].character;

            if (!char.IsWhiteSpace(letra) && !char.IsPunctuation(letra))
            {
                ReproducirLetra();
            }

            yield return new WaitForSeconds(segundosPorLetra);
        }

        ConfigurarBotones(nodo);
    }

    private void ReproducirLetra()
    {
        fuenteAudio.pitch = 1f + Random.Range(-variacionPitch, variacionPitch);
        fuenteAudio.PlayOneShot(sonidoLetra);
    }

    private void ConfigurarBotones(DialogoNodo nodo)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].onClick.RemoveAllListeners();

            if (i < nodo.opciones.Length)
            {
                int destino = nodo.opciones[i].proximoNodoID;
                botones[i].gameObject.SetActive(true);
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = nodo.opciones[i].texto;
                botones[i].onClick.AddListener(() => MostrarNodo(destino));
            }
            else
            {
                botones[i].gameObject.SetActive(false);
            }
        }
    }
}