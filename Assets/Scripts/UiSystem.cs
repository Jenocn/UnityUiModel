using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(RectTransform))]
public sealed class UiSystem : MonoBehaviour {

	private static UiSystem _instance = null;

	private RectTransform _normalLayer = null;
	private RectTransform _topLayer = null;

	private Canvas _canvas = null;
	private CanvasScaler _scaler = null;
	private CanvasGroup _group = null;

	private LinkedList<UiModel> _uiStack = new LinkedList<UiModel>();

	private void Awake() {

		_instance = this;

		var normal = new GameObject("UiNormalLayer");
		var top = new GameObject("UiTopLayer");
		normal.transform.SetParent(transform);
		top.transform.SetParent(transform);

		_normalLayer = normal.AddComponent<RectTransform>();
		_topLayer = top.AddComponent<RectTransform>();
		_normalLayer.localScale = Vector3.one;
		_topLayer.localScale = Vector3.one;
		_normalLayer.localPosition = Vector3.zero;
		_topLayer.localPosition = Vector3.zero;

		_canvas = GetComponent<Canvas>();
		_scaler = GetComponent<CanvasScaler>();
		_group = GetComponent<CanvasGroup>();
	}

	private void OnDestroy() {
		_instance = null;
		_Clear();
	}

	public static void Push(GameObject prefab) {
		if (_instance) {
			_instance._Push(prefab);
		}
	}

	public static void Pop() {
		if (_instance) {
			_instance._Pop();
		}
	}

	public static void Clear() {
		if (_instance) {
			_instance._Clear();
		}
	}

	private void _Push(GameObject prefab) {
		if (!prefab) {
			Debug.LogError("The prefab is null!");
			return;
		}
		if (!prefab.GetComponent<UiModel>()) {
			Debug.LogError("It's not a UiModel object!");
			return;
		}

		var last = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
		if (last) {
			last.OnPause();
		}

		var model = Instantiate(prefab, _CreateVest(prefab.name, _normalLayer)).GetComponent<UiModel>();
		_uiStack.AddLast(model);

		model.OnOpen();
		model.OnOpenAction();
	}

	private void _Pop() {
		var model = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
		if (model) {
			_uiStack.RemoveLast();
			var delay = model.OnCloseAction();
			model.OnClose();
			Destroy(model.transform.parent.gameObject, delay);
			var last = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
			if (last) {
				last.OnResume();
			}
		}
	}

	private void _Clear() {
		while (_uiStack.Last != null) {
			_Pop();
		}
	}

	private RectTransform _CreateVest(string name, Transform parent) {
		var vest = new GameObject(name);
		var rt = vest.AddComponent<RectTransform>();
		rt.SetParent(parent);
		rt.localScale = Vector3.one;
		rt.localPosition = Vector3.zero;
		return rt;
	}
}
