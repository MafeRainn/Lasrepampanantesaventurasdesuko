using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class EntradaAvanceUnity : IEntradaAvance
{
    public bool AvancePresionado()
    {
        if (UiManager.Pausado)
        {
            return false;
        }

#if ENABLE_INPUT_SYSTEM
        return (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
#endif
    }
}