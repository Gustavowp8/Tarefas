﻿using System;
using System.Collections.Generic;

namespace Tarefas.data;

public partial class Tarefa
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Concluida { get; set; }
}
