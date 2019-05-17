$(function () {

    var groupField = ["JobLvl", "RoleName"];

    var LTValue = $("#searchValue").text();
    //. this value will done "Lewis"
    console.log(LTValue);

    var searchField = {
        field: "LT",
        value: [LTValue]
    }

    var stockField = [{
        field: "Location",
        value: ""
    }, {
        field: "TeamName",
        value: ""
    }];


    var mainChart = Highcharts.chart('container', {
        chart: {
            type: 'column',
            events: {
                drilldown: setDrilldownChart,
                load: getMainLevelData
            }
        },
        title: {
            text: 'Drilldown Chart'
        },
        xAxis: {
            type: 'category'
        },
        credits: {
            enabled: false
        },

        legend: {
            enabled: true
        },

        plotOptions: {
            column: {
                stacking: 'normal',
                enabled: true,
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                }
            }
        },

        series: [],

        drilldown: {
            series: []
        }
    });

    function getMainLevelData() {
        var urlParam = "field=" + searchField.field;
        urlParam += "&value=" + JSON.stringify(searchField.value); 
        $.ajax({
            url: '/api/member/first?' + urlParam,
            method: 'POST',
            data: JSON.stringify(stockField),
            contentType: "application/json",
            dataType: "json",
            success: function (chartData) {
                for (var i = 0; i < chartData.length; i++) {
                    var parentField = {
                        field: stockField[0].field,
                        name: chartData[i].name
                    }

                    for (var j = 0; j < chartData[i].data.length; j++) {
                        chartData[i].data[j].field = stockField[1].field;
                        chartData[i].data[j].parentField = parentField;
                        chartData[i].data[j].drilldown = true;
                    }

                    mainChart.addSeries(chartData[i]);
                }

                
            }
        });
    }

    function makeReqData(point) {
        return {
            field: point.field,
            value: point.name
        }
    }

    function setDrilldownChart(e) {
        var postData = [];
        var urlParam = "field=" + searchField.field;
        urlParam += "&value=" + JSON.stringify(searchField.value); 

        if (!e.seriesOptions) {
            var chart = this;            
            var levelsData = chart.drilldownLevels;
            var nLevel = 0, field;
            if (levelsData && levelsData.length > 0) {
                nLevel = levelsData.length;
                for (var i = 0; i < levelsData.length; i++) {
                    if (levelsData[i].pointOptions.parentField) {
                        postData.push(makeReqData(levelsData[i].pointOptions.parentField));
                    }
                    postData.push(makeReqData(levelsData[i].pointOptions));
                }
            }
            if (e.point.parentField) {
                postData.push(makeReqData(e.point.parentField));
            }
            postData.push(makeReqData(e.point));
            postData.push({
                field: groupField[nLevel],
                value: ""
            });
            field = groupField[nLevel];

            chart.showLoading("Loading data...");
            $.ajax({
                url: '/api/member?' + urlParam,
                method: 'POST',
                data: JSON.stringify(postData),
                contentType: "application/json",
                dataType: "json",
                success: function (chartData) {
                    if (nLevel < groupField.length - 1) {
                        for (var i = 0; i < chartData.length; i++) {
                            chartData[i].field = field;
                            chartData[i].drilldown = true;
                        }
                    }
                    
                    chart.addSeriesAsDrilldown(e.point, {
                        name: field,
                        data: chartData
                    });
                    chart.hideLoading();                   
                }
            });
        }
    }
});