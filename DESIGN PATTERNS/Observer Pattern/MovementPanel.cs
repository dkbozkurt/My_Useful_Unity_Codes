// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MovementPanel
///
/// Add this script into where operation will held.
/// 
/// Ref: https://www.youtube.com/watch?v=hnxzYdnjH1U
/// </summary>

public class MovementPanel : Subject
{

    public void ForwardOnClick()
    {
        Notify(NotificationType.ForwardButton);
    }

    public void BackOnClick()
    {
        Notify(NotificationType.BackButton);
    }

    public void RightOnCLick()
    {
        Notify(NotificationType.RightButton);
    }

    public void LeftOnClick()
    {
        Notify(NotificationType.LeftButton);
    }
}
