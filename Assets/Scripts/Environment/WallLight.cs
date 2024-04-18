using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WallLight : MonoBehaviour
{
    [SerializeField] public ParticleSystem sparks;

    [SerializeField] public Light2D light;

    [SerializeField] public SpriteRenderer bulb;

    public bool isBroken = false;

    public Color color;

    public float intensity;

    void Awake(){
        color = bulb.color;
        intensity = light.intensity;
    }

    
    public void Break(){
        StartCoroutine(ReleaseSparks());
        StartCoroutine(Flicker(3, true));
        isBroken = true;
    }

    public void Break(int lower, int upper){
        StartCoroutine(ReleaseSparks());
        StartCoroutine(Flicker(Random.Range(lower, upper), true));
        isBroken = true;
    }

    public void FlickerOn(int lower, int upper){
        StartCoroutine(Flicker(Random.Range(lower, upper), false));
        isBroken = false;
    }

    public void FlickerOn(){
        StartCoroutine(Flicker(3, false));
        isBroken = false;
    }

    IEnumerator Flicker(int numTimes, bool willTurnOff){

        for (int i = 0; i < numTimes; i++){
            bulb.color = new Color(0f, 0f, 0f);
            light.intensity = 5f;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));

            light.intensity = intensity;
            bulb.color = color;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));
        }
        if (willTurnOff){
            bulb.color = new Color(0f, 0f, 0f);
            light.intensity = 0f;
        }
    }

    IEnumerator ReleaseSparks(){
        sparks.Play();
        yield return new WaitForSeconds(0.2f);
        sparks.Stop();
    }

    void BreakInstantly(){
        StartCoroutine(ReleaseSparks());
        bulb.color = new Color(0f, 0f, 0f);
        light.intensity = 0f;
        isBroken = true;
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.tag == "Projectile" && !isBroken){
            BreakInstantly();
        }
    }
}
