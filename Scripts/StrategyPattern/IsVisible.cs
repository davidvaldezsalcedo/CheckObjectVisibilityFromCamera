using UnityEngine;
using System.Collections;

namespace CamV
{
	public class IsVisible : MonoBehaviour
	{
		[SerializeField]
		private TransformVariable _CamTransform;
		[SerializeField]
		private FloatVariable _ZOffset;
		[SerializeField]
		private FloatVariable _YOffset;
		[SerializeField]
		private float _CheckPlayerVisibityInterval = 0.5f;
		[SerializeField]
		private BoolVariable _CamCanSeePlayer;
		[SerializeField]
		private string _ObjectTagToCheck = "Player";
		[SerializeField]
		private Vector3[] _BoundingPoints = new Vector3[8];
		[SerializeField]
		private Vector3 _CamOrigin;

		
		private Renderer _Rend;
		private Collider _Col;



		private void Awake()
		{
			_Rend = GetComponent<Renderer>();
			_Col = GetComponent<Collider>();

			InvokeRepeating("CheckVisibility", 0, _CheckPlayerVisibityInterval);
		}

		private void GetCamOrigin()
		{
			_CamOrigin = new Vector3(transform.position.x, transform.position.y + _YOffset.Value, transform.position.z + _ZOffset.Value);
		}

		private void CheckVisibility()
		{
			GetCamOrigin();
			_CamCanSeePlayer.Value = CheckOccludedVisibility();
		}

		private Vector3[] GetBoundPoints()
		{
			_BoundingPoints[0] = _Col.bounds.min;
			_BoundingPoints[1] = _Col.bounds.max;
			_BoundingPoints[2] = new Vector3(_BoundingPoints[0].x, _BoundingPoints[0].y, _BoundingPoints[1].z);
			_BoundingPoints[3] = new Vector3(_BoundingPoints[0].x, _BoundingPoints[1].y, _BoundingPoints[0].z);
			_BoundingPoints[4] = new Vector3(_BoundingPoints[1].x, _BoundingPoints[0].y, _BoundingPoints[0].z);
			_BoundingPoints[5] = new Vector3(_BoundingPoints[0].x, _BoundingPoints[1].y, _BoundingPoints[1].z);
			_BoundingPoints[6] = new Vector3(_BoundingPoints[1].x, _BoundingPoints[0].y, _BoundingPoints[1].z);
			_BoundingPoints[7] = new Vector3(_BoundingPoints[1].x, _BoundingPoints[1].y, _BoundingPoints[0].z);

			return _BoundingPoints;
		}

		private bool CheckOccludedVisibility()
		{
			if(_Rend.isVisible)
			{
				Vector3[] points = GetBoundPoints();
				
				for (int i = 0; i < points.Length; ++i)
				{
					Vector3 dir = points[i] - _CamOrigin; 
					RaycastHit hit;
					if(Physics.Raycast(_CamOrigin, dir, out hit))
					{
						if(hit.transform.CompareTag(_ObjectTagToCheck))
						return true;					
					}
				}
				return false;
			}
			else
			{
				return false;
			}
		}

	}
}