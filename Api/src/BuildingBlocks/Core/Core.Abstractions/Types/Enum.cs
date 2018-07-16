using System;
using System.Collections.Generic;

namespace Core.Abstractions.Types
{
    public class Enum<T> where T : struct, IConvertible
    {
        public static IEnumerable<T> AsEnumerable
        {
            get
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException(typeof(T).ToString() + " deve ser um enumerado.");
                
                Type enumType = typeof(T);

                Array enumValArray = Enum.GetValues(enumType);
                List<T> enumValList = new List<T>(enumValArray.Length);

                foreach (int val in enumValArray)
                {
                    enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
                }

                return enumValList;
            }
        }

	}
}