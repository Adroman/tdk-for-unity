using System;

namespace Scrips.Variables
{
    [Serializable]
    public class Reference<TConstant, TVariable> where TVariable : Variable<TConstant>
    {
        public bool UseConstant = true;
        public TConstant ConstantValue;
        public TVariable ReferenceVariable;

        public Reference()
        {
        }

        public Reference(TConstant value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Reference(TVariable variable)
        {
            UseConstant = false;
            ReferenceVariable = variable;
        }

        public TConstant Value => UseConstant ? ConstantValue : ReferenceVariable.Value;
    }
}