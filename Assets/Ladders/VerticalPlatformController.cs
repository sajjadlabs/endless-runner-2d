using UnityEngine;

public class VerticalPlatformController : MonoBehaviour
{
    private PlatformEffector2D _platform;
    public float waitTime;

    private void Start()
    {
        _platform = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        ChangeColliderAngle();
    }
    

    private void ChangeColliderAngle()
    {
        if (ReleaseDownKey())
        {
            waitTime = 0.2f;
        }
        
        if (HitDownArrowKey())
        {
            waitTime -= Time.deltaTime;

            if (waitTime < 0)
            {
                _platform.rotationalOffset = 180f;

                waitTime = 0.2f;
            }
        }

        if (HitUpArrowKey())
        {
            _platform.rotationalOffset = 0f;
        }
    }

    private static bool ReleaseDownKey()
    {
        return Input.GetKeyUp(KeyCode.DownArrow);
    }

    private static bool HitDownArrowKey()
    {
        return Input.GetKey(KeyCode.DownArrow);
    }

    private static bool HitUpArrowKey()
    {
        return Input.GetKey(KeyCode.UpArrow);
    }
}