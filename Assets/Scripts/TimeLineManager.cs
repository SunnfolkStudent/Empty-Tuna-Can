using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class TimeLineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    private bool cutsceneRunning = true;
    
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (cutsceneRunning)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine("SkipTimeLine");
            }
        }
        
    }

    IEnumerator SkipTimeLine()
    {
        cutsceneRunning = false;
        playableDirector.time = 63f;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
