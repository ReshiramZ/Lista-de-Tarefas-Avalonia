using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Lista_de_Tarefas_Avalonia.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Formats.Tar;
using System.Linq;
using System.Threading.Tasks;

namespace Lista_de_Tarefas_Avalonia;

public partial class SelecionarListaDialogo : Window
{
    private MainWindow janelaPrincipal;
    public SelecionarListaDialogo(MainWindow JanelaPrincipal)
    {
        InitializeComponent();
        janelaPrincipal = JanelaPrincipal;
        MostrarListasDeTarefasNomes();
    }

    private void MostrarListasDeTarefasNomes()
    {
        string ConnectionString = "Data Source=Tarefas-Salvas/Tarefas.db;Vesion=3;";

        using (var Connection = new SQLiteConnection(ConnectionString))
        {
            var Tabelas = new List<string>();
            Connection.Open();

            string SQL = "SELECT name FROM sqlite_master WHERE type='table' AND name != 'sqlite_sequence';";
            var Command = new SQLiteCommand(SQL, Connection);
            using (var Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    Tabelas.Add(Reader["name"].ToString());
                }
            }
            var TabelasAZ = Tabelas.Order();
            ListBoxListas.ItemsSource = TabelasAZ;
        }
    }

    private void BtnTarefaEscolhida_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (BtnTarefaEscolhida.Content == "Sobrescrever" && BtnTarefaEscolhida.Content != null)
        {
            SobrescreverLista();
        }
        else
        {
            CarregarLista();
        }
        janelaPrincipal.BarraDePesquisa.TextBoxBarraDePesquisa.Text = null;
        janelaPrincipal.BarraDePesquisa.ComboBoxFiltro.SelectedIndex = 0;
        this.Close();

    }

    private void CarregarLista()
    {
        var TabelaEscolhida = ListBoxListas.SelectedItem as string;
        MainWindow.TabelaEscolhida = TabelaEscolhida;
        if (!String.IsNullOrEmpty(TabelaEscolhida))
        {
            string ConnectionString = "Data Source=Tarefas-Salvas/Tarefas.db;Vesion=3;";

            using (var Connection = new SQLiteConnection(ConnectionString))
            {
                janelaPrincipal.ListaDeTarefas.Clear();
                Connection.Open();

                string Query = $"SELECT * FROM {TabelaEscolhida}";

                var Command = new SQLiteCommand(Query, Connection);

                using (var Reader = Command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        Tarefa tarefa = new();

                        tarefa.ID = Convert.ToInt32(Reader["ID"]);
                        tarefa.Descricao = Reader["Descricao"].ToString();
                        tarefa.StatusID = Convert.ToInt32(Reader["StatusID"]);
                        janelaPrincipal.ListaDeTarefas.Add(tarefa);
                    }
                }
            }
        }
    }
    private void SobrescreverLista()
    {
        var TabelaEscolhida = ListBoxListas.SelectedItem as string;
        MainWindow.TabelaEscolhida = TabelaEscolhida;
        if (!String.IsNullOrEmpty(TabelaEscolhida))
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

                foreach (Tarefa tarefa in janelaPrincipal.ListaDeTarefas)
                {

                    string InsertSQL = $"INSERT INTO {TabelaEscolhida} (Descricao, StatusID) VALUES (@Des, @St)";

                    SQLiteCommand InsertCommand = new(InsertSQL, Connection);
                    InsertCommand.Parameters.AddWithValue("@Des", tarefa.Descricao);
                    InsertCommand.Parameters.AddWithValue("@St", tarefa.StatusID);

                    InsertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}