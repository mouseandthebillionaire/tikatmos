using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextResize : MonoBehaviour {
    public RectTransform t;
    
    // Start is called before the first frame update
    void Start() {
        t.sizeDelta = new Vector2(300, 300);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
