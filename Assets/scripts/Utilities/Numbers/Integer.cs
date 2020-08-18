using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// GÜELCOM TU JAVA IN C#. Clsase que envuelve los ints para tratarlos como objetos.
/// </summary>
public class Integer
{
    public int Value { get; set; }

    public Integer()
    {
        Value = 0;
    }

    public Integer(int value)
    {
        Value = value;
    }

    public static Integer operator +(Integer f1, Integer f2)
    {
        return new Integer(f1.Value + f2.Value);
    }

    public static Integer operator +(Integer f1, int f2)
    {
        return new Integer(f1.Value + f2);
    }

    public static Integer operator +(int f1, Integer f2)
    {
        return new Integer(f1 + f2.Value);
    }

    public static Integer operator -(Integer f1, Integer f2)
    {
        return new Integer(f1.Value - f2.Value);
    }

    public static Integer operator -(Integer f1, int f2)
    {
        return new Integer(f1.Value - f2);
    }

    public static Integer operator -(int f1, Integer f2)
    {
        return new Integer(f1 - f2.Value);
    }

    public static Integer operator *(Integer f1, Integer f2)
    {
        return new Integer(f1.Value * f2.Value);
    }

    public static Integer operator *(Integer f1, int f2)
    {
        return new Integer(f1.Value * f2);
    }

    public static Integer operator *(int f1, Integer f2)
    {
        return new Integer(f1 * f2.Value);
    }

    public static Integer operator /(Integer f1, Integer f2)
    {
        return new Integer(f1.Value / f2.Value);
    }

    public static Integer operator /(Integer f1, int f2)
    {
        return new Integer(f1.Value / f2);
    }

    public static Integer operator /(int f1, Integer f2)
    {
        return new Integer(f1 / f2.Value);
    }

    public static Integer operator ++(Integer f1)
    {
        return new Integer(f1.Value++);
    }

    public static Integer operator --(Integer f1)
    {
        return new Integer(f1.Value--);
    }

    public static bool operator ==(Integer f1, Integer f2)
    {
        return f1.Value == f2.Value;
    }

    public static bool operator ==(Integer f1, int f2)
    {
        return f1.Value == f2;
    }

    public static bool operator ==(int f1, Integer f2)
    {
        return f1 == f2.Value;
    }

    public static bool operator !=(Integer f1, Integer f2)
    {
        return f1.Value != f2.Value;
    }

    public static bool operator !=(Integer f1, int f2)
    {
        return f1.Value != f2;
    }

    public static bool operator !=(int f1, Integer f2)
    {
        return f1 != f2.Value;
    }

    public static bool operator <(Integer f1, Integer f2)
    {
        return f1.Value < f2.Value;
    }

    public static bool operator <(Integer f1, int f2)
    {
        return f1.Value < f2;
    }

    public static bool operator <(int f1, Integer f2)
    {
        return f1 < f2.Value;
    }

    public static bool operator >(Integer f1, Integer f2)
    {
        return f1.Value > f2.Value;
    }

    public static bool operator >(Integer f1, int f2)
    {
        return f1.Value > f2;
    }

    public static bool operator >(int f1, Integer f2)
    {
        return f1 > f2.Value;
    }

    public static bool operator <=(Integer f1, Integer f2)
    {
        return f1.Value <= f2.Value;
    }

    public static bool operator <=(Integer f1, int f2)
    {
        return f1.Value <= f2;
    }

    public static bool operator <=(int f1, Integer f2)
    {
        return f1 <= f2.Value;
    }

    public static bool operator >=(Integer f1, Integer f2)
    {
        return f1.Value >= f2.Value;
    }

    public static bool operator >=(Integer f1, int f2)
    {
        return f1.Value >= f2;
    }

    public static bool operator >=(int f1, Integer f2)
    {
        return f1 >= f2.Value;
    }

    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return Value.Equals(((Integer)obj).Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
