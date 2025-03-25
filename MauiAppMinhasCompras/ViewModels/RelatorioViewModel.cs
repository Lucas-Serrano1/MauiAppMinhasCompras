using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppMinhasCompras.ViewModels;

public class RelatorioViewModel : BindableObject
{
    private DateTime _dataInicio = DateTime.Now.AddDays(-7);
    private DateTime _dataFim = DateTime.Now;
    private double _totalGasto;
    private ObservableCollection<Produto> _produtosFiltrados = new();

    public DateTime DataInicio
    {
        get => _dataInicio;
        set
        {
            _dataInicio = value;
            OnPropertyChanged();
        }
    }

    public DateTime DataFim
    {
        get => _dataFim;
        set
        {
            _dataFim = value;
            OnPropertyChanged();
        }
    }

    public double TotalGasto
    {
        get => _totalGasto;
        set
        {
            _totalGasto = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Produto> ProdutosFiltrados
    {
        get => _produtosFiltrados;
        set
        {
            _produtosFiltrados = value;
            OnPropertyChanged();
        }
    }

    public ICommand FiltrarCommand => new Command(async () => await CarregarDados());

    private async Task CarregarDados()
    {
        try
        {
            var produtos = await App.Db.GetByDateRange(DataInicio, DataFim);
            ProdutosFiltrados = new ObservableCollection<Produto>(produtos);

            TotalGasto = await App.Db.GetTotalSpentByDateRange(DataInicio, DataFim);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}