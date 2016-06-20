using System;
using System.Configuration;

namespace Infrastructure
{
    public class ConfigSettings
    {
        public static string ConnectionString { get; set; }
        public static string BusinessConnectionString { get; set; }

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString ?? "server=124.251.46.179;Database=Home_Customer_Test;uid=Home_Customer_w;pwd=bc948602;Connect Timeout=60";
            BusinessConnectionString = ConfigurationManager.ConnectionStrings["EBS_WRITE"].ConnectionString ?? "Data Source=123.103.35.138;Initial Catalog=jjjy_test1107;UID=jjjy_test_admin;PWD=3791f38D;";
        }    }
}
