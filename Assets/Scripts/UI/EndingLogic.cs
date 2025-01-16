using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingLogic : MonoBehaviour
{
    [SerializeField]
    VideoPlayer endingVideo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (endingVideo == null)
        {
            endingVideo = GetComponent<VideoPlayer>();
        }

        if (endingVideo != null)
        {
            endingVideo.loopPointReached += OnVideoEnd; 
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video has finished playing!");
        StartCoroutine(waitAndLoadScene(0));
    }

    void OnDestroy()
    {
        if (endingVideo != null)
        {
            endingVideo.loopPointReached -= OnVideoEnd;
        }
    }

    protected IEnumerator waitAndLoadScene(int sceneToLoad, float waitTime = 1f)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
