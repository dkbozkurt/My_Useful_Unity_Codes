// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Observer Pattern
///
/// It is generally used in notification and achievement systems.
///  It is laid out so that different parts of a system work without interfering with each other.
/// A subject's operation is monitored by other observer classes and these classes make progress according to this operation.
/// Here our subject does not have to reach others, because it just informs. Observer classes follow this.
///
/// The objects we want to observe will be derived from this class, so we have converted our class to an abstract class.
/// 
/// Ref: https://www.youtube.com/watch?v=hnxzYdnjH1U
/// </summary>

// Listener/ Receiver
public abstract class Observer : MonoBehaviour
{

    public abstract void OnNotify(NotificationType notificationType);

    

}
