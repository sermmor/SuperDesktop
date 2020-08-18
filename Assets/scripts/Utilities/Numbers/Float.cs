using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// GÜELCOM TU JAVA IN C#. Clsase que envuelve los floats para tratarlos como objetos.
/// </summary>
public class Float
{
    public float Value { get; set; }

    public Float()
    {
        Value = 0;
    }

    public Float(float value)
    {
        Value = value;
    }

    public static Float operator +(Float f1, Float f2)
    {
        return new Float(f1.Value + f2.Value);
    }

    public static Float operator +(Float f1, float f2)
    {
        return new Float(f1.Value + f2);
    }

    public static Float operator +(float f1, Float f2)
    {
        return new Float(f1 + f2.Value);
    }

    public static Float operator -(Float f1, Float f2)
    {
        return new Float(f1.Value - f2.Value);
    }

    public static Float operator -(Float f1, float f2)
    {
        return new Float(f1.Value - f2);
    }

    public static Float operator -(float f1, Float f2)
    {
        return new Float(f1 - f2.Value);
    }

    public static Float operator *(Float f1, Float f2)
    {
        return new Float(f1.Value * f2.Value);
    }

    public static Float operator *(Float f1, float f2)
    {
        return new Float(f1.Value * f2);
    }

    public static Float operator *(float f1, Float f2)
    {
        return new Float(f1 * f2.Value);
    }

    public static Float operator /(Float f1, Float f2)
    {
        return new Float(f1.Value / f2.Value);
    }

    public static Float operator /(Float f1, float f2)
    {
        return new Float(f1.Value / f2);
    }

    public static Float operator /(float f1, Float f2)
    {
        return new Float(f1 / f2.Value);
    }

    public static Float operator ++(Float f1)
    {
        return new Float(f1.Value++);
    }

    public static Float operator --(Float f1)
    {
        return new Float(f1.Value--);
    }

    public static bool operator ==(Float f1, Float f2)
    {
        return f1.Value == f2.Value;
    }

    public static bool operator ==(Float f1, float f2)
    {
        return f1.Value == f2;
    }

    public static bool operator ==(float f1, Float f2)
    {
        return f1 == f2.Value;
    }

    public static bool operator !=(Float f1, Float f2)
    {
        return f1.Value != f2.Value;
    }

    public static bool operator !=(Float f1, float f2)
    {
        return f1.Value != f2;
    }

    public static bool operator !=(float f1, Float f2)
    {
        return f1 != f2.Value;
    }

    public static bool operator <(Float f1, Float f2)
    {
        return f1.Value < f2.Value;
    }

    public static bool operator <(Float f1, float f2)
    {
        return f1.Value < f2;
    }

    public static bool operator <(float f1, Float f2)
    {
        return f1 < f2.Value;
    }

    public static bool operator >(Float f1, Float f2)
    {
        return f1.Value > f2.Value;
    }

    public static bool operator >(Float f1, float f2)
    {
        return f1.Value > f2;
    }

    public static bool operator >(float f1, Float f2)
    {
        return f1 > f2.Value;
    }

    public static bool operator <=(Float f1, Float f2)
    {
        return f1.Value <= f2.Value;
    }

    public static bool operator <=(Float f1, float f2)
    {
        return f1.Value <= f2;
    }

    public static bool operator <=(float f1, Float f2)
    {
        return f1 <= f2.Value;
    }

    public static bool operator >=(Float f1, Float f2)
    {
        return f1.Value >= f2.Value;
    }

    public static bool operator >=(Float f1, float f2)
    {
        return f1.Value >= f2;
    }

    public static bool operator >=(float f1, Float f2)
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

        return Value.Equals(((Float)obj).Value);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
