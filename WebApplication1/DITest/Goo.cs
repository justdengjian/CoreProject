using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DITest
{
    public class Goo : IFoo
    {
        public string GetInputString(string input)
        {
            return $"Goo输入的字符串为：{ input }";
        }
    }
}
