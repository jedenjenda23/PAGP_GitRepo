using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnTrigger : MonoBehaviour
{
    public GameObject child;
    public float hidingSpeed = 10f;
    public float hidingHeight = -1f;

    Vector3 targetHideScale;
    Vector3 initScale;

    private void Awake()
    {
        child.SetActive(true);

        initScale = child.transform.localScale;

        targetHideScale = child.transform.localScale;
        targetHideScale.y = hidingHeight;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            // child.SetActive(false);
            StartCoroutine("Hide", hidingSpeed);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            //child.SetActive(true);
            StartCoroutine("Unhide", hidingSpeed);
        }
    }


    private IEnumerator Hide(float time)
    {
        StopCoroutine("Unhide");
        float elapsedTime = 0;
        while (elapsedTime < time)
        {

            child.transform.localScale = Vector3.Lerp(child.transform.localScale, targetHideScale, hidingSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Unhide(float time)
    {
        StopCoroutine("Hide");

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            child.transform.localScale = Vector3.Lerp(child.transform.localScale, initScale, hidingSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
