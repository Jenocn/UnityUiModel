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

	private RectTransform _rectTransform = null;
	private RectTransform _normalLayer = null;
	private RectTransform _topLayer = null;

	private LinkedList<UiModel> _uiStack = new LinkedList<UiModel>();

	private void Awake() {

		_instance = this;

		_rectTransform = GetComponent<RectTransform>();

		_normalLayer = _CreateChild("UiNormalLayer", _rectTransform);
		_topLayer = _CreateChild("UiTopLayer", _rectTransform);

		_InitTransform(ref _normalLayer);
		_InitTransform(ref _topLayer);
	}

	private void OnDestroy() {
		_instance = null;
		_Clear();
	}

	public static void Push(GameObject prefab, bool top = false) {
		if (_instance) {
			_instance._Push(prefab, top);
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

	private void _Push(GameObject prefab, bool top = false) {
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

		var model = _InstantiateModel(prefab, top);
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
			Destroy(model.gameObject, delay);
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

	private RectTransform _CreateChild(string name, RectTransform parent) {
		var obj = new GameObject(name);
		var ret = obj.AddComponent<RectTransform>();
		ret.SetParent(parent);
		_InitTransform(ref ret);
		return ret;
	}

	private UiModel _InstantiateModel(GameObject prefab, bool top) {
		var parent = _normalLayer;
		if (top) {
			parent = _topLayer;
		}
		var modelObj = Instantiate(prefab, parent);
		modelObj.name = prefab.name;
		var modelTransform = modelObj.GetComponent<RectTransform>();
		_InitTransform(ref modelTransform);
		var model = modelObj.GetComponent<UiModel>();
		return model;
	}

	private void _InitTransform(ref RectTransform target) {
		target.localScale = Vector3.one;
		target.localPosition = Vector3.zero;
	}
}
