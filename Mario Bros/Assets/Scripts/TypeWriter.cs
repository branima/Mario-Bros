using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{

    TextMeshProUGUI tmpText;

    public float timeBetweenLetters = 0.15f;

    public List<GameObject> objectsToAppearAfter;

    public void OnEnable()
    {
        GameManager.Instance.GetPlayer().GetComponentInChildren<Renderer>().sortingOrder = -1;
        tmpText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Write(tmpText.text, timeBetweenLetters));
        tmpText.text = "";
    }

    public IEnumerator Write(string text, float waitTime)
    {
        for (var i = 0; i <= text.Length; i++)
        {
            tmpText.text = text.Substring(0, i);

            yield return new WaitForSecondsRealtime(waitTime);
        }

        foreach (GameObject item in objectsToAppearAfter)
            item.SetActive(true);
    }
}
