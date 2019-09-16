using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Local
namespace TestUpdateDisconnectedEntity2
{
    /// <summary>
    /// Test program to evaluate the performance of updating a specific
    /// entity in an object graph.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var siteScenarios = FetchSiteScenarios();
            DisplaySiteScenarios(siteScenarios);

            var siteScenario = siteScenarios.First();
            var siteScenarioDatums = siteScenario.SiteScenarioDatums;
            DisplaySiteScenarioDatums(siteScenarioDatums);

            DisplaySiteEnrichments(siteScenarioDatums.First().SiteEnrichments);
        }

        private static List<SiteScenario> FetchSiteScenarios()
        {
            using (var context = new AppDbContext())
            {
                return context.SiteScenarios
                    .Include(x => x.SiteScenarioDatums)
                    .Include(x => x.SiteScenarioDatums.Select(e => e.SiteEnrichments))
                    .ToList();
            }
        }

        private static void DisplaySiteScenarios(IEnumerable<SiteScenario> siteScenarios)
        {
            foreach (var siteScenario in siteScenarios)
            {
                Console.WriteLine($"{siteScenario.Id} {siteScenario.ModifiedBy}");
            }

            Console.WriteLine();
        }

        private static IEnumerable<SiteScenarioDatum> FetchSiteScenarioDatums()
        {
            using (var context = new AppDbContext())
            {
                return context.SiteScenarioDatums
                    .ToList();
            }
        }

        private static void DisplaySiteScenarioDatums(ICollection<SiteScenarioDatum> siteScenarioDatums)
        {
            Console.WriteLine($"Count:{siteScenarioDatums.Count}");
            foreach (var siteScenarioDatum in siteScenarioDatums)
            {
                Console.WriteLine($"{siteScenarioDatum.Id} {siteScenarioDatum.ModifiedBy} {siteScenarioDatum.SiteScenarioId} {siteScenarioDatum.SiteId}");
            }

            Console.WriteLine();
        }

        private static IEnumerable<SiteEnrichment> FetchSiteEnrichments()
        {
            using (var context = new AppDbContext())
            {
                return context.SiteEnrichments
                    .ToList();
            }
        }

        private static void DisplaySiteEnrichments(ICollection<SiteEnrichment> siteEnrichments)
        {
            Console.WriteLine($"Count:{siteEnrichments.Count}");
            foreach (var siteEnrichment in siteEnrichments)
            {
                Console.WriteLine($"{siteEnrichment.Id} {siteEnrichment.ModifiedBy} {siteEnrichment.CylinderReference}");
            }

            Console.WriteLine();
        }
    }
}
