using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static string connectionStrings = @"data source=206-13\MSSQLSERVER99;initial catalog=practice;integrated security=True;";
        static void Main(string[] args)
        {
            Task9();
        }

        public static void GetPlayerById(int id)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var player = con.QuerySingle<Player>
                    ("select * from Player where PlayerId = @id",
                    new { id = id });
            }

            sw.Stop();
            Console.WriteLine("GetPlayerById dapper: " +
                sw.ElapsedMilliseconds);
        }
        public static void GetRposteryByTeamId(int teamlId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();
                var team = con.QuerySingle<Team>
                    ("select * from Team where TeamId = @teamlId",
                    new { teamlId = teamlId });

                team.Player = con.Query<Player>
                    ("select * from Player where TeamId = @teamlId",
                    new { teamlId = teamlId });
            }

            sw.Stop();
            Console.WriteLine("GetRposteryByTeamId dapper: " +
                sw.ElapsedMilliseconds);
        }
        public static void GetTeamRostersForSport(int sportId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var teams = con.Query<Team>
                    ("select * from Team where SportId = @sportId",
                    new { sportId = sportId });

                var teamIDs = teams.Select(s => s.TeamId).ToList();

                var players = con.Query<Player>
                    ("select * from Player where TeamId in @teamIDs",
                    new { teamIDs = teamIDs });

                foreach (var team in teams)
                {
                    team.Player = players
                        .Where(w => w.TeamId == team.TeamId)
                        .ToList();
                }
            }

            sw.Stop();
            Console.WriteLine("GetTeamRostersForSport dapper: " +
                sw.ElapsedMilliseconds);
        }

        public async Task Exmpl01()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                List<Sport> sports = new List<Sport>();

                con.Execute("update Sport set Name=@Name" +
                    " where SportId = @SportId", sports);

                await con.ExecuteAsync(
                    new CommandDefinition("update Sport set Name=@Name" +
                    " where SportId = @SportId", sports,
                    flags: CommandFlags.Pipelined));

            }
        }

        public static void Task2()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                IEnumerable<dynamic> data = con.Query("select  1 as A, 2 as B");
                var first = data.First();

                int a = (int)first.A;
                int b = (int)first.B;
            }
        }

        public static void Exmpl3(string color, int age)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var query = "select * from CATS " +
                    "where Color = :Color and Age > :Age";

                var dynParams = new DynamicParameters();
                dynParams.Add("Color", color);
                dynParams.Add("Age", age);

                var result = con.Query(query, dynParams);
            }
        }

        public static void Exmpl3(DynamicParameters dynParams)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var query = "select * from CATS " +
                    "where Color = :Color and Age > :Age";

                var result = con.Query(query, dynParams);
            }
        }

        public static void Exmpl3<T>(DynamicParameters dynParams, string query)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var result = con.Query<T>(query, dynParams);
            }
        }

        public void Task4()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var query = "select * from Player where PlayerId = @PlayerId;" +
                            "select * from Sport where SportId = @SportId;";

                using (var multi = con.QueryMultiple(query,
                    new { PlayerId = 1, SportId = 1 }))
                {
                    var player = multi.Read<Player>().FirstOrDefault();
                    var sport = multi.Read<Sport>().FirstOrDefault();
                }
            }
        }

        public static void Task5(string name)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                int result = con.Execute("AddSport", new { Name = name },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public static void Task6()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var teams = con.Query<Team>("GetTeam",
                    new { SportId = 1 },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public static void Task7()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();
                using (SqlTransaction trn = con.BeginTransaction())
                {
                    try
                    {
                        var query = "update Sport set Name=@Name where SportId = 4";
                        var parametrs = new { Name = "Теннис" };
                        con.Execute(query, parametrs, trn);

                        // throw new Exception();
                        trn.Commit();
                    }
                    catch (Exception ex)
                    {
                        trn.Rollback();
                    }
                }
            }
        }

        public async Task Task8(IEnumerable<Widget> widgets)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                await con.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.BulkCopyTimeout = 100;
                    bulkCopy.BatchSize = 500;
                    bulkCopy.DestinationTableName = "Widget";
                    bulkCopy.EnableStreaming = true;

                    using (IDataReader dataReader = widgets as IDataReader)
                    {
                        //await bulkCopy.WriteToServer(dataReader);
                    }
                }
            }
        }



        public static void Task9()
        {
            using (SqlConnection con = new SqlConnection(connectionStrings))
            {
                con.Open();

                var query = "select * from Player where PlayerId = @PlayerId;";
                var data = con.Query<Player>(query, new { PlayerId = 1 });

                var data3 = con.Query<Sport>("select s.SportId, s.Name from Sport s left join Team t on s.SportId = t.SportId");


                //c QueryObject
                SelectPlayer sp = new SelectPlayer();
                var data2 = con.Query<Player>(sp.All());

            }
        }
    }

    public class SelectPlayer
    {
        public QueryObject All()
        {
            return new QueryObject("select * from Player", null);
        }
        public QueryObject GetPlayerById(int playerId)
        {
            return new QueryObject("select * from Player where PlayerId = @PlayerId",
                new { PlayerId = playerId });
        }
    }





    public class Widgets : List<Widget>
    {

    }

    public class Widget
    {
        public int WidgetId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}