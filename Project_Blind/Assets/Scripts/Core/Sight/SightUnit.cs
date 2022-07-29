using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

namespace Blind
{
    public class SightUnit : MonoBehaviour
    {
        
        public int Range = 3;
        // 시야 영역의 반지름과 시야 각도
        public float viewRadius = 10;
        [Range(0, 360)]
        public float viewAngle = 360;
        // 마스크 2종
        public LayerMask targetMask, obstacleMask;
        // Target mask에 ray hit된 transform을 보관하는 리스트
        public List<Transform> visibleTargets = new List<Transform>();
        
        public float meshResolution = 0.1f;
        
        public Mesh viewMesh;
        public MeshFilter viewMeshFilter;
        
        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;
            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }
        private void Awake()
        {
            if (SightController.Instance is not null)
                SightController.Instance.AssignUnit(this);
        }
        void Start()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
            // 0.2초 간격으로 코루틴 호출
            StartCoroutine(FindTargetsWithDelay(0.2f));
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        void FindVisibleTargets()
        {
            visibleTargets.Clear();
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius,targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                
                // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                    
                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
        ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            var ray = new Ray(transform.position, dir);
            var hit = Physics2D.Raycast(transform.position,dir,viewRadius,obstacleMask);
            if (hit)
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                var point = transform.position + dir * viewRadius;
                return new ViewCastInfo(false, point , viewRadius, globalAngle);
            }
        }
        void DrawFieldOfView()
        {
            Profiler.BeginSample("DrawFieldOfView");
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector2> viewPoints = new List<Vector2>();
            ViewCastInfo prevViewCast = new ViewCastInfo();
            Profiler.BeginSample("raycast");
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

                ViewCastInfo newViewCast = ViewCast(angle);   
                viewPoints.Add(newViewCast.point);
            }
            Profiler.EndSample();
            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];
            vertices[0] = Vector3.zero;
            
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
            Profiler.EndSample();
        }
        // y축 오일러 각을 3차원 방향 벡터로 변환한다.
        // 원본과 구현이 살짝 다름에 주의. 결과는 같다.
        public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad),0);
        }
        
    }
}