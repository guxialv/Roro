using System;
using System.Collections.Generic;
using System.Text;

namespace Roro.Steps
{
    public interface IExecutionContext
    {
        public T Get<T>(Input<T> input);

        public T Set<T>(Output<T> output, T value);
    }
}
