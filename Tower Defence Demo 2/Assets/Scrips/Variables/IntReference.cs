using System;

namespace Scrips.Variables
{
    [Serializable]
    public class IntReference
    {
        public bool UseConstant = true;
        public int ConstantValue;
        public IntVariable ReferenceVariable;

        public IntReference()
        {
        }

        public IntReference(int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public IntReference(IntVariable variable)
        {
            UseConstant = false;
            ReferenceVariable = variable;
        }

        public int Value => UseConstant ? ConstantValue : ReferenceVariable.Value;
    }
}