﻿using UnityEditor;
using UnityEngine;

namespace SmartData.Graph
{
	public class SmartGraphWindow : EditorWindow
	{
		[SerializeField]
		private SmartGraph _graph;
		[SerializeField]
		private SmartGraphGUI _graphGUI;

		private const float kZoomMin = 0.1f;
		private const float kZoomMax = 1.0f;

		private Rect _zoomArea = new Rect(0.0f, 75.0f, 600.0f, 300.0f - 100.0f);
		private float _zoom = 0.5f;
		private Vector2 _zoomCoordsOrigin = Vector2.zero;

		private const float kBarHeight = 17;

		[MenuItem("Window/Smart Graph editor")]
		static void ShowEditor()
		{
			SmartGraphWindow editor = EditorWindow.GetWindow<SmartGraphWindow>();
			editor.hideFlags = HideFlags.HideAndDontSave;
			editor.Initialize();
		}

		private void OnEnable()
		{
			Editors.SmartDataRegistry.graphIsOpen = true;
		}

		private void OnDisable()
		{
			Editors.SmartDataRegistry.graphIsOpen = false;
		}

		public void Initialize()
		{
			_graph = SmartGraph.Create();
			_graph.RebuildGraph();

			_graphGUI = _graph.GetEditor();
			_graphGUI.CenterGraph();

			EditorUtility.SetDirty(_graphGUI);
			EditorUtility.SetDirty(_graph);
		}

		void OnGUI()
		{
			var width = position.width;
			var height = position.height;
			_zoomArea = new Rect(0, 0, width, height);
			HandleEvents();

			if (_graphGUI != null)
			{
				Rect r = EditorZoomArea.Begin(_zoom, _zoomArea);
				// Main graph area
				_graphGUI.BeginGraphGUI(this, r);
				_graphGUI.OnGraphGUI();
				_graphGUI.EndGraphGUI();

				// Clear selection on background click
				var e = Event.current;
				if (e.type == EventType.MouseDown && e.clickCount == 1)
					_graphGUI.ClearSelection();


				EditorZoomArea.End();
			}


			// Status bar
			GUILayout.BeginArea(new Rect(0, 0, width, kBarHeight + 5));
			string[] toolbarStrings = new string[] { "Update connections", "Clear" };
			int result = GUILayout.Toolbar(-1, toolbarStrings);
			if (result == 0)
			{
				RefreshGraphConnections();
			}
			else if (result == 1)
			{
				RebuildGraph();
			}
			GUILayout.EndArea();

		}

		private void Update()
		{
			if (EdgeTriggersTracker.HasData())
			{
				Repaint();
			}
		}

		public void OverrideSelection(int overrideIndex)
		{
			if (_graphGUI == null){
				Initialize();
			}
			_graphGUI.SelectionOverride = overrideIndex;
		}

		public Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
		{
			return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
		}


		private void HandleEvents()
		{
			if (Event.current.type == EventType.ScrollWheel)
			{
				Vector2 screenCoordsMousePos = Event.current.mousePosition;
				Vector2 delta = Event.current.delta;
				Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
				float zoomDelta = -delta.y / 150.0f;
				float oldZoom = _zoom;
				_zoom += zoomDelta;
				_zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
				_zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

				Event.current.Use();
			}
		}

		void RebuildGraph()
		{
			if (_graph != null)
			{
				_graph.RebuildGraph();
			}
		}
		void RefreshGraphConnections()
		{
			if (_graph != null)
			{
				_graph.RefreshGraphConnections();
			}
		}
	}
}


public static class RectExtensions
{
	public static Vector2 TopLeft(this Rect rect)
	{
		return new Vector2(rect.xMin, rect.yMin);
	}
	public static Rect ScaleSizeBy(this Rect rect, float scale)
	{
		return rect.ScaleSizeBy(scale, rect.center);
	}
	public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
	{
		Rect result = rect;
		result.x -= pivotPoint.x;
		result.y -= pivotPoint.y;
		result.xMin *= scale;
		result.xMax *= scale;
		result.yMin *= scale;
		result.yMax *= scale;
		result.x += pivotPoint.x;
		result.y += pivotPoint.y;
		return result;
	}
	public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
	{
		return rect.ScaleSizeBy(scale, rect.center);
	}
	public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
	{
		Rect result = rect;
		result.x -= pivotPoint.x;
		result.y -= pivotPoint.y;
		result.xMin *= scale.x;
		result.xMax *= scale.x;
		result.yMin *= scale.y;
		result.yMax *= scale.y;
		result.x += pivotPoint.x;
		result.y += pivotPoint.y;
		return result;
	}
}

public class EditorZoomArea
{
	private static Matrix4x4 _prevGuiMatrix;

	public static Rect Begin(float zoomScale, Rect screenCoordsArea)
	{
		GUI.EndGroup();

		Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
		GUI.BeginGroup(clippedArea);

		_prevGuiMatrix = GUI.matrix;
		Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
		Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
		GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

		return clippedArea;
	}

	public static void End()
	{
		GUI.matrix = _prevGuiMatrix;
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(0.0f, 0.0f, Screen.width, Screen.height));
	}
}