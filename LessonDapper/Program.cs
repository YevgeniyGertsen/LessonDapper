using LessonDapper.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPlayerById(2);
            GetRposteryByTeamId(2);
            GetTeamRostersForSport(2);

            Console.WriteLine("----------------------");

            GetPlayerById(3);
            GetRposteryByTeamId(3);
            GetTeamRostersForSport(3);
        }

        public static void GetPlayerById(int id)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = new SportContextEfCore())
            {
                var playerId = db.Player.Find(id);
            }

            sw.Stop();
            Console.WriteLine("GetPlayerById: " +
                sw.ElapsedMilliseconds);
        }

        public static void GetRposteryByTeamId(int teamlId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = new SportContextEfCore())
            {
                var teamRoster = db.Team
                    .Include("Player")
                    .Single(s => s.TeamId == teamlId);
            }

            sw.Stop();
            Console.WriteLine("GetRposteryByTeamId: " +
                sw.ElapsedMilliseconds);
        }

        public static void GetTeamRostersForSport(int sportId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = new SportContextEfCore())
            {
                var players = db.Team.Include("Player")
                    .Where(w => w.SportId == sportId).ToList();
            }

            sw.Stop();
            Console.WriteLine("GetTeamRostersForSport: "+
                sw.ElapsedMilliseconds);
        }


        public static void GetPlayerById_(int id)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = 
                new SportContextEfCore())
            {
                var playerId = db.Player.Find(id);
            }

            sw.Stop();
            Console.WriteLine("GetPlayerById: " +
                sw.ElapsedMilliseconds);
        }

        public static void GetRposteryByTeamId_(int teamlId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = new SportContextEfCore())
            {
                var teamRoster = db.Team
                    .Include("Player").AsNoTracking()
                    .Single(s => s.TeamId == teamlId);
            }

            sw.Stop();
            Console.WriteLine("GetRposteryByTeamId: " +
                sw.ElapsedMilliseconds);
        }

        public static void GetTeamRostersForSport_(int sportId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SportContextEfCore db = new SportContextEfCore())
            {
                var players = db.Team
                    .Include("Player").AsNoTracking()
                    .Where(w => w.SportId == sportId).ToList();
            }

            sw.Stop();
            Console.WriteLine("GetTeamRostersForSport: " +
                sw.ElapsedMilliseconds);
        }
    }

}
