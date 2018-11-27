using System;
using System.Data.SqlClient;
using TestRepository.Model;

namespace TestRepository.Tests
{
    internal class TestInitializeUtils
    {
        internal static int CreateBusinessPartner(string name)
        {
            var sql = $@"INSERT INTO BusinessPartners ([ModifiedBy], [ModificationDate], [ShortName], [Name], [SAPId], [ColourCodeArgb], [RegionalGrouping], [IsCustomer], [IsFabricator], [IsDeconverter], [IsConverter]) 
                            VALUES ('testuser', '{DateTime.UtcNow:s}', '{name}', '{name}', 12345, 0, 0, 0, 0, 0, 0);SELECT @@IDENTITY";

            return ExecuteInsertCommand(sql);
        }

        internal static int CreateContract(int businessPartnerId, string name, int businessEntityId)
        {
            var sql = $@"INSERT INTO [dbo].[Contracts] (
                        [ModifiedBy], 
                        [ModificationDate], 
                        [BusinessPartnerId], 
                        [ContractNumber], 
                        [WeightTolerance], 
                        [ProductContainerSupply], 
                        [FinalNotificationPeriod], 
                        [ProvisionalNotificationPeriod], 
                        [ContainerDeliveryPeriod], 
                        [MinimumAssayTolerance], 
                        [AssayTolerance], 
                        [CollectionDateOffset], 
                        [TransportAllowancePeriod], 
                        [ArbitrationPeriod], 
                        [DeliveryContainerOwnership], 
                        [BusinessEntityId])
                        VALUES ('testuser', '{DateTime.UtcNow:s}', {businessPartnerId}, '{name}', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, {businessEntityId});SELECT @@IDENTITY";

            return ExecuteInsertCommand(sql);
        }

        public static int CreateContractSWUOriginPermitted(int contractId, int swuOriginId)
        {
            var sql = $@"INSERT INTO [dbo].[ContractSWUOriginPermitteds] ([ModifiedBy], [ModificationDate], [ContractId], [SWUOriginId])
                        VALUES ('testuser', '{DateTime.UtcNow:s}', {contractId}, {swuOriginId});SELECT @@IDENTITY";

            return ExecuteInsertCommand(sql);
        }

        public static int CreateContractCylinderOwnershipPermitteds(int contractId, int businessPartnerId)
        {
            var sql = $@"INSERT INTO [dbo].[ContractCylinderOwnershipPermitteds] ([ModifiedBy], [ModificationDate], [ContractId], [BusinessPartnerId])
                        VALUES ('testuser', '{DateTime.UtcNow:s}', {contractId}, {businessPartnerId});SELECT @@IDENTITY";

            return ExecuteInsertCommand(sql);
        }

        public static int GetSWUOriginFromName(string code)
        {
            var sql = $@"SELECT Id FROM SWUOrigins WHERE Code = '{code}'";

            return ExecuteReaderCommandWithSingleResult<int>(sql, 0);
        }

        public static int GetBusinessEntityFromName(string name)
        {
            var sql = $@"SELECT Id FROM BusinessEntities WHERE BusinessEntityName = '{name}'";

            return ExecuteReaderCommandWithSingleResult<int>(sql, 0);
        }

        public static int GetBusinessPartnerFromName(string name)
        {
            var sql = $@"SELECT Id FROM BusinessPartners WHERE Name = '{name}'";

            return ExecuteReaderCommandWithSingleResult<int>(sql, 0);
        }

        public static int GetContractFromName(string contractNumber)
        {
            var sql = $@"SELECT Id FROM Contracts WHERE ContractNumber = '{contractNumber}'";

            return ExecuteReaderCommandWithSingleResult<int>(sql, 0);
        }

        public static int GetTableRowCount(string tableName)
        {
            var sql = $@"SELECT COUNT(Id) FROM [{tableName}]";

            return ExecuteReaderCommandWithSingleResult<int>(sql, 0);
        }

        private static T ExecuteReaderCommandWithSingleResult<T>(string sql, int resultIndex)
        {
            using (var connection = new SqlConnection(AppDbContext.ConnectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return (T) reader[resultIndex];
                    }

                    return default(T);
                }
            }
        }

        private static int ExecuteInsertCommand(string sql)
        {
            using (var connection = new SqlConnection(AppDbContext.ConnectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}