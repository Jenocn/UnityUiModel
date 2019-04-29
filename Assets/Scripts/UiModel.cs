using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(ObjectBinding))]
public class UiModel : MonoBehaviour {

	private ObjectBinding _objectBinding = null;

	protected void Awake() {
		OnCreate();
	}

	protected void Start() {
		OnOpen();
		OnOpenAction();
	}

	protected void OnDestroy() {
		OnRelease();
	}

	protected void Update() {
		OnUpdate();
	}

	//=================== methods ====================
	public GameObject Query(string key) {
		return _GetObjectBinding().Query(key);
	}
	public T QueryCommponent<T>(string key) where T : Component {
		return _GetObjectBinding().QueryComponent<T>(key);
	}

	/**
	 * =================== life circle ===================
	 * OnCreate			↓ [self 'Awake']
	 * OnOpen			↓ [self 'Start']
	 * OnOpenAction		↓ [after 'OnOpen']
	 * OnUpdate			↓ [self 'Update']
	 * OnPause			↓ [if new UiModel 'Push' on top]
	 * OnResume			↓ [if top UiModel 'Pop' and self on top]
	 * OnCloseAction	↓ [self 'Pop']
	 * OnClose			↓ [after 'OnCloseAction']
	 * OnRelease		↓ [self 'OnDestroy']
	 */

	public virtual void OnCreate() { }
	public virtual void OnRelease() { }

	public virtual void OnOpen() { }
	public virtual void OnClose() { }

	public virtual void OnOpenAction() { }
	public virtual float OnCloseAction() { return 0; }

	public virtual void OnPause() { }
	public virtual void OnResume() { }

	public virtual void OnUpdate() { }

	// ====================== end =====================
	private ObjectBinding _GetObjectBinding() {
		if (!_objectBinding) {
			_objectBinding = GetComponent<ObjectBinding>();
		}
		return _objectBinding;
	}
}