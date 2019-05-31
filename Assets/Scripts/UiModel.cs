using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UiModel : MonoBehaviour {

	private UiStack _uiStack = null;

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

	public UiModel PushUI(GameObject prefab) {
		if (!_uiStack) { return null; }
		return _uiStack.Push(prefab);
	}

	public T PushUI<T>(GameObject prefab) where T : UiModel {
		return PushUI(prefab) as T;
	}

	public void Close() {
		if (_uiStack) {
			_uiStack.Pop();
		}
	}

	public void _BindUiStack(UiStack uiStack) {
		_uiStack = uiStack;
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
	public virtual void OnOpen() { }
	public virtual void OnOpenAction() { }

	public virtual void OnPause() { }
	public virtual void OnResume() { }

	public virtual void OnUpdate() { }
	public virtual float OnCloseAction() { return 0; }
	public virtual void OnClose() { }
	public virtual void OnRelease() { }

	// ====================== end =====================
}