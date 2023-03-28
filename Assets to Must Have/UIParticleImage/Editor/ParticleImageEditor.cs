// Version: 1.1.0
using AssetKits.ParticleImage.Enumerations;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using PlayMode = AssetKits.ParticleImage.Enumerations.PlayMode;

namespace AssetKits.ParticleImage.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ParticleImage))]
    public class ParticleImageEditor : UnityEditor.Editor
    {
        private ParticleImage _particle;
        
        //General Properties
        private SerializedProperty _space;
        private SerializedProperty _timeScale;
        private SerializedProperty _playMode;
        private SerializedProperty _loop;
        private SerializedProperty _duration;
        private SerializedProperty _delay;
        private SerializedProperty _life;
        private SerializedProperty _speed;
        private SerializedProperty _startSize;
        private SerializedProperty _startRotation;
        private SerializedProperty _startColor;
        private SerializedProperty _raycast;
        private SerializedProperty _maskable;
        
        //Emitter Properties
        private SerializedProperty _rate;
        private SerializedProperty _rateOverLifetime;
        private SerializedProperty _rateOverDistance;
        private SerializedProperty _burst;
        private SerializedProperty _emitterShape;
        private SerializedProperty _width;
        private SerializedProperty _height;
        private SerializedProperty _length;
        private SerializedProperty _radius;
        private SerializedProperty _angle;
        private SerializedProperty _surface;
        private SerializedProperty _edge;
        private SerializedProperty _spreadType;
        private SerializedProperty _spreadLoop;
        private SerializedProperty _startPoint;
        private SerializedProperty _startPointTrans;
        
        //Particle Properties
        private SerializedProperty _texture;
        private SerializedProperty _material;
        private SerializedProperty _speedOverLifetime;
        private SerializedProperty _sizeOverLifetime;
        private SerializedProperty _sizeBySpeed;
        private SerializedProperty _sizeSpeedRange;
        private SerializedProperty _colorOverLifetime;
        private SerializedProperty _colorBySpeed;
        private SerializedProperty _colorSpeedRange;
        private SerializedProperty _rotateOverLifetime;
        private SerializedProperty _rotationBySpeed;
        private SerializedProperty _rotationSpeedRange;
        private SerializedProperty _alignDirection;
        private SerializedProperty _sheetModule;
        private SerializedProperty _sheetType;
        private SerializedProperty _tile;
        private SerializedProperty _frameOverTime;
        private SerializedProperty _startFrame;
        private SerializedProperty _frameSpeedRange;
        private SerializedProperty _frameFps;
        private SerializedProperty _cycles;
        
        private SerializedProperty _trailModule;
        private SerializedProperty _trailWidth;
        private SerializedProperty _trailColorOverLifetime;
        private SerializedProperty _trailColorOverTrail;
        private SerializedProperty _trailLifetime;
        private SerializedProperty _inheritColor;
        private SerializedProperty _dieWithParticle;
        private SerializedProperty _trailRatio;
        private SerializedProperty _minimumVertexDistance;
        
        //Movement Properties
        private SerializedProperty _targetModule;
        private SerializedProperty _targetTransform;
        private SerializedProperty _targetCurve;
        private SerializedProperty _targetMode;
        
        private SerializedProperty _noiseModule;
        private SerializedProperty _noiseFreq;
        private SerializedProperty _noiseOct;
        private SerializedProperty _noiseStrength;
        
        private SerializedProperty _velocityModule;
        private SerializedProperty _velocitySpace;
        private SerializedProperty _velocityOverLifetime;
        
        private SerializedProperty _gravityModule;
        private SerializedProperty _gravity;
        
        private SerializedProperty _vortexModule;
        private SerializedProperty _vortexStrength;
        
        //Event Properties
        private SerializedProperty _onStart;
        private SerializedProperty _onFinish;
        private SerializedProperty _onFirstParticleFinish;
        private SerializedProperty _onLastParticleFinish;
        private SerializedProperty _onParticleFinish;

        //Editor Icons
        private Texture _particleModuleIcon;
        private Texture _movementModuleIcon;
        private Texture _emitterModuleIcon;
        private Texture _eventModuleIcon;
        
        //Module Foldouts
        private GUIStyle _foldoutStyle;
        private GUIStyle FoldoutStyle
        {
            get
            {
                if (_foldoutStyle == null)
                {
                    _foldoutStyle = new GUIStyle("ShurikenModuleTitle");
                    _foldoutStyle.font = new GUIStyle(EditorStyles.label).font;
                    _foldoutStyle.border = new RectOffset(15, 7, 4, 4);
                    _foldoutStyle.fixedHeight = 24;
                    _foldoutStyle.contentOffset = new Vector2(20f, -2f);
                }
                return _foldoutStyle;
            }
        }
        void Awake()
        {
            //Set Icons
            MonoScript.FromMonoBehaviour(target as ParticleImage).SetIcon(Resources.Load<Texture2D>("ComponentIcon"));
            _emitterModuleIcon = EditorGUIUtility.IconContent("AreaLight Gizmo").image;
            _particleModuleIcon = Resources.Load<Texture>("ParticleModule");
            _movementModuleIcon = Resources.Load<Texture>("MovementModule"); 
            _eventModuleIcon =  Resources.Load<Texture>("EventModule");
        }

        private void OnEnable() 
        {
            serializedObject.Update();
            _particle = target as ParticleImage;
            
            //Initialize Properties
            _startPoint = serializedObject.FindProperty("_emitterConstraintEnabled");
            _startPointTrans = serializedObject.FindProperty("_emitterConstraintTransform");
            _timeScale = serializedObject.FindProperty("_timeScale");
            _playMode = serializedObject.FindProperty("_playMode");
            _delay = serializedObject.FindProperty("_startDelay");
            _loop = serializedObject.FindProperty("_loop");
            _duration = serializedObject.FindProperty("_duration");
            _maskable = serializedObject.FindProperty("m_Maskable");
            _raycast = serializedObject.FindProperty("m_RaycastTarget");
            _rate = serializedObject.FindProperty("_rate");
            _rateOverLifetime = serializedObject.FindProperty("_rateOverLifetime");
            _rateOverDistance = serializedObject.FindProperty("_rateOverDistance");
            _emitterShape = serializedObject.FindProperty("_shape");
            _spreadType = serializedObject.FindProperty("_spread");
            _spreadLoop = serializedObject.FindProperty("_spreadLoop");
            _space = serializedObject.FindProperty("_space");
            _angle = serializedObject.FindProperty("_angle");
            _width = serializedObject.FindProperty("_width");
            _height = serializedObject.FindProperty("_height");
            _length = serializedObject.FindProperty("_length");
            _radius = serializedObject.FindProperty("_radius");
            _surface = serializedObject.FindProperty("_emitOnSurface");
            _edge = serializedObject.FindProperty("_emitterThickness");
            _colorOverLifetime = serializedObject.FindProperty("_colorOverLifetime");
            _colorBySpeed = serializedObject.FindProperty("_colorBySpeed");
            _colorSpeedRange = serializedObject.FindProperty("_colorSpeedRange");
            _sizeOverLifetime = serializedObject.FindProperty("_sizeOverLifetime");
            _sizeBySpeed = serializedObject.FindProperty("_sizeBySpeed");
            _sizeSpeedRange = serializedObject.FindProperty("_sizeSpeedRange");
            _startRotation = serializedObject.FindProperty("_startRotation");
            _rotateOverLifetime = serializedObject.FindProperty("_rotationOverLifetime");
            _rotationBySpeed = serializedObject.FindProperty("_rotationBySpeed");
            _rotationSpeedRange = serializedObject.FindProperty("_rotationSpeedRange");
            _targetTransform = serializedObject.FindProperty("_attractorTarget");
            _targetCurve = serializedObject.FindProperty("_toTarget");
            _burst = serializedObject.FindProperty("_bursts");
            _speed = serializedObject.FindProperty("_startSpeed");
            _startSize = serializedObject.FindProperty("_startSize");
            _material = serializedObject.FindProperty("m_Material");
            _startColor = serializedObject.FindProperty("_startColor");
            _life = serializedObject.FindProperty("_lifetime");
            _speedOverLifetime = serializedObject.FindProperty("_speedOverLifetime");
            _texture = serializedObject.FindProperty("m_Texture");
            _sheetModule = serializedObject.FindProperty("_sheetModule");
            _sheetType = serializedObject.FindProperty("_sheetType");
            _tile = serializedObject.FindProperty("_textureTile");
            _frameOverTime = serializedObject.FindProperty("_frameOverTime");
            _startFrame = serializedObject.FindProperty("_startFrame");
            _frameSpeedRange = serializedObject.FindProperty("_frameSpeedRange");
            _frameFps = serializedObject.FindProperty("_textureSheetFPS");
            _cycles = serializedObject.FindProperty("_textureSheetCycles");
            _targetMode = serializedObject.FindProperty("_targetMode");
            _gravity = serializedObject.FindProperty("_gravity");
            _targetModule = serializedObject.FindProperty("_targetModule");
            _noiseModule = serializedObject.FindProperty("_noiseModule");
            _noiseFreq = serializedObject.FindProperty("_noiseFrequency");
            _noiseOct = serializedObject.FindProperty("_noiseOctaves");
            _noiseStrength = serializedObject.FindProperty("_noiseStrength");
            _gravityModule = serializedObject.FindProperty("_gravityModule");
            _vortexModule = serializedObject.FindProperty("_vortexModule");
            _vortexStrength = serializedObject.FindProperty("_vortexStrength");
            _velocityModule = serializedObject.FindProperty("_velocityModule");
            _velocitySpace = serializedObject.FindProperty("_velocitySpace");
            _velocityOverLifetime = serializedObject.FindProperty("_velocityOverLifetime");
            _alignDirection = serializedObject.FindProperty("_alignToDirection");
            _trailModule = serializedObject.FindProperty("_trailModule");
            _trailWidth = serializedObject.FindProperty("_trailWidth");
            _trailColorOverLifetime = serializedObject.FindProperty("_trailColorOverLifetime");
            _trailColorOverTrail = serializedObject.FindProperty("_trailColorOverTrail");
            _trailLifetime = serializedObject.FindProperty("_trailLifetime");
            _inheritColor = serializedObject.FindProperty("_inheritParticleColor");
            _dieWithParticle = serializedObject.FindProperty("_dieWithParticle");
            _trailRatio = serializedObject.FindProperty("_trailRatio");
            _minimumVertexDistance = serializedObject.FindProperty("_minimumVertexDistance");
            _onStart = serializedObject.FindProperty("_onStart");
            _onFinish = serializedObject.FindProperty("_onStop");
            _onFirstParticleFinish = serializedObject.FindProperty("_onFirstParticleFinish");
            _onLastParticleFinish = serializedObject.FindProperty("_onLastParticleFinish");
            _onParticleFinish = serializedObject.FindProperty("_onParticleFinish");
            
            _particle.OnEnable();

            if (Application.isEditor && !EditorApplication.isPlaying)
            {
                _particle.Invoke(nameof(ParticleImage.Play), 0.1f);
            }

            EditorApplication.update += EditorUpdate;
            SceneView.duringSceneGui += DrawSceneWindow;
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
            SceneView.duringSceneGui -= DrawSceneWindow;
            Undo.undoRedoPerformed -= UndoRedoPerformed;
            
            if (Application.isEditor && !EditorApplication.isPlaying && _particle)
            {
                _particle.CancelInvoke();
                _particle.Pause();
            }
        }
        
        private void UndoRedoPerformed()
        {
            // Temporary fix for NullReferenceException on undo performed when Curve Editor is open (Unity bug)
            if (EditorWindow.focusedWindow.titleContent.text == "Curve Editor")
            {
                EditorWindow.focusedWindow.Close();
            }
        }

        void OnSceneGUI()
        {
            Handles.color = Color.cyan;
            Handles.matrix = _particle.transform.localToWorldMatrix;
            
            // Draw emitter shape
            switch (_particle.shape)
            {
                case EmitterShape.Circle:
                    Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _particle.circleRadius);
                    
                    if (_particle.emitOnSurface)
                        break;

                    // Draw inner circle
                    Handles.color = Color.gray;
                    Handles.DrawWireDisc(Vector3.zero, Vector3.forward,
                        _particle.circleRadius - _particle.emitterThickness);
                    
                    break;
                case EmitterShape.Rectangle:
                    Handles.DrawWireCube(Vector3.zero, new Vector3(_particle.rectWidth, _particle.rectHeight));

                    if(_particle.emitOnSurface)
                        break;
                    
                    // Draw inner rectangle
                    Handles.DrawWireCube(Vector3.zero, new Vector3(_particle.rectWidth - _particle.emitterThickness, _particle.rectHeight - _particle.emitterThickness));

                    break;
                case EmitterShape.Line:
                    Handles.DrawLine(new Vector3(-_particle.lineLength/2, 0, 0), new Vector3(_particle.lineLength/2, 0, 0));
                    break;
                
                case EmitterShape.Directional:
                    Handles.DrawWireArc(Vector3.zero, Vector3.forward, PointInCircle(_angle.floatValue/2).normalized, _angle.floatValue,100);
                    Handles.DrawLine(Vector3.zero, PointInCircle(_angle.floatValue/2).normalized * 100f);
                    Handles.DrawLine(Vector3.zero, PointInCircle(-_angle.floatValue/2).normalized * 100f);
                    break;
            }

            Handles.color = Color.white;
        }
        
        // Get point on circle from angle
        private Vector3 PointInCircle(float angle){
            var rad = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin( rad ), Mathf.Cos( rad ), 0);
        }

        // Draw control panel in scene view
        void DrawSceneWindow(SceneView sceneView)
        {
            Handles.BeginGUI();
            
            Rect rect;

            if (PrefabStageUtility.GetCurrentPrefabStage())
            {
                rect = new Rect(Screen.width / EditorGUIUtility.pixelsPerPoint - 230,
                    Screen.height / EditorGUIUtility.pixelsPerPoint - 168, 220, 88);
            }
            else
            {
                rect = new Rect(Screen.width / EditorGUIUtility.pixelsPerPoint - 230,
                    Screen.height / EditorGUIUtility.pixelsPerPoint - 142, 220, 88);
            }

            GUILayout.BeginArea(rect, new GUIContent(_particle.gameObject.name),new GUIStyle("window"));
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(_particle.isPlaying ? "Pause" : "Play", new GUIStyle("ButtonLeft")))
            {
                if (_particle.isPlaying)
                    _particle.Pause();
                else
                    _particle.Play();
            }

            if (GUILayout.Button("Reset", new GUIStyle("ButtonMid")))
            {
                _particle.Stop(true);
                _particle.Play();
            }

            if (GUILayout.Button("Stop", new GUIStyle("ButtonRight")))
            {
                _particle.Stop(true);
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Playback Time",_particle.main ? _particle.main.time.ToString("F") : _particle.time.ToString("F"));
            EditorGUILayout.LabelField("Particles",_particle.particles.Count.ToString());

            GUILayout.EndArea();
            
            Handles.EndGUI();
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_space, new GUIContent("Simulation Space"), false);
            EditorGUILayout.PropertyField(_timeScale, new GUIContent("Simulation Time"), false);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_playMode, new GUIContent("Play*", "Shared between the particles group"), false);
            if (EditorGUI.EndChangeCheck())
            {
                _particle.PlayMode = (PlayMode)_playMode.enumValueIndex;
                EditorUtility.SetDirty(_particle);
            }
            EditorGUILayout.PropertyField(_loop, false);
            EditorGUILayout.PropertyField(_duration, false);
            EditorGUILayout.PropertyField(_delay,new GUIContent("Start Delay"), false);
            EditorGUILayout.PropertyField(_life, false);
            EditorGUILayout.PropertyField(_speed, false);
            EditorGUILayout.PropertyField(_startSize, new GUIContent("Start Size"), false);
            EditorGUILayout.PropertyField(_startRotation, new GUIContent("Start Rotation"), false);
            EditorGUILayout.PropertyField(_startColor, new GUIContent("Start Color"), false);
            EditorGUILayout.PropertyField(_raycast, false);
            EditorGUILayout.PropertyField(_maskable, false);
            
            _particle.moduleEmitterFoldout = Foldout("Emission", _particle.moduleEmitterFoldout, _emitterModuleIcon);
            if (_particle.moduleEmitterFoldout)
            {
                EditorGUILayout.PropertyField(_rate, new GUIContent("Rate per Second"), false);
                EditorGUILayout.PropertyField(_rateOverLifetime, new GUIContent("Rate over Duration") ,false);
                EditorGUILayout.PropertyField(_rateOverDistance, new GUIContent("Rate over Distance"), false);
                EditorGUILayout.PropertyField(_burst, true);
                DrawHorizontalLine();
                EditorGUILayout.PropertyField(_emitterShape, new GUIContent("Shape"),false);
                switch (_particle.shape)
                {
                    case EmitterShape.Point:
                        EditorGUILayout.PropertyField(_spreadType, new GUIContent("Spread"), false);
                        if (_particle.spreadType == SpreadType.Uniform)
                        {
                            EditorGUILayout.PropertyField(_spreadLoop, new GUIContent("Loop"), false);
                        }
                        break;
                    case EmitterShape.Circle:
                        EditorGUI.BeginDisabledGroup(_particle.fitRect);
                        EditorGUILayout.PropertyField(_radius,new GUIContent("Radius"), false);
                        EditorGUI.EndDisabledGroup();
                        _particle.fitRect = EditorGUILayout.Toggle("Fit Rect", _particle.fitRect);
                        EditorGUILayout.PropertyField(_surface, false);
                        if(!_surface.boolValue)
                            EditorGUILayout.PropertyField(_edge, false);
                        
                        _edge.floatValue= Mathf.Clamp(_edge.floatValue, 0f, _particle.circleRadius);

                        EditorGUILayout.PropertyField(_spreadType, new GUIContent("Spread"), false);
                        if (_particle.spreadType == SpreadType.Uniform)
                        {
                            EditorGUILayout.PropertyField(_spreadLoop, new GUIContent("Loop"), false);
                        }
                        break;
                    case EmitterShape.Rectangle:
                        EditorGUI.BeginDisabledGroup(_particle.fitRect);
                        EditorGUILayout.PropertyField(_width, false);
                        EditorGUILayout.PropertyField(_height, false);
                        EditorGUI.EndDisabledGroup();
                        _particle.fitRect = EditorGUILayout.Toggle("Fit Rect",_particle.fitRect);
                        EditorGUILayout.PropertyField(_surface, false);
                        if(!_surface.boolValue)
                            EditorGUILayout.PropertyField(_edge, false);
                        _edge.floatValue= Mathf.Clamp(_edge.floatValue, 0f, _particle.rectTransform.sizeDelta.x<_particle.rectTransform.sizeDelta.y?_particle.rectWidth:_particle.rectHeight);
                        EditorGUILayout.PropertyField(_spreadType, new GUIContent("Spread"), false);
                        if (_particle.spreadType == SpreadType.Uniform)
                        {
                            EditorGUILayout.PropertyField(_spreadLoop, new GUIContent("Loop"), false);
                        }
                        break;
                    case EmitterShape.Line:
                        EditorGUI.BeginDisabledGroup(_particle.fitRect);
                        EditorGUILayout.PropertyField(_length, false);
                        EditorGUI.EndDisabledGroup();
                        _particle.fitRect = EditorGUILayout.Toggle("Fit Rect",_particle.fitRect);
                        EditorGUILayout.PropertyField(_spreadType, new GUIContent("Spread"), false);
                        if (_particle.spreadType == SpreadType.Uniform)
                        {
                            EditorGUILayout.PropertyField(_spreadLoop, new GUIContent("Loop"), false);
                        }
                        break;
                    case EmitterShape.Directional:
                        EditorGUILayout.PropertyField(_angle, false);
                        EditorGUILayout.PropertyField(_spreadType, new GUIContent("Spread"), false);
                        if (_particle.spreadType == SpreadType.Uniform)
                        {
                            EditorGUILayout.PropertyField(_spreadLoop, new GUIContent("Loop"), false);
                        }
                        break;
                }
                
                DrawHorizontalLine();

                EditorGUILayout.PropertyField(_startPoint,new GUIContent("Emitter Constraint"), false);
                if (_particle.emitterConstraintEnabled)
                {
                    EditorGUILayout.PropertyField(_startPointTrans,new GUIContent("Transform"), false);
                }
                
                GUILayout.Space(2);
            }

            _particle.moduleParticleFoldout = Foldout("Particle", _particle.moduleParticleFoldout,_particleModuleIcon);
            if (_particle.moduleParticleFoldout)
            {
                EditorGUILayout.PropertyField(_texture, new GUIContent("Texture"));
                EditorGUILayout.PropertyField(_material, new GUIContent("Material"));
                
                DrawHorizontalLine();
                
                EditorGUILayout.PropertyField(_speedOverLifetime, false);
                
                DrawHorizontalLine();
                
                EditorGUILayout.PropertyField(_sizeOverLifetime, false);
                EditorGUILayout.PropertyField(_sizeBySpeed, false);
                EditorGUILayout.PropertyField(_sizeSpeedRange, new GUIContent("Size Speed Range"), false);
                
                DrawHorizontalLine();
                
                EditorGUILayout.PropertyField(_colorOverLifetime, false);
                EditorGUILayout.PropertyField(_colorBySpeed, false);
                EditorGUILayout.PropertyField(_colorSpeedRange, new GUIContent("Color Speed Range"), false);
                
                DrawHorizontalLine();
                
                EditorGUILayout.PropertyField(_rotateOverLifetime, false);
                EditorGUILayout.PropertyField(_rotationBySpeed, false);
                EditorGUILayout.PropertyField(_rotationSpeedRange, new GUIContent("Rotation Speed Range"), false);
                EditorGUILayout.PropertyField(_alignDirection);

                DrawHorizontalLine();
                
                EditorGUILayout.PropertyField(_sheetModule, new GUIContent("Texture Sheet"));
                
                if (_sheetModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_tile, new GUIContent("Tiles"), false);
                    EditorGUILayout.PropertyField(_sheetType, new GUIContent("Time Mode"), false);
                    
                    switch (_particle.textureSheetType)
                    {
                        case SheetType.Lifetime:
                            EditorGUILayout.PropertyField(_frameOverTime, false);
                            EditorGUILayout.PropertyField(_startFrame, false);
                            EditorGUILayout.PropertyField(_cycles, false);
                            ParticleSystem.MinMaxCurve c = _particle.textureSheetFrameOverTime;
                            switch (_particle.textureSheetFrameOverTime.mode)
                            {
                                case ParticleSystemCurveMode.Curve:
                                    c.curveMultiplier = Mathf.Clamp(_particle.textureSheetFrameOverTime.curveMultiplier, 0, _particle.textureTile.x*_particle.textureTile.y);
                                    break;
                                case ParticleSystemCurveMode.TwoCurves:
                                    c.curveMultiplier = Mathf.Clamp(_particle.textureSheetFrameOverTime.curveMultiplier, 0, _particle.textureTile.x*_particle.textureTile.y);
                                    break;
                                case ParticleSystemCurveMode.TwoConstants:
                                    c.constantMax = Mathf.Clamp(_particle.textureSheetFrameOverTime.constantMax, 0, _particle.textureTile.x*_particle.textureTile.y);
                                    break;
                                case ParticleSystemCurveMode.Constant:
                                    c.constant = Mathf.Clamp(_particle.textureSheetFrameOverTime.constant, 0, _particle.textureTile.x*_particle.textureTile.y);
                                    break;
                            }
                            _particle.textureSheetFrameOverTime = c;
                            break;
                        case SheetType.Speed:
                            EditorGUILayout.PropertyField(_frameSpeedRange, new GUIContent("Speed Range"), false);
                            EditorGUILayout.PropertyField(_startFrame, false);
                            break;
                        case SheetType.FPS:
                            EditorGUILayout.PropertyField(_startFrame, false);
                            EditorGUILayout.PropertyField(_frameFps, new GUIContent("FPS"),false);
                            break;
                    }

                    ParticleSystem.MinMaxCurve sf = _particle.textureSheetStartFrame;
                    switch (_particle.textureSheetStartFrame.mode)
                    {
                        case ParticleSystemCurveMode.Curve:
                            sf.curveMultiplier =  Mathf.Clamp(_particle.textureSheetStartFrame.curveMultiplier, 0, _particle.textureTile.x*_particle.textureTile.y);
                            break;
                        case ParticleSystemCurveMode.TwoCurves:
                            sf.curveMultiplier = Mathf.Clamp(_particle.textureSheetStartFrame.curveMultiplier, 0, _particle.textureTile.x*_particle.textureTile.y);
                            break;
                        case ParticleSystemCurveMode.TwoConstants:
                            sf.constantMax = Mathf.Clamp(_particle.textureSheetStartFrame.constantMax, 0, _particle.textureTile.x*_particle.textureTile.y);
                            break;
                        case ParticleSystemCurveMode.Constant:
                            sf.constant = Mathf.Clamp(_particle.textureSheetStartFrame.constant, 0, _particle.textureTile.x*_particle.textureTile.y);
                            break;
                    }

                    _particle.textureSheetStartFrame = sf;
                }
                
                DrawHorizontalLine();
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_trailModule, new GUIContent("Trails"), false);
                if (EditorGUI.EndChangeCheck())
                {
                    if (_trailModule.FindPropertyRelative("enabled").boolValue)
                    {
                        GameObject tr = new GameObject("Trails");
                        tr.transform.parent = _particle.transform;
                        tr.transform.localPosition = Vector3.zero;
                        tr.transform.localScale = Vector3.one;
                        tr.transform.localEulerAngles = Vector3.zero;
                        tr.AddComponent<CanvasRenderer>();
                        ParticleTrailRenderer r = tr.AddComponent<ParticleTrailRenderer>();
                        _particle.particleTrailRenderer = r;
                        r.particle = _particle;
                    }
                    else
                    {
                        var ch = _particle.GetComponentsInChildren<ParticleTrailRenderer>();
                        for (int i = 0; i < ch.Length; i++)
                        {
                            DestroyImmediate(ch[i].gameObject);
                        }
                    }
                }
                if (_trailModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.HelpBox("Please note that trails are in the beta stage and are not optimized for production use. Therefore, it is recommended that you apply for as few particles as possible.", MessageType.Warning);
                    EditorGUILayout.PropertyField(_trailRatio, new GUIContent("Trail Ratio"),false);
                    EditorGUILayout.PropertyField(_minimumVertexDistance, new GUIContent("Vertex Distance"),false);
                    EditorGUILayout.PropertyField(_trailLifetime, new GUIContent("Trail Lifetime"),false);
                    EditorGUILayout.PropertyField(_trailWidth, new GUIContent("Trail Width"),false);
                    EditorGUILayout.PropertyField(_inheritColor, new GUIContent("Inherit Particle Color"),false);
                    EditorGUILayout.PropertyField(_trailColorOverLifetime, new GUIContent("Color over Lifetime"),false);
                    EditorGUILayout.PropertyField(_trailColorOverTrail, new GUIContent("Color over Trail"),false);
                    EditorGUILayout.PropertyField(_dieWithParticle, new GUIContent("Die With Particle"),false);
                }
                GUILayout.Space(4);
            }

            _particle.moduleMovementFoldout = Foldout("Movement", _particle.moduleMovementFoldout,_movementModuleIcon);
            if (_particle.moduleMovementFoldout)
            {
                EditorGUILayout.PropertyField(_targetModule, new GUIContent("Attractor"),false);
                if (_targetModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_targetTransform,new GUIContent("Attractor"), false);
                    EditorGUILayout.PropertyField(_targetCurve,new GUIContent("Attractor Lerp"), false);
                    if(_particle.attractorTarget != null)
                        EditorGUI.BeginDisabledGroup(_particle.attractorTarget.GetType() != typeof(RectTransform));
                    else
                        EditorGUI.BeginDisabledGroup(false);
                    EditorGUILayout.PropertyField(_targetMode, new GUIContent("Attractor Mode"),false);
                    EditorGUI.EndDisabledGroup();
                }
                DrawHorizontalLine();
                EditorGUILayout.PropertyField(_noiseModule, new GUIContent("Noise"), false);
                if (_noiseModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_noiseStrength,new GUIContent("Strength"), false);
                    
                    EditorGUILayout.PropertyField(_noiseFreq,new GUIContent("Frequency"), false);
                    
                    EditorGUILayout.PropertyField(_noiseOct, new GUIContent("Octaves"),false);
                    
                    _particle.noise.SetFrequency(_particle.noiseFrequency/100f);
                    _particle.noise.SetFractalOctaves(_particle.noiseOctaves);
                }

                DrawHorizontalLine();
                EditorGUILayout.PropertyField(_velocityModule, new GUIContent("Velocity"), false);
                if (_velocityModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_velocitySpace,new GUIContent("Space"), false);
                    EditorGUILayout.PropertyField(_velocityOverLifetime,new GUIContent("Velocity Over Lifetime"), false);
                }
                DrawHorizontalLine();
                EditorGUILayout.PropertyField(_gravityModule, new GUIContent("Gravity"), false);
                if (_gravityModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_gravity,new GUIContent("Gravity Force"), false);
                }
                DrawHorizontalLine();
                EditorGUILayout.PropertyField(_vortexModule, new GUIContent("Vortex"), false);
                if (_vortexModule.FindPropertyRelative("enabled").boolValue)
                {
                    EditorGUILayout.PropertyField(_vortexStrength, false);
                }
                GUILayout.Space(4);
            }
            _particle.moduleEventsFoldout = Foldout("Events", _particle.moduleEventsFoldout,_eventModuleIcon);
            if (_particle.moduleEventsFoldout)
            {
                EditorGUILayout.PropertyField(_onStart, false);
                EditorGUILayout.PropertyField(_onFirstParticleFinish, false);
                EditorGUILayout.PropertyField(_onParticleFinish, false);
                EditorGUILayout.PropertyField(_onLastParticleFinish, false);
                EditorGUILayout.PropertyField(_onFinish, false);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        void EditorUpdate()
        {
            EditorApplication.QueuePlayerLoopUpdate();
        } 
        
        private void DrawHorizontalLine(int height = 1) {
            GUILayout.Space(2);
 
            Rect rect = GUILayoutUtility.GetRect(10, height, GUILayout.ExpandWidth(true));
            rect.height = height;
            rect.xMin = 18;
            rect.xMax = EditorGUIUtility.currentViewWidth - 18;
 
            Color lineColor = new Color(0.15f, 0.15f, 0.15f, 1);
            EditorGUI.DrawRect(rect, lineColor);
            GUILayout.Space(2);
        }

        private bool Foldout(string title, bool display, Texture icon)
        {
            var rect = GUILayoutUtility.GetRect(16f, 24f, FoldoutStyle);
            GUI.Box(rect, title, FoldoutStyle);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(new Rect(rect.width - 4f, rect.y + 4f, 13f, 13f), false, false, display, false);
                GUI.DrawTexture(toggleRect, icon);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
        }
    }
}