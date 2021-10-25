using System;

namespace Volo.Abp.Validation.StringValues
{
    [Serializable]
    [ValueValidator("NUMERIC")]
    public class NumericValueValidator : ValueValidatorBase
    {
        public int MinValue
        {
            get => (this["MinValue"] ?? "0").To<int>();
            set => this["MinValue"] = value;
        }

        public int MaxValue
        {
            get => (this["MaxValue"] ?? "0").To<int>();
            set => this["MaxValue"] = value;
        }

        public NumericValueValidator()
        {
            MinValue = int.MinValue;
            MaxValue = int.MaxValue;
        }

        public NumericValueValidator(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is int)
            {
                return IsValidInternal((int)value);
            }

            if (value is string)
            {
                int intValue;
                if (int.TryParse(value as string, out intValue))
                {
                    return IsValidInternal(intValue);
                }
            }

            return false;
        }

        protected virtual bool IsValidInternal(int value)
        {
            return value.IsBetween(MinValue, MaxValue);
        }
    }
}