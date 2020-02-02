using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTrigger : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particleSystems;


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Play();
        }
    }

    public void Explode()
    {
        StartCoroutine(Delay());

    }

    public void Stop()
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Stop();
        }
    }
}
