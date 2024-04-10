using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class TimeLineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine("SkipTimeLine");
        }
    }

    IEnumerator SkipTimeLine()
    {
        playableDirector.time = 63f;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
