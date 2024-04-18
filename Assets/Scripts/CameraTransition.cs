using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTransition : MonoBehaviour
{

    public bool isBattleOver;
    public Animator animator;
    public float transitionTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            LoadNextEncounter();
        }
    }

    public void LoadNextEncounter()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
