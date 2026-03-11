window.inventoryCharts = {
  _charts: {},

  renderBar: function(canvasId, labels, data) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (this._charts[canvasId]) this._charts[canvasId].destroy();

    this._charts[canvasId] = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Movimientos',
                data: data
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
  },

  renderBar2: function(canvasId, labels, data1, data2, label1, label2) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        if (this._charts[canvasId]) this._charts[canvasId].destroy();

        this._charts[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    { label: label1, data: data1 },
                    { label: label2, data: data2 }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    },
  renderDoughnut: function(canvasId, labels, data) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (this._charts[canvasId]) this._charts[canvasId].destroy();

    this._charts[canvasId] = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: 'Salidas',
                data: data
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
  }



};