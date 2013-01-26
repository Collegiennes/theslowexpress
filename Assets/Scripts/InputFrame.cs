using System;

struct InputFrame : IEquatable<InputFrame>
{
    public float TimeRatio { get; set; }

    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public bool Fire1 { get; set; }
    public bool Fire2 { get; set; }

    public override string ToString()
    {
        return string.Format("H = {0:0.00}, V = {1:0.00}, F1 = {2}, F2 = {3}", 
                             Horizontal, Vertical, Fire1, Fire2);
    }

    public override bool Equals(object lhs)
    {
        return lhs != null && Equals((InputFrame)lhs);
    }
    public bool Equals(InputFrame lhs)
    {
        return lhs.Horizontal == Horizontal &&
               lhs.Vertical == Vertical &&
               lhs.Fire1 == Fire1 &&
               lhs.Fire2 == Fire2;
    }

    public static bool operator ==(InputFrame x, InputFrame y)
    {
        return x.Equals(y);
    }
    public static bool operator !=(InputFrame x, InputFrame y)
    {
        return x.Horizontal != y.Horizontal ||
               x.Vertical != y.Vertical ||
               x.Fire1 != y.Fire1 ||
               x.Fire2 != y.Fire2;
    }
}
