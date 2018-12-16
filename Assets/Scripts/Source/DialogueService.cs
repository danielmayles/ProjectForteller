using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueData {
	public int id;
	public string dialogueTitle;
	public string dialogue;
	public Vector3 Position;
	public Quaternion Rotation;
	public int[] DialogueLinks;
	public delegate void DialogueUpdated();
	public DialogueUpdated OnDialogueUpdated;
	
	public DialogueData(int id, string dialogueTitle, string dialogue, Vector3 position, Quaternion rotation, int[] dialogueLinks) {
		this.id = id;
		this.dialogueTitle = dialogueTitle;
		this.dialogue = dialogue;
		this.Position = position;
		this.Rotation = rotation;
		this.DialogueLinks = dialogueLinks;
		OnDialogueUpdated = null;
	}
}

public class DialogueService {
	
	private Dictionary<int, DialogueData> dialogue = new Dictionary<int, DialogueData>();
	private Serializer serializer = new Serializer();
	private int currentIndex;

	public delegate void LoadDialogueFinished(List<DialogueData> dialogue);
	public LoadDialogueFinished OnLoadDialogueFinished;
	
	public void AddNewDialogueObject(DialogueObject newDialogue) {
		while (dialogue.ContainsKey(currentIndex)) {
			currentIndex++;
		}

		DialogueData dialogueData = new DialogueData(currentIndex, newDialogue.GetDialogueTitle(), newDialogue.GetDialogue(), newDialogue.transform.position, newDialogue.transform.rotation, newDialogue.DialogueLinks());
		newDialogue.init(dialogueData);
		dialogue.Add(currentIndex, dialogueData);		
	}
	
	public void UpdateDialogueObject(DialogueData dialogueData) {
		dialogue[dialogueData.id] = dialogueData;
		if (dialogue[dialogueData.id].OnDialogueUpdated != null) {
			dialogue[dialogueData.id].OnDialogueUpdated.Invoke();
		}
	}
	
	public void UpdateDialogueObject(DialogueObject dialogueObject) {
		dialogue[dialogueObject.GetDialogueId()] = new DialogueData(dialogueObject.GetDialogueId(),  dialogueObject.GetDialogueTitle(), dialogueObject.GetDialogue(), dialogueObject.transform.position, dialogueObject.transform.rotation, dialogueObject.DialogueLinks());		
		if (dialogue[dialogueObject.GetDialogueId()].OnDialogueUpdated != null) {
			dialogue[dialogueObject.GetDialogueId()].OnDialogueUpdated.Invoke();
		}
	}

	public void RemoveDialogueObject(int id) {
		if (dialogue.ContainsKey(id))
		{
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
			Debug.Log(dialogue[dialogueKey].Position);
			serializer.SerializeInt(dialogueKey);
			serializer.SerializeVector3(dialogue[dialogueKey].Position);
			serializer.SerializeQuaternion(dialogue[dialogueKey].Rotation);
			
			serializer.SerializeString(dialogue[dialogueKey].dialogueTitle);
			serializer.SerializeString(dialogue[dialogueKey].dialogue);
			serializer.SerializeInt(dialogue[dialogueKey].DialogueLinks.Length);
			for (int i = 0; i < dialogue[dialogueKey].DialogueLinks.Length; i++) {
				serializer.SerializeInt(dialogue[dialogueKey].DialogueLinks[i]);
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
			int[] dialogueLinks = new int[serializer.ReadInt()];
			for (int linkIndex = 0; linkIndex < dialogueLinks.Length; linkIndex++) {
				dialogueLinks[linkIndex] = serializer.ReadInt();
			}
			DialogueData dialogueData = new DialogueData(id, dialogueTitleString, dialogueString, position, rotation, dialogueLinks);
			dialogue.Add(id, dialogueData);
		}

		if (OnLoadDialogueFinished != null){
			OnLoadDialogueFinished(dialogue.Values.ToList());
		}
	}
}
