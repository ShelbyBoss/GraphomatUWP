﻿namespace MathFunction
{
    class PartMult : PartMultDiv
    {
        protected override double Calc()
        {
            return Value1.Value * Value2.Value;
        }

        public override string ToEquationString()
        {
            return " * ";
        }

        public override PartCalc Clone()
        {
            return new PartMult();
        }
    }
}