using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTreeEditor
{
    public interface Observable
    {
        void AddObserver(Observer obs);
        void RemoveObserver(Observer obs);
    }

    public interface Observer
    {
        void Notify(uint id);
    }
}
