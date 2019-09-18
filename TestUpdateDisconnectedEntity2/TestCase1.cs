using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Local
namespace TestUpdateDisconnectedEntity2
{
    /// <summary>
    /// Test program to evaluate the performance of updating a specific
    /// entity in an object graph.
    /// </summary>
    /// <remarks>
    /// Test:
    /// - Fetching a SiteScenario and all its related data (SiteScenarioDatum and SiteEnrichment)
    /// - Update a specific SiteEnrichment entity
    /// - Save the complete object graph
    /// Result:
    /// - The update took in the region of 2 - 2.5 seconds
    /// </remarks>
    internal class TestCase1
    {
        internal void Run()
        {
            var siteScenarios = FetchSiteScenarios();

            var siteScenario = siteScenarios.First();
            var siteScenarioDatums = siteScenario.SiteScenarioDatums;

            ChangeSiteEnrichments(siteScenarioDatums.First().SiteEnrichments);
            UpdateSiteScenarios(siteScenarios);
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

        private static void DisplaySiteScenarioDatums(ICollection<SiteScenarioDatum> siteScenarioDatums)
        {
            Console.WriteLine($"Count:{siteScenarioDatums.Count}");
            foreach (var siteScenarioDatum in siteScenarioDatums)
            {
                Console.WriteLine($"{siteScenarioDatum.Id} {siteScenarioDatum.ModifiedBy} {siteScenarioDatum.SiteScenarioId} {siteScenarioDatum.SiteId}");
            }

            Console.WriteLine();
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

        private static void ChangeSiteEnrichments(IReadOnlyList<SiteEnrichment> siteEnrichments)
        {
            var siteEnrichment = siteEnrichments[9];
            siteEnrichment.IsUpdated = true;

            var oldCylinderReference = siteEnrichment.CylinderReference;
            var newCylinderReference = $"JH-{DateTime.Now.Ticks}";

            siteEnrichment.CylinderReference = newCylinderReference;

            Console.WriteLine($"Changed the SiteEnrichment.Id {siteEnrichment.Id}, old CylinderReference '{oldCylinderReference}' new CylinderReference='{newCylinderReference}'");
        }

        private static void UpdateSiteScenarios(IEnumerable<SiteScenario> siteScenarios)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Updating...");

            using (var context = new AppDbContext())
            {
                foreach (var siteScenario in siteScenarios)
                {
                    AddEntityToContext(context, siteScenario);

                    foreach (var siteScenarioDatum in siteScenario.SiteScenarioDatums)
                    {
                        AddEntityToContext(context, siteScenarioDatum);

                        foreach (var siteEnrichment in siteScenarioDatum.SiteEnrichments)
                        {
                            AddEntityToContext(context, siteEnrichment);
                        }
                    }
                }

                context.SaveChanges();
            }

            stopwatch.Stop();

            Console.WriteLine($"Update complete. Time taken: {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void AddEntityToContext<T>(DbContext context, T entity) where T : Entity
        {
            if (entity.IsNew)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else if (entity.IsUpdated)
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            else if (entity.IsDeleted)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                context.Entry(entity).State = EntityState.Unchanged;
            }
        }
    }
}
