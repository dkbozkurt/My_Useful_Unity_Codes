using UnityEngine;

namespace LaserReflection3D.Scripts
{
	[RequireComponent(typeof(LineRenderer))]
	public class LaserReflectionBehaviour : MonoBehaviour
	{
		[SerializeField] private LayerMask _reflectionLayerMask;
		[SerializeField] private LayerMask _targetLayer;
		[SerializeField] private float _laserLength = 50;
		[SerializeField] private int _numOfReflectionLimit = 2;

		private LineRenderer _lineRenderer;
		private Ray _ray;
		private RaycastHit _hit;
		private Vector3 _direction;

		private bool _canLaserRun = true;
		
		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
		}
		private void Update()
		{
			if(!_canLaserRun) return;
			
			ReflectLaser();
		}

		private void ReflectLaser()
		{
			_ray = new Ray(transform.position, transform.forward);

			_lineRenderer.positionCount = 1;
			_lineRenderer.SetPosition(0,transform.position);

			float remainLength = _laserLength;

			for (int i = 0; i < _numOfReflectionLimit; i++)
			{
				if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainLength, _targetLayer))
				{
					_lineRenderer.positionCount += 1;
					_lineRenderer.SetPosition(_lineRenderer.positionCount-1,_hit.point);
				}
				else if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainLength, _reflectionLayerMask))
				{
					_lineRenderer.positionCount += 1;
					_lineRenderer.SetPosition(_lineRenderer.positionCount-1,_hit.point);

					remainLength -= Vector3.Distance(_ray.origin, _hit.point);

					_ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
				}
				else
				{
					_lineRenderer.positionCount += 1;
					_lineRenderer.SetPosition(_lineRenderer.positionCount-1,_ray.origin + (_ray.direction * remainLength));
				}
			}
		}

		private void NormalLaser()
		{
			_lineRenderer.SetPosition(0,transform.position);

			if (Physics.Raycast(transform.position, transform.forward, out _hit, _laserLength, _reflectionLayerMask))
			{
				_lineRenderer.SetPosition(1,_hit.point);
			}
			else
			{
				_lineRenderer.SetPosition(1,transform.position +(transform.forward * _laserLength));
			}
		}

		public void PlayLaser()
		{
			_canLaserRun = true;
		}
		
		public void StopLaser()
		{
			_canLaserRun = false;
		}
	}
}
