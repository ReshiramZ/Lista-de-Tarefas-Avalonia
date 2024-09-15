using Avalonia.Controls;
using Avalonia.Metadata;
using Lista_de_Tarefas_Avalonia.SQLite;
using MsBox.Avalonia;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lista_de_Tarefas_Avalonia.Janelas
{
    public partial class NomeDaListaDialogo : Window
    {
        public static string NomeDaLista;
        private static string NomeValidoSQL = @"^[a-zA-Z_][\sa-zA-Z0-9_]*$";
        private string[] PalavrasSQL =
        {
            "SELECT", "INSERT", "UPDATE",
            "DELETE", "FROM", "WHERE", 
            "JOIN", "CREATE", "DROP",
            "ALTER", "TABLE", "DATABASE", 
            "AND", "OR", "NOT", "NULL",
                     "LIKE"
        };
        private Regex Validador = new(NomeValidoSQL);
        public NomeDaListaDialogo()
        {
            InitializeComponent();
        }

        private void BtnConfirmarNome_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ConfirmarNome();
        }

        private void Window_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                ConfirmarNome();
            }
        }
        private void ConfirmarNome()
        {
            if (!String.IsNullOrWhiteSpace(EntradaDeNome.Text))
            {
                NomeDaLista = EntradaDeNome.Text;

                if (Validador.IsMatch(NomeDaLista) && !PalavrasSQL.Contains(NomeDaLista.ToUpper()))
                {
                    TarefaContext tarefaContext = new();
                    tarefaContext.SalvarTarefas();

                    MessageBoxManager.GetMessageBoxStandard("",
                    "A lista foi salva com sucesso!.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Success,
                    WindowStartupLocation.CenterScreen).ShowWindowAsync();

                    this.Close();
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("Erro ao colocar nome para a lista",
                    "O nome da lista é inválido, tente novamente.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error,
                    WindowStartupLocation.CenterScreen).ShowWindowDialogAsync(this);
                }
            }
        }
    }
}