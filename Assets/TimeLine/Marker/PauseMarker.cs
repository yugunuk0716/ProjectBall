
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PauseMarker : Marker, INotification
{
    [SerializeField, Header("멈춰! 번호")] int pauseId;
    [SerializeField, Header("멈추고 있을 시간")] float pauseTime;
    [SerializeField, Header("멈춘동안 돌아갈 시간")] float rewindTime;

    public PropertyName id => new PropertyName();
    public float pausetime => pauseTime;
    public int pauseid => pauseId;
    public float rewindtime => rewindTime;
}
