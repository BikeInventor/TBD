using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.ViewModel.Utils
{
    public static class NameCutter
    {
        public static String MakeShorter(String fullName)
        {
            try
            {
                String result = "";
                var words = fullName.Split(' ');
                result += words[0] + " ";
                result += words[1].Substring(0, 1) + ".";
                result += words[2].Substring(0, 1) + ".";
                return result;
            }
            catch
            {
                return fullName;
            }
        }
    }
}
