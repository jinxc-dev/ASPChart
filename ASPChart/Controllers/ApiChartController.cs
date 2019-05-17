using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPChart.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ASPChart.Controllers
{

    [Route("api/member")]
    public class ApiChartController : Controller
    {
        private IMemberCURDModel _member;

        public ApiChartController(IMemberCURDModel member)
        {
            _member = member;
        }

        [HttpGet]
        public IEnumerable<ChartDataModel> Get()
        {
            return _member.GetDataByRoleName();
        }

        [HttpPost]
        public IEnumerable<ChartDataModel> Post([FromBody]IEnumerable<QueryModel> value)
        {
            //public IEnumerable<QueryModel> Post([FromBody]IEnumerable<QueryModel> value)
            string condField = HttpContext.Request.Query["field"];
            string[] condValues = JsonConvert.DeserializeObject<string[]>(HttpContext.Request.Query["value"]);
            var a = value;

            return _member.GetChartModelByGroupInfo(value, condField, condValues);
        }

        [HttpPost("first")]
        public IEnumerable<StockChartModel> First([FromBody]IEnumerable<QueryModel> value)
        {
            string condField = HttpContext.Request.Query["field"];
            string[] condValues = JsonConvert.DeserializeObject<string[]>(HttpContext.Request.Query["value"]);

            List<string> items = _member.GetStockedModel(value.ElementAt(0).field, condField, condValues);
            List<StockChartModel> retData = new List<StockChartModel>();
            foreach (string element in items)
            {
                StockChartModel tmpData = new StockChartModel();
                List<QueryModel> queryData = new List<QueryModel>();
                queryData.Add(new QueryModel{ field = value.ElementAt(0).field, value = element });
                queryData.Add(value.ElementAt(1));

                tmpData.Name = element;
                tmpData.Data = _member.GetChartModelByGroupInfo(queryData, condField, condValues);
                retData.Add(tmpData);
            }

            return retData;
        }

    }
}
