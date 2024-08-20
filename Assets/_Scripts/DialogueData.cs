using UnityEngine;

[CreateAssetMenu()]
public class DialogueData : ScriptableObject {
    [TextArea] public string[] lines;
}