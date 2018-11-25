using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public struct Dialogue {
	public string text;
	public int time;

	public Dialogue(String text, int time) {
		this.time = time;
		this.text = text;
	}
}

public class DialogueDisplay : MonoBehaviour {
	[SerializeField] private GameObject _dialogueChoicesParent;
	private TextMeshProUGUI _dialogueText;
	private Queue<Dialogue> _queuedDialogue = new Queue<Dialogue>();
	
	void Awake () {
		_dialogueText = GetComponent<TextMeshProUGUI>();
		StartCoroutine((TextUpdate()));
		
		ChangeText(new Dialogue("Company man woke me up.", 5));
		ChangeText(new Dialogue("Said you'd need ferry to the rooster's perch" , 10));
	}

	private void ChangeText(Dialogue dialogue) {
		_queuedDialogue.Enqueue(dialogue);

	}

	IEnumerator TextUpdate() {
		while (true) {
			if (_queuedDialogue.Count > 0) {
				_dialogueChoicesParent.gameObject.SetActive(false);
				Dialogue text = _queuedDialogue.Dequeue();
				_dialogueText.text = text.text;
				yield return  new WaitForSeconds(text.time);
			}
			else {
				_dialogueText.text = "";
				_dialogueChoicesParent.gameObject.SetActive(true);
				yield return  new WaitForEndOfFrame();
			}




		}
	}
	
	

}
