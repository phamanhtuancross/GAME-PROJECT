using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DebugUtil
{
#if BUILD_LIVE
    public static bool isDebugBuild = false;
#else
    public static bool isDebugBuild = true;
#endif
    static public void debug(string message, bool force = false)
    {

        if (force || isDebugBuild)
        {
#if NETFX_CORE || WINDOWS_PHONE || UNITY_WP8
            System.Diagnostics.Debug.WriteLine(message);
#else
            Debug.Log(message);
#endif
        }

    }
    static public void debugYellow(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.Log("<color=yellow>" + message + "</color>");
        }
    }
    static public void debugGreen(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.Log("<color=green>" + message + "</color>");
        }
    }

    static public void debugWarning(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.LogWarning(message);
        }
    }

    static public void debugError(string message, bool force = false)
    {
        if (force || isDebugBuild)
        {
            Debug.LogError(message);
        }
    }
}
