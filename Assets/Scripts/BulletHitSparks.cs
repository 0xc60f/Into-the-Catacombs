using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitSparks : MonoBehaviour
{

    [SerializeField] ParticleSystem parts;

    public void Flash(Vector2 loc){
        gameObject.transform.parent = null;
        gameObject.transform.position = loc;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(BulletFlash());
    }

    IEnumerator BulletFlash(){
        parts.Play();

        yield return new WaitForSeconds(0.1f);

        parts.Stop();

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
