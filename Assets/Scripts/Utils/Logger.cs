using UnityEngine;

public static class Logger
{
    public static void Camera(string message)
    {
#if DEBUG_CAMERA || DEBUG_ALL
        Debug.Log(message);
#endif
    }

    public static void Input(string message)
    {
#if DEBUG_INPUT || DEBUG_ALL
        Debug.Log(message);
#endif
    }
}
