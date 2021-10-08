using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using Oracle.DataAccess.Client;
using Devart.Data;
using Devart.Data.Oracle;
using FirebirdSql.Data.FirebirdClient;
using Rep33.Domain;
using Serilog;

namespace Rep33.Data
{
    public class ReportData : IDisposable
    {
        OracleConnection _connection = null;
        FbConnection _FBconnection = null;
        Dictionary<string, DataTable> _QueriesResult;
        public bool UseSavedData = false;

        string lic = "License Key=UdKWmLNCOGgy9gkeFWpZ0olVcfBkYIs4QQLxtS/9xaAgzJAmaLGASLr/laYll6NnBhW/P5c7ThG5CPmD/vgVajPO0ekFT8QH5dMLnw7wYSS5YK6hXbleGprFdA4NqO+OxdHvXrXp4RvxCqQtO1B/eoupKg4q78tTA+oqxNu+9geAlrY7oqXpr7S16GQClQOryptsZNGWvHgJricFrB6dwZlTFP8Mw9YjPQV2X1/46LBvHRJgzd/pQG40JPUKzeyJNuNzwn5dxL/qtJCaF1VqH9B7zxOAAajUcezX3OZpxfU=";

        public ReportData(string UserName, string Password, string DataSource)
        {
            Error = "";
            Schema = "smusin";
            _QueriesResult = new Dictionary<string, DataTable>();
            string _connectionString = string.Format("User Id={0};Password={1};Data Source={2};" , UserName, Password, DataSource);
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection.Dispose();
            }
            _connection = new OracleConnection();
            _connection.ConnectionString = _connectionString;
            try
            {
                _connection.Open();
            }
            catch (OracleException ex)
            {
                Error = ex.Message;
                Log.Error($"Ошибка ReportData:{ex}");
            }
        }

        public ReportData(string UserName, string Password, string DataSource, string FBUserName, string FBPassword, string FBDatabase, string FBDataSource)
        {
            Error = "";
            Schema = "smusin";
            _QueriesResult = new Dictionary<string, DataTable>();
            string _connectionString = string.Format("User Id={0};Password={1};Data Source={2};Validate Connection=true;direct=true;" + lic, UserName, Password, DataSource);
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection.Dispose();
            }
            _connection = new OracleConnection();
            _connection.ConnectionString = _connectionString;
            try
            {
                _connection.Open();
            }
            catch (OracleException ex)
            {
                Error = ex.Message;
                Log.Error($"Ошибка ReportData:{ex}");
            }
            _connectionString = string.Format("User={0};Password={1};Database={2};DataSource={3};", FBUserName, FBPassword, FBDatabase, FBDataSource);
            if (_FBconnection != null)
            {
                if (_FBconnection.State == ConnectionState.Open) _FBconnection.Close();
                _FBconnection.Dispose();
            }
            _FBconnection = new FbConnection();
            _FBconnection.ConnectionString = _connectionString;
            try
            {
                _FBconnection.Open();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Log.Error($"Ошибка ReportData:{ex}");
            }
        }

        public bool GetSavedData()
        {
            RunQuery("SavedData", "SELECT * FROM REPORTER.RPT_PREV_DATA_33B where TRUNC(DATEREPORT, 'DD') = TRUNC(:DBEGIN, 'DD')", "1");
            return true;
        }

        public string Error { get; set; }

        public string Schema { get; set; }

        public DateTime ReportDate { get; set; }

        public bool RunQueries()
        {
            if (_connection == null || _connection.State != ConnectionState.Open) return false;
            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = string.Format("SELECT * FROM RPT_QUERIES where IsActiv='1'");
                cmd.CommandType = CommandType.Text;
                try
                {
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        int iColName = dr.GetOrdinal("QUERY_NAME");
                        int iColCmd = dr.GetOrdinal("QUERY_TEXT");
                        int iColParam = dr.GetOrdinal("IS_NEED_PARAM");
                        int iColDB = dr.GetOrdinal("DB_TYPE");
                        while (dr.Read())
                        {
                            string QueryName = dr.GetString(iColName);
                            if ((!UseSavedData) || (QueryName == "PrevDays") || (QueryName == "PrevMonth") || (QueryName == "CurMonth") || (QueryName == "CurMonthYear") || (QueryName == "CurMonthYearTotal") || (QueryName == "PlanMonthYear") || (QueryName == "PlanMonthYearTotal") || (QueryName == "Prev2MonthYear") || (QueryName == "Prev2MonthYearTotal") || (QueryName == "PrevMonthYear") || (QueryName == "PrevMonthYearTotal"))
                            {
                                string QueryText = dr.GetString(iColCmd);
                                string IsParam = dr.GetString(iColParam);
                                string DBType = dr.GetString(iColDB);
                                if (string.Equals(DBType, DatabaseTypes.ORACLE, StringComparison.CurrentCultureIgnoreCase)) RunQuery(QueryName, QueryText, IsParam);
                                else RunQueryFB(QueryName, QueryText, IsParam);
                            }
                        }
                        dr.Close();
                    }
                }
                catch (OracleException ex)
                {
                    Error = ex.Message;
                    Log.Error($"Ошибка RunQueries:{ex}");
                    return false;
                }
            }
            //dr.Dispose();
            return true;
        }

        private bool RunQuery(string QueryName, string QueryText, string IsParam)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.Connection = _connection;
                cmd.CommandText = string.Format(QueryText);
                cmd.CommandType = CommandType.Text;
                if (IsParam == "1")
                {
                    OracleParameter param1 = new OracleParameter("DBEGIN", OracleDbType.Date);
                    param1.Value = ReportDate;
                    cmd.Parameters.Add(param1);
                }
                using (OracleDataAdapter ad = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        ad.Fill(dt);
                    }
                    catch (OracleException ex)
                    {
                        Error = ex.Message;
                        Log.Error($"Ошибка RunQueries {QueryName} : {ex}");
                        return false;
                    }
                    _QueriesResult.Add(QueryName, dt);
                }
            }
            return true;
        }

        private bool RunQueryFB(string QueryName, string QueryText, string IsParam)
        {
            FbCommand cmd = new FbCommand();
            cmd.Connection = _FBconnection;
            cmd.CommandText = string.Format(QueryText);
            cmd.CommandType = CommandType.Text;
            if (IsParam == "1")
            {
                FbParameter param1 = new FbParameter("@DBEGIN", OracleDbType.Date);
                param1.Value = ReportDate;
                cmd.Parameters.Add(param1);
                //OracleParameter param2 = new OracleParameter("DEND", OracleDbType.Date);
                //param2.Value = ReportDate;
                //cmd.Parameters.Add(param2);
            }
            FbDataAdapter ad = new FbDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            try
            {
                _QueriesResult.Add(QueryName, dt);
            }
            catch (FbException ex)
            {
                Error = ex.Message;
                Log.Error($"Ошибка RunQueryFB {QueryName} : {ex}");
                return false;
            }
            return true;
        }

        public decimal GetValueFromQuery(string QueryName, string Filter, string ValueName, string ValDataName)
        {
            if ((!UseSavedData) || (QueryName == "PrevDays") || (QueryName == "PrevMonth") || (QueryName == "CurMonth") || (QueryName == "CurMonthYear") || (QueryName == "CurMonthYearTotal") || (QueryName == "PlanMonthYear") || (QueryName == "PlanMonthYearTotal") || (QueryName == "Prev2MonthYear") || (QueryName == "Prev2MonthYearTotal") || (QueryName == "PrevMonthYear") || (QueryName == "PrevMonthYearTotal"))
            {
                if (!_QueriesResult.ContainsKey(QueryName)) return 0;
                using (DataView dv = new DataView(_QueriesResult[QueryName]))
                {
                    dv.RowFilter = Filter;
                    if (dv.Count < 1) return 0;
                    return Convert.ToDecimal(dv[0][ValueName] == DBNull.Value ? 0 : dv[0][ValueName]);
                }
            }
            else
            {
                DataRow[] rs = _QueriesResult["SavedData"].Select($"VALUENAME  = '{ValDataName}'");
                if (rs.Count<DataRow>() != 0)
                    return rs[0].IsNull("VALUE") ? 0 : rs[0].Field<decimal>("VALUE");
                else return 0;
            }
        }

        public bool Save(List<DataToSave> Data, DateTime ReportDate)
        {
            using (OracleTransaction ot = _connection.BeginTransaction())
            {
                using (OracleCommand cmd = new OracleCommand("delete from RPT_PREV_DATA_33B where DATEREPORT=:RDATE", _connection))
                {
                    OracleParameter param = new OracleParameter("RDATE", ReportDate);
                    param.DbType = DbType.Date;
                    cmd.Parameters.Add(param);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ot.Rollback();
                        Log.Error($"Ошибка Save : {ex}");
                        return false;
                    }
                    foreach (var row in Data)
                    {
                        using (var cmd1 = new OracleCommand("INSERT INTO RPT_PREV_DATA_33B (DATEREPORT, VALUENAME, VALUE) VALUES(:RDATE, :NAME, :VALUE)", _connection))
                        {
                            param = new OracleParameter("RDATE", row.ReportDate);
                            param.DbType = DbType.Date;
                            cmd1.Parameters.Add(param);
                            param = new OracleParameter("NAME", row.ValueName);
                            param.DbType = DbType.String;
                            cmd1.Parameters.Add(param);
                            param = new OracleParameter("VALUE", row.Value);
                            param.DbType = DbType.Decimal;
                            cmd1.Parameters.Add(param);
                            try
                            {
                                cmd1.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ot.Rollback();
                                Log.Error($"Ошибка Save foreach: {ex}");
                                return false;
                            }
                        }
                    }
                    cmd.CommandText = @"DELETE FROM REPORTER.RPT_AVG_DATA_33B 
                                        WHERE TRUNC(DATEREPORT, 'month') = TRUNC(TO_DATE(:RDATE), 'month')";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"INSERT INTO REPORTER.RPT_AVG_DATA_33B (DATEREPORT, VALUENAME, VALUE)
                                            SELECT TRUNC(TO_DATE(:RDATE), 'month'), VALUENAME, ROUND(AVG(VALUE))
                                            FROM REPORTER.RPT_PREV_DATA_33B
                                            WHERE TRUNC(DATEREPORT, 'month') = TRUNC(TO_DATE(:RDATE), 'month')
                                            GROUP BY VALUENAME";
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ot.Rollback();
                        Log.Error($"Ошибка Save : {ex}");
                        return false;
                    }
                    ot.Commit();
                }
            }
            return true;
        }

        //~ReportData()
        //{
        //    if (_connection.State == ConnectionState.Open) _connection.Close();
        //    _connection.Dispose();
        //    if (_FBconnection.State == ConnectionState.Open) _FBconnection.Close();
        //    _FBconnection.Dispose();
        //}

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
            _connection.Dispose();
            if (_FBconnection.State == ConnectionState.Open) _FBconnection.Close();
            _FBconnection.Dispose();
        }
    }
}
