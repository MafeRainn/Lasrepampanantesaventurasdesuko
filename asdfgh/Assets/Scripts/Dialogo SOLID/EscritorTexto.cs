using System;
using UnityEngine;

public class EscritorTexto : MonoBehaviour, IEscritorTexto
{
    [SerializeField] private float segundosPorLetra = 0.04f;

    private IVistaTexto vista;
    private ISonidoLetra sonido;
    private char[] letras;
    private VozPersonaje voz;
    private Action alTerminar;
    private int indice;
    private float acumulado;

    public bool Escribiendo { get; private set; }

    public void Configurar(IVistaTexto vista, ISonidoLetra sonido)
    {
        this.vista = vista;
        this.sonido = sonido;
    }

    public void Escribir(string texto, VozPersonaje voz, Action alTerminar)
    {
        this.voz = voz;
        this.alTerminar = alTerminar;
        letras = vista.PrepararTexto(texto);
        indice = 0;
        acumulado = 0f;
        Escribiendo = true;

        if (letras.Length == 0)
        {
            Terminar();
        }
    }

    public void Actualizar(float deltaTiempo)
    {
        if (!Escribiendo)
        {
            return;
        }

        acumulado += deltaTiempo;

        while (Escribiendo && acumulado >= segundosPorLetra)
        {
            acumulado -= segundosPorLetra;
            indice++;
            vista.RevelarCaracteres(indice);

            char letra = letras[indice - 1];
            if (!char.IsWhiteSpace(letra) && !char.IsPunctuation(letra))
            {
                sonido.Reproducir(voz);
            }

            if (indice >= letras.Length)
            {
                Terminar();
            }
        }
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
        Escribiendo = false;
    }

    private void Update()
    {
        Actualizar(Time.deltaTime);
    }

    private void Terminar()
    {
        Escribiendo = false;
        alTerminar?.Invoke();
    }
}