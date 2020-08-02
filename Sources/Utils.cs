using System;
using System.Collections.Generic;
using System.Text;

namespace Derivas.Utils
{
    internal interface IInstanceInfo<Base>
    {
        Type GetType();
        Base CreateInstance(params object[] args);
    }
}
