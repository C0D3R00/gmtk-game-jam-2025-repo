using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text dialogueText;

    [Header("Typewriter Settings")]
    public float typeSpeed = 0.05f;
    public float lineDelay = 1.2f;

    [Header("Loop Dialogues")]
    public LoopDialogue[] loopDialogues;

    private Coroutine currentCoroutine;
    private int currentLoopIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayNextDialogue()
    {
        if (currentLoopIndex >= loopDialogues.Length)
        {
            Debug.Log("No more dialogue for this loop count.");
            return;
        }

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        var lines = loopDialogues[currentLoopIndex].lines;
        currentCoroutine = StartCoroutine(PlayLines(lines));

        currentLoopIndex++;
    }

    private IEnumerator PlayLines(string[] lines)
    {
        foreach (var line in lines)
        {
            dialogueText.text = "";

            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }

            yield return new WaitForSeconds(lineDelay);
        }

        dialogueText.text = "";
    }

    public void ResetDialogue()
    {
        currentLoopIndex = 0;
    }
}
