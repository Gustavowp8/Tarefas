using System;
using System.Collections.Generic;

namespace Tarefas.db;

public partial class Listatarefas
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Concluido { get; set; }
}
