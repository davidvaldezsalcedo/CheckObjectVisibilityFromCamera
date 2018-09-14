using UnityEngine;

namespace CamV
{
    public class CameraBehaviour : MonoBehaviour 
    {
        [SerializeField]
        private FloatVariable _YOffset;
        [SerializeField]
        private FloatVariable _ZOffset;
        [SerializeField]
        private float _AltZOffset = 1;
        [SerializeField]
        private float _FollowSpeed = 0.5f;
        [SerializeField]
        private float _SpeedToRotate = 0.5f;
        [SerializeField]
        private TransformVariable _PlayerTransform;
        [SerializeField]
        private TransformVariable _CamTransform;
        [SerializeField]
        private BoolVariable _CanSeePlayer;

        [SerializeField]
        private Renderer _TargetRenderer;

        private void Awake()
        {
            if(_CamTransform != null)
            {
                _CamTransform.Value = gameObject.transform;
            }
        }

        private void LateUpdate()
        {
            MoveToPlayer();
            RotateBasedOnPlayer();
        }

        private void MoveToPlayer()
        {
            Vector3 currentPos = _CamTransform.Value.transform.position;
            Vector3 targetPos = new Vector3(_PlayerTransform.Value.position.x, _PlayerTransform.Value.position.y + _YOffset.Value, _PlayerTransform.Value.position.z + ChooseOffsetBasedOnPlayer());
            Vector3 newPos = Vector3.Lerp(currentPos, targetPos, _FollowSpeed * Time.deltaTime);
            _CamTransform.Value.transform.position = newPos;
        }

        private float ChooseOffsetBasedOnPlayer()
        {
            return  _CanSeePlayer.Value ? _ZOffset.Value : _AltZOffset;
        }

        private void RotateBasedOnPlayer()
        {
            Quaternion posToLookAt = Quaternion.LookRotation(_PlayerTransform.Value.position - _CamTransform.Value.transform.position, Vector3.up);
            Quaternion targetRotation = Quaternion.Slerp(_CamTransform.Value.transform.rotation, posToLookAt, _SpeedToRotate * Time.deltaTime);

            _CamTransform.Value.transform.rotation = targetRotation;
        }
    }
}
