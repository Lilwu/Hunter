using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableControl : MonoBehaviour
{
    public GameObject startCamera;
    public PlayableDirector pb;
    private bool isPresent;

    private void Awake()
    {
        isPresent = GetComponent<GameManager>().ISPRESENT;
    }

    private void Start()
    {
        if(isPresent)
        {
            Destroy(pb.gameObject);
            Destroy(GetComponent<PlayableControl>());
            Destroy(startCamera);
        }
    }

    private void Update()
    {
        if(pb.state == PlayState.Paused && !isPresent )
        {
            Destroy(pb.gameObject);
            Destroy(GetComponent<PlayableControl>());
            Destroy(startCamera);
            GetComponent<GameManager>().StartAnnounce();
        }
    }
}
