// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace CpiTemplate.Game.Creative.Scripts.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class CreativeShortcutNotePad : MonoBehaviour
    {
        // [Multiline]
        [TextArea(minLines: 30, maxLines :50)]
        [SerializeField] private string _notes;
    }
}
