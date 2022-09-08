using UnityEngine;
using System.Collections.Generic;

namespace HeneGames.Airplane
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleAirPlaneController : MonoBehaviour
    {
        #region Private variables

        private List<SimpleAirPlaneCollider> airPlaneColliders = new List<SimpleAirPlaneCollider>();

        private float maxSpeed = 0.6f;
        private float currentYawSpeed;
        private float currentPitchSpeed;
        private float currentRollSpeed;
        private float currentSpeed;
        private float currentEngineLightIntensity;
        private float currentEngineSoundPitch;
        private bool planeIsDead;
        private Rigidbody rb;

        #endregion

        [Header("Wing trail effects")]
        [Range(0.01f, 1f)]
        [SerializeField] private float trailThickness = 0.045f;
        [SerializeField] private TrailRenderer[] wingTrailEffects;

        [Header("Rotating speeds")]
        [Range(5f, 500f)]
        [SerializeField] private float yawSpeed = 50f;

        [Range(5f, 500f)]
        [SerializeField] private float pitchSpeed = 100f;

        [Range(5f, 500f)]
        [SerializeField] private float rollSpeed = 200f;

        [Header("Rotating speeds multiplers when turbo is used")]
        [Range(0.1f, 5f)]
        [SerializeField] private float yawTurboMultiplier = 0.3f;

        [Range(0.1f, 5f)]
        [SerializeField] private float pitchTurboMultiplier = 0.5f;

        [Range(0.1f, 5f)]
        [SerializeField] private float rollTurboMultiplier = 1f;

        [Header("Moving speed")]
        [Range(5f, 100f)]
        [SerializeField] private float defaultSpeed = 10f;

        [Range(10f, 200f)]
        [SerializeField] private float turboSpeed = 20f;

        [Range(0.1f, 50f)]
        [SerializeField] private float accelerating = 10f;

        [Range(0.1f, 50f)]
        [SerializeField] private float deaccelerating = 5f;

        [Header("Engine sound settings")]
        [SerializeField] private AudioSource engineSoundSource;

        [SerializeField] private float defaultSoundPitch = 1f;

        [SerializeField] private float turboSoundPitch = 1.5f;

        [Header("Engine propellers settings")]
        [Range(10f, 10000f)]
        [SerializeField] private float propelSpeedMultiplier = 100f;

        [SerializeField] private GameObject[] propellers;

        [Header("Turbine light settings")]
        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightDefault = 1f;

        [Range(0.1f, 20f)]
        [SerializeField] private float turbineLightTurbo = 5f;

        [SerializeField] private Light[] turbineLights;

        [Header("Colliders")]
        [SerializeField] private Transform crashCollidersRoot;

        private void Start()
        {
            //Setup speeds
            maxSpeed = defaultSpeed;
            currentSpeed = defaultSpeed;

            //Get and set rigidbody
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            SetupColliders(crashCollidersRoot);
        }

        private void Update()
        {
            AudioSystem();

            //Airplane move only if not dead
            if (!planeIsDead)
            {
                Movement();

                //Rotate propellers if any
                if (propellers.Length > 0)
                {
                    RotatePropellers(propellers);
                }
            }
            else
            {
                ChangeWingTrailEffectThickness(0f);
            }

            //Control lights if any
            if (turbineLights.Length > 0)
            {
                ControlEngineLights(turbineLights, currentEngineLightIntensity);
            }

            //Crash
            if (!planeIsDead && HitSometing())
            {
                Crash();
            }
        }

        #region Movement

        private void Movement()
        {
            //Move forward
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            //Rotate airplane by inputs
            transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * currentRollSpeed * Time.deltaTime);
            transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * currentPitchSpeed * Time.deltaTime);

            //Rotate yaw
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(Vector3.up * currentYawSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(-Vector3.up * currentYawSpeed * Time.deltaTime);
            }

            //Accelerate and deacclerate
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerating * Time.deltaTime;
            }
            else
            {
                currentSpeed -= deaccelerating * Time.deltaTime;
            }

            //Turbo
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //Set speed to turbo speed and rotation to turbo values
                maxSpeed = turboSpeed;

                currentYawSpeed = yawSpeed * yawTurboMultiplier;
                currentPitchSpeed = pitchSpeed * pitchTurboMultiplier;
                currentRollSpeed = rollSpeed * rollTurboMultiplier;

                //Engine lights
                currentEngineLightIntensity = turbineLightTurbo;

                //Effects
                ChangeWingTrailEffectThickness(trailThickness);

                //Audio
                currentEngineSoundPitch = turboSoundPitch;
            }
            else
            {
                //Speed and rotation normal
                maxSpeed = defaultSpeed;

                currentYawSpeed = yawSpeed;
                currentPitchSpeed = pitchSpeed;
                currentRollSpeed = rollSpeed;

                //Engine lights
                currentEngineLightIntensity = turbineLightDefault;

                //Effects
                ChangeWingTrailEffectThickness(0f);

                //Audio
                currentEngineSoundPitch = defaultSoundPitch;
            }
        }

        #endregion

        #region Audio
        private void AudioSystem()
        {
            engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, currentEngineSoundPitch, 10f * Time.deltaTime);

            if (planeIsDead)
            {
                engineSoundSource.volume = Mathf.Lerp(engineSoundSource.volume, 0f, 0.1f);
            }
        }

        #endregion

        #region Private methods

        private void SetupColliders(Transform _root)
        {
            //Get colliders from root transform
            Collider[] colliders = _root.GetComponentsInChildren<Collider>();

            //If there are colliders put components in them
            for (int i = 0; i < colliders.Length; i++)
            {
                //Change collider to trigger
                colliders[i].isTrigger = true;

                GameObject _currentObject = colliders[i].gameObject;

                //Add airplane collider to it and put it on the list
                SimpleAirPlaneCollider _airplaneCollider = _currentObject.AddComponent<SimpleAirPlaneCollider>();
                airPlaneColliders.Add(_airplaneCollider);

                //Add rigid body to it
                Rigidbody _rb = _currentObject.AddComponent<Rigidbody>();
                _rb.useGravity = false;
                _rb.isKinematic = true;
                _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }

        private void RotatePropellers(GameObject[] _rotateThese)
        {
            float _propelSpeed = currentSpeed * propelSpeedMultiplier;

            for (int i = 0; i < _rotateThese.Length; i++)
            {
                _rotateThese[i].transform.Rotate(Vector3.forward * -_propelSpeed * Time.deltaTime);
            }
        }

        private void ControlEngineLights(Light[] _lights, float _intensity)
        {
            float _propelSpeed = currentSpeed * propelSpeedMultiplier;

            for (int i = 0; i < _lights.Length; i++)
            {
                if(!planeIsDead)
                {
                    _lights[i].intensity = Mathf.Lerp(_lights[i].intensity, _intensity, 10f * Time.deltaTime);
                }
                else
                {
                    _lights[i].intensity = Mathf.Lerp(_lights[i].intensity, 0f, 10f * Time.deltaTime);
                }
               
            }
        }

        private void ChangeWingTrailEffectThickness(float _thickness)
        {
            for (int i = 0; i < wingTrailEffects.Length; i++)
            {
                wingTrailEffects[i].startWidth = Mathf.Lerp(wingTrailEffects[i].startWidth, _thickness, Time.deltaTime * 10f);
            }
        }

        private bool HitSometing()
        {
            for (int i = 0; i < airPlaneColliders.Count; i++)
            {
                if (airPlaneColliders[i].collideSometing)
                {
                    return true;
                }
            }

            return false;
        }

        private void Crash()
        {
            //Set rigidbody to non cinematic
            rb.isKinematic = false;
            rb.useGravity = true;

            //Change every collider trigger state and remove rigidbodys
            for (int i = 0; i < airPlaneColliders.Count; i++)
            {
                airPlaneColliders[i].GetComponent<Collider>().isTrigger = false;
                Destroy(airPlaneColliders[i].GetComponent<Rigidbody>());
            }

            //Kill player
            planeIsDead = true;

            //Here you can add your own code...
        }

        #endregion

        #region Variables

        /// <summary>
        /// Returns a percentage of how fast the current speed is from the maximum speed between 0 and 1
        /// </summary>
        /// <returns></returns>
        public float PercentToMaxSpeed()
        {
            float _percentToMax = currentSpeed / turboSpeed;

            return _percentToMax;
        }

        public bool PlaneIsDead()
        {
            return planeIsDead;
        }

        public bool UsingTurbo()
        {
            if(maxSpeed == turboSpeed)
            {
                return true;
            }

            return false;
        }

        public float CurrentSpeed()
        {
            return currentSpeed;
        }

        #endregion
    }
}