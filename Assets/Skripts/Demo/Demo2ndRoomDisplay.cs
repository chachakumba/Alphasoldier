using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Demo2ndRoomDisplay : MonoBehaviour
{
    [SerializeField] Saying[] saying;
    [SerializeField] TMP_Text text;
    [SerializeField] float waitTime = 1;
    int index = 0;
    bool sayedStart = false;
    int score = 0;
    int final = 0;
    public void StartTalking(int finalScore)
    {
        final = finalScore;
        saying[0].text = saying[0].text.Replace("{score}", final.ToString());
        StartCoroutine(Talkcour());
    }
    IEnumerator Talkcour()
    {
        while (index < saying.Length)
        {
            for (int i = 0; i < saying.Length; i++)
            {
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
                sayedStart = true;
                text.text = "Current kills:" + score;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
    public void UpdateScore()
    {
        score++;
        if (sayedStart)
        {
            text.text = "Current kills:" + score;
        }
    }
    public void EndScoring()
    {
        text.text = "Exit is open";
    }
}
