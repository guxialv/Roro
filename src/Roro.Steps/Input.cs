using System;
using System.Runtime.CompilerServices;

namespace Roro.Steps
{
    public sealed class Input<T>
    {
        public string Name { get; }

        public Type Type => typeof(T);

        public Input([CallerMemberName] string? name = null)
        {
            Name = name!;
        }
    }
}
