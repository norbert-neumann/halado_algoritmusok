using System;
using System.Collections.Generic;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    public interface IHCSolvableProblem : IProblem
    {
        object RandomNeighbour(object solution);
    }
}
