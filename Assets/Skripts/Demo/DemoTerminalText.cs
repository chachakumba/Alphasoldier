using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoTerminalText : MonoBehaviour
{
    public Saying[] saying;
    [SerializeField] TMP_Text text;
    [SerializeField] float waitTime = 1;
    [SerializeField] Saying[] angryPhrases;
    Coroutine waitCour;
    Coroutine talkCour;
    public int index = 0;
    public static int anger = 0;
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
            if(angryPhrases.Length > 0)
            if (gameObject.GetComponent<Collider2D>().Cast(Vector2.zero, new RaycastHit2D[1]) < 1)
            {
                anger++;
                int chosenAngry = Random.Range(0, angryPhrases.Length);
                string displayedText = "";
                for (int i = 0; i < angryPhrases[chosenAngry].text.Length; i++)
                {
                    displayedText += angryPhrases[chosenAngry].text[i];
                    text.text = displayedText;
                    yield return new WaitForSeconds(0.03f * angryPhrases[chosenAngry].speed);
                }
                yield return new WaitForSeconds(angryPhrases[chosenAngry].delay);
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
