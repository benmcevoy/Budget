(() => {
    var query = function () {
        let result = "";

        document
            .querySelectorAll("single-value-facet, multiple-value-facet")
            .forEach(x => result += `&${x.name}=${x.value}`);

        return result;
    };

    var drawChart = function() {
        fetch(`/transactions?${query()}`)
            .then(r => r.json())
            .then((json) => {
                var columns = json.cols;
                var rows = json.rows;

                var data = google.visualization.arrayToDataTable([
                    columns,
                    ...rows
                ]);

                var options = {
                    legend: { position: 'top', maxLines: 3 },
                    bar: { groupWidth: '75%' },
                    height: 600,
                    isStacked: true
                };

                new google.visualization
                    .BarChart(document.getElementById('chart'))
                    .draw(data, options);
            });
    };

    // attach to form and hijack submit
    document.querySelector("#main").addEventListener("submit", (e) => {
        e.preventDefault();

        google.charts.load('current', { packages: ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

    });

    
})();

