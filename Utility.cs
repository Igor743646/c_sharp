using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTranslator
{
    
    public static class Utility
    {
        public static string convert(ArrayList arrayList)
        {
            var strings = arrayList.Cast<string>().ToArray();
            var theString = string.Join(" ", strings);
            return theString;
        }

        public static string convertInt(ArrayList arrayList)
        {
            var strings = arrayList.Cast<Int32>().ToArray();
            var theString = string.Join(" ", strings);
            return theString;
        }

        public static bool IsSameArrayList(ArrayList lar, ArrayList rar)
        {
            if (lar.Count != rar.Count)
                return false;

            for (int i = 0; i < lar.Count; i++)
            {
                if (lar[i] != rar[i])
                    return false;
            }

            return true;
        }
    }
}
