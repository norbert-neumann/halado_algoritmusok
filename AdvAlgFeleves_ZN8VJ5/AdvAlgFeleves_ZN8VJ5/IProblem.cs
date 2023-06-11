using System;
using System.Collections.Generic;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    public interface IProblem
    {
        object CreateNewSolution();
        float Fitness(object solution);
        object GenotypePhenotypeMapping(object solution);
    }
}
