using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : Effect
{
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] GameObject thunderObj;
    [SerializeField] float thunderTime;

    private void Start()
    {
        StartCoroutine(PlayAndContinue());
    }

    private IEnumerator PlayAndContinue()
    {

        yield return new WaitForSeconds(particles[particles.Count-1].startDelay);

        foreach (var particle in particles)
        {
            var rotOverLifetime = particle.GetComponent<ParticleSystem>().rotationOverLifetime;
            rotOverLifetime.enabled = true;
        }

        yield return new WaitForSeconds(1f);

        thunderObj.SetActive(true);

        yield return new WaitForSeconds(thunderTime);

        Destroy(thunderObj);
        Destroy(gameObject);
    }
}
