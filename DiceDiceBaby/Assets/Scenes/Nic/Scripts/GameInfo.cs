using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetRole { unknown, host, hostPlayer, player }

public static class GameInfo
{
    public static NetRole netRole = NetRole.unknown;
}
