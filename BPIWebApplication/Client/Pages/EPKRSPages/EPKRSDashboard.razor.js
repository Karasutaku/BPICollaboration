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

    new Chart(ctx, {
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
        }
    });
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

    new Chart(ctx, {
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
            }
        }
    });
}

export function initializeLineChart(chartId, arrayLabels, arrayValue, chartTitle, sub) {
    const ctx = document.getElementById(chartId);

    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };

    new Chart(ctx, {
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
        }
    });
}