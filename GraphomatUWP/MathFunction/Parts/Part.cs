﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    enum PartRuleType
    {
        Value, Start, End, OneValueFunction, TwoValueFunction, MultDiv, AddSub
    }

    abstract class Part
    {
        public abstract PartRuleType GetRuleType();

        public void CheckIfPossibleNextPart(Parts parts)
        {
            while (true)
            {
                int thisIndex = parts.ExtendedIndexOf(this);
                PartRuleType nextType = parts[thisIndex + 1].GetRuleType();

                if (IsOptimalNextPart(nextType)) return;

                if (!IsPossibleNextPart(nextType) || !WasAbleToChangeIfNecessary(parts, thisIndex, nextType))
                {
                    throw new ArgumentException();
                }
            }
        }

        protected abstract bool IsPossibleNextPart(PartRuleType nextType);

        protected abstract bool IsOptimalNextPart(PartRuleType nextType);

        protected abstract bool WasAbleToChangeIfNecessary(Parts parts, int thisIndex, PartRuleType nextType);

        protected virtual string[] GetLowerLooks()
        {
            return new string[] { "?" };
        }

        public bool IsType(Equation equation)
        {
            foreach (string look in GetLowerLooks())
            {
                if (LooksLike(equation, look)) return true;
            }

            return false;
        }

        private bool LooksLike(Equation equation, string look)
        {
            for (int i = 0; i < look.Length; i++)
            {
                if (i >= equation.Count || char.ToLower(equation[i]) != look[i]) return false;
            }

            for (int i = 0; i < look.Length; i++) equation.RemoveAt(0);

            return true;
        }

        public abstract Part Clone();

        public virtual string ToEquationString()
        {
            return GetLowerLooks()[0];
        }
    }
}
