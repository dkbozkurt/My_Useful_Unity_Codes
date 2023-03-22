using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Presets;
using UnityEditor;
#endif
public class WireController : MonoBehaviour
{
    //author: nicogarcia.s.dev / nicogarcia_s_dev
    //no license

    /// <summary>
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
    /// </summary>

    /// <summary>
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
    /// </summary>


    #region TIPS
    [TextArea]
    [Tooltip("Dont remove Notes variable.")]
    public string Notes = "With the WireBuilder object selected use right click to select position. Have active Gizmos.";
    [TextArea]
    [Tooltip("Dont remove Notes2 variable.")]
    public string Notes2 = "Wire render settings in TubeRender.cs on WireRender object.";
    #endregion

    [Header("SETTINGS")]
    [Tooltip("Disabling it removes the wire physics, for use as a prop (Only change after clearing).")]
    public bool usePhysics = true;
    [Tooltip("Distance between segments and position selected with the mouse. Lowering it allows more precision. Increase it when you want to set the end anchor point. Dont go below than 0.01")]
    public float maxDistanceWithSelectedPos = 0.2f;
    [Tooltip("Separation between segments, lower it instance less segments. Dont go below than 0.01")]
    public float segmentsSeparation = 0.2f;
    [Tooltip("Prevents infinite segments from being instantiated in case of an error in the code.")]
    public int limitMax = 200;
    private int limit = 0;
    [Tooltip("A higher value improves the stability of the physics.")]
    public float segmentsRadius = 1.5f;
    public float currentDistanceToStartAnchor;
    [Tooltip("Sets the maximum distance from the start anchor point to the end anchor point, based on the number of segments and the separation between them.")]
    public float maxDistanceToStarAnchor;



    [Header("SPAWNED SEGMENTS")]
    public List<Transform> segments;
    [HideInInspector]
    [Tooltip("You can delete these references when you are no longer modifying the wire.")]
    public List<int> undoSegments;
    private int undoCount = 0;

    [Header("REFERENCES")]
    public TubeRenderer ropeMesh;
    public Transform starAnchorTemp;
    public Transform firstSegment;
    public Transform endAnchorTemp;
    public Transform plugTemp;

    [Header("PREFABS")]
    public Transform startAnchorPoint;
    public Transform segment;
    public Transform segmentNoPhysics;
    public Transform endAnchorPoint;
    public Transform plugObjt;

    [Header("MOUSE POSS")]
    public Vector3 selectPosition;
    public Transform mousePossHelper;

#if UNITY_EDITOR
    [Header("PRESETS")]
    /// <summary>
    /// Presset of the ConfigurableJoint used by the segments.
    /// </summary>
    public Preset presetJoint;
#endif


#if UNITY_EDITOR
    private void Start()
    {
        mousePossHelper.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        ChangeRadius();
    }

    public void GetSegmentsDistance()
    {
        /// <summary>
        /// Instantiate the segments by checking the distance of the last instantiated segment from the selected position.
        /// </summary>

        //Get distance between lastSegment and selected position.
        int lastSegment = segments.Count - 1;
        float distance = Vector3.Distance(segments[lastSegment].position, selectPosition);

        //If the last segment has not reached the selected position another one is created.
        if (distance >= maxDistanceWithSelectedPos + segmentsSeparation && limit <= limitMax)
        {
            //Limit to prevent infinite loop
            limit++;
            if (usePhysics)
            {
                //Instantiate new segment.
                Transform newSegment = Instantiate(segment, segments[lastSegment].position + (segments[lastSegment].forward * segmentsSeparation), segments[lastSegment].rotation, transform);
                newSegment.GetComponent<ConfigurableJoint>().connectedBody = segments[lastSegment].GetComponent<Rigidbody>();
                segments.Add(newSegment);
            }
            else
            {
                //Instantiate new segment.
                Transform newSegment = Instantiate(segmentNoPhysics, segments[lastSegment].position + (segments[lastSegment].forward * segmentsSeparation), segments[lastSegment].rotation, transform);
                segments.Add(newSegment);
            }
            #region Undo
            undoCount++;
            #endregion

            //The function is repeated until the selected position is reached.
            GetSegmentsDistance();
            return;
        }



        /// <summary>
        /// Sets the maximum distance from the start anchor point to the end anchor point, based on the number of segments and the separation between them.
        /// </summary>
        SetMaxDistance();
    }

    public void AddStar()
    {


        if(starAnchorTemp == null)
        {
            #region unpack prefab
            //When the first segment is created, the prefab is unpacked, to avoid an error that causes references to be lost in play mode.
            if (PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
                PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            #endregion

            starAnchorTemp = Instantiate(startAnchorPoint, selectPosition, Quaternion.identity, transform);
        }

        //If you do not use physics, the components are removed to the start anchor point, to improve performance.
        if (!usePhysics)
        {
            DestroyImmediate(starAnchorTemp.GetComponent<ConfigurableJoint>());
            DestroyImmediate(starAnchorTemp.GetComponent<Collider>());
            DestroyImmediate(starAnchorTemp.GetComponent<Rigidbody>());
        }
    }

    public void AddSegment()
    {
        #region undo
        undoCount = 0;
        #endregion
        if (firstSegment == null)
        {

            if (usePhysics)
            {
                firstSegment = Instantiate(segment, starAnchorTemp.position, starAnchorTemp.rotation, transform);
                firstSegment.GetComponent<ConfigurableJoint>().connectedBody = starAnchorTemp.GetComponent<Rigidbody>();
            }
            else
            {
                firstSegment = Instantiate(segmentNoPhysics, starAnchorTemp.position, starAnchorTemp.rotation, transform);
            }

            segments.Add(firstSegment);

            #region undo
            undoCount++;
            #endregion
        }

        //The last current segment is rotated in the direction of selected position.
        int lastSegment = segments.Count - 1;
        segments[lastSegment].LookAt(selectPosition);

        //Segment is added based on the distance to the selected position.
        GetSegmentsDistance();
        RenderWireMesh();

        #region undo
        undoSegments.Add(undoCount);
        #endregion
    }

    public void AddEnd()
    {
        //Adds the final anchor point.
        int lastSegment = segments.Count - 1;
        endAnchorTemp = Instantiate(endAnchorPoint, segments[lastSegment].position + (segments[lastSegment].forward * .0005f), segments[lastSegment].rotation, transform);
        //The ConfigurableJoint is added to the anchor point and anchored with the end segment.
        endAnchorTemp.GetComponent<ConfigurableJoint>().connectedBody = segments[lastSegment].GetComponent<Rigidbody>();

        

        if (usePhysics)
        {
            //Another ConfigurableJoint joint is added to the "final segment" and configured with the preset.
            ConfigurableJoint newComponent = segments[lastSegment].gameObject.AddComponent<ConfigurableJoint>();
            presetJoint.ApplyTo(newComponent);
            //Connects to the end anchor point.
            newComponent.connectedBody = endAnchorTemp.GetComponent<Rigidbody>();
        }
        else
        {
            //If you do not use physics, the components are removed to the end anchor point, to improve performance.
            DestroyImmediate(endAnchorTemp.GetComponent<ConfigurableJoint>());
            DestroyImmediate(endAnchorTemp.GetComponent<Collider>());
            DestroyImmediate(endAnchorTemp.GetComponent<Rigidbody>());
        }


    }

    public void AddPlug()
    {
        //Instances the plug in the selected position.
        plugTemp = Instantiate(plugObjt, selectPosition, plugObjt.transform.rotation, transform);
        PlugController plugScritp = plugTemp.GetComponent<PlugController>();

        plugScritp.endAnchor = endAnchorTemp;
        plugScritp.endAnchorRB = endAnchorTemp.GetComponent<Rigidbody>();
        plugScritp.wireController = this;

    }

    public void SetMaxDistance()
    {
        maxDistanceToStarAnchor = segments.Count * segmentsSeparation;
    }

    public void ChangeRadius()
    {
        ///<summary>
        ///Modifies the radius of the sphere colliders of all instantiated segments.
        ///Increasing the radius usually improves the stability of the physics but makes the collisions less accurate in relation to the mesh.
        /// </summary>
        if (usePhysics)
            foreach (Transform segment in segments)
            {
                segment.GetComponent<SphereCollider>().radius = segmentsRadius;
            }
    }

    #region Buttons
    public void Clear()
    {
        //Destroy the segments.
        for (int i = 1; i < segments.Count; i++)
        {
            DestroyImmediate(segments[i].gameObject);
        }

        //Destroy the start anchor point.
        if (firstSegment != null)
            DestroyImmediate(firstSegment.gameObject);

        //Destroy the start anchor point.
        if (starAnchorTemp != null)
            DestroyImmediate(starAnchorTemp.gameObject);

        //Destroy the end anchor point.
        if (endAnchorTemp != null)
            DestroyImmediate(endAnchorTemp.gameObject);

        //Destroy the plug.
        if (plugTemp != null)
            DestroyImmediate(plugTemp.gameObject);


        //Clears the lists.
        segments.Clear();
        #region undo
        //Clear undo list.
        undoSegments.Clear();
        undoCount = 0;
        #endregion

        //Render wire
        RenderWireMesh();
        ClearWireMesh();

        //Reset limit
        limit = 0;
    }

    public void Undo()
    {
        //Destroy the end anchor point.
        if (endAnchorTemp != null)
            DestroyImmediate(endAnchorTemp.gameObject);

        //Undo the last segment creation.
        for (int i = 1; i <= undoSegments[undoSegments.Count - 1]; i++)
        {
            DestroyImmediate(segments[segments.Count - 1].gameObject);
            segments.Remove(segments[segments.Count - 1]);
        }
        undoSegments.RemoveAt(undoSegments.Count - 1);

        //The wire rendering is cleaned.
        if (undoSegments.Count == 0)
            ClearWireMesh();
        //Wire rendering updated
        RenderWireMesh();
    }

    public void ClearWireMesh()
    {
        /// <summary>
        /// When the TubeRender.cs position array is cleared the mesh render is not updated properly.
        /// For it to work properly you have to add 2 momentary positions in the array, that's what this function is for. 
        /// </summary>

        Vector3[] temp = new Vector3[]
        {
            new Vector3 (0,0,0),
            new Vector3 (0,0,0)
        };
        ropeMesh.SetPositions(temp);
    }

    public void FinishNoPhysicsWire()
    {
        /// <summary>
        /// Only when it is wire without physics.
        /// Only when you no longer want to modify the position of the segments.
        /// Eliminates segments and segment references to improve performance.
        /// </summary>>
        if (!usePhysics)
        {
            foreach (Transform segment in segments)
            {
                DestroyImmediate(segment.gameObject);
            }
            segments.Clear();
            undoSegments.Clear();
        }
        else
        {
            Debug.LogWarning("only use in no-physics wires and when you don't want to modify them anymore.");
        }
    }
    #endregion

    #region Selected Position
    public void SetPosition(Vector3 position)
    {
        //The position of the mouse is saved based on the raycast hit.
        selectPosition = position;
        AddClickPosHelper();
    }
    public void AddClickPosHelper()
    {
        //The helper is placed in the position to see graphically where it is.
        mousePossHelper.transform.position = selectPosition;
    }
    #endregion

#endif


    private void Update()
    {
        if (usePhysics)
        {
            RenderWireMesh();
            DistanceBetweenStartAndEnd();
        }
    }

    public void DistanceBetweenStartAndEnd()
    {
        currentDistanceToStartAnchor = Vector3.Distance(endAnchorTemp.position, starAnchorTemp.position);

        if (currentDistanceToStartAnchor > maxDistanceToStarAnchor)
        {
            /// <summary>
            /// Call a function when the distance between the start anchor point and the End anchor point exceeds the maximum.
            /// Example: do not let the wire rope move any further.
            /// </summary>>
        }
    }

        public void RenderWireMesh()
    {
        /// <summary>
        /// For more wire render settings see TubeRender.cs.
        /// </summary>

        //Render the wire.
        List<Vector3> tempPos = new List<Vector3>();
        foreach (Transform pos in segments)
        {
            tempPos.Add(pos.localPosition);
        }
        ropeMesh.SetPositions(tempPos.ToArray());
    }


}
