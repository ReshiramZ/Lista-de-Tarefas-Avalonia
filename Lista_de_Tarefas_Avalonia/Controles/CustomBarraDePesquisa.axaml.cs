using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lista_de_Tarefas_Avalonia.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lista_de_Tarefas_Avalonia;

public partial class CustomBarraDePesquisa : UserControl
{
    public MainWindow janelaPrincipal;
    private IEnumerable<Tarefa> Filtro;
    public CustomBarraDePesquisa()
    {
        InitializeComponent();
    }

    public CustomBarraDePesquisa(MainWindow JanelaPrincipal)
    {
        janelaPrincipal = JanelaPrincipal;
    }

    private void TextBoxBarraDePesquisa_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        Filtrar();
    }
    private void ComboBoxFiltro_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (TextBoxBarraDePesquisa != null)
        {
            Filtrar();
        }
    }

    private void BtnLimpar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TextBoxBarraDePesquisa.Text = null;
        janelaPrincipal.ListBoxTarefa.ItemsSource = janelaPrincipal.ListaDeTarefas;
    }

    public void Filtrar()
    {
        Filtro = janelaPrincipal.ListaDeTarefas;

        // Se puder resumir ou otimizar o código, mande um Issue no Github.
        if (!String.IsNullOrWhiteSpace(TextBoxBarraDePesquisa.Text))
        {
            string? Pesquisa = TextBoxBarraDePesquisa.Text.ToLower();
            Filtro = janelaPrincipal.ListaDeTarefas.Where(T => !String.IsNullOrWhiteSpace(T.Descricao) && T.Descricao.ToLower().Contains(Pesquisa));
        }
        if (ComboBoxFiltro.SelectedIndex != 0)
        {
            int FiltroStatus = ComboBoxFiltro.SelectedIndex - 1;
            Filtro = janelaPrincipal.ListaDeTarefas.Where(T => T.StatusID == FiltroStatus);
        }
        if (!String.IsNullOrWhiteSpace(TextBoxBarraDePesquisa.Text) && ComboBoxFiltro.SelectedIndex != 0)
        {
            string? Pesquisa = TextBoxBarraDePesquisa.Text.ToLower();
            int FiltroStatus = ComboBoxFiltro.SelectedIndex - 1;
            Filtro = janelaPrincipal.ListaDeTarefas.Where(T => !String.IsNullOrWhiteSpace(T.Descricao) && T.Descricao.ToLower().Contains(Pesquisa) && T.StatusID == FiltroStatus);
        }

        if (Filtro.Any())
        {
            janelaPrincipal.ListBoxTarefa.ItemsSource = Filtro;
        }
        else
        {
            janelaPrincipal.ListBoxTarefa.ItemsSource = janelaPrincipal.ListaDeTarefas;
        }
    }
}