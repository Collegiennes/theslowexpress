using UnityEngine;
using System.Collections;

public class PoisedNoise : MonoBehaviour
{
    public static int RandomRound(float val)
    {
        return Mathf.FloorToInt(val) + (Random.value > Mathf.Repeat(val, 1) ? 0 : 1);
    }

    public static uint Hash(uint x)
    {
        return someNumbers[x & 0xFF] ^ someNumbers[(x>>8) & 0xFF] ^
               someNumbers[(x>>16) & 0xFF] ^ someNumbers[(x>>24) & 0xFF];
    }

    public static uint Hash(uint x, uint y)
    {
        return Hash(x) ^ Hash(y * 27644437);
    }

    public static uint Hash(uint x, uint y, uint z)
    {
        return Hash(x) ^ Hash(y * 27644437) ^ Hash(z * 44318);
    }

    public static float UintToFloat(uint x)
    {
        return (float)(x/(double)0x100000000);
    }

    public static float FourOctaveHash(int x, int y)
    {
        return
            UintToFloat(Hash((uint)x, (uint)y))/3 +
            UintToFloat(Hash((uint)x/2, (uint)y/2))/3 +
            UintToFloat(Hash((uint)x/4, (uint)y/4))/6 +
            UintToFloat(Hash((uint)x/8, (uint)y/8))/6;
    }

    static PoisedNoise()
    {
        ResetHash();
    }

    public static void ResetHash()
    {
        System.Random r = new System.Random();
        for(int i = 0; i < someNumbers.Length; i++)
        {
            someNumbers[i] = (uint)(r.NextDouble() * 0x100000000);
        }
    }

    static uint[] someNumbers = new uint[256]; /*{ 641092716, 872704008,
        3657282555, 516308364, 1034451792, 3844196931, 4274519439, 2894813462,
        2994169070, 4109363679, 724981356, 540033120, 1193709133, 3631984887,
        1246139533, 1516680397, 4107528615, 1393600033, 2251099226, 3014748,
        3784557351, 856581660, 838624248, 1575533521, 3613634247, 2367494714,
        936669096, 3168762302, 1670170393, 4261542915, 1937958661, 4081444491,
        1183616281, 4209374667, 3786130263, 1992879505, 1551677689, 3088805942,
        192288492, 3113710382, 2784709622, 2926140626, 2528587118, 434123712,
        2576429858, 1187941789, 1652999437, 736384968, 1704905533, 3033622946,
        411185412, 2793229562, 1254135169, 2268663410, 2590061762, 3122361398,
        3065474414, 4150783695, 874801224, 442250424, 1204588441, 2132344369,
        4206884223, 2181760022, 1623376261, 3164961098, 1060798068, 4188664659,
        1966795381, 769153968, 2120416453, 2826129638, 3062983970, 3374944851,
        3035326934, 2012278753, 1230541489, 4253809431, 2148859946, 1375118317,
        2872399466, 548815212, 4074890691, 1093567069, 1900077697, 3555567579,
        2275872590, 1008891972, 117706248, 1732169341, 374353056, 3804480903,
        2513513378, 3277686459, 921857508, 4134268119, 3165354326, 3788882859,
        4177261047, 3564349671, 1085309281, 1541846989, 1457040817, 3140318810,
        1577892889, 398077812, 1310891077, 561005280, 3780100767, 2621257850,
        71436420, 3903574359, 2975949506, 324282024, 3751264047, 2975294126,
        4058375115, 2193163634, 4246862403, 437400612, 1713425473, 1127253601,
        3937129815, 824205888, 1013741784, 1046379708, 225319644, 3765944559,
        3927036963, 4211471883, 2638035578, 2621126774, 1068662628, 4292345775,
        2620078166, 466892712, 1581038713, 2494638434, 2998101350, 1702021861,
        3024447626, 1785910501, 1482600637, 4094814243, 2885113838, 825909876,
        821453292, 1073905669, 829973232, 2771339870, 102632508, 1912136689,
        3118035890, 432944028, 1166183173, 2290684178, 4067419359, 195827544,
        1803605761, 3091034234, 3234431379, 3234169227, 1112573089, 1309580317,
        3138352670, 2134310509, 1571208013, 3699226875, 3237052899, 1654310197,
        3135731150, 4211471883, 3708271119, 1472114557, 3988511607, 1614463093,
        381824388, 1190563309, 3243344547, 2655206534, 1557051805, 4108446147,
        3052366814, 1239716809, 805068792, 2712224594, 1126598221, 3557402643,
        916876620, 4007386551, 1182436597, 3721247643, 1621147969, 329525064,
        2256866570, 1325440513, 3281356587, 3885748023, 2996790590, 4147375719,
        3623464947, 3423180819, 3339685407, 20447856, 1047035088, 4222744419,
        1034320716, 20316780, 3046730546, 1226609209, 4076070375, 3844721235,
        1756942705, 3545867955, 669142980, 2414288846, 1046248632, 3108860570,
        2280198098, 1722994021, 1340383177, 1400153833, 2611689302, 2570662514,
        828793548, 4231264359, 2418614354, 1813960765, 840197160, 4002405663,
        3127080134, 1309056013, 1025800776, 1133020945, 3212672762, 4280024631,
        2201421422, 2514037682, 3379663587, 2467636778, 2487691406, 1412999281,
        2955894878, 2512595846, 302261256, 1244435545, 2135490193, 437269536,
        2731099538, 1733217949, 1075216429, 4220909355, 1291360753, 1292802589,
        1885528261, 445527324};*/
}
    
