export async function exportStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

export async function previewFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();

    const blob = new Blob([arrayBuffer], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    window.open(url);

    //const anchorElement = document.createElement('a');
    //anchorElement.href = url;
    //anchorElement.download = fileName ?? '';
    //anchorElement.click();
    //anchorElement.remove();

    URL.revokeObjectURL(url);
}

export function showAlert(message) {
    alert(message);
}

var topLocChart;
var itemCategoryChart;
var ItemValuebyRiskTypeChart;
var ItemCountRiksTypeChart;

export function initializeDoughnutChart(chartId, arrayLabels, arrayValue, chartTitle, sub) {
    const ctx = document.getElementById(chartId);
    var chartColor = [];

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ",0.6)";
    };

    for (var i in arrayLabels) {
        chartColor.push(dynamicColors());
    }

    if (chartId == "itemCaseCategoryStats")
    {
        ItemCountRiksTypeChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor
                }],
                hoverOffset: 4
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        // get the data label and data value to display
                        // convert the data value to local string so it uses a comma seperated number
                        var dataLabel = data.labels[tooltipItem.index];
                        // var dataLabel = 'Loss Estimation ';
                        var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                        // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                        if (Chart.helpers.isArray(dataLabel)) {
                            // show value on first line of multiline label
                            // need to clone because we are changing the value
                            dataLabel = dataLabel.slice();
                            dataLabel[0] += value;
                        } else {
                            dataLabel += value;
                        }

                        // return the text to display on the tooltip
                        return dataLabel;
                    }
                }
            }
        });
    }
}

export function updateDoughnutChart(element, chartId, arrayLabels, arrayValue, chartTitle, sub) {
    //const ctx = document.getElementById(chartId);

    if (chartId == "itemCaseCategoryStats")
    {
        ItemCountRiksTypeChart.destroy();
    }

    var chartColor = [];

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ",0.6)";
    };

    for (var i in arrayLabels) {
        chartColor.push(dynamicColors());
    }

    if (chartId == "itemCaseCategoryStats") {
        ItemCountRiksTypeChart = new Chart(element, {
            type: 'doughnut',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor
                }],
                hoverOffset: 4
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        // get the data label and data value to display
                        // convert the data value to local string so it uses a comma seperated number
                        var dataLabel = data.labels[tooltipItem.index];
                        // var dataLabel = 'Loss Estimation ';
                        var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                        // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                        if (Chart.helpers.isArray(dataLabel)) {
                            // show value on first line of multiline label
                            // need to clone because we are changing the value
                            dataLabel = dataLabel.slice();
                            dataLabel[0] += value;
                        } else {
                            dataLabel += value;
                        }

                        // return the text to display on the tooltip
                        return dataLabel;
                    }
                }
            }
        });
    }
}

export function initializeBarChart(chartId, arrayLabels, arrayValue, chartTitle, sub) {
    const ctx = document.getElementById(chartId);
    var chartColor = [];
    var borderColor = [];

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ",0.6)";
    };

    for (var i in arrayLabels) {
        chartColor.push(dynamicColors());
        borderColor.push(dynamicColors());
    }

    if (chartId == "topLocationReportStats") {
        topLocChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            var dataLabel = data.labels[tooltipItem.index];
                            // var dataLabel = 'Loss Estimation ';
                            var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
    else if (chartId == "itemCategoryStats")
    {
        itemCategoryChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            var dataLabel = data.labels[tooltipItem.index];
                            // var dataLabel = 'Loss Estimation ';
                            var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
    else if (chartId == "itemCaseValueStats") {
        ItemValuebyRiskTypeChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            // var dataLabel = data.labels[tooltipItem.index];
                            var dataLabel = 'Loss Estimation ';
                            var value = ': Rp. ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString() + ' ,-';

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
}

export function updateBarChart(element, chartId, arrayLabels, arrayValue, chartTitle, sub) {
    //const ctx = document.getElementById(chartId);

    if (chartId == "topLocationReportStats") {
        topLocChart.destroy();
    }
    else if (chartId == "itemCategoryStats") {
        itemCategoryChart.destroy();
    }
    else if (chartId == "itemCaseValueStats") {
        ItemValuebyRiskTypeChart.destroy();
    }

    var chartColor = [];
    var borderColor = [];

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ",0.6)";
    };

    for (var i in arrayLabels) {
        chartColor.push(dynamicColors());
        borderColor.push(dynamicColors());
    }

    if (chartId == "topLocationReportStats") {
        topLocChart = new Chart(element, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            var dataLabel = data.labels[tooltipItem.index];
                            // var dataLabel = 'Loss Estimation ';
                            var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
    else if (chartId == "itemCategoryStats") {
        itemCategoryChart = new Chart(element, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            var dataLabel = data.labels[tooltipItem.index];
                            // var dataLabel = 'Loss Estimation ';
                            var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
    else if (chartId == "itemCaseValueStats") {
        ItemValuebyRiskTypeChart = new Chart(element, {
            type: 'bar',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    backgroundColor: chartColor,
                    borderColor: borderColor,
                    borderWidth: 1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            // get the data label and data value to display
                            // convert the data value to local string so it uses a comma seperated number
                            var dataLabel = data.labels[tooltipItem.index];
                            var value = ': Rp. ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString() + ' ,-';

                            // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                            if (Chart.helpers.isArray(dataLabel)) {
                                // show value on first line of multiline label
                                // need to clone because we are changing the value
                                dataLabel = dataLabel.slice();
                                dataLabel[0] += value;
                            } else {
                                dataLabel += value;
                            }

                            // return the text to display on the tooltip
                            return dataLabel;
                        }
                    }
                }
            }
        });
    }
}

export function initializeLineChart(chartId, arrayLabels, arrayValue, chartTitle, sub) {
    const ctx = document.getElementById(chartId);

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };

    if (chartId == "itemCaseValueStats")
    {
        ItemValuebyRiskTypeChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    fill: false,
                    borderColor: dynamicColors(),
                    tension: 0.1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        // get the data label and data value to display
                        // convert the data value to local string so it uses a comma seperated number
                        var dataLabel = data.labels[tooltipItem.index];
                        // var dataLabel = 'Loss Estimation ';
                        var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                        // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                        if (Chart.helpers.isArray(dataLabel)) {
                            // show value on first line of multiline label
                            // need to clone because we are changing the value
                            dataLabel = dataLabel.slice();
                            dataLabel[0] += value;
                        } else {
                            dataLabel += value;
                        }

                        // return the text to display on the tooltip
                        return dataLabel;
                    }
                }
            }
        });
    }
}

export function updateLineChart(element, chartId, arrayLabels, arrayValue, chartTitle, sub) {
    //const ctx = document.getElementById(chartId);

    if (chartId == "itemCaseValueStats")
    {
        ItemValuebyRiskTypeChart.destroy();
    }

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };

    if (chartId == "itemCaseValueStats") {
        ItemValuebyRiskTypeChart = new Chart(element, {
            type: 'line',
            data: {
                labels: arrayLabels,
                datasets: [{
                    label: sub,
                    data: arrayValue,
                    fill: false,
                    borderColor: dynamicColors(),
                    tension: 0.1
                }]
            },
            options: {
                title: {
                    display: true,
                    text: chartTitle,
                    fontSize: 12
                }
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        // get the data label and data value to display
                        // convert the data value to local string so it uses a comma seperated number
                        var dataLabel = data.labels[tooltipItem.index];
                        // var dataLabel = 'Loss Estimation ';
                        var value = ': ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();

                        // make this isn't a multi-line label (e.g. [["label 1 - line 1, "line 2, ], [etc...]])
                        if (Chart.helpers.isArray(dataLabel)) {
                            // show value on first line of multiline label
                            // need to clone because we are changing the value
                            dataLabel = dataLabel.slice();
                            dataLabel[0] += value;
                        } else {
                            dataLabel += value;
                        }

                        // return the text to display on the tooltip
                        return dataLabel;
                    }
                }
            }
        });
    }
}

