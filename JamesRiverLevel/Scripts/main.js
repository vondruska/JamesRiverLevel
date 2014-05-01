var JamesRiverLevel = {
    init: function (chartUrl) {
        document.getElementById('display-forecast').style.display = 'block';
        
        document.getElementById('display-forecast').onclick = function (e) {
            e.preventDefault();
            var chartDiv = document.getElementById('chart');

            this.style.display = 'none';
            
            chartDiv.style.display = 'block';
            
            chartDiv.innerText = chartDiv.textContent = "loading...";
            
            
            var d = document.getElementById("wrapper");
            d.className = d.className + " chart";

            JamesRiverLevel.loadScript("//code.highcharts.com/adapters/standalone-framework.js");
            JamesRiverLevel.loadScript("//code.highcharts.com/highcharts.js");
            JamesRiverLevel.loadScript(chartUrl);
        };
    },
    loadScript: function(url) {
        var s = document.createElement("script");
        s.type = "text/javascript";
        s.src = url;
        document.body.appendChild(s);
    }
}

