using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
	
public class RadioButtons:MonoBehaviour {

	public string defaultValue;

	public Color normalColor;
	public Color pressedColor;

	public UnityEvent onValueChanged;

	public static bool tilt;

	[NonSerialized] public string currentValue;
	
	public void ForceToValue(string v) {
		_oneRadioButtonLiterallyClicked(v);
	}
	
	///////////////////////////////////////////////////////////////////
	
	void Awake() {
		_setup();
		_unselectAll();
		_selectValue(defaultValue);
	}
	
	private void _setup() {
		int k=0;
		Transform[] tts = gameObject.GetComponentsInChildren<Transform>();
		tilt = true;
		foreach (Transform tt in tts) {
			if (tt.GetComponent<Button>()) {
				Button bb = tt.GetComponent<Button>();
				string val = tt.name;
				//Debug.Log("In RadioButtons " +gameObject.name +" there's a button: " +val);
				bb.onClick.AddListener(delegate { _oneRadioButtonLiterallyClicked(val); });
				k++;
			}
		}
		//Debug.Log("In RadioButtons " +gameObject.name +" there are count buttons: " +k);
		if (k==0) Debug.LogError("YOU HAVE AN EMPTY RadioButton Panel, named " + gameObject.name);
	}
	
	public void _oneRadioButtonLiterallyClicked(string v) {
		_unselectAll();
		if(v == "Tilt") tilt = true;
		else tilt = false;
		_selectValue(v);
		onValueChanged.Invoke();
	}
	
	/// set the whole group ...
	
	private void _unselectAll() {
		Transform[] tts = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform tt in tts) {
			if (tt.GetComponent<Button>()) {
				Button bb = tt.GetComponent<Button>();
				_unselectedStyle(bb);
			}
		}
	}
	
	private void _selectValue(string v)  {
		Transform[] tts = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform tt in tts) {
			if (tt.GetComponent<Button>()) {
				if (tt.name == v) {
					Button bb = tt.GetComponent<Button>();
					_selectedStyle(bb);
					currentValue = tt.name;
					return;
				}
			}
		}
		Debug.LogError("Button not found" +gameObject.name);
	}
	
	/// set colors on Button ...
	
	private void _unselectedStyle(Button bb) {
		ColorBlock cb = bb.colors;
		cb.normalColor = normalColor;
		cb.highlightedColor = normalColor;
		bb.colors = cb;
	}
	
	private void _selectedStyle(Button bb) {
		ColorBlock cb = bb.colors;
		cb.normalColor = pressedColor;
		cb.highlightedColor = pressedColor;
		bb.colors = cb;
	}
}