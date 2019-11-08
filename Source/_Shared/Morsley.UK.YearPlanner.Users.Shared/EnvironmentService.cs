//using System;

//namespace Morsley.UK.YearPlanner.Users.Shared
//{
//    public static class EnvironmentService : IEnvironmentService
//    {
//        public static string GetEnvironmentVariableValueByKey(string key)
//        {
//            if (key == null) throw new ArgumentNullException(nameof(key));
//            if (key.Length == 0) throw new ArgumentException("Cannot be empty", nameof(key));
//            try
//            {
//                var value = System.Environment.GetEnvironmentVariable(key);
//                return value;
//            }
//            catch (Exception e)
//            {
//                throw new ArgumentException($"Could not determine value for environment variable with name: '{key}'", nameof(key), e);
//            }
//        }
//    }
//}