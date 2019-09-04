using System;
using System.Collections.Generic;
using System.Text;

namespace Automapper.Extensions {
    public interface ITypeAdapterFactory {

        ITypeAdapter Create();
    }
}
