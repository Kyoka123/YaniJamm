using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DiveIn());
    }

    IEnumerator DiveIn()
    {
        animator.Play("DiveIn");
        yield return new WaitForSeconds(7f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Son seviyedeyken ilk seviyeye (veya Main Menu - Scene 0) döner
            SceneManager.LoadScene(0);
        }
    }
}
