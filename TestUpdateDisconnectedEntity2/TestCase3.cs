using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// ReSharper disable StringLiteralTypo
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

            DisplayStatistics(siteScenarios);

            ChangeSiteEnrichments(siteScenarioDatums.First().SiteEnrichments);
            UpdateSiteScenarios(siteScenarios);
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
            using (var context = new AppDbContext())
            {
                return context.SiteScenarios
                    .Include(x => x.SiteScenarioDatums)
                    .Include(x => x.SiteScenarioDatums.Select(e => e.SiteEnrichments.Select(so => so.FeedType)))
                    .Include(x => x.SiteScenarioDatums.Select(e => e.SiteEnrichments.Select(pu => pu.ProductionUnit)))
                    .Include(x => x.SiteScenarioDatums.Select(e => e.SiteCylinderStockDatums))
                    .ToList();
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

                    if (siteScenario.SiteScenarioDatums != null)
                    {
                        foreach (var siteScenarioDatum in siteScenario.SiteScenarioDatums)
                        {
                            AddEntityToContext(context, siteScenarioDatum);

                            foreach (var siteEnrichment in siteScenarioDatum.SiteEnrichments)
                            {
                                AddEntityToContext(context, siteEnrichment);
                            }
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
