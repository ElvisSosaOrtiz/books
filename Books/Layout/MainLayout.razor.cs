namespace Books.Layout
{
    using Microsoft.AspNetCore.Components;

    public partial class MainLayout
    {
        [Inject] private NavigationManager NavManager { get; set; } = null!;
    }
}
