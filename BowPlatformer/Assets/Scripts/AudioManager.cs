using UnityEngine;

public static class AudioManager{
    //Play audio clip
    public static void PlayClip(AudioClip clip, float pritchVariance = 0f, float volume = 1f){
        //Create a new audio source
        GameObject audioGO = new GameObject(clip.name);
        AudioSource source = audioGO.AddComponent<AudioSource>();

        //Generate pitch value
        float pitch = 1f + Random.Range(-pritchVariance, pritchVariance);
        source.pitch = pitch;

        //Adjust volume
        source.volume = volume;

        //Assign and play clip
        source.clip = clip;
        source.Play();

        //Clean up gameObject once done playing
        GameObject.Destroy(audioGO, clip.length);
    }
}
