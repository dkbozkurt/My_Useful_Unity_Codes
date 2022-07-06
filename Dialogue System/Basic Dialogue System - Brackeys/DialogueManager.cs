// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue_System.Basic_Dialogue_System___Brackeys
{
    /// <summary>
    ///
    /// Attach this script onto new gameobject's inspector.
    ///
    /// Add this scripts object onto "On Click" event of the "Continue..."
    /// button. And select "DisplayNextSentence method"
    /// 
    /// Ref : https://www.youtube.com/watch?v=_nRzoTzeyxU 
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton

        public static DialogueManager _instance = null;

        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<DialogueManager>();
                    if (_instance == null)
                        _instance = new GameObject("SimpleSingleton").AddComponent<DialogueManager>();
                }

                return _instance;
            }
        }

        #endregion
        
        private Queue<string> _sentences;

        // public Animator _animator;
        
        public Text nameText;
        public Text dialogueText;
        private void OnEnable()
        {
            _instance = this;
        }

        private void Start()
        {
            _sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            //Debug.Log("Starting conversation with: " + dialogue.name);
            
            // _animator.SetBool("IsOpen",true);
            nameText.text = dialogue.name;
            
            _sentences.Clear();
            AssignSentences(dialogue);
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = _sentences.Dequeue();
            
            // If user starts new sentence before the previous one finish, it stops immediately previous animation and starts the next one.
            StopAllCoroutines();
            StartCoroutine(TypeSentenceCoroutine(sentence));
        }

        private void EndDialogue()
        {
            Debug.Log("End of conversation.");
            
            // _animator.SetBool("IsOpen",false);
        }

        private void AssignSentences(Dialogue dialogue)
        {
            foreach (string sentence in dialogue.sentences)
            {
                _sentences.Enqueue(sentence);
            }
        }

        IEnumerator TypeSentenceCoroutine(string sentence)
        {
            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
    }
}