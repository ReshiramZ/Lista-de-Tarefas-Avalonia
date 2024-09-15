using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Lista_de_Tarefas_Avalonia.Janelas;
using Lista_de_Tarefas_Avalonia.SQLite;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Tmds.DBus.Protocol;

namespace Lista_de_Tarefas_Avalonia
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Tarefa> ListaDeTarefas { get; set; } = new();

        public static string TabelaEscolhida;
        public MainWindow()
        {
            InitializeComponent();
            ListBoxTarefa.ItemsSource = ListaDeTarefas;
            Directory.CreateDirectory("Tarefas-Salvas");
            DataContext = this;
            BarraDePesquisa.janelaPrincipal = this;
        }

        private void BtnAdicionar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Tarefa NovaTarefa = new();
            ListaDeTarefas.Add(NovaTarefa);
        }

        private async void BtnDeletar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ListBoxTarefa.SelectedItem != null)
            {
                var ConfirmarExcluir = await MessageBoxManager.GetMessageBoxCustom(
                new MsBox.Avalonia.Dto.MessageBoxCustomParams
                {
                    ButtonDefinitions = new List<ButtonDefinition>
                    {
                        new ButtonDefinition {Name = "Sim"},
                        new ButtonDefinition {Name = "Cancelar"},
                    },
                    ContentTitle = "Confirmar exclusão",
                    ContentMessage = "Você tem certeza que deseja excluir essa tarefa?",
                    Icon = MsBox.Avalonia.Enums.Icon.Question,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    ShowInCenter = true,

                }).ShowWindowDialogAsync(this);

                if (ConfirmarExcluir == "Sim")
                {
                    ListaDeTarefas.Remove((Tarefa)ListBoxTarefa.SelectedItem);
                }
            }
        }

        private void BtnSalvar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ComboBoxSalvamento.SelectedIndex == 0)
            {
                if (ListaDeTarefas.Count > 0)
                {
                    var NomeDaLista = new NomeDaListaDialogo();
                    NomeDaLista.ShowDialog(this);
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("A lista não pode estar vazia.",
                    "Você não pode salvar uma lista se a mesma estiver vazia.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error,
                    WindowStartupLocation.CenterScreen).ShowWindowDialogAsync(this);
                }
            }
            else if (ComboBoxSalvamento.SelectedIndex == 1)
            {
                if (ListaDeTarefas.Count > 0)
                {
                    var SelecionarLista = new SelecionarListaDialogo(this);
                    SelecionarLista.BtnTarefaEscolhida.Content = "Sobrescrever";
                    SelecionarLista.ShowDialog(this);
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("Não foi possível Sobrescrever.",
                    "Não é possível sobrescrever uma lista com o conteúdo atual, pois o mesmo está vazio.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error,
                    WindowStartupLocation.CenterScreen).ShowWindowDialogAsync(this);
                }
            }
            else if (ComboBoxSalvamento.SelectedIndex == 2)
            {
                if (ListaDeTarefas.Count > 0 && TabelaEscolhida != null) 
                {
                    string ConnectionString = "Data Source=Tarefas-Salvas/Tarefas.db;Vesion=3;";

                    using (var Connection = new SQLiteConnection(ConnectionString))
                    {
                        Connection.Open();

                        string DropTableSQL = $"DROP TABLE IF EXISTS {TabelaEscolhida}";
                        string CreateTableSQL = $"CREATE TABLE IF NOT EXISTS {TabelaEscolhida} (ID INTEGER PRIMARY KEY AUTOINCREMENT, Descricao TEXT, StatusID INTEGER)";

                        using (var DropCommand = new SQLiteCommand(DropTableSQL, Connection))
                        {
                            DropCommand.ExecuteNonQuery();
                        }
                        using (var CreateCommand = new SQLiteCommand(CreateTableSQL, Connection))
                        {
                            CreateCommand.ExecuteNonQuery();
                        }

                        foreach (Tarefa tarefa in ListaDeTarefas)
                        {

                            string InsertSQL = $"INSERT INTO {TabelaEscolhida} (Descricao, StatusID) VALUES (@Des, @St)";

                            SQLiteCommand InsertCommand = new(InsertSQL, Connection);
                            InsertCommand.Parameters.AddWithValue("@Des", tarefa.Descricao);
                            InsertCommand.Parameters.AddWithValue("@St", tarefa.StatusID);

                            InsertCommand.ExecuteNonQuery();
                        }
                        MessageBoxManager.GetMessageBoxStandard("",
                        "A lista foi salva com sucesso!.",
                        MsBox.Avalonia.Enums.ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Success,
                        WindowStartupLocation.CenterScreen).ShowWindowDialogAsync(this);
                        ComboBoxSalvamento.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("A lista não pode estar vazia e tem que estar em uma já criada.",
                    "Para salvar rápido, precisasse colocar conteúdo e estar em uma lista já salva.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error,
                    WindowStartupLocation.CenterScreen).ShowWindowDialogAsync(this);
                }
            }
        }

        private void BtnCarregar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var CarregarTarefa = new SelecionarListaDialogo(this);
            CarregarTarefa.ShowDialog(this);
        }
    }
}
