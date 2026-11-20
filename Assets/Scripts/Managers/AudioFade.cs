using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFade : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private float fadeInTargetVolume;
    private float fadeOutTargetVolume = 0f;
    private float fadeSpeed = 0.5f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        fadeInTargetVolume = source.volume;
        source.volume = 0;
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine("DoFadeIn");
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine("DoFadeOut");
    }

    private IEnumerator DoFadeIn()
    {
        while(source.volume != fadeInTargetVolume)
        {
            source.volume = Mathf.MoveTowards(source.volume, fadeInTargetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        StopAllCoroutines();
    }

    private IEnumerator DoFadeOut()
    {
        while (source.volume != fadeOutTargetVolume)
        {
            source.volume = Mathf.MoveTowards(source.volume, fadeOutTargetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        StopAllCoroutines();
    }

}
