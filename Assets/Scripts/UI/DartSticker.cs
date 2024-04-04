using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DartSticker : MonoBehaviour
{
    public static DartSticker inst;
    [SerializeField] Transform Location;
    [SerializeField] Image DartImage;
    [SerializeField] Sprite StuckSprite;
    [SerializeField] Sprite FlyingSprite;
    [SerializeField] Vector2 StartingDistanceFromTargetInPixels;
    [SerializeField] float PixelsTraveledPerSecond;
    Vector2 TargetLocation;
    bool First;

    public void Awake()
    {
        if(inst!=null)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += LevelLoad;
        SetVisible(true);
        enabled = false;
        DontDestroyOnLoad(this);
        inst = this;
        First = true;
    }
    public void LevelLoad(Scene s, LoadSceneMode m)
    {
        SetVisible(false);
        if (ControlState.inst != null)
            if (ControlState.inst.IsUsingController())
                First = true;
    }

    bool SkipFirstInteractionOfLevel()
    {
        First = false;
        if (ControlState.inst == null)
            return true;
        if (ControlState.inst.IsUsingController())
            return true;
        return false;
    }

    public void NewLocation(Vector2 newLocation)
    {
        if (!UIState.inst.GetCurrentState())
            return;
        if (First)
        {
            if (SkipFirstInteractionOfLevel())
                return;
        }
            
        DartImage.enabled = true;
        DartImage.sprite = FlyingSprite;
        TargetLocation = newLocation;
        //Location.SetPositionAndRotation(newLocation + StartingDistanceFromTargetInPixels, Location.rotation);
        Location.position = newLocation + StartingDistanceFromTargetInPixels;
        enabled = true;
        
    }

    public void SetVisible(bool b)
    {
        DartImage.enabled = b;
    }

    public void Update()
    {
        Location.position = Vector2.MoveTowards(Location.position, TargetLocation, PixelsTraveledPerSecond * Time.deltaTime);
        if(Vector2.Distance(Location.position, TargetLocation) <.005f)
        {
            DartImage.sprite = StuckSprite;
            //Audio.inst.PlayDartClipReverb((DartAudioClips)Random.Range(0,3), AudioReverbPreset.Bathroom);
            Audio.inst.PlayClip(AudioClips.RandomDart);
            enabled = false;
        }

    }

#if UNITY_EDITOR
    [SerializeField] [Range(.01f,.05f)] float __secondsTaken;
    [SerializeField] bool __useSeconds;
    private void OnValidate()
    {
        if (__useSeconds)
        {
            PixelsTraveledPerSecond = StartingDistanceFromTargetInPixels.magnitude / __secondsTaken;
        }
    }

#endif
}
