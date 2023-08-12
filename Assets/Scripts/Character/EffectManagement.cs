using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManagement : MonoBehaviour
{
    private ParticleSystem effect;
    private Color color;                //Efektimizin renk deðiþkeni

    private void Awake()
    {
        effect = GetComponent<ParticleSystem>();
        effect.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            color = Color.red;

        else if(collision.gameObject.CompareTag("Apple"))
            color = Color.green;

        else if(collision.gameObject.CompareTag("Potion"))
            color = Color.blue;

        StartCoroutine(playStopEffect());
    }

    private IEnumerator playStopEffect()
    {
        //Renk paletimizi tanýmlar ve efektimizin colorOverLifetime özelliðini bir deðiþkene eþitleriz.
        Gradient grad = new Gradient();
        var colorOverLifetime = effect.colorOverLifetime;

        //Renk paletimiz (grad) için color ve alfa key`leri belirleriz : 
        GradientColorKey[] colorKey = { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        GradientAlphaKey[] alphaKey = { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) };

        //Belirlediðimiz key`leri ren paletimize set ederiz. 
        grad.SetKeys( colorKey, alphaKey );

        //Efektimizin colorOverLifetime özelliðini aktif eder, paletimizide aktarýrýz.
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = grad;

        effect.Play();
        yield return new WaitForSeconds(0.3f);
        effect.Stop();
    }
}
