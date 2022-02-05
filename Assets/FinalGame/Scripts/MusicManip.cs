using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManip : MonoBehaviour
{
    public LibPdInstance pdPatch;

    void OnMouseDown ()
    {
    	pdPatch.SendBang("knobOn");
    }
}
