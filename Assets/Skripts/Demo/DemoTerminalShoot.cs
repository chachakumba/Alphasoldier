using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DemoTerminalShoot : MonoBehaviour
{
    [SerializeField] Saying[] saying;
    [SerializeField] TMP_Text text;
    [SerializeField] float waitTime = 1;
    [SerializeField] Saying[] angryPhrases;
    [SerializeField] Saying[] reallyAngryPhrases;
    Coroutine waitCour;
    Coroutine talkCour;
    int index = 0;
    [SerializeField] int maxAnger = 3;
    [SerializeField] GameObject strongTurrets;
    [SerializeField] DemoTerminalText[] prevTerminals;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (talkCour == null && index < saying.Length)
            talkCour = StartCoroutine(Talkcour());
    }
    IEnumerator Talkcour()
    {
        if (index < saying.Length)
        {

            for (int i = 0; i < saying.Length; i++)
            {
                if (gameObject.GetComponent<Collider2D>().Cast(Vector2.zero, new RaycastHit2D[1]) < 1)
                    break;

                if (i >= index)
                {
                    string displayedText = "";
                    for (int j = 0; j < saying[i].text.Length; j++)
                    {
                        displayedText += saying[i].text[j];
                        text.text = displayedText;
                        yield return new WaitForSeconds(0.03f * saying[i].speed);
                    }
                    yield return new WaitForSeconds(saying[i].delay);
                    text.text = "";
                    index++;
                }
            }
            if (index >= saying.Length)
            {
                StopCoroutine(talkCour);
                talkCour = null;
            }

            yield return new WaitForSeconds(waitTime);
            if (gameObject.GetComponent<Collider2D>().Cast(Vector2.zero, new RaycastHit2D[1]) < 1)
            {
                if (DemoTerminalText.anger >= 3 || (prevTerminals[0].index < prevTerminals[0].saying.Length && prevTerminals[1].index < prevTerminals[1].saying.Length))
                {
                    strongTurrets.SetActive(true);
                    int chosenAngry = Random.Range(0, reallyAngryPhrases.Length);
                    string displayedText = "";
                    for (int i = 0; i < reallyAngryPhrases[chosenAngry].text.Length; i++)
                    {
                        displayedText += reallyAngryPhrases[chosenAngry].text[i];
                        text.text = displayedText;
                        yield return new WaitForSeconds(0.03f * reallyAngryPhrases[chosenAngry].speed);
                    }
                    yield return new WaitForSeconds(reallyAngryPhrases[chosenAngry].delay);
                }
                else
                {
                    DemoTerminalText.anger++;
                    int chosenAngry = Random.Range(0, angryPhrases.Length);
                    string displayedText = "";
                    for (int i = 0; i < angryPhrases[chosenAngry].text.Length; i++)
                    {
                        displayedText += angryPhrases[chosenAngry].text[i];
                        text.text = displayedText;
                        yield return new WaitForSeconds(0.03f * angryPhrases[chosenAngry].speed);
                    }
                    yield return new WaitForSeconds(angryPhrases[chosenAngry].delay);
                }
                text.text = "";
                StopCoroutine(talkCour);
                talkCour = null;
            }
            else
            {
                talkCour = StartCoroutine(Talkcour());
            }
        }
    }
}
