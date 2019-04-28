using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UiModel : MonoBehaviour {

	//=================== life circle ===================
	public virtual void OnOpen() { }
	public virtual void OnOpenAction() { }
	public virtual float OnCloseAction() { return 0; }
	public virtual void OnClose() { }

	public virtual void OnPause() { }
	public virtual void OnResume() { }
}
