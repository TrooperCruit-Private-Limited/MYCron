//using System.Data;
//using System.Reflection;
//using Microsoft.Data.SqlClient;
//using Npgsql;

//namespace MYCentralModels;
//public class GetSetDB(List<KeyValueModel> connectionStrings)
//{
//    List<KeyValueModel> _connectionStrings = connectionStrings;

//    public List<SqlParameter>? SqlParameters { get; set; }
//    public string GetConnectionString(string Key)
//    {
//        return _connectionStrings.SingleOrDefault(a => a.Key == Key).Value;
//    }

//    public static List<SqlParameter> GetSqlParameters<T>(dynamic model)
//    {
//        List<SqlParameter> parameters = [];
//        foreach (PropertyInfo property in typeof(T).GetProperties())
//        {
//            object value = property.GetValue(model);
//            SqlParameter parameter = new("@" + property.Name, value ?? DBNull.Value);
//            parameters.Add(parameter);
//        }
//        return parameters;
//    }
//    public void GetSQLToDataSet1<T>(string procedure, dynamic? model, ref DataSet dataSet)
//    {
//        try
//        {
//            using SqlConnection conn = new(GetConnectionString("TrooperCruitSQL"));
//            SqlCommand sqlComm = new(procedure, conn);
//            if (model != null)
//            {
//                sqlComm.Parameters.AddRange(GetSqlParameters<T>(model).ToArray());
//            }
//            sqlComm.CommandType = CommandType.StoredProcedure;
//            SqlDataAdapter da = new()
//            {
//                SelectCommand = sqlComm
//            };
//            da.Fill(dataSet);
//        }
//        catch (Exception ex) {
//            throw new Exception(ex.ToString());
//        }
//    }

//    public static List<NpgsqlParameter> GetNpgsqlParameters<T>(dynamic model)
//    {
//        List<NpgsqlParameter> parameters = [];
//        foreach (PropertyInfo property in typeof(T).GetProperties())
//        {
//            object value = property.GetValue(model);
//            NpgsqlParameter parameter = new("@" + property.Name, value ?? DBNull.Value);
//            parameters.Add(parameter);
//        }
//        return parameters;
//    }

//    public void GetSQLToDataSet<T>(string procedure, dynamic? model, ref DataSet dataSet)
//    {
//        try
//        {
//            using var conn = new NpgsqlConnection(GetConnectionString("TrooperCruitPostgreSQL"));
//            conn.Open();

//            using var cmd = new NpgsqlCommand(procedure, conn);
//            cmd.CommandType = CommandType.StoredProcedure;

//            if (model != null)
//            {
//                cmd.Parameters.AddRange(GetNpgsqlParameters<T>(model).ToArray());
//            }

//            using var da = new NpgsqlDataAdapter(cmd);
//            da.Fill(dataSet);
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.ToString());
//        }
//    }
//}