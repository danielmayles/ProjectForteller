using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public struct DialogueData {
	public int id;
	public string dialogue;
	public Vector3 Position;
	public Quaternion Rotation;
	public int[] DialogueLinks;
	
	public DialogueData(int id, string dialogue, Vector3 position, Quaternion rotation, int[] dialogueLinks) {
		this.id = id;
		this.dialogue = dialogue;
		this.Position = position;
		this.Rotation = rotation;
		this.DialogueLinks = dialogueLinks;
	}
}

public class DialogueService {
	
	private Dictionary<int, DialogueData> dialogue = new Dictionary<int, DialogueData>();
	private Serializer serializer = new Serializer();
	private int currentIndex;

	public DialogueService() {
		LoadDialogueFromFile();
	}

	public void AddNewDialogueObject(DialogueObject newDialogue) {
		while (dialogue.ContainsKey(currentIndex)) {
			currentIndex++;
		}
		dialogue.Add(currentIndex, new DialogueData(currentIndex, newDialogue.GetDialogue(), newDialogue.transform.position, newDialogue.transform.rotation, newDialogue.DialogueLinks()));		
	}

	public void RemoveDialogueObject(int key) {
		dialogue.Remove(key);
	}

	public List<DialogueData> GetDialogue() {
		return new List<DialogueData>(dialogue.Values);
	}
	
	private void WriteDialogueToFile() {
		const string path = "Assets/Resources/Dialogue/dialogue.txt";
		File.Delete(path);
		
		serializer.SerializeInt(dialogue.Count);
		foreach (int dialogueKey in dialogue.Keys) {
			serializer.SerializeInt(dialogueKey);
			serializer.SerializeString(dialogue[dialogueKey].dialogue);
			serializer.SerializeVector3(dialogue[dialogueKey].Position);
			serializer.SerializeQuaternion(dialogue[dialogueKey].Rotation);
			serializer.SerializeInt(dialogue[dialogueKey].DialogueLinks.Length);
			for (int i = 0; i < dialogue[dialogueKey].DialogueLinks.Length; i++) {
				serializer.SerializeInt(dialogue[dialogueKey].DialogueLinks[i]);
			}
		}
		File.WriteAllBytes(path, serializer.ReadData());
	}
	
	private void LoadDialogueFromFile() {
		dialogue.Clear();
		const string path = "Assets/Resources/Dialogue/dialogue.txt";	
		serializer.WriteInData(File.ReadAllBytes(path));

		int amountOfDialogue = serializer.ReadInt();
		for (int i = 0; i < amountOfDialogue; i++) {
			int id = serializer.ReadInt();
			string dialogueString = serializer.ReadString();
			Vector3 position = serializer.ReadVector3();
			Quaternion rotation = serializer.ReadQuaternion();
			int[] dialogueLinks = new int[serializer.ReadInt()];
			for (int linkIndex = 0; linkIndex < dialogueLinks.Length; linkIndex++) {
				dialogueLinks[linkIndex] = serializer.ReadInt();
			}
			DialogueData dialogueData = new DialogueData(id, dialogueString, position, rotation, dialogueLinks);
			dialogue.Add(id, dialogueData);
		}
	}
}
