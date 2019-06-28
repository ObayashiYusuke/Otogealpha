using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Note))]
public class NoteEditor : Editor
{

	public override void OnInspectorGUI()
	{
		Note n = target as Note;
		/*
		n.noteType = (NoteType)EditorGUILayout.EnumFlagsField("Note Type", n.noteType);
		*/
	}
}
