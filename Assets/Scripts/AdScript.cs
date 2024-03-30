using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class AdScript : MonoBehaviour
{
    [Serializable]
    struct AdUrls {
        public VideoClip video;
        public string adEmbeddedLink;
    }
    [SerializeField]
    private List<AdUrls> videosAndAdvertisementUrls;
    [SerializeField]
    private float videoChangeTimeInSeconds = 5f;
    [SerializeField]
    private VideoPlayer masterVideoPlayer;
    private int currentlyPlayingVideoIndex = 0;

    private void Start()
    {
        if (masterVideoPlayer == null) Debug.LogError("No video player assigned for " + gameObject.name);
        StartCoroutine(VideoPlayerCoroutine());
    }
    private IEnumerator VideoPlayerCoroutine()
    {
        while (true)
        {
            currentlyPlayingVideoIndex = (currentlyPlayingVideoIndex+1)% videosAndAdvertisementUrls.Count;
            masterVideoPlayer.clip = videosAndAdvertisementUrls[currentlyPlayingVideoIndex].video;
            masterVideoPlayer.Prepare();
            yield return new WaitForSeconds(5f);
        }
    }
    public void GoToUrl()
    {
        Application.OpenURL(videosAndAdvertisementUrls[currentlyPlayingVideoIndex].adEmbeddedLink);
    }
}
