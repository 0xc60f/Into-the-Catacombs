using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{

    [SerializeField] GameObject follow;

    [SerializeField] float yOffset;

    [SerializeField] float shake;

    [SerializeField] float textSpeed;

    TextMeshProUGUI text;

    bool running = false;

    public bool isShaking = false;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        //StartSentence("yo this is afunnit asdiu asiud a");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position + new Vector3(0f, yOffset, 0f);
        if (isShaking){
            transform.position += new Vector3(Random.Range(0f, shake), Random.Range(0f, shake), 0f);
        }
        
    }

    IEnumerator TypeSentence(string sentence){
        running = true;
        text.text = "";
        foreach (char letter in sentence.ToCharArray()){
            text.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        for (int i = 0; i < 100; i++){
            yield return null;
        }
        running = false;
        text.text = "";
        isShaking = false;
    }

    public void StopText(){
        StopAllCoroutines();
    }

    public void Shake(bool b){
        isShaking = b;
    }

    public void StartSentence(string sent){
        StartCoroutine(TypeSentence(sent));
    }

    public void StartSentence(string sent, bool shaking){
        isShaking = true;
        StartCoroutine(TypeSentence(sent));
    }

}
