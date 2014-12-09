using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;

namespace CompositeExample {
    internal class Program {
        private static void Main(string[] args) {
            var topLevel1 = new TopLevel();

            topLevel1.Add(new LowLevel2());
            Console.WriteLine(topLevel1.Count());

            topLevel1.Add(new MiddleLevel());
            Console.WriteLine(topLevel1.Count());


            Console.ReadLine();
        }
    }


    public interface IComposite : IComponent {
        void Add(IComponent component);
    }

    public interface IComponent {}














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


    [Serializable]
    public class GlobalNodeSearch : AssemblyLevelAspect {
        private readonly string interfaceName;

        public GlobalNodeSearch(string interfaceName) {
            this.interfaceName = interfaceName;
        }

        public override void CompileTimeInitialize(Assembly assembly, AspectInfo aspectInfo) {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterface(interfaceName) != null && t.IsClass)
                .Select(t => t);

            var refs = new Dictionary<Type, List<Type>>();

            foreach (var type in types) {
                var attributes = type.GetCustomAttributes(typeof (NodeAttribute), false).SingleOrDefault();
                if (attributes == null) {
                    continue;
                }

                var parent = ((NodeAttribute) attributes).Parent;

                if (parent == null) {
                    continue;
                }

                if (!refs.ContainsKey(parent)) {
                    refs.Add(parent, new List<Type>());
                }

                refs[parent].Add(type);
            }

            NodeAttribute.TotalElements = types.Count();
            NodeAttribute.Refs = refs;

            base.CompileTimeInitialize(assembly, aspectInfo);
        }
    }

    [Serializable]
    public class NodeAttribute : TypeLevelAspect, IAspectProvider {
        public static int TotalElements;
        public static Dictionary<Type, List<Type>> Refs = new Dictionary<Type, List<Type>>();

        public Type Parent { get; set; }

        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo) {
            base.CompileTimeInitialize(type, aspectInfo);
        }

        public IEnumerable<AspectInstance> ProvideAspects(object targetElement) {
            var type = ((Type) targetElement);

            var methodInfo = type.GetMethod("Add");
            if (methodInfo != null && Refs.ContainsKey(type)) {
                yield return new AspectInstance(methodInfo, new NodeControl(Refs[type]));
            }
        }
    }

    [Serializable]
    public class NodeControl : MethodInterceptionAspect {
        private readonly List<Type> acceptableTypes;

        public NodeControl(List<Type> acceptableTypes) {
            this.acceptableTypes = acceptableTypes;
        }

        public override void OnInvoke(MethodInterceptionArgs args) {
            var type = args.Arguments.GetArgument(0).GetType();
            if (acceptableTypes.Contains(type)) {
                base.OnInvoke(args);
                return;
            }

            var message = string.Format("{0}.{1} can not accept {2}", args.Instance.GetType().Name, args.Method.Name, type.Name);
            throw new InvalidDataException(message);
        }

        public override bool CompileTimeValidate(MethodBase method) {
            var parameterInfos = method.GetParameters();
            return base.CompileTimeValidate(method);
        }
    }
}