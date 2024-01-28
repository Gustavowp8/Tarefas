using System;
using System.Threading;
using Tarefas.db;

namespace Tarefas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Conectado ao MySQL");
            Thread.Sleep(1000);
            Menu();
        }

        static void Menu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***** MENU *****");
            Console.ResetColor();

            Console.WriteLine();

            Console.WriteLine("1 - Ver lista de tarefas");
            Console.WriteLine("2 - Ver lista de pendentes");
            Console.WriteLine("3 - Pesquisar por descrição");
            Console.WriteLine("4 - Pesquisar por ID");
            Console.WriteLine("5 - Incluir nova tarefa");
            Console.WriteLine("6 - Editar tarefa");
            Console.WriteLine("7 - CONCLUIR TAREFA");
            Console.WriteLine("8 - Excluir Tarefa");
            Console.WriteLine("0 - SAIR");
            Console.WriteLine();

            Console.Write("Digite o numero da opção: ");
            short opc = short.Parse(Console.ReadLine());

            switch (opc)
            {
                case 1: Ver(); break;
                case 2: Pendente(); break;
                case 3: Descricao(); break;
                case 4: PesqquisaId(); break;
                case 5: Incluir(); break;
                case 6: Editar(); break;
                case 7: Concluir(); break;
                case 8: Excluir(); break;
                default: ErroConsole(); break;
                case 0: Sair(); break;
            }
        }

        static void Ver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***** VER LISTA DE TAREFAS *****");
            Console.ResetColor();
            Console.WriteLine();

            using (var db = new TarefasContext())
            {
                var tarefas = db.Listatarefas.ToList<Listatarefas>();

                Console.WriteLine($"{tarefas.Count()} tarefas encontradas");
                Console.WriteLine();

                foreach (var item in tarefas)
                {
                    Console.WriteLine($"[{(item.Concluido ?  "X" : " ")}] #{item.Id}: {item.Descricao}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void Pendente()
        {
            Console.Clear();
            Console.WriteLine("***** VER LISTA DE PENDENTES *****");
            Console.WriteLine();

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas
                .OrderByDescending(x => x.Id)
                .Where(x => !x.Concluido)
                .ToList<Listatarefas>();

                foreach (var t in tarefa)
                {
                    Console.WriteLine($"[{(t.Concluido ? "X" : " ")}] #{t.Id} {t.Descricao}");
                }
            }

                Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void Descricao()
        {
            Console.Clear();
            Console.WriteLine("***** PESQUISA POR DESCRIÇÃO *****");
            Console.WriteLine();

            Console.Write("Digite a descrição: ");
            string descrcao = Console.ReadLine();

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas
                    .Where(x => x.Descricao.Contains(descrcao))
                    .OrderBy(x => x.Descricao)
                    .ToList<Listatarefas>();

                foreach (var t in tarefa)
                {
                    Console.WriteLine($"[{(t.Concluido ? "X" : " ")}] #{t.Id} {t.Descricao}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void PesqquisaId()
        {
            Console.Clear();
            Console.WriteLine("***** PESQUISAR POR ID *****");
            Console.WriteLine();

            Console.Write("Digite o ID: ");
            int id = int.Parse(Console.ReadLine());

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas.Find(id);

                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");

                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                    Console.ReadKey();
                    Menu();

                    return;
                }

                Console.WriteLine($"[{(tarefa.Concluido ? "X" : " ")}] #{tarefa.Id} {tarefa.Descricao}");
            }

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void Incluir()
        {
            Console.Clear();
            Console.WriteLine("***** INCLUIR UMA NOVA TAREFA *****");
            Console.WriteLine();

            Console.Write("Digite a descrição da tarefa: ");
            string descricao = Console.ReadLine();

            if (String.IsNullOrEmpty(descricao))
            {
                Console.WriteLine("Não e possivel incluir tarefa sem descrição.");

                Console.WriteLine();
                Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                Console.ReadKey();
                Menu();
                return;
            }

            using (var db = new TarefasContext())
            {
                var tarefa = new Listatarefas
                {
                    Descricao = descricao
                };
                db.Add(tarefa);
                db.SaveChanges();
            }

            Console.WriteLine();
            Console.WriteLine("TAREFA SALVA COM SUCESSO!!! Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void Editar()
        {

            Console.Clear();
            Console.WriteLine("***** EDITAR TAREFA *****");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Digite a nova descrição: ");
            string descricao = Console.ReadLine();
            Console.WriteLine();

            if (String.IsNullOrEmpty(descricao))
            {
                Console.WriteLine("ERRO! Não e permitido deixar uma descrição vazia.");
                return;
            }

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas.Find(id);

                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");
                    return;
                }

                //Ateração
                tarefa.Descricao = descricao;
                db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluido ? "X" : " ")}] #{tarefa.Id} {tarefa.Descricao}");
            }

            

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();

        }

        static void Concluir()
        {
            Console.Clear();
            Console.WriteLine("***** CONCLUIR TAREFA *****");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa: ");
            int id = int.Parse(Console.ReadLine());

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas.Find(id);

                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");
                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                    Console.ReadKey();
                    Menu();
                    return;
                }

                if (tarefa.Concluido)
                {
                    Console.WriteLine("Tarafa já esta concluido!");
                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                    Console.ReadKey();
                    Menu();
                    return;
                }

                tarefa.Concluido = true;
                db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluido ? "X" : " ")}] #{tarefa.Id} {tarefa.Descricao}");
            }

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void Sair()
        {
            Console.Clear();
            Console.WriteLine("Obrigado por usar o nosso software!");
            Console.WriteLine();
        }

        static void Excluir()
        {
            Console.Clear();
            Console.WriteLine("***** EXCLUIR TAREFA *****");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa que deseja EXCLUIR: ");
            int id = int.Parse(Console.ReadLine());

            using (var db = new TarefasContext())
            {
                var tarefa = db.Listatarefas.Find(id);

                if (tarefa == null)
                {
                    Console.WriteLine("Tarafa não encontrada!");
                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                    Console.ReadKey();
                    Menu();
                    return;
                }

                //Excluir
                db.Listatarefas.Remove(tarefa);
                db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluido ? "X" : " ")}] #{tarefa.Id} {tarefa.Descricao} EXCLUIDA COM SUCESSO!");
                Console.WriteLine();
                Console.WriteLine("Aperte qualquer botão para volta ao MENU");
                Console.ReadKey();
                Menu();
            }

            

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }

        static void ErroConsole()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Esta opção não existe no menu!");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Aperte qualquer botão para volta ao MENU");
            Console.ReadKey();
            Menu();
        }
    }
}