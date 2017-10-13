using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Target))]
public class TargetEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var mjTarget = (Target) target;
		
		mjTarget.MaxScore = EditorGUILayout.IntField("Max Score", mjTarget.MaxScore);
		mjTarget.IsPlainScoreTarget = EditorGUILayout.Toggle("Is Plain Score Target", mjTarget.IsPlainScoreTarget);
		if (!mjTarget.IsPlainScoreTarget)
		{
			mjTarget.ScoreTexture =
				(Texture2D) EditorGUILayout.ObjectField("ScoreTexture", mjTarget.ScoreTexture, typeof(Texture2D), false);
		}
		else
		{
			mjTarget.ScoreTexture = null;
		}

		mjTarget.HasSubTargets = EditorGUILayout.Toggle("Has Sub Targets", mjTarget.HasSubTargets);
		if (mjTarget.HasSubTargets)
		{
			mjTarget.CanOutliveSubTargets = EditorGUILayout.Toggle("Can outlive sub targets", mjTarget.CanOutliveSubTargets);
			mjTarget.BonusScore = EditorGUILayout.IntField("Bonus Score", mjTarget.BonusScore);
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("subTargets"), true);
			serializedObject.ApplyModifiedProperties();
		}
		else
		{
			mjTarget.CanOutliveSubTargets = false;
			mjTarget.BonusScore = 0;
			mjTarget.subTargets = new Target[0];
		}

		
	}
}
