using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using AssetKits.ParticleImage.Enumerations;
using UnityEngine.Serialization;
using PlayMode = AssetKits.ParticleImage.Enumerations.PlayMode;

namespace AssetKits.ParticleImage
{
    [AddComponentMenu("UI/Particle Image/Particle Image")]
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class ParticleImage : MaskableGraphic
    {
        [SerializeField]
        private ParticleImage _main;
        [SerializeField]
        private ParticleImage[] _children;

        /// <summary>
        /// Child emitters of this emitter.
        /// </summary>
        public ParticleImage[] children
        {
            get
            {
                return _children;
            }
            private set
            {
                _children = value;
            }
        }
        
        /// <summary>
        /// Root emitter of this system.
        /// </summary>
        public ParticleImage main
        {
            get
            {
                if (_main == null) _main = GetMain();
                return _main;
            }
            private set
            {
                _main = value;
            }
        }

        /// <summary>
        /// Returns true if this emitter is the root emitter of this system.
        /// </summary>
        public bool isMain
        {
            get
            {
                return main == this;
            }
        }

        private RectTransform _canvasRect;
        public RectTransform canvasRect
        {
            get
            {
                return _canvasRect;
            }
            set
            {
                _canvasRect = value;
            }
        }
        
        [SerializeField]
        private Simulation _space = Simulation.Local;
        public Simulation space
        {
            get => _space;
            set
            {
                _space = value;
            }
        }
        
        [SerializeField]
        private TimeScale _timeScale = TimeScale.Normal;
        public TimeScale timeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
            }
        }

        [SerializeField]
        private Module _emitterConstraintEnabled = new Module(false);
        public bool emitterConstraintEnabled
        {
            get
            {
                return _emitterConstraintEnabled.enabled;
            }
            set
            {
                _emitterConstraintEnabled.enabled = value;
            }
        }
        
        [SerializeField]
        private Transform _emitterConstraintTransform;
        public Transform emitterConstraintTransform
        {
            get => _emitterConstraintTransform;
            set
            {
                _emitterConstraintTransform = value;
            }
        }
        
        [SerializeField]
        private EmitterShape _shape = EmitterShape.Circle;
        /// <summary>
        ///   <para>The type of shape to emit particles from.</para>
        /// </summary>
        public EmitterShape shape
        {
            get => _shape;
            set => _shape = value;
        }
        
        [SerializeField]
        private SpreadType _spread = SpreadType.Random;
        
        /// <summary>
        ///  <para>The type of spread to use when emitting particles.</para>
        /// </summary>
        public SpreadType spreadType
        {
            get => _spread;
            set => _spread = value;
        }
        
        [SerializeField]
        private float _spreadLoop = 1;
        
        /// <summary>
        /// Loop count for spread.
        /// </summary>
        public float spreadLoop
        {
            get => _spreadLoop;
            set => _spreadLoop = value;
        }
        
        [SerializeField]
        private float _startDelay = 0;
        
        /// <summary>
        ///   <para>Start delay in seconds.</para>
        /// </summary>
        public float startDelay
        {
            get => _startDelay;
            set => _startDelay = value;
        }
        
        [SerializeField]
        private float _radius = 50;
        /// <summary>
        ///   <para>Radius of the circle shape to emit particles from.</para>
        /// </summary>
        public float circleRadius
        {
            get => _radius;
            set => _radius = value;
        }
        
        [SerializeField]
        private float _width = 100;
        /// <summary>
        ///   <para>Width of the rectangle shape to emit particles from.</para>
        /// </summary>
        public float rectWidth
        {
            get => _width;
            set => _width = value;
        }
        
        [SerializeField]
        private float _height = 100;
        /// <summary>
        ///   <para>Height of the rectangle shape to emit particles from.</para>
        /// </summary>
        public float rectHeight
        {
            get => _height;
            set => _height = value;
        }
        
        [SerializeField]
        private float _angle = 45;
        /// <summary>
        ///   <para>Angle of the directional shape to emit particles from.</para>
        /// </summary>
        public float directionAngle
        {
            get => _angle;
            set => _angle = value;
        }

        [SerializeField]
        private float _length = 100f;
        /// <summary>
        ///   <para>Length of the line shape to emit particles from.</para>
        /// </summary>
        public float lineLength
        {
            get => _length;
            set => _length = value;
        }
        
        [SerializeField]
        private bool _fitRect;
        
        public bool fitRect
        {
            get
            {
                return _fitRect;
            }
            set
            {
                _fitRect = value;
                if(value) 
                    FitRect();
            }
        }
        
        [SerializeField]
        private bool _emitOnSurface = true;
        /// <summary>
        ///   <para>Emit on the whole surface of the current shape.</para>
        /// </summary>
        public bool emitOnSurface
        {
            get => _emitOnSurface;
            set => _emitOnSurface = value;
        }
        
        [SerializeField]
        private float _emitterThickness;
        /// <summary>
        ///   <para>Thickness of the shape's edge from which to emit particles if emitOnSurface is disabled.</para>
        /// </summary>
        public float emitterThickness
        {
            get => _emitterThickness;
            set => _emitterThickness = value;
        }
        
        [SerializeField]
        private bool _loop = true;
        /// <summary>
        ///   <para>Determines whether the Particle Image is looping.</para>
        /// </summary>
        public bool loop
        {
            get => _loop;
            set => _loop = value;
        }
        
        [SerializeField]
        private float _duration = 5f;
        /// <summary>
        ///   <para>The duration of the Particle Image in seconds</para>
        /// </summary>
        public float duration
        {
            get => _duration;
            set => _duration = value;
        }
        
        [SerializeField]
        private PlayMode _playMode = PlayMode.OnAwake;
        public PlayMode PlayMode
        {
            get
            {
                return _playMode;
            }
            set
            {
                _playMode = value;
                if (isMain && children != null)
                {
                    foreach (var particleImage in children)
                    {
                        particleImage._playMode = value;
                    }
                }
                else if(!isMain)
                {
                    main.PlayMode = value;
                }
            }
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _startSize = new SeparatedMinMaxCurve(40f);
        public SeparatedMinMaxCurve startSize
        {
            get => _startSize;
            set => _startSize = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxGradient _startColor = new ParticleSystem.MinMaxGradient(Color.white);
        public ParticleSystem.MinMaxGradient startColor
        {
            get => _startColor;
            set => _startColor = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _lifetime = new ParticleSystem.MinMaxCurve(1f);
        public ParticleSystem.MinMaxCurve lifetime
        {
            get => _lifetime;
            set => _lifetime = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _startSpeed = new ParticleSystem.MinMaxCurve(2f);
        public ParticleSystem.MinMaxCurve startSpeed
        {
            get => _startSpeed;
            set => _startSpeed = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxGradient _colorOverLifetime = new ParticleSystem.MinMaxGradient(new Gradient());
        public ParticleSystem.MinMaxGradient colorOverLifetime
        {
            get => _colorOverLifetime;
            set => _colorOverLifetime = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxGradient _colorBySpeed = new ParticleSystem.MinMaxGradient(new Gradient());
        public ParticleSystem.MinMaxGradient colorBySpeed
        {
            get => _colorBySpeed;
            set => _colorBySpeed = value;
        }
        
        [SerializeField]
        private SpeedRange _colorSpeedRange = new SpeedRange(0f, 1f);
        public SpeedRange colorSpeedRange
        {
            get => _colorSpeedRange;
            set => _colorSpeedRange = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _sizeOverLifetime = new SeparatedMinMaxCurve(new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(1f,1f)}));
        public SeparatedMinMaxCurve sizeOverLifetime
        {
            get => _sizeOverLifetime;
            set => _sizeOverLifetime = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _sizeBySpeed = new SeparatedMinMaxCurve(new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(1f,1f)}));
        public SeparatedMinMaxCurve sizeBySpeed
        {
            get => _sizeBySpeed;
            set => _sizeBySpeed = value;
        }
        
        [SerializeField]
        private SpeedRange _sizeSpeedRange = new SpeedRange(0f, 1f);
        public SpeedRange sizeSpeedRange
        {
            get => _sizeSpeedRange;
            set => _sizeSpeedRange = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _startRotation = new SeparatedMinMaxCurve(0f);
        public SeparatedMinMaxCurve startRotation
        {
            get => _startRotation;
            set => _startRotation = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _rotationOverLifetime = new SeparatedMinMaxCurve(0f);
        public SeparatedMinMaxCurve rotationOverLifetime
        {
            get => _rotationOverLifetime;
            set => _rotationOverLifetime = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _rotationBySpeed = new SeparatedMinMaxCurve(new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(1f,1f)}));
        public SeparatedMinMaxCurve rotationBySpeed
        {
            get => _rotationBySpeed;
            set => _rotationBySpeed = value;
        }
        
        [SerializeField]
        private SpeedRange _rotationSpeedRange = new SpeedRange(0f, 1f);
        public SpeedRange rotationSpeedRange
        {
            get => _rotationSpeedRange;
            set => _rotationSpeedRange = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _speedOverLifetime = new ParticleSystem.MinMaxCurve(1f);
        public ParticleSystem.MinMaxCurve speedOverLifetime
        {
            get => _speedOverLifetime;
            set => _speedOverLifetime = value;
        }
        
        [SerializeField]
        private bool _alignToDirection;
        /// <summary>
        ///   <para>Align particles based on their direction of travel.</para>
        /// </summary>
        public bool alignToDirection
        {
            get => _alignToDirection;
            set => _alignToDirection = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _gravity = new ParticleSystem.MinMaxCurve(-9.81f);
        public ParticleSystem.MinMaxCurve gravity
        {
            get => _gravity;
            set => _gravity = value;
        }

        [SerializeField]
        private Module _targetModule = new Module(false);
        public bool attractorEnabled
        {
            get
            {
                return _targetModule.enabled;
            }
            set
            {
                _targetModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Transform _attractorTarget;
        public Transform attractorTarget
        {
            get => _attractorTarget;
            set => _attractorTarget = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _toTarget = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(new []{new Keyframe(0f,0f), new Keyframe(1f,1f)}));
        public ParticleSystem.MinMaxCurve attractorLerp
        {
            get => _toTarget;
            set => _toTarget = value;
        }
        
        [SerializeField]
        private AttractorType _targetMode = AttractorType.Pivot;
        public AttractorType attractorType
        {
            get => _targetMode;
            set => _targetMode = value;
        }
        
        [SerializeField]
        private Module _noiseModule = new Module(false);
        public bool noiseEnabled
        {
            get
            {
                return _noiseModule.enabled;
            }
            set
            {
                _noiseModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Module _gravityModule = new Module(false);
        public bool gravityEnabled
        {
            get
            {
                return _gravityModule.enabled;
            }
            set
            {
                _gravityModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Module _vortexModule = new Module(false);
        public bool vortexEnabled
        {
            get
            {
                return _vortexModule.enabled;
            }
            set
            {
                _vortexModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Module _velocityModule = new Module(false);
        public bool velocityEnabled
        {
            get
            {
                return _velocityModule.enabled;
            }
            set
            {
                _velocityModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Simulation _velocitySpace;
        public Simulation velocitySpace
        {
            get => _velocitySpace;
            set => _velocitySpace = value;
        }
        
        [SerializeField]
        private SeparatedMinMaxCurve _velocityOverLifetime = new SeparatedMinMaxCurve(0f, true, false);
        public SeparatedMinMaxCurve velocityOverLifetime
        {
            get => _velocityOverLifetime;
            set => _velocityOverLifetime = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _vortexStrength;
        public ParticleSystem.MinMaxCurve vortexStrength
        {
            get => _vortexStrength;
            set => _vortexStrength = value;
        }

        [SerializeField]
        private Module _sheetModule = new Module(false);
        public bool textureSheetEnabled
        {
            get
            {
                return _sheetModule.enabled;
            }
            set
            {
                _sheetModule.enabled = value;
            }
        }
        
        [SerializeField]
        private Vector2Int _textureTile = Vector2Int.one;
        public Vector2Int textureTile
        {
            get => _textureTile;
            set => _textureTile = value;
        }
        
        [SerializeField]
        private SheetType _sheetType = SheetType.FPS;
        public SheetType textureSheetType
        {
            get => _sheetType;
            set => _sheetType = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _frameOverTime;
        public ParticleSystem.MinMaxCurve textureSheetFrameOverTime
        {
            get => _frameOverTime;
            set => _frameOverTime = value;
        }
        
        [SerializeField]
        private ParticleSystem.MinMaxCurve _startFrame = new ParticleSystem.MinMaxCurve(0f);
        public ParticleSystem.MinMaxCurve textureSheetStartFrame
        {
            get => _startFrame;
            set => _startFrame = value;
        }
        
        [SerializeField]
        private SpeedRange _frameSpeedRange = new SpeedRange(0f, 1f);
        public SpeedRange textureSheetFrameSpeedRange
        {
            get => _frameSpeedRange;
            set => _frameSpeedRange = value;
        }
        
        [SerializeField]
        private int _textureSheetFPS = 25;
        public int textureSheetFPS
        {
            get => _textureSheetFPS;
            set => _textureSheetFPS = value;
        }
        
        [SerializeField]
        private int _textureSheetCycles = 1;
        public int textureSheetCycles
        {
            get => _textureSheetCycles;
            set => _textureSheetCycles = value;
        }
        
        private List<Particle> _particles = new List<Particle>();
        
        /// <summary>
        /// List of particles in the system.
        /// </summary>
        public List<Particle> particles => _particles;

        [SerializeField]
        private float _rate = 50;
        
        /// <summary>
        /// The rate at which the emitter spawns new particles per second.
        /// </summary>
        public float rateOverTime
        {
            get => _rate;
            set => _rate = value;
        }
        
        [SerializeField]
        private float _rateOverLifetime = 0;
        
        /// <summary>
        /// The rate at which the emitter spawns new particles over emitter duration.
        /// </summary>
        public float rateOverLifetime
        {
            get => _rateOverLifetime;
            set => _rateOverLifetime = value;
        }
        
        [SerializeField]
        private float _rateOverDistance = 0;
        
        /// <summary>
        /// The rate at which the emitter spawns new particles over distance per pixel.
        /// </summary>
        public float rateOverDistance
        {
            get => _rateOverDistance;
            set => _rateOverDistance = value;
        }
        
        [SerializeField]
        private List<Burst> _bursts = new List<Burst>(); 

        [FormerlySerializedAs("_trailRenderer")] [SerializeField]
        private ParticleTrailRenderer _particleTrailRenderer;

        public ParticleTrailRenderer particleTrailRenderer
        {
            get
            {
                if (trailsEnabled)
                {
                    if (!_particleTrailRenderer)
                    {
                        _particleTrailRenderer = GetComponentInChildren<ParticleTrailRenderer>();

                        if (!_particleTrailRenderer)
                        {
                            GameObject tr = new GameObject("Trails");
                            tr.transform.parent = transform;
                            tr.transform.localPosition = Vector3.zero;
                            tr.transform.localScale = Vector3.one;
                            tr.transform.localEulerAngles = Vector3.zero;
                            tr.AddComponent<CanvasRenderer>();
                            ParticleTrailRenderer r = tr.AddComponent<ParticleTrailRenderer>();
                            _particleTrailRenderer = r;
                        }
                    }
                    return _particleTrailRenderer;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _particleTrailRenderer = value;
            }
        }

        [SerializeField] private Module _trailModule;

        /// <summary>
        /// The trails enabled.
        /// </summary>
        public bool trailsEnabled
        {
            get => _trailModule.enabled;
            set => _trailModule.enabled = value;
        }

        [SerializeField] private ParticleSystem.MinMaxCurve _trailWidth = new ParticleSystem.MinMaxCurve(1f,new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(1f,0f)}));

        /// <summary>
        /// The width of the trail in pixels.
        /// </summary>
        public ParticleSystem.MinMaxCurve trailWidth
        {
            get => _trailWidth;
            set => _trailWidth = value;
        }
        
        [SerializeField] private float _trailLifetime = 1f;
        
        /// <summary>
        /// Trail lifetime in seconds
        /// </summary>
        public float trailLifetime
        {
            get => _trailLifetime;
            set => _trailLifetime = value;
        }
        
        [SerializeField] private float _minimumVertexDistance = 10f;
        
        /// <summary>
        /// Vertex distance in canvas pixels
        /// </summary>
        public float minimumVertexDistance
        {
            get => _minimumVertexDistance;
            set => _minimumVertexDistance = value;
        }
        
        [SerializeField] private ParticleSystem.MinMaxGradient _trailColorOverLifetime = new ParticleSystem.MinMaxGradient(Color.white);
        
        /// <summary>
        /// The color of the trail over its lifetime.
        /// </summary>
        public ParticleSystem.MinMaxGradient trailColorOverLifetime
        {
            get => _trailColorOverLifetime;
            set => _trailColorOverLifetime = value;
        }
        
        [SerializeField] private ParticleSystem.MinMaxGradient _trailColorOverTrail = new ParticleSystem.MinMaxGradient(Color.white);
        
        /// <summary>
        /// The color of the trail over the lifetime of the trail.
        /// </summary>
        public ParticleSystem.MinMaxGradient trailColorOverTrail
        {
            get => _trailColorOverTrail;
            set => _trailColorOverTrail = value;
        }
        

        [SerializeField] private bool _inheritParticleColor;

        public bool inheritParticleColor
        {
            get => _inheritParticleColor;
            set => _inheritParticleColor = value;
        }

        [SerializeField] private bool _dieWithParticle = false;
        
        public bool dieWithParticle
        {
            get => _dieWithParticle;
            set => _dieWithParticle = value;
        }
        
        [Range(0f,1f)]
        [SerializeField] private float _trailRatio = 1f;
        
        public float trailRatio
        {
            get => _trailRatio;
            set
            {
                _trailRatio = Mathf.Clamp01(value);
            } 
        }
        
        private float _time;
        public float time => _time;

        private float _loopTimer;

        private float _t;
        private float _t2;
        private float _burstTimer;
        private Vector2 _position;
        
        private Noise _noise = new Noise();
        public Noise noise
        {
            get => _noise;
            set => _noise = value;
        }
        
        [SerializeField]
        private int _noiseOctaves = 1;
        public int noiseOctaves
        {
            get => _noiseOctaves;
            set
            {
                _noiseOctaves = value;
                _noise.SetFractalOctaves(_noiseOctaves);
            }
        }
        
        [SerializeField]
        private float _noiseFrequency = 1f;
        public float noiseFrequency
        {
            get => _noiseFrequency;
            set
            {
                _noiseFrequency = value;
                _noise.SetFrequency(_noiseFrequency);
            }
        }
        
        [SerializeField]
        private float _noiseStrength = 1f;
        
        public float noiseStrength
        {
            get => _noiseStrength;
            set => _noiseStrength = value;
        }
        
        
        private bool _emitting;
        
        /// <summary>
        /// Determines if the particle system is emitting.
        /// </summary>
        public bool isEmitting
        {
            get { return _emitting;}
            private set { _emitting = value; }
        }
        
        private bool _playing;
        
        /// <summary>
        /// Determines if the particle system is playing.
        /// </summary>
        public bool isPlaying
        {
            get { return _playing;}
            private set { _playing = value; }
        }
        
        private bool _stopped;
        
        /// <summary>
        /// Determines whether the Particle System is stopped.
        /// </summary>
        public bool isStopped
        {
            get { return _stopped;}
            private set { _stopped = value; }
        }

        private bool _paused;
        
        /// <summary>
        /// Determines whether the Particle System is paused.
        /// </summary>
        public bool isPaused
        {
            get { return _paused;}
            private set { _paused = value; }
        }

        [SerializeField]
        private UnityEvent _onStart = new UnityEvent();
        
        /// <summary>
        /// Called when the particle system starts.
        /// </summary>
        public UnityEvent onStart => _onStart;

        [SerializeField]
        private UnityEvent _onFirstParticleFinish = new UnityEvent();
        
        /// <summary>
        /// Called when the first piece of a particle finishes.
        /// </summary>
        public UnityEvent onFirstParticleFinish => _onFirstParticleFinish;
        
        [SerializeField]
        private UnityEvent _onParticleFinish = new UnityEvent();
        
        /// <summary>
        /// Called when any piece of a particle finishes.
        /// </summary>
        public UnityEvent onParticleFinish => _onParticleFinish;
        
        [SerializeField]
        private UnityEvent _onLastParticleFinish = new UnityEvent();
        
        /// <summary>
        /// Called when the last piece of a particle finishes.
        /// </summary>
        public UnityEvent onLastParticleFinish => _onLastParticleFinish;
        
        [SerializeField]
        private UnityEvent _onStop = new UnityEvent();
        
        /// <summary>
        /// Called when the particle system is stopped.
        /// </summary>
        public UnityEvent onStop => _onStop;

        private Vector3 _lastPosition;
        private Vector3 _deltaPosition;
        /// <summary>
        /// Delta position of the particle system.
        /// </summary>
        public Vector3 deltaPosition => _deltaPosition;

        private Camera _camera;
        private Camera camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }
        
        private bool _firstParticleFinished;
        
        private int _orderPerSec;
        private int _orderOverLife;
        private int _orderOverDistance;

        public bool moduleEmitterFoldout;
        public bool moduleParticleFoldout;
        public bool moduleMovementFoldout;
        public bool moduleEventsFoldout;

        void Awake()
        {
            _noise.SetNoiseType(Noise.NoiseType.OpenSimplex2);
            _noise.SetFrequency(_noiseFrequency / 100f);
            _noise.SetFractalOctaves(_noiseOctaves);
            
            Clear();

            if (PlayMode == PlayMode.OnAwake && Application.isPlaying)
            {
                Play();
            }
        }

        public void OnEnable()
        {
            
            if (isMain)
            {
                children = GetChildren();
            }
            
            if (fitRect)
            {
                FitRect();
            }
            
            main = GetMain();
            main.children = main.GetChildren();
            
            _lastPosition = transform.position;
            
            if (canvas)
            {
                canvasRect = canvas.gameObject.GetComponent<RectTransform>();
            }

            if (PlayMode == PlayMode.OnEnable && Application.isPlaying)
            {
                Stop(true);
                Clear();
                Play();
            }
            
            RecalculateMasking();
            RecalculateClipping();
            
            SetAllDirty();
        }

        public ParticleImage GetMain()
        {
            if (transform.parent)
            {
                if (transform.parent.TryGetComponent<ParticleImage>(out ParticleImage p))
                {
                    return p.GetMain();
                }
            }

            return this;
        }
        
        /// <summary>
        /// Get all children of this Particle Image.
        /// </summary>
        /// <returns>
        /// A list of all children ParticleImage.
        /// </returns>
        public ParticleImage[] GetChildren()
        {
            if (transform.childCount <= 0) return null;

            var ch = GetComponentsInChildren<ParticleImage>().Where(t => t != this);

            if (ch.Any())
            {
                return ch.ToArray();
            }

            return null;
        }

        private void OnTransformChildrenChanged()
        {
            main = GetMain();
            if(isMain)
                children = GetChildren();

            if (Application.isEditor)
            {
                Stop(true);
                Clear();
                Play();
            }
        }

        void OnTransformParentChanged()
        {
            main = GetMain();
            if(isMain)
                children = GetChildren();
            
            if (Application.isEditor)
            {
                Stop(true);
                Clear();
                Play();
            }
        }

        /// <summary>
        /// Starts the particle system.
        /// </summary>
        public void Play()
        {
            main.DoPlay();
        }

        private void DoPlay()
        {
            if (isMain && children != null)
            {
                foreach (var particleImage in children)
                {
                    particleImage.DoPlay();
                }
            }
            onStart.Invoke();
            _time = 0f;
            _burstTimer = 0f;
            for (int i = 0; i < _bursts.Count; i++)
            {
                _bursts[i].used = false;
            }
            isEmitting = true;
            isPlaying = true;
            isPaused = false;
            isStopped = false;
        }

        /// <summary>
        /// Pauses the particle system.
        /// </summary>
        public void Pause()
        {
            main.DoPause();
        }

        private void DoPause()
        {
            if (isMain && children != null)
            {
                foreach (var particleImage in children)
                {
                    particleImage.DoPause();
                }
            }
            
            isEmitting = false;
            isPlaying = false;
            isPaused = true;
        }
        
        /// <summary>
        /// Stops playing the Particle System.
        /// </summary>
        public void Stop()
        {
            Stop(false);
        }

        /// <summary>
        /// Stops playing the Particle System using the supplied stop behaviour.
        /// </summary>
        /// <param name="stopAndClear">
        /// If true, the particle system will be cleared and all emitted particles will be destroyed.
        /// </param>
        public void Stop(bool stopAndClear)
        {
            main.DoStop(stopAndClear);
        }

        private void DoStop(bool stopAndClear)
        {
            if (isMain && children != null)
            {
                foreach (var particleImage in children)
                {
                    particleImage.DoStop(stopAndClear);
                }
            }
            
            _orderPerSec = 0;
            _orderOverLife = 0;
            _orderOverDistance = 0;
            for (int i = 0; i < _bursts.Count; i++)
            {
                _bursts[i].used = false;
            }
            
            if (stopAndClear)
            {
                isStopped = true;
                isPlaying = false;
                Clear();
            }
            isEmitting = false;
            if (isPaused)
            {
                isPaused = false;
                isStopped = true;
                isPlaying = false;
                Clear();
            }
            for (int i = 0; i < _bursts.Count; i++)
            {
                _bursts[i].used = false;
            }
            _firstParticleFinished = false;
            SetVerticesDirty();
            SetMaterialDirty();
            if (particleTrailRenderer)
            {
                particleTrailRenderer.SetVerticesDirty();
                particleTrailRenderer.SetMaterialDirty();
            }
        }

        /// <summary>
        /// Remove all particles from the Particle System.
        /// </summary>
        public void Clear()
        {
            main.DoClear();
        }

        private void DoClear()
        {
            if (isMain && children != null)
            {
                foreach (var particleImage in children)
                {
                    particleImage.DoClear();
                }
            }
            for (int i = 0; i < _bursts.Count; i++)
            {
                _bursts[i].used = false;
            }
            _time = 0;
            _burstTimer = 0;
            _particles.Clear();
            SetVerticesDirty();
            SetMaterialDirty();
            if (particleTrailRenderer)
            {
                particleTrailRenderer.SetVerticesDirty();
                particleTrailRenderer.SetMaterialDirty();
            }
        }

        void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (isPlaying)
            {
                _deltaPosition = transform.position - _lastPosition;
           
                if (_emitterConstraintTransform && _emitterConstraintEnabled.enabled)
                {
                    if (_emitterConstraintTransform is RectTransform)
                    {
                        transform.position = _emitterConstraintTransform.position;
                    }
                    else
                    {
                        Vector3 canPos;
                        Vector3 viewportPos = camera.WorldToViewportPoint(_emitterConstraintTransform.position);

                        canPos = new Vector3(viewportPos.x.Remap(0.5f, 1.5f, 0f, canvasRect.rect.width),
                            viewportPos.y.Remap(0.5f, 1.5f, 0f, canvasRect.rect.height), 0);
                        
                        canPos = canvasRect.transform.TransformPoint(canPos);

                        canPos = transform.parent.InverseTransformPoint(canPos);

                        transform.localPosition = canPos;
                    }
                }

                if (isMain)
                {
                    _time += _timeScale == TimeScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                }
                else
                {
                    _time = main.time;
                }
                
                _loopTimer += _timeScale == TimeScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                
                _burstTimer += _timeScale == TimeScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                
                
                SetVerticesDirty();
                SetMaterialDirty();
                
                if (particleTrailRenderer)
                {
                    particleTrailRenderer.SetVerticesDirty();
                    particleTrailRenderer.SetMaterialDirty();
                }
            }
            
            
            if (isEmitting)
            {
                //Emit per second
                if(_rate > 0)
                {
                    if ((_time < (_duration + _startDelay) || _loop) && _time > _startDelay)
                    {
                        float dur = 1f / _rate;
                        _t += _timeScale == TimeScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                        while(_t >= dur)
                        {
                            _t -= dur;
                            _orderPerSec++;
                            _particles.Insert(0,GenerateParticle(_orderPerSec,1, null));
                        }
                    }
                }

                //Emit over lifetime
                if (_rateOverLifetime > 0)
                {
                    if ((_time < (_duration + _startDelay) || _loop) && _time > _startDelay)
                    {
                        float dur = _duration / _rateOverLifetime;
                        _t2 += _timeScale == TimeScale.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
                        while(_t2 >= dur)
                        {
                            _t2 -= dur;
                            _orderOverLife++;
                            _particles.Insert(0,GenerateParticle(_orderOverLife,2, null));
                        }
                    }
                }

                //Emit over distance
                if (_rateOverDistance > 0)
                {
                    if (_deltaPosition.magnitude > 1f / _rateOverDistance)
                    {
                        _orderOverDistance++;
                        _particles.Insert(0,GenerateParticle(_orderOverDistance,3, null));
                        _lastPosition = transform.position;
                    }
                }

                //Emit bursts
                if (_bursts != null)
                {
                    for (int i = 0; i < _bursts.Count; i++)
                    {
                        if (_burstTimer > _bursts[i].time + _startDelay && _bursts[i].used == false)
                        {
                            for (int j = 0; j < _bursts[i].count; j++)
                            {
                                _particles.Insert(0,GenerateParticle(j,0, _bursts[i]));
                            }
                            
                            _bursts[i].used = true;
                        }
                    }
                }
                
                if (_loop && _burstTimer >= _duration)
                {
                    _burstTimer = 0;
                    for (int i = 0; i < _bursts.Count; i++)
                    {
                        _bursts[i].used = false;
                    }
                }

                if (_time >= _duration + _startDelay && !_loop)
                {
                    isEmitting = false;
                }
                
                if(_loop && _loopTimer >= _duration + _startDelay)
                {
                    _loopTimer = 0;
                    _orderPerSec = 0;
                    _orderOverLife = 0;
                    _orderOverDistance = 0;
                }
            }
            
            if (isPlaying && _particles.Count <= 0 && !isEmitting && isMain)
            {
                if (canStop())
                {
                    onStop.Invoke();
                    Stop(true);
                }
            }
        }

        /// <summary>
        /// Add a burst to the particle system
        /// </summary>
        public void AddBurst(float time, int count)
        {
            _bursts.Add(new Burst(time, count));
        }
        
        /// <summary>
        /// Remove burst at index
        /// </summary>
        public void RemoveBurst(int index)
        {
            _bursts.RemoveAt(index);
        }
        
        /// <summary>
        /// Set particle burst at index
        /// </summary>
        public void SetBurst(int index, float time, int count)
        {
            if (_bursts.Count > index)
            {
                _bursts[index] = new Burst(time, count);
            }
        }

        private bool canStop()
        {
            if (children != null)
            {
                return children.All(x => x.isEmitting == false && x._particles.Count <= 0);
            }
            else
            {
                return true;
            }
        }
        
        private Vector2 GetPointOnRect(float angle, float w, float h)
        {
            // Calculate the sine and cosine of the angle
            var sine = Mathf.Sin(angle);
            var cosine = Mathf.Cos(angle);

            // Calculate the x and y coordinates of the point
            // based on the sign of sine and cosine.
            // If sine is positive, the y coordinate is half the height of the rectangle.
            // If sine is negative, the y coordinate is negative half the height of the rectangle.
            // Similarly, if cosine is positive, the x coordinate is half the width of the rectangle.
            // If cosine is negative, the x coordinate is negative half the width of the rectangle.
            float dy = sine > 0 ? h / 2 : h / -2;
            float dx = cosine > 0 ? w / 2 : w / -2;

            // Check if the slope of the line between the origin and the point is steeper in
            // the x direction or in the y direction. If it is steeper in the x direction,
            // adjust the y coordinate so that the point is on the edge of the rectangle.
            // If it is steeper in the y direction, adjust the x coordinate instead.
            if (Mathf.Abs(dx * sine) < Mathf.Abs(dy * cosine))
            {
                dy = (dx * sine) / cosine;
            }
            else
            {
                dx = (dy * cosine) / sine;
            }

            // Return the point as a Vector2 object
            return new Vector2(dx, dy);
        }
        
        private Particle GenerateParticle(int order, int source, Burst burst)
        {
            float angle = 0;
            if (source == 0)//Burst
            {
                angle = order * (360f / burst.count) * _spreadLoop;
            }
            else if(source == 1)//Rate per Sec
            {
                angle = order * (360f / (_rate)) / _duration * _spreadLoop;
            }
            else if(source == 2)//Rate over Life
            {
                angle = order * (360f / (_rateOverLifetime)) * _spreadLoop;
            }
            else if(source == 3)//Rate over Distance
            {
                angle = order * (360f / (_rateOverDistance)) / _duration * _spreadLoop;
            }
            
            // Create new particle at system's starting position
            Vector2 p = Vector2.zero;
            switch (_shape)
            {
                case EmitterShape.Point:
                    p = _position;
                    break;
                case EmitterShape.Circle:
                    if (_emitOnSurface)
                    {
                        if (_spread == SpreadType.Random)
                        {
                            p = _position + (Random.insideUnitCircle * _radius);
                        }
                        else
                        {
                            p = (RotateOnAngle(new Vector3(0,Random.Range(0f,1f),0), angle) * _radius);
                        }
                    }
                    else
                    {
                        if (_spread == SpreadType.Random)
                        {
                            Vector2 r = Random.insideUnitCircle.normalized;
                            p = _position + Vector2.Lerp(r * _radius, r * (_radius - _emitterThickness), Random.value);
                        }
                        else
                        {
                            p = (RotateOnAngle(new Vector3(0,1f,0), angle) * (UnityEngine.Random.Range(_radius, _radius - _emitterThickness)));
                        }
                    }
                    break;
                case EmitterShape.Rectangle:
                    if (_emitOnSurface)
                    {
                        if(_spread == SpreadType.Uniform)
                        {
                            p = Vector2.Lerp(GetPointOnRect(angle*Mathf.Deg2Rad, _width, _height), Vector2.one, Random.value);
                        }
                        else
                        {
                            p = _position + new Vector2(Random.Range(-_width / 2, _width / 2),
                                Random.Range(-_height / 2, _height / 2));
                        }
                    }
                    else
                    {
                        float a = Random.Range(0f, 360f);
                        
                        if(_spread == SpreadType.Uniform)
                        {
                            a = angle;
                        }
                        
                        p = Vector2.Lerp(GetPointOnRect(a*Mathf.Deg2Rad, _width, _height), GetPointOnRect(a*Mathf.Deg2Rad, _width-_emitterThickness, _height-_emitterThickness), Random.value);
                    }
                    break;
                case EmitterShape.Line:
                    if(_spread == SpreadType.Uniform)
                    {
                        p = _position + new Vector2(Mathf.Repeat(angle, 361).Remap(0,360,-_length/2, _length/2), 0);
                    }
                    else
                    {
                        p = _position + new Vector2(Random.Range(-_length/2, _length/2), 0);
                    }
                    
                    break;
                case EmitterShape.Directional:
                    p = _position;
                    break;
            }

            if (space == Simulation.World)
            {
                p = Quaternion.Euler(transform.eulerAngles) * (p);
            }
            
            Vector2 v = Vector2.zero;
            switch (_shape)
            {
                case EmitterShape.Point:
                    if (_spread == SpreadType.Uniform)
                    {
                        v = RotateOnAngle(new Vector3(0,1f,0), angle) * _startSpeed.Evaluate(Random.value, Random.value);;
                    }
                    else
                    {
                        v = Random.insideUnitCircle.normalized * _startSpeed.Evaluate(Random.value, Random.value);
                    }
                    break;
                case EmitterShape.Circle:
                    v = p.normalized * _startSpeed.Evaluate(Random.value, Random.value);
                    break;
                case EmitterShape.Rectangle:
                    v = p.normalized * _startSpeed.Evaluate(Random.value, Random.value);
                    break;
                case EmitterShape.Line:
                    v = (space == Simulation.World ? transform.up : Vector3.up) * _startSpeed.Evaluate(Random.value, Random.value);
                    break;
                case EmitterShape.Directional:
                    float a = 0;
                    if (space == Simulation.World)
                    {
                        if (_spread == SpreadType.Uniform)
                        {
                            a = Mathf.Repeat(angle, 361).Remap(0,360, -_angle / 2, _angle / 2) - transform.eulerAngles.z;
                        }
                        else
                        {
                            a = Random.Range(-_angle / 2, _angle / 2) - transform.eulerAngles.z;
                        }
                    }
                    else
                    {
                        if (_spread == SpreadType.Uniform)
                        {
                            a = Mathf.Repeat(angle, 361).Remap(0, 360, -_angle / 2, _angle / 2);
                        }
                        else
                        {
                            a = Random.Range(-_angle/2, _angle/2);
                        }
                    }
                    v = RotateOnAngle(a) * _startSpeed.Evaluate(Random.value, Random.value);
                    break;
            }

            float sLerp = Random.value;
            
            Particle part = new Particle(
                this,
                p,
                _startRotation.separated ? new Vector3(_startRotation.xCurve.Evaluate(Random.value, Random.value), _startRotation.yCurve.Evaluate(Random.value, Random.value),_startRotation.zCurve.Evaluate(Random.value, Random.value)) :
                    new Vector3(0, 0,_startRotation.mainCurve.Evaluate(Random.value, Random.value)),
                v,
                _startColor.Evaluate(Random.value, Random.value),
                _startSize.separated ? new Vector3(_startSize.xCurve.Evaluate(Random.value, Random.value), _startSize.yCurve.Evaluate(Random.value, Random.value),_startSize.zCurve.Evaluate(Random.value, Random.value)) :
                    new Vector3(_startSize.mainCurve.Evaluate(sLerp, sLerp), _startSize.mainCurve.Evaluate(sLerp, sLerp),_startSize.mainCurve.Evaluate(sLerp, sLerp)),
                _lifetime.Evaluate(Random.value, Random.value));
            
            return part;
        }
        
        public Material material
        {
            get
            {
                return m_Material;
            }
            set
            {
                if (m_Material == value)
                {
                    return;
                }

                m_Material = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        [SerializeField]
        private Texture m_Texture;

        public Texture texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                if (m_Texture == value)
                {
                    return;
                }

                m_Texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (fitRect)
            {
                FitRect();
            }

            if (!_emitOnSurface)
            {
                switch (_shape)
                {
                    case EmitterShape.Circle:
                        _emitterThickness = Mathf.Clamp(_emitterThickness, 0f, _radius);
                        break;
                    case EmitterShape.Rectangle:
                        _emitterThickness = Mathf.Clamp(_emitterThickness, 0f, rectTransform.sizeDelta.x<rectTransform.sizeDelta.y?_width:_height);
                        break;
                    case EmitterShape.Line:
                        _emitterThickness = Mathf.Clamp(_emitterThickness, 0f, _radius);
                        break;
                }
            }
        }

        private void FitRect()
        {
            switch (_shape)
            {
                // If the emitter has a circle shape, set the radius to half of the smaller
                // of the width and height of the emitter's RectTransform.
                case EmitterShape.Circle:
                    if (rectTransform.rect.width > rectTransform.rect.height)
                    {
                        _radius = rectTransform.rect.height/2;
                    }
                    else
                    {
                        _radius = rectTransform.rect.width/2;
                    }
                    break;
        
                // If the emitter has a rectangle shape, set the width and height of the emitter
                // to the width and height of the RectTransform.
                case EmitterShape.Rectangle:
                    _width = rectTransform.rect.width;
                    _height = rectTransform.rect.height;
                    break;

                // If the emitter has a line shape, set the length of the emitter to the width
                // of the RectTransform.
                case EmitterShape.Line:
                    _length = rectTransform.rect.width;
                    break;
            }
        }

        public override Texture mainTexture
        {
            get
            {
                return m_Texture == null ? s_WhiteTexture : m_Texture;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Animate();
                _particles[i].Render(vh);
            }
        }

        void LateUpdate()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].TimeSinceBorn > _particles[i].Lifetime && _particles[i].trailPoints.Count <= 3)
                {
                    onParticleFinish.Invoke();
                    _particles.RemoveAt(i);
                    if (_firstParticleFinished == false)
                    {
                        _firstParticleFinished = true;
                        onFirstParticleFinish.Invoke();
                    }
                    if (particles.Count < 1)
                    {
                        onLastParticleFinish.Invoke();
                    }
                }
            }
        }
        
        private Vector3 RotateOnAngle(float angle){
            float rad = angle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin( rad ), Mathf.Cos( rad ), 0);
            return position * 1f;
        }
        
        private Vector3 RotateOnAngle(Vector3 p, float angle){
            return Quaternion.Euler(new Vector3(0,0,angle)) * p;
        }
        
        /// <summary>
        /// Converts world position to viewport position using the current camera
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns>Viewport position</returns>
        public Vector3 WorldToViewportPoint(Vector3 position)
        {
            Vector3 pos = camera.WorldToViewportPoint(position);
            return pos;
        }
    }
    
    [Serializable]
    public class Burst
    {
        public float time = 0;
        public int count = 1;
        public bool used = false;
        
        public Burst(float time, int count)
        {
            this.time = time;
            this.count = count;
        }
    }

    [Serializable]
    public struct SpeedRange
    {
        public float from;
        public float to;
        public SpeedRange(float from, float to)
        {
            this.from = from;
            this.to = to;
        }
    }

    [Serializable]
    public struct Module
    {
        public bool enabled;

        public Module(bool enabled)
        {
            this.enabled = enabled;
        }
    }

    [Serializable]
    public struct SeparatedMinMaxCurve
    {
        [SerializeField]
        private bool separable;
        public bool separated;
        public ParticleSystem.MinMaxCurve mainCurve;
        public ParticleSystem.MinMaxCurve xCurve;
        public ParticleSystem.MinMaxCurve yCurve;
        public ParticleSystem.MinMaxCurve zCurve;

        public SeparatedMinMaxCurve(float startValue, bool separated = false, bool separable = true)
        {
            mainCurve = new ParticleSystem.MinMaxCurve(startValue);
            xCurve = new ParticleSystem.MinMaxCurve(startValue);
            yCurve = new ParticleSystem.MinMaxCurve(startValue);
            zCurve = new ParticleSystem.MinMaxCurve(startValue);
            this.separated = separated;
            this.separable = separable;
        }
        
        public SeparatedMinMaxCurve(AnimationCurve startValue, bool separated = false, bool separable = true)
        {
            mainCurve = new ParticleSystem.MinMaxCurve(1f,new AnimationCurve(startValue.keys));
            xCurve = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(startValue.keys));
            yCurve = new ParticleSystem.MinMaxCurve(1f,new AnimationCurve(startValue.keys));
            zCurve = new ParticleSystem.MinMaxCurve(1f,new AnimationCurve(startValue.keys));
            this.separated = separated;
            this.separable = separable;
        }
    }
    
    public static class Extensions {
        public static float Remap (this float value, float from1, float to1, float from2, float to2) {
            float v = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            if(float.IsNaN(v) || float.IsInfinity(v))
                return 0;
            return v;
        }
    }
}

namespace AssetKits.ParticleImage.Enumerations
{
    public enum EmitterShape
    {
        Point, Circle, Rectangle, Line, Directional
    }

    public enum SpreadType
    {
        Random, Uniform
    }

    public enum Simulation
    {
        Local, World
    }

    public enum AttractorType
    {
        Pivot, Surface
    }

    public enum PlayMode
    {
        None, OnEnable, OnAwake
    }
    
    public enum SheetType
    {
        Lifetime, Speed, FPS
    }

    public enum TimeScale
    {
        Unscaled, Normal
    }
}
