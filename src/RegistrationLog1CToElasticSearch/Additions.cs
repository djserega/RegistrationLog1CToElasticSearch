namespace RegistrationLog1CToElasticSearch
{
    public static class Additions
    {
        public static long DateToSQLite(this DateTime date)
        {
            long dateSqlite = (long)(date - DateTime.MinValue).TotalMilliseconds * 10;

            return dateSqlite;
        }
        public static DateTime DateFromSQLite(this long SQLDate)
        {
            DateTime date = DateTime.MinValue.AddSeconds(SQLDate / 10000);

            return date;
        }
    }
}
