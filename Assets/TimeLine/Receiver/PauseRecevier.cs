using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PauseRecevier : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    private PlayableDirector director;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            director.Play();

        }
    }


    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is PauseMarker)
        {
            PauseMarker marker = notification as PauseMarker;

        }
    }

    public IEnumerator Waiting(PauseMarker marker)
    {
        director.time = marker.rewindtime;
        yield return new WaitForSeconds(marker.pausetime);
        director.time = marker.time + .01f;
    }
}
