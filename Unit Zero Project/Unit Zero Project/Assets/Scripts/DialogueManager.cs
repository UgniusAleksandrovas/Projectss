using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
	public bool endText;

	public Animator animator;

	private Queue<string> sentences;

	// Use this for initialization
	void Start() {
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue) {
		endText = false;
		animator.SetBool("IsOpen", true);
		nameText.text = dialogue.name;
		sentences.Clear();
		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence();
	}

	public void DisplayNextSentence() {
		if (sentences.Count == 0) {
			endText = true;
			EndDialogue();
			return;
		}
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence) {
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
			//yield return new WaitForSeconds(0.05f);
		}
	}

	public void EndDialogue() {
		animator.SetBool("IsOpen", false);
	}
}