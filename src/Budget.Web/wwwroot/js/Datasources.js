const datasources = (() => {
    const _request = (url) => {
        var data = "";
        let request = new XMLHttpRequest();

        request.responseType = 'application/json';
        request.onload = () => data = JSON.parse(request.response);
        request.open("GET", url, false);
        request.send();

        return data;
    };

    const tags = () => _request("/datasources/tags");
    const dateRange = () => _request("/datasources/dateRange");
    const dateResolution = () => _request("/datasources/dateResolution");

    return {
        tags: tags,
        dateRange: dateRange,
        dateResolution: dateResolution
    };
})();