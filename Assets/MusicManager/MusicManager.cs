using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicSound;    
    public AudioClip crashSound;    
    public AudioClip photoSound;
    public AudioClip driftSound;

    private AudioSource musicSource;
    private AudioSource crashSource;
    private AudioSource photoSource;
    private AudioSource driftSource;
    private Coroutine resumeCoroutine;

    void Start()
    {
        // Creo los objetos de sonido
        musicSource = gameObject.AddComponent<AudioSource>();
        crashSource = gameObject.AddComponent<AudioSource>();
        photoSource = gameObject.AddComponent<AudioSource>();
        driftSource = gameObject.AddComponent<AudioSource>();

        // Asigno los sonidos
        musicSource.clip = musicSound;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();

        crashSource.clip = crashSound;
        crashSource.loop = false;
        crashSource.volume = 1.0f;

        photoSource.clip = photoSound;
        photoSource.loop = false;
        photoSource.volume = 1.0f;

        driftSource.clip = driftSound;
        driftSource.loop = false;
        driftSource.volume = 1.0f;
    }

    public void PlayCrashSound()
    {
        PlaySound(crashSource);
    }

    public void PlayPhotoSound()
    {
        PlaySound(photoSource);
    }

    public void PlaySound(AudioSource audioSource)
    {
        // Se desactiva la musica principal mientras dura el sonido
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }

        audioSource.Play();

        if (resumeCoroutine != null)
        {
            StopCoroutine(resumeCoroutine);
        }
        resumeCoroutine = StartCoroutine(ResumeCoroutine(audioSource));
    }

    public void Drift()
    {
        // En el drift no se pausa la musica principal
        if (!driftSource.isPlaying)
        {
            // Si no está reproduciéndose, lo reproduce
            driftSource.Play();
        }
    }


    private IEnumerator ResumeCoroutine(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        musicSource.UnPause(); // Reanuda desde donde se pausó
        resumeCoroutine = null;
    }
}
