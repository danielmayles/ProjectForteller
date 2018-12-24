using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public struct DialogueData {
	public int id;
	public string dialogueTitle;
	public string dialogue;
	public Vector3 position;
	public Quaternion rotation;
	public int targetDialogueId;
	public DialogueChoice[] dialogueChoices;
	
	public DialogueData(int id, string dialogueTitle, string dialogue, Vector3 position, Quaternion rotation, int targetDialogueId) {
		this.id = id;
		this.dialogueTitle = dialogueTitle;
		this.dialogue = dialogue;
		this.position = position;
		this.rotation = rotation;
		this.targetDialogueId = targetDialogueId;
		this.dialogueChoices = new DialogueChoice[0];
	}

	public DialogueData(int id, string dialogueTitle, string dialogue, Vector3 position, Quaternion rotation, DialogueChoice[] dialogueChoices) {
		this.id = id;
		this.dialogueTitle = dialogueTitle;
		this.dialogue = dialogue;
		this.position = position;
		this.rotation = rotation;
		this.targetDialogueId = -1;
		this.dialogueChoices = dialogueChoices;
	}
}

public struct DialogueChoice {
	public int id;
	public int targetDialogueId;
	public string choice;
	public Vector3 position;
	public Quaternion rotation;

	public DialogueChoice(int id, int targetDialogueId, string choice, Vector3 position, Quaternion rotation) {
		this.id = id;
		this.targetDialogueId = targetDialogueId;
		this.choice = choice;
		this.position = position;
		this.rotation = rotation;
	}
}

public class DialogueService {
	
	private Dictionary<int, DialogueData> dialogue = new Dictionary<int, DialogueData>();
	private Serializer serializer = new Serializer();
	private int currentDialogueIndex;

	private Dictionary<int, DialogueChoice> dialogueChoices = new Dictionary<int, DialogueChoice>();
	private int currentDialogueChoiceIndex;

	public delegate void LoadDialogueFinished(List<DialogueData> dialogue);
	public LoadDialogueFinished OnLoadDialogueFinished;
	
	public delegate void DialogueUpdated(DialogueData dialogueData);
	public DialogueUpdated OnDialogueUpdated;
	
	public void AddNewDialogueObject(EditorDialogueObject newDialogue) {
		while (dialogue.ContainsKey(currentDialogueIndex)) {
			currentDialogueIndex++;
		}

		DialogueData dialogueData = new DialogueData(currentDialogueIndex, newDialogue.GetDialogueTitle(), newDialogue.GetDialogue(), newDialogue.transform.position, newDialogue.transform.rotation, new DialogueChoice[0]);
		newDialogue.init(dialogueData);
		dialogue.Add(currentDialogueIndex, dialogueData);		
	}

	public void AddNewDialogueChoice(EditorDialogueChoice newDialogueChoice) {
		while (dialogueChoices.ContainsKey(currentDialogueChoiceIndex)) {
			currentDialogueChoiceIndex++;
		}
		newDialogueChoice.Init(currentDialogueChoiceIndex);
		DialogueChoice dialogueChoiceData = new DialogueChoice(currentDialogueChoiceIndex, newDialogueChoice.GetTargetDialogueId(), newDialogueChoice.GetChoice(), newDialogueChoice.transform.position, newDialogueChoice.transform.rotation);
		dialogueChoices.Add(currentDialogueChoiceIndex, dialogueChoiceData);
	}

	public void UpdateDialogueObject(DialogueData dialogueData) {
		dialogue[dialogueData.id] = dialogueData;
		if (OnDialogueUpdated != null) {
			OnDialogueUpdated.Invoke(dialogue[dialogueData.id]);
		}
	}
	
	public void UpdateDialogueObject(EditorDialogueObject dialogueObject) {
		dialogue[dialogueObject.GetDialogueId()] = new DialogueData(dialogueObject.GetDialogueId(),  dialogueObject.GetDialogueTitle(), dialogueObject.GetDialogue(), dialogueObject.transform.position, dialogueObject.transform.rotation, new DialogueChoice[0]);		
		if (OnDialogueUpdated != null) {
			OnDialogueUpdated.Invoke(dialogue[dialogueObject.GetDialogueId()]);
		}
	}

	public void RemoveDialogueObject(int id) {
		if (dialogue.ContainsKey(id)){
			dialogue.Remove(id);
		}
	}

	public DialogueData GetDialogue(int dialogueId) {
		return dialogue[dialogueId];
	}
	
	public void WriteDialogueToFile() {
		const string path = "Assets/Resources/Dialogue/dialogue.txt";
		File.Delete(path);
		
		serializer.SerializeInt(dialogue.Count);
		foreach (int dialogueKey in dialogue.Keys) {
			Debug.Log(dialogue[dialogueKey].position);
			serializer.SerializeInt(dialogueKey);
			serializer.SerializeVector3(dialogue[dialogueKey].position);
			serializer.SerializeQuaternion(dialogue[dialogueKey].rotation);
			
			serializer.SerializeString(dialogue[dialogueKey].dialogueTitle);
			serializer.SerializeString(dialogue[dialogueKey].dialogue);
			serializer.SerializeInt(dialogue[dialogueKey].dialogueChoices.Length);
			for (int i = 0; i < dialogue[dialogueKey].dialogueChoices.Length; i++) {
				serializer.SerializeInt(dialogue[dialogueKey].dialogueChoices[i].targetDialogueId);
				serializer.SerializeString(dialogue[dialogueKey].dialogueChoices[i].choice);
			}
		}
		File.WriteAllBytes(path, serializer.ReadData());
	}
	
	public void LoadDialogueFromFile() {
		dialogue.Clear();
		const string path = "Assets/Resources/Dialogue/dialogue.txt";	
		serializer.WriteInData(File.ReadAllBytes(path));

		int amountOfDialogue = serializer.ReadInt();
		for (int i = 0; i < amountOfDialogue; i++) {
			int id = serializer.ReadInt();
			Vector3 position = serializer.ReadVector3();
			Quaternion rotation = serializer.ReadQuaternion();
			Debug.Log(position);
			
			string dialogueTitleString = serializer.ReadString();
			string dialogueString = serializer.ReadString();
			DialogueChoice[] dialogueChoices = new DialogueChoice[serializer.ReadInt()];
			for (int linkIndex = 0; linkIndex < dialogueChoices.Length; linkIndex++) {
				dialogueChoices[linkIndex].targetDialogueId = serializer.ReadInt();
				dialogueChoices[linkIndex].choice = serializer.ReadString();
			}
			DialogueData dialogueData = new DialogueData(id, dialogueTitleString, dialogueString, position, rotation, dialogueChoices);
			dialogue.Add(id, dialogueData);
		}

		if (OnLoadDialogueFinished != null){
			OnLoadDialogueFinished(dialogue.Values.ToList());
		}
	}
}
