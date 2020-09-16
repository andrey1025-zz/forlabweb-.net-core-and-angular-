import { Component, ViewChild } from '@angular/core';
import * as Highcharts from 'highcharts';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { Router } from "@angular/router";

const borderRadius = require('highcharts-border-radius');
borderRadius(Highcharts);

@Component({
    selector: 'sa-dashboard',
    templateUrl: './Dashboard.component.html',
    styleUrls: ['./Dashboard.component.css']
})
export class DashboardComponent {
    chartOptions: Object;
    Dashboard1 = new Array();
    Regionlist = new Array();
    region = new Array();
    //  Highcharts1 = Highcharts;
    seriesData: Array<string> = [];
    nooftestData: Array<string> = [];
    noofinsData: Array<string> = [];
    noofproductData: Array<string> = [];
    noofsiteData: Array<string> = [];
    max_product = 0;
    total_product = 0;
    max_test = 0;
    total_test = 0;
    max_ins = 0;
    total_ins = 0;
    colors = ['#1BB192', '#9229E2', '#E53560', '#EFBF20', '#90ED7D'];

    constructor(private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {
        this.getseriessite();
        this.getnoofsitessbycategory();
        this.getnoofproduct();
        this.getnooftest();
        this.getnoofins();
    }

    getseriessite() {
        let Countryid: any;
        if (localStorage.getItem("role") == "admin") {
            Countryid = 0
        }
        else {
            Countryid = localStorage.getItem("countryid")
        }
        this._APIwithActionService.getDatabyID(Countryid, 'Dasboard', 'Getnoofsiteperregion').subscribe((data) => {
            this.seriesData = data;
            for (var idx = 0; idx < this.seriesData.length; idx++) {
                this.seriesData[idx]["color"] = this.colors[idx];
            }
            this._APIwithActionService.getDatabyID(Countryid, 'Site', 'GetregionbyCountryID').subscribe((data) => {
                this.Regionlist = data;
                this.Regionlist.forEach(element => {
                    this.region.push(element.regionName);
                });
                this.chartOptions = Highcharts.chart('container', {
                    chart: {
                        type: 'column',
                        marginTop: 45,
                        events: {
                            render: function (e) {
                                for (var idx = 0; idx < this.series[0]["yData"].length; idx++) {
                                    if (document.getElementById("total_series_" + idx)) {
                                        document.getElementById("total_series_" + idx).parentElement.remove();
                                    }
                                }
                                var sum = new Array();
                                var max_y = new Array();
                                for (var idx = 0; idx < this.series.length; idx++) {
                                    for (var idx_c = 0; idx_c < this.series[idx]["yData"].length; idx_c++) {
                                        if (sum[idx_c]) sum[idx_c] += this.series[idx]["yData"][idx_c];
                                        else sum[idx_c] = this.series[idx]["yData"][idx_c];
                                        if (max_y[idx_c] == undefined) max_y[idx_c] = this.series[idx]["yData"][idx_c];
                                        else {
                                            if (max_y[idx_c] < this.series[idx]["yData"][idx_c]) max_y[idx_c] = this.series[idx]["yData"][idx_c];
                                        }
                                    }
                                }
                                for (var idx = 0; idx < sum.length; idx++) {
                                    var text = this.renderer.html('<span id="total_series_' + idx + '" style="font-weight: bold;font-size: 14px;color: #2B394E; opacity: 0.2;">' + sum[idx] + '</span>').add();
                                    var textBBox = text.getBBox();
                                    var series_width = this.plotWidth / sum.length;
                                    var step_height = this.plotHeight / 10;
                                    var x = this.plotLeft + series_width * idx + (series_width * 0.5) - (textBBox.width * 0.5);
                                    var y = this.plotTop + (step_height * (10 - max_y[idx])) - (textBBox.height * 0.5);
                                    text.attr({ x: x, y: y - 10 });

                                }

                            }
                        }
                    },
                    legend: false,
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: false,
                    },
                    xAxis: {
                        categories: this.region,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        max: 10,
                        tickAmount: 11,
                        title: {
                            text: 'Sites Per Region',
                            style: {
                                fontSize: '14px',
                                fontWeight: 'Bold',
                                color: '#2B394E',
                                whiteSpace: '0.1em'
                            }
                        },
                        gridLineDashStyle: 'dot',
                        stackLabels: {
                            enabled: true
                        }
                    },
                    tooltip: {
                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}:{sums(point.y)}</b><br/>',
                        shared: true
                    },
                    plotOptions: {
                        series: {
                            pointWidth: 6,
                        },
                        column: {
                            borderRadiusTopLeft: 5,
                            borderRadiusTopRight: 5
                        },
                        line: {
                            dashStyle: "Dot"
                        }
                    },
                    series: this.seriesData
                });

            })
        }), err => {
        }
    }

    getnoofsitessbycategory() {
        let Countryid: any
        if (localStorage.getItem("role") == "admin") {
            Countryid = 0
        }
        else {
            Countryid = localStorage.getItem("countryid")
        }
        this._APIwithActionService.getDatabyID(Countryid, 'Dasboard', 'Getnoofsitespercategory').subscribe((data => {
            this.noofsiteData = data;
            for (var idx = 0; idx < this.noofsiteData.length; idx++) {
                this.noofsiteData[idx]["color"] = this.colors[idx];
            }
            Highcharts.chart('container4', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    type: 'pie',
                    events: {
                        render: function (e) {
                            if (document.getElementById("total_pie")) {
                                document.getElementById("total_pie").parentElement.remove();
                            }
                            var total = 0;
                            for (var i = 0, len = this.series[0].yData.length; i < len; i++) {
                                total += this.series[0].yData[i];
                            }
                            var text = this.renderer.html('<h1 id="total_pie" style="font-weight:bold; letter-spacing: 0.1em">' + total + '</h1><span style="letter-spacing: 0.1em;">Sites</span>').add();
                            var textBBox = text.getBBox();
                            var x = this.plotLeft + (this.plotWidth * 0.5) - (textBBox.width * 0.5);
                            var y = this.plotTop + (this.plotHeight * 0.5) - (textBBox.height * 0.5);
                            text.attr({ x: x, y: y + 6 });
                        },
                    }
                },
                legend: false,
                credits: {
                    enabled: false
                },
                title: {
                    text: 'Sites By Category',
                    style: {
                        fontSize: '14px',
                        fontWeight: 'bold'
                    },
                    verticalAlign: 'bottom'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            useHTML: true,
                            enabled: true,
                            formatter: function () {
                                return '<span style="color:' + this.point.color + '; font-size: 12px;">' + this.point.name + ':' + this.point.y + '</span>';
                            }
                        },
                        className: 'pie-charts123',
                        showInLegend: true
                    }
                },
                series: [{
                    colorByPoint: true,
                    data: this.noofsiteData,
                    innerSize: '50%',
                    size: 180
                }]
            });
        }))
    }

    getnoofproduct() {
        this._APIwithActionService.getDataList('Dasboard', 'Getnoofproductpertype').subscribe(data => {
            this.noofproductData = data;
            for (var idx = 0; idx < this.noofproductData.length; idx++) {
                if (this.max_product < this.noofproductData[idx]["y"]) this.max_product = this.noofproductData[idx]["y"];
                this.total_product += this.noofproductData[idx]["y"];
            }
            for (var idx = 0; idx < this.noofproductData.length; idx++) {
                this.noofproductData[idx]["percent"] = Math.round(this.noofproductData[idx]["y"] / this.max_product * 100);
            }
        })
    }
    getnooftest() {
        this._APIwithActionService.getDataList('Dasboard', 'Getnooftestperarea').subscribe(data => {
            this.nooftestData = data;
            console.log("this.nooftestData", this.nooftestData)
            for (var idx = 0; idx < this.nooftestData.length; idx++) {
                if (this.max_test < this.nooftestData[idx]["y"]) this.max_test = this.nooftestData[idx]["y"];
                this.total_test += this.nooftestData[idx]["y"];
            }
            for (var idx = 0; idx < this.nooftestData.length; idx++) {
                this.nooftestData[idx]["percent"] = Math.round(this.nooftestData[idx]["y"] / this.max_test * 100);
            }

        })
    }
    getnoofins() {
        this._APIwithActionService.getDataList('Dasboard', 'Getnoofinsperarea').subscribe(data => {
            this.noofinsData = data;
            //console.log("this.nooftestData", this.nooftestData)
            for (var idx = 0; idx < this.noofinsData.length; idx++) {
                if (this.max_ins < this.noofinsData[idx]["y"]) this.max_ins = this.noofinsData[idx]["y"];
                this.total_ins += this.noofinsData[idx]["y"];
            }
            for (var idx = 0; idx < this.noofinsData.length; idx++) {
                this.noofinsData[idx]["percent"] = Math.round(this.noofinsData[idx]["y"] / this.max_ins * 100);
            }

        })
    }
    getregions() {
        let Countryid: any
        if (localStorage.getItem("role") == "admin") {
            Countryid = 0
        }
        else {
            Countryid = localStorage.getItem("countryid")
        }
        this._APIwithActionService.getDatabyID(Countryid, 'Site', 'GetregionbyCountryID').subscribe((data) => {
            this.Regionlist = data
            this.Regionlist.forEach(element => {
                this.region.push(element.regionName);
            });

        })
    }

}