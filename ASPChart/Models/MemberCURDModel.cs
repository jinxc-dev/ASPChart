using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System;

namespace ASPChart.Models
{
    public class MemberCURDModel : IMemberCURDModel
    {
        private readonly IConfiguration _config;
        private SqlConnection _conn;

        public MemberCURDModel(IConfiguration config)
        {
            _config = config;
            _conn = new SqlConnection(config.GetSection("ConnectionString").GetSection("DefaultConnection").Value);
        }

        public List<MemberDataModel> GetAll(string LTValue)
        {
            //return _conn.Query<MemberDataModel>("Select * From dbo.memberInfo Order By ID Asc").ToList();
            string sql = $"Select * From dbo.memberInfo where LT='{LTValue}'";
            return _conn.Query<MemberDataModel>(sql).ToList();
        }

        public List<ChartDataModel> GetDataByRoleName()
        {
            return _conn.Query<ChartDataModel>("Select Count(id) as TotalRows, RoleName as Name From dbo.memberInfo Group By RoleName").ToList();
        }

        public List<ChartDataModel> GetChartModelByGroupInfo(IEnumerable<QueryModel> queryModel, string condField, string[] condValues)
        {
            QueryModel lastModel = queryModel.Last();
            string strSQL = $"Select Count(id) as Y, {lastModel.field} as Name From dbo.memberInfo ";
            string strWhere = "";
            string strGroup = "Group By ";
            string condSQL = "";

            if (!String.IsNullOrEmpty(condField))
            {
                condSQL = condField + " IN (";
                for (var i = 0; i < condValues.Length; i++)
                {
                    condSQL += "'" + condValues[i] + "'";                    
                    if (i < condValues.Length - 1)
                    {
                        condSQL += ",";
                    }
                }
                condSQL += ")";
            }

            strWhere += condSQL;

            for (int i = 0; i < queryModel.Count() - 1; i++)
            {
                QueryModel item = queryModel.ElementAt(i);
                if (i != 0 || !condSQL.Equals(""))
                {
                    strWhere += " and ";
                }
                strWhere += item.field + "='" + item.value + "' ";
                strGroup += item.field + ",";
            }

            strGroup += lastModel.field;

            if (!strWhere.Equals(""))
            {
                strWhere = "Where " + strWhere;
            } 

            strSQL += strWhere + strGroup;

            return _conn.Query<ChartDataModel>(strSQL).ToList();

            //return null;
        }

        public List<string> GetStockedModel(string field, string condField, string[] condValues)
        {
            string strSQL;
            string condSQL;

            if (String.IsNullOrEmpty(condField))
            {
                strSQL = $"Select {field} as Name From dbo.memberInfo Group By {field}";
            } else
            {
                condSQL = condField + " IN (";
                for (var i = 0; i < condValues.Length; i++) {
                    condSQL += "'" + condValues[i] + "'";
                    if (i < condValues.Length - 1) {
                        condSQL += ",";
                    }
                }
                condSQL += ")";

                strSQL = $"Select {field} as Name From dbo.memberInfo Where {condSQL} Group By {field}";
            }
            return _conn.Query<string>(strSQL).ToList();
        }

    }

    public class ChartDataModel
    {
        public int y { set; get; }
        public string Name { set; get; }
    }

    public class QueryModel
    {
        public string field { set; get; }
        public string value { set; get; }
    }

    public class StockChartModel
    {
        public string Name { set; get; }
        public List<ChartDataModel> Data { set; get; }
    }
}