using UnityEngine;

namespace Addressables
{
    /// <summary>
    /// ADDRESSABLES
    /// 
    /// Control how and when your assets are loaded, optionally from the cloud.
    /// Instead of everything being loaded automatically, it only loads when you ask it to.
    /// It helps you manage memory and also unloading times.
    /// Addressables are internally synchronous so you are never waiting for your assets to load by looking at a frozen
    /// screen.
    ///
    /// Import "Addressables" package from package manager> Unity Registry.
    ///
    /// Window> Asset Management> Addressables> Groups, will open "Addressables Groups" windows.
    ///
    /// We can add items into Group by dragging and dropping.(By right clicking on the opened window you can add new groups)
    /// Or by enabling Addressable checkbox from the inspector of the object or file.
    ///
    /// When you add an item into group, item will have addressable path in the inspector section.(It is also editable)
    ///
    /// 
    /// 
    /// Ref : https://www.youtube.com/watch?v=C6i_JiRoIfk&ab_channel=CodeMonkey
    /// </summary>
    public class AddressablesREADME : MonoBehaviour
    { }
}
