using System;
using System.Collections.Generic;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    public interface IGASolvableProblem : IProblem
    {
        object Crossover(object[] solutions);
        object Mutate(object solution);
    }
}
