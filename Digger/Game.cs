using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public static class Game
{
    private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

    private const string mapWithPlayerTerrainSackGold = @"
PTTGTT TS
TST  TSTT
TTTTTTSTT
T TSTS TT
T TTTG ST
TSTSTT TT";

    private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TST
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TMT M TS
TSTSTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";

    private const string mapWithPlayerTerrainSackGoldMonsterRandom = @"
P    TTTRTTTRT        TTRTT       TRTT
  TT TTTTTTTR TTTTTTR     T  TRTT    R
  TT      R   TRTTTTTTTTS    TTRT  TTT
 TT RRTTR T TTTTTTTTRTTTSTTTTSRTT TRTT
TTT  TSTT    TS   TTTTT TTTT       RTT
 TST TT    R    TTTTSTT   TTS TTTTTRTT
 TT     TSTTST TSTTTTTTT    TTTTTTTTTR
 TTTTTRTTTTTTT TTTTTTTSST T  TTSTTTTRT
  TTTTRTTTSTTT RTTTTTTTTTTT  TTTRTTTTT
T TTTTTTSTTTTT TTTTRTTTTTTTTRTTRTTTTTT
T TTSSTTTT                      TSTTRT
TRTTTTTTTT TTTTTRTT  TGT TSGTT   TTTTT
T           GTTTTTTS TTT TTTTSTRTSTTGT
TTT  GTTSTTTTTTTTTTSSTTT TTSTTT TTTTTT
TTGT TTTTSTTGTTGTTTTTTGT        TTSTTT
TT T  T TTT TTTTTTTTTTTTTGTTTGTTTTRTTT
TTT T TTT     TTT TTTT T TTTTTTTTGTTTG
";

    public static ICreature[,] Map;
    public static int Scores;
    public static bool IsOver;

    public static Key KeyPressed;
    public static int MapWidth => Map.GetLength(0);
    public static int MapHeight => Map.GetLength(1);

    public static void CreateMap()
    {
        var randomMap = RandomMapCreator.GenerateRandomMap(20, 20);
        Map = CreatureMapCreator.CreateMap(mapWithPlayerTerrainSackGoldMonsterRandom);
    }
}