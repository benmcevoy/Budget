(() => {

    google.charts.load('current', { packages: ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    var query = function () {
        let result = "";

        document
            .querySelectorAll("single-value-facet, multiple-value-facet")
            .forEach(x => result += `&${x.name}=${x.value}`);

        return result;
    };

    function drawChart() {
        fetch(`/transactions?${query()}`).then(r => r.json())
            .then((json) => {
                var columns = json.rows[0].concat([{ role: 'annotation' }]);
                var rows = json.rows.slice(1);

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
    }
})();

