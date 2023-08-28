using System;
using Tarefas.data;
using System.Threading;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace Tarefas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Função tarefas");
            Console.WriteLine();

            int nTarafas = 0;

            using (var _db = new TarefasContext())
            {
                var tarefas = _db.Tarefa;

                nTarafas = tarefas.Count();
            }

            Console.WriteLine($"Tarefas cadastradas {nTarafas}");
            Console.WriteLine();
            Console.WriteLine("Carregando o Menu...");
            Thread.Sleep(3500);
            Menu();

        }

        static void Menu()
        {
            Console.Clear();

            Console.WriteLine("****** MENU ******");
            Console.WriteLine();
            Console.WriteLine("01 - Listar todas as tarefas");
            Console.WriteLine("02 - Tarefas pendenstes");
            Console.WriteLine("03 - Lista por Descrição");
            Console.WriteLine("04 - Procurar por ID");
            Console.WriteLine("05 - Criar nova tarefa");
            Console.WriteLine("06 - Alterar descrição");
            Console.WriteLine("07 - Concluir tarefa");
            Console.WriteLine("08 - Excluir tarefa");
            Console.WriteLine("09 - Sair do programa");

            Console.WriteLine("_______________________");
            Console.Write("Digite o n° da opção: ");
            short res = short.Parse(Console.ReadLine());

            switch (res)
            {
                case 1: Lista(); break;
                case 2: ListaPendentes(); break;
                case 3: ListaPorDescricao(); break;
                case 4: PorId(); break;
                case 5: Criar(); break;
                case 6: AlterarDescricao(); break;
                case 7: Concluir(); break;
                case 8: Excluir(); break;
                case 9: Sair(); break;
                default: Menu(); break;
            }

        }

        static void Lista()
        {
            Console.Clear();

            Console.WriteLine("****** Lista de tarefas ******");
            Console.WriteLine();

            using (var _db = new TarefasContext())
            {
                var tarefas = _db.Tarefa.ToList<Tarefa>();

                Console.WriteLine($"{tarefas.Count()} encontradas");
                Console.WriteLine();

                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
                }

                Console.WriteLine();
                Console.WriteLine("Precione ENTER para volta ao menu...");
                Console.ReadKey();
                Menu();
            }


        }

        static void ListaPendentes()
        {
            Console.Clear();

            Console.WriteLine("****** Lista de tarefas pendentes ******");
            Console.WriteLine();

            using (var _db = new TarefasContext())
            {
                var tarefas = _db.Tarefa
                    .Where(t => !t.Concluida)
                    .OrderBy(t => t.Id)
                    .ToList<Tarefa>();

                Console.WriteLine($"{tarefas.Count()} encontradas");
                Console.WriteLine();

                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
                }

                Console.WriteLine();
                Console.WriteLine("Precione ENTER para volta ao menu...");
                Console.ReadKey();
                Menu();
            }

        }

        static void ListaPorDescricao()
        {
            Console.Clear();
            Console.WriteLine("****** Lista por descrição ******");
            Console.WriteLine();

            Console.Write("Buscar por: ");
            string nome = Console.ReadLine();

            using (var _db = new TarefasContext())
            {
                var tarefas = _db.Tarefa
                    .Where(t => t.Descricao.Contains(nome))
                    .OrderBy(t => t.Descricao)
                    .ToList<Tarefa>();

                Console.WriteLine($"{tarefas.Count()} encontradas");
                Console.WriteLine();

                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
                }

            }


            Console.WriteLine();
            Console.WriteLine("Precione ENTER para volta ao menu...");
            Console.ReadKey();
            Menu();
        }

        static void PorId()
        {
            Console.Clear();
            Console.WriteLine("****** Buscar por ID ******");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa: ");
            int id = int.Parse(Console.ReadLine());

            using (var _db = new TarefasContext())
            {
                var tarefa = _db.Tarefa.Find(id);


                if (tarefa == null)
                {
                    Console.WriteLine($"Tarefa não encontrada");
                    Thread.Sleep(2500);
                    PorId();
                }

                Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
            }
            Console.WriteLine();
            Console.WriteLine("Precione ENTER para volta ao menu...");
            Console.ReadKey();
            Menu();
        }

        static void Criar()
        {
            Console.Clear();
            Console.WriteLine("****** Criar nova tarefa ******");
            Console.WriteLine();

            Console.Write("Digite a descrição da nova tarefa: ");
            string descticao = Console.ReadLine();

            if (String.IsNullOrEmpty(descticao))
            {
                Console.WriteLine("Error! Não e possivel inserir tarefa sem descrição");
                Thread.Sleep(2500);
                Menu();
            }

            using (var _db = new TarefasContext())
            {
                var tarefa = new Tarefa
                {
                    Descricao = descticao,
                    //Concluida = false,
                };
                _db.Tarefa.Add(tarefa);
                _db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");

            }

            Console.WriteLine();
            Console.WriteLine("Precione ENTER para volta ao menu...");
            Console.ReadKey();
            Menu();
        }

        static void AlterarDescricao()
        {
            Console.Clear();
            Console.WriteLine("****** Alterar descrição ******");
            Console.WriteLine();

            Console.Write("Digite o ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Digite a nova descrição: ");
            string descricao = Console.ReadLine();

            Console.WriteLine();

            if (String.IsNullOrEmpty(descricao))
            {
                Console.WriteLine("Não e permitido deixar uma tarefa sem descrição");
                Thread.Sleep(2500);
                Menu();
            }

            using (var _db = new TarefasContext())
            {
                var tarefa = _db.Tarefa.Find(id);
                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");
                    Thread.Sleep(2500);
                    Menu();
                }

                //alteração
                tarefa.Descricao = descricao;
                _db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
            }

            Console.WriteLine();
            Console.WriteLine("Precione ENTER para volta ao menu...");
            Console.ReadKey();
            Menu();

        }

        static void Concluir()
        {
            Console.Clear();
            Console.WriteLine("****** Concluir tarefa ******");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa a ser concluida: ");
            int id = int.Parse(Console.ReadLine());

            using (var _db = new TarefasContext())
            {
                var tarefa = _db.Tarefa.Find(id);
                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");
                    Thread.Sleep(2500);
                    Menu();
                }

                if (tarefa.Concluida)
                {
                    Console.WriteLine("Tarefa já concluida");
                }

                //alteração
                tarefa.Concluida = true;
                _db.SaveChanges();

                Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");

            }


            Console.WriteLine();
            Console.WriteLine("Precione ENTER para volta ao menu...");
            Console.ReadKey();
            Menu();
        }

        static void Excluir()
        {
            Console.Clear();
            Console.WriteLine("****** Excluir ******");
            Console.WriteLine();

            Console.Write("Digite o ID da tarefa a ser Excuida: ");
            int id = int.Parse(Console.ReadLine());

            using (var _db = new TarefasContext())
            {
                var tarefa = _db.Tarefa.Find(id);
                if (tarefa == null)
                {
                    Console.WriteLine("Tarefa não encontrada!");
                    Thread.Sleep(2500);
                    Menu();
                }

                //exclusão
                _db.Tarefa.Remove(tarefa);
                _db.SaveChanges();

                Console.WriteLine("Tarefa excluida");

                Console.WriteLine();
                Console.WriteLine("Precione ENTER para volta ao menu...");
                Console.ReadKey();
                Menu();

            }
        }

        static void Sair()
        {
            Console.Clear();
            Console.WriteLine("Obrigado por usar o programa! ");
        }
    }
}