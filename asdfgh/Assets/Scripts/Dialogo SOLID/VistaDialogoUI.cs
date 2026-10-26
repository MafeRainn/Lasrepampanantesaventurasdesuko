using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VistaDialogoUI : MonoBehaviour, IVistaTexto, IVistaOpciones
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private Button[] botones;

    public void Mostrar(bool visible)
    {
        panel.SetActive(visible);
    }

    public void EstablecerPersonaje(string nombre)
    {
        textoNombre.text = nombre;
    }

    public char[] PrepararTexto(string texto)
    {
        textoDialogo.text = texto;
        textoDialogo.maxVisibleCharacters = 0;
        textoDialogo.ForceMeshUpdate();

        int total = textoDialogo.textInfo.characterCount;
        char[] letras = new char[total];
        for (int i = 0; i < total; i++)
        {
            letras[i] = textoDialogo.textInfo.characterInfo[i].character;
        }

        return letras;
    }

    public void RevelarCaracteres(int cantidad)
    {
        textoDialogo.maxVisibleCharacters = cantidad;
    }

    public void MostrarOpciones(Opcion[] opciones, Action<int> alElegir)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].onClick.RemoveAllListeners();

            bool hayOpcion = i < opciones.Length;
            botones[i].gameObject.SetActive(hayOpcion);

            if (!hayOpcion)
            {
                continue;
            }

            int destino = opciones[i].proximoNodoID;
            botones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i].texto;
            botones[i].onClick.AddListener(() => alElegir(destino));
        }
    }

    public void OcultarOpciones()
    {
        foreach (Button boton in botones)
        {
            boton.onClick.RemoveAllListeners();
            boton.gameObject.SetActive(false);
        }
    }
}