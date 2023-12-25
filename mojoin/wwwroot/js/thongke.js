Highcharts.chart('container', {
    data: {
        table: 'datatable'
    },
    chart: {
        type: 'column'
    },
    title: {
        text: 'Thống kê số tiền đã nạp'
    },
    subtitle: {
        text:
            'Source: <a href="https://www.ssb.no/en/statbank/table/04231" target="_blank">SSB</a>'
    },
    xAxis: {
        type: 'category'
    },
    yAxis: {
        allowDecimals: false,
        title: {
            text: 'Amount'
        }
    }
});
