using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmergencyAlarm : WallLight
{

    float rotation = 0f;

    [SerializeField] float rotationSpeed;

    void Update(){
        rotation += (rotationSpeed * Time.deltaTime) % 360;
        light.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        light1.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, (rotation + 180) % 360);
    }

    [SerializeField] Light2D light1;

    void Awake(){
        color = bulb.color;
        intensity = light.intensity;
    }

    public void Break(){
        StartCoroutine(ReleaseSparks());
        StartCoroutine(Flicker(3, true));
        isBroken = true;
    }

    public void FlickerOn(){
        StartCoroutine(Flicker(3, false));
        isBroken = false;
    }

    IEnumerator Flicker(int numTimes, bool willTurnOff){

        for (int i = 0; i < numTimes; i++){
            bulb.color = new Color(0f, 0f, 0f);
            light.intensity = 5f;
            light1.intensity = 5f;

            yield return new WaitForSeconds(Random.Range(0.03f, 0.15f));

            light.intensity = intensity;
            light1.intensity = intensity;
            bulb.color = color;

            yield return new WaitForSeconds(Random.Range(0.03f, 0.15f));
        }
        if (willTurnOff){
            bulb.color = new Color(0f, 0f, 0f);
            light.intensity = 0f;
            light1.intensity = 0f;
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
        light1.intensity = 0f;
        isBroken = true;
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.tag == "Projectile" && !isBroken){
            BreakInstantly();
        }
    }

}
