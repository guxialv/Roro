using System;
using System.Runtime.CompilerServices;

namespace Roro.Steps
{
    public sealed class Output<T>
    {
        public string Name { get; }

        public Type Type => typeof(T);

        public Output([CallerMemberName] string? name = null)
        {
            Name = name!;
        }
    }
}
