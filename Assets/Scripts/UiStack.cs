using System.Collections.Generic;
using UnityEngine;

namespace Game.Base {
	public class UiStack : MonoBehaviour {
		private LinkedList<UiModel> _modelList = new LinkedList<UiModel>();

		[SerializeField, InspectorLabel("根节点")]
		private GameObject _root = null;

		private void Awake() {
			if (!_root) {
				_root = gameObject;
			}
		}

		public TModel PushUI<TModel>() where TModel : UiModel {
			if (_modelList.Count > 0) {
				_modelList.Last.Value.OnTopLostUI();
			}
			var modelObj = new GameObject("Model");
			modelObj.transform.SetParent(_root.transform);
			var model = modelObj.AddComponent<TModel>();
			model.__InitUiStack(this);
			model.OnInitUI();

			_modelList.AddLast(model);
			model.OnTopUI();
			return model;
		}

		public void PopUI() {
			if (_modelList.Last != null) {
				var model = _modelList.Last.Value;
				_modelList.RemoveLast();
				_InvokeRemove(model, true);
			}
		}

		public void PopUI(UiModel model) {
			if (model) {
				if (GetTopUI() == model) {
					_modelList.RemoveLast();
					_InvokeRemove(model, true);
				} else {
					var node = _modelList.Find(model);
					if (node != null) {
						_modelList.Remove(node);
						_InvokeRemove(model, false);
					}
				}
			}
		}

		public UiModel GetTopUI() {
			if (_modelList.Last != null) {
				return _modelList.Last.Value;
			}
			return null;
		}

		public void _InvokeRemove(UiModel model, bool bTop) {
			if (bTop) {
				var topUI = GetTopUI();
				if (topUI) {
					topUI.OnTopUI();
					topUI.OnTopBackUI();
				}
			}

			model.__SetHideListener(() => {
				model.OnDestroyUI();
				Destroy(model.gameObject);
			});
			model.HideUI();
		}

	};
}