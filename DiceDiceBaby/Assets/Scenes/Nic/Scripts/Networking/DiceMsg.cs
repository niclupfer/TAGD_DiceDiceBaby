using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DiceMsg
{
    public static short Something = 1000;
    public static short Welcome = 1001;
    public static short Chat = 1002;
    public static short Ready = 1003;
    public static short Lobby = 1004;

    public static short DicePool = 1005;
    public static short DraftTurn = 1006;
    public static short DraftPick= 1007;

    // X host battle
    // x join battle
    // X battle info
    // X welcome to battle <player info>
    // X ready to battle
    // X battle start

    // X -- dice drafting
    // X -- server generates dice to choose from
    // X available dice <[dice]>
    // X its your turn
    // X i choose dice <dice>
    // X -- repeat until all players have N dice
    // X draft complete

    // do your turn ( roll, choose spell)
    // heres my mana <[mana + counts]>
    // i'm casting <spell>
    // -- once all players have sent in their spells
    // -- figure our order and effects
    // 
    // - effcect rounds
    // - applies the effects to each to player
    // receive spells <[playerName, spell]>
    // update stats <[player info]>
    // 
    // -- server checks for win
    // - gameover <winner info>
    // - repeat do your turn

    // - cancel match
    // - player quit

    //public static short PlayerJoin = 1001;
    //public static short BattleBegin = 1002;
    //public static short Spell = 1003;
};

public class SomethingMessage : MessageBase
{
    public string whatever;
}

public class WelcomeMsg : MessageBase
{
    public int totalPlayers;
    public DicePlayer you;
    public DicePlayer them;
}

public class ChatMsg : MessageBase
{
    public int fromPlayer;
    public string msg;
}

public class ReadyMsg : MessageBase
{
    public int fromPlayer;
    public bool ready;
}

public class LobbyMsg : MessageBase
{
    public DicePlayer[] allPlayers;
}

public class DicePoolMsg : MessageBase
{
    public string diceData;
}

public class DiceDraftTurnMsg : MessageBase
{
    public int whosTurn;
}

public class DraftPickMsg : MessageBase
{
    public int fromPlayer;
    public string dice;
}