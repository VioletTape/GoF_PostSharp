using System.Collections.Generic;

namespace CompositeExample.AOP {
    [Node]
    public class TopLevel : IComposite {
        private readonly List<IComponent> items = new List<IComponent>();

        public void Add(IComponent component) {
            items.Add(component);
        }

        public int Count() {
            return items.Count;
        }
    }

    [Node(Parent = typeof (TopLevel))]
    public class MiddleLevel : IComposite {
        private List<IComponent> items = new List<IComponent>();

        public void Add(IComponent component) {
            // implementation
        }
    }

    [Node(Parent = typeof (MiddleLevel))]
    public class LowLevel1 : IComponent {}

    [Node(Parent = typeof (TopLevel))]
    public class LowLevel2 : IComponent {}

    public class LowLevel3 : IComponent {}
}