# WireBuilder
WireBuilder is a tool that allows you to create wires from the Unity Editor.


    IMPORTANT

    /// Project setup:
    /// Project Settings > Physics > Default Solver Iterations: Set between 10 to 20
    /// If you use URP update materials.
    /// 
    /// Create a new layer, example: "wire".
    /// Go to Proyect Settings > Physics > Layer Collision Matrix > disables collisions of the layer with itself.
    /// Set the layer "wire" in the prefabs EndAnchor, segment, segmentNoPhysics, StartAnchor, WireBuilder. Change childrens as well. 
    /// Do not change plug layer. 
    /// 
    /// If you use URP add layer "wire" to URP Renderer Data > Filtering.
    /// 
    /// Keep the gizmos active to be able to select position.
    
    /// How to use:
    /// Put the prefab WireBuilder in your scene.
    /// Choose the starting position by right-clicking with the WireBuilder object selected in the hierarchy.
    /// Press Set Start
    /// Choose again the position by right-clicking.
    /// Press Add Segment.
    /// You can select position again and add more segments if you want.
    /// Press Set End to finish the wire.
    /// Select position and press Set Plug to add the plug if needed.
    /// Press Clear if you want to delete the entire wire and start over from scratch.
    /// Press undo to undo the previous segment creation.
    /// Press Render Wire to update the mesh render of the wire in case you move segments individually from the editor.
    /// 
    /// Only if you are using the wire without physics and you don't want to modify the wire anymore
    /// press Finish no physics wire, this removes the segments as they are not needed because the positions are stored in TubeRender.cs
    /// it also removes references and some components. To improve performance.
    
    /// This tool contains the TubeRenderer code by Mathias Soeholm.
    /// https://gist.github.com/mathiassoeholm/15f3eeda606e9be543165360615c8bef

    /// REF : https://www.youtube.com/watch?v=-uLCjX21YZo
