using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Lista_de_Tarefas_Avalonia.Janelas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace Lista_de_Tarefas_Avalonia.SQLite
{
    internal class TarefaContext
    {
        string NomeDaListaSemEspacos = NomeDaListaDialogo.NomeDaLista.Replace(" ", "_");
        string ConnectionString = $"Data Source=Tarefas-Salvas/Tarefas.db;Version=3;";
        public TarefaContext()
        {

        }

        public void SalvarTarefas()
        {
            var AcessarJanelaPrincipal = (ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
            var JanelaPrincipal = AcessarJanelaPrincipal.MainWindow as MainWindow;

            if (JanelaPrincipal != null)
            {
                using (var Connection = new SQLiteConnection(ConnectionString))
                {
                    Connection.Open();
                    string SQLcreateTable = $"CREATE TABLE IF NOT EXISTS {NomeDaListaSemEspacos} (ID INTEGER PRIMARY KEY AUTOINCREMENT, Descricao TEXT, StatusID INTEGER)";
                    SQLiteCommand CommandCreateTable = new SQLiteCommand(SQLcreateTable, Connection);
                    CommandCreateTable.ExecuteNonQuery();

                    foreach (Tarefa tarefa in JanelaPrincipal.ListaDeTarefas)
                    {
                        string SQLinsert = $"INSERT INTO {NomeDaListaSemEspacos} (Descricao, StatusID) VALUES (@Desc, @St)";
                        SQLiteCommand CommandInsert = new(SQLinsert, Connection);
                        CommandInsert.Parameters.AddWithValue("@Desc", tarefa.Descricao);
                        CommandInsert.Parameters.AddWithValue("@St", tarefa.StatusID);
                        CommandInsert.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}