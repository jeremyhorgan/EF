using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Local
namespace TestUpdateDisconnectedEntity2Core
{
    /// <summary>
    /// Test program to evaluate the performance of updating a specific
    /// entity in an object graph.
    /// </summary>
    /// <remarks>
    /// Test:
    /// - Fetch a SiteScenario, SWOrigin, ProductionUnit, ProductionPlant (in separate queries)
    /// - Separately fetch a SiteScenarioDatum and its related SiteEnrichment
    /// - Connect the different objects in memory
    /// - Update a specific SiteEnrichment entity
    /// - Save the complete object graph (from SiteScenario)
    /// Result:
    /// - The update took in the region of ??? second
    /// </remarks>
    internal class TestCase3
    {
        internal void Run()
        {
            var siteScenarios = FetchSiteScenarios();

            var siteScenario = siteScenarios.First();
            var siteScenarioDatums = siteScenario.SiteScenarioDatums;

            // DisplayStatistics(siteScenarios);

            ChangeSiteEnrichments(siteScenarioDatums.First().SiteEnrichments);
            UpdateSiteScenarios2(siteScenario);
        }

        private void DisplayStatistics(List<SiteScenario> siteScenarios)
        {
            Console.WriteLine("Counts:");
            Console.WriteLine($" SiteScenarios: {siteScenarios.Count}");

            foreach (var siteScenario in siteScenarios)
            {
                var count = siteScenario.SiteScenarioDatums?.Count ?? 0;
                Console.WriteLine($" SiteScenarioDatums: {count} for SiteScenario.Id: {siteScenario.Id}");

                if (siteScenario.SiteScenarioDatums != null)
                {
                    foreach (var siteScenarioDatum in siteScenario.SiteScenarioDatums)
                    {
                        Console.WriteLine(
                            $" SiteEnrichments: {siteScenarioDatum.SiteEnrichments.Count} for SiteScenarioDatum.Id: {siteScenarioDatum.Id}");
                    }
                }
            }
        }

        private static List<SiteScenario> FetchSiteScenarios()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Fetching...");

            using (var context = new AppDbContext())
            {
                var items = context.SiteScenarios
                    .Include(e => e.SiteScenarioDatums)
                        .ThenInclude(e => e.SiteEnrichments)
                        .ThenInclude(e => e.FeedType)
                    .Include(e => e.SiteScenarioDatums)
                        .ThenInclude(e => e.SiteEnrichments)
                        .ThenInclude(e => e.ProductionUnit)
                    .Include(e => e.SiteScenarioDatums)
                        .ThenInclude(e => e.SiteCylinderStockDatums)
                    .ToList();

                stopwatch.Stop();

                Console.WriteLine($"Fetch complete. Time taken: {stopwatch.ElapsedMilliseconds}ms");

                return items;
            }
        }

        private static void ChangeSiteEnrichments(IReadOnlyList<SiteEnrichment> siteEnrichments)
        {
            var siteEnrichment = siteEnrichments[9];
            siteEnrichment.IsUpdated = true;

            var oldCylinderReference = siteEnrichment.CylinderReference;
            var newCylinderReference = $"JH-{DateTime.Now.Ticks}";

            siteEnrichment.CylinderReference = newCylinderReference;

            Console.WriteLine($"Changed the SiteEnrichment.Id {siteEnrichment.Id}, \n\told CylinderReference '{oldCylinderReference}' new CylinderReference='{newCylinderReference}'");
        }

        private static void UpdateSiteScenarios(SiteScenario siteScenario)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Updating...");

            using (var context = new AppDbContext())
            {
                context.ChangeTracker.TrackGraph(siteScenario, UpdateEntityState);
                context.SaveChanges();
            }

            stopwatch.Stop();

            Console.WriteLine($"Update complete. Time taken: {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void UpdateSiteScenarios2(SiteScenario siteScenario)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Updating2...");

            using (var context = new AppDbContext())
            {
                foreach (var scenarioDatum in siteScenario.SiteScenarioDatums)
                {
                    foreach (var siteEnrichment in scenarioDatum.SiteEnrichments)
                    {
                        if (siteScenario.IsUpdated)
                        {
                            context.Entry(siteEnrichment).State = EntityState.Modified;
                        }
                    }
                }
                context.SaveChanges();
            }

            stopwatch.Stop();

            Console.WriteLine($"Update2 complete. Time taken: {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void UpdateEntityState(EntityEntryGraphNode node)
        {
            var entity = (Entity)node.Entry.Entity;
            if (entity != null)
            {
                if (entity.IsNew)
                {
                    node.Entry.State = EntityState.Added;
                }
                else if (entity.IsUpdated)
                {
                    node.Entry.State = EntityState.Modified;
                }
                else if (entity.IsDeleted)
                {
                    node.Entry.State = EntityState.Deleted;
                }
                else
                {
                    node.Entry.State = EntityState.Unchanged;
                }
            }
        }
    }
}
