using MauiAppMinhasCompras.ViewModels;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
        BindingContext = new RelatorioViewModel();
    }
}