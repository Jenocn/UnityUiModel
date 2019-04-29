using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(ObjectBinding))]
public class UiModel : MonoBehaviour {

	private ObjectBinding _objectBinding = null;

	//=================== methods ====================
	public GameObject Query(string key) {
		return _GetObjectBinding().Query(key);
	}
	public Transform QueryTransform(string key) {
		return QueryCommponent<Transform>(key);
	}
	public T QueryCommponent<T>(string key) where T : Component {
		return _GetObjectBinding().QueryComponent<T>(key);
	}

	/**
	 * =================== life circle ===================
	 * OnOpen			↓ [self 'Push']
	 * OnOpenAction		↓ [after 'OnOpen']
	 * OnPause			↓ [if new UiModel 'Push' on top]
	 * OnResume			↓ [if top UiModel 'Pop' and self on top]
	 * OnCloseAction	↓ [self 'Pop']
	 * OnClose			↓ [after 'OnCloseAction']
	 */

	public virtual void OnOpen() { }
	public virtual void OnClose() { }

	public virtual void OnOpenAction() { }
	public virtual float OnCloseAction() { return 0; }

	public virtual void OnPause() { }
	public virtual void OnResume() { }

	// ====================== end =====================
	private ObjectBinding _GetObjectBinding() {
		if (!_objectBinding) {
			_objectBinding = GetComponent<ObjectBinding>();
		}
		return _objectBinding;
	}
}