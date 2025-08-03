namespace Wholesaler.Frontend.Presentation.Views.Generic;

public abstract class Component<TOutput>
{
    public abstract TOutput Render();
}

public abstract class Component
{
    public abstract void Render();
}
