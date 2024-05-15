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
    private int _currentlyPlayingVideoIndex = 0;

    private void Start()
    {
        if (masterVideoPlayer == null) Debug.LogError("No video player assigned for " + gameObject.name);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        StartCoroutine(VideoPlayerCoroutine());
    }

    private IEnumerator VideoPlayerCoroutine()
    {
        while (true)
        {
            _currentlyPlayingVideoIndex = (_currentlyPlayingVideoIndex + 1)% videosAndAdvertisementUrls.Count;
            masterVideoPlayer.clip = videosAndAdvertisementUrls[_currentlyPlayingVideoIndex].video;
            masterVideoPlayer.Prepare();
            yield return new WaitForSeconds(5f);
        }
    }
    public void GoToUrl()
    {
        Application.OpenURL(videosAndAdvertisementUrls[_currentlyPlayingVideoIndex].adEmbeddedLink);
    }
}
