﻿namespace MathFunction
{
    class PartMult : PartMultDiv
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "•", "*" };
        }

        protected override double Calc()
        {
            return Value1.Value * Value2.Value;
        }

        public override FunctionPart Clone()
        {
            return new PartMult();
        }
    }
}
