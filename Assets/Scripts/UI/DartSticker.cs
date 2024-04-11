using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DartSticker : MonoBehaviour
{
    public static DartSticker inst;
    [SerializeField] Transform Location;
    [SerializeField] Image DartImage;
    //[SerializeField] Sprite StuckSprite;
    [SerializeField] SpriteCollection FlyingSprite;
    [SerializeField] Vector2 StartingDistanceFromTargetInPixels;
    [SerializeField] float PixelsTraveledPerSecond;
    Vector2 TargetLocation;
    bool First;
    bool CurrentlyUsingController;
    bool CurrentUIState;



    public void Awake()
    {
        if(inst!=null)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += LevelLoad;
        ControlState.UsingController += IsUsingController;
        SetVisible(true);
        enabled = false;
        DontDestroyOnLoad(this);
        inst = this;
        First = true;
    }

    public void IsUsingController(bool b) {
        CurrentlyUsingController = b;
    }

    public void LevelLoad(Scene s, LoadSceneMode m) {
        SetVisible(false);
        if (CurrentlyUsingController)
            First = true;
    }

    bool SkipFirstInteractionOfLevel()
    {
        First = false;
        if (ControlState.inst == null)
            return true;
        if (CurrentlyUsingController)
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
        DartImage.sprite = FlyingSprite.Sprites[Random.Range(0, FlyingSprite.Sprites.Length)];
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
            //DartImage.sprite = StuckSprite;
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
