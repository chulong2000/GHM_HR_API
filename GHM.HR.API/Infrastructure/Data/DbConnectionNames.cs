namespace GHM.HR.API.Infrastructure.Data
{
    public static class DbConnectionNames
    {
        /// <summary>Main QLPK database (GHM_QLPK).</summary>
        public const string Qlpk = "Qlpk";

        /// <summary>Shared HR database (read-only cross-service lookups: users, departments).</summary>
        public const string Hr = "Hr";

        /// <summary>Default connection used by repositories in this feature.</summary>
        public const string Default = Qlpk;
    }
}
