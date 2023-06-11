using System;
using System.Collections.Generic;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    public interface ISASolvableProblem : IProblem
    {
        float Temperature(int t, float T_base, float epsilon);
        object RandomNeighbour(object solution);
        object DeepCopy(object solution);
    }
}
