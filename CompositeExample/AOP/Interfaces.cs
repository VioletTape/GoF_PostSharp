namespace CompositeExample.AOP {
    public interface IComposite : IComponent {
        void Add(IComponent component);
    }

    public interface IComponent {}
}