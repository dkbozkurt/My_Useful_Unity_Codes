// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Dialogue_System.Basic_Dialogue_System___Brackeys
{
    /// <summary>
    /// Attach this script into "Dialog trigger buttons" inspector.
    ///
    /// From button's "On Click" event. Assign dialogue trigger and call the
    /// TriggerDialogue method.
    /// 
    /// Ref : https://www.youtube.com/watch?v=_nRzoTzeyxU 
    /// </summary>
    
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}