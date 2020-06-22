using UnityEngine;

public static class AudioManager{
    public static void PlayClip(AudioClip clip, Vector3 position, bool is3D, Transform parent, float volume, float pitchVariance){
        GameObject sourceGO = new GameObject("Audio: " + clip.name);
        AudioSource audioSource = sourceGO.AddComponent<AudioSource>();

        sourceGO.transform.position = position;
        sourceGO.transform.parent = parent;
        audioSource.clip = clip;
        audioSource.spatialBlend = is3D ? 1f : 0f;
        audioSource.volume = volume;
        audioSource.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);

        audioSource.Play();

        GameObject.Destroy(sourceGO, clip.length);
    }
}