using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSGIS.SubSystems
{
    static public class DataTypeTools
    {
        public static Type GetTypeFromConstant(MyMapObjects.moValueTypeConstant TypeConstant)
        {
            if(TypeConstant == MyMapObjects.moValueTypeConstant.dInt16)
            {
                return Type.GetType("System.Int16");
            }
            else if (TypeConstant == MyMapObjects.moValueTypeConstant.dInt32)
            {
                return Type.GetType("System.Int32");
            }
            else if (TypeConstant == MyMapObjects.moValueTypeConstant.dInt64)
            {
                return Type.GetType("System.Int64");
            }
            else if (TypeConstant == MyMapObjects.moValueTypeConstant.dSingle)
            {
                return Type.GetType("System.Single");
            }
            else if (TypeConstant == MyMapObjects.moValueTypeConstant.dDouble)
            {
                return Type.GetType("System.Double");
            }
            else if (TypeConstant == MyMapObjects.moValueTypeConstant.dText)
            {
                return Type.GetType("System.String");
            }
            return null;
        }
    }
}
