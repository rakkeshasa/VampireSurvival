using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource[] soundEffects;
    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(int index)
    {
        soundEffects[index].Stop();
        soundEffects[index].Play();
    }

    public void PitchControl(int index)
    {
        soundEffects[index].pitch = Random.Range(.8f, 1.2f);
        PlaySFX(index);
    }
}
