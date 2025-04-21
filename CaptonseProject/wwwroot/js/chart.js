window.drawChart = () => {
  const ctx = document.getElementById('patientChart').getContext('2d');
  new Chart(ctx, {
      type: 'line',
      data: {
          labels: ['14-12', '15-12', '16-12', '17-12', '18-12', '19-12', '20-12'],
          datasets: [
              {
                  label: 'Patient Visits',
                  data: [412, 254, 314, 412, 254, 314, 412],
                  borderColor: 'rgba(75, 192, 192, 1)',
                  fill: false
              }
          ]
      },
      options: {
          scales: {
              y: {
                  beginAtZero: true
              }
          }
      }
  });
};