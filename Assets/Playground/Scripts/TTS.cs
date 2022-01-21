using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTS : MonoBehaviour {

	public string voice = "Samantha";
	public int outputChannel = 48;

	public string text2speak;

	public UnityEvent onStartedSpeaking;
	public UnityEvent onStoppedSpeaking;

	System.Diagnostics.Process speechProcess;
	bool wasSpeaking;

	void Update() {
		bool isSpeaking = (speechProcess != null && !speechProcess.HasExited);
		if (isSpeaking != wasSpeaking) {
			if (isSpeaking) onStartedSpeaking.Invoke();
			else onStoppedSpeaking.Invoke();
			wasSpeaking = isSpeaking;
		}
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			Speak(text2speak);
		}
	}

	public void Speak(string text) {
		string cmdArgs = string.Format("-a {2} -v {0} \"{1}\"", voice, text.Replace("\"", ","), outputChannel);
		speechProcess = System.Diagnostics.Process.Start("/usr/bin/say", text2speak);
	}
}