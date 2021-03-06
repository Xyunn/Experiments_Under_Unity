﻿using UnityEditor;
using UnityEngine;

namespace _05_CurvesAndSplines
{
	[CustomEditor(typeof(BezierCurve))]
	public class BezierCurveInspector : Editor {

		private BezierCurve curve;
		private Transform handleTransform;
		private Quaternion handleRotation;
		
		private const int _lineStep = 10;
		private const float _directionScale = 0.5f;

		private void OnSceneGUI()
		{
			curve = target as BezierCurve;
			handleTransform = curve.transform;
			handleRotation = handleTransform.rotation;

			Vector3 p0 = ShowPoint(0);
			Vector3 p1 = ShowPoint(1);
			Vector3 p2 = ShowPoint(2);
			Vector3 p3 = ShowPoint(3);

			Handles.color = Color.gray;
			Handles.DrawLine(p0, p1);
			Handles.DrawLine(p2, p3);

			/*Vector3 lineStart = curve.GetPoint(0f);
			Handles.color = Color.green;
			Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));
			for (int i=1; i <= _lineStep; i++)
			{
				Vector3 lineEnd = curve.GetPoint(i / (float)_lineStep);
				Handles.color = Color.white;
				Handles.DrawLine(lineStart, lineEnd);

				Handles.color = Color.green;
				Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection(i / (float) _lineStep));
				lineStart = lineEnd;
			}*/

			ShowDirections();
			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
		}

		///Display the directions of each segment for the bezier curve
		private void ShowDirections()
		{
			Handles.color = Color.green;
			Vector3 point = curve.GetPoint(0f);
			Handles.DrawLine(point, point + curve.GetDirection(0f) * _directionScale);
			for (int i=1; i <= _lineStep; i++)
			{
				point = curve.GetPoint(i / (float) _lineStep);
				Handles.DrawLine(point, point + curve.GetDirection(i / (float) _directionScale));
			}
		}

		private Vector3 ShowPoint(int index)
		{
			if (index > curve.points.Length)
			{
				return Vector3.zero;
			}

			Vector3 point = handleTransform.TransformPoint(curve.points[index]);
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, handleRotation);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(curve, "Move Point");
				EditorUtility.SetDirty(curve);
				curve.points[index] = handleTransform.InverseTransformPoint(point);
			}
			return point;
		}
	}
}