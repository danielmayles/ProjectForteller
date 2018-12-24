using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDialogueChoice : MonoBehaviour
{
	private int id;
	private int targetDialogueId;
	private string choice;

	public void Init(int id) {
		this.id = id;
	}

	public void SetChoice(string choiceString) {
		this.choice = choiceString;
	}

	public void SetTargetDialogueId(int id) {
		this.targetDialogueId = id;
	}

	public int GetId() {
		return id;
	}

	public int GetTargetDialogueId() {
		return targetDialogueId;
	}

	public string GetChoice() {
		return choice;
	}
}
