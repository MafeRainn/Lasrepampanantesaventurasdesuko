using System;
using System.Collections;
using UnityEngine;

public class EscritorTexto : MonoBehaviour, IEscritorTexto
{
    [SerializeField] private float segundosPorLetra = 0.04f;

    private IVistaTexto vista;
    private ISonidoLetra sonido;
    private Coroutine rutina;
    private char[] letras;
    private Action alTerminar;

    public bool Escribiendo { get; private set; }

    public void Configurar(IVistaTexto vista, ISonidoLetra sonido)
    {
        this.vista = vista;
        this.sonido = sonido;
    }

    public void Escribir(string texto, VozPersonaje voz, Action alTerminar)
    {
        Detener();
        this.alTerminar = alTerminar;
        letras = vista.PrepararTexto(texto);
        Escribiendo = true;
        rutina = StartCoroutine(Rutina(voz));
    }

    public void Completar()
    {
        if (!Escribiendo)
        {
            return;
        }

        vista.RevelarCaracteres(letras.Length);
        Terminar();
    }

    public void Detener()
    {
        if (rutina != null)
        {
            StopCoroutine(rutina);
        }

        rutina = null;
        Escribiendo = false;
    }

    private IEnumerator Rutina(VozPersonaje voz)
    {
        for (int i = 1; i <= letras.Length; i++)
        {
            vista.RevelarCaracteres(i);

            char letra = letras[i - 1];
            if (!char.IsWhiteSpace(letra) && !char.IsPunctuation(letra))
            {
                sonido.Reproducir(voz);
            }

            yield return new WaitForSeconds(segundosPorLetra);
        }

        Terminar();
    }

    private void Terminar()
    {
        Detener();
        alTerminar?.Invoke();
    }
}