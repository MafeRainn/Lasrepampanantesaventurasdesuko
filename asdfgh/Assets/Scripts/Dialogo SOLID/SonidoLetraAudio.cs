using UnityEngine;

public class SonidoLetraAudio : MonoBehaviour, ISonidoLetra
{
    [SerializeField] private AudioSource fuente;
    [SerializeField] private AudioClip sonidoPorDefecto;
    [SerializeField] private float variacionPitch = 0.1f;

    public void Reproducir(VozPersonaje voz)
    {
        AudioClip clip = voz != null && voz.sonido != null ? voz.sonido : sonidoPorDefecto;
        float pitchBase = voz != null ? voz.pitch : 1f;

        fuente.pitch = pitchBase + Random.Range(-variacionPitch, variacionPitch);
        fuente.PlayOneShot(clip);
    }
}