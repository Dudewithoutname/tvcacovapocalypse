using UnityEngine;

public class AudioBulb : MonoBehaviour
{
    public AudioSource AudioSource;

    public static void playAudio(AudioClip clip, float volume)
    {
        var audioBulb = Instantiate(EnemyManager.Singleton.AudioBulb);
        
        audioBulb.AudioSource.volume = volume;
        audioBulb.AudioSource.clip = clip;
        audioBulb.AudioSource.Play();
        
        Destroy(audioBulb.gameObject, clip.length + 0.1f);
    }
}
