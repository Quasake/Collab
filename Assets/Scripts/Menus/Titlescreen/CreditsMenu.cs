using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : Menu {
	public void OpenURL (string url) {
		Application.OpenURL(url);
	}
}
