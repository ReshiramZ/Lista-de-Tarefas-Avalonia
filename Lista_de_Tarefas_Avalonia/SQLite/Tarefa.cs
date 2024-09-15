using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_Tarefas_Avalonia.SQLite
{
    public class Tarefa
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public int StatusID { get; set; }
    }
}
