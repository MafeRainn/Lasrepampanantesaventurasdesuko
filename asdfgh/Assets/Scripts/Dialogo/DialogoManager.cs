using UnityEngine;
using TMPro;

public class DialogoManager : MonoBehaviour
{
    [SerializeField] private DialogoNodo[] nodos;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDialogo;

    private int nodoActualID = 0;

    private void Start()
    {
        MostrarNodo(nodoActualID);
    }

    public void MostrarNodo(int id)
    {
        nodoActualID = id;
        DialogoNodo nodo = nodos[id];

        textoNombre.text = nodo.nombrePersonaje;
        textoDialogo.text = nodo.textoDialogo;
    }

    public void IrAlNodo(int id)
    {
        MostrarNodo(id);
    }
}