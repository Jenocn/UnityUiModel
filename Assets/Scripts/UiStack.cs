﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(RectTransform))]
public sealed class UiStack : MonoBehaviour {

	private RectTransform _rectTransform = null;

	private LinkedList<UiModel> _uiStack = new LinkedList<UiModel>();

	private void Awake() {
		_rectTransform = GetComponent<RectTransform>();
	}

	private void OnDestroy() {
		Clear();
	}

	public UiModel Push(GameObject prefab) {
		if (!prefab) {
			Debug.LogError("The prefab is null!");
			return null;
		}
		if (!prefab.GetComponent<UiModel>()) {
			Debug.LogError("It's not a UiModel object!");
			return null;
		}

		var last = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
		if (last) {
			last.OnPause();
		}

		var model = _InstantiateModel(prefab);
		_uiStack.AddLast(model);
		model._BindUiStack(this);
		return model;
	}

	public T Push<T>(GameObject prefab) where T : UiModel {
		return Push(prefab) as T;
	}

	public void Pop() {
		var model = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
		if (model) {
			_uiStack.RemoveLast();
			model._BindUiStack(null);
			var delay = model.OnCloseAction();
			model.OnClose();
			Destroy(model.gameObject, delay);
			var last = (_uiStack.Last != null) ? _uiStack.Last.Value : null;
			if (last) {
				last.OnResume();
			}
		}
	}

	public void Clear() {
		while (_uiStack.Last != null) {
			Pop();
		}
	}

	private UiModel _InstantiateModel(GameObject prefab) {
		var modelObj = Instantiate(prefab, _rectTransform);
		modelObj.name = prefab.name;
		var modelTransform = modelObj.GetComponent<RectTransform>();
		modelTransform.localScale = Vector3.one;
		modelTransform.localPosition = Vector3.zero;
		var model = modelObj.GetComponent<UiModel>();
		return model;
	}
}