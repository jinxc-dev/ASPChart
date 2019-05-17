using System.Collections.Generic;

namespace ASPChart.Models
{
    public interface IMemberCURDModel
    {
        List<MemberDataModel> GetAll(string LTValue);
        List<ChartDataModel> GetDataByRoleName();
        List<ChartDataModel> GetChartModelByGroupInfo(IEnumerable<QueryModel> queryModel, string condField, string[] condValue);
        List<string> GetStockedModel(string field, string condField, string[] condValue);
    }
}